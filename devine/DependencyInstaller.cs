using System.IO;
using System.Net;

public static class DependencyInstaller
{
    public static void DownloadTools()
    {
        ConsoleHelper.AddProcessLog("Downloading dependencies...");

        try
        {
            DownloadStrings2();
            DownloadYara();
            DownloadYaraRules();
        }
        catch (Exception ex)
        {
            ConsoleHelper.AddProcessLog($"Download error: {ex.Message}");
        }
    }

    private static void DownloadStrings2()
    {
        if (!File.Exists("strings2.exe"))
        {
            new WebClient().DownloadFile(
                "https://github.com/glmcdona/strings2/releases/latest/download/strings2.exe",
                "strings2.exe");
        }
    }

    private static void DownloadYara()
    {
        if (!Directory.Exists("yara"))
        {
            Directory.CreateDirectory("yara");
            string yaraZip = "yara64.zip";
            new WebClient().DownloadFile(
                "https://github.com/VirusTotal/yara/releases/latest/download/yara-master-v4.5.4-win64.zip",
                yaraZip);
            System.IO.Compression.ZipFile.ExtractToDirectory(yaraZip, "yara");
            File.Delete(yaraZip);
        }
    }

    private static void DownloadYaraRules()
    {
        var rules = new Dictionary<string, string>
        {
            {"troxill.yar", "https://github.com/shadowardik/yara-for-devine/raw/main/troxill.yar"},
            {"nixploit.yar", "https://github.com/shadowardik/yara-for-devine/raw/main/nixploit.yar"},
            {"m1rch_hb.yar", "https://github.com/shadowardik/yara-for-devine/raw/main/m1rch_hb.yar"},
            {"suspicious.yar", "https://raw.githubusercontent.com/shadowardik/yara-for-devine/main/suspicious.yar"}
        };

        foreach (var rule in rules)
        {
            try
            {
                if (!File.Exists(rule.Key))
                {
                    new WebClient().DownloadFile(rule.Value, rule.Key);
                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.AddProcessLog($"Failed to download {rule.Key}: {ex.Message}");
            }
        }
    }
}