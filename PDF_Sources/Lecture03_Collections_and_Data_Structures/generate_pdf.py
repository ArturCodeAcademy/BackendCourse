from pathlib import Path
import textwrap
from reportlab.lib import colors
from reportlab.lib.pagesizes import landscape
from reportlab.lib.units import inch
from reportlab.pdfgen import canvas
from reportlab.pdfbase.pdfmetrics import stringWidth

ROOT = Path(__file__).resolve().parents[2]
OUTPUT = ROOT / "PDF" / "Lecture03_Collections_and_Data_Structures.pdf"
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
    {"kind":"title","title":"Lecture 03","subtitle":"Collections and Data Structures","body":["Basics of Internet Technologies 2","From one item to many items"]},
    {"title":"Learning Objectives","bullets":["Explain why one variable is not enough for real applications.","Use arrays and explain fixed size.","Use List<T> as the main collection for changing data.","Add, read, remove, count, index, and iterate list items.","Find values with loops and simple conditions.","Use Dictionary<TKey,TValue> for lookup by key.","Use HashSet<T> for unique values.","Use enum for fixed named choices.","Explain generics at a beginner level."]},
    {"title":"Connection to Lecture 02","bullets":["Lecture 02 created an interactive menu.","The application could add and show one current expense.","That is better than Lecture 01, but still not enough.","Today we keep the menu, but store many values instead of one value."]},
    {"title":"Lesson Timing Plan","bullets":["10 min - recap and the problem with one item.","10 min - arrays and fixed size.","30 min - List<T>: Add, Count, indexing, iteration, Remove, RemoveAt.","15 min - searching and deleting by index.","15 min - parallel lists and why they are risky.","15 min - Dictionary and HashSet.","10 min - enum and generics.","15 min - live coding the multi-expense menu.","10 min - student increment and exercises."]},
    {"title":"Problem: One Item Is Not Enough","code":"""string expenseName = \"Coffee\";\ndecimal expenseAmount = 4.50m;\n\n// What about Tea? Lunch? Bus ticket?""","bullets":["A real application usually stores many records.","Creating a new variable for every item does not scale.","We need a structure that can hold many values together."]},
    {"title":"Bad Scaling with Separate Variables","code":"""string expense1 = \"Coffee\";\nstring expense2 = \"Tea\";\nstring expense3 = \"Lunch\";\nstring expense4 = \"Bus\";""","bullets":["This becomes ugly quickly.","There is no easy way to loop through all expenses.","Adding the 100th expense would require another variable name."]},
    {"title":"Data Structure Idea","bullets":["A data structure organizes values so the program can work with them.","Different structures solve different problems.","Today we focus on beginner structures built into C# and .NET.","The main structure for the demo is List<T>."]},
    {"title":"Arrays Recap","code":"""string[] names = new string[3];\nnames[0] = \"Coffee\";\nnames[1] = \"Tea\";\nnames[2] = \"Lunch\";""","bullets":["An array stores several values of the same type.","Indexes start at 0.","Array length is fixed after creation."]},
    {"title":"Array Fixed Size","code":"""string[] names = new string[3];\nConsole.WriteLine(names.Length);""","bullets":["The array has exactly three slots.","You cannot simply Add a fourth value to this array.","Fixed size is useful sometimes, but not ideal for unknown user input."]},
    {"title":"Array Indexing","code":"""Console.WriteLine(names[0]);\nConsole.WriteLine(names[1]);\nConsole.WriteLine(names[2]);""","bullets":["Index 0 is the first element.","Index 2 is the third element.","Index 3 would be outside a three-element array."]},
    {"title":"Array Initializer","code":"""string[] commands = { \"add\", \"list\", \"exit\" };""","bullets":["Initializer syntax is compact for known values.","The array length is inferred from the number of values.","This is useful for examples and fixed option lists."]},
    {"title":"List<T>: Main Collection Today","code":"""List<string> names = new List<string>();\nnames.Add(\"Coffee\");\nnames.Add(\"Tea\");\nnames.Add(\"Lunch\");""","bullets":["List<T> stores values of one type.","A list can grow as values are added.","T is a placeholder for the element type, such as string or decimal."]},
    {"title":"List<T> Count and Indexing","code":"""Console.WriteLine(names.Count);\nConsole.WriteLine(names[0]);""","bullets":["Count tells how many values are currently in the list.","Indexing works like arrays: the first index is 0.","Use Count, not Length, for List<T>."]},
    {"title":"List<T> with Different Types","code":"""List<string> expenseNames = new List<string>();\nList<decimal> expenseAmounts = new List<decimal>();\nList<int> quantities = new List<int>();""","bullets":["The collection idea is the same.","The element type changes what values are allowed.","This is the first simple example of generics."]},
    {"title":"Adding Items","code":"""expenseNames.Add(\"Coffee\");\nexpenseAmounts.Add(4.50m);""","bullets":["Add places a new value at the end of the list.","After Add, Count increases by one.","The value type must match the list type."]},
    {"title":"Reading Items by Index","code":"""string firstName = expenseNames[0];\ndecimal firstAmount = expenseAmounts[0];""","bullets":["Indexing is useful when the position matters.","The index must be inside the list range.","For display, users usually see index + 1, not the zero-based index."]},
    {"title":"Displaying User Numbers","code":"""for (int index = 0; index < expenseNames.Count; index++)\n{\n    int displayNumber = index + 1;\n    Console.WriteLine(displayNumber + \". \" + expenseNames[index]);\n}""","bullets":["Program indexes start at 0.","Human-friendly numbers usually start at 1.","Convert display number back to index when deleting."]},
    {"title":"foreach with List<T>","code":"""foreach (string name in expenseNames)\n{\n    Console.WriteLine(name);\n}""","bullets":["foreach is clear when you only need the value.","It hides the index.","Use for when you need index-based access to parallel lists."]},
    {"title":"Removing by Value","code":"""expenseNames.Remove(\"Tea\");""","bullets":["Remove deletes the first matching value.","It returns whether something was removed.","This is simple for unique values, but ambiguous when duplicates exist."]},
    {"title":"Removing by Index","code":"""expenseNames.RemoveAt(0);""","bullets":["RemoveAt deletes the value at a specific index.","Indexes after that position move left.","Always validate the index before removing."]},
    {"title":"Parallel Lists","code":"""List<string> names = new List<string>();\nList<decimal> amounts = new List<decimal>();\n\nnames.Add(\"Coffee\");\namounts.Add(4.50m);""","bullets":["Parallel lists store related data at the same index.","names[0] belongs with amounts[0].","This is useful for now, but risky if the lists get out of sync."]},
    {"title":"Why Parallel Lists Are Risky","code":"""names.RemoveAt(0);\n// Forgot: amounts.RemoveAt(0);""","bullets":["Now names and amounts no longer describe the same records.","This is a real problem students can understand before OOP.","Lecture 05 will solve it with classes and objects."]},
    {"title":"Finding with a Loop","code":"""string search = \"cof\";\nfor (int index = 0; index < names.Count; index++)\n{\n    if (names[index].ToLowerInvariant().Contains(search))\n    {\n        Console.WriteLine(names[index]);\n    }\n}""","bullets":["Searching combines a loop and a condition.","Contains checks whether text includes another text.","ToLowerInvariant makes a simple case-insensitive search possible."]},
    {"title":"Delete by Display Number","code":"""int index = displayNumber - 1;\n\nif (index >= 0 && index < names.Count)\n{\n    names.RemoveAt(index);\n    amounts.RemoveAt(index);\n}""","bullets":["The user enters 1, 2, 3, ...","The list needs indexes 0, 1, 2, ...","Validation prevents out-of-range errors."]},
    {"title":"Summing a List","code":"""decimal total = 0m;\nforeach (decimal amount in amounts)\n{\n    total += amount;\n}""","bullets":["Start with an empty total.","Add each amount to the total.","This is a common collection pattern."]},
    {"title":"Dictionary<TKey, TValue>","code":"""Dictionary<string, decimal> pricesByName = new Dictionary<string, decimal>();\npricesByName[\"Coffee\"] = 4.50m;\npricesByName[\"Tea\"] = 3.20m;""","bullets":["A dictionary stores key-value pairs.","The key is used for lookup.","Keys must be unique."]},
    {"title":"Dictionary Lookup","code":"""if (pricesByName.ContainsKey(\"Tea\"))\n{\n    Console.WriteLine(pricesByName[\"Tea\"]);\n}""","bullets":["Check that a key exists before reading it.","Reading a missing key causes an error.","Dictionaries are useful when lookup by key is the main operation."]},
    {"title":"TryGetValue","code":"""if (pricesByName.TryGetValue(\"Water\", out decimal price))\n{\n    Console.WriteLine(price);\n}\nelse\n{\n    Console.WriteLine(\"Not found\");\n}""","bullets":["TryGetValue combines checking and reading.","It returns true when the key exists.","It avoids reading a missing key directly."]},
    {"title":"Iterating a Dictionary","code":"""foreach (KeyValuePair<string, decimal> pair in pricesByName)\n{\n    Console.WriteLine(pair.Key + \" -> \" + pair.Value);\n}""","bullets":["Each dictionary item has a Key and a Value.","Order is not the main point of a dictionary.","Use a dictionary for lookup, not for ordered display."]},
    {"title":"HashSet<T>: Unique Values","code":"""HashSet<string> categories = new HashSet<string>();\ncategories.Add(\"Food\");\ncategories.Add(\"Transport\");\ncategories.Add(\"Food\");""","bullets":["A hash set stores unique values.","Adding the same value twice does not create a duplicate.","Use it when uniqueness matters more than order."]},
    {"title":"HashSet Example","code":"""Console.WriteLine(categories.Count);\nConsole.WriteLine(categories.Contains(\"Food\"));""","bullets":["Count shows how many unique values exist.","Contains checks whether a value is present.","This can help with tags, categories, usernames, or visited items."]},
    {"title":"enum: Named Choices","code":"""enum ExpenseCategory\n{\n    Food = 1,\n    Transport = 2,\n    Study = 3,\n    Other = 4\n}""","bullets":["enum gives names to fixed choices.","It is better than magic numbers such as 1, 2, 3.","Enums are useful for status, category, role-like labels, and modes."]},
    {"title":"Using an enum","code":"""ExpenseCategory category = ExpenseCategory.Food;\n\nif (category == ExpenseCategory.Food)\n{\n    Console.WriteLine(\"Food expense\");\n}""","bullets":["The code becomes easier to read.","The compiler helps limit possible values.","Later projects can use enums for TaskStatus, BookingStatus, or ExpenseCategory."]},
    {"title":"Generics: Beginner View","code":"""List<int> numbers = new List<int>();\nList<string> words = new List<string>();\nList<ExpenseCategory> categories = new List<ExpenseCategory>();""","bullets":["Generic types use angle brackets.","The type inside the angle brackets says what the collection stores.","This gives type safety while reusing one collection design."]},
    {"title":"Choosing the Right Structure","columns":[("Structure",["Array","List<T>","Dictionary<TKey,TValue>","HashSet<T>","enum"]),("Use When",["Fixed number of values","Changing number of values","Lookup by key","Unique values","Fixed named choices"])]},
    {"title":"Live Demo: Multi-expense Menu","code":"""1. Add expense\n2. List expenses\n3. Find expense by name\n4. Delete expense by number\n5. Show total\n6. Exit""","bullets":["The demo now stores multiple expenses.","List<T> is the main storage mechanism.","Find and delete combine loops, indexes, and validation."]},
    {"title":"Demo Storage","code":"""List<string> expenseNames = new List<string>();\nList<decimal> expenseAmounts = new List<decimal>();\nList<string> expenseCategories = new List<string>();""","bullets":["This is intentionally not OOP yet.","The lists are parallel and must stay synchronized.","That weakness prepares students for classes later."]},
    {"title":"Student Project Increment","bullets":["Update the Lecture 02 project so it can store multiple objects.","Use List<T> as the main collection.","Add commands: Add, List, Find, Delete.","Use Count, indexing, for, and foreach where appropriate.","Validate indexes before deleting.","Optionally use enum for status or category.","Keep the project single-user and in-memory for now."]},
    {"title":"Student Examples","columns":[("Project",["Task Manager","Expense Tracker","Veterinary Clinic","Movie Collection"]),("Multiple Items",["Tasks with title and status","Expenses with amount and category","Pets or visits","Movies with title and rating"])]},
    {"title":"What Not To Add Yet","bullets":["Do not add JSON persistence yet.","Do not add classes yet unless the teacher explicitly permits preview work.","Do not add database storage.","Do not add login or users.","Today is about in-memory multiple items and choosing basic data structures."]},
    {"title":"Common Mistakes Today","bullets":["Using array when the number of values changes.","Using Length on List<T> instead of Count.","Forgetting that indexes start at 0.","Deleting from one parallel list but not the others.","Reading a missing dictionary key directly.","Using Dictionary when the order of display is the main requirement."]},
    {"title":"Exercises","bullets":["Add a category filter command to the demo.","Add a command that shows the largest expense.","Add a HashSet<string> to collect unique categories.","Create an enum for three statuses in your own project.","Write a method that prints all items from a List<string>.","Write a method that validates a delete number before RemoveAt."]},
    {"title":"Review Questions","bullets":["Why is one variable per item a bad approach?","What is the difference between array Length and list Count?","When should you use for instead of foreach?","Why are parallel lists risky?","What problem does Dictionary solve?","What problem does HashSet solve?","Why is enum better than magic numbers?","What does the T in List<T> represent?"]},
    {"title":"Summary","bullets":["Collections let programs work with many values.","Arrays are fixed size.","List<T> is the main beginner collection for changing data.","Dictionary<TKey,TValue> supports lookup by key.","HashSet<T> supports uniqueness.","enum gives readable names to fixed choices.","The student project now supports Add, List, Find, and Delete for multiple objects."]},
]

