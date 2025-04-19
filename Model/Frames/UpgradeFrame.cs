using MvcModel.Elements;
using MvcModel.Upgrades;
using MvcModel.Сreatures.Heros;
using System.Drawing;

namespace MvcModel.Frames
{
    /// <summary>
    /// Класс UpgradeFrame представляет фрейм для выбора улучшений героя.
    /// Наследуется от базового класса DialogFrame.
    /// </summary>
    public class UpgradeFrame : DialogFrame
    {
        /// <summary>
        /// Событие, возникающее при инициализации фрейма улучшений.
        /// Передает экземпляр фрейма и героя, для которого выбираются улучшения.
        /// </summary>
        public static event Action<UpgradeFrame, Hero> UpgradeFrameInitialized;

        /// <summary>
        /// Конструктор класса UpgradeFrame, принимающий героя.
        /// Вызывает событие UpgradeFrameInitialized при создании экземпляра.
        /// </summary>
        /// <param name="hero">Герой, для которого выбираются улучшения.</param>
        public UpgradeFrame(Hero hero)
        {
            UpgradeFrameInitialized?.Invoke(this, hero);
        }

        /// <summary>
        /// Генерирует список случайных улучшений для героя.
        /// </summary>
        /// <param name="parHero">Герой, для которого генерируются улучшения.</param>
        /// <returns>Список случайных улучшений.</returns>
        public List<UpgradeType> GenerateRandomUpgrades(Hero parHero)
        {
            List<UpgradeType> allUpgradeTypes = Enum.GetValues(typeof(UpgradeType)).Cast<UpgradeType>().ToList();

            List<Upgrade> upgrades = parHero.Upgrades.Where(upgrade => upgrade.Level == 7).ToList();

            List<UpgradeType> missingUpgradeTypes = allUpgradeTypes
                .Except(upgrades.Select(upgrade => upgrade.Type))
                .Where(type => type != UpgradeType.Time)
                .ToList();

            var randomUpgrades = missingUpgradeTypes
                .OrderBy(_ => Guid.NewGuid())
                .Take(3)
                .ToList();

            if (randomUpgrades.Count == 0)
            {
                randomUpgrades.Add(UpgradeType.Time);
            }

            return randomUpgrades;
        }
    }
}