
class Timer : IDisposable
{
    private Stopwatch stopwatch;
    private string taskName;

    public Timer(string taskName)
    {
        this.taskName = taskName;
        stopwatch = Stopwatch.StartNew();
    }

    public void Dispose()
    {
        stopwatch.Stop();
        Console.WriteLine($"{taskName} took {stopwatch.ElapsedMilliseconds} ms");
    }
}