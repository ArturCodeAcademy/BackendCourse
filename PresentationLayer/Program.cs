using AppLayer;
using ModelLayer;
using RepoLayer;
using System.Drawing;

IFlowerRepository flowerRepository =
	new FlowerCsvRepository();

IColorRepository colorRepository =
	new ColorCsvRepository();

var service = new FlowerService(
	flowerRepository,
	colorRepository);

while (true)
{
	Console.Clear();

	Console.WriteLine("=== FLOWER MANAGEMENT ===");
	Console.WriteLine();
	Console.WriteLine("1. Show flowers");
	Console.WriteLine("2. Add flower");
	Console.WriteLine("3. Update flower");
	Console.WriteLine("4. Delete flower");
	Console.WriteLine();
	Console.WriteLine("5. Show colors");
	Console.WriteLine("6. Add color");
	Console.WriteLine("7. Update color");
	Console.WriteLine("8. Delete color");
	Console.WriteLine();
	Console.WriteLine("0. Exit");
	Console.WriteLine();

	Console.Write("Choose: ");
	var choice = Console.ReadLine();

	Console.Clear();

	try
	{
		switch (choice)
		{
			case "1":
				ShowFlowers(service);
				break;

			case "2":
				AddFlower(service);
				break;

			case "3":
				UpdateFlower(service);
				break;

			case "4":
				DeleteFlower(service);
				break;

			case "5":
				ShowColors(service);
				break;

			case "6":
				AddColor(service);
				break;

			case "7":
				UpdateColor(service);
				break;

			case "8":
				DeleteColor(service);
				break;

			case "0":
				return;

			default:
				Console.WriteLine("Unknown command.");
				break;
		}
	}
	catch (ValidationException ex)
	{
		Console.WriteLine();
		Console.WriteLine($"Validation error: {ex.Message}");
	}
	catch (Exception ex)
	{
		Console.WriteLine();
		Console.WriteLine($"Unexpected error: {ex.Message}");
	}

	Console.WriteLine();
	Console.WriteLine("Press Enter to continue...");
	Console.ReadLine();
}


// ==================================================
// FLOWERS
// ==================================================

void ShowFlowers(FlowerService service)
{
	var flowers = service.GetAllFlowers();

	Console.WriteLine("=== FLOWERS ===");
	Console.WriteLine();

	if (flowers.Count == 0)
	{
		Console.WriteLine("No flowers found.");
		return;
	}

	foreach (var flower in flowers)
	{
		PrintFlower(flower);
		Console.WriteLine();
	}
}


void AddFlower(FlowerService service)
{
	Console.WriteLine("=== ADD FLOWER ===");
	Console.WriteLine();

	ShowColors(service);

	Console.WriteLine();

	Console.Write("Type: ");
	var type = Console.ReadLine() ?? "";

	Console.Write("Amount: ");
	var amount = ReadInt();

	Console.Write("Color ID: ");
	var colorId = ReadGuid();

	var flower = service.AddFlower(
		type,
		amount,
		colorId);

	Console.WriteLine();
	Console.WriteLine("Flower added successfully:");
	PrintFlower(flower);
}


void UpdateFlower(FlowerService service)
{
	Console.WriteLine("=== UPDATE FLOWER ===");
	Console.WriteLine();

	ShowFlowers(service);

	Console.WriteLine();

	Console.Write("Flower ID: ");
	var flowerId = ReadGuid();

	var currentFlower =
		service.GetFlower(flowerId);

	Console.WriteLine();
	Console.WriteLine("Current flower:");
	PrintFlower(currentFlower);

	Console.WriteLine();
	Console.WriteLine("Available colors:");
	ShowColors(service);

	Console.WriteLine();

	Console.Write($"Type ({currentFlower.Type}): ");
	var typeInput = Console.ReadLine();

	var type = string.IsNullOrWhiteSpace(typeInput)
		? currentFlower.Type
		: typeInput;

	Console.Write($"Amount ({currentFlower.Amount}): ");
	var amountInput = Console.ReadLine();

	var amount = string.IsNullOrWhiteSpace(amountInput)
		? currentFlower.Amount
		: ParseInt(amountInput);

	Console.Write(
		$"Color ID ({currentFlower.ColorId}): ");

	var colorInput = Console.ReadLine();

	var colorId = string.IsNullOrWhiteSpace(colorInput)
		? currentFlower.ColorId
		: ParseGuid(colorInput);

	var updatedFlower = service.UpdateFlower(
		flowerId,
		type,
		amount,
		colorId);

	Console.WriteLine();
	Console.WriteLine("Flower updated successfully:");
	PrintFlower(updatedFlower);
}


