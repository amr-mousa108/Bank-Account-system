using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
List<BankAccount> accounts = new List<BankAccount>();
Console.WriteLine("Enter details for Savings Account (HolderName,Balance)");
string holderName1 = Console.ReadLine();
decimal balance1 = decimal.Parse(Console.ReadLine());
SavingAccount savingAccount = new SavingAccount(holderName1, balance1);
accounts.Add(savingAccount);
Console.WriteLine();

Console.WriteLine("Enter details for Investement Account (HolderName,Balance)");
string holderName2 = Console.ReadLine();
decimal balance2 = decimal.Parse(Console.ReadLine());
InvestementAccount investementAccount = new InvestementAccount(holderName2, balance2);
accounts.Add(investementAccount);
Console.WriteLine();

Console.WriteLine("Enter details for Checking Account (HolderName,Balance)");
string holderName3 = Console.ReadLine();
decimal balance3 = decimal.Parse(Console.ReadLine());
CheckingAccount checkingAccount = new CheckingAccount(holderName3, balance3);
accounts.Add(checkingAccount);
Console.WriteLine();

Console.WriteLine("Enter details for Credit Account (HolderName,Balance,Credit Limit)");
string holderName4 = Console.ReadLine();
decimal balance4 = decimal.Parse(Console.ReadLine());
decimal creditLimit = decimal.Parse(Console.ReadLine());
CreditAccount creditAccount = new CreditAccount(holderName4, balance4, creditLimit);
accounts.Add(creditAccount);
Console.WriteLine();

foreach (var account in accounts)
{
    try
    {
        Console.WriteLine();
        account.CheckBalance();

        Console.WriteLine("Please enter the amount desposited:");
        decimal depositAmount = decimal.Parse(Console.ReadLine());
        account.Deposit(depositAmount);

        Console.WriteLine("Please enter the amount withdrawn:");
        decimal withdrawAmount = decimal.Parse(Console.ReadLine());
        account.Withdraw(withdrawAmount);
        if (account is IInterestEarning interestaccount)
        {
            Console.WriteLine("Calculating Interest between two dates");
            Console.WriteLine("Please insert StartDate:(dd/MM/yyyy)");
            DateTime startDate = DateTime.ParseExact(Console.ReadLine(), "(dd/MM/yyyy)", null);
            Console.WriteLine("Please insert StartDate:(dd/MM/yyyy)");
            DateTime endtDate = DateTime.ParseExact(Console.ReadLine(), "(dd/MM/yyyy)", null);

            if (interestaccount is SavingAccount)
            {
                SavingAccount savingaccount = (SavingAccount)interestaccount;
                decimal totalInterest = savingaccount.CalculateTotalInterest(startDate, endtDate);
                Console.WriteLine($"Total interest earned in{endtDate.Month - endtDate.Month} is :{totalInterest}");
            }
            else if (interestaccount is InvestementAccount)
            {
                InvestementAccount investement = (InvestementAccount)interestaccount;
                decimal totalInterest = investement.CalculateTotalInterest(startDate, endtDate);
                Console.WriteLine($"Total interest earned in{endtDate.Month - endtDate.Month} is :{totalInterest}");
                investement.ApplyInterst();
            }
        }
    }
    catch (Exception e)
    {
        throw new Exception($"Error :{e.Message}");
    }
}
public abstract class BankAccount
{
    public string AccountNumber { get; set; }
    public string AccountHolderName { get; set; }
    public decimal Balance { get; set; }
    public BankAccount( string accountHolderName, decimal balance)
    {
        AccountNumber = GenerateAccountNumber();
        AccountHolderName = accountHolderName;
        Balance = balance;
    }

    public virtual void Deposit(decimal amount)
    {
        if (Balance < 0)
            throw new ArgumentException("Deposit amount must be non negative");
        else
        {
            Balance += amount;
            Console.WriteLine($"Deposited E£{amount:F2} for {AccountNumber} and new balance is E£{Balance:F2}");
        }

    }
    public virtual void Withdraw(decimal amount)
    {
        if (Balance < 0)
        {
            throw new ArgumentException("Sorry ,Insufficient Funds for Withdraw");
        }
        if (amount < 0)
        {
            throw new Exception("The process of Withdraw for money must be non negative");
            
        }

        Balance -= amount;
        Console.WriteLine($"Withdraw E£{amount:F2} from {AccountNumber} .new Balance is E£{Balance:F2} ");

    }
    public string GenerateAccountNumber()
    {
        Random random = new Random();
        string accountType = GetType().Name.Substring(0, 3).ToUpper();
        string randomDigits = random.Next(1000000, 9999999).ToString();
        return accountType + randomDigits;
    }
    public virtual void CheckBalance()
    {
        Console.WriteLine($"Account Type: {GetType().Name}\nAccountHolder: {AccountHolderName}\n");
        Console.WriteLine($"Account {AccountNumber} , Balance is E£{Balance} ");
    }

}

