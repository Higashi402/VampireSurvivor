using Microsoft.Win32.SafeHandles;
using MvcModel.Frames;
using MvcController;
using MvcModel;
using MvcModel.Elements;
using System.Diagnostics;
using static VimpireSurvivors_Console.Displayer.ConsoleFastOutput;
using System.Drawing;
using MvcModel.Сreatures.Enemies;
using MvcModel.Сreatures;
using MvcModel.Сreatures.Heros;

namespace VimpireSurvivors_Console.Displayer
{
    /// <summary>
    /// Класс RenderManager управляет отрисовкой игровых элементов в консоли.
    /// </summary>
    public class RenderManager
    {
        private Stopwatch _stopwatch = new Stopwatch(); // Таймер для расчета FPS
        private int _frameCount = 0; // Счетчик кадров
        private double _fps = 0; // Текущее значение FPS
        private CharInfo[] _buffer; // Буфер для отрисовки

        private int WIDTH; // Ширина окна консоли
        private int HEIGHT; // Высота окна консоли

        /// <summary>
        /// Дескриптор консоли для вывода.
        /// </summary>
        public SafeFileHandle ConsoleOutput { get; set; }

        /// <summary>
        /// Контроллер, управляющий текущим состоянием игры.
        /// </summary>
        public Controller Controller { get; set; }

        /// <summary>
        /// Флаг, указывающий, выполняется ли отрисовка.
        /// </summary>
        public bool IsRendering { get; set; }

        /// <summary>
        /// Текущий фрейм для отрисовки.
        /// </summary>
        public Frame Frame { get; set; }

        /// <summary>
        /// Конструктор класса RenderManager.
        /// </summary>
        /// <param name="parConsoleOutput">Дескриптор консоли для вывода.</param>
        public RenderManager(SafeFileHandle parConsoleOutput)
        {
            WIDTH = GameWindow.GetInstance().Width;
            HEIGHT = GameWindow.GetInstance().Height;

            ConsoleOutput = parConsoleOutput;
            Controller.FrameChanged += OnFrameChanged; // Подписка на изменение фрейма
            KeyListener.ControllerChanged += OnControllerChanged; // Подписка на изменение контроллера
            LevelController.WhipAttacked += OnWhipAttacked; // Подписка на атаку кнутом
            LevelController.ThunderAttacked += OnThunderAttacked; // Подписка на атаку молнией
        }

        /// <summary>
        /// Обработчик изменения текущего фрейма.
        /// </summary>
        /// <param name="parFrame">Новый фрейм.</param>
        public void OnFrameChanged(Frame parFrame)
        {
            Frame = parFrame;
        }

        /// <summary>
        /// Обработчик изменения контроллера.
        /// </summary>
        /// <param name="parController">Новый контроллер.</param>
        public void OnControllerChanged(Controller parController)
        {
            Controller = parController;
        }

        /// <summary>
        /// Запускает процесс отрисовки.
        /// </summary>
        public void StartRender()
        {
            IsRendering = true;
            Task.Run(() => { Render(); });
        }

        /// <summary>
        /// Останавливает процесс отрисовки.
        /// </summary>
        public void StopRender()
        {
            IsRendering = false;
        }

        /// <summary>
        /// Основной метод отрисовки.
        /// </summary>
        public void Render()
        {
            _stopwatch.Start();

            while (IsRendering)
            {
                if (Controller.Frame is LevelFrame level)
                {
                    RenderFrame(level);
                }
                else if (Controller.Frame is MainMenuFrame menu)
                {
                    RenderMainMenu(menu);
                }
                else if (Controller.Frame is LevelsMenuFrame levelsMenu)
                {
                    RenderLevelsMenu(levelsMenu);
                }
                else if (Controller.Frame is GameOverFrame gameOver)
                {
                    RenderGameOver(gameOver);
                }
                else if (Controller.Frame is UpgradeFrame upgrade)
                {
                    RenderUpgrade(upgrade);
                }
                else if (Controller.Frame is NameInputFrame nameInput)
                {
                    RenderNameInput(nameInput);
                }
                else if (Controller.Frame is RecordsFrame recordsFrame)
                {
                    RenderRecords(recordsFrame);
                }
                else if (Controller.Frame is RulesFrame rulesFrame)
                {
                    RenderRules(rulesFrame);
                }
                else if (Controller.Frame is PauseFrame pauseFrame)
                {
                    RenderPause(pauseFrame);
                }

                _frameCount++;

                if (_stopwatch.ElapsedMilliseconds >= 1000)
                {
                    _fps = _frameCount / (_stopwatch.ElapsedMilliseconds / 1000.0);
                    _frameCount = 0;
                    _stopwatch.Restart();
                }
            }

            _stopwatch.Stop();
        }

