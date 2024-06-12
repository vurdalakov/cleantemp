namespace Vurdalakov.CleanTemp
{
    using System;
    using System.Diagnostics;
    using System.IO;

    internal class Application
    {
        private Int32 _totalDirectories;
        private Int32 _deletedDirectories;

        private Int32 _totalFiles;
        private Int32 _deletedFiles;

        public void Run()
        {
            var tempDirectoryInfo = new DirectoryInfo(Path.GetTempPath());

            Console.WriteLine($"Cleaning '{tempDirectoryInfo.FullName}'...");

            var windowsStartupTime = WindowsComputer.GetStartupTime();
            Console.WriteLine($"Windows startup time: {windowsStartupTime:G}");

            Console.WriteLine("-----------------------------------------------------");

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            this._totalDirectories = 0;
            this._deletedDirectories = 0;

            this._totalFiles = 0;
            this._deletedFiles = 0;

            Console.WriteLine("----- Deleting empty directories");

            foreach (var directoryInfo in tempDirectoryInfo.EnumerateDirectories())
            {
                this._totalDirectories++;

                if (directoryInfo.IsEmpty() || !directoryInfo.HasFiles())
                {
                    this.DeleteDirectory(directoryInfo);
                }
            }

            Console.WriteLine("----- Deleting old directories");

            foreach (var directoryInfo in tempDirectoryInfo.EnumerateDirectories())
            {
                if (directoryInfo.LastWriteTime >= windowsStartupTime)
                {
                    continue;
                }

                var isOld = true;

                foreach (var fileSystemInfo in directoryInfo.EnumerateFileSystemInfosSafe("*.*", SearchOption.AllDirectories))
                {
                    if (fileSystemInfo.LastWriteTime >= windowsStartupTime)
                    {
                        isOld = false;
                        break;
                    }
                }

                if (isOld)
                {
                    this.DeleteDirectory(directoryInfo);
                }
            }

            Console.WriteLine("----- Deleting old files");

            foreach (var fileInfo in tempDirectoryInfo.EnumerateFiles())
            {
                this._totalFiles++;

                if (fileInfo.LastWriteTime < windowsStartupTime)
                {
                    this.DeleteFile(fileInfo);
                }
            }

            stopWatch.Stop();

            Console.WriteLine("-----------------------------------------------------");

            Console.WriteLine($"Finished in {stopWatch.ElapsedMilliseconds:N0} ms");
            Console.WriteLine($"Directories deleted: {this._deletedDirectories:N0} out of {this._totalDirectories:N0}");
            Console.WriteLine($"Files deleted: {this._deletedFiles:N0} out of {this._totalFiles:N0}");
        }

        private void DeleteDirectory(DirectoryInfo directoryInfo)
        {
            try
            {
                var directoryPath = this.AppendLongPathPrefix(directoryInfo.FullName);

                Directory.Delete(directoryPath, true);

                this._deletedDirectories++;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cannot delete directory '{directoryInfo.Name}': {ex.Message}");
            }
        }

        private void DeleteFile(FileInfo fileInfo)
        {
            try
            {
                var filePath = this.AppendLongPathPrefix(fileInfo.FullName);

                File.Delete(filePath);

                this._deletedFiles++;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cannot delete file '{fileInfo.Name}': {ex.Message}");
            }
        }

        private const String LongPathPrefix = @"\\?\";

        private String AppendLongPathPrefix(String filePath) => (filePath != null) && !filePath.StartsWith(LongPathPrefix) ? LongPathPrefix + filePath : filePath;
    }
}
