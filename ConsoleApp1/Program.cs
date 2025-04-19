using MvcController;
using MvcController.Tools;
using MvcModel;
using MvcModel.Frames;
using MvcModel.Upgrades;
using MvcModel.Сreatures.Enemies;
using MvcModel.Сreatures.Heros;

namespace console
{
    class Tests
    {
        public static void Main(string[] args)
        {
            EnemyGenerator _enemyGenerator = new EnemyGenerator();
            LevelFrame level = new LevelFrame();
            LevelController.GetInstance(level);
            LevelController.GameTimer = GameTimer.Instance;
            level.Player = new Hero("Vlad", 100, GameWindow.GetInstance().Width / 2, GameWindow.GetInstance().Height / 2, 3, 1);
            level.Player.Name = "Vlad";
            LevelController.GetInstance(level);

            var enemyGenerator = new EnemyGenerator();
            var initialZombieHealth = enemyGenerator.ZombieHealth;
            var initialZombieSpeedFirst = enemyGenerator.ZombieSpeedFirst;

            LevelController.GameTimer.Start();
            LevelController.GameTimer.AddSeconds(70);
            int time = LevelController.GameTimer.GetElapsedSeconds();
            System.Console.WriteLine(time);

            Enemy enemy = null;
            while(!(enemy is Zombie zombie))
            {
                enemy = enemyGenerator.GenerateEnemy();
            }
            System.Console.WriteLine(enemy.Speed);
        }
    }
}
