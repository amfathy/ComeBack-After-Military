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
            var evens = new List<int> { 2, 4, 6, 8 };
            var names = new List<string> { "Sara", "Ali", "sam", "Omar", "Salma", "Mona" };
            var employees = Repository.LoadEmployees();

            // ===================== PART 1: Any =====================
            // TODO: Task 1.1 ... 1.6
            // task 1 : 
            var res1 = employees.Any(x => x.Name.StartsWith("jac", StringComparison.OrdinalIgnoreCase));

            //task 4 :
            var res4 = employees.Any(x => x.Skills.Contains("c#"));
            var res4_2 = employees.Any(x => x.Skills.Contains("Python"));

            //task 5 
            var res5 = employees.Where(x => x.Salary > 5000).Any(); 

            // ===================== PART 2: All =====================
            // TODO: Task 2.1 ... 2.6

            //task 1 : 
            var res2_1 = employees.All(x=> !string.IsNullOrWhiteSpace(x.Email));

            //task 2 : 
            var res2_2 = employees.All(x => x.Skills.Any());

            // ===================== PART 3: Any / All inside queries =====================
            // TODO: Task 3.1 ... 3.5

            // task 1 : 
            var res3_1 = employees.Where(x => x.Skills.Any(x => string.Equals(x , "nodejs" , StringComparison.OrdinalIgnoreCase)));

            // ===================== PART 4: Contains =====================
            // TODO: Task 4.1 ... 4.5


            // ===================== PART 5: Mixed =====================
            // TODO: Task 5.1 ... 5.4

            Console.ReadKey();
        }
    }
}