# LINQ Session 9: Tasks (`Join`, `GroupJoin`)

## How to use

1. Create a new Console project (.NET 5 or later).
2. Replace `Program.cs` with the **Starter Code** below. It already has the data, classes and helper, so **no shared project is needed**.
3. Solve each task in its `// TODO` area and print the results.
4. Use **method syntax** unless the task says **(both)**, in which case also write the **query syntax** version.

## The data

**Departments** (5)

| Id | Name | LocationId |
|---|---|---|
| 1 | IT | 1 |
| 2 | HR | 2 |
| 3 | Finance | 1 |
| 4 | Sales | 3 |
| 5 | Legal | 2 |

**Locations** (3): `1 Cairo`, `2 Alexandria`, `3 Giza`

**Employees** (15): 5 in IT, 3 in HR, 4 in Finance, 2 in Sales, **0 in Legal**, and **1 employee (Manuela Berger) with `DepartmentId = 99`**, a department that does not exist.

> These two "traps" (a department with no employees and an employee with no valid department) are on purpose. They show the difference between `Join`, `GroupJoin` and a left join.

---

## Starter Code (copy into `Program.cs`)

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSession9Tasks
{
    // ---------- Models ----------
    public class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int DepartmentId { get; set; }
        public decimal Salary { get; set; }

        public override string ToString() => $"{Id,2} | {FullName,-20} | Dept {DepartmentId,2} | {Salary,5:N0}";
    }

    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LocationId { get; set; }

        public override string ToString() => $"{Id} | {Name} | Location {LocationId}";
    }

    public class Location
    {
        public int Id { get; set; }
        public string City { get; set; }

        public override string ToString() => $"{Id} | {City}";
    }

    // ---------- Data ----------
    public static class Repository
    {
        public static List<Employee> LoadEmployees() => new()
        {
            new Employee { Id = 1,  FullName = "Cochran Cole",       DepartmentId = 3,  Salary = 1032 },
            new Employee { Id = 2,  FullName = "Jaclyn Wolfe",       DepartmentId = 3,  Salary = 1924 },
            new Employee { Id = 3,  FullName = "Wallace Buck",       DepartmentId = 1,  Salary = 3158 },
            new Employee { Id = 4,  FullName = "Acevedo Wall",       DepartmentId = 1,  Salary = 3437 },
            new Employee { Id = 5,  FullName = "Jacqueline Pickett", DepartmentId = 1,  Salary = 3700 },
            new Employee { Id = 6,  FullName = "Oconnor Espinoza",   DepartmentId = 2,  Salary = 1556 },
            new Employee { Id = 7,  FullName = "Allie Elliott",      DepartmentId = 4,  Salary = 3154 },
            new Employee { Id = 8,  FullName = "Elva Decker",        DepartmentId = 2,  Salary = 3459 },
            new Employee { Id = 9,  FullName = "Hayes Beasley",      DepartmentId = 2,  Salary = 3727 },
            new Employee { Id = 10, FullName = "Florine Cervantes",  DepartmentId = 3,  Salary = 3387 },
            new Employee { Id = 11, FullName = "Bullock Carney",     DepartmentId = 4,  Salary = 2144 },
            new Employee { Id = 12, FullName = "Carroll Cantu",      DepartmentId = 3,  Salary = 3432 },
            new Employee { Id = 13, FullName = "Debra Hogan",        DepartmentId = 1,  Salary = 2491 },
            new Employee { Id = 14, FullName = "Winnie Mccall",      DepartmentId = 1,  Salary = 2873 },
            new Employee { Id = 15, FullName = "Manuela Berger",     DepartmentId = 99, Salary = 1725 }, // no such department
        };

        public static List<Department> LoadDepartments() => new()
        {
            new Department { Id = 1, Name = "IT",      LocationId = 1 },
            new Department { Id = 2, Name = "HR",      LocationId = 2 },
            new Department { Id = 3, Name = "Finance", LocationId = 1 },
            new Department { Id = 4, Name = "Sales",   LocationId = 3 },
            new Department { Id = 5, Name = "Legal",   LocationId = 2 },   // no employees
        };

        public static List<Location> LoadLocations() => new()
        {
            new Location { Id = 1, City = "Cairo" },
            new Location { Id = 2, City = "Alexandria" },
            new Location { Id = 3, City = "Giza" },
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
                Console.WriteLine(item);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var employees   = Repository.LoadEmployees();
            var departments = Repository.LoadDepartments();
            var locations   = Repository.LoadLocations();

            // ===================== PART 1: Join (inner join) =====================
            // TODO: Task 1.1 ... 1.5


            // ===================== PART 2: GroupJoin =====================
            // TODO: Task 2.1 ... 2.6


            // ===================== PART 3: Left join =====================
            // TODO: Task 3.1 ... 3.4


            // ===================== PART 4: Joining three lists =====================
            // TODO: Task 4.1 ... 4.3


            // ===================== PART 5: Mixed =====================
            // TODO: Task 5.1 ... 5.4

            Console.ReadKey();
        }
    }
}
```

---

## Part 1: `Join` (inner join)

**Task 1.1. (both)** Show every employee with the name of their department, like `Cochran Cole [Finance]`.
*Expected:* **14** rows. The first is `Cochran Cole [Finance]`.

**Task 1.2.** Which employee is **missing** from the result of Task 1.1, and why? Answer in a comment, then prove it in code (find the employees whose `DepartmentId` is not in `departments`).
*Expected:* `Manuela Berger` (`DepartmentId = 99`).

**Task 1.3.** Show only the employees of the **IT** department, with their salary, sorted by salary (highest first).
*Expected:* 5 rows, the first is `Jacqueline Pickett 3700`.

**Task 1.4.** Is the department `Legal` in the result of Task 1.1? Show it using `Join` and explain in a comment.

**Task 1.5.** In **query syntax**, swap the two sides of `equals` (write `dept.Id equals emp.DepartmentId` in a query that starts with `from emp in employees`). What happens? Write the answer in a comment (do not leave the broken code uncommented).
*Expected:* it does not compile; the outer variable must be on the left.

---

## Part 2: `GroupJoin`

**Task 2.1. (both)** For each department, print the department name and the names of its employees. Use an anonymous object with `Department` and `Employees`.
*Expected:* **5** groups, including `Legal` with no employees. Manuela does not appear anywhere.

**Task 2.2.** For each department print the **number of employees**.
*Expected:* `IT 5`, `HR 3`, `Finance 4`, `Sales 2`, `Legal 0`.

**Task 2.3.** For each department print the **total** and the **average** salary. Make it work for `Legal` too (no exception!).
*Expected:*

| Department | Total | Average |
|---|---|---|
| IT | 15,659 | 3,131.80 |
| HR | 8,742 | 2,914.00 |
| Finance | 9,775 | 2,443.75 |
| Sales | 5,298 | 2,649.00 |
| Legal | 0 | 0 |

*Hint:* what does `Average` do on an empty sequence?

**Task 2.4.** Find the departments that have **no employees**.
*Expected:* `Legal`.

**Task 2.5.** Find the departments that have **more than 3 employees**.
*Expected:* `IT` and `Finance`.

**Task 2.6.** Compute "employees per department" a second way: `Join` followed by `GroupBy`. Compare it with Task 2.2. What is different? (Answer in a comment.)
*Expected:* `Legal` is missing.

---

## Part 3: Left join

**Task 3.1. (both)** Show **all 15 employees** with their department name. If there is no department, show `"No Department"`.
*Expected:* 15 rows; `Manuela Berger [No Department]`.
*Hint:* `GroupJoin` + `DefaultIfEmpty` + `SelectMany`.

**Task 3.2.** Using the result of Task 3.1, list only the employees **without** a valid department.
*Expected:* `Manuela Berger`.

**Task 3.3.** Show **all departments** with their employees as **flat rows** (`"IT - Wallace Buck"`). A department without employees must still appear as `"Legal - (no employees)"`.
*Expected:* **15** rows (14 employee rows + 1 for `Legal`).

**Task 3.4.** Print the number of rows from the inner join (Task 1.1) and from the left join (Task 3.1), and the difference.
*Expected:* `14`, `15`, difference `1`.

---

## Part 4: Joining three lists

**Task 4.1. (both)** Join `employees`, `departments` and `locations`. Show `Cochran Cole - Finance - Cairo`.
*Expected:* 14 rows.

**Task 4.2.** Count the employees **per city**.
*Expected:* `Cairo 9`, `Alexandria 3`, `Giza 2`.

**Task 4.3.** Show the employees working in **Cairo**, sorted by salary (highest first). Print the first 3.
*Expected:* `Jacqueline Pickett 3700`, `Acevedo Wall 3437`, `Carroll Cantu 3432`.

---

## Part 5: Mixed

**Task 5.1.** Show the **HR** employees sorted by salary descending, as `"Hayes Beasley - 3727"`.
*Expected order:* `Hayes Beasley`, `Elva Decker`, `Oconnor Espinoza`.

**Task 5.2.** For each department show its **highest-paid employee** (or `"-"` if none).
*Expected:* `IT: Jacqueline Pickett`, `HR: Hayes Beasley`, `Finance: Carroll Cantu`, `Sales: Allie Elliott`, `Legal: -`.

**Task 5.3.** Order the departments by **total salary** (highest first).
*Expected order:* `IT 15659`, `Finance 9775`, `HR 8742`, `Sales 5298`, `Legal 0`.

**Task 5.4.** Reproduce Task 1.1 **without** `Join`, using a cross join: `from e in employees from d in departments where e.DepartmentId == d.Id select ...`. Check that it gives the same 14 results. In a comment, explain why `Join` is the better choice for large lists.

---

## Challenge (optional)

Write a **full outer join** of employees and departments: every employee and every department, matched where possible.
*Expected:* **16** rows: 14 matched rows, `Manuela Berger` with no department, and `Legal` with no employees.
*Hint:* a left join `Union` a right join (swap the lists and handle the `null`s).

---

## Submission checklist

- [ ] Every task prints clear output
- [ ] Both syntaxes used where `(both)` is written
- [ ] You can explain the difference between `Join` and `GroupJoin`
- [ ] You can explain why `Legal` and `Manuela Berger` are treated differently by each operator
- [ ] You can write a left join and explain what `DefaultIfEmpty` does
- [ ] You can explain why `outerKey equals innerKey` order matters in query syntax
