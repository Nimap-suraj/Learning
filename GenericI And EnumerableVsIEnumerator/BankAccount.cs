using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace March26
{
    public class InvalidFundException : Exception
    {

        public InvalidFundException(string message) : base(message) { }
    }

    internal class BankAccount
    {
        public double Balance { get; private set; }

        public BankAccount(double initialBalance)
        {
            Balance = initialBalance;
        }

        public void Withdraw(double amount)
        {
            if (amount > Balance)
            {
                throw new InvalidFundException("Insufficient funds for this withdrawal.");
            }
            Balance -= amount;
            Console.WriteLine($"Withdrawal successful! Remaining balance: {Balance:C}");
        }
    }
}
