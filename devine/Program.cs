using System;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            ConsoleHelper.Initialize();
            ConsoleHelper.Logo();

            var scanner = new DetectionEngine();
            scanner.SCANNAHUI();

            ConsoleHelper.FinalizeScan(scanner.TotalScope, scanner.Detections);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"error: {ex.Message}");
            Console.ReadKey();
        }
    }
}