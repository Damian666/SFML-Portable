using System;
using System.IO;
using System.Windows.Forms;
using SFML.Properties;

namespace SFML
{
    public static partial class CSFML
    {
        private static void PackLibraries()
        {
            var cd = Environment.CurrentDirectory + "/";
            var files = new[] { "csfml-win32-2", "csfml-win64-2", "openal32", "libsndfile-1",
                "csfml-audio-2", "csfml-graphics-2", "csfml-window-2" };

            foreach (var file in files)
                if (File.Exists(cd + file + ".dll")) File.Delete(cd + file + ".dll");
        }
        private static void UnpackLibraries()
        {
            var cd = Environment.CurrentDirectory + "/";
            byte[] fBytes, dBytes;

            if (WINDOWS)
            {
                if (IntPtr.Size == 4)
                {
                    var file = cd + "csfml-" + "bit32-2" + ".dll";
                    if (!File.Exists(file)) { PackLibraries(); File.Create(file).Dispose(); }

                    file = cd + "csfml-" + "audio-2" + ".dll";
                    if (!File.Exists(file))
                    {
                        fBytes = Resources.audio_win32;
                        dBytes = Decompress(ref fBytes);
                        File.WriteAllBytes(file, dBytes);
                    }

                    file = cd + "csfml-" + "graphics-2" + ".dll";
                    if (!File.Exists(file))
                    {
                        fBytes = Resources.graphics_win32;
                        dBytes = Decompress(ref fBytes);
                        File.WriteAllBytes(file, dBytes);
                    }

                    file = cd + "csfml-" + "window-2" + ".dll";
                    if (!File.Exists(file))
                    {
                        fBytes = Resources.window_win32;
                        dBytes = Decompress(ref fBytes);
                        File.WriteAllBytes(file, dBytes);
                    }

                    file = cd + "libsndfile-1" + ".dll";
                    if (!File.Exists(file))
                    {
                        fBytes = Resources.libsndfile_win32;
                        dBytes = Decompress(ref fBytes);
                        File.WriteAllBytes(file, dBytes);
                    }

                    file = cd + "openal32" + ".dll";
                    if (!File.Exists(file))
                    {
                        fBytes = Resources.openal_win32;
                        dBytes = Decompress(ref fBytes);
                        File.WriteAllBytes(file, dBytes);
                    }
                }
                else if (IntPtr.Size == 8)
                {
                    var file = cd + "csfml-" + "bit64-2" + ".dll";
                    if (!File.Exists(file)) { PackLibraries(); File.Create(file).Dispose(); }

                    file = cd + "csfml-" + "audio-2" + ".dll";
                    if (!File.Exists(file))
                    {
                        fBytes = Resources.audio_win64;
                        dBytes = Decompress(ref fBytes);
                        File.WriteAllBytes(file, dBytes);
                    }

                    file = cd + "csfml-" + "graphics-2" + ".dll";
                    if (!File.Exists(file))
                    {
                        fBytes = Resources.graphics_win64;
                        dBytes = Decompress(ref fBytes);
                        File.WriteAllBytes(file, dBytes);
                    }

                    file = cd + "csfml-" + "window-2" + ".dll";
                    if (!File.Exists(file))
                    {
                        fBytes = Resources.window_win64;
                        dBytes = Decompress(ref fBytes);
                        File.WriteAllBytes(file, dBytes);
                    }

                    file = cd + "libsndfile-1" + ".dll";
                    if (!File.Exists(file))
                    {
                        fBytes = Resources.libsndfile_win64;
                        dBytes = Decompress(ref fBytes);
                        File.WriteAllBytes(file, dBytes);
                    }

                    file = cd + "openal32" + ".dll";
                    if (!File.Exists(file))
                    {
                        fBytes = Resources.openal_win64;
                        dBytes = Decompress(ref fBytes);
                        File.WriteAllBytes(file, dBytes);
                    }
                }
            }
            else if (LINUX || MAC)
            {
                PackLibraries();
                MessageBox.Show("SFML.Net for Linux/Mac requires installing 'csfml' " +
                                "from your package distributon or manual compilation " +
                                "of the source found at https://www.sfml-dev.org/download/csfml/ " +
                                "using version 2.0", "SFML Install Notice",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else Environment.Exit(0);
        }
    }
}
