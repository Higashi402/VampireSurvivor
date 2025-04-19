using MvcModel.Elements;
using System.Drawing;

namespace MvcModel.Frames
{
    /// <summary>
    /// Класс NameInputFrame представляет фрейм для ввода имени пользователя.
    /// Наследуется от базового класса Frame.
    /// </summary>
    public class NameInputFrame : Frame
    {
        /// <summary>
        /// Текстовое поле для ввода имени.
        /// </summary>
        public TextBox Name;

        /// <summary>
        /// Флаг, указывающий, активно ли сообщение об ошибке.
        /// </summary>
        public bool IsErrorMessaheActive = false;

        /// <summary>
        /// Позиция курсора в текстовом поле.
        /// </summary>
        public int CursorPosition { get; set; }

        /// <summary>
        /// Событие, возникающее при инициализации фрейма для ввода имени.
        /// </summary>
        public static event Action<NameInputFrame> NameInputInitialized;

        /// <summary>
        /// Конструктор по умолчанию для класса NameInputFrame.
        /// Вызывает событие NameInputInitialized при создании экземпляра.
        /// </summary>
        public NameInputFrame()
        {
            NameInputInitialized?.Invoke(this);
        }
    }
}