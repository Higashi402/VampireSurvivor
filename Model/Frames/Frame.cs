using MvcModel.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcModel.Frames
{
    /// <summary>
    /// Абстрактный класс, представляющий базовый фрейм.
    /// </summary>
    /// <remarks>
    /// Класс предоставляет базовую функциональность для работы с метками (Label) в рамках фрейма.
    /// </remarks>
    public abstract class Frame
    {
        /// <summary>
        /// Список меток, связанных с фреймом.
        /// </summary>
        private List<Label> _labels = new List<Label>();

        /// <summary>
        /// Получает список меток, связанных с фреймом.
        /// </summary>
        /// <value>
        /// Список меток.
        /// </value>
        public List<Label> Labels { get { return _labels; } }

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <remarks>
        /// Инициализирует новый экземпляр класса <see cref="Frame"/>.
        /// </remarks>
        public Frame()
        {
        }

        /// <summary>
        /// Добавляет метку в список меток фрейма.
        /// </summary>
        /// <param name="parElement">Метка, которую необходимо добавить.</param>
        /// <remarks>
        /// Метод добавляет переданную метку в список меток, связанных с фреймом.
        /// </remarks>
        public void AddLabel(Label parElement)
        {
            Labels.Add(parElement);
        }
    }
}