# Drawing helpers copied from Lecture 02 generator.
def fit_text(c, text, font, max_size, min_size, max_width):
    size = max_size
    while size > min_size and stringWidth(text, font, size) > max_width:
        size -= 1
    return size

def wrap(text, font, size, width):
    chars = max(22, int(width / (size * 0.46)))
    lines = []
    for paragraph in str(text).split("\n"):
        if not paragraph:
            lines.append("")
        else:
            lines.extend(textwrap.wrap(paragraph, width=chars, break_long_words=False))
    return lines

def draw_header(c, title, page, total):
    c.setFillColor(BLUE); c.rect(0,0,0.22*inch,H,fill=1,stroke=0)
    c.setFillColor(TEAL); c.rect(0.22*inch,H-0.16*inch,W-0.22*inch,0.16*inch,fill=1,stroke=0)
    c.setFillColor(INK)
    c.setFont("Helvetica-Bold", fit_text(c,title,"Helvetica-Bold",27,18,W-1.3*inch))
    c.drawString(0.65*inch,H-0.72*inch,title)
    c.setStrokeColor(LINE); c.line(0.65*inch,H-0.92*inch,W-0.65*inch,H-0.92*inch)
    c.setFont("Helvetica",9); c.setFillColor(MUTED); c.drawRightString(W-0.65*inch,0.35*inch,f"Lecture 03 | {page}/{total}")

