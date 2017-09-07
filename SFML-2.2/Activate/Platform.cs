using System;
using System.IO;

namespace SFML
{
    public static partial class CSFML
    {
        /// <summary>
        /// If running on Windows OS, will return true.
        /// </summary>
        public static bool WINDOWS { get; private set; }

        /// <summary>
        /// If running on Linux OS, will return true.
        /// </summary>
        public static bool LINUX { get; private set; }

        /// <summary>
        /// If running on Mac OS, will return true.
        /// </summary>
        public static bool MAC { get; private set; }

        /// <summary>
        /// Detects current platform.
        /// </summary>
        internal static void SetPlatform()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                    if (Directory.Exists("/Applications") && Directory.Exists("/System") && Directory.Exists("/Users") &&
                        Directory.Exists("/Volumes"))
                        MAC = true;
                    else
                        LINUX = true;
                    break;
                case PlatformID.MacOSX:
                    MAC = true;
                    break;
                case PlatformID.Win32Windows:
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.WinCE:
                    WINDOWS = true;
                    break;
                case PlatformID.Xbox:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
