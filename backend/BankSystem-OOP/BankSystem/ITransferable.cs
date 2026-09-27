using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem
{
    internal interface ITransferable
    {
        void Transfer (BankAccount destination , decimal amount);
    }
}
