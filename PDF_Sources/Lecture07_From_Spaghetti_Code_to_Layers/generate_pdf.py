from pathlib import Path
import textwrap
from reportlab.pdfgen import canvas
from reportlab.lib import colors
from reportlab.lib.units import inch
from reportlab.pdfbase.pdfmetrics import stringWidth
ROOT=Path(__file__).resolve().parents[2]; OUTPUT=ROOT/'PDF'/'Lecture07_From_Spaghetti_Code_to_Layers.pdf'
W,H=13.333*inch,7.5*inch
BLUE=colors.HexColor('#1F4E79'); TEAL=colors.HexColor('#2A9D8F'); GOLD=colors.HexColor('#E9B949'); INK=colors.HexColor('#1F2933'); MUTED=colors.HexColor('#52606D'); LIGHT=colors.HexColor('#F5F7FA'); LINE=colors.HexColor('#D9E2EC'); CODE=colors.HexColor('#17212B')
slides=[
{'kind':'title','title':'Lecture 07','subtitle':'From Spaghetti Code to Layers','body':['Basics of Internet Technologies 2','Giving every part of an application one clear job']},
{'title':'Learning Objectives','bullets':['Recognize when one file or method has too many responsibilities.','Explain presentation, application, domain, and infrastructure layers.','Move console input and output into a presentation class.','Move validation and use cases into an application service.','Keep the Expense model in the domain layer.','Keep JSON file access in infrastructure.','Use Program.cs as a composition root.','Navigate the new Examples and Project folder structure.']},
{'title':'Solution Structure','code':'Lecture 07 - From Spaghetti Code to Layers\n|-- Layers\n|   |-- Lecture07.Application\n|   |-- Lecture07.Domain\n|   |-- Lecture07.Infrastructure\n|   `-- Lecture07.Presentation\n|-- Lecture07.CodeExamples\n`-- Lecture07.Project','bullets':['Layers contains the four layer class-library projects.','Lecture07.CodeExamples is a small runnable experiment.','Lecture07.Project is the evolving console application that references the layers.']},
{'title':'Connection to Lecture 06','bullets':['Lecture 06 made the storage boundary explicit with IExpenseRepository.','The project still needs a home for menu code, rules, models, and file code.','Interfaces help a boundary; layers organize the whole application.','Today we use the JSON repository from Lecture 06 inside an explicit infrastructure layer.']},
{'title':'Three-Hour Lesson Plan','bullets':['20 min - recognize the problem: Program.cs becomes a junk drawer.','25 min - responsibilities and four beginner-friendly layers.','25 min - domain model and application service.','25 min - presentation and infrastructure responsibilities.','30 min - composition root and dependency direction.','35 min - live refactoring of the expense tracker.','20 min - exercises, architecture map, and shared database task.']},
{'title':'What Is Spaghetti Code?','bullets':['Code is called spaghetti when responsibilities are tangled together.','One method reads input, validates, calculates, writes JSON, and prints output.','A small change requires searching through unrelated details.','The problem is not file length alone; it is unclear ownership.','We improve structure gradually, not by adding folders for decoration.']},
{'title':'Before: One Method With Too Many Jobs','code':'void AddExpense()\n{\n    string name = Console.ReadLine();\n    // Validate input\n    // Create Expense\n    // Add to list\n    // Serialize JSON\n    // Write file\n    // Print message\n}','bullets':['This can work for a first console program.','As features grow, it becomes hard to test and change.','Each comment names a different responsibility.']},
{'title':'One Change, Many Risks','bullets':['Want a database? File code is mixed with menu code.','Want a web API? Console calls are mixed with application rules.','Want different validation? Rules are scattered in command handlers.','Want to test a calculation? Starting the console may be required.','Layers create places where future changes can live.']},
{'title':'A Layer Is a Responsibility Boundary','bullets':['A layer groups code with a similar reason to change.','Presentation changes when the user interface changes.','Application changes when a use case or business rule changes.','Infrastructure changes when storage or external technology changes.','Domain changes when the meaning of the model changes.']},
{'title':'Four Layer Projects in Layers','columns':[('Presentation',['Console menu','Reads input','Prints output']),('Application',['Use cases','Validation','Coordinates work']),('Domain',['Expense model','Meaningful types','Core data']),('Infrastructure',['JSON repository','File access','Technology details'])]},
{'title':'Dependency Direction','bullets':['Presentation calls Application.','Application uses Domain and depends on the repository contract.','Infrastructure implements the repository contract.','Domain does not know about console menus, JSON, or files.','Program.cs creates the concrete objects and connects them.']},
{'title':'Project Map','code':'Program.cs\n  -> ConsoleMenu (Presentation)\n  -> ExpenseService (Application)\n  -> IExpenseRepository (Application contract)\n  -> JsonExpenseRepository (Infrastructure)\n  -> expenses.json\n\nExpense is used by all layers as a Domain model.','bullets':['Arrows mean one part uses the next part.','The JSON class is chosen only in Program.cs.']},
{'title':'Domain: Expense','code':'namespace Lecture07.Project.Domain;\n\npublic class Expense\n{\n    public int Id { get; set; }\n    public string Name { get; set; }\n    public decimal Amount { get; set; }\n    public string Category { get; set; }\n    public DateTime CreatedAt { get; set; }\n}','bullets':['Expense represents a concept from the problem domain.','The model uses meaningful types: decimal for money and DateTime for time.','It does not read files or ask questions in the console.']},
{'title':'Why Domain Has No Console Code','bullets':['An Expense should mean the same thing in a console app, web API, or test.','Console.ReadLine is a presentation concern.','File.WriteAllText is an infrastructure concern.','Keeping the model independent makes it reusable.']},
{'title':'Application: The Repository Contract','code':'public interface IExpenseRepository\n{\n    List<Expense> GetAll();\n    void Add(Expense expense);\n    bool DeleteById(int id);\n}','bullets':['The application defines what it needs from storage.','It names actions in the language of the application.','It does not mention JSON, file paths, SELECT, or INSERT.']},
{'title':'Application: ExpenseService','code':'public class ExpenseService\n{\n    private readonly IExpenseRepository repository;\n\n    public ExpenseService(IExpenseRepository repository)\n    {\n        this.repository = repository;\n    }\n}','bullets':['The service represents application use cases.','It receives a storage contract in its constructor.','It coordinates domain objects and repository actions.']},
{'title':'Validation Moves Into the Service','code':'public bool Add(string name, decimal amount, string category,\n    out string message)\n{\n    if (string.IsNullOrWhiteSpace(name))\n    {\n        message = "Name cannot be empty.";\n        return false;\n    }\n\n    if (amount <= 0)\n    {\n        message = "Amount must be greater than zero.";\n        return false;\n    }\n}','bullets':['The rule is not tied to a console command.','A future web API can call the same service method.','The message is returned so the presentation layer can show it.']},
{'title':'Application Service Creates the Model','code':'int nextId = expenses.Count == 0\n    ? 1\n    : expenses.Max(expense => expense.Id) + 1;\n\nrepository.Add(new Expense(\n    nextId, name, amount, category, DateTime.Now));','bullets':['The service owns the use case of adding an expense.','The repository owns storage after Add is called.','The menu does not decide how an Expense is saved.']},
{'title':'Infrastructure: JSON Repository','code':'public class JsonExpenseRepository : IExpenseRepository\n{\n    private readonly string filePath;\n    private readonly List<Expense> expenses;\n\n    public JsonExpenseRepository(string filePath)\n    {\n        this.filePath = filePath;\n        expenses = Load();\n    }\n}','bullets':['This class owns the JSON and file path details.','It implements an application contract.','Later a database repository can replace it.']},
{'title':'Infrastructure Owns File Details','code':'string json = JsonSerializer.Serialize(expenses, options);\nFile.WriteAllText(filePath, json);','bullets':['System.Text.Json belongs in Infrastructure here.','Directory creation and IOException handling belong here too.','The service never needs to know how JSON is written.']},
{'title':'Presentation: ConsoleMenu','code':'public class ConsoleMenu\n{\n    private readonly ExpenseService service;\n\n    public ConsoleMenu(ExpenseService service)\n    {\n        this.service = service;\n    }\n}','bullets':['ConsoleMenu owns questions, commands, and printed messages.','It receives the application service, not a JSON repository.','The presentation layer should be thin: collect input and display results.']},
{'title':'Presentation Converts Input','code':'Console.Write("Amount: ");\nstring amountText = Console.ReadLine();\n\nif (!decimal.TryParse(amountText, out decimal amount))\n{\n    Console.WriteLine("Amount must be a number.");\n    return;\n}\n\nservice.Add(name, amount, category, out string message);','bullets':['Parsing text is needed because the console gives us strings.','The menu handles parsing feedback.','The service handles whether the parsed data is valid for the use case.']},
{'title':'Thin Presentation, Useful Application','columns':[('ConsoleMenu',['Read input','Parse numbers','Call service','Print result']),('ExpenseService',['Validate rules','Create Expense','Choose ids','Request storage'])]},
{'title':'Program.cs Is the Composition Root','code':'IExpenseRepository repository =\n    new JsonExpenseRepository(dataPath);\nExpenseService service = new ExpenseService(repository);\nConsoleMenu menu = new ConsoleMenu(service);\n\nmenu.Run();','bullets':['Composition root means the place that creates and connects concrete objects.','Only this file chooses JsonExpenseRepository.','This is simple manual dependency injection; a framework comes much later.']},
{'title':'Why Separate Layer Projects?','bullets':['Each project makes a dependency visible and enforceable at compile time.','Domain has no reference to console or JSON technology.','Application references Domain; Infrastructure and Presentation reference only what they need.','Lecture07.Project is the composition point that references the concrete layers.']},
{'title':'Three Kinds of Projects','columns':[('Layers',['Four class libraries','One responsibility each','Reusable application parts','Dependencies are explicit']),('CodeExamples',['Small runnable snippets','One concept at a time','Safe place to experiment','No application architecture required']),('Project',['Console application','References the layers','Composition root in Program.cs','Preserves user features'])]},
{'title':'Refactoring Sequence','bullets':['1. Keep the working console project as a reference.','2. Create the Domain/Expense class file.','3. Move IExpenseRepository and ExpenseService into Application.','4. Move JSON code into Infrastructure/JsonExpenseRepository.','5. Move the menu loop into Presentation/ConsoleMenu.','6. Leave Program.cs only to connect objects and run the menu.','7. Build and test after each move.']},
{'title':'Before and After','columns':[('Before',['Program.cs knows everything','JSON and menu intertwined','Rules repeat in commands','Future changes spread widely']),('After',['Each class has one main job','JSON isolated in Infrastructure','Rules are in ExpenseService','Future UI or storage changes are local'])]},
{'title':'Common Mistakes','bullets':['Putting Console.WriteLine inside ExpenseService or Expense.','Letting ConsoleMenu call File.WriteAllText directly.','Putting all code in folders but keeping the same tangled dependencies.','Creating a service with no real use case logic.','Making Domain depend on JSON or the console.','Splitting into dozens of folders before there is a need.']},
{'title':'Guided Exercise A','bullets':['Create a TaskItem domain class with Id, Title, IsCompleted, and CreatedAt.','Create ITaskRepository with GetAll, Add, and DeleteById.','Create TaskService with validation for a non-empty title.','Create a small ConsoleMenu method that calls the service.','Describe which file belongs in each layer.']},
{'title':'Guided Exercise B','bullets':['Take one method from a previous project that does three or more jobs.','List its responsibilities in comments.','Move one storage responsibility behind an interface and repository.','Move one validation rule into a service method.','Build after every move and explain what became easier to change.']},
{'title':'Student Project Increment','bullets':['Create a Layers solution folder with four class-library projects.','Put the entity in LectureNN.Domain and the service plus contracts in LectureNN.Application.','Put JSON or SQL technology in LectureNN.Infrastructure.','Put console interaction in LectureNN.Presentation.','Create a separate LectureNN.Project console application that references the layers.','Keep its Program.cs as the composition root.']},
{'title':'Shared Database-Course Increment','bullets':['Use the same table designed in the database course.','Map future SQL code to Infrastructure, not to ConsoleMenu or ExpenseService.','Keep IExpenseRepository in Application because it describes what the app needs.','Write a short diagram showing Console -> Service -> Repository -> Database.','The actual SQL connection begins in a later lecture.']},
{'title':'Exit Ticket','bullets':['Which layer should own Console.ReadLine?','Which layer should own JSON serialization?','Why does ExpenseService depend on IExpenseRepository instead of JSON?','What is the one job of Program.cs now?','What will change first if we replace the console with a web API?']},
{'kind':'title','title':'Next Time','subtitle':'Services, Repositories, Validation and Multiple Users','body':['We will deepen the Application layer with real use cases and ownership.','The layered console becomes Milestone 03.']}
]
def wrap(t,f,s,w):
 words=t.split(); out=[]; cur=''
 for z in words:
  cand=z if not cur else cur+' '+z
  if stringWidth(cand,f,s)<=w: cur=cand
  else:
   if cur: out.append(cur)
   cur=z
 if cur: out.append(cur)
 return out