        /// <summary>
        /// Отрисовывает главное меню.
        /// </summary>
        /// <param name="parMainMenu">Фрейм главного меню.</param>
        public void RenderMainMenu(MainMenuFrame parMainMenu)
        {
            _buffer = new CharInfo[WIDTH * HEIGHT];
            SmallRect writeRegion = new SmallRect
            {
                Left = 0,
                Top = 0,
                Right = (short)(WIDTH - 1),
                Bottom = (short)(HEIGHT - 1)
            };
            foreach (Button button in parMainMenu.Buttons)
            {
                BufferWriter.WriteButton(button, _buffer);
            }

            foreach (Label label in parMainMenu.Labels)
            {
                BufferWriter.WriteLabel(label, _buffer);
            }

            WriteConsoleOutput(ConsoleOutput, _buffer, new Coord((short)WIDTH, (short)HEIGHT), new Coord(0, 0), ref writeRegion);
        }

        /// <summary>
        /// Отрисовывает поле ввода имени.
        /// </summary>
        /// <param name="parNameInput">Фрейм ввода имени.</param>
        public void RenderNameInput(NameInputFrame parNameInput)
        {
            _buffer = new CharInfo[WIDTH * HEIGHT];

            SmallRect writeRegion = new SmallRect
            {
                Left = 0,
                Top = 0,
                Right = (short)(WIDTH - 1),
                Bottom = (short)(HEIGHT - 1)
            };
            BufferWriter.WriteTextBox(parNameInput.Name, _buffer);

            if (parNameInput.IsErrorMessaheActive)
            {
                BufferWriter.WriteLabel(parNameInput.Labels[0], _buffer);
            }
            BufferWriter.WriteLabel(parNameInput.Labels[1], _buffer);

            WriteConsoleOutput(ConsoleOutput, _buffer, new Coord((short)WIDTH, (short)HEIGHT), new Coord(0, 0), ref writeRegion);
        }

        /// <summary>
        /// Отрисовывает меню выбора уровней.
        /// </summary>
        /// <param name="parMainMenu">Фрейм меню выбора уровней.</param>
        public void RenderLevelsMenu(LevelsMenuFrame parMainMenu)
        {
            _buffer = new CharInfo[WIDTH * HEIGHT];
            SmallRect writeRegion = new SmallRect
            {
                Left = 0,
                Top = 0,
                Right = (short)(WIDTH - 1),
                Bottom = (short)(HEIGHT - 1)
            };

            foreach (Button button in parMainMenu.Buttons)
            {
                BufferWriter.WriteButton(button, _buffer);
            }

            WriteConsoleOutput(ConsoleOutput, _buffer, new Coord((short)WIDTH, (short)HEIGHT), new Coord(0, 0), ref writeRegion);
        }

        /// <summary>
        /// Отрисовывает игровой уровень.
        /// </summary>
        /// <param name="parLevel">Фрейм игрового уровня.</param>
        private void RenderFrame(LevelFrame parLevel)
        {
            _buffer = new CharInfo[WIDTH * HEIGHT];

            for (int i = 0; i < WIDTH * HEIGHT; i++)
            {
                _buffer[i].Char.UnicodeChar = ' ';
                _buffer[i].Attributes = 0x0020;
            }

            foreach (Rock rock in parLevel.Rocks)
            {
                DrawRock(rock);
            }

            lock (parLevel)
            {
                parLevel.Enemies.ForEach(item =>
                {
                    DrawEnemy(item);
                });
            }

            DrawPlayer(parLevel.Player);

            DrawHeroLevelBar(parLevel.Player);

            DrawHeroHealthBar(parLevel.Player);

            DrawFps(_buffer);

            DrawTimer(parLevel);

            SmallRect writeRegion = new SmallRect
            {
                Left = 0,
                Top = 0,
                Right = (short)(WIDTH - 1),
                Bottom = (short)(HEIGHT - 1)
            };

            WriteConsoleOutput(ConsoleOutput, _buffer, new Coord((short)WIDTH, (short)HEIGHT), new Coord(0, 0), ref writeRegion);
        }

