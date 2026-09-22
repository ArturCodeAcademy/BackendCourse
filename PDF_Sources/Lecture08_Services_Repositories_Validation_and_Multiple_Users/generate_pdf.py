from pathlib import Path
from reportlab.lib.colors import HexColor, white
from reportlab.lib.pagesizes import landscape
from reportlab.pdfbase.pdfmetrics import stringWidth
from reportlab.pdfgen import canvas

OUT = Path(r"D:\Projects\BackendCourse\PDF\Lecture08_Services_Repositories_Validation_and_Multiple_Users.pdf")
OUT.parent.mkdir(parents=True, exist_ok=True)
W, H = landscape((720, 405))

NAVY = HexColor("#102A43")
BLUE = HexColor("#1877A8")
TEAL = HexColor("#0B8F7A")
ORANGE = HexColor("#D87919")
RED = HexColor("#C53B3B")
INK = HexColor("#17324D")
MUTED = HexColor("#536779")
PALE = HexColor("#EAF2F7")
GREENPALE = HexColor("#E8F5F1")
CODE_BG = HexColor("#17212B")
CODE_TEXT = HexColor("#E9F4FA")

slides = []

def add(title, kind, body, note=""):
    slides.append((title, kind, body, note))

add("Services, Repositories, Validation\nand Multiple Users", "title",
    ["Lecture 08 | Backend Development with C#",
     "Build a layered, persistent console application where each user can see only their own expenses.",
     "Milestone 03: Layered Multi-user Console Application"],
    "Three-hour lesson. Continue the Expense Tracker from Lecture 07.")

add("Today: a complete user story", "bullets",
    ["Register a user without saving their password in plain text.",
     "Log in and remember the current user for the console session.",
     "Add, list, total and delete expenses only for that user.",
     "Persist users and expenses in separate JSON files.",
     "Keep rules in services, file details in repositories, and prompts in Presentation."],
    "The goal is one coherent program, not four folders for decoration.")

add("Lesson route", "timeline",
    ["00:00-00:20  Why Lecture 07 layers need services and repositories",
     "00:20-01:00  Domain and Application: rules, validation, ownership",
     "01:00-01:35  Infrastructure: JSON repositories and PBKDF2",
     "01:35-02:20  Presentation and multi-user workflow",
     "02:20-03:00  Guided build, Alice/Bob security checks, extension"],
    "Pause after the password section for a short break.")

add("The new problem", "compare",
    ["One-user version: every expense in one list.", "Multi-user version: every expense has an owner.",
     "Question: where should the rule 'Bob cannot remove Alice's expense' live?",
     "Answer: in the application boundary, enforced by repository queries that include UserId."],
    "A menu can hide a button, but a rule must still hold even when another caller reaches the service.")

add("Why one Program.cs stops scaling", "bullets",
    ["Registration, input conversion, validation, JSON, menus and security have different reasons to change.",
     "Mixing them makes code difficult to test and dangerous to copy.",
     "A layer is a responsibility boundary, not an extra ceremony.",
     "The payoff: replace JSON with SQL later without rewriting login and expense rules."],
    "Tie this directly to the spaghetti-code example in Lecture 07.")

add("Solution layout", "tree",
    ["Lecture08_Services_Repositories_Validation_and_Multiple_Users",
     "  Layers",
     "    Domain          User and Expense data",
     "    Application     service rules and repository contracts",
     "    Infrastructure  JSON files and PBKDF2 hashing",
     "    Presentation    console menus and input conversion",
     "  Lecture08.CodeExamples",
     "  Project          composition root and executable app"],
    "The Layers node is a solution folder. Each layer is its own class-library project.")

add("The rule for each layer", "table",
    [["Domain", "Business nouns and their properties", "Console, JSON, cryptography"],
     ["Application", "Use cases, validation, interfaces", "File.ReadAllText, menu loops"],
     ["Infrastructure", "JSON and PBKDF2 implementations", "Menu decisions"],
     ["Presentation", "Questions, TryParse, output", "Persistence and rules"],
     ["Project", "Wiring concrete implementations", "Duplicate business logic"]],
    "Ask students to name a real reason each layer might change.")