def bullets(c,items,x,y,w,size=18,lead=.31*inch):
 cur=y
 for item in items:
  ls=wrap(item,'Helvetica',size,w-.33*inch); c.setFillColor(TEAL); c.circle(x+.08*inch,cur+.055*inch,.045*inch,fill=1,stroke=0); c.setFillColor(INK); c.setFont('Helvetica',size)
  for i,line in enumerate(ls): c.drawString(x+.24*inch,cur-i*lead,line)
  cur-=max(lead,len(ls)*lead)+.10*inch
def header(c,t,n):
 c.setFillColor(BLUE); c.rect(0,H-.66*inch,W,.66*inch,fill=1,stroke=0); c.setFillColor(colors.white); c.setFont('Helvetica-Bold',25); c.drawString(.52*inch,H-.43*inch,t); c.setStrokeColor(TEAL); c.setLineWidth(4); c.line(.52*inch,H-.78*inch,2.15*inch,H-.78*inch); c.setFillColor(MUTED); c.setFont('Helvetica',9); c.drawRightString(W-.5*inch,.26*inch,f'Lecture 07 | {n:02d}')
def code(c,t,x,y,w,h):
 c.setFillColor(CODE); c.roundRect(x,y-h,w,h,8,fill=1,stroke=0); c.setFillColor(colors.HexColor('#E6EDF3')); c.setFont('Courier',13.1); cur=y-.30*inch
 for raw in t.splitlines():
  ls=textwrap.wrap(raw,width=max(20,int(w/7.95)),replace_whitespace=False,drop_whitespace=False) or ['']
  for line in ls: c.drawString(x+.22*inch,cur,line); cur-=.20*inch
