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
    /// Класс PauseFrame представляет фрейм паузы в игре.
    /// Наследуется от базового класса DialogFrame.
    /// </summary>
    public class PauseFrame : DialogFrame
    {
        /// <summary>
        /// Событие, возникающее при инициализации фрейма паузы.
        /// </summary>
        public static event Action<PauseFrame> PauseFrameInitialized;

        /// <summary>
        /// Конструктор по умолчанию для класса PauseFrame.
        /// Вызывает событие PauseFrameInitialized при создании экземпляра.
        /// </summary>
        public PauseFrame()
        {
            PauseFrameInitialized?.Invoke(this);
        }
    }
}