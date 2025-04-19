using MvcModel.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcModel.Frames
{
    /// <summary>
    /// Класс, представляющий диалоговый фрейм.
    /// </summary>
    /// <remarks>
    /// Класс предоставляет функциональность для управления кнопками и формами в диалоговом окне.
    /// </remarks>
    public class DialogFrame : Frame
    {
        /// <summary>
        /// Индекс активной кнопки.
        /// </summary>
        public int _activeButtonIndex = 1;

        /// <summary>
        /// Получает или задает список кнопок в диалоговом фрейме.
        /// </summary>
        /// <value>
        /// Список кнопок.
        /// </value>
        public List<Button> Buttons { get; set; }

        /// <summary>
        /// Получает или задает список форм в диалоговом фрейме.
        /// </summary>
        /// <value>
        /// Список форм.
        /// </value>
        public List<Form> Forms { get; set; }

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <remarks>
        /// Инициализирует новый экземпляр класса <see cref="DialogFrame"/>.
        /// </remarks>
        public DialogFrame() { }

        /// <summary>
        /// Изменяет активную кнопку в диалоговом фрейме.
        /// </summary>
        /// <param name="parIndex">Индекс новой активной кнопки.</param>
        /// <remarks>
        /// Метод изменяет состояние всех кнопок, делая активной кнопку с указанным индексом.
        /// </remarks>
        public void ChangeActiveButton(int parIndex)
        {
            foreach (Button button in Buttons)
            {
                button.ChangeStatus(false);
            }
            Buttons[parIndex - 1].ChangeStatus(true);
            _activeButtonIndex = parIndex;
        }

        /// <summary>
        /// Находит индекс активной кнопки.
        /// </summary>
        /// <remarks>
        /// Метод обновляет значение <see cref="_activeButtonIndex"/> на основе текущей активной кнопки.
        /// Если активная кнопка не найдена, устанавливает значение по умолчанию (1).
        /// </remarks>
        public void FindActiveButtonIndex()
        {
            for (int i = 1; i <= Buttons.Count; i++)
            {
                if (Buttons[i].IsActive)
                {
                    _activeButtonIndex = i;
                }
            }
            _activeButtonIndex = 1;
        }
    }
}