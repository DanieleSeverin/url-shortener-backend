namespace UrlShortener;

public static class Logger
{
    public static void Log(string message)
    {
        System.Diagnostics.Trace.TraceInformation($"TraceInformation: {message}");
        System.Diagnostics.Trace.WriteLine($"Trace.WriteLine: {message}");
        Console.WriteLine($"Console.WriteLine: {message}");
    }
}
