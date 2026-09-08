using System.Globalization;

string expenseName = "";
decimal expenseAmount = 0m;
bool hasExpense = false;
bool isRunning = true;

Console.WriteLine("Personal Expense Menu");
Console.WriteLine("=====================");

while (isRunning)
{
    Console.WriteLine();
    Console.WriteLine("1. Add expense");
    Console.WriteLine("2. Show current expense");
    Console.WriteLine("3. Exit");
    Console.Write("Choose command: ");

    string command = Console.ReadLine();
    Console.WriteLine();

    switch (command)
    {
        case "1":
            Console.Write("Expense name: ");
            string enteredName = Console.ReadLine();

            Console.Write("Amount: ");
            string amountText = Console.ReadLine();

            bool amountIsValid = decimal.TryParse(
                amountText,
                CultureInfo.InvariantCulture,
                out decimal enteredAmount);

            if (enteredName == "")
            {
                Console.WriteLine("Expense name cannot be empty.");
            }
            else if (!amountIsValid)
            {
                Console.WriteLine("Amount must be a number. Use dot, for example 4.50.");
            }
            else if (enteredAmount <= 0)
            {
                Console.WriteLine("Amount must be greater than zero.");
            }
            else
            {
                expenseName = enteredName;
                expenseAmount = enteredAmount;
                hasExpense = true;

                Console.WriteLine("Expense saved.");
            }
            break;

        case "2":
            if (hasExpense)
            {
                Console.WriteLine($"Current expense: {expenseName} - {expenseAmount.ToString("F2", CultureInfo.InvariantCulture)} EUR");
            }
            else
            {
                Console.WriteLine("No expense has been added yet.");
            }
            break;

        case "3":
            isRunning = false;
            Console.WriteLine("Goodbye.");
            break;

        default:
            Console.WriteLine("Unknown command. Choose 1, 2, or 3.");
            break;
    }
}
