# LINQ Session 6: Tasks (`Skip`, `Take`, `Chunk`, Pagination)

## How to use

1. Create a new Console project using **.NET 6 or later** (`Chunk` and `Take(Range)` need it).
2. Replace `Program.cs` with the **Starter Code** below. It already has the data, classes and helper, so **no shared project is needed**.
3. Solve each task in its `// TODO` area and display it with `Print(...)`.
4. Use **method syntax** for everything.

---

## Starter Code (copy into `Program.cs`)

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSession6Tasks
{
    // ---------- Model ----------
    public class Employee
    {
        public string EmployeeNo { get; set; }   // "2017-FI-1343"  (Year-Department-Sequence)
        public string Name { get; set; }
        public decimal Salary { get; set; }

        public override string ToString() =>
            $"{EmployeeNo.PadRight(13)}\t{Name.PadRight(20)}\t{Salary,6:N0}";
    }

    // ---------- Data (25 employees) ----------
    public static class Repository
    {
        private static Employee E(string no, string name, decimal salary) =>
            new Employee { EmployeeNo = no, Name = name, Salary = salary };

        public static List<Employee> LoadEmployees() => new()
        {
            E("2017-FI-1343", "Cochran Cole",        1032),   //  1
            E("2018-FI-1755", "Jaclyn Wolfe",        1924),   //  2
            E("2016-IT-1441", "Cochran Cole",        1728),   //  3
            E("2016-FI-3447", "Hester Evans",        1555),   //  4
            E("2014-IT-2956", "Wallace Buck",        3158),   //  5
            E("2020-IT-6341", "Acevedo Wall",        3437),   //  6
            E("2021-IT-4488", "Jacqueline Pickett",  3700),   //  7
            E("2017-HR-3248", "Oconnor Espinoza",    1556),   //  8
            E("2020-AC-9097", "Allie Elliott",       3154),   //  9
            E("2016-HR-3770", "Elva Decker",         3459),   // 10
            E("2020-HR-8809", "Hayes Beasley",       3727),   // 11
            E("2015-FI-3038", "Florine Cervantes",   3387),   // 12
            E("2017-AC-7409", "Bullock Carney",      2144),   // 13
            E("2021-FI-3725", "Carroll Cantu",       3432),   // 14
            E("2019-IT-9738", "Debra Hogan",         2491),   // 15
            E("2019-IT-7184", "Winnie Mccall",       2873),   // 16
            E("2015-IT-4664", "Manuela Berger",      1725),   // 17
            E("2017-IT-3609", "Lakeisha Lowe",       3143),   // 18
            E("2016-FI-2682", "Stewart Lott",        1466),   // 19
            E("2014-IT-3754", "Stafford Peck",       3207),   // 20
            E("2020-HR-7462", "Barron Bird",         1512),   // 21
            E("2015-IT-5171", "Nona Brooks",         1365),   // 22
            E("2014-IT-6267", "Clara Reeves",        2458),   // 23
            E("2018-IT-8208", "Karin Blanchard",     3412),   // 24
            E("2019-AC-8905", "Burris Morgan",       3603),   // 25
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

        // TODO (Part 5): write your Paginate extension method here
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            // ---------- Sample data ----------
            var numbers = new List<int> { 2, 4, 6, 7, 8, 10, 3, 12 };
            string[] days = { "Sat", "Sun", "Mon", "Tue", "Wed", "Thu", "Fri" };
            var employees = Repository.LoadEmployees();

            // ===================== PART 1: Take / TakeLast / TakeWhile =====================
            // TODO: Task 1.1 ... 1.5


            // ===================== PART 2: Skip / SkipLast / SkipWhile =====================
            // TODO: Task 2.1 ... 2.5


            // ===================== PART 3: Chunk =====================
            // TODO: Task 3.1 ... 3.3


            // ===================== PART 4: Skip + Take together =====================
            // TODO: Task 4.1 ... 4.4


            // ===================== PART 5: Pagination =====================
            // TODO: Task 5.1 ... 5.4

            Console.ReadKey();
        }
    }
}
```

---

## Part 1: `Take`, `TakeLast`, `TakeWhile`

**Task 1.1.** Get the **first 5** employees.
*Expected last one:* `Wallace Buck`

**Task 1.2.** Get the **last 5** employees.
*Expected first one:* `Barron Bird`

**Task 1.3.** From `days`, get the first 3 days, then try `Take(100)`. What happens? Explain in a comment.

**Task 1.4.** From `numbers`, compare:
`Where(x => x % 2 == 0)`, `TakeWhile(x => x % 2 == 0)`.
*Expected:* `2, 4, 6, 8, 10, 12` vs `2, 4, 6`. Why do they differ?

**Task 1.5.** Use `TakeWhile` on `employees` to get everyone **before** the first employee with salary `3700`.
*Expected:* 6 employees (the last is `Acevedo Wall`).

---

## Part 2: `Skip`, `SkipLast`, `SkipWhile`

**Task 2.1.** Skip the first 10 employees.
*Expected:* 15 employees, starting with `Hayes Beasley`.

**Task 2.2.** Skip the last 10 employees.
*Expected:* 15 employees, ending with `Debra Hogan`.

**Task 2.3.** From `numbers`, use `SkipWhile(x => x % 2 == 0)`.
*Expected:* `7, 8, 10, 3, 12`. Notice that `8`, `10` and `12` are returned even though they are even. Explain why.

**Task 2.4.** Use `SkipWhile` on `employees` to start from the first employee with salary `3700`.
*Expected:* 19 employees, starting with `Jacqueline Pickett`.

**Task 2.5.** Prove that `Take(10)` and `Skip(10)` together give back **all** the employees (use `Concat` and `SequenceEqual`).
Do the same for `TakeWhile` and `SkipWhile` with the rule from Task 1.5.

---

## Part 3: `Chunk`

**Task 3.1.** Split `employees` into chunks of **10** and print each chunk with its number.
*Expected sizes:* `10, 10, 5`

**Task 3.2.** Split `employees` into chunks of **4**. How many chunks are there, and how big is the last one?
*Expected:* 7 chunks, the last has 1 employee.

**Task 3.3.** For each chunk of 10, print the **average salary** (`"Chunk 1: 2470.3"`).
*Expected:* `2470.3`, `2759.5`, `2470`

---

## Part 4: `Skip` + `Take` together

**Task 4.1.** Skip 3 employees, then take 3.
*Expected:* `Hester Evans`, `Wallace Buck`, `Acevedo Wall`

**Task 4.2.** Get the **top 3 highest-paid** employees.
*Expected salaries:* `3727, 3700, 3603`

**Task 4.3.** Get the **2nd to 4th highest-paid** employees (skip the top one, take three).
*Expected salaries:* `3700, 3603, 3459`

**Task 4.4.** Use ranges instead: write `employees.Take(2..5)`, `employees.Take(..3)` and `employees.Take(^3..)`, and explain what each returns.
*Expected for `Take(2..5)`:* `Cochran Cole` (1728), `Hester Evans`, `Wallace Buck`.

---

## Part 5: Pagination

**Task 5.1.** Write this extension method in the `Extensions` class:

```csharp
public static IEnumerable<T> Paginate<T>(this IEnumerable<T> source, int page = 1, int size = 10)
```

It must:
- Replace `page <= 0` with `1` and `size <= 0` with `10`.
- Return the requested page using `Skip` and `Take`.
- Work for **any** type (no `where T : class`). Test it with `numbers` and with `employees`.

**Task 5.2.** Test your method:

| page | size | Expected result |
|---|---|---|
| 1 | 10 | employees 1 - 10 |
| 3 | 10 | employees 21 - 25 (5 items) |
| 2 | 7 | employees 8 - 14 |
| 5 | 10 | empty |
| 0 | 0 | same as page 1, size 10 |

Print a title like `"showing employees 21 - 25"`. Make the label work correctly even when the page is empty.

**Task 5.3.** Paginate employees **sorted by salary (highest first)**, page 2 with size 5.
*Expected salaries:* `3432, 3412, 3387, 3207, 3158`
*Question:* Why must you sort **before** paging?

**Task 5.4.** Write a method `GetTotalPages(int totalItems, int size)` and use it to print `"Page 2 of 3"`.
*Expected for 25 items and size 10:* 3 pages.
*Hint:* `Math.Ceiling` with a decimal or double division, not integer division.

---

## Challenge (optional)

Build a small console pager: show page 1, and let the user press **N** for next, **P** for previous and **Q** to quit. Show `Page X of Y` each time, and do not allow going before page 1 or after the last page.

---

## Submission checklist

- [ ] Every task has its own `Print(...)` call with a clear title
- [ ] You can explain `TakeWhile` vs `Where`, and `SkipWhile` vs `Where`
- [ ] You can explain why `Skip(100)` and `Take(100)` do not throw on a short list
- [ ] You can explain the pagination formula `Skip((page - 1) * size).Take(size)`
- [ ] You always sort before paginating
