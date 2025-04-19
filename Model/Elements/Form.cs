using System;
using System.Drawing;

namespace MvcModel.Elements
{
    /// <summary>
    /// Класс Form представляет собой элемент интерфейса в виде формы.
    /// Наследуется от базового класса Element.
    /// </summary>
    public class Form : Element
    {
        /// <summary>
        /// Цвет фона формы.
        /// </summary>
        public ConsoleColor BackGroundColor { get; set; }

        /// <summary>
        /// Конструктор класса Form, инициализирующий форму с указанными параметрами.
        /// </summary>
        /// <param name="parCoordinates">Координаты расположения формы на экране.</param>
        /// <param name="parWidth">Ширина формы.</param>
        /// <param name="parHeight">Высота формы.</param>
        /// <param name="parBackGroundColor">Цвет фона формы.</param>
        public Form(Point parCoordinates, int parWidth, int parHeight, ConsoleColor parBackGroundColor)
        {
            Width = parWidth;
            Height = parHeight;
            BackGroundColor = parBackGroundColor;
            Coordinates = parCoordinates;
        }
    }
}