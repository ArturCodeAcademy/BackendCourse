from pathlib import Path
import textwrap
from reportlab.pdfgen import canvas
from reportlab.lib import colors
from reportlab.lib.units import inch
from reportlab.pdfbase.pdfmetrics import stringWidth

ROOT = Path(__file__).resolve().parents[2]
OUTPUT = ROOT / "PDF" / "Lecture05_Classes_and_Objects.pdf"
W, H = (13.333 * inch, 7.5 * inch)
BLUE = colors.HexColor("#1F4E79")
TEAL = colors.HexColor("#2A9D8F")
GOLD = colors.HexColor("#E9B949")
INK = colors.HexColor("#1F2933")
MUTED = colors.HexColor("#52606D")
LIGHT = colors.HexColor("#F5F7FA")
LINE = colors.HexColor("#D9E2EC")
CODE_BG = colors.HexColor("#17212B")

slides = [
    {"kind":"title", "title":"Lecture 05", "subtitle":"Classes and Objects", "body":["Basics of Internet Technologies 2", "From loose data to meaningful models"]},
    {"title":"Learning Objectives", "bullets":["Explain the difference between a class and an object.", "Create classes with fields, properties, constructors, and methods.", "Use access modifiers: public and private.", "Explain instance members and static members.", "Use object references and lists of objects.", "Override ToString for readable console output.", "Save and load a List<Expense> as JSON.", "Replace dictionary-based expenses with an Expense class."]},
    {"title":"Connection to Lecture 04", "bullets":["Lecture 04 stored one expense as Dictionary<string, string>.", "Dictionary keys such as name and amount are text and can be mistyped.", "The compiler cannot tell us that an expense should have an amount.", "Today we give the data a name, structure, and behavior: Expense."]},
    {"title":"Three-Hour Lesson Plan", "bullets":["20 min - problem with anonymous data; class and object vocabulary.", "30 min - fields, properties, object creation, and constructors.", "25 min - methods, ToString, and object references.", "20 min - access modifiers and encapsulation.", "20 min - static members and constructor overloads.", "35 min - convert the expense tracker from dictionaries to Expense objects.", "20 min - exercises, shared database increment, and homework briefing."]},
    {"title":"The Problem With a Dictionary", "code":"""Dictionary<string, string> expense = new Dictionary<string, string>();\nexpense[\"name\"] = \"Coffee\";\nexpense[\"amount\"] = \"4.50\";\nexpense[\"category\"] = \"Food\";""", "bullets":["Every key is just a string.", "expense[\"ammount\"] compiles but creates a different key.", "All values are strings, even the numeric amount.", "The structure is invisible in the type name."]},
    {"title":"Class and Object", "columns":[("Class", ["A blueprint or template", "Defines data and behavior", "Example: Expense", "Written once in code"]), ("Object", ["One value created from a class", "Has its own data", "Example: coffee expense", "Many objects can use one class"])]},
    {"title":"A Simple Expense Class", "code":"""public class Expense\n{\n    public string Name { get; set; }\n    public decimal Amount { get; set; }\n    public string Category { get; set; }\n}""", "bullets":["class defines a new C# type.", "Expense is now as real a type as string, int, or DateTime.", "The braces contain members belonging to that type."]},
    {"title":"Creating an Object", "code":"""Expense coffee = new Expense();\ncoffee.Name = \"Coffee\";\ncoffee.Amount = 4.50m;\ncoffee.Category = \"Food\";""", "bullets":["new creates an object in memory.", "coffee is a variable that refers to that object.", "The m suffix makes 4.50 a decimal literal.", "Each object can have different property values."]},
    {"title":"One Class, Many Objects", "code":"""Expense coffee = new Expense();\ncoffee.Name = \"Coffee\";\n\nExpense bus = new Expense();\nbus.Name = \"Bus ticket\";""", "bullets":["coffee and bus have the same type.", "They are separate objects with separate data.", "A class is reusable; an object is one concrete instance."]},
    {"title":"Members Inside a Class", "columns":[("Data", ["Fields", "Properties", "Read-only values"]), ("Behavior", ["Methods", "Constructors", "Overrides such as ToString"]) ]},
    {"title":"Fields: Internal Variables", "code":"""public class BankAccount\n{\n    private decimal balance;\n}""", "bullets":["A field is a variable stored inside each object.", "private means code outside BankAccount cannot access balance directly.", "Fields are useful for internal implementation details.", "The object keeps its own state between method calls."]},
    {"title":"Properties: Controlled Data Access", "code":"""public string Name { get; set; }\n\npublic int Id { get; private set; }\n\npublic string FullName => FirstName + \" \" + LastName;""", "bullets":["get reads a property value; set changes it.", "private set allows reading from outside but changes only inside the class.", "A get-only calculated property can derive a value from other properties.", "Properties are usually preferred for public data."]},
    {"title":"Auto-Property Sugar", "code":"""public string Category { get; set; }\n\n// C# creates hidden storage for us.\n// No manual field is needed here.""", "bullets":["This is an auto-property.", "It is shorter than writing a private field plus get and set methods.", "Use it when no extra validation or logic is needed yet."]},
    {"title":"Property With Validation", "code":"""private decimal amount;\n\npublic decimal Amount\n{\n    get { return amount; }\n    set\n    {\n        if (value > 0) amount = value;\n    }\n}""", "bullets":["value is the incoming value in a property setter.", "The property can reject invalid state.", "Later we will discuss richer validation strategies."]},
    {"title":"Constructor: Start an Object", "code":"""public Expense(int id, string name, decimal amount, string category)\n{\n    Id = id;\n    Name = name;\n    Amount = amount;\n    Category = category;\n}""", "bullets":["A constructor has the same name as its class.", "It runs automatically when new Expense(...) is used.", "Parameters provide the first values for the object.", "Constructors help avoid half-created objects."]},
    {"title":"Using the Constructor", "code":"""Expense coffee = new Expense(\n    1,\n    \"Coffee\",\n    4.50m,\n    \"Food\");""", "bullets":["Arguments are matched to constructor parameters by position.", "The object is ready immediately after creation.", "Use meaningful parameter names to make construction easier to read."]},
    {"title":"Default Constructor", "code":"""public Expense()\n{\n}\n\npublic Expense(int id, string name, decimal amount, string category)\n{\n    // Set initial values.\n}""", "bullets":["A parameterless constructor has no parameters.", "System.Text.Json can use public properties when loading JSON.", "For this beginner project we keep both constructors.", "Later, design choices around construction become more deliberate."]},
    {"title":"Methods Belong to Objects", "code":"""public bool IsLargeExpense()\n{\n    return Amount >= 100m;\n}\n\nif (coffee.IsLargeExpense())\n{\n    Console.WriteLine(\"Review this expense.\");\n}""", "bullets":["An instance method works with one object's data.", "Inside the method, Amount means this object's Amount.", "Methods give behavior a meaningful name."]},
    {"title":"void Method vs Returning a Value", "columns":[("void", ["Does an action", "Returns no result", "Example: Enroll(course)"]), ("Return type", ["Calculates an answer", "Returns a value", "Example: bool IsLargeExpense()"]) ]},
    {"title":"ToString for Console Output", "code":"""public override string ToString()\n{\n    return Id + \". \" + Name + \" - \"\n        + Amount.ToString(\"F2\") + \" EUR - \" + Category;\n}\n\nConsole.WriteLine(coffee);""", "bullets":["Every object has a ToString method inherited from object.", "override replaces the default type-name output.", "Console.WriteLine(coffee) calls coffee.ToString()."]},
    {"title":"Object Reference: Important Mental Model", "code":"""Student firstStudent = new Student(\"Ada\", \"Lovelace\", 21);\nStudent alias = firstStudent;\n\nalias.Age = 22;\nConsole.WriteLine(firstStudent.Age); // 22""", "bullets":["Class objects are reference values.", "The two variables point to the same Student object.", "Changing through alias changes the shared object.", "This differs from copying an int or decimal value."]},
    {"title":"Visual Model: Reference", "bullets":["firstStudent  ->  Student object { Name: Ada, Age: 22 }", "alias         ->  same Student object", "There is one object and two references to it.", "A new Student(...) expression would create another separate object."]},
    {"title":"List of Objects", "code":"""List<Expense> expenses = new List<Expense>();\nexpenses.Add(coffee);\nexpenses.Add(bus);\n\nforeach (Expense expense in expenses)\n{\n    Console.WriteLine(expense.Name);\n}""", "bullets":["List<Expense> tells the compiler what every item must be.", "Autocomplete now knows properties such as Name and Amount.", "This is safer and clearer than List<Dictionary<string, string>>."]},
    {"title":"LINQ Becomes Clearer", "code":"""decimal total = expenses.Sum(expense => expense.Amount);\n\nList<Expense> food = expenses\n    .Where(expense => expense.Category == \"Food\")\n    .ToList();""", "bullets":["The lambda receives an Expense object.", "expense.Amount is a decimal, not a text dictionary lookup.", "The domain model makes LINQ easier to read."]},
    {"title":"Access Modifiers", "columns":[("public", ["Accessible from other code", "The class contract", "Use for intended operations"]), ("private", ["Accessible only inside the class", "Protects implementation", "Use for internal fields and helpers"]) ]},
    {"title":"Encapsulation", "code":"""public class BankAccount\n{\n    private decimal balance;\n\n    public void Deposit(decimal amount)\n    {\n        if (amount > 0) balance += amount;\n    }\n}""", "bullets":["Outside code cannot write account.balance = -100m.", "The Deposit method decides which changes are valid.", "This keeps rules close to the data they protect."]},
    {"title":"Why Not Make Everything Public?", "bullets":["Public state can be changed from anywhere.", "Rules become scattered across Program.cs and other files.", "A later change can break many callers.", "Expose the smallest useful public surface.", "For simple teaching models, auto-properties are acceptable; learn the trade-off."]},
    {"title":"Static Members", "code":"""public class Student\n{\n    public static int CreatedCount { get; private set; }\n\n    public Student(...)\n    {\n        CreatedCount++;\n    }\n}\n\nConsole.WriteLine(Student.CreatedCount);""", "bullets":["static belongs to the class itself, not to one object.", "Student.CreatedCount is shared by all Student objects.", "Use the class name to access a static member."]},
    {"title":"Instance vs Static", "columns":[("Instance", ["Belongs to one object", "coffee.Amount", "Each object has its own value"]), ("Static", ["Belongs to the class", "Student.CreatedCount", "One shared value"]) ]},
    {"title":"Constructor Overloading", "code":"""public Book(string title)\n    : this(title, \"Unknown\", 0)\n{\n}\n\npublic Book(string title, string author, int year)\n{\n    Title = title;\n    Author = author;\n    Year = year;\n}""", "bullets":["Overloading means using the same constructor name with different parameter lists.", "this(...) calls another constructor in the same class.", "It prevents repeating initialization code."]},
    {"title":"readonly Field", "code":"""private readonly List<string> courses =\n    new List<string>();""", "bullets":["readonly means the field reference is assigned only during declaration or construction.", "The list object can still receive items with courses.Add(...).", "It prevents replacing the list by mistake later."]},
    {"title":"Calculated Property", "code":"""public string FullName\n{\n    get { return FirstName + \" \" + LastName; }\n}\n\n// Short form:\npublic string FullName => FirstName + \" \" + LastName;""", "bullets":["A calculated property does not store another copy of the full name.", "It computes the current result when read.", "The short expression-bodied syntax is C# sugar for a simple getter."]},
    {"title":"Class Design Checklist", "bullets":["Choose a noun for the class: Expense, Student, Book, User.", "Give the object only data that belongs to that concept.", "Use meaningful types: decimal for money, DateTime for dates.", "Add a constructor when an object needs required starting data.", "Put behavior near the data it works with.", "Keep the public API small and understandable."]},
    {"title":"From Dictionary to Expense", "columns":[("Before", ["Dictionary<string, string>", "expense[\"amount\"]", "Every value is text", "Keys can be mistyped"]), ("After", ["Expense", "expense.Amount", "Amount is decimal", "Compiler knows the model"]) ]},
    {"title":"Expense Model Used in the Demo", "code":"""public class Expense\n{\n    public int Id { get; set; }\n    public string Name { get; set; }\n    public decimal Amount { get; set; }\n    public string Category { get; set; }\n    public DateTime CreatedAt { get; set; }\n}""", "bullets":["Id identifies an expense in the console list.", "Name and Category are descriptive text.", "Amount uses decimal because this is money.", "CreatedAt records when the item was added."]},
    {"title":"JSON Still Works With Classes", "code":"""List<Expense> loaded =\n    JsonSerializer.Deserialize<List<Expense>>(json);\n\nstring json = JsonSerializer.Serialize(expenses,\n    new JsonSerializerOptions { WriteIndented = true });""", "bullets":["JSON property names are matched with public properties.", "The file becomes easier to understand because Expense is explicit.", "The persistence idea from Lecture 04 stays the same."]},
    {"title":"Live Coding Sequence", "bullets":["1. Create the Expense class with five properties.", "2. Change List<Dictionary<string, string>> to List<Expense>.", "3. Change AddExpense to call new Expense(...).", "4. Replace expense[\"name\"] with expense.Name.", "5. Add ToString and simplify console output.", "6. Run, save JSON, restart, and verify loading."]},
    {"title":"Common Beginner Errors", "bullets":["Using Expense.Name instead of coffee.Name for an instance property.", "Forgetting new before Expense(...).", "Writing a constructor with a return type such as void.", "Expecting a class variable copy to create a new object.", "Making a field public only to avoid writing a method or property.", "Using double for money instead of decimal."]},
    {"title":"Guided Exercise A", "bullets":["Create a Book class: Title, Author, Year.", "Add a constructor that receives all three values.", "Create two Book objects.", "Override ToString to show a readable sentence.", "Put both books into List<Book> and print them with foreach."]},
    {"title":"Guided Exercise B", "bullets":["Create a Student class with FirstName, LastName, and Age.", "Add a FullName calculated property.", "Add a method Introduce that returns a string.", "Create a static CreatedCount member.", "Explain which data belongs to one object and which belongs to the class."]},
    {"title":"Shared Database-Course Increment", "bullets":["Use the same project domain as the backend application.", "Design an Expense table with a primary key and five columns.", "Choose SQL types for id, name, amount, category, and created date.", "Write a short note explaining why Amount is numeric, not text.", "The backend still saves JSON today; this schema prepares the database milestone."]},
    {"title":"Homework: Project Evolution Portfolio", "bullets":["Upgrade Milestone 01 from dictionary-based data to classes.", "Keep the menu operations: add, list, search, filter, delete, and summary.", "Persist List<YourEntity> in JSON.", "Submit the full solution and a README explaining what changed in Lecture 05.", "This is one step in Assignment 1, worth 30 percent of the course grade."]},
    {"title":"Exit Ticket", "bullets":["What is the difference between a class and an object?", "Why is decimal appropriate for an expense amount?", "When would private be better than public?", "What does new Expense(...) do?", "What will ToString change in the console application?"]},
    {"kind":"title", "title":"Next Time", "subtitle":"Interfaces and Abstraction", "body":["We will compare different implementations behind one common contract.", "The Expense class becomes part of a larger design."]},
]