        /// <summary>
        /// Отрисовывает экран завершения игры.
        /// </summary>
        /// <param name="parGameOver">Фрейм завершения игры.</param>
        private void RenderGameOver(GameOverFrame parGameOver)
        {
            SmallRect writeRegion = new SmallRect
            {
                Left = 0,
                Top = 0,
                Right = (short)(WIDTH - 1),
                Bottom = (short)(HEIGHT - 1)
            };
            foreach (Form form in parGameOver.Forms)
            {
                BufferWriter.WriteForm(form, _buffer);
            }

            foreach (Button button in parGameOver.Buttons)
            {
                BufferWriter.WriteButton(button, _buffer);
            }

            foreach (Label label in parGameOver.Labels)
            {
                BufferWriter.WriteLabel(label, _buffer);
            }

            WriteConsoleOutput(ConsoleOutput, _buffer, new Coord((short)WIDTH, (short)HEIGHT), new Coord(0, 0), ref writeRegion);
        }

        /// <summary>
        /// Отрисовывает экран с правилами игры.
        /// </summary>
        /// <param name="parRulesFrame">Фрейм с правилами игры.</param>
        private void RenderRules(RulesFrame parRulesFrame)
        {
            _buffer = new CharInfo[WIDTH * HEIGHT];

            SmallRect writeRegion = new SmallRect
            {
                Left = 0,
                Top = 0,
                Right = (short)(WIDTH - 1),
                Bottom = (short)(HEIGHT - 1)
            };

            foreach (Label label in parRulesFrame.Labels)
            {
                BufferWriter.WriteLabel(label, _buffer);
            }

            foreach (Button button in parRulesFrame.Buttons)
            {
                BufferWriter.WriteButton(button, _buffer);
            }

            WriteConsoleOutput(ConsoleOutput, _buffer, new Coord((short)WIDTH, (short)HEIGHT), new Coord(0, 0), ref writeRegion);
        }

        /// <summary>
        /// Отрисовывает экран с рекордами.
        /// </summary>
        /// <param name="parRecordsFrame">Фрейм с рекордами.</param>
        private void RenderRecords(RecordsFrame parRecordsFrame)
        {
            _buffer = new CharInfo[WIDTH * HEIGHT];

            SmallRect writeRegion = new SmallRect
            {
                Left = 0,
                Top = 0,
                Right = (short)(WIDTH - 1),
                Bottom = (short)(HEIGHT - 1)
            };

            foreach (Button button in parRecordsFrame.Buttons)
            {
                BufferWriter.WriteButton(button, _buffer);
            }

            foreach (Label label in parRecordsFrame.Labels)
            {
                BufferWriter.WriteLabel(label, _buffer);
            }

            WriteConsoleOutput(ConsoleOutput, _buffer, new Coord((short)WIDTH, (short)HEIGHT), new Coord(0, 0), ref writeRegion);
        }

        /// <summary>
        /// Отрисовывает экран улучшений.
        /// </summary>
        /// <param name="parUpgradeFrame">Фрейм улучшений.</param>
        private void RenderUpgrade(UpgradeFrame parUpgradeFrame)
        {
            SmallRect writeRegion = new SmallRect
            {
                Left = 0,
                Top = 0,
                Right = (short)(WIDTH - 1),
                Bottom = (short)(HEIGHT - 1)
            };
            foreach (Form form in parUpgradeFrame.Forms)
            {
                BufferWriter.WriteForm(form, _buffer);
            }

            foreach (Button button in parUpgradeFrame.Buttons)
            {
                BufferWriter.WriteButton(button, _buffer);
                Label descriptionLabel = new Label(button.Discription, new Point(button.Coordinates.X, button.Coordinates.Y + button.Height + 2), 5, 25);
                descriptionLabel.TextColor = ConsoleColor.Blue;
                BufferWriter.WriteLabel(descriptionLabel, _buffer);
            }

            foreach (Label label in parUpgradeFrame.Labels)
            {
                BufferWriter.WriteLabel(label, _buffer);
            }

            WriteConsoleOutput(ConsoleOutput, _buffer, new Coord((short)WIDTH, (short)HEIGHT), new Coord(0, 0), ref writeRegion);
        }

