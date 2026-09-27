using System;

namespace BankSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Customer customer =
             new Customer("Ahmed");


            SavingAccount savings =
                new SavingAccount(
                    "SAV-001",
                    "Ahmed",
                    5000m,
                    0.05m);


            CheckingAccount checking =
                new CheckingAccount(
                    "CHK-001",
                    "Ahmed",
                    1000m,
                    500m);


            customer.AddAccount(savings);
            customer.AddAccount(checking);


            savings.deposite(1000m);

            checking.deposite(
                500m);

            bool withdrawn =
                savings.withdraw(2000m);

            Console.WriteLine(
                $"Withdrawal successful: {withdrawn}"
            );

            Console.WriteLine(
                $"Savings Interest: {savings.calculateInterstRate()}"
            );

            savings.Transfer(
                checking,
                1000m);

            Console.WriteLine(
                "\n===== ALL ACCOUNTS ====="
            );

            customer.DisplayAccounts();
        }
    }
}