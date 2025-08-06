using System.Diagnostics;
using System.IO;
using System.Threading;

public class ProcessScanner
{
    public void ScanJavaProcesses(DetectionEngine detectionEngine)
    {

        var heaviestProcess = GetHeaviestJavaProcess();
        if (heaviestProcess != null)
        {
            ScanJavaProcess(heaviestProcess, detectionEngine);
        }
    }

    private Process GetHeaviestJavaProcess()
    {
        Process heaviest = null;
        long maxWorkingSet = 0;

        foreach (var process in Process.GetProcessesByName("javaw"))
        {
            try
            {
                if (process.WorkingSet64 > maxWorkingSet)
                {
                    maxWorkingSet = process.WorkingSet64;
                    heaviest = process;
                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.AddProcessLog($"Error open process {process.Id}: {ex.Message}");
            }
        }

        return heaviest;
    }

    private void ScanJavaProcess(Process process, DetectionEngine detectionEngine)
    {
        ConsoleHelper.AddProcessLog($"Scanning Java process: {process.ProcessName} (PID: {process.Id})");

        string outputFile = $"java_{process.Id}_strings.txt";

        if (RunStringsUtility(process.Id, outputFile))
        {
            AnalyzeStrings(outputFile, detectionEngine);
        }
    }

    private bool RunStringsUtility(int pid, string outputFile)
    {
        try
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "strings2.exe",
                Arguments = $"-pid {pid} -l 4",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(psi))
            {
                using (var writer = File.CreateText(outputFile))
                {
                    while (!process.StandardOutput.EndOfStream)
                    {
                        string line = process.StandardOutput.ReadLine();
                        writer.WriteLine(line);
                    }
                }

                process.WaitForExit();

                return process.ExitCode == 0;
            }
        }
        catch (Exception ex)
        {
            ConsoleHelper.AddProcessLog($"Strings error: {ex.Message}");
            return false;
        }
    }

    private void AnalyzeStrings(string filePath, DetectionEngine detectionEngine)
    {
        try
        {
            Thread.Sleep(500);

            if (!File.Exists(filePath))
            {
                ConsoleHelper.AddProcessLog($"Strings output not found: {filePath}");
                return;
            }

            string content = File.ReadAllText(filePath);

            foreach (var pattern in Patterns.GetStringPatterns())
            {
                try
                {
                    if (content.Contains(pattern.Key))
                    {
                        detectionEngine.AddDetection(pattern.Value.scope, pattern.Value.description, pattern.Key);
                    }
                }
                catch (Exception ex)
                {
                    ConsoleHelper.AddProcessLog($"String pattern error ({pattern.Key}): {ex.Message}");
                }
            }

            foreach (var pattern in Patterns.GetRegexPatterns())
            {
                try
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(content, pattern.Key,
                        System.Text.RegularExpressions.RegexOptions.None,
                        TimeSpan.FromSeconds(10)))
                    {
                        detectionEngine.AddDetection(pattern.Value.scope, pattern.Value.description, pattern.Key);
                    }
                }
                catch (System.Text.RegularExpressions.RegexMatchTimeoutException)
                {
                    ConsoleHelper.AddProcessLog($"timeout for pattern: {pattern.Key}");
                }
                catch (ArgumentException ex)
                {
                    ConsoleHelper.AddProcessLog($"Invalid regex pattern '{pattern.Key}': {ex.Message}");
                }
                catch (Exception ex)
                {
                    ConsoleHelper.AddProcessLog($"Regex error ({pattern.Key}): {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            ConsoleHelper.AddProcessLog($"error: {ex.Message}");
        }
        finally
        {
            try { File.Delete(filePath); } catch { }
        }
    }
}