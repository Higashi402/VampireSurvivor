using MvcModel.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcModel.Frames
{
    /// <summary>
    /// Класс, представляющий фрейм меню выбора уровней.
    /// </summary>
    /// <remarks>
    /// Класс наследует от <see cref="DialogFrame"/> и предоставляет функциональность для отображения меню выбора уровней.
    /// При создании экземпляра класса вызывается событие <see cref="LevelsMenuInitialized"/>.
    /// </remarks>
    public class LevelsMenuFrame : DialogFrame
    {
        /// <summary>
        /// Событие, возникающее при инициализации фрейма меню выбора уровней.
        /// </summary>
        /// <remarks>
        /// Событие позволяет выполнить дополнительные действия при создании меню выбора уровней.
        /// </remarks>
        public static event Action<LevelsMenuFrame> LevelsMenuInitialized;

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <remarks>
        /// Инициализирует новый экземпляр класса <see cref="LevelsMenuFrame"/> и вызывает событие <see cref="LevelsMenuInitialized"/>.
        /// </remarks>
        public LevelsMenuFrame()
        {
            LevelsMenuInitialized?.Invoke(this);
        }
    }
}