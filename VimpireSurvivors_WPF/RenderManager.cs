using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using Microsoft.Win32.SafeHandles;
using MvcController;
using System.Diagnostics;
using System.Windows.Threading;
using MvcModel.Frames;
using MvcModel;
using MvcModel.Сreatures.Enemies;
using MvcModel.Сreatures.Heros;
using MvcModel.Elements;
using MvcModel.Сreatures;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using VimpireSurvivors_WPF.Displayer;

namespace VimpireSurvivors_WPF
{
    /// <summary>
    /// Класс RenderManager отвечает за управление отрисовкой игровых элементов на экране.
    /// </summary>
    /// <remarks>
    /// Класс использует таймер для обновления визуального представления и поддерживает
    /// различные состояния игры, такие как главное меню, уровни, пауза и т.д.
    /// </remarks>
    public class RenderManager : FrameworkElement
    {
        private readonly DispatcherTimer renderTimer;

        private DrawingVisual pathVisual = new DrawingVisual();
        private DrawingVisual lightningVisual = new DrawingVisual();

        private int WIDTH;
        private int HEIGHT;

        /// <summary>
        /// Дескриптор вывода в консоль.
        /// </summary>
        public SafeFileHandle ConsoleOutput { get; set; }

        /// <summary>
        /// Контроллер, управляющий логикой игры.
        /// </summary>
        public Controller Controller { get; set; }

        /// <summary>
        /// Флаг, указывающий, выполняется ли в данный момент отрисовка.
        /// </summary>
        public bool IsRendering { get; set; }

        /// <summary>
        /// Текущий кадр, который должен быть отрисован.
        /// </summary>
        public Frame Frame { get; set; }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="RenderManager"/>.
        /// </summary>
        /// <remarks>
        /// Конструктор настраивает таймер для обновления отрисовки и подписывается на события
        /// изменения кадра, контроллера и атак.
        /// </remarks>
        public RenderManager()
        {
            renderTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(16)
            };
            renderTimer.Tick += (sender, args) => InvalidateVisual();

            WIDTH = GameWindow.GetInstance().Width;
            HEIGHT = GameWindow.GetInstance().Height;
            Controller.FrameChanged += OnFrameChanged;
            KeyListener.ControllerChanged += OnControllerChanged;
            LevelController.WhipAttacked += OnWhipAttacked;
            LevelController.ThunderAttacked += OnThunderAttacked;

            AddVisualChild(pathVisual);
            AddLogicalChild(pathVisual);

            AddVisualChild(lightningVisual);
            AddLogicalChild(lightningVisual);
        }

        /// <summary>
        /// Возвращает визуальный элемент по указанному индексу.
        /// </summary>
        /// <param name="parIndex">Индекс визуального элемента.</param>
        /// <returns>Визуальный элемент.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Выбрасывается, если индекс выходит за пределы допустимого диапазона.</exception>
        protected override Visual GetVisualChild(int parIndex)
        {
            if (parIndex == 0) return pathVisual;
            if (parIndex == 1) return lightningVisual;
            throw new ArgumentOutOfRangeException();
        }

        /// <summary>
        /// Возвращает количество визуальных элементов.
        /// </summary>
        protected override int VisualChildrenCount => 2;

        /// <summary>
        /// Запускает процесс отрисовки.
        /// </summary>
        public void StartRender()
        {
            renderTimer.Start();
            IsRendering = true;
        }

        /// <summary>
        /// Останавливает процесс отрисовки.
        /// </summary>
        public void StopRender()
        {
            renderTimer.Stop();
        }

        /// <summary>
        /// Обрабатывает изменение текущего кадра.
        /// </summary>
        /// <param name="parFrame">Новый кадр.</param>
        public void OnFrameChanged(Frame parFrame)
        {
            Frame = parFrame;
        }

        /// <summary>
        /// Обрабатывает изменение контроллера.
        /// </summary>
        /// <param name="parController">Новый контроллер.</param>
        public void OnControllerChanged(Controller parController)
        {
            Controller = parController;
        }

