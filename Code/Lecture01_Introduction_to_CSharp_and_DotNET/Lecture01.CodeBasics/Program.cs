using System.Globalization;

Console.WriteLine("Lecture 01 - C# Code Basics");
Console.WriteLine("================================");
Console.WriteLine();

Console.WriteLine("This project is a slow walkthrough of basic C# code.");
Console.WriteLine("It is meant for reading, running, changing, and discussing line by line.");
Console.WriteLine();

Console.WriteLine("1. Variables: names for values");
string expenseName = "Coffee";
int quantity = 2;
decimal price = 4.50m;
bool isNecessary = false;
char currencyLetter = 'E';

Console.WriteLine("expenseName = " + expenseName);
Console.WriteLine("quantity = " + quantity);
Console.WriteLine("price = " + price.ToString("F2", CultureInfo.InvariantCulture));
Console.WriteLine("isNecessary = " + isNecessary);
Console.WriteLine("currencyLetter = " + currencyLetter);
Console.WriteLine();

Console.WriteLine("2. Common beginner types");
byte smallWholeNumber = 25;
int wholeNumber = 1000;
long largeWholeNumber = 5000000000L;
float approximateFloat = 3.14f;
double approximateDouble = 3.1415926535;
decimal moneyValue = 19.99m;
string textValue = "Backend";

Console.WriteLine("byte: " + smallWholeNumber);
Console.WriteLine("int: " + wholeNumber);
Console.WriteLine("long: " + largeWholeNumber);
Console.WriteLine("float: " + approximateFloat.ToString(CultureInfo.InvariantCulture));
Console.WriteLine("double: " + approximateDouble.ToString(CultureInfo.InvariantCulture));
Console.WriteLine("decimal: " + moneyValue.ToString("F2", CultureInfo.InvariantCulture));
Console.WriteLine("string: " + textValue);
Console.WriteLine();

Console.WriteLine("3. Constants: values that should not change");
const decimal VatRate = 0.25m;
const string DefaultCurrency = "EUR";
Console.WriteLine("VatRate = " + VatRate);
Console.WriteLine("DefaultCurrency = " + DefaultCurrency);
Console.WriteLine();

Console.WriteLine("4. Arithmetic operators");
decimal subtotal = quantity * price;
decimal vat = subtotal * VatRate;
decimal total = subtotal + vat;

Console.WriteLine("subtotal = quantity * price = " + subtotal.ToString("F2", CultureInfo.InvariantCulture));
Console.WriteLine("vat = subtotal * VatRate = " + vat.ToString("F2", CultureInfo.InvariantCulture));
Console.WriteLine("total = subtotal + vat = " + total.ToString("F2", CultureInfo.InvariantCulture));
Console.WriteLine("10 / 3 with int values = " + (10 / 3));
Console.WriteLine("10m / 3m with decimal values = " + (10m / 3m).ToString("F2", CultureInfo.InvariantCulture));
Console.WriteLine();

Console.WriteLine("5. Operator precedence");
int firstResult = 2 + 3 * 4;
int secondResult = (2 + 3) * 4;
Console.WriteLine("2 + 3 * 4 = " + firstResult);
Console.WriteLine("(2 + 3) * 4 = " + secondResult);
Console.WriteLine();

Console.WriteLine("6. Reading text from the console");
Console.Write("Enter product name: ");
string productName = Console.ReadLine();

Console.Write("Enter price using dot, for example 12.50: ");
string priceText = Console.ReadLine();

Console.Write("Enter quantity as a whole number: ");
string quantityText = Console.ReadLine();

Console.WriteLine("The console gave us text values:");
Console.WriteLine("productName = " + productName);
Console.WriteLine("priceText = " + priceText);
Console.WriteLine("quantityText = " + quantityText);
Console.WriteLine();

Console.WriteLine("7. Converting text to numbers with Parse");
decimal enteredPrice = decimal.Parse(priceText, CultureInfo.InvariantCulture);
int enteredQuantity = int.Parse(quantityText, CultureInfo.InvariantCulture);
decimal enteredTotal = enteredPrice * enteredQuantity;

Console.WriteLine("enteredPrice as decimal = " + enteredPrice.ToString("F2", CultureInfo.InvariantCulture));
Console.WriteLine("enteredQuantity as int = " + enteredQuantity);
Console.WriteLine("enteredTotal = " + enteredTotal.ToString("F2", CultureInfo.InvariantCulture));
Console.WriteLine();

Console.WriteLine("8. Safer conversion with TryParse");
Console.Write("Enter discount percent as a whole number: ");
string discountText = Console.ReadLine();

