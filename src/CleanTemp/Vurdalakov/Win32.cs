namespace Vurdalakov
{
    using System;
    using System.Runtime.InteropServices;

    public static class WindowsComputer
    {
        public static DateTime GetStartupTime()
            => DateTime.Now - new TimeSpan(GetTickCount64() * 10000);

        [DllImport("Kernel32.dll")]
        private static extern Int64 GetTickCount64();
    }
}