        /// <summary>
        /// Выполняет отрисовку элементов на экране.
        /// </summary>
        /// <param name="parDrawingContext">Контекст отрисовки.</param>
        protected override void OnRender(DrawingContext parDrawingContext)
        {
            base.OnRender(parDrawingContext);

            parDrawingContext.DrawRectangle(new SolidColorBrush(Colors.Black), null, new Rect(0, 0, GameWindow.GetInstance().Width, GameWindow.GetInstance().Height));

            if (Controller.Frame is MainMenuFrame mainMenu)
            {
                RenderMainMenu(parDrawingContext, mainMenu);
            }
            else if (Controller.Frame is LevelsMenuFrame levelsMenu)
            {
                RenderLevelsMenu(parDrawingContext, levelsMenu);
            }
            else if (Controller.Frame is NameInputFrame nameInput)
            {
                RenderNameInput(parDrawingContext, nameInput);
            }
            else if (Controller.Frame is RulesFrame rulesFrame)
            {
                RenderRulesFrame(parDrawingContext, rulesFrame);
            }
            else if (Controller.Frame is RecordsFrame recordsFrame)
            {
                RenderRecordsFrame(parDrawingContext, recordsFrame);
            }
            else if (Controller.Frame is LevelFrame level)
            {
                RenderFrame(parDrawingContext, level);
            }
            else if (Controller.Frame is GameOverFrame gameOverFrame)
            {
                RenderGameOverFrame(parDrawingContext, gameOverFrame);
            }
            else if (Controller.Frame is PauseFrame pauseFrame)
            {
                RenderPauseFrame(parDrawingContext, pauseFrame);
            }
            else if (Controller.Frame is UpgradeFrame upgradeFrame)
            {
                RenderUpgradeFrame(parDrawingContext, upgradeFrame);
            }
        }

        /// <summary>
        /// Отрисовывает главное меню.
        /// </summary>
        /// <param name="parDrawingContext">Контекст отрисовки.</param>
        /// <param name="parMainMenu">Кадр главного меню.</param>
        public void RenderMainMenu(DrawingContext parDrawingContext, MainMenuFrame parMainMenu)
        {
            foreach (Button button in parMainMenu.Buttons)
            {
                ElementDisplayer.DrawButton(parDrawingContext, button);
            }

            foreach (Label label in parMainMenu.Labels)
            {
                ElementDisplayer.DrawLabel(parDrawingContext, label);
            }
        }

        /// <summary>
        /// Отрисовывает меню уровней.
        /// </summary>
        /// <param name="parDrawingContext">Контекст отрисовки.</param>
        /// <param name="parLevelsMenu">Кадр меню уровней.</param>
        public void RenderLevelsMenu(DrawingContext parDrawingContext, LevelsMenuFrame parLevelsMenu)
        {
            foreach (Button button in parLevelsMenu.Buttons)
            {
                ElementDisplayer.DrawButton(parDrawingContext, button);
            }
        }

        /// <summary>
        /// Отрисовывает поле ввода имени.
        /// </summary>
        /// <param name="parDrawingContext">Контекст отрисовки.</param>
        /// <param name="parNameInput">Кадр ввода имени.</param>
        public void RenderNameInput(DrawingContext parDrawingContext, NameInputFrame parNameInput)
        {
            ElementDisplayer.DrawTextBox(parDrawingContext, parNameInput.Name);

            if (parNameInput.IsErrorMessaheActive)
            {
                ElementDisplayer.DrawLabel(parDrawingContext, parNameInput.Labels[0]);
            }
            ElementDisplayer.DrawLabel(parDrawingContext, parNameInput.Labels[1]);
        }

        /// <summary>
        /// Отрисовывает кадр с правилами игры.
        /// </summary>
        /// <param name="parDrawingContext">Контекст отрисовки.</param>
        /// <param name="parRulesFrame">Кадр с правилами.</param>
        public void RenderRulesFrame(DrawingContext parDrawingContext, RulesFrame parRulesFrame)
        {
            foreach (Button button in parRulesFrame.Buttons)
            {
                ElementDisplayer.DrawButton(parDrawingContext, button);
            }

            foreach (Label label in parRulesFrame.Labels)
            {
                ElementDisplayer.DrawLabel(parDrawingContext, label, true);
            }
        }

