using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace PoGoBot.Logic.Helpers
{
    internal class Utils
    {
        public static string GetEmbeddedResourceString(string path, Assembly assembly = null)
        {
            string resourceContent = null;
            if (assembly == null)
            {
                assembly = Assembly.GetExecutingAssembly();
            }
            Stream resourceStream = null;
            try
            {
                resourceStream = assembly.GetManifestResourceStream(path);
                if (resourceStream != null)
                {
                    using (var streamReader = new StreamReader(resourceStream))
                    {
                        resourceStream = null;
                        resourceContent = streamReader.ReadToEnd();
                    }
                }
            }
            finally
            {
                resourceStream?.Dispose();
            }
            return resourceContent;
        }

        public static IEnumerable<T> CreateInstancesOf<T>(Assembly assembly = null, params object[] args)
            where T : class
        {
            if (assembly == null)
            {
                assembly = Assembly.GetExecutingAssembly();
            }
            var type = typeof (T);
            var modules = assembly.GetTypes().Where(p => type.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract);
            return modules.Select(module => (T) Activator.CreateInstance(module, args));
        }

        public static T GenerateResource<T>(out bool newFile) where T : class
        {
            newFile = false;
            T data;
            var name = typeof (T).Name;
            var path = ConfigurationManager.AppSettings[$"PoGoBot.Logic.{name}"];
            if (string.IsNullOrEmpty(path))
            {
                path = $"{name.ToLower()}.json";
            }
            var directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory))
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
            var defaultSettingsStr = GetEmbeddedResourceString($"PoGoBot.Logic.Resources.{name.ToLower()}.json");
            var defaultSettings = JsonConvert.DeserializeObject<T>(defaultSettingsStr);
            if (!File.Exists(path))
            {
                File.WriteAllText(path, defaultSettingsStr);
                data = defaultSettings;
                newFile = true;
            }
            else
            {
                var settingsStr = File.ReadAllText(path);
                data = JsonConvert.DeserializeObject<T>(settingsStr);
            }
            return data;
        }

        public static double RandomizeCoordinate(double coordinate)
        {
            Random rnd = new Random();
            return coordinate + (rnd.Next(100) - 50.0) / 1000000;
        }
    }
}