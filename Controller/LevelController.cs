using MvcModel.Frames;
using MvcModel.Сreatures.Enemies;
using MvcModel.Сreatures;
using MvcModel.Сreatures.Heros;
using MvcController.Tools;
using MvcModel.Upgrades;
using System.Numerics;
using System.Runtime.Serialization.Formatters.Binary;
using MvcModel;
using MvcController;

namespace MvcController
{
    /// <summary>
    /// Класс, управляющий игровым уровнем.
    /// </summary>
    /// <remarks>
    /// Класс отвечает за управление игровым уровнем, включая движение игрока, генерацию врагов,
    /// обработку атак, улучшений и завершение игры.
    /// </remarks>
    public class LevelController : Controller
    {
        /// <summary>
        /// Событие, возникающее при атаке кнутом (Whip).
        /// </summary>
        public static event Action<Enemy, Hero> WhipAttacked;

        /// <summary>
        /// Событие, возникающее при атаке молнией (Thunder).
        /// </summary>
        public static event Action<Enemy, Hero> ThunderAttacked;

        /// <summary>
        /// Событие, возникающее при смерти героя.
        /// </summary>
        public static event Action<Hero> HeroDied;

        /// <summary>
        /// Событие, возникающее при улучшении героя.
        /// </summary>
        public static event Action<Hero> HeroUpgraded;

        private static GameTimer _gameTimer;
        private static EnemyGenerator enemyGenerator;

        private static LevelController _instance;

        /// <summary>
        /// Получает генератор врагов.
        /// </summary>
        /// <value>
        /// Генератор врагов.
        /// </value>
        public EnemyGenerator EnemyGenerator
        {
            get
            {
                return enemyGenerator;
            }
        }

        /// <summary>
        /// Получает или задает игровой таймер.
        /// </summary>
        /// <value>
        /// Игровой таймер.
        /// </value>
        public static GameTimer GameTimer
        {
            get
            {
                return _gameTimer;
            }
            set
            {
                _gameTimer = value;
            }
        }

        private static readonly object _lock = new();

        /// <summary>
        /// Конструктор класса <see cref="LevelController"/>.
        /// </summary>
        /// <param name="level">Фрейм уровня.</param>
        /// <remarks>
        /// Инициализирует новый экземпляр класса и подписывается на события атак, смерти и улучшения героя.
        /// </remarks>
        private LevelController(LevelFrame level) : base(level)
        {
            Whip.WhipAttacked += OnWhipAttacked;
            Lightning.ThunderAttacked += OnThunderAttacked;
            Hero.HeroDied += OnHeroDied;
            Hero.HeroUpgraded += OnHeroUpgraded;
            Frame = level;
        }

        /// <summary>
        /// Инициализирует уровень.
        /// </summary>
        /// <param name="level">Фрейм уровня.</param>
        /// <remarks>
        /// Метод инициализирует игровой таймер, генератор врагов и запускает генерацию врагов.
        /// </remarks>
        public static void Initialize(LevelFrame level)
        {
            _gameTimer = GameTimer.Instance;
            enemyGenerator = new EnemyGenerator();
            _gameTimer.Start();
            LevelController.GetInstance().Frame = level;
            enemyGenerator.StartGenerating(level.Enemies, level.Player);
        }

