using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcModel.Upgrades
{
    /// <summary>
    /// Перечисление UpgradeType представляет типы улучшений, доступных в игре.
    /// </summary>
    public enum UpgradeType
    {
        /// <summary>
        /// Улучшение "Кнут".
        /// </summary>
        Whip,

        /// <summary>
        /// Улучшение "Молния".
        /// </summary>
        Lightning,

        /// <summary>
        /// Улучшение "Корень дуба".
        /// </summary>
        OakRoot,

        /// <summary>
        /// Улучшение "Точильный камень".
        /// </summary>
        Whetstone,

        /// <summary>
        /// Улучшение "Время".
        /// </summary>
        Time
    }
}