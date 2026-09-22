Console.WriteLine("Lecture 07 - From Spaghetti Code to Layers");
Console.WriteLine("===========================================");

Console.WriteLine("\n1. One method with too many jobs");
Console.WriteLine("Read input + validate + calculate + save + print = hard to change.");

Console.WriteLine("\n2. Small responsibilities working together");
IMessageStore store = new InMemoryMessageStore();
MessageService service = new MessageService(store);
ConsoleScreen screen = new ConsoleScreen(service);
screen.AddAndShow("First layered message");
screen.AddAndShow("Second layered message");

Console.WriteLine("\n3. The screen only knows the service");
Console.WriteLine("The service only knows the store contract.");
Console.WriteLine("The store owns the data collection.");

public interface IMessageStore
{
    void Add(string message);
    List<string> GetAll();
}

public class InMemoryMessageStore : IMessageStore
{
    private readonly List<string> messages = new List<string>();

    public void Add(string message)
    {
        messages.Add(message);
    }

    public List<string> GetAll()
    {
        return messages;
    }
}

public class MessageService
{
    private readonly IMessageStore store;

    public MessageService(IMessageStore store)
    {
        this.store = store;
    }

    public bool Add(string message, out string result)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            result = "Message cannot be empty.";
            return false;
        }

        store.Add(message.Trim());
        result = "Message added.";
        return true;
    }

    public List<string> GetAll()
    {
        return store.GetAll();
    }
}

public class ConsoleScreen
{
    private readonly MessageService service;

    public ConsoleScreen(MessageService service)
    {
        this.service = service;
    }

    public void AddAndShow(string message)
    {
        service.Add(message, out string result);
        Console.WriteLine(result);
        foreach (string item in service.GetAll()) Console.WriteLine("- " + item);
    }
}
