using System;
using System.IO;

public static class SimpleLogger
{
    public static string TargetLogPath { get; set; } = string.Empty;

    public static void Log(string message)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(TargetLogPath))
                throw new InvalidOperationException("TargetLogPath is not set.");

            if (!Directory.Exists(TargetLogPath))
                Directory.CreateDirectory(TargetLogPath);

            var logFile = Path.Combine(TargetLogPath, "log.txt");
            var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}{Environment.NewLine}";

            File.AppendAllText(logFile, logEntry);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Logging failed: {ex.Message}");
        }
    }
}