def wrap(text, font, size, width):
    words = text.split()
    lines, current = [], ""
    for word in words:
        candidate = word if not current else current + " " + word
        if stringWidth(candidate, font, size) <= width:
            current = candidate
        else:
            if current: lines.append(current)
            current = word
    if current: lines.append(current)
    return lines

def draw_header(c, title, number):
    c.setFillColor(BLUE)
    c.rect(0, H - 0.66*inch, W, 0.66*inch, fill=1, stroke=0)
    c.setFillColor(colors.white)
    c.setFont("Helvetica-Bold", 25)
    c.drawString(0.52*inch, H - 0.43*inch, title)
    c.setStrokeColor(TEAL)
    c.setLineWidth(4)
    c.line(0.52*inch, H - 0.78*inch, 2.15*inch, H - 0.78*inch)
    c.setFillColor(MUTED)
    c.setFont("Helvetica", 9)
    c.drawRightString(W - 0.5*inch, 0.26*inch, f"Lecture 05 | {number:02d}")

def draw_bullets(c, bullets, x, y, width, size=18, leading=0.31*inch):
    cursor = y
    for bullet in bullets:
        lines = wrap(bullet, "Helvetica", size, width - 0.32*inch)
        c.setFillColor(TEAL)
        c.circle(x + 0.08*inch, cursor + 0.055*inch, 0.045*inch, fill=1, stroke=0)
        c.setFillColor(INK)
        c.setFont("Helvetica", size)
        for index, line in enumerate(lines):
            c.drawString(x + 0.24*inch, cursor - index*leading, line)
        cursor -= max(leading, len(lines)*leading) + 0.10*inch
    return cursor

