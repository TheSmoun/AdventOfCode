using System.Text;

#if DEBUG
var lines = File.ReadAllLines("input.txt");
#else
using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
var lines = reader.ReadToEnd().TrimEnd('\n').Split('\n');
#endif

var warehousePart1 = new WarehousePart1(lines);
var warehousePart2 = new WarehousePart2(lines, warehousePart1.Height);
var instructions = string.Join(string.Empty, lines.Skip(warehousePart1.Height)).ToCharArray();

foreach (var instruction in instructions)
{
    warehousePart1.MoveRobot(instruction);
    warehousePart2.MoveRobot(instruction);
}

Console.WriteLine($"Part 1: {warehousePart1.GetSumOfChests()}");
Console.WriteLine($"Part 2: {warehousePart2.GetSumOfChests()}");

internal class WarehousePart1
{
    public int Height { get; }

    private readonly int _width;
    private readonly Tile[,] _tiles;
    private readonly List<(int, int)> _positionsToMove;
    private int _robotX;
    private int _robotY;
    
    public WarehousePart1(string[] lines)
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
        } while (_tiles[y, x] != Tile.Wall && _tiles[y, x] != Tile.Empty);

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

internal class WarehousePart2
{
    private readonly int _height;
    private readonly int _width;
    private readonly Tile[,] _tiles;
    private readonly List<(int, int, Tile)> _positionsToMove;
    private int _robotX;
    private int _robotY;

    public WarehousePart2(string[] lines, int height)
    {
        _height = height;
        _width = lines[0].Length * 2;
        _tiles = new Tile[height, _width];

        for (var y = 0; y < _height; y++)
        {
            var line = lines[y];

            for (var x = 0; x < line.Length; x++)
            {
                switch (line[x])
                {
                    case '.':
                        _tiles[y, x * 2] = Tile.Empty;
                        _tiles[y, x * 2 + 1] = Tile.Empty;
                        break;
                    case '#':
                        _tiles[y, x * 2] = Tile.Wall;
                        _tiles[y, x * 2 + 1] = Tile.Wall;
                        break;
                    case 'O':
                        _tiles[y, x * 2] = Tile.ChestLeft;
                        _tiles[y, x * 2 + 1] = Tile.ChestRight;
                        break;
                    case '@':
                        _tiles[y, x * 2] = Tile.Robot;
                        _tiles[y, x * 2 + 1] = Tile.Empty;
                        
                        _robotX = x * 2;
                        _robotY = y;
                        break;
                }
            }
        }
        
        _positionsToMove = new List<(int, int, Tile)>(_width - 4);
    }

    public void MoveRobot(char instruction)
    {
        _positionsToMove.Clear();
        
        switch (instruction)
        {
            case 'v':
                MoveRobot(_robotX, _robotY, 1, 0);
                break;
            case '^':
                MoveRobot(_robotX, _robotY, -1, 0);
                break;
            case '<':
                MoveRobot(_robotX, _robotY, 0, -1);
                break;
            case '>':
                MoveRobot(_robotX, _robotY, 0, 1);
                break;
        }
    }

    public int GetSumOfChests()
    {
        var sum = 0;
        for (var y = 0; y < _height; y++)
        {
            for (var x = 0; x < _width; x++)
            {
                if (_tiles[y, x] == Tile.ChestLeft)
                {
                    sum += 100 * y + x;
                }
            }
        }

        return sum;
    }

    private void MoveRobot(int x, int y, int yOffset, int xOffset)
    {
        if (!GetPositionsToMove(x, y, xOffset, yOffset))
            return;
        
        _positionsToMove.Reverse();

        foreach (var (x1, y1, _) in _positionsToMove)
        {
            _tiles[y1, x1] = Tile.Empty;
        }
        
        foreach (var (x1, y1, tile) in _positionsToMove)
        {
            _tiles[y1 + yOffset, x1 + xOffset] = tile;
        }
        
        _robotX += xOffset;
        _robotY += yOffset;
    }

    private bool GetPositionsToMove(int x, int y, int xOffset, int yOffset)
    {
        while (_tiles[y, x] != Tile.Wall && _tiles[y, x] != Tile.Empty)
        {
            var tile = _tiles[y, x];
            TryAddPositionToMove((x, y, tile));

            if (tile == Tile.ChestLeft)
            {
                TryAddPositionToMove((x + 1, y, Tile.ChestRight));
                if (yOffset != 0 && !GetPositionsToMove(x + 1, y + yOffset, xOffset, yOffset))
                    return false;
            }

            if (tile == Tile.ChestRight)
            {
                TryAddPositionToMove((x - 1, y, Tile.ChestLeft));
                if (yOffset != 0 && !GetPositionsToMove(x - 1, y + yOffset, xOffset, yOffset))
                    return false;
            }
            
            x += xOffset;
            y += yOffset;
        }
        
        return _tiles[y, x] == Tile.Empty;
    }
    
    private void TryAddPositionToMove((int, int, Tile) position)
    {
        if (!_positionsToMove.Any(e => e.Item1 == position.Item1 && e.Item2 == position.Item2))
        {
            _positionsToMove.Add(position);
        }
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        for (var y = 0; y < _height; y++)
        {
            for (var x = 0; x < _width; x++)
            {
                var c = _tiles[y, x] switch
                {
                    Tile.Wall => '#',
                    Tile.ChestLeft => '[',
                    Tile.ChestRight => ']',
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
    ChestLeft = 4,
    ChestRight = 5,
}
