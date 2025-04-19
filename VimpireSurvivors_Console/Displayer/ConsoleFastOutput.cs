using Microsoft.Win32.SafeHandles;
using MvcController;
using MvcModel;
using System.Runtime.InteropServices;

namespace VimpireSurvivors_Console.Displayer
{
    /// <summary>
    /// Класс ConsoleFastOutput предоставляет функционал для быстрой отрисовки в консоли с использованием WinAPI.
    /// </summary>
    public class ConsoleFastOutput
    {
        // Импорт функций WinAPI для работы с консолью
        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern SafeFileHandle CreateFile(
            string fileName,
            [MarshalAs(UnmanagedType.U4)] uint fileAccess,
            [MarshalAs(UnmanagedType.U4)] uint fileShare,
            nint securityAttributes,
            [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
            [MarshalAs(UnmanagedType.U4)] int flags,
            nint template);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteConsoleOutput(
            SafeFileHandle hConsoleOutput,
            CharInfo[] lpBuffer,
            Coord dwBufferSize,
            Coord dwBufferCoord,
            ref SmallRect lpWriteRegion);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleScreenBufferSize(SafeFileHandle hConsoleOutput, Coord dwSize);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleWindowInfo(SafeFileHandle hConsoleOutput, bool bAbsolute, ref SmallRect lpConsoleWindow);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern Coord GetLargestConsoleWindowSize(SafeFileHandle hConsoleOutput);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern nint GetStdHandle(int nStdHandle);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool ShowWindow(nint hWnd, int nCmdShow);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern nint GetConsoleWindow();

        /// <summary>
        /// Структура для хранения информации о шрифте консоли.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct CONSOLE_FONT_INFO_EX
        {
            public uint cbSize;
            public uint nFont;
            public short dwFontSizeX;
            public short dwFontSizeY;
            public uint FontFamily;
            public uint FontWeight;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string FaceName;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetCurrentConsoleFontEx(
            nint consoleOutput,
            bool maximumWindow,
            ref CONSOLE_FONT_INFO_EX fontInfo
        );

        private const int SW_MAXIMIZE = 3; // Константа для максимизации окна
        public const int STD_OUTPUT_HANDLE = -11; // Константа для дескриптора стандартного вывода

        /// <summary>
        /// Структура для хранения координат.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Coord
        {
            public short X;
            public short Y;

            public Coord(short X, short Y)
            {
                this.X = X;
                this.Y = Y;
            }
        };

        /// <summary>
        /// Структура для хранения символа (Unicode или ASCII).
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        public struct CharUnion
        {
            [FieldOffset(0)] public char UnicodeChar;
            [FieldOffset(0)] public byte AsciiChar;
        }

        /// <summary>
        /// Структура для хранения информации о символе и его атрибутах.
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        public struct CharInfo
        {
            [FieldOffset(0)] public CharUnion Char;
            [FieldOffset(2)] public short Attributes;
        }

        /// <summary>
        /// Структура для хранения координат прямоугольной области.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct SmallRect
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }

        /// <summary>
        /// Инициализирует консоль для быстрой отрисовки.
        /// </summary>
        /// <param name="hConsoleOutput">Дескриптор консоли.</param>
        public static void InitializeConsoleFastOutput(SafeFileHandle hConsoleOutput)
        {
            SetConsoleFullScreen();
        }

        /// <summary>
        /// Устанавливает консоль в полноэкранный режим.
        /// </summary>
        public static void SetConsoleFullScreen()
        {
            SafeFileHandle consoleHandle = new SafeFileHandle(GetStdHandle(STD_OUTPUT_HANDLE), ownsHandle: false);

            if (consoleHandle.IsInvalid)
                throw new InvalidOperationException("Не удалось получить дескриптор консоли.");

            nint consoleOutput = GetStdHandle(STD_OUTPUT_HANDLE);

            CONSOLE_FONT_INFO_EX fontInfo = new CONSOLE_FONT_INFO_EX
            {
                dwFontSizeX = 12,
                dwFontSizeY = 16,
                FontFamily = 0x30,
                FontWeight = 400
            };

            if (!SetCurrentConsoleFontEx(consoleOutput, false, ref fontInfo))
            {
                Console.WriteLine("Ошибка изменения шрифта консоли!");
            }

            Coord largestSize = GetLargestConsoleWindowSize(consoleHandle);

            GameWindow.GetInstance().ResizeConsole(largestSize.X, largestSize.Y);

            Coord bufferSize = new Coord
            {
                X = largestSize.X,
                Y = largestSize.Y
            };
            if (!SetConsoleScreenBufferSize(consoleHandle, bufferSize))
                throw new InvalidOperationException("Не удалось установить размер буфера.");

            SmallRect windowRect = new SmallRect
            {
                Left = 0,
                Top = 0,
                Right = (short)(largestSize.X - 1),
                Bottom = (short)(largestSize.Y - 1)
            };
            if (!SetConsoleWindowInfo(consoleHandle, true, ref windowRect))
                throw new InvalidOperationException("Не удалось установить размеры окна.");

            nint consoleWindow = GetConsoleWindow();
            if (consoleWindow == nint.Zero)
            {
                Console.WriteLine("Не удалось получить окно консоли.");
                return;
            }

            ShowWindow(consoleWindow, SW_MAXIMIZE);
        }
    }
}