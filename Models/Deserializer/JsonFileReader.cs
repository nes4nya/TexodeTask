using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataGrid.Models.Deserializer
{
    public static class JsonFileReader
    {
        private static async Task<List<T>> ReadAsync<T>(string filePath)
        {
                using FileStream stream = File.OpenRead(filePath);
                var item = await JsonSerializer.DeserializeAsync<List<T>>(stream);

                return item;
        }

        public static async Task<Dictionary<string, List<int?>>> GetStepsOfPersons()
        {
            var path = GetRealPath();

            var i = 1;
            var dayPath = path + "\\day" + i.ToString() + ".json";
            Dictionary<string, List<int?>> stepsOfPersons = new Dictionary<string, List<int?>>();

            while (File.Exists(dayPath))
            {
                var deserializeItem = await JsonFileReader.ReadAsync<DeserializeItem>(dayPath);               
                foreach (DeserializeItem item in deserializeItem)
                {
                    if (stepsOfPersons.ContainsKey(item.User))
                        stepsOfPersons[item.User].Add(item.Steps);
                    else
                
                        stepsOfPersons.Add(item.User, new List<int?>(){null,item.Steps });
                  
                }

                dayPath = path + "\\day" + (++i).ToString() + ".json";
                
            }

            return stepsOfPersons;
        }
        private static string GetRealPath()
        {
            var appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            int indexOfChar = appDir.IndexOf("\\bin");
            appDir = appDir.Remove(indexOfChar, appDir.Length - indexOfChar);

            var relativePath = @"StaticData";
            var fullPath = Path.Combine(appDir, relativePath);

            return fullPath;
        }
    }
}
