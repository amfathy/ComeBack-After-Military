using System.Reflection.Metadata.Ecma335;

namespace Employee_Salary_Processing
{
    internal class Program
    {
        class Employee 
        {
            public string Name { get; set; }
            public decimal Salary { get; set; }


        }
        delegate decimal SalaryCalc(Employee employee);
       
        static decimal CalculateRegularSalary(Employee employee)
        {
            //any body 
            return 50;
        }
        static decimal CalculateSalaryWithBonus(Employee employee)
        {
            //any body 
            return 50;
        }
        static decimal CalculateSalaryWithOvertime(Employee employee)
        {
            //any body 
            return 50;
        }

        static decimal calculateFinalSalary(Employee employee, SalaryCalc salaryCalc)
        {
            return salaryCalc(employee);
        }
        


        static void Main(string[] args)
        {
            Employee E = new Employee
            {
                Name = "ahmed",
                Salary = 20000
            };

            decimal finalSalary = calculateFinalSalary(E, CalculateSalaryWithBonus);

            Console.WriteLine(finalSalary);

        }

       
    }
}
