# LINQ Practice Tasks: `Where`, Lazy Execution, Syntax Styles

Do these **after** reading `LINQ_Lesson_README.md`. Try each task yourself before looking at any solution.

## Sample data

Use this class and list for the employee tasks (or use your own `Repository.LoadEmployees()`):

```csharp
class Employee
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Gender { get; set; }       // "male" / "female"
    public string Department { get; set; }  // "HR", "IT", "Finance", ...
    public decimal Salary { get; set; }
}
```

Add at least 10 employees with different genders, departments and salaries, including a few names that start with S and a few where the capitalization differs (`"Sara"`, `"sam"`).

---

## Level 1: Basics

### Task 1: Even and odd

Given:

```csharp
var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
```

Write queries using `Where` to get:

1. Odd numbers
2. Numbers greater than 5
3. Numbers divisible by 3

Print each result.

### Task 2: Names starting with a letter

Given:

```csharp
var names = new List<string> { "Sara", "Ali", "sam", "Omar", "Salma", "Mona" };
```

1. Get names that start with "S" (should match both `Sara` and `sam`).
2. Get names longer than 4 characters.
3. Get names that contain the letter "a" (any case).

**Hint:** use `ToLowerInvariant()`.

### Task 3: Employees by one rule

Write one `Where` query for each:

1. All male employees
2. Employees with salary >= 50,000
3. Employees in the "IT" department (make it work for "it", "It", "IT")

---

## Level 2: Combining and chaining

### Task 4: Multiple conditions with `&&` and `||`

1. Female employees in "HR"
2. Employees in "IT" **or** "Finance"
3. Employees with salary between 30,000 and 80,000 (inclusive)

### Task 5: Chaining

Create `var males = employees.Where(...)` first. Then, **reusing** `males`:

1. Males in "HR"
2. Males with salary > 100,000
3. Males in "IT" with a first name starting with "A"

Then rewrite #3 as **one** `Where` using `&&` and confirm the result is the same.

### Task 6: Write your own `Filter`

Write your own extension method without using `Where`:

```csharp
public static IEnumerable<T> Filter<T>(this IEnumerable<T> source, Func<T, bool> rule)
```

- Use `foreach` and `yield return`.
- Test it with numbers and with employees.
- Prove it gives the same result as `Where`.

**Bonus:** write `Filter` a second time using `Predicate<T>` instead of `Func<T, bool>`. What changes when you call it?

---

## Level 3: Lazy (deferred) execution

### Task 7: Predict the output

Do **not** run the code first. Write down what you think prints, then run it.

```csharp
var numbers = new List<int> { 1, 2, 3, 4, 5, 6 };

var big = numbers.Where(x => x > 3);

numbers.Add(10);
numbers.Remove(5);

foreach (var n in big)
    Console.Write($" {n}");
```

### Task 8: Freeze the result

Change Task 7 so that the output is **only** `4 5 6` (the values that were big *at the time the query was written*), even though the list changes afterwards.

**Hint:** which method forces immediate execution?

### Task 9: Count the executions

Add a `Console.WriteLine` inside your `Where` lambda:

```csharp
var evens = numbers.Where(x =>
{
    Console.WriteLine($"Checking {x}");
    return x % 2 == 0;
});
```

Answer, **before** running:

1. Does anything print when this line runs?
2. If you `foreach` over `evens` **twice**, how many times is each number checked?
3. If you add `.ToList()` and then loop twice, how many times is each number checked?

Run it and compare with your guess.

### Task 10: Which of these run immediately?

For each, write "lazy" or "immediate", then verify using the `Console.WriteLine` trick from Task 9:

```csharp
numbers.Where(x => x > 2)
numbers.Where(x => x > 2).ToList()
numbers.Where(x => x > 2).Count()
numbers.Where(x => x > 2).First()
numbers.Where(x => x > 2).ToArray()
numbers.Select(x => x * 2)
```

---

## Level 4: Three syntax styles

### Task 11: Write each query 3 ways

For each requirement, write (a) method syntax, (b) `Enumerable.Where(...)` static call, (c) query syntax (`from ... where ... select`):

1. Numbers greater than 4
2. Employees in "IT"
3. Female employees with salary > 40,000

Verify all three versions print the same result.

### Task 12: Translate

Convert this query-syntax code to method syntax:

```csharp
var result = from e in employees
             where e.Department == "HR" && e.Salary > 20000
             select e;
```

Then convert this method-syntax code to query syntax:

```csharp
var result = employees.Where(e => e.Gender == "female" && e.FirstName.StartsWith("S"));
```

---

## Level 5: Mini project

### Task 13: Employee report

Build a console app with a menu:

```
1. Show all employees
2. Show male employees
3. Show female employees whose name starts with S
4. Show employees by department (user types the department)
5. Show employees with salary above X (user types X)
6. Show male employees in HR with salary above X
0. Exit
```

Requirements:

- Use `Where` (and chaining where it helps).
- Write a small helper `Print(this IEnumerable<Employee> list, string title)` extension method.
- Department comparison must be case-insensitive.
- If no employees match, print "No results".

### Task 14: Think and explain (write short answers)

1. Why does `Where` return `IEnumerable<T>` and not `List<T>`?
2. What is the difference between **construction** and **enumeration** of a query?
3. Why can chaining `.Where(a).Where(b)` be reused better than a single `.Where(a && b)`?
4. When would you pick **query syntax** over **method syntax**?
5. How is the lambda inside `Where` related to `Func<T, bool>` and `Predicate<T>`?

---

## Checklist

- [ ] I can filter numbers, strings and objects with `Where`
- [ ] I can combine conditions with `&&`, `||` and by chaining
- [ ] I can write my own `Filter` using `Func<T, bool>`
- [ ] I can explain why a LINQ query reflects changes made *after* it was written
- [ ] I know how to force immediate execution (`ToList()`, `ToArray()`, ...)
- [ ] I can write the same query in method syntax and query syntax
