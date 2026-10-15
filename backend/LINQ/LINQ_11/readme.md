# LINQ Session 12: Equality (`SequenceEqual`)

`SequenceEqual` answers one question: **are these two sequences the same?** It returns a `bool`, so it runs immediately.

```csharp
bool equal = quiz1.SequenceEqual(quiz2);
```

It returns `true` only when **all three** are true:

1. Both sequences have the **same number of items**.
2. The items are in the **same order**.
3. Each pair of items at the same position is **equal**.

| `first` | `second` | Result |
|---|---|---|
| `1, 2, 3` | `1, 2, 3` | `true` |
| `1, 2, 3` | `3, 2, 1` | `false` (order matters) |
| `1, 2, 3` | `1, 2, 3, 4` | `false` (different length) |
| *(empty)* | *(empty)* | `true` |

> The two sequences can be different collection types: an array and a `List<int>` with the same items are equal.

---

## 1. How "equal" is decided

For each pair of items, `SequenceEqual` uses `EqualityComparer<T>.Default`, which calls the item's `Equals` method.

| Item type | What "equal" means by default |
|---|---|
| `int`, `double`, `bool`, `DateTime`... | Same **value** |
| `string` | Same **text** (case-sensitive) |
| Your own `class` | Same **object in memory** (**reference equality**), unless you override `Equals` |
| `record` | Same **values** of its properties (but see section 5) |

So with simple types it just works:

```csharp
new[] { 1, 2, 3 }.SequenceEqual(new List<int> { 1, 2, 3 });     // true
new[] { "a", "b" }.SequenceEqual(new[] { "A", "B" });           // false (case matters)
```

---

## 2. The three sample methods explained

```csharp
// Method 1: two lists holding the SAME three objects
var quiz1 = new List<Question>(new[] { q1, q2, q3 });
var quiz2 = new List<Question>(new[] { q1, q2, q3 });
quiz1.SequenceEqual(quiz2);        // true: same object references in the same order
```

```csharp
// Method 2: the SAME list variable used twice
var quiz1 = ramdomFourQuestion;
var quiz2 = ramdomFourQuestion;
quiz1.SequenceEqual(quiz2);        // true: it is literally the same list
```

```csharp
// Method 3: two SEPARATE calls, each one builds new Question objects
var quiz1 = QuestionBank.GetQuestionRange(Enumerable.Range(1, 4));
var quiz2 = QuestionBank.GetQuestionRange(Enumerable.Range(1, 4));
quiz1.SequenceEqual(quiz2);        // depends on how Question defines equality
```

Method 3 is the interesting one. The two quizzes contain the **same data** but **different objects** (because `All` is declared with `=>` and rebuilds the whole bank on every read).

| `Question` class | Result of Method 3 |
|---|---|
| No `Equals` override | `false` (different objects) |
| `Question.Equals` overridden, but `Choice` is **not** | **still `false`** (see below) |
| `Question.Equals` **and** `Choice.Equals` overridden | `true` |

> If `All` were a `static readonly` list (built once), Method 3 would return `true` even without any override, because both quizzes would contain the same objects.

---

## 3. Overriding `Equals` correctly

To make equal-looking objects equal, override `Equals` **and** `GetHashCode`.

### The trap in the edited sample

```csharp
public override bool Equals(object obj)
{
    ...
    return Title.Equals(other.Title)
        && CorrectAnswer == other.CorrectAnswer
        && Choices.SequenceEqual(other.Choices);     // compares each Choice with Choice.Equals
}
```

`Choices.SequenceEqual(...)` compares `Choice` objects. If `Choice` does **not** override `Equals`, two `Choice`s with the same text are **different objects**, so the questions are still not equal. **Every class inside the comparison needs correct equality too.**

### The second trap: `GetHashCode`

```csharp
hash = hash * 23 + Choices.GetHashCode();     // hash of the LIST OBJECT, not of its contents
```

`List<T>` does not override `GetHashCode`, so two lists with identical contents have **different** hash codes. Two questions that are `Equals` would then have **different** hash codes. That breaks a rule of .NET:

> **If `a.Equals(b)` is `true`, then `a.GetHashCode()` must equal `b.GetHashCode()`.**

Operators and collections that use the hash code (`Distinct`, `Union`, `Except`, `Intersect`, `GroupBy`, `ToLookup`, `ToDictionary`, `HashSet`, `Dictionary` keys) will silently treat equal questions as different.

### A corrected version

```csharp
public class Choice
{
    public int Order { get; set; }
    public string Description { get; set; }

    public override bool Equals(object obj) =>
        obj is Choice other && Order == other.Order && Description == other.Description;

    public override int GetHashCode() => HashCode.Combine(Order, Description);
}

public class Question
{
    public string Title { get; set; }
    public List<Choice> Choices { get; set; } = new();
    public int CorrectAnswer { get; set; }

    public override bool Equals(object obj) =>
        obj is Question other
        && Title == other.Title
        && CorrectAnswer == other.CorrectAnswer
        && Choices.SequenceEqual(other.Choices);       // uses Choice.Equals now

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Title);
        hash.Add(CorrectAnswer);
        foreach (var choice in Choices)                // hash the CONTENTS, not the list object
            hash.Add(choice);
        return hash.ToHashCode();
    }
}
```