public interface IInterestEarning
{
    int Balance { get; }

    void CalculateInterest();
    void ApplyInterst();
}

class SavingAccount : BankAccount, IInterestEarning
{
    private decimal MonthlyInterestRate = 0.3m;

    int IInterestEarning.Balance => throw new NotImplementedException();

    public SavingAccount(string accountHolderName, decimal balance) : base(accountHolderName, balance)
    {
    }
    public void CalculateInterest()
    {
        decimal interest = Balance * MonthlyInterestRate;
        Console.WriteLine($"Interest that has been earned on saving account {AccountNumber}:E£{interest:F2}. New Balance :E£{Balance + interest:F2} ");
    }
    public void ApplyInterst()
    {
        decimal interest = Balance * MonthlyInterestRate;
        Balance += interest;
        Console.WriteLine($"Applied Interest earned on saving account {AccountNumber}.E£{interest:F2}.New Balance :E£{Balance}");
    }
    public decimal CalculateTotalInterest(DateTime startDate, DateTime endDate)
    {
        int totalMnonths = (endDate.Year - endDate.Year) * 12 + endDate.Month - endDate.Month;
        return Balance + totalMnonths * 0.01m;



    }
}
    class InvestementAccount : BankAccount, IInterestEarning
    {
        private decimal MonthlyInterestRate = 0.4m;

        int IInterestEarning.Balance => throw new NotImplementedException();

        public InvestementAccount(string accountHolderName, decimal balance) : base(accountHolderName, balance)
        {
        }
        public void CalculateInterest()
        {
            decimal interest = Balance * MonthlyInterestRate;
            Console.WriteLine($"Interest that has been earned on Investement account {AccountNumber}:E£{interest:F2}. New Balance :E£{Balance + interest:F2} ");
        }
        public void ApplyInterst()
        {
            decimal interest = Balance * MonthlyInterestRate;
            Balance += interest;
            Console.WriteLine($"Applied Interest on Investement account {AccountNumber}.E£{interest:F2}.New Balance :E£{Balance}");
        }
        public decimal CalculateTotalInterest( DateTime startDate, DateTime endDate)
        {
            int totalMnonths = (endDate.Year - endDate.Year) * 12 + endDate.Month - endDate.Month;
            return Balance + totalMnonths * 0.01m;


        }
    }

    public class CheckingAccount : BankAccount
{
    public CheckingAccount(string accountHolderName, decimal balance) : base(accountHolderName, balance)
    {
    }
}
public class CreditAccount : BankAccount
{
    private decimal CreditLimit;
    public CreditAccount(string accountHolderName, decimal balance,decimal creditLimit ) : base(accountHolderName, balance)
    {
         CreditLimit = creditLimit;
    }
    public override void Withdraw(decimal amount)
    {
        if (Balance < 0 && amount < 0)
        {
            throw new ArgumentException("Sorry ,Insufficient Funds for Withdraw and The process of Withdraw for money must be non negative");
        }
        if (amount <= Balance)
        {
            Balance -= amount;
            Console.WriteLine($"Withdrawn E£{amount:F2} from {AccountNumber} .new Balance is {Balance:F2} ");
        }
        else if (amount <= Balance + CreditLimit)
        {
            CreditLimit -= (amount - Balance);   // creditlimit = creditlimit - (amount - balance)
            Balance = 0;
            Console.WriteLine($"Withdrawn E£{amount:F2} from {AccountNumber} .Credit :{CreditLimit:F2} ");

        }
        else
        {
            Console.WriteLine($"Error:Withdraw limit is reached for Account{AccountNumber}");
        }

    }
}

        public static class InterestExtensionMethod
        {

            public static decimal CalculateTotalInterest(this IInterestEarning account, DateTime startDate, DateTime endDate)
            {
                int totalMnonths = (endDate.Year - endDate.Year) * 12 + endDate.Month - endDate.Month;
                return account.Balance + totalMnonths * 0.01m;


            }
        }