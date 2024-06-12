namespace Vurdalakov
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static class DirectoryInfoExtensions
    {
        public static Boolean IsEmpty(this DirectoryInfo directoryInfo) => !directoryInfo.Exists || (!directoryInfo.HasFiles() && !directoryInfo.HasDirectories());

        public static Boolean HasFiles(this DirectoryInfo directoryInfo)
        {
            foreach (var _ in directoryInfo.EnumerateFilesSafe())
            {
                return true;
            }

            return false;
        }

        public static Boolean HasDirectories(this DirectoryInfo directoryInfo)
        {
            foreach (var _ in directoryInfo.EnumerateDirectoriesSafe())
            {
                return true;
            }

            return false;
        }

        public static IEnumerable<FileInfo> EnumerateFilesSafe(this DirectoryInfo directoryInfo, String searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            IEnumerator<String> enumerator = null;

            try
            {
                enumerator = Directory.EnumerateFiles(directoryInfo.FullName, searchPattern, searchOption).GetEnumerator();
            }
            catch
            {
                yield break;
            }

            while (true)
            {
                try
                {
                    if (!enumerator.MoveNext())
                    {
                        break;
                    }
                }
                catch
                {
                }

                yield return new FileInfo(enumerator.Current);
            }
        }

        public static IEnumerable<DirectoryInfo> EnumerateDirectoriesSafe(this DirectoryInfo directoryInfo, String searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            IEnumerator<String> enumerator = null;

            try
            {
                enumerator = Directory.EnumerateDirectories(directoryInfo.FullName, searchPattern, searchOption).GetEnumerator();
            }
            catch
            {
                yield break;
            }

            while (true)
            {
                try
                {
                    if (!enumerator.MoveNext())
                    {
                        break;
                    }
                }
                catch
                {
                }

                yield return new DirectoryInfo(enumerator.Current);
            }
        }

        public static IEnumerable<FileSystemInfo> EnumerateFileSystemInfosSafe(this DirectoryInfo directoryInfo, String searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            IEnumerator<String> enumerator = null;

            try
            {
                enumerator = Directory.EnumerateFileSystemEntries(directoryInfo.FullName, searchPattern, searchOption).GetEnumerator();
            }
            catch
            {
                yield break;
            }

            while (true)
            {
                try
                {
                    if (!enumerator.MoveNext())
                    {
                        break;
                    }
                }
                catch
                {
                }

                var path = enumerator.Current;

                yield return Directory.Exists(path) ? (FileSystemInfo)new DirectoryInfo(path) : new FileInfo(path);
            }
        }
    }
}
