using System.Diagnostics;
using System.IO;
using System.Threading;
using devine.Models;

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

        try
        {
            foreach (var region in GetRwxRegions(process))
            {
                File.AppendAllText(ConsoleHelper.LogFilePath, $"[#] rwx region: {region.BaseAddress:X} {region.RegionSize}\n");
            }
        }
        catch (Exception ex)
        {
            ConsoleHelper.AddProcessLog($"RWX regions error: {ex.Message}");
        }

        string outputFile = $"java_{process.Id}_strings.txt";

        if (stringipoiskda(process.Id, outputFile))
        {
            AnalyzeStrings(outputFile, detectionEngine);
        }
    }

    private struct MemoryBasicInformation
    {
        public ulong BaseAddress;
        public ulong AllocationBase;
        public uint AllocationProtect;
        public ulong RegionSize;
        public uint State;
        public uint Protect;
        public uint Type;
    }

    [System.Runtime.InteropServices.DllImport("kernel32.dll")]
    private static extern int VirtualQueryEx(
        System.IntPtr hProcess,
        System.IntPtr lpAddress,
        out MemoryBasicInformation lpBuffer,
        uint dwLength);

    [System.Runtime.InteropServices.DllImport("kernel32.dll")]
    private static extern bool ReadProcessMemory(
        System.IntPtr hProcess,
        System.IntPtr lpBaseAddress,
        byte[] lpBuffer,
        int dwSize,
        out int lpNumberOfBytesRead);

    private const uint PAGE_EXECUTE_READWRITE = 0x40;
    private const uint MEM_COMMIT = 0x1000;

    private IEnumerable<(ulong BaseAddress, ulong RegionSize)> GetRwxRegions(Process process)
    {
        var regions = new List<(ulong, ulong)>();
        var handle = process.Handle;
        ulong address = 0;
        var mbiSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(typeof(MemoryBasicInformation));
        while (true)
        {
            MemoryBasicInformation mbi;
            int result = VirtualQueryEx(handle, (System.IntPtr)address, out mbi, mbiSize);
            if (result == 0)
                break;
            if ((mbi.State & MEM_COMMIT) != 0 && (mbi.Protect & PAGE_EXECUTE_READWRITE) != 0)
            {
                regions.Add((mbi.BaseAddress, mbi.RegionSize));
            }
            address = mbi.BaseAddress + mbi.RegionSize;
            if (address >= 0x7FFFFFFFFFFF)
                break;
        }
        return regions;
    }

    private bool stringipoiskda(int pid, string outputFile)
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
                        detectionEngine.AddDetection(pattern.Value.Description, pattern.Key);
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
                        detectionEngine.AddDetection(pattern.Value.Description, pattern.Key);
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