def title(c,s):
 c.setFillColor(BLUE); c.rect(0,0,W,H,fill=1,stroke=0); c.setFillColor(TEAL); c.rect(0,0,.2*inch,H,fill=1,stroke=0); c.setFillColor(GOLD); c.rect(.58*inch,H-1.15*inch,1.7*inch,.08*inch,fill=1,stroke=0); c.setFillColor(colors.white); c.setFont('Helvetica-Bold',38); c.drawString(.58*inch,H-2.15*inch,s['title']); c.setFont('Helvetica',25); c.drawString(.58*inch,H-2.72*inch,s['subtitle']); c.setStrokeColor(colors.HexColor('#5EA9A0')); c.line(.58*inch,H-3.1*inch,W-.65*inch,H-3.1*inch); c.setFont('Helvetica',17); cur=H-3.65*inch
 for z in s.get('body',[]): c.setFillColor(colors.HexColor('#DDEAF4')); c.drawString(.62*inch,cur,z); cur-=.34*inch
 c.setFillColor(colors.HexColor('#B8D8D4')); c.setFont('Helvetica',11); c.drawString(.62*inch,.46*inch,'Basics of Internet Technologies 2')
def draw(c,s,n):
 if s.get('kind')=='title': title(c,s); return
 header(c,s['title'],n)
 if 'columns' in s:
  cols=s['columns']; gap=.30*inch; cw=(W-1.04*inch-gap*(len(cols)-1))/len(cols); x=.52*inch
  for h,items in cols:
   c.setFillColor(LIGHT); c.roundRect(x,1.10*inch,cw,H-2.15*inch,8,fill=1,stroke=0); c.setStrokeColor(LINE); c.roundRect(x,1.10*inch,cw,H-2.15*inch,8,fill=0,stroke=1); c.setFillColor(BLUE); c.setFont('Helvetica-Bold',21); c.drawString(x+.25*inch,H-1.25*inch,h); bullets(c,items,x+.18*inch,H-1.78*inch,cw-.38*inch,15.5,.27*inch); x+=cw+gap
 elif 'code' in s:
  ch=min(3.75*inch,.48*inch+max(5,len(s['code'].splitlines()))*.22*inch); code(c,s['code'],.52*inch,H-1.10*inch,W-1.04*inch,ch); bullets(c,s.get('bullets',[]),.58*inch,H-1.34*inch-ch,W-1.15*inch,15.2,.25*inch)
 else: bullets(c,s.get('bullets',[]),.64*inch,H-1.23*inch,W-1.26*inch,18,.31*inch)
def main():
 OUTPUT.parent.mkdir(parents=True,exist_ok=True); c=canvas.Canvas(str(OUTPUT),pagesize=(W,H)); c.setTitle('Lecture 07 - From Spaghetti Code to Layers')
 for n,s in enumerate(slides,1): draw(c,s,n); c.showPage()
 c.save(); print(f'Created {OUTPUT} with {len(slides)} pages')
if __name__=='__main__': main()



