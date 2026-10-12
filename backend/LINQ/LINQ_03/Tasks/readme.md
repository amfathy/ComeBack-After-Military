# LINQ Session 3: Tasks (`Select`, `SelectMany`, `Zip`)

## How to use

1. Create a new Console project (any .NET version 5 or later).
2. Replace `Program.cs` with the **Starter Code** below. It already contains the data and classes, so **no shared project is needed**.
3. Solve each task inside the matching `// TODO` section.
4. Use `Print(...)` to display your results.

Solve every task with **method syntax**. Where the task says **(both)**, also write the **query syntax** version.

---

## Starter Code (copy into `Program.cs`)

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSession3Tasks
{
    // ---------- Models ----------
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<string> Skills { get; set; } = new List<string>();

        public string FullName => $"{FirstName} {LastName}";

        public override string ToString() =>
            $"{Id}\t{FullName.PadRight(22)}\t{Email.PadRight(32)}\t[ {string.Join(", ", Skills)} ]";
    }

    public class EmployeeDto
    {
        public string Name { get; set; }
        public int TotalSkills { get; set; }

        public override string ToString() => $"{Name} => {TotalSkills} skills";
    }

    // ---------- Data ----------
    public static class Repository
    {
        private static Employee E(int id, string first, string last, params string[] skills) =>
            new Employee
            {
                Id = id,
                FirstName = first,
                LastName = last,
                Email = $"{last}.{first}@example.com",
                Skills = skills.ToList()
            };

        public static List<Employee> LoadEmployees() => new()
        {
            E(1001, "Cochran",    "Cole",      "ASP.NET", "CSS", "Oracle"),
            E(1002, "Jaclyn",     "Wolfe",     "ASP.NET", "SQL Server", "Javascript", "CSS", "HTML"),
            E(1003, "Warner",     "Jones",     "HTML", "Oracle", "SQL Server"),
            E(1004, "Hester",     "Evans",     "ASP.NET"),
            E(1005, "Wallace",    "Buck",      "C++", "ASP.NET", "C#", "NodeJS"),
            E(1006, "Acevedo",    "Wall",      "C++", "SQL Server", "CSS", "HTML"),
            E(1007, "Jacqueline", "Pickett",   "SQL Server", "CSS", "C++", "Javascript", "C#"),
            E(1008, "Oconnor",    "Espinoza",  "NodeJS", "Oracle"),
            E(1009, "Allie",      "Elliott",   "ASP.NET", "C#"),
            E(1010, "Elva",       "Decker",    "HTML", "C#", "SQL Server", "Javascript"),
            E(1011, "Hayes",      "Beasley",   "ASP.NET"),
            E(1012, "Florine",    "Cervantes", "C#", "Oracle", "Javascript"),
            E(1013, "Bullock",    "Carney",    "CSS", "NodeJS", "Javascript"),
            E(1014, "Carroll",    "Cantu",     "C#", "C++", "PHP"),
            E(1015, "Debra",      "Hogan",     "C++", "ASP.NET", "NodeJS", "Oracle"),
            E(1016, "Winnie",     "Mccall",    "SQL Server", "C++", "NodeJS"),
            E(1017, "Manuela",    "Berger",    "HTML", "C#"),
            E(1018, "Lakeisha",   "Lowe",      "C++", "CSS", "ASP.NET", "SQL Server"),
            E(1019, "Stewart",    "Lott",      "CSS", "HTML", "Javascript", "PHP"),
            E(1020, "Stafford",   "Peck",      "C++", "CSS", "ASP.NET"),
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
            // ---------- Sample data ----------
            var numbers   = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var names     = new List<string> { "Sara", "Ali", "sam", "Omar", "Salma", "Mona" };
            var employees = Repository.LoadEmployees();

            string[] sentences =
            {
                "LINQ makes data easy",
                "I love asp.net core",
                "select many flattens lists"
            };

            string[] products = { "Laptop", "Phone", "Tablet", "Monitor" };
            decimal[] prices  = { 25000m, 15000m, 9000m };
            int[] quantities  = { 2, 5, 3, 10 };

            string[] days   = { "Sat", "Sun", "Mon", "Tue" };
            string[] topics = { "C#", "LINQ", "EF Core" };

            // ===================== PART 1: SELECT =====================
            // TODO: Task 1.1 ... 1.6


            // ===================== PART 2: SELECT MANY =====================
            // TODO: Task 2.1 ... 2.5


            // ===================== PART 3: ZIP =====================
            // TODO: Task 3.1 ... 3.5


            // ===================== PART 4: MIXED =====================
            // TODO: Task 4.1 ... 4.3

            Console.ReadKey();
        }
    }
}
```

---

## Part 1: `Select`

**Task 1.1.** From `numbers`, get the **cube** of each number (`x * x * x`). **(both)**
*Expected start:* `1, 8, 27, 64 ...`

**Task 1.2.** From `names`, get the **length** of each name.
*Expected:* `4, 3, 3, 4, 5, 4`

**Task 1.3.** From `names`, produce strings in the form `Name -> NAME` (original and upper-case). **(both)**

**Task 1.4.** From `employees`, get only the **full names**.

**Task 1.5.** From `employees`, project into `EmployeeDto` (`Name`, `TotalSkills`) and print it.
*Expected first line:* `Cochran Cole => 3 skills`

**Task 1.6.** From `employees`, project into an **anonymous type** with `Id`, `Email`, and `HasCSharp` (`true` if the employee knows `"C#"`).

---

## Part 2: `SelectMany`

**Task 2.1.** From `sentences`, get **all words** in one flat sequence. **(both)**

**Task 2.2.** From `sentences`, get all **distinct** words, ignoring case (`"I"` and `"i"` count as one).
*Hint:* use `StringComparer.OrdinalIgnoreCase` with `Distinct`.

**Task 2.3.** From `employees`, get **all skills** of all employees (with duplicates) and print how many there are.

**Task 2.4.** From `employees`, get the **distinct skills**, sorted alphabetically. **(both)**
*Expected count:* `10` skills.

**Task 2.5.** From `employees`, get every `(FullName, Skill)` pair as a string such as `Cochran Cole - ASP.NET`.
*Hint:* use the `SelectMany` overload with a result selector, or two `from` clauses.

---

## Part 3: `Zip`

**Task 3.1.** Zip `products` and `prices` into `"Laptop costs 25000"`.
*What happens to `"Monitor"`? Explain in a comment.*

**Task 3.2.** Zip `products` with `quantities` and `prices`. Zip takes two sequences, so chain it twice or use tuples. Output: `Laptop x2 @ 25000`.

**Task 3.3.** Zip `days` with `topics` into a schedule such as `Sat: C#`.

