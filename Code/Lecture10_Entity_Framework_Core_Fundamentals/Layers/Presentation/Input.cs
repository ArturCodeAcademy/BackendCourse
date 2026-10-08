namespace Lecture10.Presentation;
public static class Input { public static decimal ReadAmount(string raw) => decimal.TryParse(raw, out decimal amount) ? amount : throw new ArgumentException("Amount must be a number."); }