add("Dependency direction", "diagram",
    ["Presentation -> Application -> Domain",
     "Infrastructure -> Application + Domain",
     "Project references Application + Infrastructure + Presentation",
     "Domain references nothing in this solution."],
    "Application uses interfaces. Infrastructure implements them. The Project chooses the implementation.")

add("Domain: User", "code",
    ['public class User', '{', '    public int Id { get; set; }',
     '    public string Username { get; set; } = string.Empty;',
     '    public string PasswordHash { get; set; } = string.Empty;',
     '    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;', '}'],
    "PasswordHash is data needed by the application. A Password property must never be persisted.")

add("Domain: Expense and ownership", "code",
    ['public class Expense', '{', '    public int Id { get; set; }',
     '    public int UserId { get; set; }',
     '    public string Name { get; set; } = string.Empty;',
     '    public decimal Amount { get; set; }',
     '    public string Category { get; set; } = string.Empty;', '}'],
    "UserId is a foreign-key idea before we use a database. It connects an expense to its owner.")

add("Data model before the database", "diagram",
    ["User: Id 1, Username alice, PasswordHash ...",
     "User: Id 2, Username bob, PasswordHash ...",
     "Expense: Id 1, UserId 1, Lunch, 12.50",
     "Expense: Id 2, UserId 1, Train, 4.20",
     "Expense: Id 3, UserId 2, Book, 19.00"],
    "Later this becomes Users(Id) and Expenses(UserId) in the shared Database course.")

add("Repository contracts are promises", "code",
    ['public interface IUserRepository', '{', '    List<User> GetAll();',
     '    User? GetByUsername(string username);', '    void Add(User user);', '}',
     '', 'public interface IExpenseRepository', '{',
     '    List<Expense> GetByUserId(int userId);',
     '    bool DeleteByIdForUser(int expenseId, int userId);', '}'],
    "Interfaces describe what Application needs. They do not reveal JSON or a future SQL table.")

add("Why GetByUserId is better than GetAll", "compare",
    ["Risky: service receives every expense, then hopes to filter correctly.",
     "Focused: repository receives userId and returns only matching records.",
     "DeleteByIdForUser checks both conditions in one operation.",
     "This is defense in depth: the service and repository both express ownership."],
    "In a later SQL repository, this becomes WHERE Id = @id AND UserId = @userId.")

add("Service: one job, one language", "bullets",
    ["AuthService speaks about Register and Login.",
     "ExpenseService speaks about AddForUser, GetForUser and DeleteForUser.",
     "Services coordinate domain data and repository contracts.",
     "Services return a result and a clear message; Presentation decides how to print it.",
     "Services do not call Console.ReadLine or File.ReadAllText."],
    "A service is where a use case is named and protected.")

add("Registration flow", "flow",
    ["Presentation reads username and password.",
     "AuthService trims input and validates its rules.",
     "AuthService asks IUserRepository whether the username is already used.",
     "IPasswordService creates a protected record.",
     "AuthService creates User and calls users.Add(user).",
     "JsonUserRepository serializes the user list."],
    "Each arrow crosses a responsibility boundary intentionally.")

add("Validation belongs in Application", "code",
    ['username = username?.Trim() ?? string.Empty;', 'password ??= string.Empty;', '',
     'if (username.Length < 3)', '{',
     '    message = "Username must contain at least 3 characters.";',
     '    return false;', '}',
     'if (password.Length < 8)', '{',
     '    message = "Password must contain at least 8 characters.";',
     '    return false;', '}'],
    "Presentation may give friendly hints, but the service must enforce the rule for every caller.")

add("Validation: precise responsibilities", "table",
    [["Presentation", "Can raw text become decimal or int?", "TryParse"],
     ["Application", "Is this value acceptable for the use case?", "amount > 0"],
     ["Repository", "Can the operation be stored or queried?", "JSON read and write"],
     ["Domain", "What does the data represent?", "Expense.UserId"]],
    "Do not confuse parsing with validation. 12x is a conversion problem; -12 is a business-rule problem.")

