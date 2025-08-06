using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public class YaraScanner
{
    public void ScanMuiCache(DetectionEngine detectionEngine)
    {
        ConsoleHelper.AddProcessLog("Scanning MUICache...");

        try
        {
            using (var key = Registry.CurrentUser.OpenSubKey(
                @"Software\Classes\Local Settings\Software\Microsoft\Windows\Shell\MuiCache"))
            {
                if (key == null) return;

                var executables = key.GetValueNames()
                    .Select(name => Regex.Replace(name, @"(,.*|\.FriendlyAppName|\.ApplicationCompany)$", ""))
                    .Where(path => path.EndsWith(".exe", StringComparison.OrdinalIgnoreCase) && File.Exists(path))
                    .Distinct()
                    .ToList();

                foreach (var exe in executables)
                {
                    ScanWithYaraRules(exe, detectionEngine);
                }
            }
        }
        catch (Exception ex)
        {
            ConsoleHelper.AddProcessLog($"MUICache scan error: {ex.Message}");
        }
    }

    private void ScanWithYaraRules(string filePath, DetectionEngine detectionEngine)
    {
        if (!File.Exists(filePath)) return;

        try
        {
            string yaraPath = Path.Combine("yara", "yara64.exe");
            if (!File.Exists(yaraPath))
            {
                ConsoleHelper.AddProcessLog("YARA executable not found!");
                return;
            }

            var rules = new[] { "troxill.yar", "nixploit.yar", "m1rch_hb.yar", "suspicious.yar" };

            foreach (var rule in rules)
            {
                if (!File.Exists(rule))
                {
                    ConsoleHelper.AddProcessLog($"Rule not found: {rule}");
                    continue;
                }

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = yaraPath,
                    Arguments = $"-w \"{rule}\" \"{filePath}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(psi))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    if (!string.IsNullOrEmpty(output))
                    {
                        string ruleName = Path.GetFileNameWithoutExtension(rule);
                        detectionEngine.AddDetection(GetScopeForRule(ruleName),
                            $"YARA: {ruleName} in {filePath}",
                            filePath);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ConsoleHelper.AddProcessLog($"YARA scan error for {filePath}: {ex.Message}");
        }
    }

    private int GetScopeForRule(string ruleName)
    {
        return ruleName switch
        {
            "troxill" => 3500,
            "nixploit" => 3500,
            "m1rch_hb" => 3500,
            "suspicious" => 2000,
            _ => 1000
        };
    }
}