using SFML.Audio;
using SFML.Graphics;
using System;
using System.Drawing;
using System.IO;
using System.IO.Compression;

namespace SFML
{
    /// <summary>
    /// This class is created to internalize the dependancy libraries
    /// and allow for unpacking needed files at application startup.
    /// </summary>
    public static partial class CSFML
    {
        /// <summary>
        /// Call this before anything else in your app to have unpack
        /// dependancy files. Also internalizes Platform name so user
        /// app may have platform specific calls.
        /// </summary>
        public static void Activate()
        {
            SetPlatform();
            UnpackLibraries();
        }

        public static Music AutoMusic(string path)
        {
            var exts = new[] { "ogg", "wav", "flac", "aiff", "au",
                "raw", "paf", "svx", "nist", "voc", "ircam", "w64",
                 "mat4", "mat5", "pvf", "htk", "sds", "avr","sd2",
                "caf", "wve", "mpc2k", "rf64"};

            foreach (var ext in exts)
                if (File.Exists(path + "." + ext)) return new Music(path + "." + ext);

            throw new Exception("File not found under any valid extension!");
        }

        public static SoundBuffer AutoSound(string path)
        {
            var exts = new[] { "ogg", "wav", "flac", "aiff", "au",
                "raw", "paf", "svx", "nist", "voc", "ircam", "w64",
                 "mat4", "mat5", "pvf", "htk", "sds", "avr","sd2",
                "caf", "wve", "mpc2k", "rf64"};

            foreach (var ext in exts)
                if (File.Exists(path + "." + ext)) return new SoundBuffer(path + "." + ext);

            throw new Exception("File not found under any valid extension!");
        }

        public static Texture AutoTexture(string path)
        {
            var exts = new[] { "bmp", "dds", "jpg", "png", "tga", "psd" };

            foreach (var ext in exts)
                if (File.Exists(path + "." + ext)) return new Texture(path + "." + ext);

            throw new Exception("File not found under any valid extension!");

        }

        public static Texture[] AutoTextureStack(string path)
        {
            var exts = new[] { "bmp", "dds", "jpg", "png", "tga", "psd" };
            var cd = path + "/";
            int size, len;
            Texture[] tex;

            if (!Directory.Exists(cd)) throw new Exception("File not found under any valid extension!");

            size = 0;
            var curSize = 0;
            while (true)
            {
                foreach (var ext in exts)
                    if (File.Exists(cd + size + "." + ext))
                    { size++; break; }
                if (curSize == size) break;
                else curSize = size;
            }
            if (size == 0) throw new Exception("File not found under any valid extension!");

            tex = new Texture[size];
            size = 0;
            curSize = 0;
            while (true)
            {
                foreach (var ext in exts)
                    if (File.Exists(cd + size + "." + ext))
                    {
                        tex[size] = new Texture(cd + size + "." + ext);
                        size++;
                        break;
                    }
                if (curSize == size) break;
                else curSize = size;
            }

            return tex;
        }

        public static Sprite[] AutoSpriteStack(string path)
        {
            var tex = AutoTextureStack(path);
            int len = tex.Length;

            var spr = new Sprite[len];
            for (var i = 0; i < len; i++)
                spr[i] = new Sprite(tex[i]);

            return spr;
        }

        public static Bitmap AutoBitmap(string path)
        {
            var exts = new[] { "bmp", "dds", "jpg", "png", "tga", "psd" };

            foreach (var ext in exts)
                if (File.Exists(path + "." + ext)) return new Bitmap(path + "." + ext);

            throw new Exception("File not found under any valid extension!");
        }

        /// <summary>
        /// Ripped from ArchaicSoft Libraries! You are welcome!
        /// </summary>
        private static byte[] Decompress(ref byte[] bytes)
        {
            MemoryStream memoryStream = new MemoryStream(bytes);
            GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress);
            byte[] buffer1 = new byte[4];
            int length = 0;
            memoryStream.Position = memoryStream.Length - 4L;
            memoryStream.Read(buffer1, 0, 4);
            memoryStream.Position = 0L;
            byte[] buffer2 = new byte[BitConverter.ToInt32(buffer1, 0) + 101];
            while (true)
            {
                int num = gzipStream.Read(buffer2, length, 100);
                if (num == 0)
                    break;

                length += num;
            }
            gzipStream.Close();
            memoryStream.Close();
            byte[] numArray = new byte[length];
            Buffer.BlockCopy(buffer2, 0, numArray, 0, length);
            return numArray;
        }
    }
}