add("Expense service: create for current user", "code",
    ['if (currentUser is null) return false;', 'if (name.Length < 2) return false;',
     'if (amount <= 0) return false;', 'if (category.Length == 0) return false;', '',
     'expenses.Add(new Expense', '{', '    Id = nextId,', '    UserId = currentUser.Id,',
     '    Name = name, Amount = amount, Category = category', '});'],
    "The client never chooses UserId. The authenticated session supplies it.")

add("Expense service: delete safely", "code",
    ['public bool DeleteForUser(User? currentUser, int expenseId,', '    out string message)', '{',
     '    if (currentUser is null) return false;', '',
     '    if (expenses.DeleteByIdForUser(expenseId, currentUser.Id))',
     '        return true;', '',
     '    message = "Expense was not found in your account.";',
     '    return false;', '}'],
    "A missing record and someone else's record receive the same response. Do not reveal extra information.")

add("Passwords: what must never happen", "compare",
    ["Never store: password = summer2026",
     "Never log the entered password.",
     "Do not use one fast SHA-256 hash by itself for user passwords.",
     "Do not use one global fixed salt.",
     "Do not invent your own crypto algorithm."],
    "Use the platform implementation: Rfc2898DeriveBytes.Pbkdf2.")

add("Hashing is not encryption", "compare",
    ["Encryption: designed to be reversed with a key.", "Password hashing: one-way verification.",
     "Login does not recover the password.", "Login derives a new candidate and compares it.",
     "A slow password-hashing function makes guessing attacks more expensive."],
    "Students often use these words interchangeably. Stop and distinguish them here.")

add("Salt: why identical passwords differ", "diagram",
    ["Password: correct-horse-battery-staple",
     "First registration  -> random salt A -> derived hash A",
     "Second registration -> random salt B -> derived hash B",
     "Both records verify the same entered password.",
     "The salt is stored; it is not secret."],
    "A unique random salt prevents precomputed hash tables from applying across all users.")

add("PBKDF2 record format", "code",
    ['PBKDF2-SHA256$210000$base64-salt$base64-derived-hash', '',
     'part 1: algorithm label', 'part 2: number of iterations', 'part 3: random salt',
     'part 4: derived hash'],
    "Keeping parameters in the record lets a future version verify old records and upgrade new ones.")

add("Create a PBKDF2 record", "code",
    ['byte[] salt = RandomNumberGenerator.GetBytes(16);',
     'byte[] hash = Rfc2898DeriveBytes.Pbkdf2(',
     '    password, salt, 210_000,',
     '    HashAlgorithmName.SHA256, 32);',
     '',
     'return algorithm + "$" + iterations + "$" +',
     '    Convert.ToBase64String(salt) + "$" +',
     '    Convert.ToBase64String(hash);'],
    "210,000 is a deliberately slow work factor in this learning example. It is not a magic forever-number.")

add("Verify without recovering a password", "code",
    ['string[] parts = passwordHash.Split("$");',
     'byte[] salt = Convert.FromBase64String(parts[2]);',
     'byte[] storedHash = Convert.FromBase64String(parts[3]);',
     'byte[] candidateHash = Rfc2898DeriveBytes.Pbkdf2(',
     '    password, salt, iterations,',
     '    HashAlgorithmName.SHA256, storedHash.Length);',
     'return CryptographicOperations.FixedTimeEquals(',
     '    candidateHash, storedHash);'],
    "FixedTimeEquals avoids a simple early-exit comparison that can leak timing information.")

add("Login flow and neutral errors", "flow",
    ["Read username and password.",
     "Find a user by username.",
     "If no user exists, fail with the same answer as a wrong password.",
     "If a user exists, verify password against PasswordHash.",
     "On success, set currentUser for this console session."],
    "Invalid username or password is intentionally less specific. It avoids username enumeration.")

add("Infrastructure: JSON repository", "code",
    ['public List<Expense> GetByUserId(int userId) =>',
     '    expenses.Where(expense => expense.UserId == userId).ToList();',
     '',
     'public bool DeleteByIdForUser(int expenseId, int userId)',
     '{', '    Expense? expense = expenses.FirstOrDefault(item =>',
     '        item.Id == expenseId && item.UserId == userId);',
     '    if (expense is null) return false;',
     '    expenses.Remove(expense); Save(); return true;', '}'],
    "Infrastructure knows the in-memory list and JSON file. It does not decide which menu is available.")

