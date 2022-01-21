using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Hulk.Shared
{
    public static class FileUtil
    {
        /// <summary>
        ///     Gets the size of the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static string GetFileSize(this string path)
        {
            var thisFile = new FileInfo(path);
            if (!thisFile.Exists) return "0 Bytes";
            double byteCount = thisFile.Length;
            return byteCount.ToFileSize();
        }

        /// <summary>
        ///     To the size of the file.
        /// </summary>
        /// <param name="contentLength">Length of the content.</param>
        /// <returns></returns>
        public static string ToFileSize(this double contentLength)
        {
            string size = "0 Bytes";
            if (contentLength >= 1073741824.0)
                size = String.Format("{0:##.##}", contentLength / 1073741824.0) + " GB";
            else if (contentLength >= 1048576.0)
                size = String.Format("{0:##.##}", contentLength / 1048576.0) + " MB";
            else if (contentLength >= 1024.0)
                size = String.Format("{0:##.##}", contentLength / 1024.0) + " KB";
            else if (contentLength > 0 && contentLength < 1024.0)
                size = contentLength + " Bytes";

            return size;
        }

        /// <summary>
        ///     To the size of the file.
        /// </summary>
        /// <param name="contentLength">Length of the content.</param>
        /// <returns></returns>
        public static string ToFileSize(this int contentLength)
        {
            string size = "0 Bytes";
            if (contentLength >= 1073741824.0)
                size = String.Format("{0:##.##}", contentLength / 1073741824.0) + " GB";
            else if (contentLength >= 1048576.0)
                size = String.Format("{0:##.##}", contentLength / 1048576.0) + " MB";
            else if (contentLength >= 1024.0)
                size = String.Format("{0:##.##}", contentLength / 1024.0) + " KB";
            else if (contentLength > 0 && contentLength < 1024.0)
                size = contentLength + " Bytes";

            return size;
        }

        /// <summary>
        ///     Creates the folder.
        /// </summary>
        /// <param name="path">The path.</param>
        public static string CreateFolder(this string path)
        {
            if (Directory.Exists(path)) return string.Empty;
            Directory.CreateDirectory(path);
            return path;
        }

        /// <summary>
        ///     Deletes the directory.
        /// </summary>
        /// <param name="targetDir">The target dir.</param>
        public static void DeleteDirectory(this string targetDir)
        {
            if(!Directory.Exists(targetDir)) return;

            string[] files = Directory.GetFiles(targetDir);
            string[] dirs = Directory.GetDirectories(targetDir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
                DeleteDirectory(dir);

            Directory.Delete(targetDir, false);
        }

        /// <summary>
        ///     Gets the name of the valid file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static string GetDuplicateFileName(this string path)
        {
            if (String.IsNullOrEmpty(path)) return null;
            string folder = Path.GetDirectoryName(path);
            if (String.IsNullOrEmpty(folder)) return null;
            if (!File.Exists(path)) return path;
            string filename = Path.GetFileNameWithoutExtension(path);
            int endIndex = filename.LastIndexOf("_", StringComparison.Ordinal);
            if (endIndex == -1) filename += "_1";
            else
            {
                try
                {
                    int position = Int32.Parse(filename.Substring(endIndex + 1)) + 1;
                    filename = filename.Substring(0, endIndex) + "_" + position;
                }
                catch
                {
                    filename += "_1";
                }
            }

            string extension = Path.GetExtension(path);
            string newPath = Path.Combine(folder, filename + extension);
            return GetDuplicateFileName(newPath);
        }

        /// <summary>
        /// Dirs the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static IList<string> DirFile(this string path)
        {
            return !Directory.Exists(path)
                ? new List<string>()
                : Directory.GetFiles(path).Select(Path.GetFileName).ToList();
        }

        /// <summary>
        /// Dirs the folder.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static IList<string> DirFolder(this string path)
        {
            return !Directory.Exists(path)
                ? new List<string>()
                : Directory.GetDirectories(path).Select(Path.GetDirectoryName).ToList();
        }

        /// <summary>
        ///     Saves as.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="filePath">The file path.</param>
        public static void SaveAs(this Byte[] bytes, string filePath)
        {
            File.WriteAllBytes(filePath, bytes);
        }

        /// <summary>
        ///     Reads the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static string ReadFile(this string path)
        {
            using (var sr = new StreamReader(path))
            {
                var sb = new StringBuilder();
                String line;
                while ((line = sr.ReadLine()) != null) sb.AppendLine(line);
                return sb.ToString();
            }
        }

        /// <summary>
        ///     Writes the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="content">The content.</param>
        public static void WriteFile(this string path, string content)
        {
            using (var file = new StreamWriter(path))
            {
                file.Write(content);
            }
        }

        /// <summary>
        ///     Deletes the file.
        /// </summary>
        /// <param name="path">The path.</param>
        public static void DeleteFile(this string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }

        /// <summary>
        ///     To the bytes.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public static Byte[] ToBytes(this string filePath)
        {
            return File.ReadAllBytes(filePath);
        }
    }
}