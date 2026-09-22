from pathlib import Path
import textwrap
from reportlab.pdfgen import canvas
from reportlab.lib import colors
from reportlab.lib.units import inch
from reportlab.pdfbase.pdfmetrics import stringWidth

ROOT=Path(__file__).resolve().parents[2]
OUTPUT=ROOT/'PDF'/'Lecture06_Interfaces_and_Abstraction.pdf'
W,H=13.333*inch,7.5*inch
BLUE=colors.HexColor('#1F4E79'); TEAL=colors.HexColor('#2A9D8F'); GOLD=colors.HexColor('#E9B949')
INK=colors.HexColor('#1F2933'); MUTED=colors.HexColor('#52606D'); LIGHT=colors.HexColor('#F5F7FA'); LINE=colors.HexColor('#D9E2EC'); CODE=colors.HexColor('#17212B')
slides=[
{'kind':'title','title':'Lecture 06','subtitle':'Interfaces and Abstraction','body':['Basics of Internet Technologies 2','One contract, several possible implementations']},
{'title':'Learning Objectives','bullets':['Explain abstraction in practical terms.','Define and implement a C# interface.','Use an interface variable to access different implementations.','Explain polymorphism with a common contract.','Use simple composition through a constructor.','Refactor the expense tracker to depend on IExpenseRepository.','Compare memory and JSON storage without rewriting menu code.','Identify when an interface is useful and when it is unnecessary.']},
{'title':'Connection to Lecture 05','bullets':['Lecture 05 gave an expense a real type: Expense.','The console program still did too many different jobs.','It managed menu input, application rules, and storage details.','Today we create a small boundary around storage.','We do not build full layers yet; first we learn the reason for the boundary.']},
{'title':'Three-Hour Lesson Plan','bullets':['20 min - the storage-change problem and abstraction.','25 min - interface syntax and implementing a contract.','25 min - polymorphism with notifier and shape examples.','30 min - IExpenseRepository with memory storage.','30 min - JSON implementation of the same contract.','25 min - composition in ExpenseTrackerApp.','25 min - live refactoring, exercises, and database connection.']},
{'title':'The Storage Problem','code':'List<Expense> expenses = LoadExpenses(path);\n\n// Program.cs reads, changes, and saves data itself.\nFile.WriteAllText(path, json);','bullets':['The menu code knows a specific storage detail: a JSON file path.','If we later want a database, Program.cs must change in many places.','We want the application to ask for storage actions, not file operations.']},
{'title':'Abstraction','bullets':['Abstraction means focusing on what code needs, not every detail of how it happens.','The expense application needs to get expenses, add an expense, and delete an expense.','It does not need to know whether those actions use a list, JSON, or SQL.','An interface can describe this small promise.']},
{'title':'What Is an Interface?','code':'public interface INotifier\n{\n    void Send(string message);\n}','bullets':['An interface is a contract.','It lists members that an implementing class must provide.','The interface usually describes a capability: notify, store, calculate, send.','It does not contain the concrete implementation in this beginner example.']},
{'title':'Naming Convention','bullets':['C# interfaces commonly begin with a capital I.','Examples: INotifier, IShape, IExpenseRepository.','The I makes a contract easy to recognize in code.','The rest of the name should describe what the contract does.']},
{'title':'Implementing an Interface','code':'public class ConsoleNotifier : INotifier\n{\n    public void Send(string message)\n    {\n        Console.WriteLine("Console: " + message);\n    }\n}','bullets':['A colon means ConsoleNotifier implements INotifier.','The class must provide every required interface member.','The method signature must match the contract.','The body is the implementation detail.']},
{'title':'A Second Implementation','code':'public class UppercaseNotifier : INotifier\n{\n    public void Send(string message)\n    {\n        Console.WriteLine(message.ToUpperInvariant());\n    }\n}','bullets':['The interface stays the same.','The implementation can behave differently.','Both classes can be used where INotifier is expected.']},
{'title':'Interface Variable','code':'INotifier notifier = new ConsoleNotifier();\nnotifier.Send("Welcome");\n\nnotifier = new UppercaseNotifier();\nnotifier.Send("Welcome");','bullets':['The variable type is the contract, not a concrete class.','The object behind it can change.','Only INotifier members are visible through this variable.','This is a deliberate limit: callers use the promised capability.']},
{'title':'Polymorphism','bullets':['Polymorphism means one interface type can represent different concrete objects.','The calling code sends the same message: notifier.Send(...).','Each object decides what that action actually does.','The call site does not need a long if or switch for every implementation.']},
{'title':'Polymorphism With a List','code':'List<IShape> shapes = new List<IShape>\n{\n    new Rectangle(4, 3),\n    new Circle(2)\n};\n\nforeach (IShape shape in shapes)\n    Console.WriteLine(shape.GetArea());','bullets':['A list can hold values with a shared interface.','Rectangle and Circle have different area formulas.','The loop calls one common method on every item.']},
{'title':'Interface Property','code':'public interface IShape\n{\n    string Name { get; }\n    double GetArea();\n}','bullets':['Interfaces can require properties as well as methods.','get-only means callers can read Name but cannot assign it through IShape.','The class chooses how to provide the property.']},
{'title':'Concrete Class: Rectangle','code':'public class Rectangle : IShape\n{\n    public string Name => "Rectangle";\n    public double Width { get; }\n    public double Height { get; }\n\n    public double GetArea()\n    {\n        return Width * Height;\n    }\n}','bullets':['Rectangle has its own state: Width and Height.','It fulfils the shared IShape contract.','Its details do not leak into code that only needs IShape.']},
{'title':'Concrete Class: Circle','code':'public class Circle : IShape\n{\n    public string Name => "Circle";\n    public double Radius { get; }\n\n    public double GetArea()\n    {\n        return Math.PI * Radius * Radius;\n    }\n}','bullets':['Circle has different data and a different formula.','No change is needed in the foreach loop.','This is a useful sign that the contract captures the right common behavior.']},
{'title':'Interface Is Not a Class','columns':[('Interface',['Defines required members','Describes a capability','Cannot be created with new','Can have many implementations']),('Class',['Contains data and behavior','Creates objects with new','Can implement interfaces','Can have constructors'])]},
{'title':'Do Not Create Interfaces Automatically','bullets':['An interface is useful when there are real alternative implementations or a real boundary.','Two classes with identical behavior do not need an interface just because they exist.','Begin with a concrete class when the problem is simple.','Introduce a contract when it removes a real dependency or enables a useful variation.']},
{'title':'The Expense Storage Contract','code':'public interface IExpenseRepository\n{\n    List<Expense> GetAll();\n    void Add(Expense expense);\n    bool DeleteById(int id);\n}','bullets':['Repository is a name for code that manages access to stored data.','The contract states what the application needs from storage.','It does not say whether the data lives in a list, JSON file, or database.']},
{'title':'Read the Contract Carefully','columns':[('GetAll',['Returns current expenses','Caller can list, filter, and summarize']),('Add',['Stores one new Expense','No file or database detail exposed']),('DeleteById',['Returns true when found','Returns false when no matching id exists'])]},
{'title':'In-Memory Implementation','code':'public class InMemoryExpenseRepository : IExpenseRepository\n{\n    private readonly List<Expense> expenses = new();\n\n    public List<Expense> GetAll() => expenses;\n    public void Add(Expense expense) => expenses.Add(expense);\n    // DeleteById removes a matching item.\n}','bullets':['This version is fast and simple for a demo.','Data disappears when the program stops.','It is useful for learning and for tests later.']},
{'title':'JSON Implementation','code':'public class JsonExpenseRepository : IExpenseRepository\n{\n    private readonly string filePath;\n    private readonly List<Expense> expenses;\n\n    public JsonExpenseRepository(string path)\n    {\n        filePath = path;\n        expenses = Load();\n    }\n}','bullets':['This version owns the file path and loading logic.','The rest of the application does not see File.ReadAllText.','The constructor prepares the repository for use.']},
{'title':'Saving Inside the Repository','code':'public void Add(Expense expense)\n{\n    expenses.Add(expense);\n    Save();\n}','bullets':['The repository decides when its storage must be updated.','The app requests Add; it does not request JSON serialization.','This keeps persistence details in one location.']},
{'title':'Choosing an Implementation','code':'IExpenseRepository repository;\n\nif (storageChoice == "1")\n    repository = new InMemoryExpenseRepository();\nelse\n    repository = new JsonExpenseRepository(dataPath);\n\nExpenseTrackerApp app = new ExpenseTrackerApp(repository);','bullets':['Only the composition point chooses the concrete storage type.','The rest of the app receives the shared interface.','Later configuration and dependency injection will improve this setup.']},
{'title':'Composition','bullets':['Composition means one object uses another object as part of its work.','ExpenseTrackerApp has an IExpenseRepository.','It does not inherit from a repository. It uses one.','This often models real software more clearly than a deep inheritance tree.']},
{'title':'Constructor Receives a Dependency','code':'public class ExpenseTrackerApp\n{\n    private readonly IExpenseRepository repository;\n\n    public ExpenseTrackerApp(IExpenseRepository repository)\n    {\n        this.repository = repository;\n    }\n}','bullets':['The app receives the object it needs through its constructor.','readonly prevents replacing the reference after construction.','The word dependency simply means something this class needs to do its job.']},
{'title':'Using the Contract in the App','code':'List<Expense> expenses = repository.GetAll();\nint nextId = expenses.Count == 0\n    ? 1\n    : expenses.Max(expense => expense.Id) + 1;\n\nrepository.Add(newExpense);','bullets':['The app calls contract methods only.','It never calls File.WriteAllText or JsonSerializer.','Both repository versions support this exact code.']},
{'title':'Before and After','columns':[('Before Lecture 06',['Program.cs loads JSON','Program.cs saves JSON','Menu is tied to files','Database requires wider change']),('After Lecture 06',['App uses IExpenseRepository','Repository owns storage details','Menu works with either implementation','Future SQL version fits the contract'])]},
{'title':'What Changes for SQL Later?','bullets':['The interface can stay nearly the same.','A future SqlExpenseRepository can implement IExpenseRepository.','GetAll can run SELECT; Add can run INSERT; DeleteById can run DELETE.','The console application can keep using the same repository methods.','We will learn SQL and database access before writing that implementation.']},
{'title':'Common Beginner Errors','bullets':['Writing new INotifier() - interfaces cannot be created directly.','Forgetting to implement one required interface member.','Adding methods to a class but not to the interface when callers need them.','Using a concrete type everywhere after creating an interface.','Creating an interface for every class without a real need.','Confusing composition with inheritance.']},
{'title':'Guided Exercise A','bullets':['Create an IMessageSender interface with Send(string text).','Implement ConsoleMessageSender and BracketMessageSender.','Write one method that accepts IMessageSender.','Call the same method with both implementations.','Explain which code knows the concrete class and which code knows only the contract.']},
{'title':'Guided Exercise B','bullets':['Create an IDiscount interface with GetDiscount(decimal price).','Implement NoDiscount and PercentageDiscount.','Store both implementations in a List<IDiscount>.','Calculate the result for the same price with each implementation.','Do not use a large switch statement for discount types.']},
{'title':'Student Project Increment','bullets':['Keep your entity class from Lecture 05.','Create an interface that describes only storage actions your project actually needs.','Implement an in-memory repository first.','Move your existing JSON load/save code into a JSON repository.','Make the menu or app class depend on the interface, not the JSON class.']},
{'title':'Shared Database-Course Increment','bullets':['Write storage actions in plain English: read all, add, delete by id.','For each action, identify its future SQL statement: SELECT, INSERT, DELETE.','Use the same table and entity designed in the database lesson.','Do not connect C# to SQL yet; the goal is to see the future boundary clearly.']},
{'title':'Exit Ticket','bullets':['What problem does IExpenseRepository solve?','Why can IExpenseRepository refer to either memory or JSON storage?','What is polymorphism in the notifier example?','Why is ExpenseTrackerApp composed with a repository?','When would adding an interface be unnecessary?']},
{'kind':'title','title':'Next Time','subtitle':'Strategy Pattern and Dependency Thinking','body':['We will choose behavior deliberately at runtime.','The interface becomes a practical tool rather than just syntax.']}
]
def wrap(t,font,size,width):
 w=t.split(); lines=[]; cur=''
 for word in w:
  candidate=word if not cur else cur+' '+word
  if stringWidth(candidate,font,size)<=width: cur=candidate
  else:
   if cur: lines.append(cur)
   cur=word
 if cur: lines.append(cur)
 return lines
