# LINQ Session 11: Element Operators (`ElementAt`, `First`, `Last`, `Single`)

These operators return **one item** from a sequence, not a sequence. They are **immediate execution** operators: they run right away and give you the item (or throw).

| Operator | Returns | If nothing is found |
|---|---|---|
| `ElementAt(i)` | The item at zero-based position `i` | Throws `ArgumentOutOfRangeException` |
| `ElementAtOrDefault(i)` | The item at position `i` | Returns `default` |
| `First()` / `First(rule)` | The first item (that matches) | Throws `InvalidOperationException` |
| `FirstOrDefault()` / `FirstOrDefault(rule)` | The first item (that matches) | Returns `default` |
| `Last()` / `Last(rule)` | The last item (that matches) | Throws `InvalidOperationException` |
| `LastOrDefault()` / `LastOrDefault(rule)` | The last item (that matches) | Returns `default` |
| `Single()` / `Single(rule)` | The **only** item (that matches) | Throws if **none** or **more than one** |
| `SingleOrDefault()` / `SingleOrDefault(rule)` | The **only** item (that matches) | `default` if none; **still throws if more than one** |

> `default` is `null` for classes and strings, `0` for numbers, `false` for `bool`.

---

## 1. `ElementAt` and `ElementAtOrDefault`

Get an item by its **position** (zero-based: the first item is index `0`).

```csharp
var questions = QuestionBank.All;

var q = questions.ElementAt(10);               // the 11th question (index 10)

// var q2 = questions.ElementAt(300);          // ArgumentOutOfRangeException (only 246 items)
var q3 = questions.ElementAtOrDefault(300);    // null, no exception

if (q3 is null)
    Console.WriteLine("No question at this position");
```

### Key points
- Index `10` is the **11th** item.
- `ElementAt` throws for a **negative** index or an index **past the end**.
- On a `List` or array it is instant. On a lazy sequence it must walk through the items until it reaches the position.
- .NET 6 and later also accept an `Index`, so `ElementAt(^1)` is the last item.

---

## 2. `First` and `FirstOrDefault`

```csharp
var first = questions.First();                                  // very first question
var firstShort = questions.First(q => q.Choices.Count == 2);    // first question matching a rule

// var none = questions.First(x => x.Title.Length == 0);        // InvalidOperationException
var none = questions.FirstOrDefault(x => x.Title.Length == 0);  // null

if (none is null)
    Console.WriteLine("Question is null");
```

### Key points
- With a rule, it **stops at the first match**, so it is fast.
- `First` on an **empty** sequence also throws.
- Use `First` when "not found" would be a **bug**. Use `FirstOrDefault` when "not found" is a **normal case** (and check for `null`).

---

## 3. `Last` and `LastOrDefault`

```csharp
var last = questions.Last();
var lastShort = questions.Last(q => q.Choices.Count == 2);

var none = questions.LastOrDefault(x => x.Title.Length == 0);   // null
```

### Key points
- Same rules as `First`, but from the end.
- On a plain lazy sequence, `Last` must go through **every** item. On a `List` or array it is instant.
- `Last` is a quick way to get the highest value after sorting: `OrderBy(x => x.Salary).Last()`. Using `OrderByDescending(...).First()` says the same thing more clearly.

---

## 4. `Single` and `SingleOrDefault`

Use them when you **expect exactly one** item, for example "find the employee with this unique ID".

```csharp
var q = questions.Single(x => x.Title.StartsWith("Q #7:"));     // exactly one match: OK
```

What happens in each situation:

| Matches found | `Single` | `SingleOrDefault` |
|---|---|---|
| 0 | throws `InvalidOperationException` | returns `default` (`null`) |
| 1 | returns it | returns it |
| 2 or more | throws `InvalidOperationException` | throws `InvalidOperationException` |

```csharp
// The bank has TWO questions numbered #245
var a = questions.Single(x => x.Title.Contains("#245"));          // throws: more than one match
var b = questions.SingleOrDefault(x => x.Title.Contains("#245")); // ALSO throws: more than one match
var c = questions.Single(x => x.Title.Length == 0);               // throws: no match
```

### Key points
- **`SingleOrDefault` only protects you from "zero".** It does **not** protect you from "more than one".
- `Single` has to check that there is **no second match**, so it keeps looking after the first match. It is slower than `First`.
- Exception messages tell you which case happened:
  - `"Sequence contains no matching element"` (none)
  - `"Sequence contains more than one matching element"` (too many)
- `Single` is a **data-quality check**: if the data should be unique and it is not, you want to know immediately.

### `First` vs `Single`

| | `First` | `Single` |
|---|---|---|
| Meaning | "Give me the first one" | "There must be exactly one; give it to me" |
| Several matches | Returns the first | **Throws** |
| Use when | Several are possible and you want one | The data must be unique (IDs, emails) |

---

## 5. The `default` trap with value types

```csharp
var numbers = new List<int> { 5, 3, 9, 1 };

var x = numbers.FirstOrDefault(n => n > 100);   // 0
```

`0` could mean "not found" or a real `0` in the list. You cannot tell them apart. Options:

```csharp
// 1) Check first
if (numbers.Any(n => n > 100)) { ... }

// 2) Project to a nullable type
int? y = numbers.Where(n => n > 100).Select(n => (int?)n).FirstOrDefault();   // null when not found

// 3) .NET 6+: choose your own default
var z = numbers.FirstOrDefault(n => n > 100, -1);   // -1
```

For classes, `null` is clear: it can only mean "not found".

---

## 6. Choosing the right operator

| Situation | Use |
|---|---|
| I know the position | `ElementAt` / `ElementAtOrDefault` |
| I want the first/last one, and it should exist | `First` / `Last` |
| I want the first/last one, and it may not exist | `FirstOrDefault` / `LastOrDefault` |
| It must be unique, and it should exist | `Single` |
| It must be unique, but may not exist | `SingleOrDefault` |
| I want the top/bottom after sorting | `OrderBy(...).First()` / `OrderByDescending(...).First()` |
| I want a small group, not one item | `Take(n)` (returns a sequence) |

> There is no query-syntax keyword for these operators. Wrap the query in parentheses and call the method: `(from q in questions select q).First()`.

## Run the examples

```
LINQTut11.ElementAt   → ElementAt vs ElementAtOrDefault
LINQTut11.First       → First vs FirstOrDefault
LINQTut11.Last        → Last vs LastOrDefault
LINQTut11.Single      → Single / SingleOrDefault (all calls are commented out; only ToQuiz runs)
```

## Things to notice in the sample code

- **`ElementAt(10)` is the 11th question** (`Q #11`), because indexes start at 0.
- **`QuestionBank.All` creates a new list every time it is read** (it is declared with `=>`), so each call builds 246 new objects. Store it in a variable once, as the samples do.
- **The `Single` example is all commented out.** Its purpose is to show the data with `ToQuiz()`, where you can see that `#245` appears **twice**. That duplicate makes both `Single` and `SingleOrDefault` throw. Students should uncomment the lines one at a time and predict the result.
- **The `if (questionAt300 is null) { }` block is empty.** Add a message to make the example clearer.
- **`Console.WriteLine(questionAt300)`** prints an empty line when the value is `null`. It does not throw.
