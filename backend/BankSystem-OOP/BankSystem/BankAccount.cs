using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem
{
    internal abstract class BankAccount
    {
        public readonly string accountNumber; 
        public string ownerName {  get; set; }
        protected decimal balance;

        protected BankAccount( string accountNumber, string ownerName, decimal balance) {
            this.accountNumber = accountNumber;
            this.balance = balance;
            this.ownerName = ownerName;
        }

        public void deposite(decimal amount)
        {
            if (amount <= 0)
            {
                Console.WriteLine("Enter valid balance");
                return; 
            }
            balance += amount;
            Console.WriteLine($"General Deposit : {amount}");
        }

        protected void removeBalance ( decimal amount) {
            balance-= amount;
        }
         
        public abstract bool withdraw(decimal amount);

        public virtual void DisplayInfo()
        {
            Console.WriteLine($"Account number is : {accountNumber}");
            Console.WriteLine($"Owner name : {ownerName}");
            Console.WriteLine($"balance now is : {balance}");
        }
    }
}
