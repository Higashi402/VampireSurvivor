using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using MvcModel.Frames;
using MvcController.Tools;
using MvcController;
using MvcModel.Сreatures.Heros;
using MvcModel.Upgrades;
using MvcModel.Elements;
using MvcModel.Json;

namespace VimpireSurvivors_Console
{
    /// <summary>
    /// Класс для отслеживания нажатий клавиш и управления игровыми событиями.
    /// </summary>
    /// <remarks>
    /// Класс отвечает за обработку нажатий клавиш, управление контроллерами и переключение между различными фреймами игры.
    /// </remarks>
    public class KeyListener
    {
        /// <summary>
        /// Задержка между проверками нажатий клавиш (в миллисекундах).
        /// </summary>
        private const long _DELAY = 50;

        /// <summary>
        /// Буферизованный фрейм, используемый для временного хранения состояния игры.
        /// </summary>
        private Frame _bufferedFrame { get; set; }

        /// <summary>
        /// Задача для асинхронного отслеживания нажатий клавиш.
        /// </summary>
        public Task KeyListenerTask { get; set; }

        /// <summary>
        /// Флаг, указывающий, активно ли отслеживание нажатий клавиш.
        /// </summary>
        public bool IsListening { get; set; }

        /// <summary>
        /// Время последнего отслеживания нажатий клавиш.
        /// </summary>
        public long LastListeningTime { get; set; }

        /// <summary>
        /// Время последнего ввода с клавиатуры.
        /// </summary>
        public long LastInputTime { get; set; }

        /// <summary>
        /// Текущее время.
        /// </summary>
        public long CurrentTime { get; set; }

        /// <summary>
        /// Разница во времени между текущим и последним отслеживанием.
        /// </summary>
        public long Delta { get; set; }

        /// <summary>
        /// Текущий контроллер, управляющий игровым процессом.
        /// </summary>
        public Controller Controller { get; set; }

        /// <summary>
        /// Событие, возникающее при изменении контроллера.
        /// </summary>
        public static event Action<Controller> ControllerChanged;

        /// <summary>
        /// Конструктор класса <see cref="KeyListener"/>.
        /// </summary>
        /// <param name="parController">Контроллер, который будет управляться слушателем клавиш.</param>
        /// <remarks>
        /// Инициализирует новый экземпляр класса с указанным контроллером.
        /// </remarks>
        public KeyListener(Controller parController)
        {
            Controller = parController;
        }

        /// <summary>
        /// Импортирует функцию GetAsyncKeyState из user32.dll для проверки состояния клавиши.
        /// </summary>
        /// <param name="parKey">Код клавиши.</param>
        /// <returns>Состояние клавиши.</returns>
        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(int parKey);

        /// <summary>
        /// Импортирует функцию FreeConsole из kernel32.dll для освобождения консоли.
        /// </summary>
        /// <returns>Результат выполнения функции.</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool FreeConsole();

        /// <summary>
        /// Проверяет, нажата ли указанная клавиша в данный момент.
        /// </summary>
        /// <param name="parKey">Код клавиши.</param>
        /// <returns><c>true</c>, если клавиша нажата; иначе <c>false</c>.</returns>
        public static bool IsKeyPressed(int parKey)
        {
            short keyState = GetAsyncKeyState(parKey);
            return (keyState & 0x8000) != 0;
        }

        /// <summary>
        /// Проверяет, была ли клавиша нажата с момента последнего вызова.
        /// </summary>
        /// <param name="parKey">Код клавиши.</param>
        /// <returns><c>true</c>, если клавиша была нажата; иначе <c>false</c>.</returns>
        public static bool WasKeyPressed(int parKey)
        {
            short keyState = GetAsyncKeyState(parKey);
            return (keyState & 0x0001) != 0;
        }

        /// <summary>
        /// Запускает отслеживание нажатий клавиш.
        /// </summary>
        /// <remarks>
        /// Метод подписывается на события смерти и улучшения героя, а затем начинает отслеживать нажатия клавиш.
        /// </remarks>
        public void StartKeyListener()
        {
            LevelController.HeroDied += OnHeroDied;
            LevelController.HeroUpgraded += OnHeroUpgraded;
            IsListening = true;
            ListenKeys();
        }

