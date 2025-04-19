using MvcModel.Сreatures.Enemies;
using MvcModel.Сreatures.Heros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcModel.Upgrades
{
    /// <summary>
    /// Абстрактный класс Weapon представляет базовый функционал для всех видов оружия.
    /// Наследуется от базового класса Upgrade.
    /// </summary>
    public abstract class Weapon : Upgrade
    {
        // Приватные поля для хранения урона, общего урона и скорости атаки
        private int _damage;
        private int _overallDamage;
        private int _attackSpeed;

        /// <summary>
        /// Флаг, указывающий, активно ли оружие (например, атакует ли оно).
        /// </summary>
        public bool IsAttacking { get; set; }

        /// <summary>
        /// Урон оружия. При изменении обновляет общий урон.
        /// </summary>
        public int Damage
        {
            get
            {
                return _damage;
            }
            set
            {
                OverallDamage -= _damage; // Вычитаем старый урон из общего урона
                _damage = value; // Устанавливаем новый урон
                OverallDamage = _damage + OverallDamage; // Добавляем новый урон к общему урону
            }
        }

        /// <summary>
        /// Скорость атаки оружия.
        /// </summary>
        public int AttackSpeed
        {
            get
            {
                return _attackSpeed;
            }
            set
            {
                _attackSpeed = value;
            }
        }

        /// <summary>
        /// Общий урон, наносимый оружием.
        /// </summary>
        public int OverallDamage
        {
            get
            {
                return _overallDamage;
            }
            set
            {
                _overallDamage = value;
            }
        }

        /// <summary>
        /// Источник токена отмены для управления асинхронными задачами атаки.
        /// </summary>
        public CancellationTokenSource _CTS = new CancellationTokenSource();

        /// <summary>
        /// Абстрактный метод для атаки оружием. Должен быть реализован в производных классах.
        /// </summary>
        /// <param name="hero">Герой, который использует оружие.</param>
        /// <param name="parEnemies">Список врагов, которые могут быть атакованы.</param>
        public abstract void WeaponAttack(Hero hero, SynchronizedList<Enemy> parEnemies);

        /// <summary>
        /// Метод для атаки конкретного врага. Уменьшает здоровье врага на значение общего урона.
        /// </summary>
        /// <param name="parTargetEnemy">Враг, который будет атакован.</param>
        public void AttackEnemy(Enemy parTargetEnemy)
        {
            parTargetEnemy.CurrentHealth -= OverallDamage;
        }
    }
}