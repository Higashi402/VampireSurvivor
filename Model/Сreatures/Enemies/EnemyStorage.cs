using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcModel.Сreatures.Enemies
{
    /// <summary>
    /// Класс EnemyStorage представляет хранилище для управления списком врагов.
    /// </summary>
    public class EnemyStorage
    {
        /// <summary>
        /// Список врагов, хранящихся в хранилище.
        /// </summary>
        public List<Enemy> Enemies;

        /// <summary>
        /// Конструктор класса EnemyStorage, инициализирующий хранилище с указанным списком врагов.
        /// </summary>
        /// <param name="parEnemies">Список врагов для инициализации хранилища.</param>
        public EnemyStorage(List<Enemy> parEnemies)
        {
            Enemies = parEnemies;
        }

        /// <summary>
        /// Конструктор по умолчанию для класса EnemyStorage.
        /// Инициализирует пустой список врагов.
        /// </summary>
        public EnemyStorage()
        {
            Enemies = new List<Enemy>();
        }

        /// <summary>
        /// Добавляет врага в хранилище.
        /// </summary>
        /// <param name="parEnemy">Враг для добавления.</param>
        public void AddEnemy(Enemy parEnemy)
        {
            Enemies.Add(parEnemy);
        }

        /// <summary>
        /// Удаляет врага из хранилища.
        /// </summary>
        /// <param name="parEnemy">Враг для удаления.</param>
        public void RemoveEnemy(Enemy parEnemy)
        {
            Enemies.Remove(parEnemy);
        }
    }
}