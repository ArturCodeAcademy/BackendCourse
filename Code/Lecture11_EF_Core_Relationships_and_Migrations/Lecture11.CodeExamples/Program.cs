var databaseUpdateCommand =
    "dotnet tool run dotnet-ef database update " +
    "--project Layers/Infrastructure --startup-project Project";

var addMigrationCommand =
    "dotnet tool run dotnet-ef migrations add AddCategoryDescription " +
    "--project Layers/Infrastructure --startup-project Project";

Console.WriteLine("Restore the local EF tool:");
Console.WriteLine("dotnet tool restore");
Console.WriteLine();
Console.WriteLine("The InitialCreate migration is already included with this lecture.");
Console.WriteLine("Apply it to the SQLite database:");
Console.WriteLine(databaseUpdateCommand);
Console.WriteLine();
Console.WriteLine("After changing the model, create a new migration:");
Console.WriteLine(addMigrationCommand);