**Task 3.4.** Zip the **first 5** employees with the **last 5** employees (use `employees[..5]` and `employees[^5..]`) and print `X is paired with Y`. **(both)**

**Task 3.5.** Zip `numbers` with the same list shifted by one (`numbers.Skip(1)`) to get the **difference** between each number and the next one.
*Expected:* `1, 1, 1, 1, 1, 1, 1, 1, 1` (nine results).

---

## Part 4: Mixed

**Task 4.1.** Get the **names of employees who know "SQL Server"**, then project them as `"<FullName> (<N> skills)"`.
*Operators:* `Where` + `Select`.

**Task 4.2.** For each employee, show `FullName` and their skills joined with commas, but only for employees with **more than 3 skills**.
*Operators:* `Where` + `Select` + `string.Join`.

**Task 4.3.** Get all **distinct skills used by employees whose first name starts with "S"**, sorted alphabetically.
*Operators:* `Where` + `SelectMany` + `Distinct` + `OrderBy`.

---

## Challenge (optional)

Using only `SelectMany` and `Select`, produce every combination of `days` and `topics` (a cross product), such as `Sat - C#`, `Sat - LINQ`, ..., `Tue - EF Core`. That makes 12 results.

*Then compare:* how is this different from `Zip` on the same two lists?

---

## Submission checklist

- [ ] Every task has its own `Print(...)` call with a clear title
- [ ] Both syntaxes used where `(both)` is written
- [ ] You can explain the difference between `Select` and `SelectMany`
- [ ] You can explain what `Zip` does when the sequences have different lengths
