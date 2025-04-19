using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MvcModel.Json
{
    /// <summary>
    /// Класс JsonRecord представляет запись рекорда, которая может быть сериализована в JSON.
    /// </summary>
    public class JsonRecord
    {
        /// <summary>
        /// Имя героя, связанное с рекордом.
        /// </summary>
        public string HeroName { get; set; }

        /// <summary>
        /// Время, за которое был достигнут рекорд (в секундах или других единицах измерения).
        /// </summary>
        public int Time { get; set; }

        /// <summary>
        /// Конструктор класса JsonRecord, инициализирующий запись рекорда.
        /// </summary>
        /// <param name="parHeroName">Имя героя.</param>
        /// <param name="parTime">Время, за которое был достигнут рекорд.</param>
        public JsonRecord(string parHeroName, int parTime)
        {
            HeroName = parHeroName;
            Time = parTime;
        }

        /// <summary>
        /// Конструктор по умолчанию для класса JsonRecord.
        /// </summary>
        public JsonRecord() { }
    }
}