def bullets(c,items,x,y,width,size=18,lead=.31*inch):
 cur=y
 for item in items:
  ls=wrap(item,'Helvetica',size,width-.33*inch)
  c.setFillColor(TEAL); c.circle(x+.08*inch,cur+.055*inch,.045*inch,fill=1,stroke=0)
  c.setFillColor(INK); c.setFont('Helvetica',size)
  for i,line in enumerate(ls): c.drawString(x+.24*inch,cur-i*lead,line)
  cur-=max(lead,len(ls)*lead)+.10*inch
def header(c,title,n):
 c.setFillColor(BLUE); c.rect(0,H-.66*inch,W,.66*inch,fill=1,stroke=0)
 c.setFillColor(colors.white); c.setFont('Helvetica-Bold',25); c.drawString(.52*inch,H-.43*inch,title)
 c.setStrokeColor(TEAL); c.setLineWidth(4); c.line(.52*inch,H-.78*inch,2.15*inch,H-.78*inch)
 c.setFillColor(MUTED); c.setFont('Helvetica',9); c.drawRightString(W-.5*inch,.26*inch,f'Lecture 06 | {n:02d}')
def code(c,text,x,y,w,h):
 c.setFillColor(CODE); c.roundRect(x,y-h,w,h,8,fill=1,stroke=0); c.setFillColor(colors.HexColor('#E6EDF3')); c.setFont('Courier',13.2)
 cur=y-.30*inch
 for raw in text.splitlines():
  ls=textwrap.wrap(raw,width=max(20,int(w/7.95)),replace_whitespace=False,drop_whitespace=False) or ['']
  for line in ls: c.drawString(x+.22*inch,cur,line); cur-=.20*inch
