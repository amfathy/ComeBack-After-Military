# LINQ Session 8: Grouping (`GroupBy` and `ToLookup`)

Grouping splits a sequence into **buckets**, one bucket per distinct **key**. It is the LINQ version of SQL's `GROUP BY`.

| Operator | What it does | Execution |
|---|---|---|
| `GroupBy(keySelector)` | Groups items by a key | **Deferred** |
| `ToLookup(keySelector)` | Groups items by a key and stores the result | **Immediate** |

Both return groups. Each group has:
- a **`Key`** (for example `"IT"`)
- the **items** that share that key (a group is itself an `IEnumerable<T>`)

```
Employees ──GroupBy(Department)──►  "Finance" → [Cochran, Jaclyn, ...]
                                    "IT"      → [Cochran, Wallace, ...]
                                    "HR"      → [Oconnor, Elva, ...]
```

---

## 1. `GroupBy`

### Method syntax

```csharp
var result = employees.GroupBy(x => x.Department);

foreach (var group in result)
{
    Console.WriteLine($"Department: {group.Key}");
    foreach (var emp in group)
        Console.WriteLine($"   {emp.Name}");
}
```

The return type is `IEnumerable<IGrouping<string, Employee>>`:

| Part | Meaning |
|---|---|
| `IGrouping<TKey, TElement>` | One group |
| `group.Key` | The shared key value |
| `foreach (var e in group)` | The items in that group |

> Because a group is an `IEnumerable<Employee>`, you can call any LINQ operator on it: `group.Count()`, `group.Average(...)`, `group.OrderBy(...)`.

### Query syntax

```csharp
var result = from emp in employees
             group emp by emp.Department;
```

A query that ends with `group ... by` has **no** `select`.

### Groups appear in order of first occurrence

Groups are returned in the order their keys **first appear** in the source, and items inside each group keep their original order.

---

## 2. Working with each group

### Aggregates per group

```csharp
var stats = employees
    .GroupBy(e => e.Department)
    .Select(g => new
    {
        Department = g.Key,
        Count      = g.Count(),
        AvgSalary  = g.Average(e => e.Salary),
        MaxSalary  = g.Max(e => e.Salary)
    });
```

This is the most common use of `GroupBy`: **group, then summarize**.

### Query syntax with `into`

`into g` lets you continue the query after grouping:

```csharp
var stats = from e in employees
            group e by e.Department into g
            orderby g.Count() descending
            select new { Department = g.Key, Count = g.Count() };
```

### Overloads you will use

```csharp
// 1) Keep only part of each item (element selector)
employees.GroupBy(e => e.Department, e => e.Name);
// "IT" → ["Wallace Buck", "Acevedo Wall", ...]  (strings, not Employee objects)

// 2) Shape each group directly (result selector)
employees.GroupBy(e => e.Department,
                  (key, group) => new { Department = key, Count = group.Count() });

// 3) Custom key comparison, e.g. ignore case
names.GroupBy(n => n, StringComparer.OrdinalIgnoreCase);
```

### Grouping by more than one key

Use an **anonymous type** as the key:

```csharp
var q = employees.GroupBy(e => new { e.Department, IsHigh = e.Salary >= 3000 });

foreach (var g in q)
    Console.WriteLine($"{g.Key.Department} / High: {g.Key.IsHigh} → {g.Count()}");
```

Anonymous types compare by value, so employees with the same department and same band land in the same group.

### Keys can be calculated

The key does not have to be a property:

```csharp
fruits.GroupBy(f => f.Length);              // group by length
numbers.GroupBy(n => n % 2 == 0 ? "Even" : "Odd");
employees.GroupBy(e => e.Name[0]);          // group by first letter
```

---

## 3. `ToLookup`

`ToLookup` groups just like `GroupBy`, but it runs **immediately** and stores the result in a `ILookup<TKey, TElement>`.

```csharp
var lookup = employees.ToLookup(x => x.Department);

foreach (var group in lookup)
    group.Print($"Employees in '{group.Key}' Department");
```

### What you get that `GroupBy` does not give you

```csharp
lookup["IT"]            // all IT employees (fast key access, like a dictionary)
lookup["Marketing"]     // key does not exist → EMPTY sequence (no exception!)
lookup.Contains("HR")   // true / false
lookup.Count            // number of groups
```

### `GroupBy` vs `ToLookup`

| | `GroupBy` | `ToLookup` |
|---|---|---|
| Execution | Deferred: runs when enumerated, and **each time** you enumerate | Immediate: runs once, result is stored |
| Result type | `IEnumerable<IGrouping<K,T>>` | `ILookup<K,T>` |
| Look up by key | No (you must loop or filter) | Yes: `lookup[key]` |
| Missing key | n/a | Returns an empty sequence |
| Sees later changes to the source | Yes (if enumerated again) | No (it is a snapshot) |
| Can be changed after creation | n/a | No, it is read-only |
| Query syntax | `group ... by` | None (method only) |

### `ToLookup` vs `ToDictionary`

```csharp
employees.ToDictionary(e => e.Department);   // throws: duplicate key!
employees.ToLookup(e => e.Department);       // fine: many items per key
```

A dictionary holds **one** value per key. A lookup holds **many** values per key.

---

## When to use which

- Need to **summarize** each group (count, sum, average) → `GroupBy` + `Select`.
- Need to **look up** groups by key many times → `ToLookup`.
- Need a **snapshot** that will not change if the source changes → `ToLookup`.
- Need **deferred** execution or to build a query for a database (EF Core) → `GroupBy`.

## Cheat sheet

| I want to... | Use |
|---|---|
| Group by a property | `GroupBy(x => x.Prop)` |
| Count per group | `.Select(g => new { g.Key, Count = g.Count() })` |
| Group by two things | `GroupBy(x => new { x.A, x.B })` |
| Keep only some data in each group | `GroupBy(key, element)` |
| Group ignoring case | `GroupBy(x => x, StringComparer.OrdinalIgnoreCase)` |
| Fast key access | `ToLookup(...)` then `lookup[key]` |
| Query syntax | `group x by x.Key` and `into g` to continue |

## Run the examples

```
RunGroupByExample()        → GroupBy, method syntax
RunLookupExample()         → ToLookup
RunGroupByWithQuerySyntax() → group ... by, query syntax
```

## Things to notice in the sample code

- `item.Print(...)` works on a group because `IGrouping<string, Employee>` **is** an `IEnumerable<Employee>`.
- The outputs of the three examples look the same. The difference is *when* the grouping runs and what you can do with the result afterwards.
- The banner titles are missing a closing parenthesis (`"GroupBy (Method Syntax"`). It is only cosmetic.
- Neither example shows counts or aggregates, which is where `GroupBy` becomes really useful. The tasks cover that.
