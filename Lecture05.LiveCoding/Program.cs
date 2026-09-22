Vector2 a = new(0, 0), b = new(3, 4), c = new(3, 0);
List<Vector2> l = new() { a, b, c };
Vector2[] arr = { a, b, c };
Polygon triangle = new(arr);

Console.WriteLine(triangle.Perimeter());

struct Vector2
{
	public float X { get; set; }
	public float Y { get; set; }
	public float Length => MathF.Sqrt(LengthSquared);
	public float LengthSquared => X * X + Y * Y;

	public Vector2() : this(0, 0) { }

	public Vector2(float x, float y)
	{
		X = x;
		Y = y;
	}

	public Vector2(Vector2 other) : this(other.X, other.Y) { }

	public static float Distance(Vector2 a, Vector2 b)
	{
		return (a - b).Length;
	}

	public float Distance(Vector2 other)
	{
		return Distance(this, other);
	}

	public static Vector2 operator +(Vector2 a, Vector2 b)
	{
		return new Vector2(a.X + b.X, a.Y + b.Y);
	}

	public static Vector2 operator -(Vector2 a, Vector2 b)
	{
		return new Vector2(a.X - b.X, a.Y - b.Y);
	}

	public static Vector2 operator *(Vector2 a, float scalar)
	{
		return new Vector2(a.X * scalar, a.Y * scalar);
	}

	public static Vector2 operator /(Vector2 a, float scalar)
	{
		return new Vector2(a.X / scalar, a.Y / scalar);
	}

	public static bool operator ==(Vector2 a, Vector2 b)
	{
		return a.X == b.X && a.Y == b.Y;
	}

	public static bool operator !=(Vector2 a, Vector2 b)
	{
		return !(a == b);
	}

	public override string ToString()
	{
		return $"({X}; {Y})";
	}

	public override bool Equals(object obj)
	{
		if (obj is Vector2 other)
		{
			return this == other;
		}

		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(X, Y);
	}
}

class Polygon
{
	public List<Vector2> Vertices { get; } = new List<Vector2>();
	
	public Polygon(IEnumerable<Vector2> vertices)
	{
		Vertices.AddRange(vertices);
	}

	public float Perimeter()
	{
		float perimeter = 0;
		for (int i = 0; i < Vertices.Count; i++)
		{
			Vector2 current = Vertices[i];
			Vector2 next = Vertices[(i + 1) % Vertices.Count];
			perimeter += Vector2.Distance(current, next);
		}
		return perimeter;
	}
}