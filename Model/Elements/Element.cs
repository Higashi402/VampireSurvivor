using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcModel.Elements
{
    /// <summary>
    /// Абстрактный класс, представляющий базовый элемент.
    /// </summary>
    /// <remarks>
    /// Класс предоставляет базовые свойства для всех элементов, таких как координаты, высота и ширина.
    /// </remarks>
    public abstract class Element
    {
        /// <summary>
        /// Получает или задает координаты элемента.
        /// </summary>
        /// <value>
        /// Координаты элемента.
        /// </value>
        public Point Coordinates { get; set; }

        /// <summary>
        /// Получает или задает высоту элемента.
        /// </summary>
        /// <value>
        /// Высота элемента.
        /// </value>
        public int Height { get; set; }

        /// <summary>
        /// Получает или задает ширину элемента.
        /// </summary>
        /// <value>
        /// Ширина элемента.
        /// </value>
        public int Width { get; set; }
    }
}