# LINQ Session 10: Generation Operators (`Range`, `Repeat`, `Empty`, `DefaultIfEmpty`)

So far every LINQ operator started from an **existing** collection. These operators **create** sequences, or **fix** empty ones.

| Operator | What it does | Example result |
|---|---|---|
| `Enumerable.Range(start, count)` | A sequence of consecutive integers | `Range(1, 5)` → `1, 2, 3, 4, 5` |
| `Enumerable.Repeat(item, count)` | The same item repeated `count` times | `Repeat("a", 3)` → `a, a, a` |
| `Enumerable.Empty<T>()` | An empty sequence of type `T` | `(nothing)` |
| `source.DefaultIfEmpty()` | If `source` is empty, returns **one** default value; otherwise returns `source` unchanged | `[]` → `[default]` |
| `source.DefaultIfEmpty(value)` | Same, but with **your** value | `[]` → `[value]` |

> **Important:** `Range`, `Repeat` and `Empty` are **static methods on the `Enumerable` class**, not extension methods. You write `Enumerable.Range(1, 10)`, not `list.Range(1, 10)`.
> `DefaultIfEmpty` **is** an extension method (called on a sequence).

---

## 1. `Enumerable.Range`

```csharp
var range = Enumerable.Range(1, 10);   // 1, 2, 3, 4, 5, 6, 7, 8, 9, 10
```

### The second parameter is a COUNT, not an end value

```csharp
Enumerable.Range(5, 3);    // 5, 6, 7        (start at 5, give me 3 numbers)
Enumerable.Range(0, 5);    // 0, 1, 2, 3, 4
Enumerable.Range(10, 1);   // 10
Enumerable.Range(1, 0);    // empty
```

To count from `a` to `b` (inclusive): `Enumerable.Range(a, b - a + 1)`.

### What it replaces

```csharp
// The old way: manual loop and array
int[] range2 = new int[10];
for (int i = 0; i < range2.Length; i++)
    range2[i] = i;

// With LINQ: one line, no array
var range = Enumerable.Range(0, 10);
```

### Typical uses

```csharp
Enumerable.Range(1, 5).Select(x => x * x);                          // 1, 4, 9, 16, 25
Enumerable.Range(1, 10).Where(x => x % 2 == 0);                     // 2, 4, 6, 8, 10
Enumerable.Range(0, 26).Select(i => (char)('A' + i));               // A, B, C ... Z
Enumerable.Range(1, 5).Select(i => new Question { Title = $"Test #{i}" });   // generate test data
Enumerable.Range(1, 100).Sum();                                     // 5050
```

### Key points
- It is **lazy** (deferred): `Range(1, 1_000_000_000)` uses almost no memory until you loop over it.
- A **negative** count (or a range that goes past `int.MaxValue`) throws `ArgumentOutOfRangeException`.
- It only produces `int`. For other types, use `Select` on top of it.

### Using a Range as indexes (the question bank example)

```csharp
public static List<Question> GetQuestionRange(IEnumerable<int> range)
{
    return All.Where((x, i) => range.Contains(i)).ToList();
}

var questions = QuestionBank.GetQuestionRange(Enumerable.Range(1, 10));
```

`Where((x, i) => ...)` gives the **zero-based index** `i`. So `Range(1, 10)` selects indexes `1` to `10`, which are **Q #2 to Q #11**, not Q #1 to Q #10. To get the first ten questions, use `Range(0, 10)`.

---

## 2. `Enumerable.Repeat`

```csharp
Enumerable.Repeat("ab", 3);       // "ab", "ab", "ab"
Enumerable.Repeat(0, 5);          // 0, 0, 0, 0, 0

string line = string.Concat(Enumerable.Repeat("=", 20));   // ====================
```

### The reference-type trap

`Repeat` repeats the **same value**. For a class, that means the **same object**, not copies:

```csharp
var q = QuestionBank.PickOne();

var list1 = new List<Question>();
for (int i = 0; i < 10; i++)
    list1.Add(new Question());                  // 10 different objects

var list2 = Enumerable.Repeat(q, 10).ToList();  // 10 references to ONE object

Console.WriteLine(ReferenceEquals(list1[0], list1[1]));  // False
Console.WriteLine(ReferenceEquals(list2[0], list2[1]));  // True
```

If you change `list2[0].Title`, **all 10 items change**, because they are the same object.

**When you need 10 different objects**, use `Range` + `Select`:

```csharp
var list3 = Enumerable.Range(0, 10).Select(_ => new Question()).ToList();   // 10 different objects
```

### Key points
- Fine for **value types** (`int`, `char`) and **strings** (immutable).
- Dangerous for **mutable reference types** (your own classes).
- A negative count throws `ArgumentOutOfRangeException`.

---