        /// <summary>
        /// Отрисовывает кадр с рекордами.
        /// </summary>
        /// <param name="parDrawingContext">Контекст отрисовки.</param>
        /// <param name="parRecordsFrame">Кадр с рекордами.</param>
        public void RenderRecordsFrame(DrawingContext parDrawingContext, RecordsFrame parRecordsFrame)
        {
            foreach (Button button in parRecordsFrame.Buttons)
            {
                ElementDisplayer.DrawButton(parDrawingContext, button);
            }

            foreach (Label label in parRecordsFrame.Labels)
            {
                ElementDisplayer.DrawLabel(parDrawingContext, label, true);
            }
        }

        /// <summary>
        /// Отрисовывает кадр завершения игры.
        /// </summary>
        /// <param name="parDrawingContext">Контекст отрисовки.</param>
        /// <param name="parGameOverFrame">Кадр завершения игры.</param>
        private void RenderGameOverFrame(DrawingContext parDrawingContext, GameOverFrame parGameOverFrame)
        {
            foreach (Form form in parGameOverFrame.Forms)
            {
                ElementDisplayer.DrawForm(parDrawingContext, form);
            }

            foreach (Button button in parGameOverFrame.Buttons)
            {
                ElementDisplayer.DrawButton(parDrawingContext, button);
            }

            foreach (Label label in parGameOverFrame.Labels)
            {
                ElementDisplayer.DrawLabel(parDrawingContext, label);
            }
        }

        /// <summary>
        /// Отрисовывает кадр паузы.
        /// </summary>
        /// <param name="parDrawingContext">Контекст отрисовки.</param>
        /// <param name="parPauseFrame">Кадр паузы.</param>
        private void RenderPauseFrame(DrawingContext parDrawingContext, PauseFrame parPauseFrame)
        {
            foreach (Form form in parPauseFrame.Forms)
            {
                ElementDisplayer.DrawForm(parDrawingContext, form);
            }

            foreach (Button button in parPauseFrame.Buttons)
            {
                ElementDisplayer.DrawButton(parDrawingContext, button);
            }

            foreach (Label label in parPauseFrame.Labels)
            {
                ElementDisplayer.DrawLabel(parDrawingContext, label);
            }
        }

        /// <summary>
        /// Отрисовывает игровой уровень.
        /// </summary>
        /// <param name="parDrawingContext">Контекст отрисовки.</param>
        /// <param name="parLevel">Кадр уровня.</param>
        private void RenderFrame(DrawingContext parDrawingContext, LevelFrame parLevel)
        {
            parDrawingContext.DrawRectangle(new SolidColorBrush(Colors.Green), null, new Rect(0, 0, GameWindow.GetInstance().Width, GameWindow.GetInstance().Height));

            foreach (Rock rock in parLevel.Rocks)
            {
                DrawRock(parDrawingContext, rock);
            }

            lock (parLevel)
            {
                parLevel.Enemies.ForEach(item =>
                {
                    DrawEnemy(parDrawingContext, item);
                });
            }

            DrawPlayer(parDrawingContext, parLevel.Player);

            DrawHeroLevelBar(parDrawingContext, parLevel.Player);

            DrawHeroHealthBar(parDrawingContext, parLevel.Player);

            DrawTimer(parDrawingContext, parLevel);
        }

