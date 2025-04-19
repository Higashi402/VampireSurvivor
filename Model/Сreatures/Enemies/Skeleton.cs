using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcModel.Сreatures.Enemies
{
    /// <summary>
    /// Класс Skeleton представляет врага типа "Скелет".
    /// Наследуется от базового класса Enemy.
    /// </summary>
    public class Skeleton : Enemy
    {
        /// <summary>
        /// Конструктор класса Skeleton, инициализирующий скелета с указанными параметрами.
        /// </summary>
        /// <param name="зparMaxHealth">Максимальное здоровье скелета.</param>
        /// <param name="parCurrentHealth">Текущее здоровье скелета.</param>
        /// <param name="parX">Координата X скелета на карте.</param>
        /// <param name="parY">Координата Y скелета на карте.</param>
        /// <param name="parHeight">Высота скелета.</param>
        /// <param name="parWidth">Ширина скелета.</param>
        /// <param name="parSpeed">Скорость скелета.</param>
        public Skeleton(int зparMaxHealth, int parCurrentHealth, int parX, int parY, int parHeight, int parWidth, int parSpeed)
        {
            this.MaxHealth = зparMaxHealth;
            this.CurrentHealth = parCurrentHealth;
            this.X = parX;
            this.Y = parY;
            this.Height = parHeight;
            this.Width = parWidth;
            this.Speed = parSpeed;
        }
    }
}