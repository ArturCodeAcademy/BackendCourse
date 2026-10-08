Console.WriteLine("Migration commands:");
Console.WriteLine("dotnet ef migrations add InitialCreate --project Layers/Infrastructure --startup-project Project");
Console.WriteLine("dotnet ef database update --project Layers/Infrastructure --startup-project Project");