        /// <summary>
        /// Отрисовывает кадр улучшений.
        /// </summary>
        /// <param name="parDrawingContext">Контекст отрисовки.</param>
        /// <param name="parUpgradeFrame">Кадр улучшений.</param>
        private void RenderUpgradeFrame(DrawingContext parDrawingContext, UpgradeFrame parUpgradeFrame)
        {
            foreach (Form form in parUpgradeFrame.Forms)
            {
                ElementDisplayer.DrawForm(parDrawingContext, form);
            }

            foreach (Button button in parUpgradeFrame.Buttons)
            {
                ElementDisplayer.DrawButton(parDrawingContext, button);
                Label descriptionLabel = new Label(button.Discription, new System.Drawing.Point(button.Coordinates.X, button.Coordinates.Y + (int)(button.Height * 1.1)), button.Height, button.Width);
                descriptionLabel.TextColor = ConsoleColor.Blue;
                descriptionLabel.BackgroundColor = ConsoleColor.DarkGray;
                ElementDisplayer.DrawLabel(parDrawingContext, descriptionLabel);
            }

            foreach (Label label in parUpgradeFrame.Labels)
            {
                ElementDisplayer.DrawLabel(parDrawingContext, label, true);
            }
        }

        /// <summary>
        /// Отрисовывает камень на игровом поле.
        /// </summary>
        /// <param name="parDrawingContext">Контекст отрисовки.</param>
        /// <param name="parRock">Камень для отрисовки.</param>
        private void DrawRock(DrawingContext parDrawingContext, Rock parRock)
        {
            BitmapImage sprite = new BitmapImage(new Uri("C:\\Users\\Higashi\\Desktop\\Radick\\C4S1\\КурсоваяКПО\\VimpireSurvivors\\VimpireSurvivors_WPF\\png\\grass.png"));

            double x = parRock.X;
            double y = parRock.Y;
            double width = 20;
            double height = 20;

            Rect drawingRect = new Rect(x, y, width, height);

            parDrawingContext.DrawImage(sprite, drawingRect);
        }

        /// <summary>
        /// Отрисовывает врага на игровом поле.
        /// </summary>
        /// <param name="parDrawingContext">Контекст отрисовки.</param>
        /// <param name="parEnemy">Враг для отрисовки.</param>
        private void DrawEnemy(DrawingContext parDrawingContext, Enemy parEnemy)
        {
            if (parEnemy is Skeleton)
            {
                DrawSkeleton(parDrawingContext, parEnemy);
            }
            else if (parEnemy is Zombie)
            {
                DrawZombie(parDrawingContext, parEnemy);
            }
            else if (parEnemy is Spider)
            {
                DrawSpider(parDrawingContext, parEnemy);
            }
            else if (parEnemy is Golem)
            {
                DrawGolem(parDrawingContext, parEnemy);
            }
        }

        /// <summary>
        /// Отрисовывает скелета на игровом поле.
        /// </summary>
        /// <param name="parDrawingContext">Контекст отрисовки.</param>
        /// <param name="parEnemy">Скелет для отрисовки.</param>
        private void DrawSkeleton(DrawingContext parDrawingContext, Enemy parEnemy)
        {
            BitmapImage sprite = new BitmapImage(new Uri("C:\\Users\\Higashi\\Desktop\\Radick\\C4S1\\КурсоваяКПО\\VimpireSurvivors\\VimpireSurvivors_WPF\\png\\skeleton.png"));

            double x = parEnemy.X;
            double y = parEnemy.Y;
            double width = parEnemy.Width;
            double height = parEnemy.Height;

            Rect drawingRect = new Rect(x, y, width, height);

            parDrawingContext.DrawImage(sprite, drawingRect);
        }

        /// <summary>
        /// Отрисовывает зомби на игровом поле.
        /// </summary>
        /// <param name="parDrawingContext">Контекст отрисовки.</param>
        /// <param name="parEnemy">Зомби для отрисовки.</param>
        private void DrawZombie(DrawingContext parDrawingContext, Enemy parEnemy)
        {
            BitmapImage sprite = new BitmapImage(new Uri("C:\\Users\\Higashi\\Desktop\\Radick\\C4S1\\КурсоваяКПО\\VimpireSurvivors\\VimpireSurvivors_WPF\\png\\zombie.png"));

            double x = parEnemy.X;
            double y = parEnemy.Y;
            double width = parEnemy.Width;
            double height = parEnemy.Height;

            Rect drawingRect = new Rect(x, y, width, height);

            parDrawingContext.DrawImage(sprite, drawingRect);
        }

