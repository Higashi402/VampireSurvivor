using MvcModel.Сreatures.Enemies;
using MvcModel.Сreatures.Heros;
using System.Threading;

namespace MvcModel.Upgrades
{
    /// <summary>
    /// Класс Lightning представляет оружие "Молния", которое может атаковать врагов.
    /// Наследуется от базового класса Weapon.
    /// </summary>
    public class Lightning : Weapon
    {
        // Генератор случайных чисел для выбора случайного врага
        private Random _random = new Random();

        /// <summary>
        /// Событие, возникающее при атаке молнией.
        /// Передает атакованного врага и героя, который произвел атаку.
        /// </summary>
        public static event Action<Enemy, Hero> ThunderAttacked;

        /// <summary>
        /// Уровень оружия. При изменении уровня обновляется урон.
        /// </summary>
        public override short Level
        {
            get
            {
                return base.Level;
            }
            set
            {
                base.Level = value;
                // Обновление урона в зависимости от уровня
                if (value == 2)
                {
                    Damage = 19;
                }
                else if (value == 3)
                {
                    Damage = 25;
                }
                else if (value == 4)
                {
                    Damage = 25;
                }
                else if (value == 5)
                {
                    Damage = 32;
                }
                else if (value == 6)
                {
                    Damage = 45;
                }
                else if (value == 7)
                {
                    Damage = 45;
                }
            }
        }

        /// <summary>
        /// Конструктор класса Lightning, инициализирующий оружие с указанным уроном и скоростью атаки.
        /// </summary>
        /// <param name="parDamage">Урон оружия.</param>
        /// <param name="parAttackSpeed">Скорость атаки оружия.</param>
        public Lightning(int parDamage, int parAttackSpeed)
        {
            this.Damage = parDamage;
            this.AttackSpeed = parAttackSpeed;
            this.Level = 1;
            Type = UpgradeType.Lightning;
        }

        /// <summary>
        /// Асинхронный метод атаки оружием. Атакует случайного врага в радиусе действия.
        /// </summary>
        /// <param name="parHero">Герой, который использует оружие.</param>
        /// <param name="parEnemies">Список врагов, которые могут быть атакованы.</param>
        public override async void WeaponAttack(Hero parHero, SynchronizedList<Enemy> parEnemies)
        {
            // Цикл атаки, пока герой жив и не остановлен
            while (parHero.CurrentHealth > 0 && !parHero.IsStopped)
            {
                try
                {
                    // Задержка перед следующей атакой
                    await Task.Delay(this.AttackSpeed, _CTS.Token);
                }
                catch (TaskCanceledException)
                {
                    // Прерывание цикла, если задача отменена
                    break;
                }

                // Проверка на остановку героя или отмену задачи
                if (parHero.IsStopped || _CTS.Token.IsCancellationRequested)
                    break;

                // Поиск врагов в радиусе действия
                var nearbyEnemies = parEnemies.Where(enemy =>
                    Math.Abs(enemy.X - parHero.X) + Math.Abs(enemy.Y - parHero.Y) <= GameWindow.GetInstance().Height * 0.7).ToList();

                // Если есть враги в радиусе, выбираем случайного и атакуем
                if (nearbyEnemies.Count > 0)
                {
                    Enemy randomEnemy = nearbyEnemies[_random.Next(0, nearbyEnemies.Count)];
                    this.AttackEnemy(randomEnemy);
                    ThunderAttacked?.Invoke(randomEnemy, parHero);

                    // Если враг убит, герой получает опыт, а враг удаляется из списка
                    if (randomEnemy.CurrentHealth <= 0)
                    {
                        parHero.EarnExperience(randomEnemy);
                        randomEnemy.IsDead = true;
                        parEnemies.Remove(randomEnemy);
                    }
                }
            }
        }
    }
}