def title(c,s):
 c.setFillColor(BLUE); c.rect(0,0,W,H,fill=1,stroke=0); c.setFillColor(TEAL); c.rect(0,0,.2*inch,H,fill=1,stroke=0); c.setFillColor(GOLD); c.rect(.58*inch,H-1.15*inch,1.7*inch,.08*inch,fill=1,stroke=0)
 c.setFillColor(colors.white); c.setFont('Helvetica-Bold',38); c.drawString(.58*inch,H-2.15*inch,s['title']); c.setFont('Helvetica',25); c.drawString(.58*inch,H-2.72*inch,s['subtitle']); c.setStrokeColor(colors.HexColor('#5EA9A0')); c.line(.58*inch,H-3.1*inch,W-.65*inch,H-3.1*inch)
 c.setFont('Helvetica',17); cur=H-3.65*inch
 for line in s.get('body',[]): c.setFillColor(colors.HexColor('#DDEAF4')); c.drawString(.62*inch,cur,line); cur-=.34*inch
 c.setFillColor(colors.HexColor('#B8D8D4')); c.setFont('Helvetica',11); c.drawString(.62*inch,.46*inch,'Basics of Internet Technologies 2')
def slide(c,s,n):
 if s.get('kind')=='title': title(c,s); return
 header(c,s['title'],n)
 if 'columns' in s:
  cols=s['columns']; gap=.30*inch; cw=(W-1.04*inch-gap)/len(cols); x=.52*inch
  for h,items in cols:
   c.setFillColor(LIGHT); c.roundRect(x,1.10*inch,cw,H-2.15*inch,8,fill=1,stroke=0); c.setStrokeColor(LINE); c.roundRect(x,1.10*inch,cw,H-2.15*inch,8,fill=0,stroke=1)
   c.setFillColor(BLUE); c.setFont('Helvetica-Bold',22); c.drawString(x+.25*inch,H-1.25*inch,h); bullets(c,items,x+.18*inch,H-1.78*inch,cw-.38*inch,16,.27*inch); x+=cw+gap
 elif 'code' in s:
  lines=max(5,len(s['code'].splitlines())); ch=min(3.75*inch,.48*inch+lines*.22*inch); code(c,s['code'],.52*inch,H-1.10*inch,W-1.04*inch,ch); bullets(c,s.get('bullets',[]),.58*inch,H-1.34*inch-ch,W-1.15*inch,15.4,.25*inch)
 else: bullets(c,s.get('bullets',[]),.64*inch,H-1.23*inch,W-1.26*inch,18,.31*inch)
def main():
 OUTPUT.parent.mkdir(parents=True,exist_ok=True); c=canvas.Canvas(str(OUTPUT),pagesize=(W,H)); c.setTitle('Lecture 06 - Interfaces and Abstraction')
 for n,s in enumerate(slides,1): slide(c,s,n); c.showPage()
 c.save(); print(f'Created {OUTPUT} with {len(slides)} pages')
if __name__=='__main__': main()