bool discountIsNumber = int.TryParse(discountText, CultureInfo.InvariantCulture, out int discountPercent);

if (discountIsNumber)
{
    Console.WriteLine("Converted discountPercent = " + discountPercent);
}
else
{
    discountPercent = 0;
    Console.WriteLine("The discount was not a number, so we use 0.");
}
Console.WriteLine();

Console.WriteLine("9. Conditions with comparison operators");
if (enteredPrice <= 0)
{
    Console.WriteLine("Price must be greater than zero.");
}
else if (enteredQuantity <= 0)
{
    Console.WriteLine("Quantity must be greater than zero.");
}
else if (discountPercent > 50)
{
    Console.WriteLine("Large discount. Ask a manager before saving.");
}
else
{
    Console.WriteLine("The entered data looks acceptable for now.");
}
Console.WriteLine();

Console.WriteLine("10. Boolean values and logical operators");
bool priceIsPositive = enteredPrice > 0;
bool quantityIsPositive = enteredQuantity > 0;
bool canSave = priceIsPositive && quantityIsPositive;
bool needsAttention = enteredTotal > 100 || discountPercent > 30;

Console.WriteLine("priceIsPositive = " + priceIsPositive);
Console.WriteLine("quantityIsPositive = " + quantityIsPositive);
Console.WriteLine("canSave = priceIsPositive && quantityIsPositive = " + canSave);
Console.WriteLine("needsAttention = enteredTotal > 100 || discountPercent > 30 = " + needsAttention);
Console.WriteLine();

Console.WriteLine("11. String concatenation and interpolation");
string sentence1 = "You entered " + enteredQuantity + " item(s) of " + productName + ".";
string sentence2 = $"You entered {enteredQuantity} item(s) of {productName}.";
Console.WriteLine(sentence1);
Console.WriteLine(sentence2);
Console.WriteLine();

Console.WriteLine("12. One-dimensional arrays");
string[] productNames = new string[3];
productNames[0] = "Coffee";
productNames[1] = "Tea";
productNames[2] = productName;

Console.WriteLine("productNames[0] = " + productNames[0]);
Console.WriteLine("productNames[1] = " + productNames[1]);
Console.WriteLine("productNames[2] = " + productNames[2]);
Console.WriteLine("Array length = " + productNames.Length);
Console.WriteLine();

Console.WriteLine("13. Numeric arrays and totals");
decimal[] prices = { 4.50m, 3.20m, enteredPrice };
decimal priceSum = prices[0] + prices[1] + prices[2];
Console.WriteLine("prices[0] = " + prices[0].ToString("F2", CultureInfo.InvariantCulture));
Console.WriteLine("prices[1] = " + prices[1].ToString("F2", CultureInfo.InvariantCulture));
Console.WriteLine("prices[2] = " + prices[2].ToString("F2", CultureInfo.InvariantCulture));
Console.WriteLine("priceSum = " + priceSum.ToString("F2", CultureInfo.InvariantCulture));
Console.WriteLine();

Console.WriteLine("14. for loop over an array");
for (int index = 0; index < productNames.Length; index++)
{
    Console.WriteLine("index " + index + " contains " + productNames[index]);
}
Console.WriteLine();

Console.WriteLine("15. foreach loop over an array");
foreach (string name in productNames)
{
    Console.WriteLine("Product: " + name);
}
Console.WriteLine();

Console.WriteLine("16. Two-dimensional rectangular array");
int[,] weeklySales = new int[2, 3];
weeklySales[0, 0] = 5;
weeklySales[0, 1] = 7;
weeklySales[0, 2] = 3;
weeklySales[1, 0] = 4;
weeklySales[1, 1] = 8;
weeklySales[1, 2] = 6;

Console.WriteLine("Rows = " + weeklySales.GetLength(0));
Console.WriteLine("Columns = " + weeklySales.GetLength(1));
Console.WriteLine("Day 1, item 1 sales = " + weeklySales[0, 0]);
Console.WriteLine("Day 2, item 3 sales = " + weeklySales[1, 2]);
Console.WriteLine();

Console.WriteLine("17. Jagged array: array of arrays");
string[][] projectIdeas = new string[2][];
projectIdeas[0] = new string[] { "ATM", "Expense Tracker" };
projectIdeas[1] = new string[] { "Veterinary Clinic", "Task Manager", "Library" };

Console.WriteLine("First beginner idea = " + projectIdeas[0][0]);
Console.WriteLine("Third business idea = " + projectIdeas[1][2]);
Console.WriteLine();

Console.WriteLine("18. Key idea for Lecture 01");
Console.WriteLine("input -> conversion -> variables -> operators -> conditions -> arrays -> output");
