using MvcModel.Elements;
using MvcModel.Сreatures.Enemies;
using MvcModel.Сreatures.Heros;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcModel.Frames
{
    /// <summary>
    /// Класс, представляющий фрейм главного меню.
    /// </summary>
    /// <remarks>
    /// Класс наследует от <see cref="DialogFrame"/> и предоставляет функциональность для отображения главного меню игры.
    /// При создании экземпляра класса вызывается событие <see cref="MainMenuInitialized"/>.
    /// </remarks>
    public class MainMenuFrame : DialogFrame
    {
        /// <summary>
        /// Событие, возникающее при инициализации фрейма главного меню.
        /// </summary>
        /// <remarks>
        /// Событие позволяет выполнить дополнительные действия при создании главного меню.
        /// </remarks>
        public static event Action<MainMenuFrame> MainMenuInitialized;

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <remarks>
        /// Инициализирует новый экземпляр класса <see cref="MainMenuFrame"/> и вызывает событие <see cref="MainMenuInitialized"/>.
        /// </remarks>
        public MainMenuFrame()
        {
            MainMenuInitialized?.Invoke(this);
        }
    }
}