        /// <summary>
        /// Получает экземпляр класса <see cref="LevelController"/>.
        /// </summary>
        /// <param name="level">Фрейм уровня.</param>
        /// <returns>Экземпляр класса <see cref="LevelController"/>.</returns>
        /// <remarks>
        /// Метод возвращает экземпляр класса, используя шаблон Singleton.
        /// </remarks>
        public static LevelController GetInstance(LevelFrame level = null)
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    if (level != null)
                    {
                        _instance = new LevelController(level);
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Сбрасывает экземпляр класса <see cref="LevelController"/>.
        /// </summary>
        /// <param name="level">Фрейм уровня.</param>
        /// <remarks>
        /// Метод сбрасывает текущий экземпляр и создает новый с указанным фреймом уровня.
        /// </remarks>
        public static void ResetInstance(LevelFrame level)
        {
            lock (_lock)
            {
                _instance = new LevelController(level);
            }
        }

        /// <summary>
        /// Перемещает игрока вниз.
        /// </summary>
        /// <remarks>
        /// Метод перемещает все камни и врагов вниз относительно игрока.
        /// </remarks>
        public void MovePlayerDown()
        {
            if (Frame is LevelFrame level)
            {
                foreach (Rock rock in level.Rocks)
                {
                    rock.Y -= GameWindow.GetInstance().Width / 160;
                }

                lock (level)
                {
                    level.Enemies.ForEach(item => item.Y -= GameWindow.GetInstance().Width / 160);
                }
            }
        }

        /// <summary>
        /// Перемещает игрока вверх.
        /// </summary>
        /// <remarks>
        /// Метод перемещает все камни и врагов вверх относительно игрока.
        /// </remarks>
        public void MovePlayerUp()
        {
            if (Frame is LevelFrame level)
            {
                foreach (Rock rock in level.Rocks)
                {
                    rock.Y += GameWindow.GetInstance().Width / 160;
                }

                lock (level)
                {
                    level.Enemies.ForEach(item => item.Y += GameWindow.GetInstance().Width / 160);
                }
            }
        }

        /// <summary>
        /// Перемещает игрока влево.
        /// </summary>
        /// <remarks>
        /// Метод перемещает все камни и врагов влево относительно игрока.
        /// </remarks>
        public void MovePlayerLeft()
        {
            if (Frame is LevelFrame level)
            {
                foreach (Rock rock in level.Rocks)
                {
                    rock.X += GameWindow.GetInstance().Width / 160;
                }

                lock (level)
                {
                    level.Enemies.ForEach(item => item.X += GameWindow.GetInstance().Width / 160);
                }
            }
        }

        /// <summary>
        /// Перемещает игрока вправо.
        /// </summary>
        /// <remarks>
        /// Метод перемещает все камни и врагов вправо относительно игрока.
        /// </remarks>
        public void MovePlayerRight()
        {
            if (Frame is LevelFrame level)
            {
                foreach (Rock rock in level.Rocks)
                {
                    rock.X -= GameWindow.GetInstance().Width / 160;
                }

                lock (level)
                {
                    level.Enemies.ForEach(item => item.X -= GameWindow.GetInstance().Width / 160);
                }
            }
        }

        /// <summary>
        /// Обрабатывает событие атаки кнутом (Whip).
        /// </summary>
        /// <param name="enemy">Враг, который был атакован.</param>
        /// <param name="hero">Герой, который атаковал.</param>
        /// <remarks>
        /// Метод вызывает событие <see cref="WhipAttacked"/>.
        /// </remarks>
        public void OnWhipAttacked(Enemy enemy, Hero hero)
        {
            WhipAttacked?.Invoke(enemy, hero);
        }

        /// <summary>
        /// Обрабатывает событие атаки молнией (Thunder).
        /// </summary>
        /// <param name="enemy">Враг, который был атакован.</param>
        /// <param name="hero">Герой, который атаковал.</param>
        /// <remarks>
        /// Метод вызывает событие <see cref="ThunderAttacked"/>.
        /// </remarks>
        public void OnThunderAttacked(Enemy enemy, Hero hero)
        {
            ThunderAttacked?.Invoke(enemy, hero);
        }

        /// <summary>
        /// Обрабатывает событие смерти героя.
        /// </summary>
        /// <param name="hero">Герой, который умер.</param>
        /// <remarks>
        /// Метод вызывает событие <see cref="HeroDied"/> и останавливает генерацию врагов.
        /// </remarks>
        public void OnHeroDied(Hero hero)
        {
            HeroDied?.Invoke(hero);
            enemyGenerator.StopGenerating();
        }

        /// <summary>
        /// Обрабатывает событие улучшения героя.
        /// </summary>
        /// <param name="hero">Герой, который был улучшен.</param>
        /// <remarks>
        /// Метод вызывает событие <see cref="HeroUpgraded"/>, останавливает генерацию врагов и приостанавливает героя.
        /// </remarks>
        public void OnHeroUpgraded(Hero hero)
        {
            if(enemyGenerator == null || hero.IsStopped)
            {
                return;
            }
            enemyGenerator.StopGenerating();
            hero.IsStopped = true;
            HeroUpgraded?.Invoke(hero);
        }

        public void ResumeGame(UpgradeType upgradeType)
        {
            if (Frame is LevelFrame level)
            {
                level.Player.IsStopped = false;
                level.Player.AddUpgrade(upgradeType);
                ApplyUpgradesAndResume(level, upgradeType);

                if (upgradeType == UpgradeType.Time)
                {
                    _gameTimer.AddSeconds(5);
                }

                enemyGenerator.StartGenerating(level.Enemies, level.Player);
            }
        }


        public void ResumeGame()
        {
            if (Frame is LevelFrame level)
            {
                level.Player.IsStopped = false;
                ApplyUpgradesAndResume(level, null);
                enemyGenerator.StartGenerating(level.Enemies, level.Player);
            }
        }

        /// <summary>
        /// Применяет улучшения и возобновляет игру.
        /// </summary>
        /// <param name="level">Фрейм уровня.</param>
        /// <param name="upgradeType">Тип улучшения (может быть null).</param>
        private void ApplyUpgradesAndResume(LevelFrame level, UpgradeType? upgradeType)
        {
            level.Enemies.ForEach(item => item.StartMovingTowardsPlayer(level.Player));
            foreach (Upgrade upgrade in level.Player.Upgrades)
            {
                if (upgrade is Weapon weapon)
                {
                    weapon.WeaponAttack(level.Player, level.Enemies);
                    weapon._CTS.TryReset();
                }
                if (upgrade is OakRoot oakRoot && (upgradeType == null || upgradeType == UpgradeType.OakRoot))
                {
                    level.Player.MaxHealth = Hero.BASE_HEALTH + oakRoot.AdditionalHealth;
                    level.Player.CurrentHealth = level.Player.MaxHealth;
                }
                if (upgrade is Whetstone whetstone && (upgradeType == null || upgradeType == UpgradeType.Whetstone))
                {
                    foreach (Upgrade upgrade1 in level.Player.Upgrades)
                    {
                        if (upgrade1 is Weapon weaponForUpgrade)
                        {
                            weaponForUpgrade.OverallDamage = weaponForUpgrade.Damage + whetstone.AdditionalDamage;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Получает количество секунд, прошедших с начала уровня.
        /// </summary>
        /// <returns>Количество секунд.</returns>
        /// <remarks>
        /// Метод возвращает количество секунд, прошедших с начала уровня, используя игровой таймер.
        /// </remarks>
        public int GetTimerSeconds()
        {
            return _gameTimer.GetElapsedSeconds();
        }

        /// <summary>
        /// Очищает состояние уровня.
        /// </summary>
        /// <remarks>
        /// Метод сбрасывает генератор врагов, игровой таймер и фрейм уровня.
        /// </remarks>
        public static void Clear()
        {
            enemyGenerator = null;
            _gameTimer = null;
            LevelController.GetInstance().Frame = null;
        }
    }
}