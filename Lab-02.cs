using System;
using System.Collections.Generic;

class Account
{
    public string CardNumber { get; set; }
    public string OwnerName { get; set; }
    public string PinCode { get; set; }
    public decimal Balance { get; set; }

    public Account(string cardNumber, string ownerName, string pinCode, decimal balance)
    {
        CardNumber = cardNumber;
        OwnerName = ownerName;
        PinCode = pinCode;
        Balance = balance;
    }
}

delegate void ATMEventHandler(string message);

class AutomatedTellerMachine
{
    public event ATMEventHandler AuthenticationEvent, ViewBalanceEvent, WithdrawEvent, DepositEvent, TransferEvent;

    private decimal availableMoney;

    public AutomatedTellerMachine(decimal initialMoney)
    {
        availableMoney = initialMoney;
    }

    public bool Authenticate(Account account)
    {
        Console.WriteLine($"Enter PIN for card {account.CardNumber}:");
        string enteredPin = Console.ReadLine();
        bool isAuthenticated = enteredPin == account.PinCode;

        AuthenticationEvent?.Invoke(isAuthenticated
            ? $"Card {account.CardNumber} authenticated."
            : "Incorrect PIN.");

        return isAuthenticated;
    }

    public void ViewBalance(Account account)
    {
        ViewBalanceEvent?.Invoke($"Balance on card {account.CardNumber}: {account.Balance}");
    }

    public void Withdraw(Account account)
    {
        Console.WriteLine("Enter the amount to withdraw:");
        if (decimal.TryParse(Console.ReadLine(), out decimal amount))
        {
            if (account.Balance >= amount && availableMoney >= amount)
            {
                account.Balance -= amount;
                availableMoney -= amount;
                WithdrawEvent?.Invoke($"Withdrawn {amount} from card {account.CardNumber}");
            }
            else
            {
                WithdrawEvent?.Invoke($"Insufficient funds or limit reached for card {account.CardNumber}.");
            }
        }
        else
        {
            WithdrawEvent?.Invoke("Invalid withdrawal amount format.");
        }
    }

    public void Deposit(Account account)
    {
        Console.WriteLine("Enter the amount to deposit:");
        if (decimal.TryParse(Console.ReadLine(), out decimal amount))
        {
            account.Balance += amount;
            availableMoney += amount;
            DepositEvent?.Invoke($"Deposited {amount} to card {account.CardNumber}");
        }
        else
        {
            DepositEvent?.Invoke("Invalid deposit amount format.");
        }
    }

    public void Transfer(Account sourceAccount, Account destinationAccount)
    {
        Console.WriteLine($"Choose an account for funds transfer:");
        Console.WriteLine($"1. {sourceAccount.OwnerName}'s account ({sourceAccount.CardNumber})");
        Console.WriteLine($"2. {destinationAccount.OwnerName}'s account ({destinationAccount.CardNumber})");

        string choice = Console.ReadLine();

        Console.WriteLine("Enter the amount to transfer:");
        if (decimal.TryParse(Console.ReadLine(), out decimal amount))
        {
            switch (choice)
            {
                case "1":
                    PerformTransfer(sourceAccount, destinationAccount, amount);
                    break;
                case "2":
                    PerformTransfer(destinationAccount, sourceAccount, amount);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Transfer not completed.");
                    break;
            }
        }
        else
        {
            TransferEvent?.Invoke("Invalid transfer amount format.");
        }
    }

    private void PerformTransfer(Account source, Account destination, decimal amount)
    {
        if (source.Balance >= amount)
        {
            source.Balance -= amount;
            destination.Balance += amount;
            TransferEvent?.Invoke($"Transferred {amount} from card {source.CardNumber} to card {destination.CardNumber}");
        }
        else
        {
            TransferEvent?.Invoke($"Insufficient funds for transfer from card {source.CardNumber}.");
        }
    }
}

class Bank
{
    public string Name { get; set; }
    public List<AutomatedTellerMachine> ATMs { get; set; }
    public List<Account> Accounts { get; set; }

    public Bank(string name)
    {
        Name = name;
        ATMs = new List<AutomatedTellerMachine>();
        Accounts = new List<Account>();
    }
}

class Program
{
    static void Main()
    {
        Account account1 = new Account("1236567430123456", "Kostya Legenkiy", "1234", 1000.0m);
        Account account2 = new Account("8559543218657654", "Aliona Bushinska", "5678", 500.0m);

        AutomatedTellerMachine atm1 = new AutomatedTellerMachine(5000.0m);
        Bank bank = new Bank("MyBank");
        bank.ATMs.Add(atm1);
        bank.Accounts.Add(account1);
        bank.Accounts.Add(account2);

        atm1.AuthenticationEvent += message => Console.WriteLine($"[Authentication] {message}");
        atm1.ViewBalanceEvent += message => Console.WriteLine($"[View Balance] {message}");
        atm1.WithdrawEvent += message => Console.WriteLine($"[Withdrawal] {message}");
        atm1.DepositEvent += message => Console.WriteLine($"[Deposit] {message}");
        atm1.TransferEvent += message => Console.WriteLine($"[Transfer] {message}");

        Console.WriteLine("Available cards and their PINs:");
        foreach (var account in bank.Accounts)
        {
            Console.WriteLine($"{account.OwnerName}'s account ({account.CardNumber}) - PIN: {account.PinCode}");
        }

        Console.WriteLine("\nEnter card number:");
        string cardNumber = Console.ReadLine();
        Console.WriteLine("Enter PIN:");
        string pin = Console.ReadLine();

        Account authenticatedAccount = bank.Accounts.Find(acc => acc.CardNumber == cardNumber && acc.PinCode == pin);

        if (authenticatedAccount != null)
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\nChoose an option:");
                Console.WriteLine("1. View balance");
                Console.WriteLine("2. Withdraw funds");
                Console.WriteLine("3. Deposit funds");
                Console.WriteLine("4. Transfer funds to another card");
                Console.WriteLine("5. Exit");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        atm1.ViewBalance(authenticatedAccount);
                        break;
                    case "2":
                        atm1.Withdraw(authenticatedAccount);
                        break;
                    case "3":
                        atm1.Deposit(authenticatedAccount);
                        break;
                    case "4":
                        Console.WriteLine("Enter the recipient's card number:");
                        string destinationCardNumber = Console.ReadLine();
                        Account destinationAccount = bank.Accounts.Find(acc => acc.CardNumber == destinationCardNumber);

                        if (destinationAccount != null)
                        {
                            atm1.Transfer(authenticatedAccount, destinationAccount);
                        }
                        else
                        {
                            Console.WriteLine("Recipient's card not found. Please check the card number.");
                        }
                        break;
                    case "5":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
        else
        {
            Console.WriteLine("Card not found or incorrect PIN entered.");
        }
    }
}
