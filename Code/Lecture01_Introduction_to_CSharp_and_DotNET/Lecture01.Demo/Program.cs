using System.Globalization;

Console.WriteLine("Personal Expense Entry");
Console.WriteLine("----------------------");

Console.Write("Expense name: ");
string expenseName = Console.ReadLine();

Console.Write("Amount: ");
string amountText = Console.ReadLine();

decimal amount = decimal.Parse(amountText, CultureInfo.InvariantCulture);

Console.Write("Currency: ");
string currency = Console.ReadLine();

Console.WriteLine();
Console.WriteLine($"You spent {amount.ToString("F2", CultureInfo.InvariantCulture)} {currency} on {expenseName}.");
Console.WriteLine();
Console.WriteLine("This tiny program already has the basic shape of many backend features:");
Console.WriteLine("input -> processing -> output");


