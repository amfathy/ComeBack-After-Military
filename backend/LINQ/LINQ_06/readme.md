# LINQ Session 7: Quantifiers (`Any`, `All`, `Contains`)

These operators do not return a sequence. They answer a **yes/no question** about a sequence and return a `bool`.

| Operator | Question it answers | Returns `true` when... |
|---|---|---|
| `Any()` | Is the sequence not empty? | It has at least one item |
| `Any(rule)` | Does **at least one** item match? | One or more items satisfy the rule |
| `All(rule)` | Do **all** items match? | Every item satisfies the rule |
| `Contains(value)` | Is this value in the sequence? | An item equals the value |

> These are **immediate execution** operators (they run right away and give a `bool`), and they **short-circuit**: they stop as soon as the answer is known.

---

## 1. `Any`

### With a rule: "is there at least one?"

```csharp
// Is there an employee whose name starts with "jac"? (ignoring case)
bool r1 = employees.Any(e => e.Name.StartsWith("jac", StringComparison.OrdinalIgnoreCase));

// Is there an employee earning less than 10,000?
bool r2 = employees.Any(e => e.Salary < 10000);

// Is there an employee with fewer than 1 skill (no skills at all)?
bool r3 = employees.Any(e => e.Skills.Count < 1);
```

### Without a rule: "is it not empty?"

```csharp
bool hasEmployees = employees.Any();
```

### Key points
- It **stops at the first match**, so it is faster than counting.
- Prefer `Any()` over `Count() > 0`. `Any()` can stop after one item, while `Count()` may need to walk the whole sequence.
- `Any()` on an **empty** sequence is always `false`.

---

## 2. `All`

```csharp
// Do all employees have an email?
bool r1 = employees.All(e => !string.IsNullOrWhiteSpace(e.Email));

// Do all employees know C#?
bool r2 = employees.All(e => e.Skills.Any(s => s.Contains("C#")));
```

### Key points
- It **stops at the first item that fails** the rule.
- `All()` on an **empty** sequence is always `true` (nothing can break the rule). This is called *vacuous truth* and often surprises students.
- `All(rule)` is the same as `!Any(x => !rule(x))`.

```csharp
employees.All(e => e.Salary > 1000)
// is the same as
!employees.Any(e => e.Salary <= 1000)
```

### `Any` vs `All` side by side

| | `Any(rule)` | `All(rule)` |
|---|---|---|
| Needs | at least one match | every item to match |
| Stops when | first match found (true) | first mismatch found (false) |
| Empty sequence | `false` | `true` |

---

## 3. `Any` / `All` inside a query (query syntax)

There is **no query keyword** for `Any` or `All`. Use them as method calls inside `where`:

```csharp
// Employees where ALL skills contain the letter "c"
var result1 = from emp in employees
              where emp.Skills.All(s => s.Contains("c", StringComparison.OrdinalIgnoreCase))
              select emp;

// Employees where ANY skill contains "node"
var result2 = from emp in employees
              where emp.Skills.Any(s => s.Contains("node", StringComparison.OrdinalIgnoreCase))
              select emp;
```

Here `Any`/`All` are used on the **inner list** (`emp.Skills`) to decide whether each employee is kept. This is a very common pattern: **a filter on a nested collection**.

---

## 4. `Contains`

Checks whether a **specific value** is in the sequence.

```csharp
int[] numbers = { 1, 3, 5, 7 };
numbers.Contains(5);     // true
numbers.Contains(4);     // false
```

### Strings: case sensitivity

```csharp
string[] names = { "Sara", "Ali", "sam" };
names.Contains("SAM");                                   // false
names.Contains("SAM", StringComparer.OrdinalIgnoreCase); // true
```

### Objects: the reference-equality trap

```csharp
var e1 = employees.First();
employees.Contains(e1);          // true   (same object in memory)

var e2 = new Employee { Email = "Cole.Cochran02@example.com" };
employees.Contains(e2);          // false  (a new object, even if the data matches)
```

By default, `Contains` compares objects by **reference** (are they the same object in memory?), not by their values. Two options:

**Option A: pass an `IEqualityComparer<T>`**

```csharp
class EmployeeEmailComparer : IEqualityComparer<Employee>
{
    public bool Equals(Employee x, Employee y) =>
        string.Equals(x?.Email, y?.Email, StringComparison.OrdinalIgnoreCase);

    public int GetHashCode(Employee obj) =>
        obj.Email?.ToLowerInvariant().GetHashCode() ?? 0;
}

employees.Contains(e2, new EmployeeEmailComparer());   // true
```

**Option B: override `Equals` and `GetHashCode` in the class** (or use a `record`), and then plain `Contains(e2)` compares by value.

**Option C: just use `Any` with a rule**

```csharp
employees.Any(e => e.Email == "Cole.Cochran02@example.com");   // true
```

Option C is usually the simplest when you only need a single check.

### Key points
- For numbers, strings and other simple types, `Contains` works as expected.
- For your own classes, remember the reference-equality default.
- A `string` is a sequence of `char`, so `"hello".Contains('l')` can use LINQ `Contains` over characters.

---

## Cheat sheet

| I want to know... | Use |
|---|---|
| Is the list not empty? | `Any()` |
| Is there at least one that matches? | `Any(rule)` |
| Do all of them match? | `All(rule)` |
| Is there none that matches? | `!Any(rule)` |
| Is this value in the list? | `Contains(value)` |
| Same, ignoring case | `Contains(value, StringComparer.OrdinalIgnoreCase)` |
| Is this object (by its data) in the list? | `Contains(obj, comparer)` or `Any(rule)` |
| Filter by a nested list | `where emp.Skills.Any(...)` / `All(...)` |

## Run the examples

```
RunAny()                → Any with different rules
RunAll()                → All
RunAnyAllQuerySyntax()  → Any / All inside "where" in query syntax
RunContain()            → Contains with the object-reference trap
```

## Things to notice in the sample code

- **Comments do not match the code in a few places** (good discussion material):
  - `"less tham 1000"` is written, but the value used is `10000`.
  - `"all employees have at least 1 skill"` is written, but the code checks for **C#**.
  - `"contains 'ee' in his/her name"` is written, but the code uses `Contains(employee)` with an object, and prints the **email**.
- **`result3`** (`Skills.Count < 1`) asks "is there an employee with **no skills**?". With the sample data the answer is `false`.
- **`RunContain` result2 is `false`** even though an employee with that email exists. The new `Employee` is a different object, so `Contains` uses reference equality. This is the main lesson of that example.
