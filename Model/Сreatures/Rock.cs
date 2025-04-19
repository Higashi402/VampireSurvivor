using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcModel.Сreatures
{
    /// <summary>
    /// Класс Rock представляет объект "Камень" на игровой карте.
    /// </summary>
    public class Rock
    {
        /// <summary>
        /// Координата X камня на карте.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Координата Y камня на карте.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Конструктор класса Rock, инициализирующий камень с указанными координатами.
        /// </summary>
        /// <param name="x">Координата X камня.</param>
        /// <param name="y">Координата Y камня.</param>
        public Rock(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}