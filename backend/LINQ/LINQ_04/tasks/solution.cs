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
            var names = new List<string> { "sara", "Ali", "sam", "Omar", "Salma", "mona" };
            var employees = Repository.LoadEmployees();

            // ===================== PART 1: OrderBy / OrderByDescending =====================
            // TODO: Task 1.1 ... 1.6
            // Task 1 : 
            var orderedFruits_1 = fruits.OrderBy(x => x);
            var orderedFruits_1_reversed = orderedFruits_1.Reverse();
            var orderedFruists_1_reversed_2 = fruits.OrderByDescending(f => f); 
            
            var orderedFruits_query =
                from f in fruits
                orderby f
                select f;

            var orderedFruits_reversed_query =
                from f in fruits
                orderby f descending
                select f;

            //task 2 : 
            var orderedFruitsByLength = fruits.OrderBy(f => f.Length);

            var orderedByFruitsLength_query =
                from f in fruits
                orderby f.Length
                select f;
            // ===================== PART 2: ThenBy / ThenByDescending =====================
            // TODO: Task 2.1 ... 2.4
            
            //task 1 : 
            var fruitsLengthAlpha = fruits
            .OrderBy(f => f.Length)
            .ThenBy(f => f);

            var fruitsLengthAlphaQuery =
                from f in fruits
                orderby f.Length, f
                select f;

            // ===================== PART 3: Reverse and Take =====================
            // TODO: Task 3.1 ... 3.4

            //task1 : 
            var reversedFruits = fruits.Reverse();

            // List<T>.Reverse() changes the existing list in place
            // and returns void. LINQ Reverse() returns a sequence
            // and does not modify the original collection.
            var numbersCopy = new List<int>(numbers);
            numbersCopy.Reverse();


            // ===================== PART 4: Custom comparer =====================
            // TODO: Task 4.1 ... 4.4



            // ===================== PART 5: Mixed =====================
            // TODO: Task 5.1 ... 5.4

            Console.ReadKey();
        }
    }
}