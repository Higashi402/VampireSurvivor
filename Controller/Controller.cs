// Подключение необходимых библиотек
using MvcModel.Frames; // Использование пространства имен для работы с кадрами (Frame)
using System;

// Пространство имен MvcController
// Содержит классы, отвечающие за управление логикой контроллера в MVC-архитектуре
namespace MvcController
{
    /// <summary>
    /// Класс Controller
    /// Реализует логику управления кадрами (Frame) и поддерживает механизм освобождения ресурсов.
    /// </summary>
    public class Controller : IDisposable
    {
        // Приватное поле для хранения текущего кадра (Frame)
        private Frame? _frame;

        // Флаг, указывающий, были ли освобождены ресурсы
        private bool _disposed = false;

        // Статическое поле для хранения ширины окна
        public static short WINDOW_WIDTH;

        // Статическое поле для хранения высоты окна
        public static short WINDOW_HEIGHT;

        // Статическое событие, возникающее при изменении кадра (Frame)
        public static event Action<Frame> FrameChanged;

        /// <summary>
        /// Свойство Frame
        /// Предоставляет доступ к текущему кадру (Frame) и уведомляет об его изменении.
        /// </summary>
        public Frame Frame
        {
            get => _frame; // Возвращает текущий кадр
            set
            {
                // Если новое значение отличается от текущего
                if (_frame != value)
                {
                    _frame = value; // Обновляем кадр
                    FrameChanged?.Invoke(value); // Вызываем событие, если подписчики есть
                }
            }
        }

        /// <summary>
        /// Конструктор класса Controller
        /// Инициализирует экземпляр класса с указанным кадром (Frame).
        /// </summary>
        /// <param name="parFrame">Кадр (Frame), который будет использоваться контроллером.</param>
        public Controller(Frame parFrame)
        {
            _frame = parFrame; // Инициализация кадра
        }

        /// <summary>
        /// Конструктор по умолчанию
        /// Создает экземпляр класса Controller без инициализации кадра.
        /// </summary>
        public Controller()
        {
            // Пустой конструктор
        }

        /// <summary>
        /// Метод Dispose
        /// Реализует интерфейс IDisposable для освобождения ресурсов.
        /// </summary>
        public void Dispose()
        {
            if (!_disposed) // Проверка, были ли уже освобождены ресурсы
            {
                _frame = null; // Освобождение ресурса
                _disposed = true; // Установка флага освобождения
            }
        }

        /// <summary>
        /// Деструктор класса Controller
        /// Обеспечивает освобождение ресурсов, если метод Dispose не был вызван явно.
        /// </summary>
        ~Controller()
        {
            Dispose(); // Вызов метода Dispose
        }
    }
}