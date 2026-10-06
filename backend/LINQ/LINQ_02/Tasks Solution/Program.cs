using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using static Task_solutions.Program;

namespace Task_solutions
{
    internal class Program
    {



        public class Employee
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Gender { get; set; }        // "male" / "female"
            public string Department { get; set; }    // "HR", "IT", "Finance", "Sales"
            public decimal Salary { get; set; }
            public int YearsOfExperience { get; set; }
            public bool NeedsTraining { get; set; }

            public override string ToString() =>
                $"{Id,2} | {FirstName,-8} {LastName,-10} | {Gender,-6} | {Department,-7} | {Salary,9:N0} | {YearsOfExperience,2} yrs | Training: {NeedsTraining}";
        }

        public static class Repository
        {
            public static List<Employee> LoadEmployees()
            {
                return new List<Employee>
            {
                new Employee { Id = 1,  FirstName = "Sara",    LastName = "Hassan",   Gender = "female", Department = "HR",      Salary = 45_000,  YearsOfExperience = 3,  NeedsTraining = false },
                new Employee { Id = 2,  FirstName = "sam",     LastName = "Adel",     Gender = "male",   Department = "IT",      Salary = 90_000,  YearsOfExperience = 7,  NeedsTraining = false },
                new Employee { Id = 3,  FirstName = "Ali",     LastName = "Mahmoud",  Gender = "male",   Department = "HR",      Salary = 30_000,  YearsOfExperience = 1,  NeedsTraining = true  },
                new Employee { Id = 4,  FirstName = "Salma",   LastName = "Youssef",  Gender = "female", Department = "Finance", Salary = 70_000,  YearsOfExperience = 6,  NeedsTraining = false },
                new Employee { Id = 5,  FirstName = "Omar",    LastName = "Khaled",   Gender = "male",   Department = "IT",      Salary = 120_000, YearsOfExperience = 10, NeedsTraining = false },
                new Employee { Id = 6,  FirstName = "Mona",    LastName = "Fathy",    Gender = "female", Department = "IT",      Salary = 55_000,  YearsOfExperience = 4,  NeedsTraining = true  },
                new Employee { Id = 7,  FirstName = "Hany",    LastName = "Samir",    Gender = "male",   Department = "Sales",   Salary = 38_000,  YearsOfExperience = 2,  NeedsTraining = true  },
                new Employee { Id = 8,  FirstName = "Nour",    LastName = "Ibrahim",  Gender = "female", Department = "Sales",   Salary = 62_000,  YearsOfExperience = 5,  NeedsTraining = false },
                new Employee { Id = 9,  FirstName = "Tarek",   LastName = "Zaki",     Gender = "male",   Department = "Finance", Salary = 310_000, YearsOfExperience = 15, NeedsTraining = false },
                new Employee { Id = 10, FirstName = "Salwa",   LastName = "Nabil",    Gender = "female", Department = "HR",      Salary = 52_000,  YearsOfExperience = 8,  NeedsTraining = false },
                new Employee { Id = 11, FirstName = "Karim",   LastName = "Ashraf",   Gender = "male",   Department = "IT",      Salary = 28_000,  YearsOfExperience = 0,  NeedsTraining = true  },
                new Employee { Id = 12, FirstName = "Laila",   LastName = "Mostafa",  Gender = "female", Department = "Finance", Salary = 85_000,  YearsOfExperience = 9,  NeedsTraining = false },
                new Employee { Id = 13, FirstName = "Adam",    LastName = "Reda",     Gender = "male",   Department = "Sales",   Salary = 47_000,  YearsOfExperience = 3,  NeedsTraining = false },
                new Employee { Id = 14, FirstName = "Yasmin",  LastName = "Gamal",    Gender = "female", Department = "IT",      Salary = 105_000, YearsOfExperience = 12, NeedsTraining = false },
                new Employee { Id = 15, FirstName = "Sherif",  LastName = "Lotfy",    Gender = "male",   Department = "HR",      Salary = 320_000, YearsOfExperience = 20, NeedsTraining = false },
                new Employee { Id = 16, FirstName = "Dina",    LastName = "Fouad",    Gender = "female", Department = "Sales",   Salary = 33_000,  YearsOfExperience = 1,  NeedsTraining = true  },
                new Employee { Id = 17, FirstName = "Mostafa", LastName = "Hamdy",    Gender = "male",   Department = "Finance", Salary = 60_000,  YearsOfExperience = 5,  NeedsTraining = false },
                new Employee { Id = 18, FirstName = "Sondos",  LastName = "Ramadan",  Gender = "female", Department = "IT",      Salary = 72_000,  YearsOfExperience = 6,  NeedsTraining = true  },
                new Employee { Id = 19, FirstName = "Ahmed",   LastName = "Said",     Gender = "male",   Department = "IT",      Salary = 150_000, YearsOfExperience = 11, NeedsTraining = false },
                new Employee { Id = 20, FirstName = "Heba",    LastName = "Salah",    Gender = "female", Department = "HR",      Salary = 41_000,  YearsOfExperience = 2,  NeedsTraining = true  },
            };
            }