add("Two JSON files, two concerns", "tree",
    ["data/",
     "  users.json     Id, Username, PasswordHash, CreatedAt",
     "  expenses.json  Id, UserId, Name, Amount, Category, CreatedAt",
     "The original password appears in neither file.",
     "The files are an educational persistence step before SQL."],
    "Open users.json after the demo. Students should recognize the PBKDF2 record.")

add("Presentation: convert, then delegate", "code",
    ['bool isAmountValid = decimal.TryParse(',
     '    rawAmount, NumberStyles.Number,',
     '    CultureInfo.InvariantCulture, out decimal amount);',
     'if (!isAmountValid)', '{',
     '    Console.WriteLine("Amount must be a number, for example 12.50.");',
     '    return;', '}',
     'expenseService.AddForUser(currentUser, name, amount, category, out message);'],
    "Console input is visible in this learning app. A web interface later uses masked password controls and HTTPS.")

add("Project: the composition root", "code",
    ['IUserRepository users = new JsonUserRepository(usersFile);',
     'IExpenseRepository expenses = new JsonExpenseRepository(expensesFile);',
     'IPasswordService passwordService = new PasswordService();',
     '',
     'var authService = new AuthService(users, passwordService);',
     'var expenseService = new ExpenseService(expenses);',
     'new ConsoleMenu(authService, expenseService).Run();'],
    "This is the only place that knows the concrete JSON classes and PasswordService.")

add("Alice and Bob: acceptance scenario", "flow",
    ["1. Register Alice. Add Lunch for 12.50.",
     "2. Log out. Register Bob. Log in as Bob.",
     "3. Bob lists expenses: the list is empty.",
     "4. Bob tries Alice's expense id: deletion fails.",
     "5. Inspect users.json: PasswordHash exists; Password does not.",
     "6. Try short password and zero amount: both fail."],
    "This is a powerful live demo because it checks the behavior a user cares about.")

add("Common mistakes and repairs", "table",
    [["Rule in menu only", "Move it to a service"],
     ["Read all data in every caller", "Ask repository for user-scoped data"],
     ["Password in JSON", "Store only PBKDF2 record"],
     ["One fixed salt", "Generate random salt per password"],
     ["Convert with decimal.Parse", "Use TryParse at the UI boundary"],
     ["Specific login error", "Use a neutral login error"]],
    "Students should be able to explain why each repair belongs where it does.")

add("Guided build sequence", "timeline",
    ["Create Domain classes and add UserId to Expense.",
     "Write interfaces in Application before JSON classes exist.",
     "Implement AuthService validation with a fake or empty repository.",
     "Implement PasswordService and run CodeExamples.",
     "Implement JSON repositories.",
     "Wire Project and run the Alice/Bob checks."],
    "This order reduces cognitive load: contracts and rules first, details later.")

add("Independent practice", "bullets",
    ["Add a maximum expense amount and explain which layer owns the rule.",
     "Add a Category enum or a category whitelist in ExpenseService.",
     "Add a change-password use case: verify old password before writing a new hash.",
     "Add a FindMyExpensesByCategory method that still takes the current user.",
     "Write a short layer audit: for five lines of code, state why each file owns that line."],
    "Do not add an administrator feature until normal ownership works.")

add("Database connection for the other subject", "diagram",
    ["Now: User.Id -> Expense.UserId in C# and JSON.",
     "Database course: Users table, Expenses table, primary key, foreign key.",
     "Lecture 09: replace JSON repository internals with SQL/Dapper.",
     "Application services should remain almost unchanged.",
     "That stability is the architectural payoff."],
    "This makes the shared homework coherent across both subjects.")

add("Milestone 03: what to submit", "bullets",
    ["A solution with the exact Layers, CodeExamples and Project structure.",
     "Working registration and login.",
     "PasswordHash stored with PBKDF2, random salt and verification.",
     "At least one user-owned entity with list and delete operations.",
     "A short README with the Alice/Bob proof steps and a JSON screenshot with no plain password."],
    "This can become the evolution portfolio checkpoint.")

