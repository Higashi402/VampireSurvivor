using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using MvcModel.Сreatures.Heros;

namespace MvcModel.Сreatures.Enemies
{
    /// <summary>
    /// Абстрактный класс, представляющий базового врага.
    /// </summary>
    /// <remarks>
    /// Класс предоставляет базовую функциональность для всех типов врагов, включая движение к игроку,
    /// обработку столкновений и управление здоровьем.
    /// </remarks>
    public abstract class Enemy
    {
        private static readonly object damageLock = new object();

        /// <summary>
        /// Получает или задает значение, указывающее, мертв ли враг.
        /// </summary>
        /// <value>
        /// <c>true</c>, если враг мертв; иначе <c>false</c>.
        /// </value>
        public bool IsDead { get; set; }

        /// <summary>
        /// Получает или задает максимальное здоровье врага.
        /// </summary>
        /// <value>
        /// Максимальное здоровье врага.
        /// </value>
        public int MaxHealth { get; set; }

        /// <summary>
        /// Получает или задает текущее здоровье врага.
        /// </summary>
        /// <value>
        /// Текущее здоровье врага.
        /// </value>
        public int CurrentHealth { get; set; }

        /// <summary>
        /// Получает или задает тип врага.
        /// </summary>
        /// <value>
        /// Тип врага.
        /// </value>
        public EnemyTypes Type { get; set; }

        /// <summary>
        /// Получает или задает координату X врага на игровом поле.
        /// </summary>
        /// <value>
        /// Координата X врага.
        /// </value>
        public int X { get; set; }

        /// <summary>
        /// Получает или задает координату Y врага на игровом поле.
        /// </summary>
        /// <value>
        /// Координата Y врага.
        /// </value>
        public int Y { get; set; }

        /// <summary>
        /// Получает или задает ширину врага.
        /// </summary>
        /// <value>
        /// Ширина врага.
        /// </value>
        public int Width { get; set; }

        /// <summary>
        /// Получает или задает высоту врага.
        /// </summary>
        /// <value>
        /// Высота врага.
        /// </value>
        public int Height { get; set; }

        /// <summary>
        /// Получает или задает скорость врага.
        /// </summary>
        /// <value>
        /// Скорость врага.
        /// </value>
        public int Speed { get; set; }

        /// <summary>
        /// Создает экземпляр врага указанного типа.
        /// </summary>
        /// <param name="parMaxHealth">Максимальное здоровье врага.</param>
        /// <param name="parCurrentHealth">Текущее здоровье врага.</param>
        /// <param name="parX">Координата X врага.</param>
        /// <param name="parY">Координата Y врага.</param>
        /// <param name="parHeight">Высота врага.</param>
        /// <param name="parWidth">Ширина врага.</param>
        /// <param name="parSpeed">Скорость врага.</param>
        /// <param name="parEnemyType">Тип врага.</param>
        /// <returns>Экземпляр врага указанного типа.</returns>
        /// <remarks>
        /// Метод создает врага на основе переданного типа и инициализирует его характеристики.
        /// </remarks>
        public static Enemy CreateEnemy(int parMaxHealth, int parCurrentHealth, int parX, int parY, int parHeight, int parWidth, int parSpeed, EnemyTypes parEnemyType)
        {
            Enemy enemy = null;

            switch (parEnemyType)
            {
                case EnemyTypes.Zombie:
                    enemy = new Zombie(parMaxHealth, parCurrentHealth, parX, parY, parHeight, parWidth, parSpeed);
                    break;
                case EnemyTypes.Skeleton:
                    enemy = new Skeleton(parMaxHealth, parCurrentHealth, parX, parY, parHeight, parWidth, parSpeed);
                    break;
                case EnemyTypes.Spider:
                    enemy = new Spider(parMaxHealth, parCurrentHealth, parX, parY, parHeight, parWidth, parSpeed);
                    break;
                case EnemyTypes.Golem:
                    enemy = new Golem(parMaxHealth, parCurrentHealth, parX, parY, parHeight, parWidth, parSpeed);
                    break;
            }
            enemy.Type = parEnemyType;

            return enemy;
        }

        /// <summary>
        /// Запускает движение врага в сторону игрока.
        /// </summary>
        /// <param name="parHero">Игрок, к которому движется враг.</param>
        /// <remarks>
        /// Метод асинхронно перемещает врага в сторону игрока с заданной скоростью.
        /// </remarks>
        public async void StartMovingTowardsPlayer(Hero parHero)
        {
            while (!(this.IsDead) && !parHero.IsStopped)
            {
                await Task.Delay(Speed);
                if(!(this.IsDead) && !parHero.IsStopped)
                {
                    MoveEnemyTowardsPlayer(parHero);
                }
            }
        }

        /// <summary>
        /// Перемещает врага в сторону игрока.
        /// </summary>
        /// <param name="parPlayer">Игрок, к которому движется враг.</param>
        /// <remarks>
        /// Метод вычисляет направление движения врага и перемещает его в сторону игрока.
        /// Если враг сталкивается с игроком, игрок теряет здоровье в зависимости от типа врага.
        /// </remarks>
        public void MoveEnemyTowardsPlayer(Hero parPlayer)
        {
            float deltaX = parPlayer.X - this.X;
            float deltaY = parPlayer.Y - this.Y;

            float distance = MathF.Sqrt(deltaX * deltaX + deltaY * deltaY);

            if (distance > 0)
            {
                float moveX = deltaX / distance;
                float moveY = deltaY / distance;

                this.X += moveX > 0 ? GameWindow.GetInstance().Width / 160 : (moveX < 0 ? -GameWindow.GetInstance().Width / 160 : 0);
                this.Y += moveY > 0 ? GameWindow.GetInstance().Width / 160 : (moveY < 0 ? -GameWindow.GetInstance().Width / 160 : 0);
            }

            bool noOverlapX = parPlayer.X + parPlayer.Width <= this.X || this.X + this.Width <= parPlayer.X;
            bool noOverlapY = parPlayer.Y + parPlayer.Height <= this.Y || this.Y + this.Height <= parPlayer.Y;

            if (!(noOverlapX || noOverlapY))
            {
                lock (damageLock)
                {
                    if (this is Zombie)
                    {
                        parPlayer.CurrentHealth -= 10;
                    }
                    else if (this is Skeleton)
                    {
                        parPlayer.CurrentHealth -= 15;
                    }
                    else if (this is Golem)
                    {
                        parPlayer.CurrentHealth -= 30;
                    }
                    else if (this is Spider)
                    {
                        parPlayer.CurrentHealth -= 5;
                    }
                }
            }
        }
    }
}