        /// <summary>
        /// Отрисовывает паука на игровом поле.
        /// </summary>
        /// <param name="parDrawingContext">Контекст отрисовки.</param>
        /// <param name="parEnemy">Паук для отрисовки.</param>
        private void DrawSpider(DrawingContext parDrawingContext, Enemy parEnemy)
        {
            BitmapImage sprite = new BitmapImage(new Uri("C:\\Users\\Higashi\\Desktop\\Radick\\C4S1\\КурсоваяКПО\\VimpireSurvivors\\VimpireSurvivors_WPF\\png\\spider.png"));

            double x = parEnemy.X;
            double y = parEnemy.Y;
            double width = parEnemy.Width;
            double height = parEnemy.Height;

            Rect drawingRect = new Rect(x, y, width, height);

            parDrawingContext.DrawImage(sprite, drawingRect);
        }


        /// <summary>
        /// Отрисовывает голема на игровом поле.
        /// </summary>
        /// <param name="parDrawingContext">Контекст отрисовки.</param>
        /// <param name="parEnemy">Голем для отрисовки.</param>
        private void DrawGolem(DrawingContext parDrawingContext, Enemy parEnemy)
        {
            BitmapImage sprite = new BitmapImage(new Uri("C:\\Users\\Higashi\\Desktop\\Radick\\C4S1\\КурсоваяКПО\\VimpireSurvivors\\VimpireSurvivors_WPF\\png\\golem.png"));

            double x = parEnemy.X;
            double y = parEnemy.Y;
            double width = parEnemy.Width;
            double height = parEnemy.Height;

            Rect drawingRect = new Rect(x, y, width, height);

            parDrawingContext.DrawImage(sprite, drawingRect);
        }

        /// <summary>
        /// Отрисовывает игрока на игровом поле.
        /// </summary>
        /// <param name="parDrawingContext">Контекст отрисовки.</param>
        /// <param name="parPlayer">Игрок для отрисовки.</param>
        private void DrawPlayer(DrawingContext parDrawingContext, Hero parPlayer)
        {
            BitmapImage sprite = new BitmapImage(new Uri("C:\\Users\\Higashi\\Desktop\\Radick\\C4S1\\КурсоваяКПО\\VimpireSurvivors\\VimpireSurvivors_WPF\\png\\hero.png"));

            double x = parPlayer.X;
            double y = parPlayer.Y;
            double width = parPlayer.Width;
            double height = parPlayer.Height;

            Rect drawingRect = new Rect(x, y, width, height);

            parDrawingContext.DrawImage(sprite, drawingRect);
        }

