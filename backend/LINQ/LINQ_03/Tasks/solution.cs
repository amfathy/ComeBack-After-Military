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
            var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var names = new List<string> { "Sara", "Ali", "sam", "Omar", "Salma", "Mona" };
            var employees = Repository.LoadEmployees();

            string[] sentences =
            {
                "LINQ makes data easy",
                "I love asp.net core",
                "select many flattens lists"
            };

            string[] products = { "Laptop", "Phone", "Tablet", "Monitor" };
            decimal[] prices = { 25000m, 15000m, 9000m };
            int[] quantities = { 2, 5, 3, 10 };

            string[] days = { "Sat", "Sun", "Mon", "Tue" };
            string[] topics = { "C#", "LINQ", "EF Core" };

            // ===================== PART 1: SELECT =====================
            // TODO: Task 1.1 ... 1.6
            // task 1 :
            var res1 = numbers.Select(x => x * x * x);
            var res1_query =
                from n in numbers
                select n * n * n;

            //task2 : 
            var res2 = names.Select(name => name.Length);
            var res2_query =
                from n in names
                select n.Length;

            //task3 


            //task4

            //task5 

            //task6 

            // ===================== PART 2: SELECT MANY =====================
            // TODO: Task 2.1 ... 2.5

            // task1 :
            var res2_1 = sentences.SelectMany(x => x.Split(' '));
            var res2_1_query =
                from sen in sentences
                select sen.Split(' ');

            //task2 :
            var res2_2 = sentences.SelectMany(x => x.Split(' ').Distinct(StringComparer.OrdinalIgnoreCase));
            var res2_2_query = from sen in sentences
                               from s in sen.Split(' ').Distinct(StringComparer.OrdinalIgnoreCase)
                               select s;

            //task3 


            //task4

            //task5 

            //task6 


            // ===================== PART 3: ZIP =====================
            // TODO: Task 3.1 ... 3.5

            //task 1 : 
            var productPrices = products.Zip(prices, (prod, pric) => "${prod} costs {pric}");
            // monitor dosen't mergerd with other beacuse zip file stopped with shorter list 

            //task2 

            // ===================== PART 4: MIXED =====================
            // TODO: Task 4.1 ... 4.3

            //task 1 : 
            var sqlEmp = employees.Where(e => e.Skills.Contains("SQL Server"))
                .Select(x => $"{x.FullName} {x.Skills.Count}");


            Console.ReadKey();
        }
    }
}