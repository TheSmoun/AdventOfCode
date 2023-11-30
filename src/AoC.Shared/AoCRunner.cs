namespace AoC.Shared;

public static class AoCRunner
{
    public static void Run<TAssemblyMarker>(int? number = null)
    {
        var days = typeof(TAssemblyMarker).Assembly.GetTypes()
            .Where(t => !t.IsAbstract && typeof(DayBase).IsAssignableFrom(t))
            .OrderBy(t => int.Parse(t.Name[3..]))
            .Select(t => Activator.CreateInstance(t) as DayBase);

        if (number.HasValue)
        {
            days.Skip(number.Value - 1).FirstOrDefault()?.Run();
        }
        else
        {
            days.Last()?.Run();
        }
    }
}
