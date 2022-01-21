using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Hulk.Shared
{
    public class Translater : Dictionary<string, string>
    {
        private static string _pathLang;
        private static readonly object Locker = new object();
        public Translater(string path)
        {
            lock (Locker)
            {
                Reload(path);
            }
        }

        /// <summary>
        /// Reloads the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        public void Reload(string path)
        {
            _pathLang = path;
            if (File.Exists(path))
            {
                var listText = File.ReadAllLines(path);
                const string pattern = @"^(Key: "")(?<key>.+)("",Text: "")(?<value>.+)("")$";
                foreach (var m in from s in listText let myRegex = new Regex(pattern) select myRegex.Match(s))
                {
                    if (this.Count(i => i.Key == m.Groups["key"].Value) == 0)
                        Add(m.Groups["key"].Value, m.Groups["value"].Value);
                }
            }
            else File.Create(path);
        }

        /// <summary>
        /// Translates the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public string Translate(string key)
        {
            if (string.IsNullOrEmpty(key))
                return string.Empty;
            lock (Locker)
            {
                var ret = this.FirstOrDefault(l => l.Key == key);
                if (!string.IsNullOrEmpty(ret.Value)) return ret.Value;

                lock (Locker)
                {
                    Add(key, key);
                    File.AppendAllLines(_pathLang, new[] {string.Format("Key: \"{0}\",Text: \"{0}\"", key)});
                }

                return key;
            }
        }
    }
}