using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem
{
    internal class CheckingAccount :  BankAccount , ITransferable
    {
        public decimal OverdraftLimit { get; private set; }

        public CheckingAccount(
            string accountNumber,
            string ownerName,
            decimal initialBalance,
            decimal overdraftLimit)
            : base(accountNumber, ownerName, initialBalance)
        {
            OverdraftLimit = overdraftLimit;
        }

        public override bool withdraw(decimal amount)
        {
            if (amount <= 0)
                return false;

            if (amount > balance + OverdraftLimit)
                return false;

            removeBalance(amount);

            return true;
        }

        public void Transfer(BankAccount destination, decimal amount)
        {
            if (destination == null)
            {
                Console.WriteLine("Invalid destination.");
                return;
            }

            if (withdraw(amount))
            {
                destination.deposite(amount);

                Console.WriteLine(
                    $"Transferred {amount} from {accountNumber}"
                );
            }
            else
            {
                Console.WriteLine("Transfer failed.");
            }
        }

        public override void DisplayInfo()
        {
            Console.WriteLine("\n--- Checking Account ---");

            base.DisplayInfo();

            Console.WriteLine(
                $"Overdraft Limit: {OverdraftLimit}"
            );
        }

        void ITransferable.Transfer(BankAccount destination, decimal amount)
        {
            Transfer(destination, amount);
        }
    }
}
