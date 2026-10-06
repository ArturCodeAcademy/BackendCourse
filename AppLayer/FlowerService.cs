using ModelLayer;
using RepoLayer;
using System.Text.RegularExpressions;

namespace AppLayer;

public class FlowerService
{
	private readonly IFlowerRepository _flowerRepository;
	private readonly IColorRepository _colorRepository;

	public FlowerService(
		IFlowerRepository flowerRepository,
		IColorRepository colorRepository)
	{
		_flowerRepository = flowerRepository;
		_colorRepository = colorRepository;
	}

	// =========================
	// FLOWERS
	// =========================

	public List<Flower> GetAllFlowers()
	{
		var flowers = _flowerRepository.GetAllFlowers();
		var colors = _colorRepository.GetAllColors();

		foreach (var flower in flowers)
		{
			flower.Color = colors.FirstOrDefault(c => c.Id == flower.ColorId);
		}

		return flowers;
	}

	public Flower GetFlower(Guid id)
	{
		var flower = _flowerRepository
			.GetAllFlowers()
			.FirstOrDefault(f => f.Id == id);

		if (flower == null)
		{
			throw new ValidationException("Flower was not found.");
		}

		flower.Color = _colorRepository
			.GetAllColors()
			.FirstOrDefault(c => c.Id == flower.ColorId);

		return flower;
	}

	public Flower AddFlower(
		string type,
		int amount,
		Guid colorId)
	{
		ValidateFlower(type, amount, colorId);

		var flower = new Flower
		{
			Id = Guid.NewGuid(),
			Type = type.Trim(),
			Amount = amount,
			ColorId = colorId
		};

		_flowerRepository.AddFlower(flower);

		flower.Color = GetColor(colorId);

		return flower;
	}

	public Flower UpdateFlower(
		Guid id,
		string type,
		int amount,
		Guid colorId)
	{
		var flower = _flowerRepository
			.GetAllFlowers()
			.FirstOrDefault(f => f.Id == id);

		if (flower == null)
		{
			throw new ValidationException("Flower was not found.");
		}

		ValidateFlower(type, amount, colorId);

		flower.Type = type.Trim();
		flower.Amount = amount;
		flower.ColorId = colorId;

		_flowerRepository.UpdateFlower(flower);

		flower.Color = GetColor(colorId);

		return flower;
	}

	public void DeleteFlower(Guid id)
	{
		var flowerExists = _flowerRepository
			.GetAllFlowers()
			.Any(f => f.Id == id);

		if (!flowerExists)
		{
			throw new ValidationException("Flower was not found.");
		}

		_flowerRepository.DeleteFlower(id);
	}

	// =========================
	// COLORS
	// =========================

	public List<Color> GetAllColors()
	{
		return _colorRepository.GetAllColors();
	}

	public Color GetColor(Guid id)
	{
		var color = _colorRepository
			.GetAllColors()
			.FirstOrDefault(c => c.Id == id);

		if (color == null)
		{
			throw new ValidationException("Color was not found.");
		}

		return color;
	}

	public Color AddColor(
		string name,
		string hexCode)
	{
		ValidateColor(name, hexCode);

		if (_colorRepository
			.GetAllColors()
			.Any(c => c.Name.Equals(
				name.Trim(),
				StringComparison.OrdinalIgnoreCase)))
		{
			throw new ValidationException(
				"A color with this name already exists.");
		}

		var color = new Color
		{
			Id = Guid.NewGuid(),
			Name = name.Trim(),
			HexCode = NormalizeHexCode(hexCode)
		};

		_colorRepository.AddColor(color);

		return color;
	}

	public Color UpdateColor(
		Guid id,
		string name,
		string hexCode)
	{
		var color = _colorRepository
			.GetAllColors()
			.FirstOrDefault(c => c.Id == id);

		if (color == null)
		{
			throw new ValidationException("Color was not found.");
		}

		ValidateColor(name, hexCode);

		var duplicateName = _colorRepository
			.GetAllColors()
			.Any(c =>
				c.Id != id &&
				c.Name.Equals(
					name.Trim(),
					StringComparison.OrdinalIgnoreCase));

		if (duplicateName)
		{
			throw new ValidationException(
				"A color with this name already exists.");
		}

		color.Name = name.Trim();
		color.HexCode = NormalizeHexCode(hexCode);

		_colorRepository.UpdateColor(color);

		return color;
	}

	public void DeleteColor(Guid id)
	{
		var colorExists = _colorRepository
			.GetAllColors()
			.Any(c => c.Id == id);

		if (!colorExists)
		{
			throw new ValidationException("Color was not found.");
		}

		var colorIsUsed = _flowerRepository
			.GetAllFlowers()
			.Any(f => f.ColorId == id);

		if (colorIsUsed)
		{
			throw new ValidationException(
				"Cannot delete this color because it is used by one or more flowers.");
		}

		_colorRepository.DeleteColor(id);
	}

	// =========================
	// VALIDATION
	// =========================

	private void ValidateFlower(
		string type,
		int amount,
		Guid colorId)
	{
		if (string.IsNullOrWhiteSpace(type))
		{
			throw new ValidationException(
				"Flower type cannot be empty.");
		}

		if (type.Trim().Length < 2)
		{
			throw new ValidationException(
				"Flower type must contain at least 2 characters.");
		}

		if (type.Trim().Length > 100)
		{
			throw new ValidationException(
				"Flower type cannot contain more than 100 characters.");
		}

		if (amount < 0)
		{
			throw new ValidationException(
				"Flower amount cannot be negative.");
		}

		if (colorId == Guid.Empty)
		{
			throw new ValidationException(
				"Flower must have a color.");
		}

		var colorExists = _colorRepository
			.GetAllColors()
			.Any(c => c.Id == colorId);

		if (!colorExists)
		{
			throw new ValidationException(
				"Selected color does not exist.");
		}
	}

	private void ValidateColor(
		string name,
		string hexCode)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new ValidationException(
				"Color name cannot be empty.");
		}

		if (name.Trim().Length < 2)
		{
			throw new ValidationException(
				"Color name must contain at least 2 characters.");
		}

		if (name.Trim().Length > 50)
		{
			throw new ValidationException(
				"Color name cannot contain more than 50 characters.");
		}

		if (string.IsNullOrWhiteSpace(hexCode))
		{
			throw new ValidationException(
				"Hex code cannot be empty.");
		}

		var normalizedHex = NormalizeHexCode(hexCode);

		if (!Regex.IsMatch(
				normalizedHex,
				"^#[0-9A-Fa-f]{6}$"))
		{
			throw new ValidationException(
				"Hex code must be in format #RRGGBB, for example #FF0000.");
		}
	}

	private string NormalizeHexCode(string hexCode)
	{
		var result = hexCode.Trim();

		if (!result.StartsWith("#"))
		{
			result = "#" + result;
		}

		return result.ToUpper();
	}
}