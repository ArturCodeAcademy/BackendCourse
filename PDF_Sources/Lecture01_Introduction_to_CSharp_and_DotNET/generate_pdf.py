from pathlib import Path
import textwrap
from reportlab.lib import colors
from reportlab.lib.pagesizes import landscape
from reportlab.lib.units import inch
from reportlab.pdfgen import canvas
from reportlab.pdfbase.pdfmetrics import stringWidth

ROOT = Path(__file__).resolve().parents[2]
OUTPUT = ROOT / "PDF" / "Lecture01_Introduction_to_CSharp_and_DotNET.pdf"
PAGE_SIZE = (13.333 * inch, 7.5 * inch)
W, H = PAGE_SIZE

BLUE = colors.HexColor("#1F4E79")
TEAL = colors.HexColor("#2A9D8F")
YELLOW = colors.HexColor("#F2C94C")
INK = colors.HexColor("#1F2933")
MUTED = colors.HexColor("#52606D")
LIGHT = colors.HexColor("#F5F7FA")
LINE = colors.HexColor("#D9E2EC")

slides = [
    {
        "kind": "title",
        "title": "Basics of Internet Technologies 2",
        "subtitle": "Back-end Programming and Database Management",
        "body": ["Lecture 01", "Introduction to C# and .NET"],
    },
    {
        "title": "Learning Objectives",
        "bullets": [
            "Explain what programming is in practical terms.",
            "Describe what backend development is responsible for.",
            "Separate C# from .NET conceptually.",
            "Create and run console applications.",
            "Read data from the console and convert text into numbers.",
            "Use variables, constants, basic types, operators, simple conditions, and arrays.",
            "Understand how the semester project will grow during the course.",
        ],
    },
    {
        "title": "Lesson Timing Plan",
        "bullets": [
            "10 min - programming, backend, C#, .NET, SDK, runtime.",
            "15 min - variables, constants, naming, basic types.",
            "20 min - console input, text values, conversion, Parse, TryParse.",
            "15 min - arithmetic, precedence, formatting, common numeric mistakes.",
            "15 min - simple conditions and boolean logic as a preview.",
            "20 min - arrays: one-dimensional, numeric, rectangular, jagged.",
            "10 min - semester project, milestones, exercises, questions.",
        ],
    },
    {
        "title": "Why This Matters",
        "bullets": [
            "A backend receives input, makes decisions, stores data, and returns useful output.",
            "Before APIs, databases, and authentication, students need confidence with ordinary code.",
            "Console applications are simple, visible, and perfect for learning the core ideas without web complexity.",
            "Every later backend concept reuses the same thinking: input -> processing -> output.",
        ],
    },
    {
        "title": "What Is Programming?",
        "bullets": [
            "Programming means giving precise instructions to a computer.",
            "A program reads data, stores values, performs operations, chooses paths, and produces results.",
            "Computers do exactly what the code says, not what the programmer hoped it meant.",
            "Good beginner code should be clear before it is clever.",
        ],
    },
    {
        "title": "What Is Backend Development?",
        "bullets": [
            "Backend code usually runs on a server or behind an application interface.",
            "It handles rules, data, users, security, persistence, and integration with other systems.",
            "A backend often exposes an API that clients can call over HTTP.",
            "In this course we start locally, then gradually move toward real web backends.",
        ],
    },
    {
        "title": "C# vs .NET",
        "columns": [
            ("C#", [
                "A programming language.",
                "Defines syntax and language features.",
                "Examples: variables, types, if, loops, arrays, classes, methods.",
            ]),
            (".NET", [
                "A development platform and runtime ecosystem.",
                "Provides libraries, tools, compilers, and runtime support.",
                "Used for console apps, web APIs, desktop apps, games, cloud services, and more.",
            ]),
        ],
    },
    {
        "title": ".NET SDK, Runtime, and CLR",
        "bullets": [
            "The SDK is used to create, build, run, and publish applications.",
            "The runtime is used to execute compiled .NET applications.",
            "The CLR is the Common Language Runtime. It manages execution of .NET code.",
            "For this course the selected SDK is .NET 10.0.302.",
        ],
    },
    {
        "title": "From Source Code to Execution",
        "diagram": ["C# source", "Compiler", "IL", "CLR", "Machine execution"],
        "note": "We only need the big picture today. JIT internals can wait.",
    },
    {
        "title": "Console Applications",
        "bullets": [
            "A console application runs in a terminal window.",
            "It is useful for learning because input and output are easy to see.",
            "The first file beginners usually meet is Program.cs.",
            "Top-level statements let us write the first program without defining a class manually.",
        ],
    },
    {
        "title": "Live Demo: Personal Expense Entry",
        "code": """Expense name: Coffee\nAmount: 4.50\nCurrency: EUR\n\nYou spent 4.50 EUR on Coffee.""",
        "bullets": [
            "This demo is intentionally tiny.",
            "It shows input, variables, parsing, and formatted output.",
            "Later lectures will turn the same idea into a menu, a collection, a stored file, a database, and an API.",
        ],
    },
    {
        "title": "Runnable Code Walkthrough Project",
        "bullets": [
            "Lecture 01 also includes a separate runnable project: Lecture01.CodeBasics.",
            "This project is not a bigger application. It is a guided code laboratory.",
            "It prints sections for variables, types, constants, operators, input, conversion, conditions, and arrays.",
            "The teacher can run it slowly and connect each output line to the source code.",
        ],
    },
    {
        "title": "Variables: Names for Values",
        "code": """string expenseName = \"Coffee\";\nint quantity = 2;\ndecimal price = 4.50m;\nbool isNecessary = false;\nchar currencyLetter = 'E';""",
        "bullets": [
            "A variable is a named place for a value.",
            "The type controls what kind of value can be stored.",
            "The name should explain the meaning of the value in the program.",
        ],
    },
    {
        "title": "Common Beginner Types",
        "code": """byte smallWholeNumber = 25;\nint wholeNumber = 1000;\nlong largeWholeNumber = 5000000000L;\nfloat approximateFloat = 3.14f;\ndouble approximateDouble = 3.1415926535;\ndecimal moneyValue = 19.99m;""",
        "bullets": [
            "Whole-number types store values without a fractional part.",
            "float and double are approximate floating-point types.",
            "decimal is usually a better beginner choice for money-like examples.",
        ],
    },
    {
        "title": "Text, Characters, and Boolean Values",
        "code": """string title = \"Coffee\";\nchar firstLetter = 'C';\nbool isPaid = false;\nbool isExpensive = price > 100;""",
        "bullets": [
            "string stores text with zero, one, or many characters.",
            "char stores one character.",
            "bool stores true or false and is used by conditions.",
        ],
    },
    {
        "title": "Constants",
        "code": """const decimal VatRate = 0.25m;\nconst string DefaultCurrency = \"EUR\";\n\ndecimal vat = subtotal * VatRate;""",
        "bullets": [
            "A constant is a value that should not change while the program runs.",
            "Constants make important fixed values visible and named.",
            "Use constants for values such as default currency, tax rates, and fixed limits.",
        ],
    },
    {
        "title": "Naming Rules and Naming Style",
        "bullets": [
            "Variable names should describe meaning: price, quantity, productName.",
            "Avoid names that only describe type: text, number, data.",
            "Local variables usually use camelCase.",
            "Constants often use PascalCase in simple C# teaching examples.",
            "Names cannot contain spaces and cannot start with a digit.",
        ],
    },
    {
        "title": "Arithmetic Operators",
        "code": """decimal subtotal = quantity * price;\ndecimal vat = subtotal * VatRate;\ndecimal total = subtotal + vat;\n\nConsole.WriteLine(total);""",
        "bullets": [
            "Operators create new values from existing values.",
            "Common arithmetic operators are +, -, *, /, and %.",
            "The result type matters: int division and decimal division behave differently.",
        ],
    },
    {
        "title": "Integer Division vs Decimal Division",
        "code": """Console.WriteLine(10 / 3);\nConsole.WriteLine(10m / 3m);""",
        "bullets": [
            "10 / 3 uses int values, so the result is 3.",
            "10m / 3m uses decimal values, so the result can include a fractional part.",
            "This is a common beginner bug when calculating averages, prices, or percentages.",
        ],
    },
    {
        "title": "Operator Precedence",
        "code": """int firstResult = 2 + 3 * 4;\nint secondResult = (2 + 3) * 4;""",
        "bullets": [
            "Multiplication happens before addition.",
            "Parentheses make the intended order obvious.",
            "Use parentheses when it helps a beginner reader understand the calculation.",
        ],
    },
    {
        "title": "Console Output",
        "code": """Console.Write(\"Product name: \" );\nConsole.WriteLine(\"Saved.\");\nConsole.WriteLine(total);""",
        "bullets": [
            "Console.Write prints without moving to the next line.",
            "Console.WriteLine prints and then moves to the next line.",
            "Clear prompts help the user know what to enter.",
        ],
    },
    {
        "title": "Console Input Is Text",
        "code": """Console.Write(\"Enter price: \" );\nstring priceText = Console.ReadLine();\n\nConsole.WriteLine(priceText);""",
        "bullets": [
            "Console.ReadLine reads what the user typed.",
            "The returned value is text, even if the user typed digits.",
            "Text must be converted before it can be used as a number.",
        ],
    },
    {
        "title": "Parsing Text to Numbers",
        "code": """string priceText = Console.ReadLine();\ndecimal price = decimal.Parse(priceText, CultureInfo.InvariantCulture);\n\nstring quantityText = Console.ReadLine();\nint quantity = int.Parse(quantityText, CultureInfo.InvariantCulture);""",
        "bullets": [
            "Parse converts text into a specific type.",
            "decimal.Parse converts text to decimal.",
            "int.Parse converts text to int.",
            "Invalid text causes an error, which is useful to demonstrate but not user-friendly yet.",
        ],
    },
    {
        "title": "Culture and Decimal Separators",
        "code": """decimal price = decimal.Parse(priceText, CultureInfo.InvariantCulture);\nConsole.WriteLine(price.ToString(\"F2\", CultureInfo.InvariantCulture));""",
        "bullets": [
            "Some systems use comma as a decimal separator, others use dot.",
            "For teaching examples we use invariant culture so 4.50 stays 4.50.",
            "This makes demo output stable on different computers.",
        ],
    },
    {
        "title": "Safer Conversion with TryParse",
        "code": """bool ok = int.TryParse(\n    discountText,\n    CultureInfo.InvariantCulture,\n    out int discountPercent);\n\nif (ok)\n{\n    Console.WriteLine(discountPercent);\n}""",
        "bullets": [
            "TryParse tries to convert text without crashing the program.",
            "It returns true when conversion succeeds and false when it fails.",
            "The converted value is placed into the out variable.",
        ],
    },
    {
        "title": "Simple Conditions",
        "code": """if (enteredPrice <= 0)\n{\n    Console.WriteLine(\"Price must be greater than zero.\");\n}\nelse\n{\n    Console.WriteLine(\"Price accepted.\");\n}""",
        "bullets": [
            "A condition lets the program choose a path.",
            "The expression inside if must produce true or false.",
            "Lecture 02 will return to conditions and use them in menus.",
        ],
    },
    {
        "title": "Comparison Operators",
        "bullets": [
            "== means equal to.",
            "!= means not equal to.",
            "> and < compare greater than and less than.",
            ">= and <= include equality.",
            "A comparison produces a bool value: true or false.",
        ],
    },
    {
        "title": "Logical Operators",
        "code": """bool priceIsPositive = enteredPrice > 0;\nbool quantityIsPositive = enteredQuantity > 0;\nbool canSave = priceIsPositive && quantityIsPositive;\nbool needsAttention = enteredTotal > 100 || discountPercent > 30;""",
        "bullets": [
            "&& means both sides must be true.",
            "|| means at least one side must be true.",
            "! reverses a bool value.",
        ],
    },
    {
        "title": "String Concatenation",
        "code": """string sentence = \"You entered \" + quantity + \" item(s) of \" + productName + \".\";""",
        "bullets": [
            "Concatenation joins text pieces together with +.",
            "Numbers can be joined into text for output.",
            "Long concatenation becomes difficult to read.",
        ],
    },
    {
        "title": "Helpful Syntax: String Interpolation",
        "code": """string sentence = $\"You entered {quantity} item(s) of {productName}.\";""",
        "bullets": [
            "String interpolation starts with $ before the string.",
            "Values are placed inside braces.",
            "This is easier to read than long concatenation.",
        ],
    },
    {
        "title": "Helpful Syntax: var",
        "code": """var title = \"Coffee\";\nvar count = 3;\nvar total = 12.50m;""",
        "bullets": [
            "var lets the compiler infer the type from the assigned value.",
            "The variable still has a real type.",
            "Use explicit types while learning, then introduce var when the type is obvious.",
        ],
    },
    {
        "title": "One-dimensional Arrays",
        "code": """string[] productNames = new string[3];\nproductNames[0] = \"Coffee\";\nproductNames[1] = \"Tea\";\nproductNames[2] = productName;""",
        "bullets": [
            "An array stores several values of the same type.",
            "Array indexes start at 0.",
            "The Length property tells how many elements the array has.",
        ],
    },
    {
        "title": "Array Indexing",
        "code": """Console.WriteLine(productNames[0]);\nConsole.WriteLine(productNames[1]);\nConsole.WriteLine(productNames[2]);\nConsole.WriteLine(productNames.Length);""",
        "bullets": [
            "productNames[0] reads the first element.",
            "productNames[2] reads the third element.",
            "Using an index outside the array causes an error.",
        ],
    },
    {
        "title": "Array Initializer Syntax",
        "code": """decimal[] prices = { 4.50m, 3.20m, enteredPrice };\n\ndecimal priceSum = prices[0] + prices[1] + prices[2];""",
        "bullets": [
            "Initializer syntax creates an array with known starting values.",
            "The array size is inferred from the number of values.",
            "This is useful for small examples and fixed test data.",
        ],
    },
    {
        "title": "for Loop Over an Array",
        "code": """for (int index = 0; index < productNames.Length; index++)\n{\n    Console.WriteLine(productNames[index]);\n}""",
        "bullets": [
            "A for loop is useful when the index matters.",
            "The loop starts at index 0.",
            "The loop continues while index is less than array Length.",
        ],
    },
    {
        "title": "foreach Loop Over an Array",
        "code": """foreach (string name in productNames)\n{\n    Console.WriteLine(name);\n}""",
        "bullets": [
            "foreach reads each value in order.",
            "It is simpler when the index is not needed.",
            "Lecture 02 and Lecture 03 will practice loops and collections more deeply.",
        ],
    },
    {
        "title": "Two-dimensional Rectangular Arrays",
        "code": """int[,] weeklySales = new int[2, 3];\nweeklySales[0, 0] = 5;\nweeklySales[1, 2] = 6;\n\nConsole.WriteLine(weeklySales.GetLength(0));\nConsole.WriteLine(weeklySales.GetLength(1));""",
        "bullets": [
            "A rectangular array has rows and columns.",
            "Use two indexes: row and column.",
            "GetLength(0) returns row count. GetLength(1) returns column count.",
        ],
    },
    {
        "title": "Jagged Arrays",
        "code": """string[][] projectIdeas = new string[2][];\nprojectIdeas[0] = new string[] { \"ATM\", \"Expense Tracker\" };\nprojectIdeas[1] = new string[] { \"Vet Clinic\", \"Task Manager\", \"Library\" };""",
        "bullets": [
            "A jagged array is an array of arrays.",
            "Inner arrays can have different lengths.",
            "This is different from a rectangular array.",
        ],
    },
    {
        "title": "Common Mistakes Today",
        "bullets": [
            "Confusing C# the language with .NET the platform.",
            "Forgetting that Console.ReadLine returns text.",
            "Trying to calculate with text before conversion.",
            "Using comma instead of dot when the code expects invariant culture.",
            "Using array index 3 in an array that has indexes 0, 1, and 2.",
            "Trying to add architecture before the program has a real problem to solve.",
        ],
    },
    {
        "title": "The Semester Project",
        "quote": "During this course you will build one application of your own.",
        "bullets": [
            "You will start with a very small console application.",
            "After almost every lecture you will improve it using the concepts learned during that lecture.",
            "During the final assessment you will show both the evolution of your application and its final backend version.",
            "The first version is single-user. Users, authentication, roles, and authorization come later.",
        ],
    },
    {
        "title": "Project Evolution Milestones",
        "diagram": [
            "Milestone 01: Basic Console",
            "Milestone 02: OOP + Persistence",
            "Milestone 03: Layered Multi-user Console",
            "Milestone 04: Database Application",
            "Milestone 05: Basic Web API",
            "Final: Full Backend Project",
        ],
        "note": "Students do not need twenty full copies. Keep important snapshot versions that can be shown during the defense.",
    },
    {
        "title": "Assessment Has Two Parts",
        "columns": [
            ("Part A - Project Evolution", [
                "Show milestone versions.",
                "Explain what changed.",
                "Explain what problem each new technique solved.",
            ]),
            ("Part B - Final Project", [
                "Run the final backend application.",
                "Show database, API, login, CRUD, validation, ownership, and code structure.",
                "Connect the final result to the semester evolution.",
            ]),
        ],
    },
    {
        "title": "Possible Project Topics 1-14",
        "bullets": [
            "1. ATM Simulator", "2. Task Manager", "3. Veterinary Clinic", "4. Personal Library",
            "5. Expense Tracker", "6. Habit Tracker", "7. Workout Tracker", "8. Movie Collection",
            "9. Recipe Manager", "10. Study Planner", "11. Inventory Manager", "12. Event Planner",
            "13. Hotel Booking System", "14. Cinema Booking",
        ],
    },
    {
        "title": "Possible Project Topics 15-28",
        "bullets": [
            "15. Restaurant Reservation System", "16. Food Delivery Simulator", "17. Car Rental", "18. Parking Manager",
            "19. Car Maintenance Tracker", "20. Bicycle Service", "21. Game Collection", "22. Achievement Tracker",
            "23. Music Collection", "24. Playlist Manager", "25. Notes Application", "26. Password Vault Simulation",
            "27. Contacts Manager", "28. Calendar Application",
        ],
    },
    {
        "title": "Possible Project Topics 29-40",
        "bullets": [
            "29. Course Registration System", "30. Grade Tracker", "31. Library Lending System", "32. Warehouse Management",
            "33. Online Shop", "34. Support Ticket System", "35. Bug Tracker", "36. Project Management System",
            "37. Pet Care Tracker", "38. Medication Schedule", "39. Travel Planner", "40. Simple Social Posts Application",
            "Students may suggest their own topic after teacher approval.",
        ],
    },
    {
        "title": "Student Project Increment",
        "bullets": [
            "Choose a topic.",
            "Create a separate Visual Studio Solution for your own project.",
            "Give the application a clear name.",
            "Write 3-5 sentences about the purpose of the program.",
            "Define the first main data type.",
            "Create a Console project.",
            "Read 2-3 values through the console and convert numeric input.",
            "Optionally store several starting values in an array and print them.",
        ],
    },
    {
        "title": "Example First Data Type Ideas",
        "columns": [
            ("Project", ["Expense Tracker", "Task Manager", "Veterinary Clinic", "ATM Simulator"]),
            ("First Data", ["Expense name + amount", "Task title + due date", "Pet name + visit reason", "Balance + deposit amount"]),
        ],
    },
    {
        "title": "Exercises and Questions",
        "bullets": [
            "Explain the difference between C# and .NET in your own words.",
            "Create a console application that reads a name, one decimal number, and one whole number.",
            "Convert the entered text into useful numeric types.",
            "Print calculations using concatenation and string interpolation.",
            "Create an array with at least three values from your project domain.",
            "Write a short paragraph describing your semester project idea.",
        ],
    },
    {
        "title": "Summary",
        "bullets": [
            "Programming is precise instruction writing.",
            "Backend development focuses on data, rules, users, persistence, and APIs.",
            "C# is the language; .NET is the platform and runtime ecosystem.",
            "Console input arrives as text and often needs conversion.",
            "Variables, operators, conditions, and arrays are the base for the next lectures.",
            "The semester project starts now and grows step by step into a real backend.",
        ],
    },
]

