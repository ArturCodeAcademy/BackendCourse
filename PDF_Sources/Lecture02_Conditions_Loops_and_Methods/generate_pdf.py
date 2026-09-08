from pathlib import Path
import textwrap
from reportlab.lib import colors
from reportlab.lib.pagesizes import landscape
from reportlab.lib.units import inch
from reportlab.pdfgen import canvas
from reportlab.pdfbase.pdfmetrics import stringWidth

ROOT = Path(__file__).resolve().parents[2]
OUTPUT = ROOT / "PDF" / "Lecture02_Conditions_Loops_and_Methods.pdf"
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
    {"kind":"title","title":"Lecture 02","subtitle":"Conditions, Loops and Methods","body":["Basics of Internet Technologies 2","From a linear program to an interactive console app"]},
    {"title":"Learning Objectives","bullets":["Use boolean values to represent yes/no facts.","Compare values with ==, !=, >, <, >=, and <=.","Combine conditions with &&, ||, and !.","Use if, else if, else, and switch.","Repeat code with while, for, and foreach.","Build a simple menu loop.","Split code into methods with parameters and return values.","Apply simple validation to console input."]},
    {"title":"Connection to Lecture 01","bullets":["Lecture 01 created a linear program: read values, convert them, calculate, print, end.","That is useful, but real applications need choices and repetition.","Today the program can respond to commands and keep running.","We still avoid classes, files, JSON, users, databases, and web APIs."]},
    {"title":"Lesson Timing Plan","bullets":["10 min - recap and problem with linear programs.","15 min - bool, comparisons, and logical operators.","20 min - if, else if, else, switch, and validation decisions.","20 min - while, for, foreach, and common loop mistakes.","25 min - methods, parameters, return values, void, and scope.","20 min - live coding the expense menu.","10 min - student increment, exercises, questions."]},
    {"title":"Problem: The Program Ends Too Quickly","code":"""Expense name: Coffee\nAmount: 4.50\n\nYou spent 4.50 EUR on Coffee.\nProgram ends.""","bullets":["The user can add only one item.","There is no command choice.","There is no way to show the result again.","Invalid input is difficult to recover from."]},
    {"title":"Target Shape for Today","code":"""while application is running\n{\n    show menu\n    read command\n    execute selected operation\n}""","bullets":["This is the base shape of many console applications.","The command controls what path the program takes.","The loop keeps the application alive."]},
    {"title":"Boolean Values","code":"""bool isRunning = true;\nbool hasExpense = false;\nbool amountIsValid = true;""","bullets":["bool stores true or false.","Conditions use bool values to choose a path.","A bool variable should usually sound like a yes/no fact."]},
    {"title":"Comparison Operators","bullets":["== checks whether two values are equal.","!= checks whether two values are not equal.","> checks greater than.","< checks less than.",">= checks greater than or equal.","<= checks less than or equal.","Every comparison produces true or false."]},
    {"title":"Comparison Example","code":"""int balance = 100;\nint withdrawal = 40;\n\nbool hasEnoughMoney = balance >= withdrawal;\nbool tooLarge = withdrawal > balance;""","bullets":["The values are numbers, but the comparison result is bool.","These bool results can be used in if statements.","Readable bool names make conditions easier to understand."]},
    {"title":"Logical Operators","code":"""bool canWithdraw = hasEnoughMoney && cardIsActive;\nbool needsHelp = !cardIsActive || withdrawal > 500;""","bullets":["&& means both sides must be true.","|| means at least one side must be true.","! reverses true to false or false to true.","Use parentheses when a condition becomes difficult to read."]},
    {"title":"Truth Table: AND","columns":[("Expression",["true && true","true && false","false && true","false && false"]),("Result",["true","false","false","false"])]},
    {"title":"Truth Table: OR","columns":[("Expression",["true || true","true || false","false || true","false || false"]),("Result",["true","true","true","false"])]},
    {"title":"if Statement","code":"""if (amount > 0)\n{\n    Console.WriteLine(\"Amount accepted.\");\n}""","bullets":["The condition is inside parentheses.","The code block runs only when the condition is true.","Braces make the controlled block clear."]},
    {"title":"if and else","code":"""if (amount > 0)\n{\n    Console.WriteLine(\"Amount accepted.\");\n}\nelse\n{\n    Console.WriteLine(\"Amount must be positive.\");\n}""","bullets":["else handles the alternative path.","Only one of the two blocks runs.","This is the basic shape of validation feedback."]},
    {"title":"else if","code":"""if (amount <= 0)\n{\n    Console.WriteLine(\"Amount must be positive.\");\n}\nelse if (amount > 1000)\n{\n    Console.WriteLine(\"Large amount. Check again.\");\n}\nelse\n{\n    Console.WriteLine(\"Amount accepted.\");\n}""","bullets":["else if checks another condition only if the previous condition was false.","The order of conditions matters.","Use it when one decision has several possible outcomes."]},
    {"title":"Nested if","code":"""if (amountIsNumber)\n{\n    if (amount > 0)\n    {\n        Console.WriteLine(\"Accepted\");\n    }\n}""","bullets":["A nested if is an if inside another if.","It is sometimes useful, but too much nesting becomes hard to read.","Later we will learn patterns that reduce deep nesting."]},
    {"title":"switch","code":"""switch (command)\n{\n    case \"1\":\n        Console.WriteLine(\"Add expense\");\n        break;\n    case \"2\":\n        Console.WriteLine(\"Show expense\");\n        break;\n    default:\n        Console.WriteLine(\"Unknown command\");\n        break;\n}""","bullets":["switch is useful when one value is compared with several fixed cases.","Each case should normally end with break.","default handles unsupported values."]},
    {"title":"Choosing if or switch","columns":[("Use if",["Complex conditions","Ranges such as amount > 100","Several variables","Validation logic"]),("Use switch",["One command value","Fixed options","Menu choices","Known labels"])]},
    {"title":"while Loop","code":"""while (isRunning)\n{\n    Console.WriteLine(\"Menu\");\n}""","bullets":["while repeats while the condition is true.","If the condition never becomes false, the loop does not stop.","Menu applications often use while with an isRunning variable."]},
    {"title":"Stopping a while Loop","code":"""bool isRunning = true;\n\nwhile (isRunning)\n{\n    string command = Console.ReadLine();\n\n    if (command == \"3\")\n    {\n        isRunning = false;\n    }\n}""","bullets":["The loop condition is checked before each iteration.","Changing isRunning to false lets the program leave the loop.","This is clearer for beginners than clever loop tricks."]},
    {"title":"for Loop","code":"""for (int index = 0; index < 3; index++)\n{\n    Console.WriteLine(index);\n}""","bullets":["A for loop is useful when the number of repetitions is known.","It has initialization, condition, and update parts.","It is common when working with array indexes."]},
    {"title":"foreach Loop","code":"""string[] commands = { \"add\", \"show\", \"exit\" };\n\nforeach (string command in commands)\n{\n    Console.WriteLine(command);\n}""","bullets":["foreach visits each value in a collection or array.","It is easier when the index is not needed.","Lecture 03 will use foreach with List<T>."]},
    {"title":"Loop Comparison","columns":[("Loop",["while","for","foreach"]),("Best Use",["Repeat until a condition changes","Repeat with an index or fixed count","Read each item in a collection"])]},
    {"title":"Common Loop Mistakes","bullets":["Forgetting to change the condition in a while loop.","Using <= instead of < with array indexes.","Changing a loop variable in two places.","Putting input outside the loop when it must happen every iteration.","Making one loop do too many unrelated jobs."]},
    {"title":"Live Demo Menu","code":"""1. Add expense\n2. Show current expense\n3. Exit""","bullets":["The demo stores one current expense.","The user can repeat commands.","Validation rejects empty names and invalid amounts.","This is still intentionally simple: no collections and no files yet."]},
    {"title":"Demo State Variables","code":"""string expenseName = \"\";\ndecimal expenseAmount = 0m;\nbool hasExpense = false;\nbool isRunning = true;""","bullets":["State means values the program remembers while running.","hasExpense tells whether showing an expense makes sense.","isRunning controls the menu loop."]},
    {"title":"Reading a Command","code":"""Console.Write(\"Choose command: \" );\nstring command = Console.ReadLine();""","bullets":["The command arrives as text.","For a beginner menu, text commands such as \"1\" are simple and visible.","switch can decide what each command means."]},
    {"title":"Validating Input","code":"""bool amountIsValid = decimal.TryParse(\n    amountText,\n    CultureInfo.InvariantCulture,\n    out decimal enteredAmount);""","bullets":["TryParse prevents the program from crashing on invalid input.","The bool result tells whether conversion succeeded.","The converted value is available after successful conversion."]},
    {"title":"Validation Conditions","code":"""if (enteredName == \"\")\n{\n    Console.WriteLine(\"Expense name cannot be empty.\");\n}\nelse if (!amountIsValid)\n{\n    Console.WriteLine(\"Amount must be a number.\");\n}\nelse if (enteredAmount <= 0)\n{\n    Console.WriteLine(\"Amount must be greater than zero.\");\n}""","bullets":["Each invalid case gets a clear message.","The order avoids using enteredAmount before checking conversion.","This is a first step toward reliable backend validation."]},
    {"title":"Methods: Why They Exist","bullets":["A method gives a name to a block of code.","A method can receive input through parameters.","A method can return a result.","Methods reduce repetition and make Program.cs easier to read.","Today we use methods inside one file. Later, classes will organize methods into objects."]},
    {"title":"void Method","code":"""void PrintSeparator()\n{\n    Console.WriteLine(\"------------------------------\");\n}""","bullets":["void means the method does not return a value.","This method performs an action: printing.","Call it by writing PrintSeparator();"]},
    {"title":"Method with Parameters","code":"""void PrintExpense(string name, decimal amount)\n{\n    Console.WriteLine(name + \": \" + amount + \" EUR\");\n}""","bullets":["Parameters are values the caller passes into the method.","Inside the method, parameters behave like local variables.","Different calls can pass different values."]},
    {"title":"Method with Return Value","code":"""decimal CalculateTotal(decimal price, int quantity)\n{\n    return price * quantity;\n}""","bullets":["The return type appears before the method name.","return sends a value back to the caller.","A calculation method should usually return a value instead of printing directly."]},
    {"title":"Boolean Method for Validation","code":"""bool IsPositiveAmount(decimal amount)\n{\n    return amount > 0;\n}""","bullets":["A bool method is useful for a yes/no question.","The method name should read like a condition.","This makes if statements easier to read."]},
    {"title":"Variable Scope","code":"""int outerNumber = 10;\n\nif (outerNumber > 0)\n{\n    int innerNumber = 5;\n}\n\nConsole.WriteLine(outerNumber);""","bullets":["A variable exists inside the block where it is declared.","innerNumber exists only inside the if block.","Scope prevents unrelated code from using temporary values by accident."]},
    {"title":"Parameters vs Local Variables","columns":[("Parameters",["Declared in method parentheses","Provided by the caller","Represent input to a method"]),("Local Variables",["Declared inside a block","Created while that block runs","Used for temporary work"])]},
    {"title":"Returning vs Printing","columns":[("Return",["Gives a value back","Good for calculations","Can be reused by other code"]),("Print",["Shows text to the user","Good for console UI","Harder to reuse for calculations"])]},
    {"title":"Student Project Increment","bullets":["Add a menu loop to the project from Lecture 01.","Create at least three commands.","Use if/else or switch to choose commands.","Use while to keep the application running.","Move repeated code into methods.","Add simple validation for empty text and invalid numbers.","Keep the application single-user for now."]},
    {"title":"Student Example Commands","columns":[("Project",["Expense Tracker","Task Manager","ATM Simulator","Veterinary Clinic"]),("Commands",["Add expense, show current expense, exit","Add task, show task, exit","Deposit, show balance, exit","Add pet, show current pet, exit"])]},
    {"title":"What Not To Add Yet","bullets":["Do not add JSON files yet.","Do not add classes yet.","Do not add users or login yet.","Do not add databases yet.","Do not build a Web API yet.","The goal is to master decisions, repetition, methods, and validation in a console program."]},
    {"title":"Common Mistakes Today","bullets":["Writing all menu logic in one huge block without methods.","Forgetting break in switch cases.","Creating an infinite loop by never changing isRunning.","Parsing numbers before checking whether conversion succeeded.","Using a method that prints when a method should return a value.","Making Lecture 02 more complex than the student can explain."]},
    {"title":"Exercises","bullets":["Add one new command to the demo menu.","Create a method that prints the menu.","Create a method that validates a positive decimal amount.","Rewrite one if/else decision as a switch if it naturally fits.","Add a for loop that prints numbers from 1 to 5.","Add a foreach loop over an array of command names."]},
    {"title":"Review Questions","bullets":["What is the difference between = and ==?","When should we use &&?","When is while better than for?","When is foreach simpler than for?","What does void mean?","What is a parameter?","What is a return value?","Why does variable scope matter?"]},
    {"title":"Summary","bullets":["Conditions let a program choose between paths.","Loops let a program repeat work.","switch is useful for simple menu commands.","while is useful for application loops.","Methods give names to actions and calculations.","Validation is the first step toward reliable backend behaviour.","The student project now becomes interactive, but still intentionally simple."]},
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
    c.setFont("Helvetica",9); c.setFillColor(MUTED); c.drawRightString(W-0.65*inch,0.35*inch,f"Lecture 02 | {page}/{total}")

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
