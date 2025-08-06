using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

public static class ConsoleHelper
{
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern IntPtr GetStdHandle(int nStdHandle);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool SetConsoleCursorPosition(IntPtr hConsoleOutput, COORD dwCursorPosition);

    [StructLayout(LayoutKind.Sequential)]
    public struct COORD
    {
        public short X;
        public short Y;
    }

    const int STD_OUTPUT_HANDLE = -11;
    const int MIN_WINDOW_WIDTH = 100;
    const int MIN_WINDOW_HEIGHT = 30;
    static readonly IntPtr consoleHandle = GetStdHandle(STD_OUTPUT_HANDLE);
    public static readonly string LogFilePath = "scan_log.txt";

    public static void Initialize()
    {
        Console.Title = "SWAGA";
        Console.CursorVisible = false;

        try
        {
            Console.WindowWidth = Math.Max(MIN_WINDOW_WIDTH, Console.WindowWidth);
            Console.WindowHeight = Math.Max(MIN_WINDOW_HEIGHT, Console.WindowHeight);
        }
        catch { }

        if (File.Exists(LogFilePath)) File.Delete(LogFilePath);
        InitializeLogFile();
    }

    static void InitializeLogFile()
    {
        File.WriteAllText(LogFilePath, $"Scan started at {DateTime.Now:dd.MM.yyyy HH:mm:ss}\n---scan log---\n");
    }

    public static void PrintLogo()
    {
        Console.Clear();
        string[] logoLines = {
            "    .___          .__               ",
            "  __| _/_______  _|__| ____   ____  ",
            " / __ |/ __ \\  \\/ /  |/    \\_/ __ \\ ",
            "/ /_/ \\  ___/\\   /|  |   |  \\  ___/ ",
            "\\____ |\\___  >\\_/ |__|___|  /\\___  >",
            "     \\/    \\/             \\/     \\/  beta"
        };

        foreach (var line in logoLines)
        {
            Console.WriteLine(line);
        }
    }

    public static void AddProcessLog(string message)
    {
        try
        {
            string logMessage = $"[{DateTime.Now:HH:mm:ss}] {message}";
            Console.WriteLine(logMessage);
            File.AppendAllText(LogFilePath, $"[*] {message}\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Failed to log: {ex.Message}");
        }
    }

    public static void FinalizeScan(int totalScope, List<string> detections)
    {
        try
        {
            File.AppendAllText(LogFilePath, "---detections---\n");
            foreach (var detection in detections)
            {
                string logEntry = detection.Replace(" (Pattern: ", "\n   Pattern: ");
                File.AppendAllText(LogFilePath, $"{logEntry}\n");
            }

            File.AppendAllText(LogFilePath, "---verdict---\n");
            File.AppendAllText(LogFilePath, $"{totalScope} scope\n");

            string verdict = totalScope >= 3500 ? "cheater" :
                           totalScope >= 2000 ? "suspicious" : "clean";

            File.AppendAllText(LogFilePath, $"{verdict}\n");

            AddProcessLog($"Scan completed! Verdict: {verdict}. See scan_log.txt");
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Finalization error: {ex.Message}");
        }
    }
}