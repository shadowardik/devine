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
        string logMessage;
        if (detectionType.StartsWith("YARA: ") && !string.IsNullOrEmpty(pattern))
        {
            int idx = detectionType.IndexOf(" in ");
            string shortType = idx > 0 ? detectionType.Substring(0, idx) : detectionType;
            logMessage = $"[+] {scope} scope - {shortType}\n   File: {pattern}";
        }
        else if (!string.IsNullOrEmpty(pattern))
        {
            logMessage = $"[+] {scope} scope - {detectionType}\n   String: {pattern}";
        }
        else
        {
            logMessage = $"[+] {scope} scope - {detectionType}";
        }
        Detections.Add(logMessage);
    }
}