        /// <summary>
        /// Отрисовывает шкалу здоровья игрока.
        /// </summary>
        /// <param name="parDrawingContext">Контекст отрисовки.</param>
        /// <param name="parHero">Игрок, для которого отрисовывается шкала здоровья.</param>
        private void DrawHeroHealthBar(DrawingContext parDrawingContext, Hero parHero)
        {
            if (GameWindow.GetInstance().Height <= 0 || GameWindow.GetInstance().Width <= 0 || parHero.MaxHealth <= 0)
            {
                return;
            }

            double barHeight = Math.Max(1, GameWindow.GetInstance().Height * 0.05);
            double barWidth = Math.Max(1, GameWindow.GetInstance().Width * 0.15);  
            double xStart = GameWindow.GetInstance().Width * 0.05;
            double yStart = GameWindow.GetInstance().Height * 0.8;
            double padding = 5;

            int currentHealth = Math.Max(0, parHero.CurrentHealth);
            double filledWidth = (double)currentHealth / parHero.MaxHealth * (barWidth - 2 * padding);

            filledWidth = Math.Max(0, filledWidth);

            string healthText = "HP";
            var dpiScale = VisualTreeHelper.GetDpi(Application.Current.MainWindow);
            FormattedText formattedText = new FormattedText(
                healthText,
                System.Globalization.CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight,
                new Typeface("Arial"),
                16,
                Brushes.White,
                dpiScale.PixelsPerDip
            );

            parDrawingContext.DrawText(formattedText, new Point(xStart, yStart + formattedText.Height));

            Rect backgroundRect = new Rect(xStart + formattedText.Width + 10, yStart, barWidth, barHeight);
            parDrawingContext.DrawRectangle(Brushes.Gray, null, backgroundRect);

            Rect filledRect = new Rect(
                backgroundRect.X + padding,
                backgroundRect.Y + padding,
                filledWidth,
                Math.Max(1, barHeight - 2 * padding)
            );
            parDrawingContext.DrawRectangle(Brushes.Red, null, filledRect);

            FormattedText healthTextInBar = new FormattedText(
                $"{currentHealth}/{parHero.MaxHealth}",
                System.Globalization.CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight,
                new Typeface("Arial"),
                14,
                Brushes.White,
                dpiScale.PixelsPerDip
            );

            double textX = backgroundRect.X + (barWidth - healthTextInBar.Width) / 2;
            double textY = backgroundRect.Y + (barHeight - healthTextInBar.Height) / 2;

            parDrawingContext.DrawText(healthTextInBar, new Point(textX, textY));
        }


        /// <summary>
        /// Отрисовывает шкалу уровня игрока.
        /// </summary>
        /// <param name="parDrawingContext">Контекст отрисовки.</param>
        /// <param name="parHero">Игрок, для которого отрисовывается шкала уровня.</param>
        private void DrawHeroLevelBar(DrawingContext parDrawingContext, Hero parHero)
        {
            double barWidth = GameWindow.GetInstance().Width * 0.4;
            double barHeight = GameWindow.GetInstance().Height * 0.05;
            double startX = (GameWindow.GetInstance().Width - barWidth) / 2;
            double startY = GameWindow.GetInstance().Height * 0.02;

            double filledWidth = parHero.Experience / (double)parHero.NextLevelExperience * barWidth;

            string levelText = $"{parHero.Level} ({parHero.Experience}/{parHero.NextLevelExperience})";

            Typeface typeface = new Typeface("Arial");
            FormattedText formattedText = new FormattedText(
                levelText,
                System.Globalization.CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight,
                typeface,
                barHeight * 0.7,
                Brushes.White,
                new NumberSubstitution(),
                1);

            parDrawingContext.DrawRectangle(null, new Pen(Brushes.White, 2), new Rect(startX, startY, barWidth, barHeight));

            parDrawingContext.DrawRectangle(Brushes.Red, null, new Rect(startX, startY, filledWidth, barHeight));

            double textX = startX + (barWidth - formattedText.Width) / 2;
            double textY = startY + (barHeight - formattedText.Height) / 2;

            parDrawingContext.DrawText(formattedText, new Point(textX, textY));
        }

        /// <summary>
        /// Отрисовывает таймер на игровом поле.
        /// </summary>
        /// <param name="parDrawingContext">Контекст отрисовки.</param>
        /// <param name="parLevel">Кадр уровня, для которого отрисовывается таймер.</param>
        private void DrawTimer(DrawingContext parDrawingContext, LevelFrame parLevel)
        {
            if (Controller is LevelController levelController)
            {
                string time = ConvertSecondsToTimeFormat(levelController.GetTimerSeconds());

                Typeface typeface = new Typeface("Arial");
                Brush textBrush = Brushes.White;
                double fontSize = 20;

                FormattedText formattedText = new FormattedText(
                    time,
                    System.Globalization.CultureInfo.InvariantCulture,
                    FlowDirection.LeftToRight,
                    typeface,
                    fontSize,
                    textBrush,
                    1.25
                );

                double centerX = GameWindow.GetInstance().Width / 2 - formattedText.Width / 2;
                double centerY = GameWindow.GetInstance().Height * 0.85 - formattedText.Height;

                parDrawingContext.DrawText(formattedText, new Point(centerX, centerY));
            }
        }