        /// <summary>
        /// Отрисовывает экран паузы.
        /// </summary>
        /// <param name="parPauseFrame">Фрейм паузы.</param>
        private void RenderPause(PauseFrame parPauseFrame)
        {
            SmallRect writeRegion = new SmallRect
            {
                Left = 0,
                Top = 0,
                Right = (short)(WIDTH - 1),
                Bottom = (short)(HEIGHT - 1)
            };
            foreach (Form form in parPauseFrame.Forms)
            {
                BufferWriter.WriteForm(form, _buffer);
            }

            foreach (Button button in parPauseFrame.Buttons)
            {
                BufferWriter.WriteButton(button, _buffer);
            }

            foreach (Label label in parPauseFrame.Labels)
            {
                BufferWriter.WriteLabel(label, _buffer);
            }

            WriteConsoleOutput(ConsoleOutput, _buffer, new Coord((short)WIDTH, (short)HEIGHT), new Coord(0, 0), ref writeRegion);
        }

        /// <summary>
        /// Отрисовывает FPS в буфере.
        /// </summary>
        /// <param name="parBuffer">Буфер для отрисовки.</param>
        private void DrawFps(CharInfo[] parBuffer)
        {
            string fpsText = $"FPS: {_fps:F2}";

            for (int i = 0; i < fpsText.Length; i++)
            {
                int index = i;
                if (i < parBuffer.Length)
                {
                    parBuffer[i].Char.UnicodeChar = fpsText[i];
                    parBuffer[i].Attributes = 0x0007;
                }
            }
        }

        /// <summary>
        /// Отрисовывает таймер в буфере.
        /// </summary>
        /// <param name="parLevel">Фрейм игрового уровня.</param>
        private void DrawTimer(LevelFrame parLevel)
        {
            if (Controller is LevelController levelController)
            {
                string time = ConvertSecondsToTimeFormat(levelController.GetTimerSeconds());

                for (int i = 0; i < time.Length; i++)
                {
                    int index = i;
                    if (index < _buffer.Length)
                    {
                        _buffer[index + WIDTH / 2 - 1 + WIDTH * (HEIGHT - 2)].Char.UnicodeChar = time[i];
                        _buffer[index].Attributes = 0x0007;
                    }
                }
            }
        }

        /// <summary>
        /// Преобразует секунды в формат времени (MM:SS).
        /// </summary>
        /// <param name="parTotalSeconds">Общее количество секунд.</param>
        /// <returns>Строка в формате MM:SS.</returns>
        private string ConvertSecondsToTimeFormat(int parTotalSeconds)
        {
            int minutes = parTotalSeconds / 60;
            int seconds = parTotalSeconds % 60;
            return $"{minutes:D2}:{seconds:D2}";
        }

        /// <summary>
        /// Отрисовывает игрока в буфере.
        /// </summary>
        /// <param name="parPlayer">Герой для отрисовки.</param>
        private void DrawPlayer(Hero parPlayer)
        {
            int index = parPlayer.Y * WIDTH + parPlayer.X;
            if (index + 2 < _buffer.Length)
            {
                _buffer[index].Char.UnicodeChar = (char)0x2302;
                _buffer[index + WIDTH].Char.UnicodeChar = (char)0x001F;
                _buffer[index + WIDTH * 2].Char.UnicodeChar = (char)0x005E;
            }
        }

