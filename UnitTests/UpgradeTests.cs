using MvcModel.Frames;
using MvcModel.Upgrades;
using MvcModel.Сreatures.Heros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class UpgradeTests
    {
        [TestMethod]
        public void GenerateRandomUpgradesTest()
        {
            var hero = new Hero("Vlad", 100, 10, 10, 3, 1);
            var upgradeFrame = new UpgradeFrame(hero);

            var upgrades = upgradeFrame.GenerateRandomUpgrades(hero);

            Assert.IsNotNull(upgrades, "Список улучшений не должен быть null.");
            Assert.IsTrue(upgrades.Count == 3, "Количество предлагаемых улучшений должно быть равно 3, так как игрок не имеет вообще улучшений");
            Assert.IsFalse(upgrades.Contains(UpgradeType.Time), "Улучшение Time не должно быть в списке, если есть другие варианты.");
        }

        [TestMethod]
        public void GenerateTimeUpgradeTest()
        {
            var hero = new Hero("Vlad", 100, 10, 10, 3, 1);

            foreach (var upgradeType in Enum.GetValues(typeof(UpgradeType)).Cast<UpgradeType>())
            {
                if (upgradeType != UpgradeType.Time)
                {
                    var upgrade = Upgrade.CreateUpgrade(upgradeType);
                    upgrade.Level = 7;
                    hero.Upgrades.Add(upgrade);
                }
            }

            var upgradeFrame = new UpgradeFrame(hero);
            var upgrades = upgradeFrame.GenerateRandomUpgrades(hero);

            Assert.IsTrue(upgrades.Contains(UpgradeType.Time), "Должно быть возвращено улучшение Time, если все остальные улучшения максимальны.");
        }

        [TestMethod]
        public void UpgradeFrameInitializedTest()
        {
            var hero = new Hero("Vlad", 100, 10, 10, 3, 1);
            bool eventRaised = false;

            UpgradeFrame.UpgradeFrameInitialized += (frame, h) =>
            {
                eventRaised = true;
            };

            var upgradeFrame = new UpgradeFrame(hero);

            Assert.IsTrue(eventRaised, "Событие UpgradeFrameInitialized должно быть вызвано.");
        }

        [TestMethod]
        public void CreateWeaponTest()
        {
            var damage = 10;
            var attackSpeed = 800;
            var whipType = UpgradeType.Whip;
            var lightningType = UpgradeType.Lightning;

            var whip = Upgrade.CreateWeapon(damage, attackSpeed, whipType);
            var lightning = Upgrade.CreateWeapon(damage, attackSpeed, lightningType);

            Assert.IsInstanceOfType(whip, typeof(Whip), "Созданный объект должен быть типа Whip.");
            Assert.IsInstanceOfType(lightning, typeof(Lightning), "Созданный объект должен быть типа Lightning.");
            Assert.IsTrue(whip.IsAttacking, "Оружие должно быть активным после создания.");
            Assert.IsTrue(lightning.IsAttacking, "Оружие должно быть активным после создания.");
        } 
    }
}
