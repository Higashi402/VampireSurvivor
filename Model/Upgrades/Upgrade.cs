using MvcModel.Сreatures.Enemies;

namespace MvcModel.Upgrades
{
    /// <summary>
    /// Абстрактный класс Upgrade представляет базовый функционал для всех улучшений.
    /// Содержит методы для создания оружия и улучшений.
    /// </summary>
    public abstract class Upgrade
    {
        // Приватное поле для хранения уровня улучшения
        private short _level;

        /// <summary>
        /// Тип улучшения.
        /// </summary>
        public UpgradeType Type;

        /// <summary>
        /// Уровень улучшения. Виртуальное свойство, которое может быть переопределено в производных классах.
        /// </summary>
        public virtual short Level
        {
            get => _level;
            set => _level = value;
        }

        /// <summary>
        /// Флаг, указывающий, активно ли улучшение (например, атакует ли оружие).
        /// </summary>
        public bool IsAttacking { get; set; }

        /// <summary>
        /// Создает экземпляр оружия с указанным уроном, скоростью атаки и типом.
        /// </summary>
        /// <param name="parDamage">Урон оружия.</param>
        /// <param name="parAttackSpeed">Скорость атаки оружия.</param>
        /// <param name="parWeaponType">Тип оружия.</param>
        /// <returns>Экземпляр оружия.</returns>
        public static Weapon CreateWeapon(int parDamage, int parAttackSpeed, UpgradeType parWeaponType)
        {
            Weapon weapon = null;

            // Создание оружия в зависимости от типа
            switch (parWeaponType)
            {
                case UpgradeType.Whip:
                    weapon = new Whip(parDamage, parAttackSpeed);
                    break;
                case UpgradeType.Lightning:
                    weapon = new Lightning(parDamage, parAttackSpeed);
                    break;
            }

            // Установка флага активности оружия
            weapon.IsAttacking = true;

            return weapon;
        }

        /// <summary>
        /// Создает экземпляр улучшения указанного типа.
        /// </summary>
        /// <param name="parType">Тип улучшения.</param>
        /// <returns>Экземпляр улучшения.</returns>
        /// <exception cref="ArgumentException">Выбрасывается, если тип улучшения неизвестен.</exception>
        public static Upgrade CreateUpgrade(UpgradeType parType)
        {
            return parType switch
            {
                UpgradeType.Whip => new Whip(10, 800),
                UpgradeType.Lightning => new Lightning(15, 1300),
                UpgradeType.OakRoot => new OakRoot(),
                UpgradeType.Whetstone => new Whetstone(),
                _ => throw new ArgumentException("Unknown upgrade type")
            };
        }
    }
}