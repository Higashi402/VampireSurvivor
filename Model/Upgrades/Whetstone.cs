using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcModel.Upgrades
{
    /// <summary>
    /// Класс Whetstone представляет улучшение "Точильный камень", которое увеличивает урон героя.
    /// Наследуется от базового класса Upgrade.
    /// </summary>
    public class Whetstone : Upgrade
    {
        /// <summary>
        /// Уровень улучшения. При изменении уровня увеличивается дополнительный урон.
        /// </summary>
        public override short Level
        {
            get
            {
                return base.Level;
            }
            set
            {
                base.Level = value;
                // Увеличение дополнительного урона в зависимости от уровня
                if (value == 1)
                {
                    AdditionalDamage += 1;
                }
                else if (value == 2)
                {
                    AdditionalDamage += 2;
                }
                else if (value == 3)
                {
                    AdditionalDamage += 2;
                }
                else if (value == 4)
                {
                    AdditionalDamage += 3;
                }
                else if (value == 5)
                {
                    AdditionalDamage += 3;
                }
                else if (value == 6)
                {
                    AdditionalDamage += 4;
                }
                else if (value == 7)
                {
                    AdditionalDamage += 5;
                }
            }
        }

        /// <summary>
        /// Дополнительный урон, предоставляемый улучшением.
        /// </summary>
        public int AdditionalDamage = 0;

        /// <summary>
        /// Конструктор по умолчанию для класса Whetstone.
        /// Инициализирует улучшение с начальным уровнем и типом.
        /// </summary>
        public Whetstone()
        {
            this.Level = 1;
            Type = UpgradeType.Whetstone;
        }
    }
}