def fit_text(c, text, font, max_size, min_size, max_width):
    size = max_size
    while size > min_size and stringWidth(text, font, size) > max_width:
        size -= 1
    return size


def wrap(text, font, size, width):
    avg = max(stringWidth("n", font, size), 1)
    chars = max(22, int(width / (size * 0.46)))
    lines = []
    for paragraph in str(text).split("\n"):
        if not paragraph:
            lines.append("")
        else:
            lines.extend(textwrap.wrap(paragraph, width=chars, break_long_words=False))
    return lines


def draw_header(c, title, page, total):
    c.setFillColor(BLUE)
    c.rect(0, 0, 0.22 * inch, H, fill=1, stroke=0)
    c.setFillColor(TEAL)
    c.rect(0.22 * inch, H - 0.16 * inch, W - 0.22 * inch, 0.16 * inch, fill=1, stroke=0)
    c.setFillColor(INK)
    size = fit_text(c, title, "Helvetica-Bold", 27, 18, W - 1.3 * inch)
    c.setFont("Helvetica-Bold", size)
    c.drawString(0.65 * inch, H - 0.72 * inch, title)
    c.setStrokeColor(LINE)
    c.line(0.65 * inch, H - 0.92 * inch, W - 0.65 * inch, H - 0.92 * inch)
    c.setFont("Helvetica", 9)
    c.setFillColor(MUTED)
    c.drawRightString(W - 0.65 * inch, 0.35 * inch, f"Lecture 01 | {page}/{total}")


