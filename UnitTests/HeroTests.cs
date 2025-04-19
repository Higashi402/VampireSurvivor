using MvcController;
using MvcModel;
using MvcModel.Frames;
using MvcModel.Upgrades;
using MvcModel.Сreatures.Enemies;
using MvcModel.Сreatures.Heros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class HeroTests
    {
        [TestMethod]
        public void EarnExperienceTest()
        {
            var hero = new Hero("TestHero", 100, 0, 0, 10, 10);
            var zombie = new Zombie(100, 0, 50, 50, 3, 1,300);

            var levelFrame = new LevelFrame();
            levelFrame.Player = hero;

            LevelController.Initialize(levelFrame);

            hero.EarnExperience(zombie);

            Assert.AreEqual(10, hero.Experience, "Опыт должен увеличиться на 10 за убийство зомби.");
            Assert.AreEqual(1, hero.Level, "Уровень не должен измениться, так как опыта недостаточно для повышения.");

            for (int i = 0; i < 10; i++)
            {
                hero.EarnExperience(zombie);
            }

            Assert.AreEqual(110, hero.Experience, "Опыт должен быть 110 после 11 убийств зомби.");
            Assert.AreEqual(2, hero.Level, "Уровень должен повыситься до 2.");
        }

        [TestMethod]
        public void AddUpgradeTest()
        {
            var hero = new Hero("TestHero", 100, 0, 0, 10, 10);

            var levelFrame = new LevelFrame();
            levelFrame.Player = hero;

            LevelController.Initialize(levelFrame);

            hero.AddUpgrade(UpgradeType.Lightning);
            hero.AddUpgrade(UpgradeType.Lightning);

            var lightningUpgrade = hero.Upgrades.FirstOrDefault(u => u.Type == UpgradeType.Lightning);
            Assert.IsNotNull(lightningUpgrade, "Улучшение Lightning должно быть добавлено.");
            Assert.AreEqual(2, lightningUpgrade.Level, "Уровень улучшения Whip должен быть 2.");
        }

        [TestMethod]
        public void FindNearestEnemyTest()
        {
            var hero = new Hero("TestHero", 100, 50, 50, 10, 10);

            var levelFrame = new LevelFrame();
            levelFrame.Player = hero;

            var levelController = LevelController.GetInstance(levelFrame);
            LevelController.Initialize(levelFrame);

            GameWindow.GetInstance().Resize(160, 62);

            var enemies = new SynchronizedList<Enemy>();
            enemies.Add(new Zombie(60, 60, 55, 55, 3, 1,300));

            Random random = new Random();
            for(int i = 0; i < 50; i++)
            {
                enemies.Add(new Zombie(100, 100, 100, random.Next(0, 100), 3, 1, 300));
            }

            var nearestEnemy = hero.FindNearestEnemy(enemies);

            Assert.IsNotNull(nearestEnemy, "Должен быть найден ближайший враг.");
            Assert.AreEqual(55, nearestEnemy.X, "Координата X ближайшего врага должна быть 60.");
            Assert.AreEqual(55, nearestEnemy.Y, "Координата Y ближайшего врага должна быть 60."); 
        }
    }
}
