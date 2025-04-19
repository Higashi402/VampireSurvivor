using MvcModel.Upgrades;
using MvcModel.Сreatures;
using MvcModel.Сreatures.Enemies;
using MvcModel.Сreatures.Heros;
using MvcModel;

namespace MvcModel.Frames
{
    /// <summary>
    /// Класс, представляющий фрейм игрового уровня.
    /// </summary>
    /// <remarks>
    /// Класс предоставляет функциональность для управления игровым уровнем, включая игрока, врагов и препятствия.
    /// При создании экземпляра класса вызывается событие <see cref="LevelInitialized"/>.
    /// </remarks>
    public class LevelFrame : Frame
    {
        /// <summary>
        /// Получает или задает игрока на уровне.
        /// </summary>
        /// <value>
        /// Игрок, управляемый пользователем.
        /// </value>
        public Hero Player { get; set; }

        /// <summary>
        /// Список врагов на уровне.
        /// </summary>
        /// <value>
        /// Синхронизированный список врагов.
        /// </value>
        public SynchronizedList<Enemy> Enemies = new SynchronizedList<Enemy>();

        /// <summary>
        /// Список препятствий на уровне.
        /// </summary>
        /// <value>
        /// Список препятствий (камней).
        /// </value>
        public List<Rock> Rocks = new List<Rock>();

        /// <summary>
        /// Событие, возникающее при инициализации фрейма уровня.
        /// </summary>
        /// <remarks>
        /// Событие позволяет выполнить дополнительные действия при создании уровня.
        /// </remarks>
        public static event Action<LevelFrame> LevelInitialized;

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <remarks>
        /// Инициализирует новый экземпляр класса <see cref="LevelFrame"/> и вызывает событие <see cref="LevelInitialized"/>.
        /// </remarks>
        public LevelFrame()
        {
            LevelInitialized?.Invoke(this);
        }
    }
}