def draw_bullets(c, bullets, x, y, width, font_size=20, leading=28):
    c.setFont("Helvetica", font_size)
    c.setFillColor(INK)
    for bullet in bullets:
        lines = wrap(bullet, "Helvetica", font_size, width - 0.25 * inch)
        if y < 0.8 * inch:
            return y
        c.setFillColor(TEAL)
        c.circle(x + 0.06 * inch, y + 0.07 * inch, 0.045 * inch, fill=1, stroke=0)
        c.setFillColor(INK)
        for i, line in enumerate(lines):
            c.drawString(x + 0.23 * inch, y - i * leading, line)
        y -= max(leading, len(lines) * leading) + 0.12 * inch
    return y


def draw_code(c, code, x, y, width, height, font_size=15):
    c.setFillColor(colors.HexColor("#111827"))
    c.roundRect(x, y - height, width, height, 8, fill=1, stroke=0)
    c.setFillColor(colors.HexColor("#E5E7EB"))
    c.setFont("Courier", font_size)
    line_y = y - 0.35 * inch
    for line in code.split("\n"):
        c.drawString(x + 0.25 * inch, line_y, line)
        line_y -= font_size + 6


def draw_diagram(c, items, note=None):
    long_flow = len(items) >= 6
    box_w = (4.9 if long_flow else 4.1) * inch
    box_h = (0.42 if long_flow else 0.52) * inch
    arrow_gap = (0.22 if long_flow else 0.42) * inch
    font_size = 13 if long_flow else 16
    x = (W - box_w) / 2
    y = H - (1.45 if long_flow else 1.75) * inch
    for index, item in enumerate(items):
        c.setFillColor(LIGHT if index % 2 == 0 else colors.white)
        c.setStrokeColor(TEAL)
        c.roundRect(x, y - box_h, box_w, box_h, 7, fill=1, stroke=1)
        c.setFillColor(INK)
        c.setFont("Helvetica-Bold", font_size)
        c.drawCentredString(W / 2, y - box_h * 0.66, item)
        y -= box_h
        if index < len(items) - 1:
            c.setStrokeColor(BLUE)
            top = y - 0.04 * inch
            bottom = y - arrow_gap + 0.05 * inch
            c.line(W / 2, top, W / 2, bottom)
            c.line(W / 2, bottom, W / 2 - 0.055 * inch, bottom + 0.06 * inch)
            c.line(W / 2, bottom, W / 2 + 0.055 * inch, bottom + 0.06 * inch)
            y -= arrow_gap
    if note:
        note_size = 12 if long_flow else 15
        c.setFont("Helvetica", note_size)
        c.setFillColor(MUTED)
        lines = wrap(note, "Helvetica", note_size, W - 2 * inch)
        note_y = y - (0.34 if long_flow else 0.20) * inch
        min_y = 0.72 * inch
        for line in lines:
            if note_y < min_y:
                break
            c.drawCentredString(W / 2, note_y, line)
            note_y -= (note_size + 4)


