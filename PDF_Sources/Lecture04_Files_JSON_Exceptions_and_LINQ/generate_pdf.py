from pathlib import Path
import textwrap
from reportlab.lib import colors
from reportlab.lib.pagesizes import landscape
from reportlab.lib.units import inch
from reportlab.pdfgen import canvas
from reportlab.pdfbase.pdfmetrics import stringWidth

ROOT = Path(__file__).resolve().parents[2]
OUTPUT = ROOT / "PDF" / "Lecture04_Files_JSON_Exceptions_and_LINQ.pdf"
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
{"kind":"title","title":"Lecture 04","subtitle":"Files, JSON, Exceptions and LINQ","body":["Basics of Internet Technologies 2","Making console data survive restart"]},
{"title":"Learning Objectives","bullets":["Explain memory vs persistent storage.","Use file paths and create directories.","Read and write text files.","Explain JSON as structured text.","Use System.Text.Json for serialization and deserialization.","Handle runtime problems with try, catch, and finally.","Use LINQ Where, Select, FirstOrDefault, Any, and OrderBy.","Add automatic load/save to a console application."]},
{"title":"Connection to Lecture 03","bullets":["Lecture 03 made the application work with many items.","The data lived in List<T> while the program was running.","When the program stopped, the list disappeared.","Today we keep data after restart by saving it to a file."]},
{"title":"Lesson Timing Plan","bullets":["10 min - problem: memory disappears.","15 min - paths, directories, text files.","20 min - JSON structure and System.Text.Json.","20 min - load/save demo with List<Dictionary<string,string>>.","20 min - exceptions and try/catch/finally.","20 min - LINQ minimum for searching and filtering.","15 min - live coding persistence into the expense app.","10 min - Milestone 01 and exercises."]},
{"title":"Problem: Data Disappears","code":"""List<string> names = new List<string>();\nnames.Add(\"Coffee\");\n\n// Program stops.\n// Memory is gone.""","bullets":["Memory is temporary.","Variables and lists live while the process runs.","Persistent storage keeps data outside the running program."]},
{"title":"Memory vs Persistent Storage","columns":[("Memory",["Fast","Temporary","Cleared when the program ends","Variables and lists"]),("Persistent Storage",["Slower","Survives restart","Stored on disk or database","Files, JSON, SQL later"])]},
{"title":"Files in This Lecture","bullets":["A file is data stored on disk.","A text file stores characters.","A JSON file stores structured text.","Today we use files before databases so students see the persistence problem clearly."]},
{"title":"Paths","code":"""string dataDirectory = Path.Combine(\n    Directory.GetCurrentDirectory(),\n    \"data\");\n\nstring filePath = Path.Combine(dataDirectory, \"expenses.json\");""","bullets":["A path tells the program where a file or folder is.","Path.Combine builds paths safely for the operating system.","Directory.GetCurrentDirectory returns where the program is running from."]},
{"title":"Creating a Directory","code":"""Directory.CreateDirectory(dataDirectory);\n\nConsole.WriteLine(Directory.Exists(dataDirectory));""","bullets":["CreateDirectory creates the folder if it does not exist.","It is safe to call even when the folder already exists.","Directory.Exists checks whether a directory is present."]},
{"title":"Writing a Text File","code":"""File.WriteAllText(filePath, \"Coffee\\nTea\\nLunch\");""","bullets":["WriteAllText creates or overwrites a text file.","The second argument is the text content.","Overwriting is simple, but later we will think about data safety."]},
{"title":"Reading a Text File","code":"""string text = File.ReadAllText(filePath);\nConsole.WriteLine(text);""","bullets":["ReadAllText reads the whole file as one string.","This is easy for small teaching examples.","Large files may need other approaches later."]},
{"title":"Reading Lines","code":"""string[] lines = File.ReadAllLines(filePath);\n\nforeach (string line in lines)\n{\n    Console.WriteLine(line);\n}""","bullets":["ReadAllLines gives an array of lines.","This connects file reading to arrays and foreach.","Line-based files are simple but not structured enough for our app."]},
{"title":"Why JSON?","bullets":["A list of lines loses meaning quickly.","JSON can represent structured data with names and values.","JSON is common in web APIs and configuration files.","Learning JSON now prepares students for HTTP and APIs later."]},
{"title":"JSON Object","code":"""{\n  \"name\": \"Coffee\",\n  \"amount\": \"4.50\",\n  \"category\": \"Food\"\n}""","bullets":["An object is wrapped in braces.","Each property has a name and a value.","JSON property names are usually written in double quotes."]},
{"title":"JSON Array","code":"""[\n  { \"name\": \"Coffee\", \"amount\": \"4.50\" },\n  { \"name\": \"Bus\", \"amount\": \"2.80\" }\n]""","bullets":["An array is wrapped in square brackets.","A JSON array can contain several objects.","This matches our list of expenses."]},
{"title":"Temporary Structure Before Classes","code":"""List<Dictionary<string, string>> expenses =\n    new List<Dictionary<string, string>>();""","bullets":["Classes are introduced in Lecture 05.","For today, a dictionary can represent one simple JSON-like object.","This keeps the focus on files and JSON, not object modeling."]},
{"title":"Creating One Dictionary Item","code":"""Dictionary<string, string> expense = new Dictionary<string, string>();\nexpense[\"name\"] = \"Coffee\";\nexpense[\"amount\"] = \"4.50\";\nexpense[\"category\"] = \"Food\";""","bullets":["The dictionary key is the property name.","The dictionary value is the stored text value.","This is not final architecture. It is a bridge toward classes."]},
{"title":"System.Text.Json","bullets":["System.Text.Json is the built-in .NET JSON library.","Serialization means C# value to JSON text.","Deserialization means JSON text back to C# value.","The same idea appears later in Web API request and response bodies."]},
{"title":"Serialization","code":"""JsonSerializerOptions options = new JsonSerializerOptions();\noptions.WriteIndented = true;\n\nstring json = JsonSerializer.Serialize(expenses, options);""","bullets":["Serialize converts C# data to JSON text.","WriteIndented makes the file easier for humans to read.","Pretty JSON is helpful during teaching and debugging."]},
{"title":"Saving JSON","code":"""Directory.CreateDirectory(dataDirectory);\nFile.WriteAllText(jsonFilePath, json);""","bullets":["Create the directory before writing into it.","WriteAllText stores the JSON string in a file.","After this, data can survive application restart."]},
{"title":"Deserialization","code":"""string json = File.ReadAllText(jsonFilePath);\nList<Dictionary<string, string>> items =\n    JsonSerializer.Deserialize<List<Dictionary<string, string>>>(json);""","bullets":["Deserialize converts JSON text back to C# data.","The type must match the expected JSON shape.","Invalid JSON can cause a JsonException."]},
{"title":"Automatic Load on Startup","code":"""List<Dictionary<string, string>> expenses =\n    LoadExpenses(dataFilePath);""","bullets":["The program loads saved data before showing the menu.","If the file does not exist, start with an empty list.","This keeps the first run simple and friendly."]},
{"title":"Automatic Save After Changes","code":"""AddExpense(expenses);\nSaveExpenses(dataFilePath, expenses);""","bullets":["Save after adding or deleting data.","This reduces the chance of losing work.","Later architecture will move this responsibility away from Program.cs."]},
{"title":"Exceptions","bullets":["An exception represents a runtime problem.","Examples: invalid number, missing file, invalid JSON, no permission.","Without handling, an exception can stop the program.","With handling, the program can show a useful message or recover."]},
{"title":"try and catch","code":"""try\n{\n    string text = File.ReadAllText(path);\n}\ncatch (FileNotFoundException ex)\n{\n    Console.WriteLine(\"File not found: \" + ex.FileName);\n}""","bullets":["Code that may fail goes inside try.","catch handles a specific exception type.","The exception object contains information about the problem."]},
{"title":"Handling Invalid JSON","code":"""catch (JsonException)\n{\n    Console.WriteLine(\"The JSON file is invalid.\");\n    return new List<Dictionary<string, string>>();\n}""","bullets":["Invalid data should not destroy the lesson demo.","The app can start with an empty list after warning the user.","In production, we would be more careful with backups and logging."]},
{"title":"finally","code":"""try\n{\n    Console.WriteLine(\"Trying work\");\n}\ncatch (IOException ex)\n{\n    Console.WriteLine(ex.Message);\n}\nfinally\n{\n    Console.WriteLine(\"Always runs\");\n}""","bullets":["finally runs whether try succeeded or failed.","It is useful for cleanup.","Many modern APIs handle cleanup with using, but finally is important conceptually."]},
{"title":"Do Not Catch Everything Silently","bullets":["A catch block should not hide every problem without explanation.","Show useful beginner messages.","Catch specific exception types when possible.","Silent failure makes debugging confusing."]},
{"title":"LINQ: Why It Helps","bullets":["LINQ means Language Integrated Query.","It lets us ask questions about collections clearly.","Today we use a small practical subset only.","LINQ becomes very important again with Entity Framework Core."]},
{"title":"Where","code":"""List<Dictionary<string, string>> foodExpenses = expenses\n    .Where(expense => expense[\"category\"] == \"Food\")\n    .ToList();""","bullets":["Where filters a collection.","The expression decides which items stay.","ToList turns the query result into a list."]},
{"title":"Select","code":"""List<string> names = expenses\n    .Select(expense => expense[\"name\"])\n    .ToList();""","bullets":["Select transforms each item into another value.","Here each expense becomes only its name.","This is useful for projections and display data."]},
{"title":"FirstOrDefault","code":"""Dictionary<string, string> firstFood = expenses\n    .FirstOrDefault(expense => expense[\"category\"] == \"Food\");""","bullets":["FirstOrDefault returns the first matching item.","If nothing matches, it returns null for reference types.","Check for null before using the result."]},
{"title":"Any","code":"""bool hasFood = expenses\n    .Any(expense => expense[\"category\"] == \"Food\");""","bullets":["Any answers a yes/no question.","It is useful for checks such as exists, has items, has category.","The result is bool."]},
{"title":"OrderBy","code":"""List<Dictionary<string, string>> ordered = expenses\n    .OrderBy(expense => expense[\"name\"])\n    .ToList();""","bullets":["OrderBy sorts items by a selected value.","It does not change the original list unless you store the result.","Sorting makes list output easier to scan."]},
{"title":"Live Demo Menu","code":"""1. Add expense\n2. List expenses\n3. Search by name\n4. Filter by category\n5. Delete expense\n6. Show summary\n7. Save\n8. Exit""","bullets":["The demo loads JSON on startup.","Add and delete automatically save JSON.","Search, filter, order, and summary use LINQ."]},
{"title":"Demo Data File","code":"""data/expenses.json""","bullets":["The file is placed in a data folder under the current working directory.","The teacher can open the JSON file and show the saved structure.","Editing JSON manually is possible, but invalid JSON should be handled."]},
{"title":"Student Project Increment","bullets":["Add automatic load when the application starts.","Add save after meaningful changes.","Store data in a JSON file.","Use try/catch for missing file, invalid JSON, and invalid input.","Add searching or filtering with LINQ.","Keep the project single-user and console-based.","After this lecture, save Milestone 01 - Basic Console."]},
{"title":"Milestone 01 - Basic Console","bullets":["Console menu exists.","Conditions and loops are used.","Methods organize repeated work.","Collections store multiple items.","Basic CRUD-like behaviour exists: Add, List, Find, Delete.","Data is saved to a JSON file.","The student can explain what changed from Lecture 01 to Lecture 04."]},
{"title":"What Not To Add Yet","bullets":["Do not add classes yet unless the teacher explicitly asks for preview work.","Do not add interfaces or repositories yet.","Do not add users or login yet.","Do not add SQL or EF Core yet.","Do not build a Web API yet.","The goal is persistence and basic collection querying."]},
{"title":"Common Mistakes Today","bullets":["Saving to a path without creating the folder first.","Assuming the file always exists.","Assuming JSON is always valid.","Catching an exception but not telling the user what happened.","Forgetting that deserialization can return null.","Using LINQ before understanding the equivalent loop.","Trying to make the JSON structure too advanced before classes."]},
{"title":"Exercises","bullets":["Add a command that shows only expenses above a chosen amount.","Add a command that prints all unique categories.","Make the app show where the JSON file is stored.","Intentionally break the JSON file and observe the error handling.","Use OrderBy to sort by category or name.","Write a short Milestone 01 explanation for your project."]},
{"title":"Review Questions","bullets":["What is the difference between memory and persistent storage?","Why do we create the data directory before saving?","What is JSON?","What is serialization?","What is deserialization?","What is an exception?","Why should catch blocks be specific?","What does Where do?","What does Select do?","When should the student save Milestone 01?"]},
{"title":"Summary","bullets":["Files let data survive program restart.","JSON stores structured text and prepares students for APIs.","System.Text.Json converts C# data to JSON and back.","Exceptions let programs handle runtime problems.","LINQ gives readable collection queries.","Lecture 04 completes the first Basic Console milestone."]},
]

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
    c.setFont("Helvetica",9); c.setFillColor(MUTED); c.drawRightString(W-0.65*inch,0.35*inch,f"Lecture 04 | {page}/{total}")

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
        if lines >= 9: code_height = min(code_height, 3.65*inch)
        else: code_height = min(code_height, 3.10*inch)
        font_size = 13 if lines >= 9 else 15
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