        /// <summary>
        /// Основной цикл отслеживания нажатий клавиш.
        /// </summary>
        /// <remarks>
        /// Метод проверяет состояние клавиш и выполняет соответствующие действия в зависимости от текущего контроллера.
        /// </remarks>
        private void ListenKeys()
        {
            LastListeningTime = GameTimer.GetCurrentTime();

            while (IsListening)
            {
                CurrentTime = GameTimer.GetCurrentTime();
                Delta = CurrentTime - LastListeningTime;

                if (Delta > _DELAY)
                {
                    switch (Controller)
                    {
                        case DialogFrameController dialogFrameController:
                            ListenDialogFrameController(dialogFrameController);
                            break;
                        case LevelController levelController:
                            ListenLevelController(levelController);
                            break;
                        default:
                            break;
                    }

                    LastListeningTime = GameTimer.GetCurrentTime();
                }
            }
        }

        /// <summary>
        /// Обрабатывает событие смерти героя.
        /// </summary>
        /// <param name="parHero">Герой, который умер.</param>
        /// <remarks>
        /// Метод сохраняет результат игры и переключает контроллер на экран завершения игры.
        /// </remarks>
        private void OnHeroDied(Hero parHero)
        {
            if (Controller is LevelController lc)
            {
                int time = lc.GetTimerSeconds();
                string name = "";
                if (lc.Frame is LevelFrame level)
                {
                    name = level.Player.Name;
                }
                JsonRecord jo = new JsonRecord(name, time);
                JsonWriter.AddJsonRecord(jo);
            }
            Controller.Frame = new GameOverFrame();
            Controller = new DialogFrameController((DialogFrame)Controller.Frame);
        }

        /// <summary>
        /// Обрабатывает событие улучшения героя.
        /// </summary>
        /// <param name="parHero">Герой, который был улучшен.</param>
        /// <remarks>
        /// Метод переключает контроллер на экран улучшений.
        /// </remarks>
        private void OnHeroUpgraded(Hero parHero)
        {
            if(!(Controller.Frame is LevelFrame))
            {
                return;
            }
            _bufferedFrame = Controller.Frame;
            if (Controller is LevelController lc)
            {
                LevelController.GameTimer.Pause();
            }
            Controller.Frame = new UpgradeFrame(parHero);
            Controller = new DialogFrameController((UpgradeFrame)Controller.Frame);
        }

        private void ListenDialogFrameController(DialogFrameController dialogFrameController)
        {
            if (dialogFrameController.Frame is MainMenuFrame mainMenuFrame)
            {
                ListenMainMenu(dialogFrameController, mainMenuFrame);
            }
            else if (dialogFrameController.Frame is LevelsMenuFrame levelsMenuFrame)
            {
                ListenLevelsMenu(dialogFrameController, levelsMenuFrame);
            }
            else if (dialogFrameController.Frame is GameOverFrame gameOverFrame)
            {
                ListenGameOverFrame(dialogFrameController, gameOverFrame);
            }
            else if (dialogFrameController.Frame is UpgradeFrame upgradeFrame)
            {
                ListenUpgradeFrame(dialogFrameController, upgradeFrame);
            }
            else if (dialogFrameController.Frame is NameInputFrame nameInput)
            {
                ListenNameInputFrame(dialogFrameController,nameInput);
            }
            else if (dialogFrameController.Frame is RecordsFrame recordsFrame)
            {
                ListenRecordsFrame(dialogFrameController, recordsFrame);
            }
            else if (dialogFrameController.Frame is RulesFrame rulesFrame)
            {
                if (WasKeyPressed((int)MyKey.ENTER))
                {
                    Controller = new DialogFrameController(new MainMenuFrame());
                    ControllerChanged?.Invoke(Controller);
                }
            }
            else if (dialogFrameController.Frame is PauseFrame pauseFrame)
            {
                ListenPauseFrame(dialogFrameController, pauseFrame);
            }
        }

