using MvcModel.Сreatures.Enemies;
using MvcModel.Сreatures.Heros;

namespace MvcModel.Upgrades
{
    /// <summary>
    /// Класс Whip представляет оружие "Кнут", которое может атаковать врагов.
    /// Наследуется от базового класса Weapon.
    /// </summary>
    public class Whip : Weapon
    {
        /// <summary>
        /// Событие, возникающее при атаке кнутом.
        /// Передает атакованного врага и героя, который произвел атаку.
        /// </summary>
        public static event Action<Enemy, Hero> WhipAttacked;

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
                if (value == 2)
                {
                    Damage = 14;
                }
                else if (value == 3)
                {
                    Damage = 18;
                }
                else if (value == 4)
                {
                    Damage = 18;
                }
                else if (value == 5)
                {
                    Damage = 26;
                }
                else if (value == 6)
                {
                    Damage = 35;
                }
                else if (value == 7)
                {
                    Damage = 35;
                }
            }
        }

        /// <summary>
        /// Конструктор класса Whip, инициализирующий оружие с указанным уроном и скоростью атаки.
        /// </summary>
        /// <param name="parDamage">Урон оружия.</param>
        /// <param name="parAttackSpeed">Скорость атаки оружия.</param>
        public Whip(int parDamage, int parAttackSpeed)
        {
            this.Damage = parDamage;
            this.AttackSpeed = parAttackSpeed;
            this.Level = 1;
            Type = UpgradeType.Whip;
        }

        /// <summary>
        /// Асинхронный метод атаки оружием. Атакует ближайшего врага.
        /// </summary>
        /// <param name="parHero">Герой, который использует оружие.</param>
        /// <param name="parEnemies">Список врагов, которые могут быть атакованы.</param>
        public override async void WeaponAttack(Hero parHero, SynchronizedList<Enemy> parEnemies)
        {
            while (parHero.CurrentHealth > 0 && !parHero.IsStopped)
            {
                try
                {
                    await Task.Delay(this.AttackSpeed, _CTS.Token);
                }
                catch (TaskCanceledException)
                {
                    break;
                }

                if (parHero.IsStopped || _CTS.Token.IsCancellationRequested)
                    break;

                Enemy nearestEnemy = parHero.FindNearestEnemy(parEnemies);

                if (nearestEnemy != null && !parHero.IsStopped)
                {
                    this.AttackEnemy(nearestEnemy);
                    WhipAttacked?.Invoke(nearestEnemy, parHero);

                    if (nearestEnemy.CurrentHealth <= 0)
                    {
                        parHero.EarnExperience(nearestEnemy);
                        nearestEnemy.IsDead = true;
                        parEnemies.Remove(nearestEnemy);
                    }
                }
            }
        }
    }
}