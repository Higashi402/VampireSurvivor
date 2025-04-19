using MvcController;
using MvcController.Tools;
using MvcModel;
using MvcModel.Frames;
using MvcModel.Сreatures.Enemies;
using MvcModel.Сreatures.Heros;
using System.Drawing;

namespace UnitTests
{
    [TestClass]
    public class EnemyGeneratorTests
    {
        private EnemyGenerator _enemyGenerator;

        [TestInitialize]
        public void Setup()
        {
            _enemyGenerator = new EnemyGenerator();
            LevelFrame level = new LevelFrame();
            LevelController.GetInstance(level);
            LevelController.GameTimer = GameTimer.Instance;
            level.Player = new Hero("Vlad", 100, GameWindow.GetInstance().Width / 2, GameWindow.GetInstance().Height / 2, 3, 1);
            level.Player.Name = "Vlad";
            LevelController.GetInstance(level);
        }

        [TestMethod]
        public void GenerateEnemyTypeChanceTest()
        {
            var enemyTypes = new Dictionary<EnemyTypes, int>
            {
                { EnemyTypes.Zombie, 0 },
                { EnemyTypes.Skeleton, 0 },
                { EnemyTypes.Spider, 0 },
                { EnemyTypes.Golem, 0 }
            };
            int totalIterations = 10000;

            for (int i = 0; i < totalIterations; i++)
            {
                var enemyType = _enemyGenerator.GenerateEnemyType();
                enemyTypes[enemyType]++;
            }

            Assert.IsTrue(enemyTypes[EnemyTypes.Zombie] > enemyTypes[EnemyTypes.Skeleton], "Зомби должны генерироваться чаще, чем скелеты.");
            Assert.IsTrue(enemyTypes[EnemyTypes.Skeleton] > enemyTypes[EnemyTypes.Spider], "Скелеты должны генерироваться чаще, чем пауки.");
            Assert.IsTrue(enemyTypes[EnemyTypes.Spider] > enemyTypes[EnemyTypes.Golem], "Пауки должны генерироваться чаще, чем големы.");
        }

        [TestMethod]
        public void GenerateCoordinatesOutsideTest()
        {
            var coordinates = _enemyGenerator.GenerateCoordinates();

            Assert.IsTrue(coordinates.X < 0 || coordinates.X > GameWindow.GetInstance().Width ||
                          coordinates.Y < 0 || coordinates.Y > GameWindow.GetInstance().Height,
                          "Координаты должны быть за пределами видимой области.");
        }

        [TestMethod]
        public void GenerateEnemy_EnemyPropertiesChangeOverTime()
        {
            var enemyGenerator = new EnemyGenerator();
            var initialZombieHealth = enemyGenerator.ZombieHealth;
            var initialZombieSpeedFirst = enemyGenerator.ZombieSpeedFirst;

            LevelController.GameTimer.Start();
            LevelController.GameTimer.AddSeconds(70);

            Enemy enemy = null;
            while (!(enemy is Zombie zombie))
            {
                enemy = enemyGenerator.GenerateEnemy();
            }

            Assert.IsTrue(enemyGenerator.ZombieHealth > initialZombieHealth, "Здоровье зомби должно увеличиться со временем.");
            Assert.IsTrue(enemyGenerator.ZombieSpeedFirst < initialZombieSpeedFirst, "Скорость зомби должна уменьшиться со временем.");
        }
    }
}