        private void ListenMainMenu(DialogFrameController dialogFrameController,MainMenuFrame mainMenuFrame)
        {
            if (WasKeyPressed((int)MyKey.S))
            {
                dialogFrameController.ChangeActiveButton((int)MyKey.S);
            }
            else if (WasKeyPressed((int)MyKey.W))
            {
                dialogFrameController.ChangeActiveButton((int)MyKey.W);
            }
            else if (WasKeyPressed((int)MyKey.ENTER))
            {
                if (mainMenuFrame._activeButtonIndex == 4)
                {
                    IsListening = false;
                    dialogFrameController.ExitGame();
                }
                else if (mainMenuFrame._activeButtonIndex == 1)
                {
                    Controller = new DialogFrameController(new LevelsMenuFrame());
                    ControllerChanged?.Invoke(Controller);
                    _bufferedFrame = null;
                }
                else if (mainMenuFrame._activeButtonIndex == 2)
                {
                    List<JsonRecord> records = JsonWriter.ReadJsonRecordList()
                        .OrderByDescending(record => record.Time)
                        .ToList();

                    if (records.Count > 10)
                    {
                        records.RemoveRange(10, records.Count - 10);
                    }
                    RecordsFrame recordsFrame = new RecordsFrame(records);

                    Controller = new DialogFrameController(recordsFrame);
                    ControllerChanged?.Invoke(Controller);
                }
                else if (mainMenuFrame._activeButtonIndex == 3)
                {
                    Controller = new DialogFrameController(new RulesFrame());
                    ControllerChanged?.Invoke(Controller);
                }
            }
        }

        private void ListenLevelsMenu(DialogFrameController dialogFrameController, LevelsMenuFrame levelsMenuFrame)
        {
            if (WasKeyPressed((int)MyKey.S))
            {
                dialogFrameController.ChangeActiveButton((int)MyKey.S);
            }
            else if (WasKeyPressed((int)MyKey.W))
            {
                dialogFrameController.ChangeActiveButton((int)MyKey.W);
            }
            else if (WasKeyPressed((int)MyKey.ENTER))
            {
                if (levelsMenuFrame._activeButtonIndex == 4)
                {
                    Controller = new DialogFrameController(new MainMenuFrame());
                    ControllerChanged?.Invoke(Controller);
                }
                else if (levelsMenuFrame._activeButtonIndex < 4)
                {
                    NameInputFrame nameInput = new NameInputFrame();
                    Controller = new DialogFrameController(nameInput);
                    ControllerChanged?.Invoke(Controller);
                }
            }
        }

        private void ListenGameOverFrame(DialogFrameController dialogFrameController, GameOverFrame gameOverFrame)
        {
            if (WasKeyPressed((int)MyKey.A))
            {
                dialogFrameController.ChangeActiveButton((int)MyKey.A);
            }
            else if (WasKeyPressed((int)MyKey.D))
            {
                dialogFrameController.ChangeActiveButton((int)MyKey.D);
            }
            else if (WasKeyPressed((int)MyKey.ENTER))
            {
                if (gameOverFrame._activeButtonIndex == 1)
                {
                    LevelController.Clear();
                    Controller = new DialogFrameController(new MainMenuFrame());
                    ControllerChanged?.Invoke(Controller);
                }
            }
        }

        private void ListenUpgradeFrame(DialogFrameController dialogFrameController, UpgradeFrame upgradeFrame)
        {
            if (WasKeyPressed((int)MyKey.A))
            {
                dialogFrameController.ChangeActiveButton((int)MyKey.A);
            }
            else if (WasKeyPressed((int)MyKey.D))
            {
                dialogFrameController.ChangeActiveButton((int)MyKey.D);
            }
            else if (WasKeyPressed((int)MyKey.ENTER))
            {
                UpgradeType upgradeType;
                if (upgradeFrame._activeButtonIndex == 1)
                {
                    upgradeType = upgradeFrame.Buttons[0].InternalUpgrade;
                }
                else if (upgradeFrame._activeButtonIndex == 2)
                {
                    upgradeType = upgradeFrame.Buttons[1].InternalUpgrade;
                }
                else
                {
                    upgradeType = upgradeFrame.Buttons[2].InternalUpgrade;
                }
                Controller = LevelController.GetInstance((LevelFrame)_bufferedFrame);
                Controller.Frame = (LevelFrame)_bufferedFrame;
                if (Controller is LevelController levelController)
                {
                    levelController.ResumeGame(upgradeType);
                    LevelController.GameTimer.Resume();
                }
            }
        }

        private void ListenNameInputFrame(DialogFrameController dialogFrameController, NameInputFrame nameInputFrame)
        {
            while (Console.KeyAvailable)
            {
                Console.ReadKey(intercept: true);
            }
            ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);

