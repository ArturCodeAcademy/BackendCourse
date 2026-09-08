using System.Globalization;

Console.WriteLine("Lecture 02 - Conditions, Loops and Methods");
Console.WriteLine("================================================");
Console.WriteLine();

Console.WriteLine("1. Boolean values");
bool isLoggedIn = false;
bool hasData = true;
Console.WriteLine("isLoggedIn = " + isLoggedIn);
Console.WriteLine("hasData = " + hasData);
Console.WriteLine();

Console.WriteLine("2. Comparison operators");
int balance = 100;
int withdrawal = 40;
Console.WriteLine("balance == withdrawal: " + (balance == withdrawal));
Console.WriteLine("balance != withdrawal: " + (balance != withdrawal));
Console.WriteLine("balance > withdrawal: " + (balance > withdrawal));
Console.WriteLine("balance < withdrawal: " + (balance < withdrawal));
Console.WriteLine("balance >= withdrawal: " + (balance >= withdrawal));
Console.WriteLine("balance <= withdrawal: " + (balance <= withdrawal));
Console.WriteLine();

Console.WriteLine("3. Logical operators");
bool hasEnoughMoney = balance >= withdrawal;
bool cardIsActive = true;
bool canWithdraw = hasEnoughMoney && cardIsActive;
bool needsHelp = !cardIsActive || withdrawal > 500;
Console.WriteLine("hasEnoughMoney && cardIsActive = " + canWithdraw);
Console.WriteLine("!cardIsActive || withdrawal > 500 = " + needsHelp);
Console.WriteLine();

Console.WriteLine("4. if, else if, else");
if (withdrawal <= 0)
{
    Console.WriteLine("Withdrawal must be greater than zero.");
}
else if (withdrawal > balance)
{
    Console.WriteLine("Not enough money.");
}
else
{
    Console.WriteLine("Withdrawal is allowed.");
}
Console.WriteLine();

Console.WriteLine("5. switch");
string role = "student";
switch (role)
{
    case "teacher":
        Console.WriteLine("Teacher menu");
        break;
    case "student":
        Console.WriteLine("Student menu");
        break;
    default:
        Console.WriteLine("Guest menu");
        break;
}
Console.WriteLine();

Console.WriteLine("6. while loop");
int countdown = 3;
while (countdown > 0)
{
    Console.WriteLine("countdown = " + countdown);
    countdown--;
}
Console.WriteLine();

Console.WriteLine("7. for loop");
for (int index = 0; index < 3; index++)
{
    Console.WriteLine("index = " + index);
}
Console.WriteLine();

Console.WriteLine("8. foreach loop");
string[] commands = { "add", "show", "exit" };
foreach (string command in commands)
{
    Console.WriteLine("command = " + command);
}
Console.WriteLine();

Console.WriteLine("9. Simple menu loop preview");
bool menuIsRunning = true;
int menuStep = 0;
while (menuIsRunning)
{
    menuStep++;
    Console.WriteLine("Menu iteration " + menuStep);

    if (menuStep >= 2)
    {
        menuIsRunning = false;
    }
}
Console.WriteLine();

Console.WriteLine("10. Methods with no parameters and no return value");
PrintSeparator();
Console.WriteLine();

Console.WriteLine("11. Method with parameters");
PrintExpense("Coffee", 4.50m);
PrintExpense("Tea", 3.20m);
Console.WriteLine();

Console.WriteLine("12. Method with return value");
decimal total = CalculateTotal(4.50m, 2);
Console.WriteLine("total = " + total.ToString("F2", CultureInfo.InvariantCulture));
Console.WriteLine();

Console.WriteLine("13. Method with validation result");
bool validAmount = IsPositiveAmount(10m);
bool invalidAmount = IsPositiveAmount(-5m);
Console.WriteLine("IsPositiveAmount(10) = " + validAmount);
Console.WriteLine("IsPositiveAmount(-5) = " + invalidAmount);
Console.WriteLine();

Console.WriteLine("14. Variable scope");
int outerNumber = 10;
if (outerNumber > 0)
{
    int innerNumber = 5;
    Console.WriteLine("outerNumber inside if = " + outerNumber);
    Console.WriteLine("innerNumber inside if = " + innerNumber);
}
Console.WriteLine("outerNumber outside if = " + outerNumber);
Console.WriteLine("innerNumber exists only inside the if block.");
Console.WriteLine();

Console.WriteLine("15. Reading and validating user input");
Console.Write("Enter amount: ");
string amountText = Console.ReadLine();

if (decimal.TryParse(amountText, CultureInfo.InvariantCulture, out decimal amount))
{
    if (IsPositiveAmount(amount))
    {
        Console.WriteLine("Accepted amount: " + amount.ToString("F2", CultureInfo.InvariantCulture));
    }
    else
    {
        Console.WriteLine("Amount must be greater than zero.");
    }
}
else
{
    Console.WriteLine("Amount must be a number.");
}

Console.WriteLine();
Console.WriteLine("Key idea for Lecture 02:");
Console.WriteLine("while running -> show menu -> read command -> use conditions -> call methods");

void PrintSeparator()
{
    Console.WriteLine("------------------------------");
}

void PrintExpense(string name, decimal amount)
{
    Console.WriteLine(name + ": " + amount.ToString("F2", CultureInfo.InvariantCulture) + " EUR");
}

decimal CalculateTotal(decimal price, int quantity)
{
    return price * quantity;
}

bool IsPositiveAmount(decimal amount)
{
    return amount > 0;
}
