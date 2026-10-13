# LINQ Session 8: Tasks (`GroupBy`, `ToLookup`)

## How to use

1. Create a new Console project (.NET 5 or later).
2. Replace `Program.cs` with the **Starter Code** below. It already has the data, classes and helper, so **no shared project is needed**.
3. Solve each task in its `// TODO` area and display it with `Console.WriteLine` or `Print(...)`.
4. Use **method syntax** unless the task says **(both)**, in which case also write the **query syntax** version.

> The data has 25 employees in 4 departments: **Finance**, **IT**, **HR**, **Accounting**.

---

## Starter Code (copy into `Program.cs`)

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSession8Tasks
{
    // ---------- Model ----------
    public class Employee
    {
        public string Name { get; set; }
        public string Department { get; set; }   // Finance, IT, HR, Accounting
        public decimal Salary { get; set; }
        public List<string> Skills { get; set; } = new List<string>();

        public override string ToString() =>
            $"{Name.PadRight(20)}\t{Department.PadRight(10)}\t{Salary,5:N0}  [ {string.Join(", ", Skills)} ]";
    }

    // ---------- Data (25 employees) ----------
    public static class Repository
    {
        private static Employee E(string name, string dept, decimal salary, params string[] skills) =>
            new Employee { Name = name, Department = dept, Salary = salary, Skills = skills.ToList() };

        public static List<Employee> LoadEmployees() => new()
        {
            E("Cochran Cole",       "Finance",    1032, "Javascript", "C++", "NodeJS"),
            E("Jaclyn Wolfe",       "Finance",    1924, "C#", "Javascript", "ASP.NET"),
            E("Cochran Cole",       "IT",         1728, "ASP.NET", "Javascript"),
            E("Hester Evans",       "Finance",    1555, "CSS", "Javascript", "NodeJS"),
            E("Wallace Buck",       "IT",         3158, "Oracle", "HTML", "CSS"),
            E("Acevedo Wall",       "IT",         3437, "ASP.NET"),
            E("Jacqueline Pickett", "IT",         3700, "NodeJS"),
            E("Oconnor Espinoza",   "HR",         1556, "SQL Server", "C#", "HTML", "C++"),
            E("Allie Elliott",      "Accounting", 3154, "Oracle"),
            E("Elva Decker",        "HR",         3459, "ASP.NET", "Javascript", "C#"),
            E("Hayes Beasley",      "HR",         3727, "SQL Server", "C++", "NodeJS", "Oracle", "Javascript"),
            E("Florine Cervantes",  "Finance",    3387, "HTML"),
            E("Bullock Carney",     "Accounting", 2144, "C++", "CSS"),
            E("Carroll Cantu",      "Finance",    3432, "C++", "C#"),
            E("Debra Hogan",        "IT",         2491, "Oracle", "NodeJS", "HTML", "ASP.NET"),
            E("Winnie Mccall",      "IT",         2873, "CSS", "HTML"),
            E("Manuela Berger",     "IT",         1725, "Oracle"),
            E("Lakeisha Lowe",      "IT",         3143, "NodeJS", "Oracle", "CSS", "ASP.NET"),
            E("Stewart Lott",       "Finance",    1466, "ASP.NET", "C#", "Javascript", "SQL Server", "HTML"),
            E("Stafford Peck",      "IT",         3207, "C++", "ASP.NET"),
            E("Barron Bird",        "HR",         1512, "NodeJS", "Javascript", "ASP.NET"),
            E("Nona Brooks",        "IT",         1365, "HTML", "NodeJS"),
            E("Clara Reeves",       "IT",         2458, "ASP.NET", "NodeJS"),
            E("Karin Blanchard",    "IT",         3412, "C#", "SQL Server", "NodeJS", "CSS", "HTML"),
            E("Burris Morgan",      "Accounting", 3603, "C++", "CSS", "ASP.NET", "Oracle"),
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

    internal class Program
    {
        static void Main(string[] args)
        {
            // ---------- Sample data ----------
            var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            string[] fruits = { "apricot", "orange", "banana", "mango", "apple", "grape", "strawberry" };
            var names = new List<string> { "Sara", "sara", "Ali", "ALI", "Omar" };
            var employees = Repository.LoadEmployees();

            // ===================== PART 1: GroupBy basics =====================
            // TODO: Task 1.1 ... 1.5


            // ===================== PART 2: Aggregates per group =====================
            // TODO: Task 2.1 ... 2.6


            // ===================== PART 3: Overloads, composite keys, query syntax =====================
            // TODO: Task 3.1 ... 3.5


            // ===================== PART 4: ToLookup =====================
            // TODO: Task 4.1 ... 4.5


            // ===================== PART 5: Mixed =====================
            // TODO: Task 5.1 ... 5.4

            Console.ReadKey();
        }
    }
}
```

---

## Part 1: `GroupBy` basics

**Task 1.1.** Group `employees` by `Department` and print each department with its number of employees, like `IT : 12`.
*Expected:* `Finance : 6`, `IT : 12`, `HR : 4`, `Accounting : 3` (in this order).

**Task 1.2. (both)** Group `employees` by `Department` and print every group with all its employees (use `Print` with the key in the title).

**Task 1.3.** Group `employees` by the **first letter of their name**. Print the letter and how many employees it has.
*Expected:* 15 groups. The biggest is `C` with 4 employees.

**Task 1.4.** Group `numbers` into `"Even"` and `"Odd"`.
*Expected:* `Odd: 1 3 5 7 9` (first) and `Even: 2 4 6 8 10`.

**Task 1.5.** Group `fruits` by **length**.
*Expected:* `7: apricot`, `6: orange banana`, `5: mango apple grape`, `10: strawberry`.

---

## Part 2: Aggregates per group

**Task 2.1.** For each department print the **number of employees** and the **average salary**.
*Expected averages:* Finance `2132.67`, IT `2724.75`, HR `2563.50`, Accounting `2967.00`.

**Task 2.2.** For each department print the **highest-paid employee** (name and salary).
*Expected:* Finance `Carroll Cantu 3432`, IT `Jacqueline Pickett 3700`, HR `Hayes Beasley 3727`, Accounting `Burris Morgan 3603`.
*Hint:* `g.OrderByDescending(e => e.Salary).First()`.

**Task 2.3.** Print the **total salary** per department, sorted from highest to lowest.
*Expected:* IT `32697`, Finance `12796`, HR `10254`, Accounting `8901`.

**Task 2.4.** Which department has the **highest average salary**? Print only its name and average.
*Expected:* `Accounting`, `2967`.

**Task 2.5.** Group employees by **salary band**: `"Low"` (below 2,000), `"Medium"` (2,000 to 2,999), `"High"` (3,000 or more). Print the count per band.
*Expected:* Low `9`, Medium `4`, High `12`.

**Task 2.6.** Group employees by **number of skills** and print `"N skill(s): X employees"`, sorted by N.
*Expected:* `1: 5`, `2: 7`, `3: 6`, `4: 4`, `5: 3`.

---

## Part 3: Overloads, composite keys, query syntax

**Task 3.1.** Group by `Department`, but keep **only the names** in each group (use the element selector overload). Print `IT: Cochran Cole, Wallace Buck, ...`.

**Task 3.2.** Use the **result selector** overload `GroupBy(key, (key, group) => ...)` to produce objects with `Department`, `Count` and `TotalSalary`. Print them.

**Task 3.3. (query syntax)** Write a query that groups by department **and** continues with `into g` to produce `{ Department, Count }`, ordered by `Count` descending.
*Expected order:* IT (12), Finance (6), HR (4), Accounting (3).

**Task 3.4.** Group by **two keys**: `Department` and whether the salary is `>= 3000`. Print `Department / High / count`.
*Expected counts:*

| Department | High (>= 3000) | Not high |
|---|---|---|
| Finance | 2 | 4 |
| IT | 6 | 6 |
| HR | 2 | 2 |
| Accounting | 2 | 1 |

**Task 3.5.** Group the `names` list (`Sara, sara, Ali, ALI, Omar`) **ignoring case**.
*Expected:* 3 groups: `Sara` (2), `Ali` (2), `Omar` (1).
Then group it **without** the comparer. How many groups? (Expected: 5.)

---

## Part 4: `ToLookup`

**Task 4.1.** Create `var lookup = employees.ToLookup(e => e.Department);` and print all IT employees using `lookup["IT"]`.
*Expected:* 12 employees.

**Task 4.2.** Ask the lookup for the key `"Marketing"`. Does it throw? How many items does it return?
*Expected:* no exception, `0` items.

**Task 4.3.** Print `lookup.Count` and the result of `lookup.Contains("HR")`.
*Expected:* `4` and `true`.

**Task 4.4. (deferred vs immediate)**
1. Create `var grouped = employees.GroupBy(e => e.Department);` and `var lookup = employees.ToLookup(e => e.Department);`.
2. **Then** add a new IT employee to the `employees` list.
3. Print the IT count from `grouped` and from `lookup`.

*Expected:* `grouped` shows `13`, `lookup` shows `12`. Explain why in a comment.

**Task 4.5. (think first)** Try `employees.ToDictionary(e => e.Department)`. What happens? Explain why `ToLookup` does not have this problem.
*Expected:* it throws an `ArgumentException` (duplicate key).

---

## Part 5: Mixed

**Task 5.1.** Print departments ordered by employee count (highest first) as `"IT (12)"`.
*Expected order:* IT, Finance, HR, Accounting.

**Task 5.2.** For each department, print its **top 2 highest-paid** employees.
*Expected:*
- Finance: `Carroll Cantu 3432`, `Florine Cervantes 3387`
- IT: `Jacqueline Pickett 3700`, `Acevedo Wall 3437`
- HR: `Hayes Beasley 3727`, `Elva Decker 3459`
- Accounting: `Burris Morgan 3603`, `Allie Elliott 3154`

**Task 5.3.** Keep only the departments that have **more than 3 employees** (filter the groups with `Where`).
*Expected:* Finance, IT, HR.

**Task 5.4.** Find the **most used skills**. Flatten all skills (`SelectMany`), group them, and print `"Skill : count"` sorted by count (highest first).
*Expected top two:* `ASP.NET : 11`, `NodeJS : 10`. Then `Javascript : 8` and `HTML : 8` (tie). In total there are 68 skill entries.

---

## Challenge (optional)

Print a **department report**: for each department show the number of employees, the minimum, maximum and average salary, and the list of employee names sorted alphabetically. Format it as a small table or card per department.

---

## Submission checklist

- [ ] Every task has its own clear output
- [ ] Both syntaxes used where `(both)` is written
- [ ] You can explain what `IGrouping<TKey, TElement>` is and what `Key` holds
- [ ] You can explain deferred (`GroupBy`) vs immediate (`ToLookup`) execution
- [ ] You can explain why a missing key in a lookup returns an empty sequence
- [ ] You can explain why `ToDictionary` fails here and `ToLookup` does not
