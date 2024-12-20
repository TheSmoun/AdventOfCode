using System.Text;

#if DEBUG
var lines = File.ReadAllLines("input.txt");
#else
using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
var lines = reader.ReadToEnd().TrimEnd('\n').Split('\n');
#endif

var warehouse = new Warehouse(lines);
var instructions = string.Join(string.Empty, lines.Skip(warehouse.Height)).ToCharArray();

//Console.WriteLine(warehouse.ToString());
//Console.WriteLine();

foreach (var instruction in instructions)
{
    //Console.WriteLine($"Instruction {instruction}");
    warehouse.MoveRobot(instruction);
    //Console.WriteLine(warehouse.ToString());
    //Console.WriteLine();
}

Console.WriteLine($"Part 1: {warehouse.GetSumOfChests()}");

internal class Warehouse
{
    public int Height { get; }

    private readonly int _width;
    private Tile[,] _tiles;
    private int _robotX;
    private int _robotY;
    private List<(int, int)> _positionsToMove;
    
    public Warehouse(string[] lines)
    {
        Height = lines.Index().First(l => l.Item.Length == 0).Index;
        _width = lines[0].Length;

        _tiles = new Tile[Height, _width];

        for (var y = 0; y < Height; y++)
        {
            var line = lines[y];

            for (var x = 0; x < _width; x++)
            {
                _tiles[y, x] = line[x] switch
                {
                    '.' => Tile.Empty,
                    '#' => Tile.Wall,
                    'O' => Tile.Chest,
                    '@' => Tile.Robot,
                    _ => Tile.Empty
                };

                if (_tiles[y, x] == Tile.Robot)
                {
                    _robotX = x;
                    _robotY = y;
                }
            }
        }
        
        _positionsToMove = new List<(int, int)>(Height - 2);
    }

    public void MoveRobot(char instruction)
    {
        switch (instruction)
        {
            case 'v':
                MoveRobot(1, 0);
                break;
            case '^':
                MoveRobot(-1, 0);
                break;
            case '<':
                MoveRobot(0, -1);
                break;
            case '>':
                MoveRobot(0, 1);
                break;
        }
    }

    public int GetSumOfChests()
    {
        var sum = 0;
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < _width; x++)
            {
                if (_tiles[y, x] == Tile.Chest)
                {
                    sum += 100 * y + x;
                }
            }
        }

        return sum;
    }

    private void MoveRobot(int yOffset, int xOffset)
    {
        _positionsToMove.Clear();

        var x = _robotX;
        var y = _robotY;
        
        do
        {
            _positionsToMove.Add((x, y));
            
            x += xOffset;
            y += yOffset;
        } while (_tiles[y, x] == Tile.Chest);

        if (_tiles[y, x] != Tile.Empty)
            return;
        
        _positionsToMove.Reverse();
        
        foreach (var (x1, y1) in _positionsToMove)
        {
            _tiles[y1 + yOffset, x1 + xOffset] = _tiles[y1, x1];
        }
        
        _tiles[_robotY, _robotX] = Tile.Empty;
        _robotX += xOffset;
        _robotY += yOffset;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < _width; x++)
            {
                var c = _tiles[y, x] switch
                {
                    Tile.Wall => '#',
                    Tile.Chest => '0',
                    Tile.Robot => '@',
                    _ => '.'
                };
                
                sb.Append(c);
            }
            
            sb.AppendLine();
        }
        
        return sb.ToString();
    }
}

internal enum Tile : byte
{
    Empty = 0,
    Wall = 1,
    Chest = 2,
    Robot = 3,
}