            if (WasKeyPressed((int)MyKey.ENTER))
            {
                //List<JsonRecord> records = JsonWriter.ReadJsonRecordList();
                bool error = false;

                //foreach (var record in records)
                //{
                    if (nameInputFrame.Name.Text.Length == 0)
                    {
                        nameInputFrame.IsErrorMessaheActive = true;
                        error = true;
                    }
                    else
                    {
                        nameInputFrame.IsErrorMessaheActive = false;
                    }
                //}
                if (!error)
                {
                    LevelFrame level = new LevelFrame();
                    level.Player.Name = nameInputFrame.Name.Text;
                    Controller = LevelController.GetInstance(level);
                    LevelController.Clear();
                    LevelController.Initialize(level);
                    ControllerChanged?.Invoke(Controller);
                }
            }

            if (keyInfo.Key == ConsoleKey.Backspace && nameInputFrame.Name.CursorPosition > 0 && nameInputFrame.Name.Text.Length > 0)
            {
                nameInputFrame.Name.Text = nameInputFrame.Name.Text.Remove(nameInputFrame.Name.CursorPosition - 1, 1);
                nameInputFrame.Name.CursorPosition--;
            }
            else if (!char.IsControl(keyInfo.KeyChar) && nameInputFrame.Name.Text.Length < nameInputFrame.Name.Width - 2)
            {
                nameInputFrame.Name.Text = nameInputFrame.Name.Text.Insert(nameInputFrame.Name.CursorPosition, keyInfo.KeyChar.ToString());
                nameInputFrame.Name.CursorPosition++;
            }
        }

        private void ListenRecordsFrame(DialogFrameController dialogFrameController, RecordsFrame recordsFrame)
        {
            if (WasKeyPressed((int)MyKey.A))
            {
                dialogFrameController.ChangeActiveButton((int)MyKey.A);
            }
            else if (WasKeyPressed((int)MyKey.D))
            {
                dialogFrameController.ChangeActiveButton((int)MyKey.D);
            }
            else if (WasKeyPressed((int)MyKey.ENTER))
            {
                if (recordsFrame._activeButtonIndex == 1)
                {
                    Controller = new DialogFrameController(new MainMenuFrame());
                    ControllerChanged?.Invoke(Controller);
                }
                else
                {
                    JsonWriter.ClearJsonRecords();
                    recordsFrame.Records.Clear();
                    recordsFrame.Labels.RemoveRange(1, recordsFrame.Labels.Count - 1);
                }
            }
        }

        private void ListenPauseFrame(DialogFrameController dialogFrameController, PauseFrame pauseFrame)
        {
            if (WasKeyPressed((int)MyKey.A))
            {
                dialogFrameController.ChangeActiveButton((int)MyKey.A);
            }
            else if (WasKeyPressed((int)MyKey.D))
            {
                dialogFrameController.ChangeActiveButton((int)MyKey.D);
            }
            else if (WasKeyPressed((int)MyKey.ENTER))
            {
                if (pauseFrame._activeButtonIndex == 2)
                {
                    LevelController.Clear();
                    Controller = new DialogFrameController(new MainMenuFrame());
                    ControllerChanged?.Invoke(Controller);
                }
                else
                {
                    Controller = LevelController.GetInstance((LevelFrame)_bufferedFrame);
                    Controller.Frame = (LevelFrame)_bufferedFrame;
                    if (Controller is LevelController levelController)
                    {
                        levelController.ResumeGame();
                        LevelController.GameTimer.Resume();
                    }
                    ControllerChanged?.Invoke(Controller);
                }
            }
        }

        private void ListenLevelController(LevelController levelController)
        {
            if (IsKeyPressed((int)MyKey.S))
            {
                levelController.MovePlayerDown();
            }
            if (IsKeyPressed((int)MyKey.W))
            {
                levelController.MovePlayerUp();
            }
            if (IsKeyPressed((int)MyKey.A))
            {
                levelController.MovePlayerLeft();
            }
            if (IsKeyPressed((int)MyKey.D))
            {
                levelController.MovePlayerRight();
            }
            if (IsKeyPressed((int)MyKey.ESCAPE))
            {
                _bufferedFrame = Controller.Frame;
                if (Controller is LevelController lc)
                {
                    lc.EnemyGenerator.StopGenerating();
                    LevelController.GameTimer.Pause();
                    if (lc.Frame is LevelFrame level)
                    {
                        level.Player.IsStopped = true;
                    }
                }
                Controller = new DialogFrameController(new PauseFrame());
                ControllerChanged?.Invoke(Controller);
            }
        }
    }
}