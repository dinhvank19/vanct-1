using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace POS.Shared
{
    public class Translater
    {
        private static string _pathLang;
        private static readonly object Locker = new object();
        private JObject Dictionary { set; get; }
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
            if (File.Exists(_pathLang))
            {
                var json = _pathLang.ReadFile();
                if(!json.IsJson())
                    throw new Exception("Invalid language file");

                Dictionary = JsonConvert.DeserializeObject<JObject>(json);
            }
            else File.Create(_pathLang);
        }

        /// <summary>
        /// Translates the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public string Translate(string key)
        {
            if (string.IsNullOrEmpty(key))
                return String.Empty;

            lock (Locker)
            {
                var node = Dictionary.Properties().SingleOrDefault(i => i.Name.Equals(key));
                if (node != null)
                    return (string) node.Value;

                Dictionary[key] = key;

                var json = JsonConvert.SerializeObject(Dictionary, Formatting.Indented);

                _pathLang.WriteFile(json);

                return key;
            }
        }
    }
}