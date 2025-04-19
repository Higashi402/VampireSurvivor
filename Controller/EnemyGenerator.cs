using System.Drawing;
using MvcModel;
using MvcModel.Сreatures.Enemies;
using MvcModel.Сreatures.Heros;

namespace MvcController
{
    /// <summary>
    /// Класс для генерации врагов в игре.
    /// </summary>
    /// <remarks>
    /// Класс отвечает за создание врагов, их размещение на игровом поле и управление процессом генерации.
    /// </remarks>
    public class EnemyGenerator
    {
        private readonly Random _random = new Random();
        private readonly int _visibleWidth;
        private readonly int _visibleHeight;
        private int _generatingDelay = 700;
        private int _currentMinute = 0;
        private int _spiderSpeedFirst = 180;
        private int _spiderSpeedSecond = 210;
        private int _zombieSpeedFirst = 300;
        private int _zombieSpeedSecond = 500;
        private int _skeletonSpeedFirst = 200;
        private int _skeletonSpeedSecond = 400;
        private int _golemSpeedFirst = 400;
        private int _golemSpeedSecond = 600;
        private int _spiderHealth = 15;
        private int _zombieHealth = 20;
        private int _skeletonHealth = 15;
        private int _golemHealth = 100;

        private const int CHANCE_ZOMBIE = 40;
        private const int CHANCE_SCELETON = 25;
        private const int CHANCE_SPIDER = 15;
        private const int CHANCE_GOLEM = 10;

        public int ZombieHealth { get { return _zombieHealth; } }
        public int ZombieSpeedFirst { get { return _zombieSpeedFirst; } }


        private CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <remarks>
        /// Инициализирует генератор врагов, устанавливая видимую ширину и высоту игрового окна.
        /// </remarks>
        public EnemyGenerator()
        {
            _visibleWidth = GameWindow.GetInstance().Width;
            _visibleHeight = GameWindow.GetInstance().Height;
        }

        /// <summary>
        /// Генерирует нового врага.
        /// </summary>
        /// <returns>Объект типа <see cref="Enemy"/>.</returns>
        /// <remarks>
        /// Метод создает врага с учетом текущего времени игры, типа врага и его характеристик.
        /// Характеристики врагов (скорость, здоровье) динамически изменяются в зависимости от времени.
        /// </remarks>
        public Enemy GenerateEnemy()
        {
            int enemyWidth = 1;
            int enemyHeight = 2;
            int enemySpeed = 500;
            int enemyMaxHealth = 10;
            int time = LevelController.GameTimer.GetElapsedSeconds();

            if (time / 60 > _currentMinute)
            {
                _zombieSpeedFirst = (int)(_zombieSpeedFirst * 0.95);
                _zombieSpeedSecond = (int)(_zombieSpeedSecond * 0.95);
                _zombieHealth = (int)(_zombieHealth * 1.1);

                _skeletonSpeedFirst = (int)(_skeletonSpeedFirst * 0.95);
                _skeletonSpeedSecond = (int)(_skeletonSpeedSecond * 0.95);
                _skeletonHealth = (int)(_skeletonHealth * 1.1);

                _spiderSpeedFirst = (int)(_spiderSpeedFirst * 0.95);
                _spiderSpeedSecond = (int)(_spiderSpeedSecond * 0.95);
                _spiderHealth = (int)(_spiderHealth * 1.1);

                _golemSpeedFirst = (int)(_golemSpeedFirst * 0.95);
                _golemSpeedSecond = (int)(_golemSpeedSecond * 0.95);
                _golemHealth = (int)(_golemHealth * 1.1);

                _currentMinute = (int)(time / 60);
                _generatingDelay = (int)(_generatingDelay / 1.5);
                if (_generatingDelay == 0)
                {
                    _generatingDelay = 1;
                }
            }

            Point enemyCoordinates = GenerateCoordinates();
            EnemyTypes enemyType = GenerateEnemyType();
            switch (enemyType)
            {
                case EnemyTypes.Zombie:
                    enemyWidth = GameWindow.GetInstance().Width / 160 * 1;
                    enemyHeight = GameWindow.GetInstance().Height / 62 * 3;
                    enemySpeed = _random.Next(_zombieSpeedFirst, _zombieSpeedSecond);
                    enemyMaxHealth = _zombieHealth;
                    break;
                case EnemyTypes.Skeleton:
                    enemyWidth = GameWindow.GetInstance().Width / 160 * 1;
                    enemyHeight = GameWindow.GetInstance().Height / 62 * 3;
                    enemySpeed = _random.Next(_skeletonSpeedFirst, _skeletonSpeedSecond);
                    enemyMaxHealth = _skeletonHealth;
                    break;
                case EnemyTypes.Spider:
                    enemyWidth = GameWindow.GetInstance().Width / 160 * 2;
                    enemyHeight = GameWindow.GetInstance().Height / 62 * 2;
                    enemySpeed = _random.Next(_spiderSpeedFirst, _spiderSpeedSecond);
                    enemyMaxHealth = _spiderHealth;
                    break;
                case EnemyTypes.Golem:
                    enemyWidth = GameWindow.GetInstance().Width / 160 * 3;
                    enemyHeight = GameWindow.GetInstance().Height / 62 * 3;
                    enemySpeed = _random.Next(_golemSpeedFirst, _golemSpeedSecond);
                    enemyMaxHealth = _golemHealth;
                    break;
            }

            return Enemy.CreateEnemy(
                    parMaxHealth: enemyMaxHealth,
                    parCurrentHealth: enemyMaxHealth,
                    parX: enemyCoordinates.X,
                    parY: enemyCoordinates.Y,
                    parHeight: enemyHeight,
                    parWidth: enemyWidth,
                    parSpeed: enemySpeed,
                    parEnemyType: enemyType
                );
        }

