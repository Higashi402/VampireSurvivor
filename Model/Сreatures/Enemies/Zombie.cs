using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcModel.Сreatures.Enemies
{
    /// <summary>
    /// Класс Zombie представляет врага типа "Зомби".
    /// Наследуется от базового класса Enemy.
    /// </summary>
    public class Zombie : Enemy
    {
        /// <summary>
        /// Конструктор класса Zombie, инициализирующий зомби с указанными параметрами.
        /// </summary>
        /// <param name="parMaxHealth">Максимальное здоровье зомби.</param>
        /// <param name="parCurrentHealth">Текущее здоровье зомби.</param>
        /// <param name="parX">Координата X зомби на карте.</param>
        /// <param name="parY">Координата Y зомби на карте.</param>
        /// <param name="parHeight">Высота зомби.</param>
        /// <param name="parWidth">Ширина зомби.</param>
        /// <param name="parSpeed">Скорость зомби.</param>
        public Zombie(int parMaxHealth, int parCurrentHealth, int parX, int parY, int parHeight, int parWidth, int parSpeed)
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