add("Exit ticket", "bullets",
    ["Why does Application depend on IUserRepository rather than JsonUserRepository?",
     "Which layer should parse text into decimal?",
     "Why is a salt stored even though it is not secret?",
     "What exact condition prevents Bob from deleting Alice's record?",
     "What is the next swap when the database course reaches SQL?"],
    "Next lesson: SQL access with Dapper. The repository interface stays; its implementation changes.")

def wrapped(c, text, font, size, maxw):
    words = text.split()
    lines, cur = [], ""
    for word in words:
        trial = word if not cur else cur + " " + word
        if stringWidth(trial, font, size) <= maxw:
            cur = trial
        else:
            lines.append(cur)
            cur = word
    if cur:
        lines.append(cur)
    return lines

def header(c, number, title):
    c.setFillColor(NAVY)
    c.rect(0, H - 54, W, 54, fill=1, stroke=0)
    c.setFillColor(white)
    c.setFont("Helvetica-Bold", 20)
    y = H - 34
    for line in wrapped(c, title, "Helvetica-Bold", 20, W - 100):
        c.drawString(34, y, line)
        y -= 21
    c.setFillColor(HexColor("#B9D9E8"))
    c.setFont("Helvetica-Bold", 10)
    c.drawRightString(W - 28, H - 33, f"{number:02d} / {len(slides):02d}")

def footer(c, note):
    c.setStrokeColor(HexColor("#C8D8E1"))
    c.line(34, 24, W - 34, 24)
    c.setFillColor(MUTED)
    c.setFont("Helvetica", 8)
    c.drawString(34, 12, "Backend Development with C# | Lecture 08")
    if note:
        c.drawRightString(W - 34, 12, note[:85])

def bullets(c, items):
    y = H - 90
    for item in items:
        c.setFillColor(BLUE)
        c.circle(51, y + 4, 3, fill=1, stroke=0)
        c.setFillColor(INK)
        c.setFont("Helvetica", 15)
        for line in wrapped(c, item, "Helvetica", 15, W - 115):
            c.drawString(68, y, line)
            y -= 20
        y -= 10

def code(c, lines):
    c.setFillColor(CODE_BG)
    c.roundRect(34, 52, W - 68, H - 128, 6, fill=1, stroke=0)
    c.setFillColor(CODE_TEXT)
    c.setFont("Courier", 12)
    y = H - 92
    for line in lines:
        c.drawString(55, y, line[:88])
        y -= 18

def tree(c, items):
    c.setFillColor(PALE)
    c.roundRect(42, 55, W - 84, H - 130, 6, fill=1, stroke=0)
    c.setFillColor(INK)
    c.setFont("Courier", 14)
    y = H - 95
    for index, item in enumerate(items):
        c.setFillColor(TEAL if index in (0, 1) else INK)
        c.drawString(68, y, item)
        y -= 25

def table(c, rows):
    x = 42
    y = H - 92
    cols = [135, 265, 235] if len(rows[0]) == 3 else [275, 360]
    row_h = min(52, (H - 145) / len(rows))
    for r, row in enumerate(rows):
        left = x
        fill = GREENPALE if r % 2 == 0 else PALE
        for col, cell in zip(cols, row):
            c.setFillColor(fill)
            c.rect(left, y - row_h, col, row_h, fill=1, stroke=0)
            c.setFillColor(INK)
            c.setFont("Helvetica-Bold" if r == 0 else "Helvetica", 11)
            ly = y - 17
            for line in wrapped(c, cell, "Helvetica", 11, col - 16)[:3]:
                c.drawString(left + 8, ly, line)
                ly -= 13
            left += col
        y -= row_h

def compare(c, items):
    left, right = 44, 378
    c.setFillColor(HexColor("#FFF3E8"))
    c.roundRect(left, 73, 295, 220, 7, fill=1, stroke=0)
    c.setFillColor(GREENPALE)
    c.roundRect(right, 73, 295, 220, 7, fill=1, stroke=0)
    c.setFillColor(ORANGE)
    c.setFont("Helvetica-Bold", 16)
    c.drawString(left + 18, 266, "Problem / risk")
    c.setFillColor(TEAL)
    c.drawString(right + 18, 266, "Better boundary")
    for colx, text, color in [(left, items[0], INK), (right, items[1], INK)]:
        c.setFillColor(color)
        c.setFont("Helvetica", 14)
        yy = 235
        for line in wrapped(c, text, "Helvetica", 14, 255):
            c.drawString(colx + 18, yy, line)
            yy -= 20
    c.setFillColor(MUTED)
    c.setFont("Helvetica-Oblique", 12)
    yy = 180
    for line in wrapped(c, items[2], "Helvetica-Oblique", 12, W - 110):
        c.drawCentredString(W / 2, yy, line)
        yy -= 17
    if len(items) > 3:
        for line in wrapped(c, items[3], "Helvetica-Oblique", 12, W - 110):
            c.drawCentredString(W / 2, yy, line)
            yy -= 17