        /// <summary>
        /// Запускает процесс генерации врагов.
        /// </summary>
        /// <param name="parEnemies">Список врагов, в который будут добавляться новые враги.</param>
        /// <param name="parPlayer">Игрок, к которому будут двигаться враги.</param>
        /// <remarks>
        /// Метод запускает асинхронную задачу, которая периодически генерирует врагов и добавляет их в список.
        /// Генерация происходит с задержкой, указанной в <see cref="_generatingDelay"/>.
        /// </remarks>
        public void StartGenerating(SynchronizedList<Enemy> parEnemies, Hero parPlayer)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = _cancellationTokenSource.Token;

            Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        lock (parEnemies)
                        {
                            Enemy newEnemy = GenerateEnemy();
                            parEnemies.Add(newEnemy);
                            newEnemy.StartMovingTowardsPlayer(parPlayer);
                        }
                    }
                    catch (Exception ex)
                    {
                        break;
                    }

                    await Task.Delay(_generatingDelay, cancellationToken);
                }
            }, cancellationToken);
        }

        /// <summary>
        /// Останавливает процесс генерации врагов.
        /// </summary>
        /// <remarks>
        /// Метод отменяет асинхронную задачу, отвечающую за генерацию врагов.
        /// </remarks>
        public void StopGenerating()
        {
            _cancellationTokenSource?.Cancel();
        }

        /// <summary>
        /// Генерирует координаты для размещения врага на игровом поле.
        /// </summary>
        /// <returns>Точка <see cref="Point"/> с координатами (X, Y).</returns>
        /// <remarks>
        /// Метод случайным образом выбирает сторону появления врага (верх, низ, лево, право)
        /// и генерирует координаты за пределами видимой области игрового окна.
        /// </remarks>
        public Point GenerateCoordinates()
        {
            int x, y;

            int spawnSide = _random.Next(4);

            switch (spawnSide)
            {
                case 0: // Сверху
                    x = _random.Next(-20, _visibleWidth + 20);
                    y = _random.Next(-20, 0);
                    break;
                case 1: // Снизу
                    x = _random.Next(-20, _visibleWidth + 20);
                    y = _random.Next(_visibleHeight, _visibleHeight + 20);
                    break;
                case 2: // Слева
                    x = _random.Next(-20, 0);
                    y = _random.Next(-20, _visibleHeight + 20);
                    break;
                case 3: // Справа
                    x = _random.Next(_visibleWidth, _visibleWidth + 20);
                    y = _random.Next(-20, _visibleHeight + 20);
                    break;
                default:
                    x = -10;
                    y = -10;
                    break;
            }

            return new Point(x, y);
        }

        /// <summary>
        /// Генерирует тип врага.
        /// </summary>
        /// <returns>Тип врага <see cref="EnemyTypes"/>.</returns>
        /// <remarks>
        /// Метод случайным образом выбирает тип врага на основе заданных вероятностей:
        /// - Zombie: CHANCE_ZOMBIE%
        /// - Skeleton: CHANCE_SCELETON%
        /// - Spider: CHANCE_SPIDER%
        /// - Golem: CHANCE_GOLEM%
        /// </remarks>
        public EnemyTypes GenerateEnemyType()
        {
            int chance = _random.Next(0, 100);

            if (chance >= 0 && chance <= CHANCE_ZOMBIE)
            {
                return EnemyTypes.Zombie;
            }
            else if (chance > CHANCE_ZOMBIE && chance <= CHANCE_ZOMBIE + CHANCE_SCELETON)
            {
                return EnemyTypes.Skeleton;
            }
            else if (chance > CHANCE_ZOMBIE + CHANCE_SCELETON && chance <= CHANCE_ZOMBIE + CHANCE_SCELETON + CHANCE_SPIDER)
            {
                return EnemyTypes.Spider;
            }
            else if (chance > CHANCE_ZOMBIE + CHANCE_SCELETON + CHANCE_SPIDER && chance <= CHANCE_ZOMBIE + CHANCE_SCELETON + CHANCE_SPIDER + CHANCE_GOLEM)
            {
                return EnemyTypes.Golem;
            }
            else
            {
                return EnemyTypes.Zombie;
            }
        }
    }
}