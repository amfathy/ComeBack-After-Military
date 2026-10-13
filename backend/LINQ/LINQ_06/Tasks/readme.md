# LINQ Session 7: Tasks (`Any`, `All`, `Contains`)

## How to use

1. Create a new Console project (.NET 5 or later).
2. Replace `Program.cs` with the **Starter Code** below. It already has the data, classes and helper, so **no shared project is needed**.
3. Solve each task in its `// TODO` area. For yes/no tasks, print a full sentence such as `Console.WriteLine($"... : {result}")`. For list tasks, use `Print(...)`.
4. Use **method syntax** unless the task says **(both)**, in which case also write a **query syntax** version (`where` with `Any`/`All` inside).

---

## Starter Code (copy into `Program.cs`)

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSession7Tasks
{
    // ---------- Model ----------
    public class Employee
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public decimal Salary { get; set; }
        public List<string> Skills { get; set; } = new List<string>();

        public override string ToString() =>
            $"{Name.PadRight(20)}\t{Email.PadRight(32)}\t{Salary,5:N0}  [ {string.Join(", ", Skills)} ]";
    }

    // ---------- Data (25 employees) ----------
    public static class Repository
    {
        private static Employee E(string name, string email, decimal salary, params string[] skills) =>
            new Employee { Name = name, Email = email, Salary = salary, Skills = skills.ToList() };

        public static List<Employee> LoadEmployees() => new()
        {
            E("Cochran Cole",       "Cole.Cochran01@example.com",     1032, "Javascript", "C++", "NodeJS"),
            E("Jaclyn Wolfe",       "Wolfe.Jaclyn@example.com",       1924, "C#", "Javascript", "ASP.NET"),
            E("Cochran Cole",       "Cole.Cochran02@example.com",     1728, "ASP.NET", "Javascript"),
            E("Hester Evans",       "Evans.Hester@example.com",       1555, "CSS", "Javascript", "NodeJS"),
            E("Wallace Buck",       "Buck.Wallace@example.com",       3158, "Oracle", "HTML", "CSS"),
            E("Acevedo Wall",       "Wall.Acevedo@example.com",       3437, "ASP.NET"),
            E("Jacqueline Pickett", "Pickett.Jacqueline@example.com", 3700, "NodeJS"),
            E("Oconnor Espinoza",   "Espinoza.Oconnor@example.com",   1556, "SQL Server", "C#", "HTML", "C++"),
            E("Allie Elliott",      "Elliott.Allie@example.com",      3154, "Oracle"),
            E("Elva Decker",        "Decker.Elva@example.com",        3459, "ASP.NET", "Javascript", "C#"),
            E("Hayes Beasley",      "Beasley.Hayes@example.com",      3727, "SQL Server", "C++", "NodeJS", "Oracle", "Javascript"),
            E("Florine Cervantes",  "Cervantes.Florine@example.com",  3387, "HTML"),
            E("Bullock Carney",     "Carney.Bullock@example.com",     2144, "C++", "CSS"),
            E("Carroll Cantu",      "Cantu.Carroll@example.com",      3432, "C++", "C#"),
            E("Debra Hogan",        "Hogan.Debra@example.com",        2491, "Oracle", "NodeJS", "HTML", "ASP.NET"),
            E("Winnie Mccall",      "Mccall.Winnie@example.com",      2873, "CSS", "HTML"),
            E("Manuela Berger",     "Berger.Manuela@example.com",     1725, "Oracle"),
            E("Lakeisha Lowe",      "Lowe.Lakeisha@example.com",      3143, "NodeJS", "Oracle", "CSS", "ASP.NET"),
            E("Stewart Lott",       "Lott.Stewart@example.com",       1466, "ASP.NET", "C#", "Javascript", "SQL Server", "HTML"),
            E("Stafford Peck",      "Peck.Stafford@example.com",      3207, "C++", "ASP.NET"),
            E("Barron Bird",        "Bird.Barron@example.com",        1512, "NodeJS", "Javascript", "ASP.NET"),
            E("Nona Brooks",        "Brooks.Nona@example.com",        1365, "HTML", "NodeJS"),
            E("Clara Reeves",       "Reeves.Clara@example.com",       2458, "ASP.NET", "NodeJS"),
            E("Karin Blanchard",    "Blanchard.Karin@example.com",    3412, "C#", "SQL Server", "NodeJS", "CSS", "HTML"),
            E("Burris Morgan",      "Morgan.Burris@example.com",      3603, "C++", "CSS", "ASP.NET", "Oracle"),
        };
    }

    // ---------- Helper ----------
    public static class Extensions
    {
        public static void Print<T>(this IEnumerable<T> source, string title)
        {
            Console.WriteLine();
            Console.WriteLine($"=== {title} ===");
            foreach (var item in source)
            {
                if (typeof(T).IsValueType) Console.Write($" {item} ");
                else Console.WriteLine(item);
            }
            Console.WriteLine();
        }
    }

    // ---------- TODO (Part 4): write your IEqualityComparer<Employee> here ----------


    internal class Program
    {
        static void Main(string[] args)
        {
            // ---------- Sample data ----------
            var numbers = new List<int> { 2, 4, 6, 7, 8, 10 };
            var evens   = new List<int> { 2, 4, 6, 8 };
            var names   = new List<string> { "Sara", "Ali", "sam", "Omar", "Salma", "Mona" };
            var employees = Repository.LoadEmployees();

            // ===================== PART 1: Any =====================
            // TODO: Task 1.1 ... 1.6


            // ===================== PART 2: All =====================
            // TODO: Task 2.1 ... 2.6


            // ===================== PART 3: Any / All inside queries =====================
            // TODO: Task 3.1 ... 3.5


            // ===================== PART 4: Contains =====================
            // TODO: Task 4.1 ... 4.5


            // ===================== PART 5: Mixed =====================
            // TODO: Task 5.1 ... 5.4

            Console.ReadKey();
        }
    }
}
```

---

## Part 1: `Any`

**Task 1.1.** Is there an employee whose name starts with `"jac"` (ignoring case)?
*Expected:* `true` (Jaclyn and Jacqueline).

**Task 1.2.** Is there an employee with a salary **below 1,000**? Then below **1,100**?
*Expected:* `false`, then `true`.

**Task 1.3.** Is there an employee with **no skills** at all?
*Expected:* `false`.

**Task 1.4.** Is there an employee who knows `"Python"`? Then one who knows `"C#"`?
*Expected:* `false`, then `true`.

**Task 1.5.** Use `Any()` **without a rule** on: (a) `employees`, (b) `employees.Where(e => e.Salary > 5000)`, (c) an empty `List<int>`.
*Expected:* `true`, `false`, `false`.

**Task 1.6.** Write the same check as Task 1.1 using `Count() > 0` and using `Any`. Which is better, and why? (Answer in a comment.)

---

## Part 2: `All`

**Task 2.1.** Do all employees have an email?
*Expected:* `true`.

**Task 2.2.** Do all employees have at least one skill?
*Expected:* `true`.

**Task 2.3.** Do all employees earn more than 1,000? Then more than 1,500?
*Expected:* `true`, then `false`.

**Task 2.4.** Do all employees know `"C#"`?
*Expected:* `false`.

**Task 2.5.** Call `All` and `Any` on an **empty** list (`new List<int>()`) with the rule `x > 100`. Print both results and explain them in a comment.
*Expected:* `All` is `true`, `Any` is `false`.

**Task 2.6.** Prove that `All(rule)` equals `!Any(x => !rule(x))` for the rule `e.Salary > 1000` on `employees`.

---

## Part 3: `Any` / `All` inside queries (filter by a nested list)

**Task 3.1. (both)** Get the employees who have **any** skill equal to `"NodeJS"`.
*Expected:* 10 employees.

**Task 3.2. (both)** Get the employees whose skills **all** contain the letter `"c"` (ignoring case).
*Expected:* 4 employees: `Allie Elliott`, `Bullock Carney`, `Carroll Cantu`, `Manuela Berger`.

**Task 3.3.** Get the employees who know **both** `"C#"` and `"SQL Server"`.
*Hint:* two `Any` calls joined with `&&`.
*Expected:* `Oconnor Espinoza`, `Stewart Lott`, `Karin Blanchard`.

**Task 3.4.** Get the "front-end only" employees: those whose skills are **all** within `{ "HTML", "CSS", "Javascript" }`.
*Hint:* `emp.Skills.All(s => frontEnd.Contains(s))`.
*Expected:* `Florine Cervantes` and `Winnie Mccall`.

**Task 3.5.** Get the employees who have **at least one skill starting with "A"**.
*Expected:* 11 employees.

---

## Part 4: `Contains`

**Task 4.1.** Check if `numbers` contains `7`, and if `evens` contains `7`.
*Expected:* `true`, `false`.

**Task 4.2.** Check if `names` contains `"SAM"`. Then repeat it ignoring case.
*Expected:* `false`, then `true`.

**Task 4.3.** Take the **first employee** from the list and check `employees.Contains(thatEmployee)`.
*Expected:* `true`.

**Task 4.4.** Create a **new** `Employee` object with `Email = "Cole.Cochran02@example.com"` (other fields empty) and call `employees.Contains(newEmployee)`.
*Expected:* `false`. Explain why in a comment.

**Task 4.5.** Fix Task 4.4 in **two ways**:
1. Write a class `EmployeeEmailComparer : IEqualityComparer<Employee>` (compare by `Email`, ignoring case) and call `Contains(newEmployee, new EmployeeEmailComparer())`.
2. Use `Any(e => e.Email == ...)` instead.

Both must return `true`.

---

## Part 5: Mixed

**Task 5.1.** Is there **any employee above the average salary**? Is it also true that **all** employees are above the average?
*Hint:* `Average` first, then `Any` / `All`.
*Expected:* `true`, then `false`.

**Task 5.2.** Do all employees who earn **more than 3,500** have **at least 2 skills**?
*Expected:* `false` (Jacqueline Pickett has only one skill). Then print the employees that break the rule.
*Hint:* `Where` + `Count`/`Any`.

**Task 5.3.** Get the employees who have **no** `"Oracle"` skill, using `!Any(...)`.
*Expected:* 18 employees.
*(Check: 7 employees have Oracle.)*

**Task 5.4.** Write a method `bool KnowsAll(Employee e, params string[] required)` that returns `true` if the employee has **all** the required skills. Use it to find employees who know `"ASP.NET"` and `"Javascript"`.
*Expected:* `Jaclyn Wolfe`, `Cochran Cole` (1728), `Elva Decker`, `Stewart Lott`, `Barron Bird`.
*Hint:* `required.All(r => e.Skills.Contains(r))`.

---

## Challenge (optional)

Build a "skill search" that takes a text like `"node"` from the user and prints:
- whether **any** employee has a matching skill,
- how many employees match,
- their names.

Make the search case-insensitive and handle empty input without crashing.

---

## Submission checklist

- [ ] Every yes/no task prints a clear sentence with its result
- [ ] You can explain `Any` vs `All` vs `Contains`
- [ ] You can explain why `All` on an empty list is `true`
- [ ] You can explain why `Contains(newEmployee)` returns `false` for an equal-looking object
- [ ] You know when to prefer `Any(rule)` over `Contains`
