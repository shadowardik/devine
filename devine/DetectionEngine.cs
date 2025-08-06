using System.Collections.Generic;

public class DetectionEngine
{
    public int TotalScope { get; private set; }
    public List<string> Detections { get; } = new List<string>();

    private readonly ProcessScanner _processScanner = new ProcessScanner();
    private readonly YaraScanner _yaraScanner = new YaraScanner();

    public void SCANNAHUI()
    {
        DependencyInstaller.DownloadTools();

        ConsoleHelper.AddProcessLog("Starting scan...");

        _processScanner.ScanJavaProcesses(this);

        _yaraScanner.ScanMuiCache(this);
    }

    public void AddDetection(int scope, string detectionType, string pattern = "")
    {
        TotalScope += scope;
        string logMessage = $"[+] {scope} scope - {detectionType}";

        if (!string.IsNullOrEmpty(pattern))
        {
            logMessage += $"\n   Pattern: {pattern}";
        }

        Detections.Add(logMessage);
    }
}