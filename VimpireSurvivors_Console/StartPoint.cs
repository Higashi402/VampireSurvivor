using Microsoft.Win32.SafeHandles;
using MvcModel.Frames;
using VimpireSurvivors_Console.Displayer;
using MvcController.Tools;
using MvcController;
using System.Text;
using MvcModel;

namespace VimpireSurvivors_Console
{
    /// <summary>
    /// Класс, представляющий точку входа в приложение.
    /// </summary>
    /// <remarks>
    /// Класс отвечает за инициализацию консоли, создание основных компонентов игры (контроллеров, менеджеров отрисовки и слушателя клавиш)
    /// и запуск игрового процесса.
    /// </remarks>
    internal class StartPoint
    {
        /// <summary>
        /// Точка входа в приложение.
        /// </summary>
        /// <param name="args">Аргументы командной строки.</param>
        /// <remarks>
        /// Метод инициализирует консоль, создает основные компоненты игры и запускает игровой процесс.
        /// </remarks>
        [STAThread]
        static void Main(string[] args)
        {
            // Инициализация консоли для быстрой отрисовки
            SafeFileHandle hConsoleOutput = ConsoleFastOutput.CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
            ConsoleFastOutput.InitializeConsoleFastOutput(hConsoleOutput);

            // Инициализация фреймов
            FrameInitializer fi = new FrameInitializer();

            // Создание главного меню и контроллера
            MainMenuFrame mainMenu = new MainMenuFrame();
            DialogFrameController mainMenuController = new DialogFrameController();

            // Инициализация менеджера отрисовки
            RenderManager renderManager = new RenderManager(hConsoleOutput);
            renderManager.Controller = mainMenuController;
            renderManager.Controller.Frame = new MainMenuFrame();

            // Инициализация слушателя клавиш и запуск игрового процесса
            KeyListener keyListener = new KeyListener(mainMenuController);
            renderManager.StartRender();
            keyListener.StartKeyListener();
        }
    }
}