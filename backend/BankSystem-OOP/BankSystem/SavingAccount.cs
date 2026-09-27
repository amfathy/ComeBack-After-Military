using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem
{
    internal class SavingAccount : BankAccount , ITransferable 
    {
        public decimal interestRate; 

        public SavingAccount(string accountNumber, string ownerName, decimal balance , decimal interestRate) : base (accountNumber , ownerName , balance)
        {
            this.interestRate = interestRate;   
        }

        public override bool withdraw(decimal amount)
        {
            if (amount <= 0 || amount > balance)
            {
                return false;
            }

            removeBalance(amount); 
            return true;    
        }

        public decimal calculateInterstRate()
        {
            return interestRate * balance;
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
            else Console.WriteLine("Transfer failed.");
        }

        void ITransferable.Transfer(BankAccount destination, decimal amount)
        {
            Transfer(destination, amount);
        }
    }
}
