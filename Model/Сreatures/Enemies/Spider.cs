using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcModel.Сreatures.Enemies
{
    /// <summary>
    /// Класс Spider представляет врага типа "Паук".
    /// Наследуется от базового класса Enemy.
    /// </summary>
    public class Spider : Enemy
    {
        /// <summary>
        /// Конструктор класса Spider, инициализирующий паука с указанными параметрами.
        /// </summary>
        /// <param name="parMaxHealth">Максимальное здоровье паука.</param>
        /// <param name="parCurrentHealth">Текущее здоровье паука.</param>
        /// <param name="parX">Координата X паука на карте.</param>
        /// <param name="parY">Координата Y паука на карте.</param>
        /// <param name="parHeight">Высота паука.</param>
        /// <param name="parWidth">Ширина паука.</param>
        /// <param name="parSpeed">Скорость паука.</param>
        public Spider(int parMaxHealth, int parCurrentHealth, int parX, int parY, int parHeight, int parWidth, int parSpeed)
        {
            this.MaxHealth = parMaxHealth;
            this.CurrentHealth = parCurrentHealth;
            this.X = parX;
            this.Y = parY;
            this.Height = parHeight;
            this.Width = parWidth;
            this.Speed = parSpeed;
        }
    }
}