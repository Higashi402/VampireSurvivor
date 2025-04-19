using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcModel.Frames;
using MvcModel.Upgrades;
using MvcModel.Сreatures.Enemies;

namespace MvcModel.Сreatures.Heros
{
    /// <summary>
    /// Класс Hero представляет героя, управляемого игроком.
    /// Содержит функционал для управления здоровьем, уровнем, опытом и улучшениями.
    /// </summary>
    public class Hero
    {
        // Приватные поля для хранения состояния героя
        private int _currentHealth;
        private int _level = 1;
        private bool _isDead = false;

        /// <summary>
        /// Базовая константа для здоровья героя.
        /// </summary>
        public const int BASE_HEALTH = 100;

        /// <summary>
        /// Флаг, указывающий, остановлен ли герой.
        /// </summary>
        public bool IsStopped = false;

        /// <summary>
        /// Имя героя.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Максимальное здоровье героя.
        /// </summary>
        public int MaxHealth { get; set; }

        /// <summary>
        /// Координата X героя на карте.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Координата Y героя на карте.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Высота героя.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Ширина героя.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Текущий опыт героя.
        /// </summary>
        public int Experience { get; set; }

        /// <summary>
        /// Опыт, необходимый для достижения следующего уровня.
        /// </summary>
        public int NextLevelExperience { get; set; }

        /// <summary>
        /// Умер ли герой
        /// </summary>
        public bool IsDead
        {
            get
            {
                return _isDead;
            }
            set {
                _isDead = value;
            }
        }

        /// <summary>
        /// Список улучшений героя.
        /// </summary>
        public List<Upgrade> Upgrades { get; set; }

        /// <summary>
        /// Список оружия героя.
        /// </summary>
        public List<Upgrade> Weapons = new List<Upgrade>();

        /// <summary>
        /// Уровень героя. При изменении уровня вызывается событие HeroUpgraded.
        /// </summary>
        public int Level
        {
            get
            {
                return _level;
            }
            set
            {
                _level = value;
                HeroUpgraded?.Invoke(this);
            }
        }

        /// <summary>
        /// Текущее здоровье героя. При изменении здоровья проверяется, не умер ли герой.
        /// </summary>
        public int CurrentHealth
        {
            get => _currentHealth;
            set
            {
                _currentHealth = value;

                if (_currentHealth <= 0 && !_isDead)
                {
                    HeroDied?.Invoke(this);
                    _isDead = true;
                }
            }
        }

        /// <summary>
        /// Событие, возникающее при смерти героя.
        /// </summary>
        public static event Action<Hero> HeroDied;

        /// <summary>
        /// Событие, возникающее при повышении уровня героя.
        /// </summary>
        public static event Action<Hero> HeroUpgraded;

        /// <summary>
        /// Конструктор класса Hero, инициализирующий героя с указанными параметрами.
        /// </summary>
        /// <param name="parName">Имя героя.</param>
        /// <param name="parMaxHealth">Максимальное здоровье героя.</param>
        /// <param name="parX">Координата X героя на карте.</param>
        /// <param name="parY">Координата Y героя на карте.</param>
        /// <param name="parHeight">Высота героя.</param>
        /// <param name="parWidth">Ширина героя.</param>
        public Hero(string parName, int parMaxHealth, int parX, int parY, int parHeight, int parWidth)
        {
            Name = parName;
            MaxHealth = parMaxHealth;
            CurrentHealth = parMaxHealth;
            X = parX;
            Y = parY;
            Height = parHeight;
            Width = parWidth;
            Upgrades = new List<Upgrade>();
            AddUpgrade(UpgradeType.Whip);

            //AddUpgrade(UpgradeType.Whip); AddUpgrade(UpgradeType.Whip); AddUpgrade(UpgradeType.Whip); AddUpgrade(UpgradeType.Whip); AddUpgrade(UpgradeType.Whip); AddUpgrade(UpgradeType.Whip);
            //AddUpgrade(UpgradeType.Lightning); AddUpgrade(UpgradeType.Lightning); AddUpgrade(UpgradeType.Lightning); AddUpgrade(UpgradeType.Lightning); AddUpgrade(UpgradeType.Lightning); AddUpgrade(UpgradeType.Lightning); AddUpgrade(UpgradeType.Lightning);
            //AddUpgrade(UpgradeType.OakRoot); AddUpgrade(UpgradeType.OakRoot); AddUpgrade(UpgradeType.OakRoot); AddUpgrade(UpgradeType.OakRoot); AddUpgrade(UpgradeType.OakRoot); AddUpgrade(UpgradeType.OakRoot);
            //AddUpgrade(UpgradeType.Whetstone); AddUpgrade(UpgradeType.Whetstone); AddUpgrade(UpgradeType.Whetstone); AddUpgrade(UpgradeType.Whetstone); AddUpgrade(UpgradeType.Whetstone); AddUpgrade(UpgradeType.Whetstone);
            IsStopped = false;

            Experience = 0;
            NextLevelExperience = 100;
        }

