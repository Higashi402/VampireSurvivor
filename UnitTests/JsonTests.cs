using MvcModel.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class JsonTests
    {
        private const string TestFilePath = @"C:\Users\Higashi\Desktop\Subjects\course1semester\КурсоваяКПО\VimpireSurvivors\records.json";

        [TestMethod]
        public void AddJsonRecordTest()
        {
            Random random = new Random();
            string playerName = "TestPlayer" + random.Next(100, 10000);
            var newRecord = new JsonRecord { HeroName = playerName, Time = 1 };
            File.Delete(TestFilePath);

            JsonWriter.AddJsonRecord(newRecord);

            Assert.IsTrue(File.Exists(TestFilePath), "Файл должен быть создан.");

            string json = File.ReadAllText(TestFilePath);
            var records = JsonSerializer.Deserialize<List<JsonRecord>>(json);

            Assert.IsNotNull(records, "Записи не должны быть null.");
            Assert.AreEqual(1, records.Count, "Должна быть добавлена одна запись.");
            Assert.AreEqual(playerName, records[0].HeroName, "Имя записи должно совпадать.");
            Assert.AreEqual(1, records[0].Time, "Счет записи должен совпадать.");
        }

        [TestMethod]
        public void ReadJsonRecordListTest()
        {
            var records = new List<JsonRecord>
            {
                new JsonRecord { HeroName = "Player1", Time = 200 },
                new JsonRecord { HeroName = "Player2", Time = 150 }
            };
            string json = JsonSerializer.Serialize(records, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(TestFilePath, json);

            var result = JsonWriter.ReadJsonRecordList();

            Assert.IsTrue(result.Count > 1, "В файле должны быть записи.");
        }

        [TestMethod]
        public void ClearJsonRecordsTest()
        {
            JsonWriter.ClearJsonRecords();

            var result = JsonWriter.ReadJsonRecordList();
            Assert.AreEqual(0, result.Count, "Файл должен быть пустым после очистки.");
        }
    }
}
