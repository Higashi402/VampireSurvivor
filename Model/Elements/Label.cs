using System; 
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcModel.Elements
{
    /// <summary>
    /// Класс Label
    /// Наследуется от базового класса Element и представляет текстовую метку.
    /// </summary>
    public class Label : Element
    {
        /// <summary>
        /// Свойство Text
        /// Получает или задает текст метки.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Свойство BackgroundColor
        /// Получает или задает цвет фона метки.
        /// </summary>
        public ConsoleColor BackgroundColor { get; set; }

        /// <summary>
        /// Свойство TextColor
        /// Получает или задает цвет текста метки.
        /// </summary>
        public ConsoleColor TextColor { get; set; }

        /// <summary>
        /// Конструктор класса Label
        /// Инициализирует новый экземпляр метки с указанными параметрами.
        /// </summary>
        /// <param name="parText">Текст метки.</param>
        /// <param name="parCoordinates">Координаты расположения метки.</param>
        /// <param name="parHeight">Высота метки.</param>
        /// <param name="parWidth">Ширина метки.</param>
        public Label(string parText, Point parCoordinates, int parHeight, int parWidth)
        {
            this.Text = parText;
            this.Coordinates = parCoordinates;
            this.Height = parHeight;
            this.Width = parWidth;
        }
    }
}