def draw_code(c, code, x, y, width, height):
    c.setFillColor(CODE_BG)
    c.roundRect(x, y-height, width, height, 8, fill=1, stroke=0)
    c.setFillColor(colors.HexColor("#E6EDF3"))
    c.setFont("Courier", 13.2)
    cursor = y - 0.30*inch
    for raw in code.splitlines():
        lines = textwrap.wrap(raw, width=max(20, int(width / 7.95)), replace_whitespace=False, drop_whitespace=False) or [""]
        for line in lines:
            c.drawString(x + 0.22*inch, cursor, line)
            cursor -= 0.20*inch

def draw_title(c, slide):
    c.setFillColor(BLUE)
    c.rect(0, 0, W, H, fill=1, stroke=0)
    c.setFillColor(TEAL)
    c.rect(0, 0, 0.20*inch, H, fill=1, stroke=0)
    c.setFillColor(GOLD)
    c.rect(0.58*inch, H - 1.15*inch, 1.7*inch, 0.08*inch, fill=1, stroke=0)
    c.setFillColor(colors.white)
    c.setFont("Helvetica-Bold", 38)
    c.drawString(0.58*inch, H - 2.15*inch, slide["title"])
    c.setFont("Helvetica", 25)
    c.drawString(0.58*inch, H - 2.72*inch, slide["subtitle"])
    c.setStrokeColor(colors.HexColor("#5EA9A0"))
    c.setLineWidth(1)
    c.line(0.58*inch, H - 3.1*inch, W - 0.65*inch, H - 3.1*inch)
    c.setFont("Helvetica", 17)
    cursor = H - 3.65*inch
    for item in slide.get("body", []):
        c.setFillColor(colors.HexColor("#DDEAF4"))
        c.drawString(0.62*inch, cursor, item)
        cursor -= 0.34*inch
    c.setFillColor(colors.HexColor("#B8D8D4"))
    c.setFont("Helvetica", 11)
    c.drawString(0.62*inch, 0.46*inch, "Basics of Internet Technologies 2")