def draw_bullets(c, bullets, x, y, width, font_size=20, leading=28):
    c.setFont("Helvetica", font_size)
    for bullet in bullets:
        lines = wrap(bullet,"Helvetica",font_size,width-0.25*inch)
        if y < 0.8*inch: return y
        c.setFillColor(TEAL); c.circle(x+0.06*inch,y+0.07*inch,0.045*inch,fill=1,stroke=0)
        c.setFillColor(INK)
        for i,line in enumerate(lines): c.drawString(x+0.23*inch,y-i*leading,line)
        y -= max(leading,len(lines)*leading)+0.12*inch
    return y

def draw_code(c, code, x, y, width, height, font_size=15):
    c.setFillColor(colors.HexColor("#111827")); c.roundRect(x,y-height,width,height,8,fill=1,stroke=0)
    c.setFillColor(colors.HexColor("#E5E7EB")); c.setFont("Courier",font_size)
    line_y = y - 0.35*inch
    for line in code.split("\n"):
        c.drawString(x+0.25*inch,line_y,line)
        line_y -= font_size + 6

def draw_columns(c, columns):
    gap=0.45*inch; col_w=(W-1.3*inch*2-gap)/2; y_top=H-1.45*inch
    for i,(heading,bullets) in enumerate(columns):
        x=1.3*inch+i*(col_w+gap)
        c.setFillColor(LIGHT); c.setStrokeColor(LINE); c.roundRect(x,0.95*inch,col_w,H-2.45*inch,8,fill=1,stroke=1)
        c.setFillColor(BLUE); c.setFont("Helvetica-Bold",21); c.drawString(x+0.25*inch,y_top,heading)
        draw_bullets(c,bullets,x+0.25*inch,y_top-0.55*inch,col_w-0.5*inch,font_size=16,leading=23)

