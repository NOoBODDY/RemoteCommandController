﻿using System.Diagnostics;
using System.Text.Json;

namespace ControlService.Core
{
    internal class FileManager<T>
    {
        string _path;

        internal FileManager(string path)
        {
            _path = path;
        }

        internal void CreateFile(string fileName, T data)
        {
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }
            using (StreamWriter writer = new StreamWriter(_path + "/" + fileName, false))
            {
                string text = JsonSerializer.Serialize(data);

                writer.WriteLine(text);
            }
        }

        internal void AddToFile(string fileName, T data)
        {
            using (StreamWriter writer = new StreamWriter(_path + "/" + fileName, true))
            {
                string text = JsonSerializer.Serialize(data);

                writer.WriteLine(text);
            }
        }

        internal T ReadFromFile(string fileName)
        {
            FileInfo info = new FileInfo(_path + "/" + fileName);
            if (info.Exists)
            {
                Trace.WriteLine($"Path = {info.Directory.FullName}");
                using (StreamReader reader = new StreamReader(_path + "/" + fileName))
                {
                    return JsonSerializer.Deserialize<T>(reader.ReadToEnd());
                }
            }
            else
            {
                return default;
            }

        }

    }
}