            public static void print<T>(IEnumerable<T> List)
            {
                foreach (T i in List)
                {
                    Console.WriteLine(i);
                }
            }

            static void Main(string[] args)
            {
                //////////////////////////////////////////////////////////////////
                // Task 1 :  Even and odd
                /*
                 Write queries using Where to get:
                    -Odd numbers
                    -Numbers greater than 5
                    -Print each result.
                 */

                var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

                var Odd = numbers.Where(x => x % 2 == 0);
                var NumbGreaterThan5 = numbers.Where(x => x > 5);

                //print(NumbGreaterThan5);
                //print(Odd);

                //////////////////////////////////////////////////////////////////
                // Task 2 : 
                /*
                 1- Get names that start with "S" (should match both Sara and sam).
                 2- Get names longer than 4 characters.
                 3- Get names that contain the letter "a" (any case).
                 */

                var names = new List<string> { "Sara", "Ali", "sam", "Omar", "Salma", "Mona" };

                var NamesStartWithS = names.Where(n => n.ToLowerInvariant().StartsWith("s"));
                var NamesLongerThan4 = names.Where(n => n.Length > 4);
                var NamesContaintA = names.Where(n => n.ToLowerInvariant().Contains("a"));

                //print(NamesStartWithS);
                //print(NamesLongerThan4);
                //print(NamesContaintA); 

                //////////////////////////////////////////////////////////////////

                // Task 4 : Combining and chaining 
                /*
                    Create var males = employees.Where(...) first. Then, reusing males:
                    Males in "HR"
                    Males with salary > 100,000
                    Males in "IT" with a first name starting with "A"
                 */

                var males = Repository.LoadEmployees();

                var res1 = males.Where(e => e.Department.ToLowerInvariant() == "hr");

                var res2 = males.Where(e => e.Salary > 100000);

                var res3 = males.Where(e => e.Department.ToLowerInvariant() == "it").Where(e => e.FirstName.ToLowerInvariant().StartsWith("a"));
                //print(res3);

                //////////////////////////////////////////////////////////////////

                // Task 6
                /*
                  Write your own extension method without using Where:
                  public static IEnumerable<T> Filter<T>(this IEnumerable<T> source, Func<T, bool> rule)
                  Use foreach and yield return.
                  Test it with numbers and with employees.
                  Prove it gives the same result as Where.
                =================================================================================================
                
                this : make the function extenstion and call like  var a = numbers.Filter(x => x % 2 == 0); 
                Not call like  var a = Filter(numbers , x => x %2 == 0 ) 



                the solution : 

                var numbers2 = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
                public static IEnumerable<T> Filter1<T>(this IEnumerable<T> source, Func<T, bool> rule)
                {
                    foreach (var item in source)
                    {
                        if (rule(item))
                            yield return item;
                    }
                }


                public static IEnumerable<T> Filter2<T>(IEnumerable<T> source, Predicate<T> rule)
                {
                    foreach (var item in source)
                    {
                        if (rule(item))
                            yield return item;
                    }
                }

                var a = numbers.Filter(x => x % 2 == 0);
                var b = numbers.Where(x => x % 2 == 0);
                Console.WriteLine(a.SequenceEqual(b)); 
             */

                // task 11 

                /*  // 1) Numbers greater than 4
                        var a1 = numbers.Where(x => x > 4);
                        var a2 = Enumerable.Where(numbers, x => x > 4);
                        var a3 = 
                                from n in numbers 
                                 where n > 4 
                                 select n;

                    // 2) Employees in IT
                        var b1 = employees.Where(e => e.Department.ToLowerInvariant() == "it");
                        var b2 = Enumerable.Where(employees, e => e.Department.ToLowerInvariant() == "it");
                        var b3 = from n in numbers
                                 where n % 2 == 0 
                                 select n;

                    // 3) Female employees with salary > 40,000
                        var c1 = employees.Where(e => e.Gender == "female" && e.Salary > 40_000);
                        var c2 = Enumerable.Where(employees, e => e.Gender == "female" && e.Salary > 40_000);
                        var c3 = from e in employees
                                 where e.Gender == "female" && e.Salary > 40_000
                                 select e;

                    */



            }



        }
    }
}