def flow(c, items):
    y = H - 90
    for i, item in enumerate(items):
        c.setFillColor(GREENPALE if i % 2 == 0 else PALE)
        c.roundRect(80, y - 32, W - 160, 42, 6, fill=1, stroke=0)
        c.setFillColor(TEAL if i % 2 == 0 else BLUE)
        c.setFont("Helvetica-Bold", 12)
        c.drawString(96, y - 7, str(i + 1))
        c.setFillColor(INK)
        c.setFont("Helvetica", 13)
        c.drawString(125, y - 7, item[:72])
        if i < len(items) - 1:
            c.setFillColor(MUTED)
            c.setFont("Helvetica-Bold", 15)
            c.drawCentredString(W / 2, y - 47, "v")
        y -= 57

def diagram(c, items):
    y = H - 96
    colors = [BLUE, TEAL, ORANGE, BLUE, TEAL]
    for i, item in enumerate(items):
        c.setFillColor(colors[i % len(colors)])
        c.roundRect(85, y - 30, W - 170, 38, 5, fill=1, stroke=0)
        c.setFillColor(white)
        c.setFont("Helvetica-Bold", 13)
        c.drawCentredString(W / 2, y - 6, item[:80])
        y -= 52

def timeline(c, items):
    y = H - 92
    c.setStrokeColor(BLUE)
    c.setLineWidth(3)
    c.line(80, 58, 80, H - 88)
    for i, item in enumerate(items):
        c.setFillColor(ORANGE if i % 2 else TEAL)
        c.circle(80, y, 7, fill=1, stroke=0)
        c.setFillColor(INK)
        c.setFont("Helvetica", 13)
        yy = y + 5
        for line in wrapped(c, item, "Helvetica", 13, W - 145):
            c.drawString(104, yy, line)
            yy -= 16
        y -= 45 if len(items) > 5 else 55

def render(c, n, title, kind, body, note):
    if kind == "title":
        c.setFillColor(NAVY)
        c.rect(0, 0, W, H, fill=1, stroke=0)
        c.setFillColor(TEAL)
        c.rect(0, 0, 16, H, fill=1, stroke=0)
        c.setFillColor(white)
        c.setFont("Helvetica-Bold", 30)
        y = H - 100
        for line in title.split("\n"):
            c.drawString(55, y, line)
            y -= 37
        c.setFillColor(HexColor("#B9D9E8"))
        c.setFont("Helvetica", 16)
        y -= 18
        for item in body:
            for line in wrapped(c, item, "Helvetica", 16, W - 120):
                c.drawString(58, y, line)
                y -= 24
            y -= 10
        c.setFillColor(ORANGE)
        c.rect(55, 48, 220, 6, fill=1, stroke=0)
        return
    header(c, n, title)
    if kind == "bullets":
        bullets(c, body)
    elif kind == "code":
        code(c, body)
    elif kind == "tree":
        tree(c, body)
    elif kind == "table":
        table(c, body)
    elif kind == "compare":
        compare(c, body)
    elif kind == "flow":
        flow(c, body)
    elif kind == "diagram":
        diagram(c, body)
    elif kind == "timeline":
        timeline(c, body)
    footer(c, note)

c = canvas.Canvas(str(OUT), pagesize=(W, H))
c.setTitle("Lecture 08 - Services, Repositories, Validation and Multiple Users")
for n, (title, kind, body, note) in enumerate(slides, 1):
    render(c, n, title, kind, body, note)
    c.showPage()
c.save()
print(f"Created {OUT} with {len(slides)} slides")

