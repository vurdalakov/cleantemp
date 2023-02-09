namespace Vurdalakov
{
    using System;
    using System.IO;

    public static class DirectoryInfoExtensions
    {
        public static Boolean IsEmpty(this DirectoryInfo directoryInfo)
        {
            foreach (var _ in directoryInfo.EnumerateFileSystemInfos())
            {
                return false;
            }

            return true;
        }

        public static Boolean HasFiles(this DirectoryInfo directoryInfo)
        {
            foreach (var _ in directoryInfo.EnumerateFiles("*.*", SearchOption.AllDirectories))
            {
                return true;
            }

            return false;
        }
    }
}
