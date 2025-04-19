using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcModel.Сreatures.Enemies
{
    /// <summary>
    /// Класс Golem представляет врага типа "Голем".
    /// Наследуется от базового класса Enemy.
    /// </summary>
    public class Golem : Enemy
    {
        /// <summary>
        /// Конструктор класса Golem, инициализирующий голема с указанными параметрами.
        /// </summary>
        /// <param name="parMaxHealth">Максимальное здоровье голема.</param>
        /// <param name="parCurrentHealth">Текущее здоровье голема.</param>
        /// <param name="parX">Координата X голема на карте.</param>
        /// <param name="parY">Координата Y голема на карте.</param>
        /// <param name="parHeight">Высота голема.</param>
        /// <param name="parWidth">Ширина голема.</param>
        /// <param name="parSpeed">Скорость голема.</param>
        public Golem(int parMaxHealth, int parCurrentHealth, int parX, int parY, int parHeight, int parWidth, int parSpeed)
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