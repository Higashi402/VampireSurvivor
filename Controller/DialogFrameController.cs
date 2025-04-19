using MvcController.Tools;
using MvcModel.Frames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcController
{
    /// <summary>
    /// Класс DialogFrameController управляет фреймами диалогов и обработкой взаимодействия с кнопками.
    /// Наследуется от базового класса Controller.
    /// </summary>
    public class DialogFrameController : Controller
    {
        /// <summary>
        /// Конструктор класса DialogFrameController, принимающий DialogFrame в качестве параметра.
        /// </summary>
        /// <param name="parFrame">Фрейм диалога для инициализации контроллера.</param>
        public DialogFrameController(DialogFrame parFrame) : base(parFrame)
        {
            Frame = parFrame;
        }

        /// <summary>
        /// Конструктор класса DialogFrameController, принимающий базовый Frame в качестве параметра.
        /// </summary>
        /// <param name="parFrame">Базовый фрейм для инициализации контроллера.</param>
        public DialogFrameController(Frame parFrame)
        {
            Frame = parFrame;
        }

        /// <summary>
        /// Конструктор по умолчанию для класса DialogFrameController.
        /// </summary>
        public DialogFrameController()
        {

        }

        /// <summary>
        /// Изменяет активную кнопку в диалоговом фрейме в зависимости от переданного параметра.
        /// </summary>
        /// <param name="parButton">Код клавиши, определяющей направление изменения активной кнопки.</param>
        public void ChangeActiveButton(int parButton)
        {
            if (Frame is DialogFrame dialogFrame)
            {
                if (dialogFrame.Buttons.Count != 0)
                {
                    // Обработка нажатия клавиш S или D (переход к следующей кнопке)
                    if (parButton == (int)MyKey.S || parButton == (int)MyKey.D)
                    {
                        if (dialogFrame._activeButtonIndex != dialogFrame.Buttons.Count)
                        {
                            dialogFrame.ChangeActiveButton(dialogFrame._activeButtonIndex + 1);
                        }
                    }
                    // Обработка нажатия клавиш W или A (переход к предыдущей кнопке)
                    else if (parButton == (int)MyKey.W || parButton == (int)MyKey.A)
                    {
                        if (dialogFrame._activeButtonIndex != 1)
                        {
                            dialogFrame.ChangeActiveButton(dialogFrame._activeButtonIndex - 1);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Завершает работу приложения.
        /// </summary>
        public void ExitGame()
        {
            Environment.Exit(0);
        }
    }
}