        /// <summary>
        /// Отрисовывает полосу уровня героя.
        /// </summary>
        /// <param name="parHero">Герой для отрисовки.</param>
        private void DrawHeroLevelBar(Hero parHero)
        {
            int barWidth = 50;
            int filledWidth = (int)((double)parHero.Experience / parHero.NextLevelExperience * barWidth);

            string levelText = $"{parHero.Level} ({parHero.Experience}/{parHero.NextLevelExperience})";

            int textStartPos = (barWidth - levelText.Length) / 2;

            int startX = (WIDTH - barWidth - 2) / 2;
            int yPosition = 0;
            int startIndex = yPosition * WIDTH + startX;

            _buffer[startIndex].Char.UnicodeChar = '[';
            _buffer[startIndex].Attributes = 0x0030;
            startIndex++;

            for (int i = 0; i < barWidth; i++)
            {
                int bufferIndex = startIndex + i;

                if (bufferIndex >= _buffer.Length)
                    break;

                _buffer[bufferIndex].Char.UnicodeChar = ' ';
                _buffer[bufferIndex].Attributes = 0x0020;

                if (i < filledWidth)
                {
                    _buffer[bufferIndex].Char.UnicodeChar = ' ';
                    _buffer[bufferIndex].Attributes = 0x0040;
                }

                if (i >= textStartPos && i < textStartPos + levelText.Length)
                {
                    _buffer[bufferIndex].Char.UnicodeChar = levelText[i - textStartPos];
                }
            }

            _buffer[startIndex + barWidth].Char.UnicodeChar = ']';
            _buffer[startIndex + barWidth].Attributes = 0x0030;
        }

        /// <summary>
        /// Отрисовывает полосу здоровья героя.
        /// </summary>
        /// <param name="parHero">Герой для отрисовки.</param>
        private void DrawHeroHealthBar(Hero parHero)
        {
            int barWidth = 40;
            int filledWidth = (int)((double)parHero.CurrentHealth / parHero.MaxHealth * barWidth);

            string healthText = $"{parHero.CurrentHealth}/{parHero.MaxHealth}";

            int textStartPos = (barWidth - healthText.Length) / 2;

            int startX = 2;
            int yPosition = HEIGHT - 2;
            int startIndex = yPosition * WIDTH + startX;

            _buffer[startIndex].Char.UnicodeChar = 'H';
            startIndex++;
            _buffer[startIndex].Char.UnicodeChar = 'P';
            startIndex++;

            _buffer[startIndex].Char.UnicodeChar = '[';
            _buffer[startIndex].Attributes = 0x0030;
            startIndex++;

            for (int i = 0; i < barWidth; i++)
            {
                int bufferIndex = startIndex + i;

                if (bufferIndex >= _buffer.Length)
                    break;

                _buffer[bufferIndex].Char.UnicodeChar = ' ';
                _buffer[bufferIndex].Attributes = 0x0020;

                if (i < filledWidth)
                {
                    _buffer[bufferIndex].Char.UnicodeChar = ' ';
                    _buffer[bufferIndex].Attributes = 0x0040;
                }

                if (i >= textStartPos && i < textStartPos + healthText.Length)
                {
                    _buffer[bufferIndex].Char.UnicodeChar = healthText[i - textStartPos];
                }
            }

            _buffer[startIndex + barWidth].Char.UnicodeChar = ']';
            _buffer[startIndex + barWidth].Attributes = 0x0030;
        }

        /// <summary>
        /// Отрисовывает врага в буфере.
        /// </summary>
        /// <param name="parEnemy">Враг для отрисовки.</param>
        private void DrawEnemy(Enemy parEnemy)
        {
            if (parEnemy is Skeleton)
            {
                DrawSkeleton(parEnemy);
            }
            else if (parEnemy is Zombie)
            {
                DrawZombie(parEnemy);
            }
            else if (parEnemy is Spider)
            {
                DrawSpider(parEnemy);
            }
            else if (parEnemy is Golem)
            {
                DrawGolem(parEnemy);
            }
        }

        /// <summary>
        /// Отрисовывает скелета в буфере.
        /// </summary>
        /// <param name="parEnemy">Скелет для отрисовки.</param>
        private void DrawSkeleton(Enemy parEnemy)
        {
            (char symbol, int offsetY)[] skeletonParts =
            {
                ((char)0x0001, 0),
                ((char)0x00C5, 1),
                ('^', 2)
            };

            foreach (var (symbol, offsetY) in skeletonParts)
            {
                int y = parEnemy.Y + offsetY;
                int index = y * WIDTH + parEnemy.X;

                if (parEnemy.X >= 0 && parEnemy.X < WIDTH && y >= 0 && y < HEIGHT && index < _buffer.Length)
                {
                    _buffer[index].Char.UnicodeChar = symbol;
                    _buffer[index].Attributes = 0x0020;
                }
            }
        }

