using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace FredsWorkmate.Util
{
    public static class Settings
    {
        private static readonly List<JsonDocument> documents = [];

        public static void LoadJson(string filePath)
        {
            JsonDocument document = JsonDocument.Parse(File.ReadAllText(filePath));
            documents.Insert(0, document);
        }

        public static T GetNotNull<T>(string key)
        {
            T? tmp = Get<T>(key);
            if (tmp == null)
            {
                throw new NullReferenceException(key);
            }
            return tmp;
        }

        public static T? Get<T>(string key)
        {
            foreach (var doc in documents)
            {
                if (doc.RootElement.TryGetProperty(key, out JsonElement element))
                {
                    return element.Deserialize<T>();
                }
            }
            throw new Exception("Could not find " + key + " in settings");
        }

        public static T GetNotNull<T>(string key, T defaultValue)
        {
            T? tmp = Get<T>(key, defaultValue);
            if (tmp == null)
            {
                throw new NullReferenceException(key);
            }
            return tmp;
        }

        public static T? Get<T>(string key, T? defaultValue)
        {
            foreach (var doc in documents)
            {
                if (doc.RootElement.TryGetProperty(key, out JsonElement element))
                {
                    return element.Deserialize<T>();
                }
            }
            return defaultValue;
        }

        static Settings()
        {
            LoadJson("settings.json");
            if (File.Exists("local_settings.json"))
            {
                LoadJson("local_settings.json");
            }
        }
    }
}
