// Подключение необходимых библиотек
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Пространство имен MvcController.Tools
// Содержит инструменты для работы с контроллерами в MVC-архитектуре
namespace MvcController.Tools
{
    /// <summary>
    /// Перечисление MyKey
    /// Определяет коды клавиш, используемых в приложении.
    /// </summary>
    public enum MyKey
    {
        /// <summary>
        /// Клавиша "Пробел"
        /// </summary>
        SPACE = 0x20,

        /// <summary>
        /// Клавиша "W"
        /// </summary>
        W = 0x57,

        /// <summary>
        /// Клавиша "A"
        /// </summary>
        A = 0x41,

        /// <summary>
        /// Клавиша "S"
        /// </summary>
        S = 0x53,

        /// <summary>
        /// Клавиша "D"
        /// </summary>
        D = 0x44,

        /// <summary>
        /// Клавиша "Escape"
        /// </summary>
        ESCAPE = 0x1B,

        /// <summary>
        /// Клавиша "Enter"
        /// </summary>
        ENTER = 0x0D,

        /// <summary>
        /// Клавиша "Shift"
        /// </summary>
        SHIFT = 0x10,

        /// <summary>
        /// Событие "Клавиша отпущена"
        /// </summary>
        KEYUP = 0x0101,

        /// <summary>
        /// Событие "Клавиша нажата"
        /// </summary>
        KEYDOWN = 0x0100
    }
}