namespace KycAggregatorService.Services
{
    using System.IO;
    using System.Text.Json;
    using System;

    public class FileCache
    {
        private readonly string _cacheDirectory;

        public FileCache(string cacheDirectory)
        {
            _cacheDirectory = cacheDirectory;
            if (!Directory.Exists(_cacheDirectory))
            {
                Directory.CreateDirectory(_cacheDirectory);
            }
        }

        public void Set<T>(string key, T data)
        {
            string filePath = Path.Combine(_cacheDirectory, $"{key}.json");
            string json = JsonSerializer.Serialize(data);
            File.WriteAllText(filePath, json);
        }

        public T Get<T>(string key)
        {
            string filePath = Path.Combine(_cacheDirectory, $"{key}.json");
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<T>(json);
            }
            return default;
        }

        public void Remove(string key)
        {
            string filePath = Path.Combine(_cacheDirectory, $"{key}.json");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