## 3. `Enumerable.Empty<T>()`

Gives you an empty sequence of a given type.

```csharp
var a = new List<Question>();               // empty list (a new object every time)
var b = Enumerable.Empty<Question>();       // empty sequence (cached, reused)

foreach (var q in b)                        // loops zero times, no error
    Console.WriteLine(q);
```

### Why use it?

- Return it **instead of `null`**, so callers can safely loop over the result without a null check:

```csharp
public static IEnumerable<Question> Find(string text)
{
    if (string.IsNullOrWhiteSpace(text))
        return Enumerable.Empty<Question>();     // not null!

    return All.Where(q => q.Title.Contains(text));
}
```

- It does **not** allocate a new object every call. `Enumerable.Empty<int>()` returns the same cached instance each time:

```csharp
ReferenceEquals(Enumerable.Empty<int>(), Enumerable.Empty<int>());   // True
ReferenceEquals(new List<int>(), new List<int>());                   // False
```

- Handy as a **starting value**, a default, or a neutral element in `Concat`/`Union`.

---

## 4. `DefaultIfEmpty`

If the sequence has items, it does nothing. If the sequence is **empty**, it gives you **one** default item.

```csharp
var questions = Enumerable.Empty<Question>();

var q1 = questions.DefaultIfEmpty();                  // one element: null (default of a class)
var q2 = questions.DefaultIfEmpty(Question.Default);  // one element: your placeholder question
```

### What is `default`?

| Type | `default(T)` |
|---|---|
| `int`, `double`, `decimal` | `0` |
| `bool` | `false` |
| `DateTime` | `01/01/0001 00:00:00` |
| classes, `string` | `null` |

### Why use it?

1. **Show a placeholder** when a search finds nothing:

```csharp
var results = QuestionBank.All
    .Where(q => q.Title.Contains("blockchain"))
    .DefaultIfEmpty(Question.Default);        // never empty, so the screen is never blank
```

2. **Avoid exceptions on aggregates** of an empty list:

```csharp
new List<int>().Average();                        // throws InvalidOperationException
new List<int>().DefaultIfEmpty(0).Average();      // 0
```

3. **Left joins** (Session 9): `join ... into g from x in g.DefaultIfEmpty()`.

### Key points
- On a **non-empty** sequence it returns the sequence unchanged.
- Without an argument, for classes it returns `null`, so remember to handle `null` (for example `item?.Name`).
- It does not add the default value to a non-empty list. It is only a fallback.

---

## Cheat sheet

| I want to... | Use |
|---|---|
| Numbers 1 to N | `Enumerable.Range(1, N)` |
| Numbers from A to B | `Enumerable.Range(A, B - A + 1)` |
| Generate N test objects | `Enumerable.Range(1, N).Select(i => new T { ... })` |
| Repeat a value N times | `Enumerable.Repeat(value, N)` |
| N different new objects | `Enumerable.Range(0, N).Select(_ => new T())` (not `Repeat`) |
| An empty sequence (not null) | `Enumerable.Empty<T>()` |
| A fallback when empty | `.DefaultIfEmpty(fallback)` |
| Safe average/sum on a possibly empty list | `.DefaultIfEmpty(0).Average()` |

## Run the examples

```
LINQTut10.Range            → Enumerable.Range + GetQuestionRange
LINQTut10.Repeat           → Enumerable.Repeat and ReferenceEquals
LINQTut10.Empty            → empty List vs Enumerable.Empty
LINQTut10.DefaultIfEmpty   → DefaultIfEmpty() vs DefaultIfEmpty(Question.Default)
```

## Things to notice in the sample code

- **Off-by-one in the `Range` example.** `Enumerable.Range(1, 10)` is used as **indexes** into the question list, so it returns Q #2 to Q #11. Use `Range(0, 10)` for the first ten questions.
- **`CorrectAnswer = 1` for every question** in the question bank. It looks like a placeholder, because several answers are not actually the first choice (for example, "A peer in a swarm that has the complete file" is `Seed`, the second choice). Treat it as sample data, not an answer key.
- **`All` is declared with `=>`** (`public static List<Question> All => new List<Question> { ... }`). That creates a **brand-new list of 246 new objects each time** `All` is read. `PickOne` reads it twice (`All[...All.Count...]`). Use `{ get; } = ...` or a `static readonly` field if you want one shared list.
- **The comment `// 0 - 245`** in `PickOne` is right for the 246 items, but `Random.Next(0, All.Count)` excludes the upper bound, which is exactly what you want.
- **`question2` is never used** in the `DefaultIfEmpty` example. It would hold a single `null`, and calling `ToQuiz()` on it would print an empty line.
- **Duplicate title:** the last two questions in the bank are both numbered `Q #245`, and the last question repeats the same text in three choices.