        /// <summary>
        /// Отрисовывает зомби в буфере.
        /// </summary>
        /// <param name="parEnemy">Зомби для отрисовки.</param>
        private void DrawZombie(Enemy parEnemy)
        {
            int topIndex = parEnemy.Y * WIDTH + parEnemy.X;
            int middleIndex = topIndex + WIDTH;
            int bottomIndex = middleIndex + WIDTH;

            if (parEnemy.X >= 0 && parEnemy.X < WIDTH && parEnemy.Y >= 0 && parEnemy.Y < HEIGHT && topIndex < _buffer.Length)
            {
                _buffer[topIndex].Char.UnicodeChar = (char)0x0003;
                _buffer[topIndex].Attributes = 0x0020;
            }

            if (parEnemy.X >= 0 && parEnemy.X < WIDTH && parEnemy.Y + 1 >= 0 && parEnemy.Y + 1 < HEIGHT && middleIndex < _buffer.Length)
            {
                _buffer[middleIndex].Char.UnicodeChar = (char)0x00BA;
                _buffer[middleIndex].Attributes = 0x0020;
            }

            if (parEnemy.X >= 0 && parEnemy.X < WIDTH && parEnemy.Y + 2 >= 0 && parEnemy.Y + 2 < HEIGHT && bottomIndex < _buffer.Length)
            {
                _buffer[bottomIndex].Char.UnicodeChar = '^';
                _buffer[bottomIndex].Attributes = 0x0020;
            }
        }

        /// <summary>
        /// Отрисовывает паука в буфере.
        /// </summary>
        /// <param name="parEnemy">Паук для отрисовки.</param>
        private void DrawSpider(Enemy parEnemy)
        {
            char[,] spider =
            {
                { (char)0x00BB, (char)0x00C9 },
                { (char)0x00BC, (char)0x00C8 }
            };

            for (int dy = 0; dy < 2; dy++)
            {
                for (int dx = 0; dx < 2; dx++)
                {
                    int x = parEnemy.X + dx;
                    int y = parEnemy.Y + dy;
                    int index = y * WIDTH + x;

                    if (x >= 0 && x < WIDTH && y >= 0 && y < HEIGHT && index < _buffer.Length)
                    {
                        _buffer[index].Char.UnicodeChar = spider[dy, dx];
                        _buffer[index].Attributes = 0x0020;
                    }
                }
            }
        }

        /// <summary>
        /// Отрисовывает голема в буфере.
        /// </summary>
        /// <param name="parEnemy">Голем для отрисовки.</param>
        private void DrawGolem(Enemy parEnemy)
        {
            char[,] golem =
                    {
                { (char)0x0000, (char)0x00DB, (char)0x0000 },
                { (char)0x00DB, (char)0x00DB, (char)0x00DB },
                { (char)0x00DB, (char)0x00DB, (char)0x00DB }
            };

            for (int dy = 0; dy < 3; dy++)
            {
                for (int dx = 0; dx < 3; dx++)
                {
                    if (golem[dy, dx] == '\0')
                    {
                        continue;
                    }

                    int x = parEnemy.X + dx;
                    int y = parEnemy.Y + dy;
                    int index = y * WIDTH + x;

                    if (x >= 0 && x < WIDTH && y >= 0 && y < HEIGHT && index < _buffer.Length)
                    {
                        _buffer[index].Char.UnicodeChar = golem[dy, dx];
                        _buffer[index].Attributes = 0x0020;
                    }
                }
            }
        }

