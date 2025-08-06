using System;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            ConsoleHelper.Initialize();
            ConsoleHelper.PrintLogo();

            var scanner = new DetectionEngine();
            scanner.ScanSystem();

            ConsoleHelper.FinalizeScan(scanner.TotalScope, scanner.Detections);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"error: {ex.Message}");
            Console.ReadKey();
        }
    }
}