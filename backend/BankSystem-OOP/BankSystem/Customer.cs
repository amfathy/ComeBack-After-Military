using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem
{
    class Customer
    {
        public Guid Id { get; }
        public string Name { get; private set; }

        private readonly List<BankAccount> accounts = new();

        public Customer(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public void AddAccount(BankAccount account)
        {
            if (account == null)
                return;

            accounts.Add(account);
        }

        public void DisplayAccounts()
        {
            foreach (BankAccount account in accounts)
            {
                account.DisplayInfo();
            }
        }
    }

}
