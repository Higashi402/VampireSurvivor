using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Http.Json;

namespace MvcModel.Json
{
    /// <summary>
    /// Статический класс JsonWriter предоставляет методы для работы с JSON-файлами,
    /// включая добавление, чтение и очистку записей.
    /// </summary>
    public static class JsonWriter
    {
        public static void AddJsonRecord(JsonRecord parNewRecord)
        {
            string filePath = @"C:\Users\Higashi\Desktop\Radick\C4S1\КурсоваяКПО\VimpireSurvivors\records.json";

            List<JsonRecord> records;

            lock (typeof(JsonRecord))
            {
                if (File.Exists(filePath))
                {
                    using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string json = reader.ReadToEnd();
                        records = JsonSerializer.Deserialize<List<JsonRecord>>(json) ?? new List<JsonRecord>();
                    }
                }
                else
                {
                    records = new List<JsonRecord>();
                }

                JsonRecord existingRecord = records.FirstOrDefault(record => record.HeroName == parNewRecord.HeroName);

                if (existingRecord != null)
                {
                    if (parNewRecord.Time > existingRecord.Time)
                    {
                        existingRecord.Time = parNewRecord.Time;
                        existingRecord.HeroName = parNewRecord.HeroName;
                    }
                }
                else
                {
                    records.Add(parNewRecord);
                }

                using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    string updatedJson = JsonSerializer.Serialize(records, new JsonSerializerOptions { WriteIndented = true });
                    writer.Write(updatedJson);
                }
            }
        }


        /// <summary>
        /// Читает список записей рекордов из JSON-файла.
        /// </summary>
        /// <returns>Список записей рекордов.</returns>
        public static List<JsonRecord> ReadJsonRecordList()
        {
            string filePath = @"C:\Users\Higashi\Desktop\Radick\C4S1\КурсоваяКПО\VimpireSurvivors\records.json";

            if (!File.Exists(filePath))
            {
                return new List<JsonRecord>();
            }

            string json = File.ReadAllText(filePath);

            return JsonSerializer.Deserialize<List<JsonRecord>>(json) ?? new List<JsonRecord>();
        }

        /// <summary>
        /// Читает список улучшений из JSON-файла.
        /// </summary>
        /// <returns>Список улучшений.</returns>
        public static List<JsonUpgrade> ReadJsonUpgradeList()
        {
            string filePath = @"C:\Users\Higashi\Desktop\Radick\C4S1\КурсоваяКПО\VimpireSurvivors\upgrades.json";

            if (!File.Exists(filePath))
            {
                return new List<JsonUpgrade>();
            }

            string json = File.ReadAllText(filePath);

            return JsonSerializer.Deserialize<List<JsonUpgrade>>(json) ?? new List<JsonUpgrade>();
        }

        /// <summary>
        /// Очищает все записи рекордов в JSON-файле.
        /// </summary>
        public static void ClearJsonRecords()
        {
            string filePath = @"C:\Users\Higashi\Desktop\Radick\C4S1\КурсоваяКПО\VimpireSurvivors\records.json";

            string emptyJson = JsonSerializer.Serialize(new List<JsonRecord>(), new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, emptyJson);
        }
    }
}