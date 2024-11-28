using System.Drawing;
using System.IO;
/*
public struct Pipe
{
    public readonly int AX;
    public readonly int AY;
    public readonly int BX;
    public readonly int BY;

    public Pipe(int ax, int ay, int bx, int by)
    {
        AX = ax;
        AY = ay;
        BX = bx;
        BY = by;
    }
}

internal class Program
{
    private static void Main(string[] args)
    {
        var inputs = File.ReadAllLines("input.txt");

        var grid = new string[inputs.Length, inputs[0].Length];

        var start = new Point();

        for (int y = 0; y < inputs.Length; y++)
        {
            var line = inputs[y].ToCharArray().Select(char.ToString).ToArray();

            for (int x = 0; x < line.Length; x++)
            {
                grid[y, x] = line[x];

                if (line[x] == "S")
                {
                    start = new Point(x, y);
                }
            }
        }

        var pipeTypes = new Dictionary<string, Pipe>
        {
            ["|"] = new Pipe(0, -1, 0, 1),
            ["-"] = new Pipe(-1, 0, 1, 0),
            ["L"] = new Pipe(0, -1, 1, 0),
            ["J"] = new Pipe(0, -1, -1, 0),
            ["7"] = new Pipe(-1, 0, 0, 1),
            ["F"] = new Pipe(1, 0, 0, 1),
            ["."] = new Pipe(0, 0, 0, 0),
        };

        var potentials = new List<Point>
        {
            new Point(start.X - 1, start.Y),
            new Point(start.X + 1, start.Y),
            new Point(start.X, start.Y - 1),
            new Point(start.X, start.Y + 1)
        };

        Point current = new Point();
        for (int i = 0; i < potentials.Count; i++)
        {
            var potential = potentials[i];
            if (CanConnect(start.X, start.Y, potential.X, potential.Y, pipeTypes[grid[potential.Y, potential.X]]))
            {
                current = potential;
                break;
            }
        }

        var previous = start;
        long steps = 0;

        var path = new int[grid.GetLength(0), grid.GetLength(1)];
        var pathStr = new string[grid.GetLength(0), grid.GetLength(1)];

        path[start.Y, start.X] = 1;
        pathStr[start.Y, start.X] = "7";

        while (current.X != start.X || current.Y != start.Y)
        {
            var pipe = pipeTypes[grid[current.Y, current.X]];

            path[current.Y, current.X] = 1;
            pathStr[current.Y, current.X] = grid[current.Y, current.X];

            Point next;

            if ((current.X + pipe.AX) == previous.X && (current.Y + pipe.AY) == previous.Y)
            {
                next = new Point(current.X + pipe.BX, current.Y + pipe.BY);
            }
            else
            {
                next = new Point(current.X + pipe.AX, current.Y + pipe.AY);
            }

            previous = current;

            current = next;

            steps++;
        }



        var resizeFactor = 3;
        var resizedGrid = ScaleUp(pathStr, resizeFactor, pipeTypes);

        DrawMaze(path);
        Console.WriteLine("\n\n\n\n\n\n");

        var totalContained = BFS(resizedGrid) / resizeFactor;

        var scaledDown = ScaleDown(resizedGrid, resizeFactor);

        DrawMaze(scaledDown);

        var totalUnvisited = 0;

        for (int y = 0; y < scaledDown.GetLength(0); y++)
        {
            for (int x = 0; x < scaledDown.GetLength(1); x++)
            {
                if (scaledDown[y, x] == 0)
                {
                    totalUnvisited++;
                }
            }
        }

        DrawMaze(scaledDown);

        Console.WriteLine($"Steps: {Math.Ceiling((float)steps / 2)}");
        Console.WriteLine($"Total Contained: {totalUnvisited}");
    }

    static int BFS(int[,] grid)
    {
        int height = grid.GetLength(0);
        int width = grid.GetLength(1);

        var visited = new bool[height, width];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                visited[y, x] = grid[y, x] == 1;
            }
        }

        Queue<Point> queue = new Queue<Point>();
        queue.Enqueue(new Point(0, 0));
        queue.Enqueue(new Point(0, height - 1));
        queue.Enqueue(new Point(width - 1, 0));
        queue.Enqueue(new Point(width - 1, height - 1));

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            if (current.X < 0 || current.X >= width || current.Y < 0 || current.Y >= height || visited[current.Y, current.X])
            {
                continue;
            }

            visited[current.Y, current.X] = true;
            queue.Enqueue(new Point(current.X - 1, current.Y));
            queue.Enqueue(new Point(current.X + 1, current.Y));
            queue.Enqueue(new Point(current.X, current.Y - 1));
            queue.Enqueue(new Point(current.X, current.Y + 1));
        }

        var totalUnvisited = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var cellVisited = visited[y, x];

                if (!cellVisited)
                {
                    totalUnvisited++;
                }

                grid[y, x] = cellVisited ? 1 : 0;

            }
        }

        return totalUnvisited;
    }

    static void DrawMaze(int[,] maze)
    {
        for (int y = 0; y < maze.GetLength(0); y++)
        {
            for (int x = 0; x < maze.GetLength(1); x++)
            {
                var pathNode = maze[y, x];

                if (pathNode == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("I");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(".");
                }

                Console.ResetColor();
            }

            Console.Write(Environment.NewLine);
        }
    }

    static int[,] ScaleUp(string[,] input, int resizeFactor, Dictionary<string, Pipe> pipeTypes)
    {
        var resized = new int[input.GetLength(0) * resizeFactor, input.GetLength(1) * resizeFactor];

        for (int x = 0; x < resized.GetLength(0); x++)
        {
            for (int y = 0; y < resized.GetLength(1); y++)
            {
                resized[x, y] = 0;
            }
        }

        for (int x = 0; x < input.GetLength(0); x++)
        {
            for (int y = 0; y < input.GetLength(1); y++)
            {
                if (input[x, y] == null) continue;

                var pipe = pipeTypes[input[x, y]];

                for (int i = 0; i < resizeFactor; i++)
                {
                    resized[(x * resizeFactor) + (pipe.AY * i), (y * resizeFactor) + (pipe.AX * i)] = 1;
                }

                for (int i = 0; i < resizeFactor; i++)
                {
                    resized[(x * resizeFactor) + (pipe.BY * i), (y * resizeFactor) + (pipe.BX * i)] = 1;
                }
            }
        }

        return resized;
    }

    static int[,] ScaleDown(int[,] input, int shrinkFactor)
    {
        var resized = new int[input.GetLength(0) / shrinkFactor, input.GetLength(1) / shrinkFactor];

        for (int x = 0; x < resized.GetLength(0); x++)
        {
            for (int y = 0; y < resized.GetLength(1); y++)
            {
                resized[x, y] = 0;
            }
        }

        for (int x = 0; x < resized.GetLength(0); x++)
        {
            for (int y = 0; y < resized.GetLength(1); y++)
            {
                resized[x, y] = input[x * shrinkFactor, y * shrinkFactor];
            }
        }

        return resized;
    }

    static bool CanConnect(int originX, int originY, int targetX, int targetY, Pipe pipe)
    {
        var sideACanConnect = ((targetX + pipe.AX) == originX && (targetY + pipe.AY) == originY);
        var sideBCanConnect = ((targetX + pipe.BX) == originX && (targetY + pipe.BY) == originY);
        return sideACanConnect || sideBCanConnect;
    }
}


/*func part2() {
   	fileContent, err := ioutil.ReadFile("input2.txt")
   	if err != nil {
   		log.Fatal(err)
   	}
   
   	text := string(fileContent)
   	lines := strings.Split(text, "\n")
   
   	for y, line := range lines {
   		//fmt.Println(line)
   		chars := []rune(line)
   		grid = append(grid, chars)
   		for x, c := range chars {
   			if c == 'S' {
   				start = Point{x, y}
   			}
   		}
   	}
   
   	p := start
   	val := 0
   	pipePath := make(map[Point]bool)
   	val = checkPos2(Point{p.x + 1, p.y}, 0, &pipePath)
   	visited = make(map[Point]bool)
   	pipePath2 := make(map[Point]bool)
   	l := checkPos2(Point{p.x - 1, p.y}, 0, &pipePath2)
   	if l > val {
   		pipePath = pipePath2
   		val = l
   	}
   	visited = make(map[Point]bool)
   	pipePath3 := make(map[Point]bool)
   	d := checkPos2(Point{p.x, p.y + 1}, 0, &pipePath3)
   	if d > val {
   		pipePath = pipePath3
   		val = d
   	}
   	visited = make(map[Point]bool)
   	pipePath4 := make(map[Point]bool)
   	u := checkPos2(Point{p.x, p.y - 1}, 0, &pipePath4)
   	if u > val {
   		pipePath = pipePath4
   		val = u
   	}
   
   	// hack: manually patch the start position after inspecting the grid
   	grid[start.y][start.x] = '|'
   	//println("val", val)
   
   	pathElements := pipePath
   	count := 0
   	for y, line := range grid {
   		inside := false
   		lastStart := '-'
   		for x, c := range line {
   			if !pathElements[Point{x, y}] {
   				if inside {
   					count++
   				}
   				continue
   			}
   			if c == '-' {
   				continue
   			}
   			if c == '|' || c == 'F' || c == 'L' {
   				inside = !inside
   				lastStart = c
   			}
   			if c == 'J' && lastStart == 'L' {
   				inside = !inside
   				lastStart = '-'
   			}
   			if c == '7' && lastStart == 'F' {
   				inside = !inside
   				lastStart = '-'
   			}
   		}
   	}
   
   	fmt.Println("part2: ", count)
   }
   
   func checkPos2(p Point, step int, pipePath *map[Point]bool) int {
   	if visited[p] || p.x < 0 || p.y < 0 || p.x >= len(grid[0]) || p.y >= len(grid) || grid[p.y][p.x] == '.' {
   		return step
   	}
   	step++
   	visited[p] = true
   	(*pipePath)[p] = true
   
   	switch grid[p.y][p.x] {
   	case '|':
   		d := checkPos2(Point{p.x, p.y + 1}, step, pipePath)
   		u := checkPos2(Point{p.x, p.y - 1}, step, pipePath)
   		return mathutil.Max(d, u)
   	case '-':
   		r := checkPos2(Point{p.x + 1, p.y}, step, pipePath)
   		l := checkPos2(Point{p.x - 1, p.y}, step, pipePath)
   		return mathutil.Max(r, l)
   	case 'F':
   		r := checkPos2(Point{p.x + 1, p.y}, step, pipePath)
   		d := checkPos2(Point{p.x, p.y + 1}, step, pipePath)
   		return mathutil.Max(r, d)
   	case '7':
   		l := checkPos2(Point{p.x - 1, p.y}, step, pipePath)
   		d := checkPos2(Point{p.x, p.y + 1}, step, pipePath)
   		return mathutil.Max(l, d)
   	case 'J':
   		l := checkPos2(Point{p.x - 1, p.y}, step, pipePath)
   		u := checkPos2(Point{p.x, p.y - 1}, step, pipePath)
   		return mathutil.Max(l, u)
   	case 'L':
   		r := checkPos2(Point{p.x + 1, p.y}, step, pipePath)
   		u := checkPos2(Point{p.x, p.y - 1}, step, pipePath)
   		return mathutil.Max(r, u)
   	case 'S':
   		//println("found", step)
   		break
   	}
   
   	return step
   }*/
