using MvcModel.Elements;
using MvcModel.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MvcModel.Frames
{
    /// <summary>
    /// Класс RecordsFrame представляет фрейм для отображения рекордов.
    /// Наследуется от базового класса DialogFrame.
    /// </summary>
    public class RecordsFrame : DialogFrame
    {
        /// <summary>
        /// Ширина поля для отображения номера записи.
        /// </summary>
        public const int NUMBER_WIDTH = 3;

        /// <summary>
        /// Ширина поля для отображения имени игрока.
        /// </summary>
        public const int NAME_WIDTH = 20;

        /// <summary>
        /// Ширина поля для отображения времени.
        /// </summary>
        public const int TIME_WIDTH = 15;

        // Приватное поле для хранения списка рекордов
        private List<JsonRecord> _records = new List<JsonRecord>();

        /// <summary>
        /// Свойство для получения и установки списка рекордов.
        /// </summary>
        public List<JsonRecord> Records
        {
            get
            {
                return _records;
            }
            set
            {
                _records = value;
            }
        }

        /// <summary>
        /// Событие, возникающее при инициализации фрейма рекордов.
        /// </summary>
        public static event Action<RecordsFrame> RecordsFrameInitialized;

        /// <summary>
        /// Конструктор класса RecordsFrame, принимающий список рекордов.
        /// </summary>
        /// <param name="parRecords">Список рекордов для отображения.</param>
        public RecordsFrame(List<JsonRecord> parRecords)
        {
            Records = parRecords;
            RecordsFrameInitialized?.Invoke(this);
        }

        /// <summary>
        /// Центрирует текст в строке заданной ширины.
        /// </summary>
        /// <param name="text">Текст для центрирования.</param>
        /// <param name="width">Ширина строки.</param>
        /// <returns>Центрированный текст.</returns>
        public string CenterText(string text, int width)
        {
            int spaces = width - text.Length;
            int padLeft = spaces / 2;
            int padRight = spaces - padLeft;
            return new string(' ', padLeft) + text + new string(' ', padRight);
        }
    }
}