namespace Lecture13.Application;

public sealed class ValidationException : Exception
{
    public ValidationException(string key, string message)
        : base(message)
    {
        Key = key;
    }

    public string Key { get; }
}