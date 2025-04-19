using MvcModel.Elements;
using MvcModel.Сreatures.Heros;
using System.Drawing;

namespace MvcModel.Frames
{
    /// <summary>
    /// Класс, представляющий фрейм завершения игры.
    /// </summary>
    /// <remarks>
    /// Класс наследует от <see cref="DialogFrame"/> и предоставляет функциональность для отображения экрана завершения игры.
    /// При создании экземпляра класса вызывается событие <see cref="GameOverInitialized"/>.
    /// </remarks>
    public class GameOverFrame : DialogFrame
    {
        /// <summary>
        /// Событие, возникающее при инициализации фрейма завершения игры.
        /// </summary>
        /// <remarks>
        /// Событие позволяет выполнить дополнительные действия при создании экрана завершения игры.
        /// </remarks>
        public static event Action<GameOverFrame> GameOverInitialized;

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <remarks>
        /// Инициализирует новый экземпляр класса <see cref="GameOverFrame"/> и вызывает событие <see cref="GameOverInitialized"/>.
        /// </remarks>
        public GameOverFrame()
        {
            GameOverInitialized?.Invoke(this);
        }
    }
}