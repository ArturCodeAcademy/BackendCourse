from pathlib import Path
from reportlab.pdfgen import canvas
from reportlab.lib.colors import HexColor, white
from reportlab.lib.pagesizes import landscape
from reportlab.pdfbase.pdfmetrics import stringWidth

OUT = Path(r"D:\Projects\BackendCourse\PDF")
W, H = landscape((720, 405))
NAVY, TEAL, BLUE, INK, MUTED, PALE = [HexColor(x) for x in ["#102A43", "#0B8F7A", "#1877A8", "#17324D", "#536779", "#EAF2F7"]]

lessons = [
("09", "SQL Access with Dapper", ["Replace JSON repositories with Dapper and SQLite.", "Keep Application contracts unchanged.", "Use parameters for every SQL value.", "Ownership stays in WHERE Id = @id AND UserId = @userId."], ["Domain: User and Expense.", "Application: repository interfaces and validation.", "Infrastructure: connections, SQL, Dapper mapping.", "Presentation: console conversion.", "Project: composition root."], ["SELECT maps rows to C# objects.", "INSERT uses parameters, never concatenated input.", "DELETE requires both expense id and current user id.", "Database constraints back up service validation."], "Dapper is explicit SQL with light object mapping."),
("10", "Entity Framework Core Fundamentals", ["Introduce DbContext, DbSet, LINQ, tracking and SaveChangesAsync.", "Replace repetitive row mapping without removing layers.", "Use SQLite for persistent local data."], ["Domain: Expense.", "Application: IExpenseRepository and use cases.", "Infrastructure: DbContext plus EF repository.", "Presentation: input conversion.", "Project: DbContextOptions."], ["DbContext is a short-lived unit of work.", "DbSet is a queryable set.", "ToListAsync executes SQL.", "AsNoTracking fits read-only queries."], "EF Core is not business logic; it is an Infrastructure tool."),
("11", "EF Core Relationships and Migrations", ["Model User - Expense and Category - Expense relationships.", "Use foreign keys, navigation properties, Include and migrations.", "Replace disposable schema creation with controlled history."], ["Domain: entities plus navigation properties.", "Application: use cases.", "Infrastructure: Fluent API mapping and migrations.", "Presentation: no connection strings.", "Project: migration startup host."], ["One user has many expenses.", "One category has many expenses.", "Migrations describe schema changes.", "Never rewrite an applied shared migration."], "Every foreign key has a business meaning before it has a database type."),
("12", "HTTP and the First ASP.NET Core Web API", ["Move from console commands to HTTP endpoints.", "Learn routes, verbs, JSON and status codes.", "Keep endpoints thin."], ["Domain: data.", "Application: use cases.", "Infrastructure: repository.", "Presentation: endpoints and HTTP translation.", "Project: WebApplication host."], ["GET reads resources.", "POST creates resources.", "200 is success, 201 is created.", "404 means a resource is absent."], "Endpoint code should call a service, never contain SQL."),
("13", "DTOs, Validation and API Design", ["Separate public API contracts from internal entities.", "Validate request shape and business rules.", "Return predictable errors and status codes."], ["Domain: internal entity.", "Application: reusable rules.", "Infrastructure: persistence.", "Presentation: DTOs, mapping and HTTP validation.", "Project: host."], ["Request DTO controls allowed input.", "Response DTO controls exposed output.", "ValidationProblem is consistent for clients.", "Routes use nouns and HTTP verbs."], "A DTO is a boundary, not a duplicate entity for decoration."),
("14", "Real Backend: API, Layers, EF Core and Dependency Injection", ["Combine API, EF and layers into a coherent host.", "Use DI lifetimes deliberately.", "Pass cancellation tokens through I/O."], ["Domain: stable business data.", "Application: services and interfaces.", "Infrastructure: EF implementation.", "Presentation: endpoints.", "Project: registers concrete services."], ["Scoped: one request and one DbContext.", "Singleton: one app-wide instance.", "Transient: a new resolution.", "Project is the composition root."], "Dependencies point inward; concrete choices live at the edge."),
("15", "Configuration, Error Handling, Identity, Authentication and Final Defense", ["Externalize settings and secrets.", "Centralize unexpected-error responses.", "Distinguish authentication from authorization.", "Prepare final defense."], ["Domain: data.", "Application: business exceptions and rules.", "Infrastructure: persistence.", "Presentation: claims and endpoint policy.", "Project: configuration and middleware."], ["Configuration is not hard-coded secrets.", "One error handler produces safe responses.", "Authentication identifies a caller.", "Authorization permits an action."], "Final project demonstrates the entire evolutionary path.")
]