def draw_slide(c, slide, number):
    if slide.get("kind") == "title":
        draw_title(c, slide)
        return
    draw_header(c, slide["title"], number)
    if "columns" in slide:
        columns = slide["columns"]
        gap = 0.30*inch
        card_w = (W - 1.04*inch - gap) / len(columns)
        x = 0.52*inch
        for heading, items in columns:
            c.setFillColor(LIGHT)
            c.roundRect(x, 1.10*inch, card_w, H - 2.15*inch, 8, fill=1, stroke=0)
            c.setStrokeColor(LINE)
            c.roundRect(x, 1.10*inch, card_w, H - 2.15*inch, 8, fill=0, stroke=1)
            c.setFillColor(BLUE)
            c.setFont("Helvetica-Bold", 22)
            c.drawString(x + 0.25*inch, H - 1.25*inch, heading)
            draw_bullets(c, items, x + 0.18*inch, H - 1.78*inch, card_w - 0.38*inch, size=16, leading=0.27*inch)
            x += card_w + gap
    elif "code" in slide:
        code = slide["code"]
        lines = max(5, len(code.splitlines()))
        code_h = min(3.75*inch, 0.48*inch + lines*0.22*inch)
        draw_code(c, code, 0.52*inch, H - 1.10*inch, W - 1.04*inch, code_h)
        draw_bullets(c, slide.get("bullets", []), 0.58*inch, H - 1.34*inch - code_h, W - 1.15*inch, size=15.4, leading=0.25*inch)
    else:
        draw_bullets(c, slide.get("bullets", []), 0.64*inch, H - 1.23*inch, W - 1.26*inch, size=18, leading=0.31*inch)

def main():
    OUTPUT.parent.mkdir(parents=True, exist_ok=True)
    c = canvas.Canvas(str(OUTPUT), pagesize=(W, H))
    c.setTitle("Lecture 05 - Classes and Objects")
    for number, slide in enumerate(slides, 1):
        draw_slide(c, slide, number)
        c.showPage()
    c.save()
    print(f"Created {OUTPUT} with {len(slides)} pages")

if __name__ == "__main__":
    main()
