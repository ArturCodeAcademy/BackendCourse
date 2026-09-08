using System.Globalization;
using System.Text.Json;

Console.WriteLine("Lecture 04 - Files, JSON, Exceptions and LINQ");
Console.WriteLine("=================================================");
Console.WriteLine();

string dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "data");
string textFilePath = Path.Combine(dataDirectory, "notes.txt");
string jsonFilePath = Path.Combine(dataDirectory, "sample-expenses.json");

Console.WriteLine("1. Memory vs persistent storage");
List<string> inMemoryNames = new List<string>();
inMemoryNames.Add("Coffee");
Console.WriteLine("In memory count = " + inMemoryNames.Count);
Console.WriteLine("When the program stops, memory data disappears unless we save it.");
Console.WriteLine();

Console.WriteLine("2. Paths");
Console.WriteLine("Current directory: " + Directory.GetCurrentDirectory());
Console.WriteLine("Data directory: " + dataDirectory);
Console.WriteLine("Text file path: " + textFilePath);
Console.WriteLine("JSON file path: " + jsonFilePath);
Console.WriteLine();

Console.WriteLine("3. Creating a directory");
Directory.CreateDirectory(dataDirectory);
Console.WriteLine("Directory exists: " + Directory.Exists(dataDirectory));
Console.WriteLine();

Console.WriteLine("4. Writing and reading a text file");
File.WriteAllText(textFilePath, "Coffee\nTea\nLunch");
string fileText = File.ReadAllText(textFilePath);
Console.WriteLine(fileText);
Console.WriteLine();

Console.WriteLine("5. Reading all lines");
string[] lines = File.ReadAllLines(textFilePath);
foreach (string line in lines)
{
    Console.WriteLine("Line: " + line);
}
Console.WriteLine();

Console.WriteLine("6. JSON structure using dictionaries");
List<Dictionary<string, string>> expenses = new List<Dictionary<string, string>>();
Dictionary<string, string> coffee = new Dictionary<string, string>();
coffee["name"] = "Coffee";
coffee["amount"] = "4.50";
coffee["category"] = "Food";
expenses.Add(coffee);

Dictionary<string, string> bus = new Dictionary<string, string>();
bus["name"] = "Bus";
bus["amount"] = "2.80";
bus["category"] = "Transport";
expenses.Add(bus);
Console.WriteLine("Expense count = " + expenses.Count);
Console.WriteLine();

Console.WriteLine("7. Serializing to JSON");
JsonSerializerOptions options = new JsonSerializerOptions();
options.WriteIndented = true;
string json = JsonSerializer.Serialize(expenses, options);
Console.WriteLine(json);
Console.WriteLine();

Console.WriteLine("8. Saving JSON to a file");
File.WriteAllText(jsonFilePath, json);
Console.WriteLine("JSON saved: " + File.Exists(jsonFilePath));
Console.WriteLine();

Console.WriteLine("9. Loading JSON from a file");
string loadedJson = File.ReadAllText(jsonFilePath);
List<Dictionary<string, string>> loadedExpenses = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(loadedJson);
Console.WriteLine("Loaded count = " + loadedExpenses.Count);
Console.WriteLine();

Console.WriteLine("10. try/catch for invalid conversion");
try
{
    decimal value = decimal.Parse("not a number", CultureInfo.InvariantCulture);
    Console.WriteLine(value);
}
catch (FormatException ex)
{
    Console.WriteLine("Conversion failed: " + ex.Message);
}
Console.WriteLine();

Console.WriteLine("11. try/catch for missing files");
try
{
    string missing = File.ReadAllText(Path.Combine(dataDirectory, "missing.txt"));
    Console.WriteLine(missing);
}
catch (FileNotFoundException ex)
{
    Console.WriteLine("File not found: " + ex.FileName);
}
Console.WriteLine();

Console.WriteLine("12. try/catch/finally");
try
{
    Console.WriteLine("Trying to read the text file.");
    string text = File.ReadAllText(textFilePath);
    Console.WriteLine("Characters read: " + text.Length);
}
catch (IOException ex)
{
    Console.WriteLine("I/O problem: " + ex.Message);
}
finally
{
    Console.WriteLine("finally runs whether the try block succeeded or failed.");
}
Console.WriteLine();

Console.WriteLine("13. LINQ Where");
List<Dictionary<string, string>> foodExpenses = loadedExpenses
    .Where(expense => expense["category"] == "Food")
    .ToList();
Console.WriteLine("Food count = " + foodExpenses.Count);
Console.WriteLine();

Console.WriteLine("14. LINQ Select");
List<string> names = loadedExpenses
    .Select(expense => expense["name"])
    .ToList();
foreach (string name in names)
{
    Console.WriteLine("Name: " + name);
}
Console.WriteLine();

Console.WriteLine("15. LINQ FirstOrDefault");
Dictionary<string, string> firstTransport = loadedExpenses
    .FirstOrDefault(expense => expense["category"] == "Transport");
if (firstTransport != null)
{
    Console.WriteLine("First transport: " + firstTransport["name"]);
}
Console.WriteLine();

Console.WriteLine("16. LINQ Any");
bool hasLargeExpense = loadedExpenses.Any(expense => decimal.Parse(expense["amount"], CultureInfo.InvariantCulture) > 10m);
Console.WriteLine("Has large expense: " + hasLargeExpense);
Console.WriteLine();

Console.WriteLine("17. LINQ OrderBy");
List<Dictionary<string, string>> ordered = loadedExpenses
    .OrderBy(expense => expense["name"])
    .ToList();
foreach (Dictionary<string, string> expense in ordered)
{
    Console.WriteLine(expense["name"]);
}
Console.WriteLine();

Console.WriteLine("Key idea for Lecture 04:");
Console.WriteLine("Files keep data after restart, JSON gives structure, exceptions handle problems, LINQ searches data clearly.");
