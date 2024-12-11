#if DEBUG
var lines = File.ReadAllLines("input.txt");
#else
using var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding);
var lines = reader.ReadToEnd().TrimEnd('\n').Split('\n');
#endif

var memoryParts = lines.Single().Select(chr => chr - '0').ToList();
var memory = new int[memoryParts.Sum()];

var disk = new Disk();

var address = 0;
var isFile = true;
var fileId = 0;

var memoryIdx = 0;
foreach (var length in memoryParts)
{
    var currentFileId = isFile ? fileId++ : -1;
    for (var i = 0; i < length; i++)
    {
        memory[memoryIdx++] = currentFileId;
    }
    
    var space = new StorageSpace(address, length, isFile, currentFileId);
    disk.Add(space);
    
    address += space.Length;
    isFile = !isFile;
}

var nextFreeAddress = 0;
for (var i = memory.Length - 1; i >= 0; i--)
{
    while (memory[nextFreeAddress] != -1)
    {
        nextFreeAddress++;
    }
    
    if (nextFreeAddress >= i)
        break;
    
    var currentFileId = memory[i];
    if (currentFileId == -1)
        continue;

    memory[i] = -1;
    memory[nextFreeAddress++] = currentFileId;
}

var part1 = 0L;
for (var i = 0; i < memory.Length; i++)
{
    if (memory[i] != -1)
    {
        part1 += memory[i] * i;
    }
}

Console.WriteLine($"Part 1: {part1}");

for (var i = disk.Files.Count - 1; i >= 0; i--)
{
    var file = disk.Files[i];
    var (j, freeSpace) = disk.FindFreeSpace(file);
    if (freeSpace is not null)
    {
        file.MoveTo(freeSpace.Address);
        freeSpace.MoveAndShrinkBy(file.Length);
        if (freeSpace.Length == 0)
        {
            disk.FreeSpaces.RemoveAt(j);
        }
    }
}

var part2 = disk.Files.Sum(f => f.Checksum);
Console.WriteLine($"Part 2: {part2}");

internal class Disk
{
    public List<StorageSpace> Files { get; } = [];
    public List<StorageSpace> FreeSpaces { get; } = [];

    public void Add(StorageSpace space)
    {
        if (space.IsFile)
        {
            Files.Add(space);
        }
        else
        {
            FreeSpaces.Add(space);
        }
    }

    public (int, StorageSpace?) FindFreeSpace(StorageSpace space)
    {
        for (var i = 0; i < FreeSpaces.Count; i++)
        {
            var freeSpace = FreeSpaces[i];
            if (freeSpace.Address >= space.Address)
                break;
            
            if (freeSpace.Length >= space.Length)
                return (i, freeSpace);
        }
        
        return (-1, null);
    }
}

internal class StorageSpace
{
    public int Address { get; private set; }
    public int Length { get; private set; }
    public bool IsFile { get; }
    public int FileId { get; }

    public long Checksum
    {
        get
        {
            if (!IsFile)
                return 0L;
            
            var checksum = 0L;
            for (var i = 0; i < Length; i++)
            {
                checksum += (Address + i) * FileId;
            }

            return checksum;
        }
    }
    
    public StorageSpace(int address, int length, bool isFile, int fileId)
    {
        Address = address;
        Length = length;
        IsFile = isFile;
        FileId = fileId;
    }

    public bool Fits(StorageSpace space)
    {
        if (IsFile || !space.IsFile)
            return false;
        
        return Length >= space.Length;
    }

    public void MoveTo(int address)
    {
        Address = address;
    }
    
    public void MoveAndShrinkBy(int amount)
    {
        Address += amount;
        Length -= amount;
    }
}