def draw_columns(c, columns):
    gap = 0.45 * inch
    col_w = (W - 1.3 * inch * 2 - gap) / 2
    y_top = H - 1.45 * inch
    for i, (heading, bullets) in enumerate(columns):
        x = 1.3 * inch + i * (col_w + gap)
        c.setFillColor(LIGHT)
        c.setStrokeColor(LINE)
        c.roundRect(x, 0.95 * inch, col_w, H - 2.45 * inch, 8, fill=1, stroke=1)
        c.setFillColor(BLUE)
        c.setFont("Helvetica-Bold", 21)
        c.drawString(x + 0.25 * inch, y_top, heading)
        draw_bullets(c, bullets, x + 0.25 * inch, y_top - 0.55 * inch, col_w - 0.5 * inch, font_size=16, leading=23)


def draw_slide(c, slide, page, total):
    if slide.get("kind") == "title":
        c.setFillColor(BLUE)
        c.rect(0, 0, W, H, fill=1, stroke=0)
        c.setFillColor(TEAL)
        c.rect(0, 0, W, 0.28 * inch, fill=1, stroke=0)
        c.setFillColor(YELLOW)
        c.rect(0.8 * inch, H - 1.12 * inch, 2.4 * inch, 0.09 * inch, fill=1, stroke=0)
        c.setFillColor(colors.white)
        c.setFont("Helvetica-Bold", 40)
        c.drawString(0.8 * inch, H - 1.85 * inch, slide["title"])
        c.setFont("Helvetica", 25)
        c.drawString(0.8 * inch, H - 2.35 * inch, slide["subtitle"])
        c.setFont("Helvetica-Bold", 22)
        y = H - 3.55 * inch
        for line in slide["body"]:
            c.drawString(0.8 * inch, y, line)
            y -= 0.42 * inch
        c.setFont("Helvetica", 10)
        c.drawRightString(W - 0.65 * inch, 0.35 * inch, f"{page}/{total}")
        return

    draw_header(c, slide["title"], page, total)
    if "quote" in slide:
        c.setFillColor(LIGHT)
        c.setStrokeColor(TEAL)
        c.roundRect(0.9 * inch, H - 2.15 * inch, W - 1.8 * inch, 0.72 * inch, 8, fill=1, stroke=1)
        c.setFillColor(BLUE)
        c.setFont("Helvetica-Bold", 21)
        c.drawCentredString(W / 2, H - 1.72 * inch, slide["quote"])
        draw_bullets(c, slide.get("bullets", []), 0.95 * inch, H - 2.75 * inch, W - 1.9 * inch, font_size=18, leading=25)
    elif "columns" in slide:
        draw_columns(c, slide["columns"])
    elif "diagram" in slide:
        draw_diagram(c, slide["diagram"], slide.get("note"))
    elif "code" in slide and "bullets" in slide:
        code_lines = len(slide["code"].split("\n"))
        code_height = max(2.15 * inch, 0.62 * inch + code_lines * 0.29 * inch)
        code_height = min(code_height, 3.10 * inch)
        draw_code(c, slide["code"], 0.85 * inch, H - 1.45 * inch, W - 1.7 * inch, code_height)
        bullet_y = H - 1.45 * inch - code_height - 0.55 * inch
        draw_bullets(c, slide["bullets"], 0.95 * inch, bullet_y, W - 1.9 * inch, font_size=17, leading=24)
    elif "code" in slide:
        draw_code(c, slide["code"], 1.0 * inch, H - 1.7 * inch, W - 2.0 * inch, 3.6 * inch, font_size=17)
    else:
        count = len(slide.get("bullets", []))
        font_size = 19 if count <= 9 else 15
        leading = 27 if count <= 9 else 19
        draw_bullets(c, slide.get("bullets", []), 0.95 * inch, H - 1.55 * inch, W - 1.9 * inch, font_size=font_size, leading=leading)


def main():
    OUTPUT.parent.mkdir(parents=True, exist_ok=True)
    c = canvas.Canvas(str(OUTPUT), pagesize=landscape(PAGE_SIZE))
    total = len(slides)
    for page, slide in enumerate(slides, start=1):
        draw_slide(c, slide, page, total)
        c.showPage()
    c.save()
    print(OUTPUT)


if __name__ == "__main__":
    main()






