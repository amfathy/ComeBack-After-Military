# LINQ Session 3: `Select`, `SelectMany` and `Zip`

This session is about **projection and combining**: changing the *shape* of data, instead of filtering it (which was `Where`).

| Operator | One-line idea | Output size |
|---|---|---|
| `Select` | Transform **each** item into something else | Same count as input |
| `SelectMany` | Transform each item into a **collection**, then flatten into one list | Usually more than input |
| `Zip` | Pair items from **two** sequences by position | Length of the shorter sequence |

> All three use **deferred execution**: nothing runs until you loop with `foreach` or call `ToList()`, `ToArray()`, `Count()`, etc.

---

## 1. `Select` (Projection)

Takes each element and returns a new value. It is a 1 → 1 mapping.

```csharp
public static IEnumerable<TResult> Select<TSource, TResult>(
    this IEnumerable<TSource> source, Func<TSource, TResult> selector)
```

### Example A: change the value

```csharp
List<string> words = new() { "i", "love", "asp.net", "core" };

var upper = words.Select(x => x.ToUpper());
// I, LOVE, ASP.NET, CORE
```

### Example B: change the type

```csharp
List<int> numbers = new() { 2, 3, 5, 7 };

var squares = numbers.Select(x => x * x);
// 4, 9, 25, 49
```

### Example C: project into a new object (DTO)

```csharp
var result = employees.Select(x => new EmployeeDto
{
    Name = $"{x.FirstName} {x.LastName}",
    TotalSkills = x.Skills.Count()
});
```

Use this when you only need **some** properties, or a **different shape** than the original class.

### Query syntax

```csharp
var upper = from word in words
            select word.ToUpper();
```

### Key points
- Output count = input count.
- The result type is whatever the lambda returns (`string`, `int`, `EmployeeDto`, anonymous type...).
- `Select` does **not** filter. To filter, combine it with `Where`.

---

## 2. `SelectMany` (Flattening)

Use it when each element contains **another collection** and you want one flat list.

```
Select      →  [ [a, b], [c], [d, e] ]      (a list of lists)
SelectMany  →  [ a, b, c, d, e ]            (one flat list)
```

### Example A: sentences to words

```csharp
string[] sentences =
{
    "I love asp.net core",
    "I like sql server also",
    "in general i love programming"
};

var words = sentences.SelectMany(x => x.Split(' '));
```

`Split(' ')` returns a `string[]` for each sentence. `SelectMany` merges all the arrays into one sequence of words.

### Example B: all skills of all employees (no duplicates)

```csharp
var skills = employees.SelectMany(x => x.Skills).Distinct();
```

### Query syntax: two `from` clauses

```csharp
var skills = (from employee in employees
              from skill in employee.Skills
              select skill).Distinct();
```

> A second `from` in query syntax is translated by the compiler into `SelectMany`.

### Key points
- The lambda must return an `IEnumerable<T>`.
- Output count is the **sum** of all inner collection sizes.
- Add `.Distinct()` when you want unique values.

### `Select` vs `SelectMany`

```csharp
employees.Select(e => e.Skills);      // IEnumerable<List<string>>  (nested)
employees.SelectMany(e => e.Skills);  // IEnumerable<string>        (flat)
```

---

## 3. `Zip` (Pairing two sequences)

Walks through **two sequences at the same time** and combines item *i* from the first with item *i* from the second.

### Example A: names with hex codes

```csharp
string[] colorName = { "Red", "Green", "Blue" };
string[] colorHEX  = { "FF0000", "00FF00", "0000FF", "extra" };

var colors = colorName.Zip(colorHEX, (name, hex) => $"{name} ({hex})");
// Red (FF0000), Green (00FF00), Blue (0000FF)
```

`"extra"` is ignored because `Zip` **stops at the shorter sequence**.

### Example B: pair first three with last three employees

```csharp
var employees = Repository.LoadEmployees().ToArray();
var firstThree = employees[..3];     // range: first 3 items
var lastThree  = employees[^3..];    // index from end: last 3 items

var teams = firstThree.Zip(lastThree, (first, last) =>
    $"{first.FullName} with {last.FullName}");
```

### Zip without a selector (tuples)

```csharp
var pairs = firstThree.Zip(lastThree);   // IEnumerable<(Employee First, Employee Second)>

var teams = from team in firstThree.Zip(lastThree)
            select $"{team.First.FullName} with {team.Second.FullName}";
```

### Key points
- Pairing is by **position**, not by key (it is not a join).
- Result length = `Min(first.Count, second.Count)`.
- There is no dedicated query-syntax keyword for `Zip`; call it as a method, then use `from ... select` on the result.

---

## Handy C# features used in this session

| Feature | Meaning |
|---|---|
| `new()` | Target-typed `new`: `List<int> n = new() { 1, 2 };` |
| `arr[..3]` | First 3 elements |
| `arr[^3..]` | Last 3 elements |
| `$"{a} {b}"` | String interpolation |
| `x => new Dto { ... }` | Lambda that projects into an object |

---

## Cheat sheet: which one do I use?

- Convert each item to something else → **`Select`**
- Each item has a list inside it and I want one flat list → **`SelectMany`**
- I have two lists and want to combine them side by side → **`Zip`**

## Run the examples

Each example is a small console project (`net5.0`) referencing `LINQTut04.Shared`:

```
LINQTut04.SELECT       → RunExample01, 02, 03
LINQTut04.SelectMany   → RunExample01, 02
LINQTut04.Zip          → RunExample01, 02
```

```bash
dotnet run --project LINQTut04.SELECT
```
