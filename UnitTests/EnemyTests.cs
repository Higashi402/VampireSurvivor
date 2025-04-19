using MvcModel;
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
    public class EnemyTests
    {
        [TestMethod]
        public void CreateEnemyTest()
        {
            int maxHealth = 100;
            int currentHealth = 100;
            int x = 0;
            int y = 0;
            int height = 10;
            int width = 10;
            int speed = 5;

            var zombie = Enemy.CreateEnemy(maxHealth, currentHealth, x, y, height, width, speed, EnemyTypes.Zombie);

            Assert.IsInstanceOfType(zombie, typeof(Zombie), "Созданный враг должен быть типа Zombie.");
            Assert.IsTrue(Enum.IsDefined(typeof(EnemyTypes), zombie.Type), "The generated enemy type should be a valid value.");
            Assert.IsNotNull(zombie, "The generated enemy should not be null.");
        }

        [TestMethod]
        public void MoveEnemyTowardsPlayerTest()
        {
            var enemy = new Zombie(100, 100, 0, 0, 10, 10, 5);
            var player = new Hero("Vlad", 100, 50, 50, 10, 10);
            GameWindow.GetInstance().Resize(160, 62);

            enemy.MoveEnemyTowardsPlayer(player);

            Assert.IsTrue(enemy.X > 0 || enemy.Y > 0, "Враг должен переместиться в сторону игрока по диагонали.");
        }

        [TestMethod]
        public void EnemyAttackTest()
        {
            var enemy = new Zombie(100, 100, 45, 45, 10, 10, 5); 
            var player = new Hero("Vlad", 100, 50, 50, 10, 10);
            GameWindow.GetInstance().Resize(160, 62);

            int initialHealth = player.CurrentHealth;

            enemy.MoveEnemyTowardsPlayer(player);

            Assert.IsTrue(player.CurrentHealth < initialHealth, "Здоровье игрока должно уменьшиться при столкновении с врагом.");
            Assert.AreEqual(initialHealth - 10, player.CurrentHealth, "Здоровье игрока должно уменьшиться на 10 при столкновении с зомби.");
        }
    }
}
