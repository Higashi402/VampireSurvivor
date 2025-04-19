using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MvcModel.Upgrades
{
    /// <summary>
    /// Класс OakRoot представляет улучшение "Корень дуба", которое увеличивает здоровье героя.
    /// Наследуется от базового класса Upgrade.
    /// </summary>
    public class OakRoot : Upgrade
    {
        /// <summary>
        /// Уровень улучшения. При изменении уровня увеличивается дополнительное здоровье.
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
                // Увеличение дополнительного здоровья в зависимости от уровня
                if (value == 1)
                {
                    AdditionalHealth += 10;
                }
                else if (value == 2)
                {
                    AdditionalHealth += 20;
                }
                else if (value == 3)
                {
                    AdditionalHealth += 20;
                }
                else if (value == 4)
                {
                    AdditionalHealth += 20;
                }
                else if (value == 5)
                {
                    AdditionalHealth += 30;
                }
                else if (value == 6)
                {
                    AdditionalHealth += 50;
                }
                else if (value == 7)
                {
                    AdditionalHealth += 50;
                }
            }
        }

        /// <summary>
        /// Дополнительное здоровье, предоставляемое улучшением.
        /// </summary>
        public int AdditionalHealth = 0;

        /// <summary>
        /// Конструктор по умолчанию для класса OakRoot.
        /// Инициализирует улучшение с начальным уровнем и типом.
        /// </summary>
        public OakRoot()
        {
            this.Level = 1;
            this.Type = UpgradeType.OakRoot;
        }
    }
}