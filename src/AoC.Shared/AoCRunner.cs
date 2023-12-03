namespace AoC.Shared;

public static class AoCRunner
{
    public static void Run<TAssemblyMarker>(int? number = null)
    {
        var days = GetAllDays<TAssemblyMarker>();

        if (number.HasValue)
        {
            days.Skip(number.Value - 1).FirstOrDefault()?.Run();
        }
        else
        {
            days.Last()?.Run();
        }
    }

    public static void RunAll<TAssemblyMarker>()
    {
        foreach (var day in GetAllDays<TAssemblyMarker>())
        {
            day?.Run();
        }
    }

    private static IEnumerable<DayBase?> GetAllDays<TAssemblyMarker>()
    {
        return typeof(TAssemblyMarker).Assembly.GetTypes()
            .Where(t => !t.IsAbstract && typeof(DayBase).IsAssignableFrom(t))
            .OrderBy(t => int.Parse(t.Name[3..]))
            .Select(t => Activator.CreateInstance(t) as DayBase);
    }
}
