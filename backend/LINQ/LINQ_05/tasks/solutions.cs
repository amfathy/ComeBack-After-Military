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

            //task 1 : 
            var first5Empl = employees.Take(5);

            //task 2
            var last5employee = employees.TakeLast(5); 


            //task 4 : 
             numbers.Where(x => x % 2 == 0).Print("All even numbers");
            // retrieve all even numbers 
             numbers.TakeWhile(x => x % 2 == 0).Print("Even numbers from the beginning");
            //retrieve untill find number not satisfied condition 



            // ===================== PART 2: Skip / SkipLast / SkipWhile =====================
            // TODO: Task 2.1 ... 2.5

            // Task 1 :
            employees.Skip(10).Print("Skip first 10 employees");

            // Task 2: 
            employees.SkipLast(10).Print("Skip last 10 employees");

            // Task 3:
            numbers.SkipWhile(x => x % 2 == 0)
                   .Print("Skip initial even numbers");

            // ===================== PART 3: Chunk =====================
            // TODO: Task 3.1 ... 3.3

            // Task 1: Split employees into chunks of 10.
            var chunksOfTen = employees.Chunk(10);

            // ===================== PART 4: Skip + Take together =====================
            // TODO: Task 4.1 ... 4.4

            // Task 4.2: Top 3 highest-paid employees.
            employees.OrderByDescending(e => e.Salary)
                     .Take(3)
                     .Print("Top 3 salaries");

            // ===================== PART 5: Pagination =====================
            // TODO: Task 5.1 ... 5.4

            Console.ReadKey();
        }
            public static IEnumerable<T> Paginate<T>(this IEnumerable<T> source,int page = 1,
            int size = 10)
                {
                    if (source == null)
                        throw new ArgumentNullException(nameof(source));

                    if (page < 1)
                        throw new ArgumentOutOfRangeException(nameof(page));

                    if (size < 1)
                        throw new ArgumentOutOfRangeException(nameof(size));

                    return source.Skip((page - 1) * size).Take(size);
                }
    }
}