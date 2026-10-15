# LINQ Session 13: Concatenation (`Concat`)

`Concat` **joins two sequences end to end**: all items of the first, then all items of the second.

```csharp
var quiz3 = quiz1.Concat(quiz2);
```

```
quiz1:  [ A, B, C ]
quiz2:  [ D, E ]
                         quiz1.Concat(quiz2)  →  [ A, B, C, D, E ]
                         quiz2.Concat(quiz1)  →  [ D, E, A, B, C ]
```

| Fact | Detail |
|---|---|
| Order | First sequence first, second sequence after. Order **matters**. |
| Duplicates | **Kept.** Concat does not remove anything. |
| Execution | **Deferred.** Nothing is copied until you loop or call `ToList()`. |
| Original lists | **Never changed.** A new sequence is produced. |
| Count | `first.Count() + second.Count()` |
| `null` input | Throws `ArgumentNullException` (for either side) |
| Type | Both sequences must have the same element type (see section 4) |

---

## 1. Basic use

```csharp
var quiz1 = QuestionBank.Randomize(3);    // 3 random questions
var quiz2 = QuestionBank.Randomize(2);    // 2 random questions

var quiz3 = quiz1.Concat(quiz2);          // 5 questions: first the 3, then the 2
quiz3.ToQuiz();
```

### Concat after Select (join projected values)

You can project each sequence first and then join the results:

```csharp
var questionTitles = quiz1.Select(q => q.Title)
                          .Concat(quiz2.Select(q => q.Title));    // IEnumerable<string>
```

### Chaining more than two

```csharp
var questionTitles =
    QuestionBank.Randomize(3).Select(q => q.Title)
    .Concat(QuestionBank.Randomize(2).Select(q => q.Title))
    .Concat(QuestionBank.GetQuestionRange(Enumerable.Range(11, 14)).Select(q => q.Title));
```

Each `.Concat(...)` adds one more sequence to the end.

---

## 2. `Concat` keeps duplicates

```csharp
var quiz1 = QuestionBank.Randomize(3);
var quiz2 = QuestionBank.Randomize(2);
```

These are **two independent random draws**, so the same question can appear in both. `Concat` will happily list it twice.

| Operator | `{1, 2, 3}` and `{3, 4}` | Duplicates |
|---|---|---|
| `Concat` | `1, 2, 3, 3, 4` | Kept |
| `Union` | `1, 2, 3, 4` | Removed |
| `Concat(...).Distinct()` | `1, 2, 3, 4` | Removed (same as `Union`) |

Use `Concat` when you want **everything**, and `Union` when you want **unique** items.

---

## 3. Another way: `SelectMany` over a list of sequences

```csharp
var quiz3 = new[] { quiz1, quiz2 }.SelectMany(q => q);     // flatten
```

`SelectMany` (Session 3) takes a collection of collections and flattens it into one. For two sequences it gives the same result as `Concat`.

| | `Concat` | `SelectMany` over a list |
|---|---|---|
| Best for | A fixed, small number of sequences (2 or 3) | Many sequences, or a number that is only known at runtime |
| Reads like | "this, then that" | "flatten all of these" |
| Chaining many | One `.Concat(...)` per sequence, so the code grows with the number of sequences | One flat operation, the same code for any number |

```csharp
List<List<int>> groups = GetGroups();            // any number of lists
var all = groups.SelectMany(g => g);             // easy
```

---

## 4. Types must match

`Concat` needs both sequences to have the **same** element type.

```csharp
var ints = new[] { 1, 2, 3 };
var doubles = new[] { 4.5, 5.5 };

// ints.Concat(doubles);                                // error: int vs double
var ok = ints.Select(i => (double)i).Concat(doubles);   // convert first
```

Do not use `Cast<double>()` on `int` values. It throws `InvalidCastException`. Use `Select`.

### Different classes with a common base class

```csharp
IEnumerable<Dog> dogs = ...;
IEnumerable<Cat> cats = ...;

var animals = dogs.Concat<Animal>(cats);     // state the common type explicitly
```

---

## 5. Adding a single item: `Append` and `Prepend`

To add **one** item at the end or at the start, you do not need a one-item array:

```csharp
var withIntro = questions.Prepend(introQuestion);     // item first
var withOutro = questions.Append(outroQuestion);      // item last

// same idea with Concat:
questions.Concat(new[] { outroQuestion });
```

`Append` / `Prepend` also return a **new** sequence and do not change the original.

---

## 6. `Concat` vs `AddRange` vs `Union` vs `+`

| Tool | What it does | Changes the original? |
|---|---|---|
| `a.Concat(b)` | New sequence: a then b (lazy) | No |
| `a.Union(b)` | New sequence: a then b without duplicates | No |
| `list.AddRange(b)` | Adds the items of b **into** the list | **Yes** |
| `string.Concat(...)` or `+` | Joins **text** | n/a |
| `array1 + array2` | Not valid in C# | n/a |

---

## 7. Deferred execution

```csharp
var a = new List<int> { 1, 2 };
var b = new List<int> { 3 };

var c = a.Concat(b);        // nothing is copied yet
a.Add(99);

foreach (var x in c)        // 1, 2, 99, 3  (sees the change!)
    Console.Write(x + " ");

var snapshot = a.Concat(b).ToList();   // a fixed copy of the result at this moment
```

---

## Cheat sheet

| I want to... | Use |
|---|---|
| Join two lists, keep everything | `a.Concat(b)` |
| Join two lists without duplicates | `a.Union(b)` or `a.Concat(b).Distinct()` |
| Join many lists | `lists.SelectMany(l => l)` |
| Add one item at the end / start | `Append(x)` / `Prepend(x)` |
| Join lists of different types | Convert with `Select` first, or `Concat<BaseType>(...)` |
| Change a list by adding another | `list.AddRange(other)` |
| Join only the titles | `a.Select(q => q.Title).Concat(b.Select(q => q.Title))` |

## Run the examples

```
RunMethod01()   → Concat of two quizzes
RunMethod02()   → Select + Concat (titles only)
RunMethod03()   → three sequences chained
RunMethod04()   → SelectMany over an array of quizzes
```

## Things to notice in the sample code

- **`Main` runs only `RunMethod04`.** Uncomment the others to compare them.
- **`Enumerable.Range(11, 14)` is a start and a COUNT** (Session 10). It gives indexes `11` to `24` (14 items), which are Q #12 to Q #25. If the idea was "indexes 11 to 14", use `Enumerable.Range(11, 4)`.
- **The two `Randomize(...)` calls are independent**, so a question can appear in both quizzes and then twice in the `Concat` result. To avoid that, draw the second quiz from the questions that are **not** already in the first one (`All.Except(quiz1)`).
- **`RunMethod02` and `RunMethod03` end up with `IEnumerable<string>`**, not questions, because of the `Select` before `Concat`.
- **`new [] { quiz1, quiz2 }` compiles** because both variables have the same type (`IEnumerable<Question>`).
