using System;
using System.IO;
using System.Text.Json;

namespace StorageService
{
    public class JsonFileStorageService : IStorageService
    {
        public void Load<T>(string key, Action<T> callback)
        {
            string path = BuildPath(key);

            var options = new JsonSerializerOptions()
            {
                IncludeFields = true,
            };

            if (File.Exists(path))
            {
                using (var fileStream = new StreamReader(path))
                {
                    var json = fileStream.ReadToEnd();
                    var data = JsonSerializer.Deserialize<T>(json, options);

                    callback.Invoke(data);
                }
            }
            else
                callback.Invoke(default);
        }

        public void Save(string key, object data, Action<bool> callback = null)
        {
            var path = BuildPath(key);
            var options = new JsonSerializerOptions()
            {
                IncludeFields = true,
            };
            string json = JsonSerializer.Serialize(data, options);

            using (var fileStream = new StreamWriter(path))
            {
                fileStream.Write(json);
            }

            callback?.Invoke(true);
        }

        private string BuildPath(string key)
        {
            return Path.Combine(UnityEngine.Application.persistentDataPath, key);
        }
    }
}
