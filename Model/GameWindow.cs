using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcModel
{
    /// <summary>
    /// Класс GameWindow представляет игровое окно и реализует шаблон Singleton для управления единственным экземпляром.
    /// </summary>
    public class GameWindow
    {
        // Приватное статическое поле для хранения единственного экземпляра класса
        private static GameWindow _instance;

        // Объект для синхронизации потоков при создании экземпляра
        private static readonly object _lock = new object();

        /// <summary>
        /// Ширина игрового окна.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Высота игрового окна.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Приватный конструктор для предотвращения создания экземпляров извне.
        /// </summary>
        private GameWindow()
        {

        }

        /// <summary>
        /// Метод для получения единственного экземпляра класса GameWindow.
        /// Реализует потокобезопасный Singleton.
        /// </summary>
        /// <returns>Единственный экземпляр GameWindow.</returns>
        public static GameWindow GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new GameWindow();
                    }
                }
            }
            return _instance;
        }

        /// <summary>
        /// Метод для изменения размеров игрового окна.
        /// </summary>
        /// <param name="parWidth">Новая ширина окна.</param>
        /// <param name="parHeight">Новая высота окна.</param>
        public void Resize(int parWidth, int parHeight)
        {
            Width = parWidth;
            Height = parHeight;
        }

        /// <summary>
        /// Метод для изменения размеров консольного окна.
        /// </summary>
        /// <param name="parWidth">Новая ширина консольного окна.</param>
        /// <param name="parHeight">Новая высота консольного окна.</param>
        public void ResizeConsole(int parWidth, int parHeight)
        {
            Width = parWidth;
            Height = parHeight;

            Console.SetWindowSize(Width, Height);
        }
    }
}