void DeleteFlower(FlowerService service)
{
	Console.WriteLine("=== DELETE FLOWER ===");
	Console.WriteLine();

	ShowFlowers(service);

	Console.WriteLine();

	Console.Write("Flower ID: ");
	var id = ReadGuid();

	var flower = service.GetFlower(id);

	Console.WriteLine();
	Console.WriteLine("Flower:");
	PrintFlower(flower);

	Console.WriteLine();
	Console.Write("Are you sure? (y/n): ");

	var answer = Console.ReadLine();

	if (answer?.Equals(
			"y",
			StringComparison.OrdinalIgnoreCase) == true)
	{
		service.DeleteFlower(id);
		Console.WriteLine("Flower deleted.");
	}
	else
	{
		Console.WriteLine("Deletion cancelled.");
	}
}


// ==================================================
// COLORS
// ==================================================

void ShowColors(FlowerService service)
{
	var colors = service.GetAllColors();

	Console.WriteLine("=== COLORS ===");
	Console.WriteLine();

	if (colors.Count == 0)
	{
		Console.WriteLine("No colors found.");
		return;
	}

	foreach (var color in colors)
	{
		PrintColor(color);
	}
}


void AddColor(FlowerService service)
{
	Console.WriteLine("=== ADD COLOR ===");
	Console.WriteLine();

	Console.Write("Name: ");
	var name = Console.ReadLine() ?? "";

	Console.Write("Hex code: ");
	var hexCode = Console.ReadLine() ?? "";

	var color = service.AddColor(
		name,
		hexCode);

	Console.WriteLine();
	Console.WriteLine("Color added successfully:");

	PrintColor(color);
}


void UpdateColor(FlowerService service)
{
	Console.WriteLine("=== UPDATE COLOR ===");
	Console.WriteLine();

	ShowColors(service);

	Console.WriteLine();

	Console.Write("Color ID: ");
	var id = ReadGuid();

	var color = service.GetColor(id);

	Console.WriteLine();
	Console.WriteLine("Current color:");
	PrintColor(color);

	Console.WriteLine();

	Console.Write($"Name ({color.Name}): ");
	var nameInput = Console.ReadLine();

	var name = string.IsNullOrWhiteSpace(nameInput)
		? color.Name
		: nameInput;

	Console.Write($"Hex code ({color.HexCode}): ");
	var hexInput = Console.ReadLine();

	var hexCode = string.IsNullOrWhiteSpace(hexInput)
		? color.HexCode
		: hexInput;

	var updatedColor = service.UpdateColor(
		id,
		name,
		hexCode);

	Console.WriteLine();
	Console.WriteLine("Color updated successfully:");

	PrintColor(updatedColor);
}


void DeleteColor(FlowerService service)
{
	Console.WriteLine("=== DELETE COLOR ===");
	Console.WriteLine();

	ShowColors(service);

	Console.WriteLine();

	Console.Write("Color ID: ");
	var id = ReadGuid();

	var color = service.GetColor(id);

	Console.WriteLine();
	Console.WriteLine("Color:");
	PrintColor(color);

	Console.WriteLine();
	Console.Write("Are you sure? (y/n): ");

	var answer = Console.ReadLine();

	if (answer?.Equals(
			"y",
			StringComparison.OrdinalIgnoreCase) == true)
	{
		service.DeleteColor(id);
		Console.WriteLine("Color deleted.");
	}
	else
	{
		Console.WriteLine("Deletion cancelled.");
	}
}


// ==================================================
// PRINT HELPERS
// ==================================================

void PrintFlower(Flower flower)
{
	Console.WriteLine($"ID:     {flower.Id}");
	Console.WriteLine($"Type:   {flower.Type}");
	Console.WriteLine($"Amount: {flower.Amount}");

	if (flower.Color != null)
	{
		Console.WriteLine(
			$"Color:  {flower.Color.Name} " +
			$"({flower.Color.HexCode})");
	}
	else
	{
		Console.WriteLine(
			$"Color:  UNKNOWN ({flower.ColorId})");
	}
}


void PrintColor(ModelLayer.Color color)
{
	Console.WriteLine(
		$"{color.Id} | " +
		$"{color.Name} | " +
		$"{color.HexCode}");
}


// ==================================================
// INPUT HELPERS
// ==================================================

int ReadInt()
{
	while (true)
	{
		var input = Console.ReadLine();

		if (int.TryParse(input, out var value))
		{
			return value;
		}

		Console.Write("Invalid number. Try again: ");
	}
}

Guid ReadGuid()
{
	while (true)
	{
		var input = Console.ReadLine();

		if (Guid.TryParse(input, out var value))
		{
			return value;
		}

		Console.Write("Invalid GUID. Try again: ");
	}
}

int ParseInt(string input)
{
	if (!int.TryParse(input, out var value))
	{
		throw new ValidationException(
			"Amount must be a valid integer.");
	}

	return value;
}

Guid ParseGuid(string input)
{
	if (!Guid.TryParse(input, out var value))
	{
		throw new ValidationException(
			"ID must be a valid GUID.");
	}

	return value;
}