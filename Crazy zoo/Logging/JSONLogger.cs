using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Crazy_zoo.Logging
{
    public class JSONLogger : ILogger
    {
        private readonly string _filePath = "log.json";

        public void Log(string message)
        {
            var entry = new
            {
                Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Message = message
            };

            List<object> logs;
            if (File.Exists(_filePath))
            {
                var existing = File.ReadAllText(_filePath);
                logs = JsonSerializer.Deserialize<List<object>>(existing) ?? new List<object>();
            }
            else
            {
                logs = new List<object>();
            }

            logs.Add(entry);
            File.WriteAllText(_filePath, JsonSerializer.Serialize(logs, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}
