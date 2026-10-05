using System;
// Use delegate as a condtion to filter
namespace Employee_Filtering
{
    internal class Program
    {
        class Employee
        {
            public string Name { get; set; }
            public decimal Salary { get; set; }
            public string Department { get; set; }
            public int ExperienceYears { get; set; }
            public bool NeedsTraining { get; set; }
        }

        delegate bool EmployeeFilter(Employee employee);

        static bool HighSalaryEmployee(Employee employee)
        {
            return employee.Salary > 50000;
        }

        static bool ITEmployee(Employee employee)
        {
            return employee.Department == "IT";
        }

        static bool ExperiencedEmployee(Employee employee)
        {
            return employee.ExperienceYears > 5;
        }

        static bool EmployeeNeedsTraining(Employee employee)
        {
            return employee.NeedsTraining;
        }

        static List<Employee> FilterEmployees(List<Employee> employees,EmployeeFilter filter)
        {
            List<Employee> result = new List<Employee>();

            foreach (Employee employee in employees)
            {
                if (filter(employee))
                {
                    result.Add(employee);
                }
            }

            return result;
        }

        static void Main(string[] args)
        {
            List<Employee> employees = new List<Employee>
            {
                new Employee
                {
                    Name = "Ahmed",
                    Salary = 60000,
                    Department = "IT",
                    ExperienceYears = 7,
                    NeedsTraining = false
                },

                new Employee
                {
                    Name = "Mohamed",
                    Salary = 40000,
                    Department = "HR",
                    ExperienceYears = 3,
                    NeedsTraining = true
                },

                new Employee
                {
                    Name = "Ali",
                    Salary = 55000,
                    Department = "IT",
                    ExperienceYears = 2,
                    NeedsTraining = true
                }
            };

            List<Employee> highSalaryEmployees =FilterEmployees(employees, HighSalaryEmployee);

            List<Employee> itEmployees = FilterEmployees(employees, ITEmployee);

            List<Employee> experiencedEmployees = FilterEmployees(employees, ExperiencedEmployee);

            List<Employee> trainingEmployees = FilterEmployees(employees, EmployeeNeedsTraining);
        }
    }
}