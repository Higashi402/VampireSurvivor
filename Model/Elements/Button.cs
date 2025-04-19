using MvcModel.Upgrades;
using System.Drawing;

namespace MvcModel.Elements
{
    /// <summary>
    /// Класс, представляющий кнопку в пользовательском интерфейсе.
    /// </summary>
    /// <remarks>
    /// Класс предоставляет функциональность для создания и управления кнопками, включая изменение их состояния,
    /// текста, цвета фона и текста, а также связь с улучшениями (Upgrade).
    /// </remarks>
    public class Button : Element
    {
        /// <summary>
        /// Флаг, указывающий, активна ли кнопка.
        /// </summary>
        private bool _isActive = false;

        /// <summary>
        /// Получает или задает текст кнопки.
        /// </summary>
        /// <value>
        /// Текст кнопки.
        /// </value>
        public string Text { get; set; }

        /// <summary>
        /// Получает или задает тип улучшения, связанного с кнопкой.
        /// </summary>
        /// <value>
        /// Тип улучшения.
        /// </value>
        public UpgradeType InternalUpgrade { get; set; }

        /// <summary>
        /// Получает или задает следующий уровень улучшения, связанного с кнопкой.
        /// </summary>
        /// <value>
        /// Следующий уровень улучшения.
        /// </value>
        public int NextLevelUpgrade { get; set; }

        /// <summary>
        /// Получает или задает описание улучшения, связанного с кнопкой.
        /// </summary>
        /// <value>
        /// Описание улучшения.
        /// </value>
        public string Discription { get; set; }

        /// <summary>
        /// Получает или задает значение, указывающее, активна ли кнопка.
        /// </summary>
        /// <value>
        /// <c>true</c>, если кнопка активна; иначе <c>false</c>.
        /// </value>
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                _isActive = value;
            }
        }

        /// <summary>
        /// Получает или задает цвет фона кнопки.
        /// </summary>
        /// <value>
        /// Цвет фона кнопки.
        /// </value>
        public ConsoleColor BackgroundColor { get; set; }

        /// <summary>
        /// Получает или задает цвет текста кнопки.
        /// </summary>
        /// <value>
        /// Цвет текста кнопки.
        /// </value>
        public ConsoleColor TextColor { get; set; }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Button"/>.
        /// </summary>
        /// <param name="text">Текст кнопки.</param>
        /// <param name="parCoordinates">Координаты кнопки.</param>
        /// <param name="parHeight">Высота кнопки.</param>
        /// <param name="parWidth">Ширина кнопки.</param>
        /// <remarks>
        /// Конструктор инициализирует кнопку с указанным текстом, координатами, высотой и шириной.
        /// По умолчанию цвет фона кнопки устанавливается в зеленый.
        /// </remarks>
        public Button(string text, Point parCoordinates, int parHeight, int parWidth)
        {
            BackgroundColor = ConsoleColor.Green;
            this.Text = text;
            this.Coordinates = parCoordinates;
            this.Height = parHeight;
            this.Width = parWidth;
        }

        /// <summary>
        /// Изменяет состояние кнопки (активна/неактивна).
        /// </summary>
        /// <param name="parStatus">Новое состояние кнопки.</param>
        /// <remarks>
        /// Метод изменяет состояние кнопки и обновляет цвет фона в зависимости от нового состояния.
        /// Если кнопка активна, цвет фона становится красным, иначе — зеленым.
        /// </remarks>
        public void ChangeStatus(bool parStatus)
        {
            IsActive = parStatus;

            if (_isActive)
            {
                BackgroundColor = ConsoleColor.Red;
            }
            else
            {
                BackgroundColor = ConsoleColor.Green;
            }
        }
    }
}