Notes:
- `Title == other.Title` and `HashCode.Add(Title)` are safe when `Title` is `null`. `Title.Equals(...)` and `Title.GetHashCode()` would throw (the sample has the comment `//check null`).
- `HashCode` needs .NET Core 2.1 or later (it works on .NET 5).
- Do not use `Equals`/hash code on **mutable** properties of objects stored in a `HashSet` or used as dictionary keys. If the values change later, the object is "lost".

---

## 4. A custom comparer instead of changing the class

You do not have to change `Question`. Pass an `IEqualityComparer<T>` to `SequenceEqual`:

```csharp
class QuestionTitleComparer : IEqualityComparer<Question>
{
    public bool Equals(Question x, Question y) =>
        string.Equals(x?.Title, y?.Title, StringComparison.OrdinalIgnoreCase);

    public int GetHashCode(Question obj) =>
        obj.Title?.ToLowerInvariant().GetHashCode() ?? 0;
}

quiz1.SequenceEqual(quiz2, new QuestionTitleComparer());     // compares by title only
```

Built-in comparers are handy for strings:

```csharp
names1.SequenceEqual(names2, StringComparer.OrdinalIgnoreCase);
```

### Which operators need what?

| Operator | Uses `Equals` | Uses `GetHashCode` |
|---|---|---|
| `SequenceEqual` | Yes | No |
| `Contains` | Yes | No |
| `Distinct`, `Union`, `Intersect`, `Except` | Yes | **Yes** |
| `GroupBy`, `ToLookup`, `ToDictionary`, `HashSet` | Yes | **Yes** |

A comparer should still implement both methods correctly.

---

## 5. Things `SequenceEqual` does **not** do

### It does not ignore order

```csharp
new[] { 1, 2, 3 }.SequenceEqual(new[] { 3, 2, 1 });     // false
```

To ignore order:

```csharp
a.OrderBy(x => x).SequenceEqual(b.OrderBy(x => x));          // same items, same counts
new HashSet<int>(a).SetEquals(b);                            // same SET of items (duplicates ignored)
!a.Except(b).Any() && !b.Except(a).Any();                    // same set, duplicates ignored
```

### It does not look inside collections

```csharp
record Person(string Name, List<string> Hobbies);

var p1 = new Person("Sara", new List<string> { "Chess" });
var p2 = new Person("Sara", new List<string> { "Chess" });
p1.Equals(p2);      // false: the two Hobbies lists are different objects
```

Records compare their properties with `Equals`, and `List<T>` uses reference equality. A record with only simple properties (`record Person(string Name, int Age)`) compares by value, and two lists of such records are `SequenceEqual`.

### It cannot handle a `null` sequence

```csharp
list.SequenceEqual(null);     // ArgumentNullException
```

### It enumerates both sequences

`SequenceEqual` walks through both sequences until it finds a difference (or reaches the end). If either side is a lazy LINQ query, that query is executed again each time you call `SequenceEqual`.

---

## Cheat sheet

| I want to know... | Use |
|---|---|
| Same items, same order | `a.SequenceEqual(b)` |
| Same text, ignoring case | `a.SequenceEqual(b, StringComparer.OrdinalIgnoreCase)` |
| Same by my own rule | `a.SequenceEqual(b, myComparer)` |
| Same items, any order | `a.OrderBy(x => x).SequenceEqual(b.OrderBy(x => x))` |
| Same set (no duplicates) | `new HashSet<T>(a).SetEquals(b)` |
| Is a word a palindrome? | `word.SequenceEqual(word.Reverse())` |
| Make my class compare by value | Override `Equals` **and** `GetHashCode` (or use a `record`) |

## Run the examples

```
RunMethod1()   → same objects in two lists
RunMethod2()   → same list variable twice
RunMethod3()   → two separate fetches (needs value equality to be true)
```

## Things to notice in the sample code

- **`Main` runs only `RunMethod3`.** Uncomment the other two to compare the three results.
- **`Choice` has no `Equals` override**, so `RunMethod3` still prints "are not equal", even after `Question` was edited. Add the override to `Choice` too.
- **`GetHashCode` uses `Choices.GetHashCode()`**, which is the hash of the list object and not of its contents. Equal questions get different hash codes. Loop over the choices instead.
- **`Title.Equals(...)` / `Title.GetHashCode()` throw if `Title` is `null`.** Use `string.Equals` and `HashCode.Add`.
- **`Enumerable.Range(1, 4)` gives indexes 1 to 4**, which are Q #2 to Q #5, not the first four questions (see Session 10).
- **`ramdomFourQuestion` is not random.** It is a fixed range. The name is misleading, and it is also a typo of "random".
- **In `RunMethod1`, `PickOne()` can return the same question more than once.** The result is still `true`.
