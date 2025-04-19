using MvcModel.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MvcModel.Frames
{
    /// <summary>
    /// Класс RulesFrame представляет фрейм для отображения правил игры.
    /// Наследуется от базового класса DialogFrame.
    /// </summary>
    public class RulesFrame : DialogFrame
    {
        /// <summary>
        /// Событие, возникающее при инициализации фрейма правил.
        /// </summary>
        public static event Action<RulesFrame> RulesFrameInitialized;

        /// <summary>
        /// Конструктор по умолчанию для класса RulesFrame.
        /// Вызывает событие RulesFrameInitialized при создании экземпляра.
        /// </summary>
        public RulesFrame()
        {
            RulesFrameInitialized?.Invoke(this);
        }
    }
}