        /// <summary>
        /// Преобразует секунды в формат времени "мм:сс".
        /// </summary>
        /// <param name="parTotalSeconds">Общее количество секунд.</param>
        /// <returns>Строка, представляющая время в формате "мм:сс".</returns>
        private string ConvertSecondsToTimeFormat(int parTotalSeconds)
        {
            int minutes = parTotalSeconds / 60;
            int seconds = parTotalSeconds % 60;
            return $"{minutes:D2}:{seconds:D2}";
        }

        /// <summary>
        /// Отрисовывает путь к врагу.
        /// </summary>
        /// <param name="parDrawingContext">Контекст отрисовки.</param>
        /// <param name="parEnemy">Враг, к которому строится путь.</param>
        /// <param name="parHero">Игрок, от которого строится путь.</param>
        private void DrawPathToEnemy(DrawingContext parDrawingContext, Enemy parEnemy, Hero parHero)
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
                parDrawingContext.DrawRectangle(Brushes.Brown, null, new Rect(x0, y0, 2, 2));

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
        /// Отрисовывает молнию на игровом поле.
        /// </summary>
        /// <param name="parDrawingContext">Контекст отрисовки.</param>
        /// <param name="parEnemy">Враг, на которого направлена молния.</param>
        private void DrawThunder(DrawingContext parDrawingContext, Enemy parEnemy)
        {
            int x = parEnemy.X;
            int y0 = 0;
            int y1 = parEnemy.Y;

            parDrawingContext.DrawLine(new Pen(Brushes.White, 2), new Point(x, y0), new Point(x, y1));
        }

        /// <summary>
        /// Обрабатывает атаку кнутом.
        /// </summary>
        /// <param name="parEnemy">Враг, на которого направлена атака.</param>
        /// <param name="parHero">Игрок, выполняющий атаку.</param>
        public void OnWhipAttacked(Enemy parEnemy, Hero parHero)
        {
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(50)
            };
            DateTime startTime = DateTime.Now;

            timer.Tick += (sender, e) =>
            {
                if ((DateTime.Now - startTime).TotalSeconds >= 0.1 || parHero.CurrentHealth <= 0)
                {
                    timer.Stop();
                    using (DrawingContext drawingContext = pathVisual.RenderOpen())
                    {
                        drawingContext.DrawRectangle(Brushes.Transparent, null, new Rect(0, 0, WIDTH, HEIGHT));
                    }
                    return;
                }

                using (DrawingContext drawingContext = pathVisual.RenderOpen())
                {
                    drawingContext.DrawRectangle(Brushes.Transparent, null, new Rect(0, 0, WIDTH, HEIGHT));
                    DrawPathToEnemy(drawingContext, parEnemy, parHero);
                }
            };
            timer.Start();
        }

        /// <summary>
        /// Обрабатывает атаку молнией.
        /// </summary>
        /// <param name="parEnemy">Враг, на которого направлена атака.</param>
        /// <param name="parHero">Игрок, выполняющий атаку.</param>
        public void OnThunderAttacked(Enemy parEnemy, Hero parHero)
        {
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(50)
            };
            DateTime startTime = DateTime.Now;

            timer.Tick += (sender, e) =>
            {
                if ((DateTime.Now - startTime).TotalSeconds >= 0.1 || parHero.CurrentHealth <= 0)
                {
                    timer.Stop();
                    using (DrawingContext drawingContext = pathVisual.RenderOpen())
                    {
                        drawingContext.DrawRectangle(Brushes.Transparent, null, new Rect(0, 0, WIDTH, HEIGHT));
                    }
                    return;
                }

                using (DrawingContext drawingContext = pathVisual.RenderOpen())
                {
                    drawingContext.DrawRectangle(Brushes.Transparent, null, new Rect(0, 0, WIDTH, HEIGHT));
                    DrawThunder(drawingContext, parEnemy);
                }
            };

            timer.Start();
        }
    }
}