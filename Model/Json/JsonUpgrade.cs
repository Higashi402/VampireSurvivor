using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcModel.Json
{
    /// <summary>
    /// Класс JsonUpgrade представляет улучшение, которое может быть сериализовано в JSON.
    /// </summary>
    public class JsonUpgrade
    {
        /// <summary>
        /// Название улучшения.
        /// </summary>
        public string Upgrade { get; set; }

        /// <summary>
        /// Описание улучшения.
        /// </summary>
        public string Discription { get; set; }

        /// <summary>
        /// Уровень улучшения.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Конструктор класса JsonUpgrade, инициализирующий улучшение с указанными параметрами.
        /// </summary>
        /// <param name="parUpgrade">Название улучшения.</param>
        /// <param name="parLevel">Уровень улучшения.</param>
        /// <param name="parDiscription">Описание улучшения.</param>
        public JsonUpgrade(string parUpgrade, int parLevel, string parDiscription)
        {
            Upgrade = parUpgrade;
            Discription = parDiscription;
            Level = parLevel;
        }

        /// <summary>
        /// Конструктор по умолчанию для класса JsonUpgrade.
        /// </summary>
        public JsonUpgrade() { }
    }
}