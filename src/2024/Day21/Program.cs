#if DEBUG
var lines = File.ReadAllLines("input.txt");
#else
using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
var lines = reader.ReadToEnd().TrimEnd('\n').Split('\n');
#endif

var numPad = new Keypad("789", "456", "123", " 0A");
var arrowPad = new Keypad(" ^A", "<v>");
var keyPads = new[] { numPad, arrowPad, arrowPad };

var part1 = lines.Sum(l => EncodeKeys(l, keyPads) * int.Parse(l[..^1]));
Console.WriteLine($"Part 1: {part1}");

return;

int EncodeKeys(string keys, Keypad[] keypads)
{
    if (keypads.Length == 0)
        return keys.Length;

    var currentKey = 'A';
    var length = 0;

    foreach (var nextKey in keys)
    {
        length += EncodeKey(currentKey, nextKey, keypads);
        currentKey = nextKey;
    }
    
    return length;
}

int EncodeKey(char from, char to, Keypad[] keypads)
{
    var keypad = keypads[0];
    var fromPos = keypad[from];
    var toPos = keypad[to];
    var delta = toPos - fromPos;
    var verticalKeyStrokes = new string(delta.Y > 0 ? 'v' : '^', Math.Abs(delta.Y));
    var horizontalKeyStrokes = new string(delta.X > 0 ? '>' : '<', Math.Abs(delta.X));

    var length = int.MaxValue;
    
    if (keypad.Contains(fromPos.X, toPos.Y))
    {
        length = Math.Min(length, EncodeKeys($"{verticalKeyStrokes}{horizontalKeyStrokes}A", keypads[1..]));
    }

    if (keypad.Contains(toPos.X, fromPos.Y))
    {
        length = Math.Min(length, EncodeKeys($"{horizontalKeyStrokes}{verticalKeyStrokes}A", keypads[1..]));
    }
    
    return length;
}

internal class Keypad
{
    private readonly HashSet<Vec2> _positions = [];
    private readonly Dictionary<char, Vec2> _key2Pos = [];

    public Keypad(params string[] parts)
    {
        for (var y = 0; y < parts.Length; y++)
        {
            var part = parts[y];
            
            for (var x = 0; x < part.Length; x++)
            {
                var key = part[x];
                if (key == ' ')
                    continue;
                
                var pos = new Vec2(x, y);

                _positions.Add(pos);
                _key2Pos[key] = pos;
            }
        }
    }

    public bool Contains(int x, int y)
    {
        return _positions.Contains(new Vec2(x, y));
    }
    
    public Vec2 this[char key] => _key2Pos.GetValueOrDefault(key);
}

internal readonly struct Vec2 : IEquatable<Vec2>
{
    public int X { get; }
    public int Y { get; }

    public Vec2(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static Vec2 operator -(Vec2 a, Vec2 b)
    {
        return new Vec2(a.X - b.X, a.Y - b.Y);
    }

    public bool Equals(Vec2 other)
    {
        return X == other.X && Y == other.Y;
    }

    public override bool Equals(object? obj)
    {
        return obj is Vec2 other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}
