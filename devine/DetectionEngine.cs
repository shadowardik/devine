using System.Collections.Generic;
using devine.Models;

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

    public void AddDetection(string detectionType, string pattern = "")
    {
        TotalScope += 1;
        string logMessage;
        if (detectionType.StartsWith("YARA: ") && !string.IsNullOrEmpty(pattern))
        {
            int idx = detectionType.IndexOf(" in ");
            string shortType = idx > 0 ? detectionType.Substring(0, idx) : detectionType;
            logMessage = $"[+] {shortType}\n   File: {pattern}";
        }
        else if (!string.IsNullOrEmpty(pattern))
        {
            logMessage = $"[+] {detectionType}\n   String: {pattern}";
        }
        else
        {
            logMessage = $"[+] {detectionType}";
        }
        Detections.Add(logMessage);
    }
}