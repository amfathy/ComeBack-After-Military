using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            var employees = Repository.LoadEmployees();
            var departments = Repository.LoadDepartments();
            var locations = Repository.LoadLocations();

            // ===================== PART 1: Join (inner join) =====================
            // TODO: Task 1.1 ... 1.5
            // task1 
            var res1_1 = employees.Join(
                departments, 
                (e => e.DepartmentId), 
                (d => d.Id) , 
                (e,d) => $"{e.FullName} [{d.Name}]");

            var res1_1_query =
                from emp in employees
                join dep in departments
                on emp.DepartmentId equals dep.Id
                select $"{emp.FullName} [{dep.Name}]", ;

            //task 3 
            var task1_3 = employees.Join(
                departments , 
                emp => emp.DepartmentId , 
                dep => dep.Id , 
                (emp , dep) => new { Employee = emp, Department = dep })
                .Where(x => x.Department.Name=="IT")
                .OrderByDescending(x => x.Employee.Salary)
                .Select(x => $"{x.Employee.FullName } : {x.Employee.Salary}");

            // ===================== PART 2: GroupJoin =====================
            // TODO: Task 2.1 ... 2.6
            // task 1 : 
            var task21 = departments.GroupJoin(
            employees,
            dept => dept.Id,
            emp => emp.DepartmentId,
            (dept, emps) => new
            {
                Department = dept.Name,
                Employees = emps.Select(emp => emp.FullName).ToList()
            });

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