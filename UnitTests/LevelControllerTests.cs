using MvcController;
using MvcModel;
using MvcModel.Frames;
using MvcModel.Сreatures.Enemies;
using MvcModel.Сreatures;
using MvcModel.Сreatures.Heros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcModel.Upgrades;

namespace UnitTests
{
    [TestClass]
    public class LevelControllerTests
    {
        [TestMethod]
        public void InitializeLevelControllerTest()
        {
            var levelFrame = new LevelFrame();
            var player = new Hero("Vlad",100, 10, 10, 3, 1);
            levelFrame.Player = player;

            var levelController = LevelController.GetInstance(levelFrame);
            LevelController.Initialize(levelFrame);

            Assert.IsNotNull(LevelController.GameTimer, "Таймер должен быть инициализирован.");
            Assert.IsNotNull(LevelController.GetInstance().EnemyGenerator, "Генератор врагов должен быть инициализирован.");
        }

        [TestMethod]
        public void MovePlayerDownTest()
        {
            var levelFrame = new LevelFrame();
            var player = new Hero("Vlad", 100, 10, 10, 3, 1);
            levelFrame.Player = player;

            var levelController = LevelController.GetInstance(levelFrame);
            LevelController.Initialize(levelFrame);

            var rock = new Rock(50, 50);
            levelFrame.Rocks.Add(rock);

            var enemy = new Skeleton(100, 100, 100, 100, 100, 5,500);
            levelFrame.Enemies.Add(enemy);

            levelController.MovePlayerDown();

            Assert.AreEqual(50 - GameWindow.GetInstance().Width / 160, rock.Y, "Камень должен быть перемещен вниз.");
            Assert.AreEqual(100 - GameWindow.GetInstance().Width / 160, enemy.Y, "Враг должен быть перемещен вниз.");
        }

        [TestMethod]
        public void ResumeGameTest()
        {
            var levelFrame = new LevelFrame();
            var player = new Hero("Vlad", 100, 10, 10, 3, 1);
            levelFrame.Player = player;

            var levelController = LevelController.GetInstance(levelFrame);
            LevelController.Initialize(levelFrame);

            player.AddUpgrade(UpgradeType.Whip);

            levelController.ResumeGame(UpgradeType.Whetstone);

            var whetstoneUpgrade = player.Upgrades.FirstOrDefault(u => u.Type == UpgradeType.Whetstone);

            Assert.IsFalse(player.IsStopped, "Герой должен быть возобновлен.");
            Assert.IsTrue(levelController.EnemyGenerator != null, "Генератор врагов должен быть запущен.");
            Assert.IsTrue(whetstoneUpgrade != null, "У игрока должен появится точильный камень.");
        }

        [TestMethod]
        public void OnHeroDiedTest()
        {
            var levelFrame = new LevelFrame();
            var player = new Hero("Vlad", 100, 10, 10, 3, 1);
            levelFrame.Player = player;

            var levelController = LevelController.GetInstance(levelFrame);
            LevelController.Initialize(levelFrame);

            player.CurrentHealth -= 1000;

            Assert.IsTrue(levelController.EnemyGenerator != null, "Генератор врагов должен быть остановлен.");
            Assert.IsTrue(levelFrame.Player.IsDead, "Игрок должен быть мертв.");
        }
    }
}
