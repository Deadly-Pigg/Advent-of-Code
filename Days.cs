using System.Diagnostics;
public abstract class Days<T>
{
    public void OutputValues(int currDay, int currYear)
    {
        Stopwatch stopwatch = new();

        long before = GC.GetTotalMemory(true);
        stopwatch.Start();
        T result = this.Answer(false);
        stopwatch.Stop();
        string part1Res = $"Part 1: {Math.Round(stopwatch.ElapsedTicks/10000.0,3)}ms, answer: {result}";
        long elapsed = stopwatch.ElapsedTicks;
        stopwatch.Reset();

        stopwatch.Start();
        result = this.Answer(true);
        stopwatch.Stop();
        string part2Res = $"\nPart 2: {Math.Round(stopwatch.ElapsedTicks/10000.0,3)}ms, answer: {result}";
        elapsed += stopwatch.ElapsedTicks;
        var after = GC.GetTotalMemory(true);

        Console.WriteLine($"Day: {currDay}, Year: {currYear}\n" +
                          $"{part1Res}, {part2Res},\n" + 
                          $"Total time taken: {Math.Round(elapsed/10000.0,3)}ms\n" +
                          $"Total memory used: {Math.Round((after-before)/1024.0,3)}Kb\n");
        GC.Collect();
    }
    public abstract T Answer(bool part);
}