def draw_slide(c, slide, page, total):
    if slide.get("kind") == "title":
        c.setFillColor(BLUE); c.rect(0,0,W,H,fill=1,stroke=0)
        c.setFillColor(TEAL); c.rect(0,0,W,0.28*inch,fill=1,stroke=0)
        c.setFillColor(YELLOW); c.rect(0.8*inch,H-1.12*inch,2.4*inch,0.09*inch,fill=1,stroke=0)
        c.setFillColor(colors.white); c.setFont("Helvetica-Bold",42); c.drawString(0.8*inch,H-1.85*inch,slide["title"])
        c.setFont("Helvetica",26); c.drawString(0.8*inch,H-2.35*inch,slide["subtitle"])
        c.setFont("Helvetica-Bold",20); y=H-3.55*inch
        for line in slide["body"]:
            c.drawString(0.8*inch,y,line); y-=0.42*inch
        c.setFont("Helvetica",10); c.drawRightString(W-0.65*inch,0.35*inch,f"{page}/{total}")
        return
    draw_header(c, slide["title"], page, total)
    if "columns" in slide:
        draw_columns(c, slide["columns"])
    elif "code" in slide and "bullets" in slide:
        lines = len(slide["code"].split("\n"))
        code_height = max(2.0*inch, 0.62*inch + lines*0.29*inch)
        if lines >= 10: code_height = min(code_height, 3.65*inch)
        else: code_height = min(code_height, 3.10*inch)
        font_size = 13 if lines >= 10 else 15
        draw_code(c, slide["code"], 0.85*inch, H-1.45*inch, W-1.7*inch, code_height, font_size=font_size)
        draw_bullets(c, slide["bullets"], 0.95*inch, H-1.45*inch-code_height-0.45*inch, W-1.9*inch, font_size=17, leading=24)
    elif "code" in slide:
        draw_code(c, slide["code"], 1.0*inch, H-1.7*inch, W-2.0*inch, 3.7*inch, font_size=16)
    else:
        count=len(slide.get("bullets",[])); font_size=19 if count<=9 else 15; leading=27 if count<=9 else 19
        draw_bullets(c, slide.get("bullets",[]), 0.95*inch, H-1.55*inch, W-1.9*inch, font_size=font_size, leading=leading)

def main():
    OUTPUT.parent.mkdir(parents=True, exist_ok=True)
    c=canvas.Canvas(str(OUTPUT), pagesize=landscape(PAGE_SIZE))
    total=len(slides)
    for page, slide in enumerate(slides, start=1):
        draw_slide(c, slide, page, total); c.showPage()
    c.save(); print(OUTPUT)

if __name__ == "__main__":
    main()