        /// <summary>
        /// Отрисовывает камень в буфере.
        /// </summary>
        /// <param name="parRock">Камень для отрисовки.</param>
        private void DrawRock(Rock parRock)
        {
            if (parRock.X + 1 < 0 || parRock.X >= WIDTH || parRock.Y + 1 < 0 || parRock.Y >= HEIGHT)
            {
                return;
            }

            int topLeftIndex = parRock.Y * WIDTH + parRock.X;
            int topRightIndex = topLeftIndex + 1;
            int bottomLeftIndex = topLeftIndex + WIDTH;
            int bottomRightIndex = bottomLeftIndex + 1;

            if (parRock.X >= 0 && parRock.X < WIDTH && parRock.Y >= 0 && parRock.Y < HEIGHT && topLeftIndex < _buffer.Length)
            {
                _buffer[topLeftIndex].Char.UnicodeChar = ' ';
                _buffer[topLeftIndex].Attributes = 0x0010;
            }

            if (parRock.X + 1 >= 0 && parRock.X + 1 < WIDTH && parRock.Y >= 0 && parRock.Y < HEIGHT && topRightIndex < _buffer.Length)
            {
                _buffer[topRightIndex].Char.UnicodeChar = ' ';
                _buffer[topRightIndex].Attributes = 0x0010;
            }

            if (parRock.X >= 0 && parRock.X < WIDTH && parRock.Y + 1 >= 0 && parRock.Y + 1 < HEIGHT && bottomLeftIndex < _buffer.Length)
            {
                _buffer[bottomLeftIndex].Char.UnicodeChar = ' ';
                _buffer[bottomLeftIndex].Attributes = 0x0010;
            }

            if (parRock.X + 1 >= 0 && parRock.X + 1 < WIDTH && parRock.Y + 1 >= 0 && parRock.Y + 1 < HEIGHT && bottomRightIndex < _buffer.Length)
            {
                _buffer[bottomRightIndex].Char.UnicodeChar = ' ';
                _buffer[bottomRightIndex].Attributes = 0x0010;
            }
        }

        /// <summary>
        /// Отрисовывает путь к врагу.
        /// </summary>
        /// <param name="parEnemy">Враг.</param>
        /// <param name="parHero">Герой.</param>
        private void DrawPathToEnemy(Enemy parEnemy, Hero parHero)
        {
            int x0 = parEnemy.X;
            int y0 = parEnemy.Y;
            int x1 = parHero.X;
            int y1 = parHero.Y;

            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);
            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;
            int err = dx - dy;

            while (true)
            {
                int index = y0 * WIDTH + x0;

                if (index >= 0 && index < _buffer.Length)
                {
                    _buffer[index].Char.UnicodeChar = 'Z';
                    _buffer[index].Attributes = 0x0020;
                }

                if (x0 == x1 && y0 == y1)
                    break;

                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x0 += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }
        }

        /// <summary>
        /// Отрисовывает молнию.
        /// </summary>
        /// <param name="parEnemy">Враг, в которого ударила молния.</param>
        private void DrawThunder(Enemy parEnemy)
        {
            int x0 = parEnemy.X;
            int y0 = 0;

            int x1 = parEnemy.X;
            int y1 = parEnemy.Y;

            Random random = new Random();

            List<int> lightningPath = new List<int>();

            while (y0 < y1)
            {
                lightningPath.Add(y0 * WIDTH + x0);

                y0++;
            }

            foreach (int index in lightningPath)
            {
                if (index >= 0 && index < _buffer.Length)
                {
                    _buffer[index].Char.UnicodeChar = '|';
                    _buffer[index].Attributes = 0x000F;
                }
            }

            int finalIndex = y1 * WIDTH + x1;
            if (finalIndex >= 0 && finalIndex < _buffer.Length)
            {
                _buffer[finalIndex].Char.UnicodeChar = '|';
                _buffer[finalIndex].Attributes = 0x000F;
            }
        }

        /// <summary>
        /// Обработчик атаки кнутом.
        /// </summary>
        /// <param name="parEnemy">Враг, в которого ударили кнутом.</param>
        /// <param name="parHero">Герой, который произвел атаку.</param>
        public void OnWhipAttacked(Enemy parEnemy, Hero parHero)
        {
            Task.Run(() =>
            {
                DateTime startTime = DateTime.Now;

                while ((DateTime.Now - startTime).TotalSeconds < 0.1 && parHero.CurrentHealth > 0)
                {
                    lock (parEnemy)
                    {
                        DrawPathToEnemy(parEnemy, parHero);
                    }
                }
            });
        }

        /// <summary>
        /// Обработчик атаки молнией.
        /// </summary>
        /// <param name="parEnemy">Враг, в которого ударила молния.</param>
        /// <param name="parHero">Герой, который произвел атаку.</param>
        public void OnThunderAttacked(Enemy parEnemy, Hero parHero)
        {
            Task.Run(() =>
            {
                DateTime startTime = DateTime.Now;

                while ((DateTime.Now - startTime).TotalSeconds < 0.1 && parHero.CurrentHealth > 0)
                {
                    lock (parEnemy)
                    {
                        DrawThunder(parEnemy);
                    }
                }
            });
        }
    }
}