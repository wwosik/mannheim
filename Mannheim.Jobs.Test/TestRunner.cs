using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Mannheim.Jobs
{
    public class TestRunner<T> where T : ICommand
    {
        private readonly ITestOutputHelper testOutput;

        public TestRunner(ITestOutputHelper testOutput, string testName, JobStartInfo info = null)
        {
            this.testOutput = testOutput;
            var localAppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            this.WorkingDirectoryPath = Path.Combine(localAppDataFolder, "Mannheim.Jobs.Test", testName);
            this.Info = info ?? new JobStartInfo();
            this.Info.Assemblies = this.Info.Assemblies ?? new List<string>();
            this.Info.Assemblies.Add(Path.Combine(Environment.CurrentDirectory, "Mannheim.Jobs.Test.dll"));
            this.Info.CommandName = typeof(T).FullName;
        }

        private static string MjExecutable { get; }
        private string WorkingDirectoryPath { get; }
        private JobStartInfo Info { get; }

        static TestRunner()
        {
            var libraryRootDir = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent.Parent;
            MjExecutable = Path.Combine(libraryRootDir.FullName, @"Mannheim.Jobs\bin\Debug\netcoreapp2.2\publish\mj.exe");
        }

        public void Run()
        {
            this.testOutput.WriteLine("Path: " + this.WorkingDirectoryPath);

            this.WipeWorkingDirectory();

            var serializedInfo = JsonConvert.SerializeObject(this.Info, Formatting.Indented);
            File.WriteAllText(Path.Combine(this.WorkingDirectoryPath, "job-start-info.json"), serializedInfo);

            var processStartInfo = new ProcessStartInfo
            {
                FileName = MjExecutable,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WorkingDirectory = WorkingDirectoryPath,
                CreateNoWindow = false,
            };

            var process = Process.Start(processStartInfo);
            process.ErrorDataReceived += (s, e) =>
            {
                this.testOutput.WriteLine("ERROR: " + e.Data);
            };
            process.OutputDataReceived += (s, e) =>
            {
                this.testOutput.WriteLine("STD: " + e.Data);
            };

            process.BeginErrorReadLine();
            process.BeginOutputReadLine();

            process.WaitForExit();
        }

        public string[] ReadGeneralLog()
        {
            return File.ReadAllLines(Path.Combine(this.WorkingDirectoryPath, "general.log"));
        }

        private void WipeWorkingDirectory()
        {
            var directory = new DirectoryInfo(this.WorkingDirectoryPath);
            if (directory.Exists)
            {
                foreach (var fi in directory.EnumerateFiles())
                {
                    this.testOutput.WriteLine("Deleting: " + fi.Name);

                    fi.Delete();
                }

                foreach (var subdir in directory.EnumerateDirectories())
                {
                    this.testOutput.WriteLine("Deleting: " + subdir.Name);

                    subdir.Delete(true);
                }
            }
            else
            {
                directory.Create();
            }
        }
    }
}
