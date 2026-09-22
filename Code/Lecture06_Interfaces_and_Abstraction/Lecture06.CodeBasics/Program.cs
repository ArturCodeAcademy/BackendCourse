Console.WriteLine("Lecture 06 - Interfaces and Abstraction");
Console.WriteLine("========================================");

Console.WriteLine("\n1. One interface, two implementations");
INotifier consoleNotifier = new ConsoleNotifier();
INotifier uppercaseNotifier = new UppercaseNotifier();
SendWelcome(consoleNotifier, "Ada");
SendWelcome(uppercaseNotifier, "Alan");

Console.WriteLine("\n2. Polymorphism with a list of interface values");
List<IShape> shapes = new List<IShape>
{
    new Rectangle(4, 3),
    new Circle(2)
};

foreach (IShape shape in shapes)
{
    Console.WriteLine(shape.Name + " area: " + shape.GetArea().ToString("F2"));
}

Console.WriteLine("\n3. Composition: an order uses a notifier");
Order order = new Order(new ConsoleNotifier());
order.Confirm("ORD-101");

void SendWelcome(INotifier notifier, string name)
{
    notifier.Send("Welcome, " + name + "!");
}

public interface INotifier
{
    void Send(string message);
}

public class ConsoleNotifier : INotifier
{
    public void Send(string message)
    {
        Console.WriteLine("Console: " + message);
    }
}

public class UppercaseNotifier : INotifier
{
    public void Send(string message)
    {
        Console.WriteLine("Uppercase: " + message.ToUpperInvariant());
    }
}

public interface IShape
{
    string Name { get; }
    double GetArea();
}

public class Rectangle : IShape
{
    public string Name => "Rectangle";
    public double Width { get; }
    public double Height { get; }

    public Rectangle(double width, double height)
    {
        Width = width;
        Height = height;
    }

    public double GetArea()
    {
        return Width * Height;
    }
}

public class Circle : IShape
{
    public string Name => "Circle";
    public double Radius { get; }

    public Circle(double radius)
    {
        Radius = radius;
    }

    public double GetArea()
    {
        return Math.PI * Radius * Radius;
    }
}

public class Order
{
    private readonly INotifier notifier;

    public Order(INotifier notifier)
    {
        this.notifier = notifier;
    }

    public void Confirm(string orderNumber)
    {
        notifier.Send("Order confirmed: " + orderNumber);
    }
}
