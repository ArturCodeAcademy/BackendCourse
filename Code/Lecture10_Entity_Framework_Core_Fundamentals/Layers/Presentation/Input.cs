namespace Lecture10.Presentation;

public static class Input
{
    public static decimal ReadAmount(string raw)
    {
        if (decimal.TryParse(raw, out decimal amount))
        {
            return amount;
        }

        throw new ArgumentException("Amount must be a number.");
    }
}