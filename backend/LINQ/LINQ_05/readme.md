# LINQ Session 6: Partitioning (`Skip`, `Take`, `Chunk`) and Pagination

This session is about taking **a part** of a sequence: the first N items, everything after N, fixed-size groups, or one "page" of results.

| Operator | What it does |
|---|---|
| `Take(n)` | First `n` items |
| `TakeLast(n)` | Last `n` items |
| `TakeWhile(rule)` | Items from the start **until** the rule first fails |
| `Skip(n)` | Everything **except** the first `n` items |
| `SkipLast(n)` | Everything **except** the last `n` items |
| `SkipWhile(rule)` | Drops items from the start **while** the rule is true, then returns the **rest** |
| `Chunk(size)` | Splits the sequence into arrays of `size` items |
| `Skip` + `Take` | **Pagination** |

> All of these use **deferred execution** and return a new sequence. The original collection is not modified.
> `Chunk` needs **.NET 6 or later**. `SkipLast` and `TakeLast` need .NET Core 2.0 or later.

---

## 1. `Take`, `TakeLast`, `TakeWhile`

```csharp
var q1 = emps.Take(10);       // first 10 employees
var q2 = emps.TakeLast(10);   // last 10 employees

var q3 = emps.TakeWhile(x => x.Salary != 214400);
// takes employees from the beginning, and STOPS at the first employee with salary 214,400
```

### Key points
- `Take(n)` when `n` is larger than the count simply returns **everything**. It does not throw.
- `Take(0)` or a negative number returns an empty sequence.
- `TakeWhile` stops at the **first** item that fails the rule. It never looks further, even if later items would pass.
- If the rule never fails, `TakeWhile` returns everything.

---

## 2. `Skip`, `SkipLast`, `SkipWhile`

```csharp
var q1 = emps.Skip(10);       // everything after the first 10
var q2 = emps.SkipLast(10);   // everything except the last 10

var q3 = emps.SkipWhile(x => x.Salary != 214400);
// skips employees until it reaches salary 214,400, then returns that employee and ALL the rest
```

### Key points
- `Skip(n)` when `n` is larger than the count returns an **empty** sequence. It does not throw.
- `Skip` of zero or a negative number returns everything.
- `SkipWhile` only skips at the **start**. Once the rule fails for the first time, every remaining item is returned, even those that would match the rule again.

### `Take`/`Skip` mirror each other

```csharp
emps.Take(10)   +   emps.Skip(10)   =   all employees
emps.TakeWhile(rule)  +  emps.SkipWhile(rule)  =  all employees
```

### `TakeWhile` is not `Where`

```csharp
numbers = { 2, 4, 6, 7, 8, 10 }

numbers.Where(x => x % 2 == 0);        // 2, 4, 6, 8, 10   (checks every item)
numbers.TakeWhile(x => x % 2 == 0);    // 2, 4, 6          (stops at 7)
numbers.SkipWhile(x => x % 2 == 0);    // 7, 8, 10         (starts at 7)
```

---

## 3. Ranges as a shortcut (.NET 6+)

`Take` also accepts a `Range`:

```csharp
emps.Take(2..5);     // items at index 2, 3, 4
emps.Take(..3);      // first 3
emps.Take(^3..);     // last 3
```

---

## 4. `Chunk`

Splits a sequence into **arrays of a fixed size**. The last chunk can be smaller.

```csharp
var chunks = emps.Chunk(10).ToList();   // IEnumerable<Employee[]>

for (int i = 0; i < chunks.Count; i++)
    chunks[i].Print($"Chunk #{i + 1}");
```

With 25 employees and `Chunk(10)` you get 3 chunks with sizes **10, 10, 5**.

### Key points
- The return type is `IEnumerable<T[]>` (each chunk is an **array**).
- Use it for batching: sending 100 emails at a time, inserting rows in groups, showing items in a grid with N per row.

---

## 5. Pagination with `Skip` + `Take`

A "page" is a window into an **ordered** list:

```
Skip((page - 1) * size)  →  Take(size)
```

| page | size | Skips | Takes | Items shown |
|---|---|---|---|---|
| 1 | 10 | 0 | 10 | 1 - 10 |
| 2 | 10 | 10 | 10 | 11 - 20 |
| 3 | 10 | 20 | 10 | 21 - 30 |

### The reusable extension method

```csharp
public static IEnumerable<T> Paginate<T>(this IEnumerable<T> source,
    int page = 1, int size = 10) where T : class
{
    if (page <= 0) page = 1;      // protect against bad input
    if (size <= 0) size = 10;

    var total = source.Count();
    var pages = (int)Math.Ceiling((decimal)total / size);   // total number of pages

    return source.Skip((page - 1) * size).Take(size);
}
```

Usage, showing a "records X - Y" label:

```csharp
var result = emps.Paginate(page, size);

var startRecord = ((page - 1) * size) + 1;
var endRecord   = result.Count() < size
                ? startRecord + result.Count() - 1
                : size * (page - 1) + size;
```

### Rules for good pagination
1. **Always sort first** (`OrderBy`) before paging. Without a stable order, pages can overlap or miss items.
2. **Validate** `page` and `size` (no zero or negative values).
3. A page number past the end returns an **empty** result, not an error.
4. Total pages = `Math.Ceiling(total / (double)size)`.

---

## Cheat sheet

| I want to... | Use |
|---|---|
| The first 5 | `Take(5)` |
| The last 5 | `TakeLast(5)` |
| Everything except the first 5 | `Skip(5)` |
| Everything except the last 5 | `SkipLast(5)` |
| Start until a condition breaks | `TakeWhile` |
| Everything from where a condition breaks | `SkipWhile` |
| Groups of N | `Chunk(N)` |
| Page number P with size S | `OrderBy(...).Skip((P-1)*S).Take(S)` |
| Top 3 highest paid | `OrderByDescending(x => x.Salary).Take(3)` |

## Run the examples

```
LINQTut06.Take         → Take, TakeWhile, TakeLast
LINQTut06.Skip         → Skip, SkipWhile, SkipLast
LINQTut06.Chunck       → Chunk
LINQTut06.Pagination   → Paginate extension (asks for page size and page number)
```

```bash
dotnet run --project LINQTut06.Pagination
```

## Things to notice in the sample code

- **`where T : class`** in `Paginate` means it works for objects (like `Employee`) but **not** for `int` or other value types. Remove the constraint to allow them.
- **`pages` is computed but never used** in `Paginate`. It becomes useful if you return the total number of pages to the caller.
- **`source.Count()` and `Skip/Take` each enumerate** the source. With an in-memory `List` this is fine. With a database (EF Core), `Skip/Take` becomes SQL `OFFSET ... FETCH`, which is efficient.
- **`214400`** in the `Skip`/`Take` projects is a salary that exists in that project's data. If you use different data, pick a salary that exists there, otherwise `TakeWhile` returns everything and `SkipWhile` returns nothing.
- **A page beyond the end:** `resultCount` is 0, so `endRecord` becomes `startRecord - 1` and the label looks odd (for example "showing employees 31 - 30"). Handle the empty case separately.
- The `Chunck` project name is a typo of `Chunk`. Rename it if you like.