def wrap(text, size, maxw):
    words = text.split()
    lines, line = [], ""
    for w in words:
        test = w if not line else line + " " + w
        if stringWidth(test, "Helvetica", size) <= maxw: line = test
        else: lines.append(line); line = w
    if line: lines.append(line)
    return lines

def title(c, heading, n, total):
    c.setFillColor(NAVY); c.rect(0, H-56, W, 56, fill=1, stroke=0)
    c.setFillColor(white); c.setFont("Helvetica-Bold", 20); c.drawString(32, H-35, heading)
    c.setFillColor(HexColor("#B9D9E8")); c.setFont("Helvetica-Bold", 10); c.drawRightString(W-28, H-34, f"{n:02d}/{total:02d}")

def footer(c, number):
    c.setStrokeColor(HexColor("#C8D8E1")); c.line(32, 23, W-32, 23)
    c.setFillColor(MUTED); c.setFont("Helvetica", 8); c.drawString(32, 11, "Basics of Internet Technologies 2 | Backend Development")
    c.drawRightString(W-32, 11, "Lecture " + number)

def bullet_page(c, heading, items, number, slide, total):
    title(c, heading, slide, total)
    y=H-94
    for item in items:
        c.setFillColor(TEAL); c.circle(48, y+3, 3, fill=1, stroke=0)
        c.setFillColor(INK); c.setFont("Helvetica", 16)
        for line in wrap(item, 16, W-115):
            c.drawString(64,y,line); y-=22
        y-=11
    footer(c, number); c.showPage()

def layers_page(c, number, lesson, layers, slide, total):
    title(c, "Why the layers stay the same", slide, total)
    y=H-95
    for i,item in enumerate(layers):
        c.setFillColor(PALE if i%2==0 else HexColor("#E8F5F1")); c.roundRect(55,y-26,610,36,5,fill=1,stroke=0)
        c.setFillColor(INK); c.setFont("Helvetica", 14); c.drawString(75,y-4,item); y-=46
    footer(c,number); c.showPage()

def code_page(c, number, lesson, points, slide, total):
    title(c, "Key implementation decisions", slide, total)
    c.setFillColor(HexColor("#17212B")); c.roundRect(45,65,630,230,6,fill=1,stroke=0)
    y=H-105
    for point in points:
        c.setFillColor(HexColor("#D7F3F0")); c.setFont("Courier", 14)
        for line in wrap(point, 14, 555):
            c.drawString(75,y,line); y-=21
        y-=12
    footer(c,number); c.showPage()

for number, lesson, goals, layers, decisions, takeaway in lessons:
    output = OUT / ("Lecture" + number + "_" + lesson.replace(": ", "_").replace(" ", "_").replace(",", "") + ".pdf")
    c = canvas.Canvas(str(output), pagesize=(W,H))
    total=6
    c.setFillColor(NAVY); c.rect(0,0,W,H,fill=1,stroke=0); c.setFillColor(TEAL); c.rect(0,0,15,H,fill=1,stroke=0)
    c.setFillColor(white); c.setFont("Helvetica-Bold",30); c.drawString(55,H-90,"Lecture " + number)
    y = H - 140
    for line in wrap(lesson, 28, W - 105):
        c.drawString(55, y, line)
        y -= 37
    c.setFillColor(HexColor("#B9D9E8")); c.setFont("Helvetica",17); c.drawString(58,y-20,"Continuation of the layered Expense Tracker")
    c.setFillColor(HexColor("#E8F5F1")); c.setFont("Helvetica-Oblique",15)
    for i,line in enumerate(wrap(takeaway,15,550)): c.drawString(58,H-255-i*22,line)
    c.showPage()
    bullet_page(c, "Learning goals", goals, number, 2, total)
    layers_page(c, number, lesson, layers, 3, total)
    code_page(c, number, lesson, decisions, 4, total)
    bullet_page(c, "Guided practice", ["Read the runnable CodeExamples project first.", "Run the Project and identify where each responsibility lives.", "Change one implementation detail without breaking the Application layer.", "Write a short explanation of the boundary you protected."], number, 5, total)
    bullet_page(c, "Connection to the course project", ["Apply the same decision to the student's chosen domain.", "Keep a screenshot or short note for the evolution portfolio.", "Use the database-session model and constraints.", "Prepare one demonstration scenario for the final defense."], number, 6, total)
    c.save()
    print(output)



