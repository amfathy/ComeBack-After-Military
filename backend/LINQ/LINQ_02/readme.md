# LINQ Lesson 2: `Where`, Lazy Execution, and Syntax Styles (Simple Explanation)

This lesson has 4 small examples. Each one teaches one idea:

| Example | Idea |
|---|---|
| Ex01 | `Where` is just a *filter* method that you can also write yourself |
| Ex02 | LINQ is **lazy**: the query doesn't run when you write it |
| Ex03 | There are 3 ways to write the same query |
| Ex04 | You can **chain** filters one after another |

---

## First, what is LINQ?

**LINQ = Language Integrated Query.** It lets you ask questions about a collection (a list, an array) using C# code, like you would ask a database.

- "Give me only the even numbers."
- "Give me only the female employees whose name starts with S."

```csharp
using System.Linq;   // you need this line to use LINQ
```

---

## Ex01: What does `Where` really do?

```csharp
var result = employees.Filter(x =>
    x.Gender == "female" && x.FirstName.ToLowerInvariant().StartsWith("s"));

var result2 = employees.Where(x =>
    x.Gender == "female" && x.FirstName.ToLowerInvariant().StartsWith("s"));
```

Both lines do **the same thing**. `Filter` is a method the course author wrote by hand; `Where` is the one Microsoft already wrote for you.

### How would you write `Filter` yourself?

It's the delegate idea from the first README (`Predicate` / `Func<T, bool>`):

```csharp
public static IEnumerable<T> Filter<T>(this IEnumerable<T> source, Func<T, bool> rule)
{
    foreach (var item in source)
    {
        if (rule(item))          // ask the rule: "should I keep this one?"
            yield return item;   // yes -> give it back
    }
}
```

Think of it like a **security guard** at a door:

```
all employees --> [ rule: true or false? ] --> only the ones that passed
```

- The **lambda** `x => x.Gender == "female" && ...` is the rule.
- `x` is one employee at a time.
- The rule must return `true` (keep) or `false` (throw away).

### Breaking down the lambda

```csharp
x => x.Gender == "female" && x.FirstName.ToLowerInvariant().StartsWith("s")
```

| Part | Meaning |
|---|---|
| `x =>` | "for each employee, call it `x`" |
| `x.Gender == "female"` | gender must be female |
| `&&` | **AND**: both sides must be true |
| `x.FirstName.ToLowerInvariant()` | make the name lowercase first |
| `.StartsWith("s")` | does it start with "s"? |

Why `ToLowerInvariant()`? So that "Sara" and "sara" both match, because we compare against a lowercase "s".

### What is `Print(...)`?

It's a helper from `LINQTut03.Shared` that prints the list with a title. It's not part of LINQ.

---

## Ex02: LINQ is lazy (the most important idea)

```csharp
List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

IEnumerable<int> evenNumbers = numbers.Where(x => x % 2 == 0);  // (1)

numbers.Add(10);
numbers.Add(12);
numbers.Remove(4);

foreach (var n in evenNumbers)   // (2)
    Console.Write($" {n}");
```

### What do you think prints?

Many people guess `2 4 6 8` (the evens that existed at line 1). **That is wrong.**

The real output is:

```
2 6 8 10 12
```

### Why?

Line (1) does **not** run the filter. It only saves a **recipe**: "when someone asks, give only even numbers."

The filter runs at line (2), when `foreach` actually asks for the items. By then the list has changed, so the recipe uses the *new* list.

Think of a **restaurant order**:

| Moment | Restaurant | LINQ |
|---|---|---|
| You say what you want | You give the waiter your order | `numbers.Where(...)` |
| Food is made | The kitchen cooks when it's time to serve | `foreach` |

If you change the ingredients (the list) between ordering and serving, you get a different dish.

### Two key words

| Term | Meaning | Happens at |
|---|---|---|
| **Deferred execution / lazy** | Query is only *described* | `Where(...)`, `Select(...)` |
| **Immediate execution** | Query actually *runs* | `foreach`, `ToList()`, `ToArray()`, `Count()`, `First()` |

### How to "freeze" the result

If you want the result **as of now**, run it immediately:

```csharp
var evenNow = numbers.Where(x => x % 2 == 0).ToList();   // runs now

numbers.Add(10);   // evenNow is NOT affected
```

---

## Ex03: Three ways to write the same query

All three give **exactly the same result**:

```csharp
// 1) Extension method (most common)
var a = numbers.Where(x => x % 2 == 0);

// 2) Calling the static method directly (same as 1, just written differently)
var b = Enumerable.Where(numbers, x => x % 2 == 0);

// 3) Query syntax (looks like SQL)
var c = from n in numbers
        where n % 2 == 0
        select n;
```

### Comparing them

| Style | Looks like | Use when |
|---|---|---|
| Method syntax (1) | `numbers.Where(...)` | Most of the time. Short and popular |
| Static call (2) | `Enumerable.Where(numbers, ...)` | Rare. Mostly to understand what (1) really is |
| Query syntax (3) | SQL | Long queries with joins/groups |

**Style 1 is secretly style 2.** `Where` is an *extension method*, which lets you write `numbers.Where(...)` instead of `Enumerable.Where(numbers, ...)`. The compiler converts one into the other.

### Query syntax in plain words

```csharp
from n in numbers     // take each item, call it n, from numbers
where n % 2 == 0      // keep it only if it's even
select n;             // give me n
```

Compare with SQL: `SELECT n FROM numbers WHERE n % 2 = 0`. Same idea, different order (C# starts with `from`).

---

## Ex04: Chaining filters

```csharp
var empMale = employees.Where(x => x.Gender == "male");

var empsSalaryOver300K = employees.Where(x => x.Salary >= 300_000);

var empMaleInHRDepartment =
    empMale.Where(x => x.Department.ToLowerInvariant() == "hr");
```

### Idea 1: Each `Where` is a new, smaller filter

```
all employees
     |  Where gender == male
     v
male employees
     |  Where department == hr
     v
male employees in HR
```

Like a **coffee filter on top of another filter**: each layer removes more.

### Idea 2: Chaining = AND

These two are equivalent:

```csharp
employees.Where(x => x.Gender == "male").Where(x => x.Department == "hr");

employees.Where(x => x.Gender == "male" && x.Department == "hr");
```

Chaining is nice when you want to **reuse** a smaller query (like `empMale` above) in several places.

### Idea 3: `300_000` is just `300000`

The underscore is only for readability. C# ignores it.

### Idea 4: It's still lazy

`empMale` and `empMaleInHRDepartment` don't run until `Print(...)` loops over them. Nothing is calculated before that.

---

## Cheat sheet

```csharp
// Filter
list.Where(x => condition)

// Run it now and save a snapshot
list.Where(x => condition).ToList()

// Chain filters (AND)
list.Where(a).Where(b)

// Query syntax
from x in list where condition select x
```

| Remember | Because |
|---|---|
| `Where` takes a rule (`Func<T, bool>`) | It's a delegate: the lambda *is* the delegate |
| `Where` is lazy | It runs only when you loop or call `ToList()` etc. |
| Method syntax = query syntax | They produce the same result |
| `Where` never changes the original list | It only produces a new filtered view |

### Connection to the delegate README

```
Where(x => x % 2 == 0)
         ^
         |__ this lambda is a Func<int, bool> (basically a Predicate<int>)
```

If you understood `Predicate<T>` and `Func<T, bool>` earlier, you already understand how `Where` works.
