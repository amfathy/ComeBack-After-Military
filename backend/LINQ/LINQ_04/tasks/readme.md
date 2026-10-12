# LINQ Session 5: Tasks (Sorting)

## How to use

1. Create a new Console project (.NET 5 or later).
2. Replace `Program.cs` with the **Starter Code** below. It already has the data, classes and helper, so **no shared project is needed**.
3. Solve each task in its `// TODO` area and display it with `Print(...)`.
4. Use **method syntax** unless the task says **(both)**, in which case also write the **query syntax** version.

> Employee numbers look like `2017-FI-1343`, which means `Year-Department-Sequence`.
> Department codes: `FI` Finance, `IT`, `HR`, `AC` Accounting.

---

## Starter Code (copy into `Program.cs`)

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSession5Tasks
{
    // ---------- Model ----------
    public class Employee
    {
        public string EmployeeNo { get; set; }   // "2017-FI-1343"
        public string Name { get; set; }
        public string Email { get; set; }
        public decimal Salary { get; set; }
        public List<string> Skills { get; set; } = new List<string>();

        public override string ToString() =>
            $"{EmployeeNo.PadRight(13)}\t{Name.PadRight(20)}\t{Email.PadRight(32)}\t{Salary,5:N0}  [ {string.Join(", ", Skills)} ]";
    }

    // ---------- Data ----------
    public static class Repository
    {
        private static Employee E(string no, string name, string email, decimal salary, params string[] skills) =>
            new Employee { EmployeeNo = no, Name = name, Email = email, Salary = salary, Skills = skills.ToList() };

        public static List<Employee> LoadEmployees() => new()
        {
            E("2017-FI-1343", "Cochran Cole",        "Cole.Cochran01@example.com",     1032, "Javascript", "C++", "NodeJS"),
            E("2018-FI-1755", "Jaclyn Wolfe",        "Wolfe.Jaclyn@example.com",       1924, "C#", "Javascript", "ASP.NET"),
            E("2016-IT-1441", "Cochran Cole",        "Cole.Cochran02@example.com",     1728, "ASP.NET", "Javascript"),
            E("2016-FI-3447", "Hester Evans",        "Evans.Hester@example.com",       1555, "CSS", "Javascript", "NodeJS"),
            E("2014-IT-2956", "Wallace Buck",        "Buck.Wallace@example.com",       3158, "Oracle", "HTML", "CSS"),
            E("2020-IT-6341", "Acevedo Wall",        "Wall.Acevedo@example.com",       3437, "ASP.NET"),
            E("2021-IT-4488", "Jacqueline Pickett",  "Pickett.Jacqueline@example.com", 3700, "NodeJS"),
            E("2017-HR-3248", "Oconnor Espinoza",    "Espinoza.Oconnor@example.com",   1556, "SQL Server", "C#", "HTML", "C++"),
            E("2020-AC-9097", "Allie Elliott",       "Elliott.Allie@example.com",      3154, "Oracle"),
            E("2016-HR-3770", "Elva Decker",         "Decker.Elva@example.com",        3459, "ASP.NET", "Javascript", "C#"),
            E("2020-HR-8809", "Hayes Beasley",       "Beasley.Hayes@example.com",      3727, "SQL Server", "C++", "NodeJS", "Oracle", "Javascript"),
            E("2015-FI-3038", "Florine Cervantes",   "Cervantes.Florine@example.com",  3387, "HTML"),
            E("2017-AC-7409", "Bullock Carney",      "Carney.Bullock@example.com",     2144, "C++", "CSS"),
            E("2021-FI-3725", "Carroll Cantu",       "Cantu.Carroll@example.com",      3432, "C++", "C#"),
            E("2019-IT-9738", "Debra Hogan",         "Hogan.Debra@example.com",        2491, "Oracle", "NodeJS", "HTML", "ASP.NET"),
            E("2019-IT-7184", "Winnie Mccall",       "Mccall.Winnie@example.com",      2873, "CSS", "HTML"),
            E("2015-IT-4664", "Manuela Berger",      "Berger.Manuela@example.com",     1725, "Oracle"),
            E("2017-IT-3609", "Lakeisha Lowe",       "Lowe.Lakeisha@example.com",      3143, "NodeJS", "Oracle", "CSS", "ASP.NET"),
            E("2016-FI-2682", "Stewart Lott",        "Lott.Stewart@example.com",       1466, "ASP.NET", "C#", "Javascript", "SQL Server", "HTML"),
            E("2014-IT-3754", "Stafford Peck",       "Peck.Stafford@example.com",      3207, "C++", "ASP.NET"),
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

    // ---------- TODO (Part 4): write your comparers here ----------


    internal class Program
    {
        static void Main(string[] args)
        {
            // ---------- Sample data ----------
            string[] fruits = { "apricot", "orange", "banana", "mango", "apple", "grape", "strawberry" };
            var numbers = new List<int> { 5, 3, 9, 1, 7, 3, 8 };
            var names   = new List<string> { "sara", "Ali", "sam", "Omar", "Salma", "mona" };
            var employees = Repository.LoadEmployees();

            // ===================== PART 1: OrderBy / OrderByDescending =====================
            // TODO: Task 1.1 ... 1.6


            // ===================== PART 2: ThenBy / ThenByDescending =====================
            // TODO: Task 2.1 ... 2.4


            // ===================== PART 3: Reverse and Take =====================
            // TODO: Task 3.1 ... 3.4


            // ===================== PART 4: Custom comparer =====================
            // TODO: Task 4.1 ... 4.4


            // ===================== PART 5: Mixed =====================
            // TODO: Task 5.1 ... 5.4

            Console.ReadKey();
        }
    }
}
```

---

## Part 1: `OrderBy` / `OrderByDescending`

**Task 1.1.** Sort `fruits` alphabetically (A to Z). **(both)**
*Expected:* `apple, apricot, banana, grape, mango, orange, strawberry`

**Task 1.2.** Sort `fruits` from Z to A. **(both)**

**Task 1.3.** Sort `fruits` by **length**, shortest first. **(both)**
*Question:* What order do `apple`, `grape` and `mango` appear in? Why? (Hint: stable sort.)

**Task 1.4.** Sort `numbers` descending.
*Expected:* `9, 8, 7, 5, 3, 3, 1`

**Task 1.5.** Sort `employees` by `Salary` ascending, then again descending.
*Expected first employee:* `Cochran Cole` (1032) ascending, `Hayes Beasley` (3727) descending.

**Task 1.6.** Sort `names` alphabetically **ignoring case**. Then sort it with the default comparison and compare the outputs.
*Hint:* `StringComparer.OrdinalIgnoreCase`.

---

## Part 2: `ThenBy` / `ThenByDescending`

**Task 2.1.** Sort `fruits` by length, then alphabetically. **(both)**
*Expected:* `apple, grape, mango, banana, orange, apricot, strawberry`

**Task 2.2.** Sort `employees` by `Name`, then by `Salary` **descending**. **(both)**
*Check:* the two `Cochran Cole` records must appear with salary `1728` before `1032`.

**Task 2.3.** Sort `employees` by **department** (the middle part of `EmployeeNo`), then by `Salary` descending.
*Hint:* `e.EmployeeNo.Split('-')[1]`

**Task 2.4.** Sort `employees` by **hiring year** descending (first part of `EmployeeNo`, converted to `int`), then by `Name` ascending.

*Question:* What happens if you write `.OrderBy(x => x.Name).OrderBy(x => x.Salary)` instead of `ThenBy`? Test it and explain.

---

## Part 3: `Reverse` and `Take`

**Task 3.1.** Reverse `fruits` using LINQ `Reverse`. Print both the original and the reversed result.
*Expected reversed:* `strawberry, grape, apple, mango, banana, orange, apricot`

**Task 3.2.** Create a `List<int>` copy of `numbers`. Call `list.Reverse();` and print the list. In a comment, explain how this differs from LINQ `Reverse` on `fruits`. (Does it return a value? Does it change the original?)

**Task 3.3.** Get the **3 highest-paid** employees (`OrderByDescending` + `Take`).
*Expected salaries:* `3727, 3700, 3459`

**Task 3.4.** Get the **3 lowest-paid** employees in **descending** order of salary. Use `OrderBy` + `Take` + `Reverse`, then write the same result a second way without `Reverse`.
*Expected salaries (descending):* `1555, 1466, 1032`

---

## Part 4: Custom comparer (`IComparer<T>`)

**Task 4.1.** Write `EmployeeNumberComparer : IComparer<Employee>` that sorts by **year**, then by **sequence number** as an integer (the last part of `EmployeeNo`). Use it with `OrderBy(e => e, new EmployeeNumberComparer())`.
*Expected first three:* `2014-IT-2956`, `2014-IT-3754`, `2015-FI-3038`

**Task 4.2.** Write `DepartmentThenYearComparer` that sorts by department (alphabetical) and then by year **descending**.

**Task 4.3.** Write `LengthThenAlphaComparer : IComparer<string>` and sort `fruits` with it. The result must match Task 2.1.

**Task 4.4.** Sort employees by `EmployeeNo` as plain text: `OrderBy(e => e.EmployeeNo)`. Compare with Task 4.1. Find **one place** where the order differs and explain why.

---

## Part 5: Mixed

**Task 5.1.** Get the employees in the **IT** department sorted by salary (highest first). Show only `Name` and `Salary` as `"Jacqueline Pickett - 3700"`.
*Operators:* `Where` + `OrderByDescending` + `Select`.
*Expected first:* `Jacqueline Pickett - 3700`. There are 9 results.

**Task 5.2.** Get all **distinct skills** across all employees, sorted alphabetically.
*Operators:* `SelectMany` + `Distinct` + `OrderBy`.

**Task 5.3.** Sort employees by **number of skills** (most first), then by `Name`. Show `"Name (N skills)"`.
*Expected first:* `Hayes Beasley (5 skills)`, then `Stewart Lott (5 skills)`.

**Task 5.4.** **Paging:** sort employees by `Name` and print **page 2** with a page size of 5 (`Skip` + `Take`). Then make it work for any `page` and `pageSize` variables.

---

## Challenge (optional)

Print a ranking of employees by salary (highest first) with their position, like `1. Hayes Beasley - 3727`.
*Hint:* use the `Select` overload that provides the **index**: `Select((e, i) => ...)`.

---

## Submission checklist

- [ ] Every task has its own `Print(...)` call with a clear title
- [ ] Both syntaxes used where `(both)` is written
- [ ] You can explain `OrderBy` vs `ThenBy` and why the second `OrderBy` breaks the first
- [ ] You can explain LINQ `Reverse()` vs `List<T>.Reverse()`
- [ ] You can explain what a negative, zero and positive value from `Compare` means
