namespace Vurdalakov
{
    using System;

    public static class WindowsComputer
    {
        public static DateTime GetStartupTime() => DateTime.Now - TimeSpan.FromMilliseconds(Environment.TickCount);
    }
}
