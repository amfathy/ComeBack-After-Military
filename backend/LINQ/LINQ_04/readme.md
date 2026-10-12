# LINQ Session 5: Sorting with LINQ

This session covers how to **order** a sequence: `OrderBy`, `OrderByDescending`, `ThenBy`, `ThenByDescending`, `Reverse`, and sorting with a **custom comparer**.

| Operator | What it does |
|---|---|
| `OrderBy` | Sort ascending by a key |
| `OrderByDescending` | Sort descending by a key |
| `ThenBy` / `ThenByDescending` | Tie-breaker: sort by a second (third, ...) key |
| `Reverse` | Flip the current order (no sorting) |
| `OrderBy(x => x, comparer)` | Sort using your own comparison rule (`IComparer<T>`) |

> Like other LINQ operators, sorting uses **deferred execution** and returns a **new** sequence. The original collection is never changed.

---

## 1. `OrderBy` and `OrderByDescending`

You pass a **key selector**: a lambda that returns the value to sort by.

```csharp
string[] fruits = { "apricot", "orange", "banana", "mango", "apple", "grape", "strawberry" };

var asc  = fruits.OrderBy(f => f);                 // A -> Z
var desc = fruits.OrderByDescending(f => f);       // Z -> A
var byLength = fruits.OrderBy(f => f.Length);      // shortest first
```

### Query syntax

```csharp
var asc = from f in fruits
          orderby f ascending        // "ascending" is optional (it is the default)
          select f;

var desc = from f in fruits
           orderby f descending
           select f;

var byLength = from f in fruits
               orderby f.Length descending
               select f;
```

### Sorting objects

The key can be any property:

```csharp
var bySalary = employees.OrderBy(e => e.Salary);
var richestFirst = employees.OrderByDescending(e => e.Salary);
```

### Key points
- The return type is `IOrderedEnumerable<T>`. This special type is what allows `ThenBy` to follow.
- The sort is **stable**: items with equal keys keep their original relative order.
- The key must be comparable (numbers, strings, dates, or a type with a comparer).
- String sorting uses the **default culture comparison**. Pass `StringComparer.OrdinalIgnoreCase` as a second argument when you need case-insensitive or exact ordinal sorting.

---

## 2. `ThenBy` and `ThenByDescending` (multi-level sorting)

Use them when the first key has **ties**.

```csharp
// Sort by Name; if two employees have the same name, sort by Salary ascending
var q1 = emps.OrderBy(x => x.Name).ThenBy(x => x.Salary);

// Same, but higher salary first for ties
var q2 = emps.OrderBy(x => x.Name).ThenByDescending(x => x.Salary);
```

### Query syntax: separate keys with a comma

```csharp
var q = from e in emps
        orderby e.Name, e.Salary descending
        select e;
```

### Common mistake

```csharp
// Wrong: the second OrderBy discards the first sort
emps.OrderBy(x => x.Name).OrderBy(x => x.Salary);

// Right
emps.OrderBy(x => x.Name).ThenBy(x => x.Salary);
```

`ThenBy` only works after `OrderBy` / `OrderByDescending` (or another `ThenBy`), because it needs an `IOrderedEnumerable<T>`.

---

## 3. `Reverse`

Flips the **current order** of a sequence. It does **not** sort.

```csharp
string[] fruits = { "apricot", "orange", "banana", "mango", "apple", "grape", "strawberry" };

var reversed = fruits.Reverse();   // strawberry, grape, apple, mango, banana, orange, apricot
```

### Key points
- LINQ `Reverse()` returns a **new** sequence; you must store or print the result. Calling `fruits.Reverse();` and then printing `fruits` shows the original order.
- `List<T>.Reverse()` is a different, **instance** method: it reverses the list **in place** and returns `void`.
- On newer compilers (C# 14 / .NET 10), `array.Reverse()` can bind to an in-place span method. To be safe, write `fruits.AsEnumerable().Reverse()` when you want the LINQ version.
- `Reverse` has no query-syntax keyword. To get "descending", prefer `OrderByDescending`, which is a real sort.
- `OrderBy(...).Reverse()` gives descending order too, but `OrderByDescending` is clearer and handles ties differently, because it keeps the stable order for equal items.

---

## 4. Custom sorting with `IComparer<T>`

When the default order is not what you need, write a class that implements `IComparer<T>` and pass it to `OrderBy`.

**Scenario:** Employee numbers look like `2017-FI-1343` (`year-department-sequence`). We want to sort by **year first**, then by the **sequence number**, as numbers, not text.

```csharp
internal class EmployeeComparer : IComparer<Employee>
{
    public int Compare(Employee e1, Employee e2)
    {
        // "2017-FI-1343" => "2017", "FI", "1343"
        var e1Year = Convert.ToInt32(e1.EmployeeNo.Split("-")[0]);
        var e2Year = Convert.ToInt32(e2.EmployeeNo.Split("-")[0]);
        var e1Seq  = Convert.ToInt32(e1.EmployeeNo.Split("-")[2]);
        var e2Seq  = Convert.ToInt32(e2.EmployeeNo.Split("-")[2]);

        if (e1Year == e2Year)
            return e1Seq.CompareTo(e2Seq);

        return e1Year.CompareTo(e2Year);
    }
}
```

Use it:

```csharp
var sorted = emps.OrderBy(e => e, new EmployeeComparer());
```

### How `Compare(x, y)` works

| Return value | Meaning |
|---|---|
| Negative (`< 0`) | `x` comes **before** `y` |
| Zero (`0`) | They are **equal** (tie) |
| Positive (`> 0`) | `x` comes **after** `y` |

Here the key selector is `e => e` (the whole object), and the comparer decides everything.

---

## Cheat sheet

| I want to... | Use |
|---|---|
| Sort A to Z / low to high | `OrderBy(x => x.Key)` |
| Sort Z to A / high to low | `OrderByDescending(x => x.Key)` |
| Break ties with another key | `.ThenBy(...)` / `.ThenByDescending(...)` |
| Flip the current order | `Reverse()` |
| Sort by my own rule | `OrderBy(x => x, new MyComparer())` |
| Sort in query syntax | `orderby a, b descending` |

## Run the examples

```
LINQTut05.OrderBy            → ascending / descending, by value and by length
LINQTut05.ThenBy             → OrderBy + ThenBy / ThenByDescending
LINQTut05.Reverse            → Reverse
LINQTut05.OrderBy.Comparer   → custom IComparer<Employee>
```

```bash
dotnet run --project LINQTut05.OrderBy
```

## Small things to watch in the sample code

- In `LINQTut05.Reverse`, the code stores `reveredOrder` but prints `fruits`. The output shows the **original** order and the title says "reverse". Print `reveredOrder` to see the reversal.
- In `LINQTut05.OrderBy`, the variable `orderedFruitsAscLengthQ` is titled "DESC Length" because the query uses `descending`, while its method-syntax partner is ascending. The two outputs differ on purpose, so make sure students notice that.
