using System.Globalization;

Console.WriteLine("Lecture 03 - Collections and Data Structures");
Console.WriteLine("================================================");
Console.WriteLine();

Console.WriteLine("1. Problem: one variable stores one value");
string firstExpense = "Coffee";
string secondExpense = "Tea";
string thirdExpense = "Lunch";
Console.WriteLine(firstExpense);
Console.WriteLine(secondExpense);
Console.WriteLine(thirdExpense);
Console.WriteLine("This does not scale when we need 10, 100, or 1000 values.");
Console.WriteLine();

Console.WriteLine("2. Arrays have fixed size");
string[] fixedNames = new string[3];
fixedNames[0] = "Coffee";
fixedNames[1] = "Tea";
fixedNames[2] = "Lunch";
Console.WriteLine("Array length = " + fixedNames.Length);
Console.WriteLine("fixedNames[0] = " + fixedNames[0]);
Console.WriteLine();

Console.WriteLine("3. List<T> can grow");
List<string> names = new List<string>();
names.Add("Coffee");
names.Add("Tea");
names.Add("Lunch");
Console.WriteLine("List count = " + names.Count);
Console.WriteLine("names[0] = " + names[0]);
Console.WriteLine();

Console.WriteLine("4. Iterating a List<T> with for");
for (int index = 0; index < names.Count; index++)
{
    Console.WriteLine(index + ": " + names[index]);
}
Console.WriteLine();

Console.WriteLine("5. Iterating a List<T> with foreach");
foreach (string name in names)
{
    Console.WriteLine("Expense: " + name);
}
Console.WriteLine();

Console.WriteLine("6. Removing from a List<T>");
names.Remove("Tea");
Console.WriteLine("After Remove(\"Tea\"), count = " + names.Count);
names.RemoveAt(0);
Console.WriteLine("After RemoveAt(0), count = " + names.Count);
Console.WriteLine();

Console.WriteLine("7. Parallel lists: useful but risky");
List<string> expenseNames = new List<string>();
List<decimal> expenseAmounts = new List<decimal>();
expenseNames.Add("Coffee");
expenseAmounts.Add(4.50m);
expenseNames.Add("Lunch");
expenseAmounts.Add(12.00m);
for (int index = 0; index < expenseNames.Count; index++)
{
    Console.WriteLine(expenseNames[index] + " = " + expenseAmounts[index].ToString("F2", CultureInfo.InvariantCulture));
}
Console.WriteLine("If the lists get out of sync, the data becomes wrong. Classes will solve this later.");
Console.WriteLine();

Console.WriteLine("8. Finding by index");
int selectedIndex = 1;
Console.WriteLine("expenseNames[1] = " + expenseNames[selectedIndex]);
Console.WriteLine();

Console.WriteLine("9. Finding by value with a loop");
string search = "cof";
for (int index = 0; index < expenseNames.Count; index++)
{
    if (expenseNames[index].ToLowerInvariant().Contains(search))
    {
        Console.WriteLine("Found: " + expenseNames[index]);
    }
}
Console.WriteLine();

Console.WriteLine("10. Dictionary<TKey, TValue>");
Dictionary<string, decimal> pricesByName = new Dictionary<string, decimal>();
pricesByName["Coffee"] = 4.50m;
pricesByName["Tea"] = 3.20m;
pricesByName["Coffee"] = 4.75m;
Console.WriteLine("Coffee price = " + pricesByName["Coffee"].ToString("F2", CultureInfo.InvariantCulture));
Console.WriteLine("Dictionary count = " + pricesByName.Count);
Console.WriteLine();

Console.WriteLine("11. Safe dictionary lookup");
if (pricesByName.ContainsKey("Tea"))
{
    Console.WriteLine("Tea exists: " + pricesByName["Tea"].ToString("F2", CultureInfo.InvariantCulture));
}
if (pricesByName.TryGetValue("Water", out decimal waterPrice))
{
    Console.WriteLine("Water price = " + waterPrice);
}
else
{
    Console.WriteLine("Water was not found.");
}
Console.WriteLine();

Console.WriteLine("12. Iterating a dictionary");
foreach (KeyValuePair<string, decimal> pair in pricesByName)
{
    Console.WriteLine(pair.Key + " -> " + pair.Value.ToString("F2", CultureInfo.InvariantCulture));
}
Console.WriteLine();

Console.WriteLine("13. HashSet<T> keeps unique values");
HashSet<string> categories = new HashSet<string>();
categories.Add("Food");
categories.Add("Transport");
categories.Add("Food");
Console.WriteLine("Category count = " + categories.Count);
foreach (string category in categories)
{
    Console.WriteLine(category);
}
Console.WriteLine();

Console.WriteLine("14. enum gives names to fixed choices");
ExpenseCategory selectedCategory = ExpenseCategory.Food;
Console.WriteLine("Selected category = " + selectedCategory);
Console.WriteLine("As number = " + (int)selectedCategory);
Console.WriteLine();

Console.WriteLine("15. Generics: same collection idea, different type");
List<int> numbers = new List<int>();
List<string> words = new List<string>();
List<ExpenseCategory> categoryList = new List<ExpenseCategory>();
numbers.Add(10);
words.Add("Backend");
categoryList.Add(ExpenseCategory.Transport);
Console.WriteLine("numbers[0] = " + numbers[0]);
Console.WriteLine("words[0] = " + words[0]);
Console.WriteLine("categoryList[0] = " + categoryList[0]);
Console.WriteLine();

Console.WriteLine("16. Choosing a structure");
Console.WriteLine("Array: fixed number of values.");
Console.WriteLine("List<T>: changing number of values.");
Console.WriteLine("Dictionary<TKey,TValue>: lookup by key.");
Console.WriteLine("HashSet<T>: unique values.");
Console.WriteLine("enum: fixed named choices.");
Console.WriteLine();

Console.WriteLine("Key idea for Lecture 03:");
Console.WriteLine("A program becomes more useful when it can store and work with many values.");

enum ExpenseCategory
{
    Food = 1,
    Transport = 2,
    Study = 3,
    Other = 4
}
