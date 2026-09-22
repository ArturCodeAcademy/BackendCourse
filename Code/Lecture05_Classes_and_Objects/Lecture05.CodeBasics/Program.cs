Console.WriteLine("Lecture 05 - Classes and Objects");
Console.WriteLine("================================");

Console.WriteLine("\n1. Creating objects from a class");
Student firstStudent = new Student("Ada", "Lovelace", 21);
Student secondStudent = new Student("Alan", "Turing", 23);
Console.WriteLine(firstStudent.GetIntroduction());
Console.WriteLine(secondStudent.GetIntroduction());
Console.WriteLine("Students created: " + Student.CreatedCount);

Console.WriteLine("\n2. Properties and methods");
firstStudent.Email = "ada@example.com";
firstStudent.Enroll("C# Basics");
firstStudent.Enroll("Databases");
Console.WriteLine(firstStudent);

Console.WriteLine("\n3. Encapsulation: a private field is changed through a method");
BankAccount account = new BankAccount("Ada Lovelace");
account.Deposit(100m);
account.Deposit(-5m);
Console.WriteLine(account.GetBalanceText());

Console.WriteLine("\n4. Constructor overloads");
Book titleOnly = new Book("Clean Code");
Book fullBook = new Book("C# in Depth", "Jon Skeet", 2024);
Console.WriteLine(titleOnly.GetDescription());
Console.WriteLine(fullBook.GetDescription());

Console.WriteLine("\n5. Object references");
Student alias = firstStudent;
alias.Age = 22;
Console.WriteLine("firstStudent age: " + firstStudent.Age);
Console.WriteLine("alias age: " + alias.Age);
Console.WriteLine("Both names point to the same Student object.");

Console.WriteLine("\n6. A list of objects");
List<Student> students = new List<Student> { firstStudent, secondStudent };
foreach (Student student in students)
{
    Console.WriteLine(student.FullName + " - " + student.Age);
}

public class Student
{
    private readonly List<string> courses = new List<string>();

    public static int CreatedCount { get; private set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
    public string FullName => FirstName + " " + LastName;

    public Student(string firstName, string lastName, int age)
    {
        FirstName = firstName;
        LastName = lastName;
        Age = age;
        CreatedCount++;
    }

    public void Enroll(string courseName)
    {
        if (!string.IsNullOrWhiteSpace(courseName))
        {
            courses.Add(courseName);
        }
    }

    public string GetIntroduction()
    {
        return "Hello, I am " + FullName + ". I am " + Age + " years old.";
    }

    public override string ToString()
    {
        string courseText = courses.Count == 0 ? "no courses" : string.Join(", ", courses);
        return FullName + " | " + Email + " | courses: " + courseText;
    }
}

public class BankAccount
{
    private decimal balance;
    public string Owner { get; }

    public BankAccount(string owner)
    {
        Owner = owner;
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Deposit must be greater than zero.");
            return;
        }

        balance += amount;
    }

    public string GetBalanceText()
    {
        return Owner + " has " + balance.ToString("F2") + " EUR.";
    }
}

public class Book
{
    public string Title { get; }
    public string Author { get; }
    public int Year { get; }

    public Book(string title) : this(title, "Unknown", 0)
    {
    }

    public Book(string title, string author, int year)
    {
        Title = title;
        Author = author;
        Year = year;
    }

    public string GetDescription()
    {
        return Year == 0 ? Title + " by " + Author : Title + " by " + Author + " (" + Year + ")";
    }
}
