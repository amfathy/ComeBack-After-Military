
using System;

namespace Payment_Processing
{
    class Payment
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
    }

    delegate bool PaymentProcessor(Payment payment);

    class PaymentHelper
    {
        public static bool ProcessCreditCard(Payment payment)
        {
            Console.WriteLine("Processed by Credit Card");
            return true;
        }

        public static bool ProcessPaypal(Payment payment)
        {
            Console.WriteLine("Processed by PayPal");
            return true;
        }

        public static bool PaymentBankTransfer(Payment payment)
        {
            Console.WriteLine("Processed by Bank Transfer");
            return true;
        }

        public static void PaymentProcess(
            Payment payment,
            PaymentProcessor processor)
        {
            bool flag = processor(payment);

            if (flag)
            {
                Console.WriteLine("Payment Succeeded");
            }
            else
            {
                Console.WriteLine("Payment Failed");
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Payment payment = new Payment
            {
                Id = 1,
                Amount = 1500m
            };

            PaymentHelper.PaymentProcess(
                payment,
                PaymentHelper.PaymentBankTransfer
            );

            PaymentHelper.PaymentProcess(
                payment,
                PaymentHelper.ProcessCreditCard
            );

            PaymentHelper.PaymentProcess(
                payment,
                PaymentHelper.ProcessPaypal
            );
        }
    }
}
