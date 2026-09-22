using Lecture08.Infrastructure;

Console.WriteLine("LECTURE 08: password hashing examples");
Console.WriteLine();

var passwordService = new PasswordService();
const string password = "correct-horse-battery-staple";

string firstHash = passwordService.Hash(password);
string secondHash = passwordService.Hash(password);

Console.WriteLine("The same password produces different stored records: " + (firstHash != secondHash));
Console.WriteLine("Correct password verifies: " + passwordService.Verify(password, firstHash));
Console.WriteLine("Wrong password verifies: " + passwordService.Verify("not-the-password", firstHash));

string[] parts = firstHash.Split('$');
Console.WriteLine();
Console.WriteLine("Stored record has " + parts.Length + " parts:");
Console.WriteLine("1. algorithm: " + parts[0]);
Console.WriteLine("2. work factor (iterations): " + parts[1]);
Console.WriteLine("3. random salt: stored as Base64");
Console.WriteLine("4. derived hash: stored as Base64");
Console.WriteLine();
Console.WriteLine("The password itself is never written to the JSON file.");