        /// <summary>
        /// Метод для получения опыта за убийство врага.
        /// </summary>
        /// <param name="parEnemy">Враг, за которого герой получает опыт.</param>
        public void EarnExperience(Enemy parEnemy)
        {
            if (parEnemy != null)
            {
                if (parEnemy is Zombie)
                {
                    Experience += 10;
                }
                else if (parEnemy is Skeleton)
                {
                    Experience += 12;
                }
                else if (parEnemy is Spider)
                {
                    Experience += 15;
                }
                else if (parEnemy is Golem)
                {
                    Experience += 50;
                }
            }
            UpdateLevel();
        }

        /// <summary>
        /// Метод для добавления улучшения герою.
        /// </summary>
        /// <param name="parUpgradeType">Тип улучшения.</param>
        public void AddUpgrade(UpgradeType parUpgradeType)
        {
            if (parUpgradeType == UpgradeType.Time)
            {
                return;
            }

            var existingUpgrade = Upgrades.FirstOrDefault(upg => upg.Type == parUpgradeType);

            if (existingUpgrade != null)
            {
                existingUpgrade.Level++;
                if (existingUpgrade.Type == UpgradeType.Whip && (existingUpgrade.Level == 4 || existingUpgrade.Level == 7))
                {
                    Upgrade newUpgrade = Upgrade.CreateUpgrade(parUpgradeType);
                    newUpgrade.Level = 3;
                    if (newUpgrade is Weapon whip)
                    {
                        whip.AttackSpeed = 700;
                    }
                    Upgrades.Add(newUpgrade);
                }
                if (existingUpgrade.Type == UpgradeType.Lightning && (existingUpgrade.Level == 4 || existingUpgrade.Level == 7))
                {
                    Upgrade newUpgrade = Upgrade.CreateUpgrade(parUpgradeType);
                    newUpgrade.Level = 1;
                    if (newUpgrade is Weapon thunder)
                    {
                        thunder.AttackSpeed = 1500;
                        thunder.Damage = 100;
                    }
                    Upgrades.Add(newUpgrade);
                }
            }
            else
            {
                Upgrade newUpgrade = Upgrade.CreateUpgrade(parUpgradeType);
                Upgrades.Add(newUpgrade);
            }
        }

        /// <summary>
        /// Метод для обновления уровня героя на основе текущего опыта.
        /// </summary>
        private void UpdateLevel()
        {
            while (Experience >= NextLevelExperience)
            {
                NextLevelExperience = (int)(NextLevelExperience * 1.5);
                Level += 1;
            }
        }

        /// <summary>
        /// Метод для поиска ближайшего врага в радиусе действия.
        /// </summary>
        /// <param name="parEnemies">Список врагов.</param>
        /// <returns>Ближайший враг или null, если врагов нет.</returns>
        public Enemy FindNearestEnemy(SynchronizedList<Enemy> parEnemies)
        {
            if (parEnemies == null || parEnemies.Count == 0)
                return null;

            double minDistance = double.MaxValue;
            List<Enemy> nearestEnemies = new List<Enemy>();

            lock (parEnemies)
            {
                parEnemies.ForEach(item =>
                {
                    double distance = Math.Sqrt(Math.Pow(item.X - X, 2) + Math.Pow(item.Y - Y, 2));

                    if (distance >= GameWindow.GetInstance().Width * 0.4)
                    {
                        return;
                    }

                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestEnemies.Clear();
                        nearestEnemies.Add(item);
                    }
                    else if (distance == minDistance)
                    {
                        nearestEnemies.Add(item);
                    }
                });
            }

            if (nearestEnemies.Count > 1)
            {
                Random random = new Random();
                return nearestEnemies[random.Next(nearestEnemies.Count)];
            }

            return nearestEnemies.FirstOrDefault();
        }
    }
}