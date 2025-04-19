using MvcController;
using MvcModel.Frames;
using MvcModel.Upgrades;
using MvcModel.Сreatures.Heros;
using MvcController.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using MvcModel.Json;

namespace VimpireSurvivors_WPF
{
    /// <summary>
    /// Класс KeyListener обрабатывает ввод с клавиатуры и управляет взаимодействием с контроллером.
    /// </summary>
    public class KeyListener
    {
        private const long _DELAY = 50; // Задержка между обработкой нажатий клавиш

        private readonly HashSet<Key> _pressedKeys; // Набор нажатых клавиш

        /// <summary>
        /// Задача для прослушивания клавиш.
        /// </summary>
        public Task KeyListenerTask { get; set; }

        /// <summary>
        /// Флаг, указывающий, активно ли прослушивание клавиш.
        /// </summary>
        public bool IsListening { get; set; }

        /// <summary>
        /// Время последнего прослушивания.
        /// </summary>
        public long LastListeningTime { get; set; }

        /// <summary>
        /// Время последнего ввода.
        /// </summary>
        public long LastInputTime { get; set; }

        /// <summary>
        /// Текущее время.
        /// </summary>
        public long CurrentTime { get; set; }

        /// <summary>
        /// Разница во времени между текущим и последним прослушиванием.
        /// </summary>
        public long Delta { get; set; }

        /// <summary>
        /// Контроллер, управляющий текущим состоянием игры.
        /// </summary>
        public Controller Controller { get; set; }

        private Frame _bufferedFrame { get; set; } // Буферизованный фрейм для временного хранения

        /// <summary>
        /// Событие, возникающее при изменении контроллера.
        /// </summary>
        public static event Action<Controller> ControllerChanged;

        /// <summary>
        /// Событие, возникающее при смерти героя.
        /// </summary>
        public static event Action<Frame> HeroDied;

        /// <summary>
        /// Конструктор класса KeyListener.
        /// </summary>
        /// <param name="parController">Контроллер для управления.</param>
        public KeyListener(Controller parController)
        {
            Controller = parController;
            _pressedKeys = new HashSet<Key>();
        }

        /// <summary>
        /// Обрабатывает событие нажатия клавиши.
        /// </summary>
        /// <param name="e">Аргументы события нажатия клавиши.</param>
        public void OnKeyDown(KeyEventArgs e)
        {
            if (!_pressedKeys.Contains(e.Key))
            {
                _pressedKeys.Add(e.Key);
            }
        }

        /// <summary>
        /// Обрабатывает событие отпускания клавиши.
        /// </summary>
        /// <param name="e">Аргументы события отпускания клавиши.</param>
        public void OnKeyUp(KeyEventArgs e)
        {
            if (_pressedKeys.Contains(e.Key))
            {
                _pressedKeys.Remove(e.Key);
            }
        }

        /// <summary>
        /// Проверяет, нажата ли указанная клавиша.
        /// </summary>
        /// <param name="parKey">Клавиша для проверки.</param>
        /// <returns>True, если клавиша нажата, иначе False.</returns>
        public bool IsKeyPressed(Key parKey)
        {
            return _pressedKeys.Contains(parKey);
        }

        /// <summary>
        /// Запускает прослушивание клавиш.
        /// </summary>
        public async void StartKeyListener()
        {
            LevelController.HeroDied += OnHeroDied;
            LevelController.HeroUpgraded += OnHeroUpgraded;
            IsListening = true;
            await ListenKeys();
        }

        /// <summary>
        /// Основной цикл прослушивания клавиш.
        /// </summary>
        private async Task ListenKeys()
        {
            LastListeningTime = GameTimer.GetCurrentTime();

            while (IsListening)
            {
                CurrentTime = GameTimer.GetCurrentTime();
                Delta = CurrentTime - LastListeningTime;

                if (Delta > _DELAY)
                {
                    await ProcessKeys();
                    LastListeningTime = GameTimer.GetCurrentTime();
                }

                await Task.Delay(10);
            }
        }

        /// <summary>
        /// Обрабатывает нажатые клавиши.
        /// </summary>
        private async Task ProcessKeys()
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                switch (Controller)
                {
                    case DialogFrameController dialogFrameController:
                        if (dialogFrameController.Frame is MainMenuFrame mainMenuFrame)
                        {
                            //Обработка клавиш для главного меню
                            if (IsKeyPressed(Key.S))
                            {
                                dialogFrameController.ChangeActiveButton((int)MyKey.S);
                                _pressedKeys.Remove(Key.S);
                            }
                            else if (IsKeyPressed(Key.W))
                            {
                                dialogFrameController.ChangeActiveButton((int)MyKey.W);
                                _pressedKeys.Remove(Key.W);
                            }
                            else if (IsKeyPressed(Key.Enter))
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
                                _pressedKeys.Remove(Key.Enter);
                            }
                        }
                        else if (dialogFrameController.Frame is LevelsMenuFrame levelsMenuFrame)
                        {
                            //Обработка клавиш для меню выбора уровней
                            if (IsKeyPressed(Key.S))
                            {
                                dialogFrameController.ChangeActiveButton((int)MyKey.S);
                                _pressedKeys.Remove(Key.S);
                            }
                            else if (IsKeyPressed(Key.W))
                            {
                                dialogFrameController.ChangeActiveButton((int)MyKey.W);
                                _pressedKeys.Remove(Key.W);
                            }
                            else if (IsKeyPressed(Key.Enter))
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
                                _pressedKeys.Remove(Key.Enter);
                            }
                        }
                        else if (dialogFrameController.Frame is GameOverFrame gameOverFrame)
                        {
                            //Обработка клавиш для экрана завершения игры
                            if (IsKeyPressed(Key.A))
                            {
                                dialogFrameController.ChangeActiveButton((int)MyKey.A);
                                _pressedKeys.Remove(Key.A);
                            }
                            else if (IsKeyPressed(Key.D))
                            {
                                dialogFrameController.ChangeActiveButton((int)MyKey.D);
                                _pressedKeys.Remove(Key.D);
                            }
                            else if (IsKeyPressed(Key.Enter))
                            {
                                if (gameOverFrame._activeButtonIndex == 1)
                                {
                                    LevelController.Clear();
                                    Controller = new DialogFrameController(new MainMenuFrame());
                                    ControllerChanged?.Invoke(Controller);
                                }
                                _pressedKeys.Remove(Key.Enter);
                            }
                        }
                        else if (dialogFrameController.Frame is UpgradeFrame upgradeFrame)
                        {
                            //Обработка клавиш для экрана улучшений
                            if (IsKeyPressed(Key.A))
                            {
                                dialogFrameController.ChangeActiveButton((int)MyKey.A);
                                _pressedKeys.Remove(Key.A);
                            }
                            else if (IsKeyPressed(Key.D))
                            {
                                dialogFrameController.ChangeActiveButton((int)MyKey.D);
                                _pressedKeys.Remove(Key.D);
                            }
                            else if (IsKeyPressed(Key.Enter))
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
                                _pressedKeys.Remove(Key.Enter);
                            }
                        }
                        else if (dialogFrameController.Frame is NameInputFrame nameInput)
                        {
                            if (IsKeyPressed(Key.Enter))
                            {
                                List<JsonRecord> records = JsonWriter.ReadJsonRecordList();
                                bool error = false;

                                //foreach (var record in records)
                                //{
                                if (nameInput.Name.Text.Length == 0)
                                {
                                    nameInput.IsErrorMessaheActive = true;
                                    error = true;
                                }
                                else
                                {
                                    nameInput.IsErrorMessaheActive = false;
                                }
                                //}

                                if (!error)
                                {
                                    LevelFrame level = new LevelFrame();
                                    level.Player.Name = nameInput.Name.Text;
                                    Controller = LevelController.GetInstance(level);
                                    LevelController.Clear();
                                    LevelController.Initialize(level);
                                    ControllerChanged?.Invoke(Controller);
                                }
                                _pressedKeys.Remove(Key.Enter);
                            }

                            if (IsKeyPressed(Key.Back) && nameInput.Name.CursorPosition > 0 && nameInput.Name.Text.Length > 0)
                            {
                                nameInput.Name.Text = nameInput.Name.Text.Remove(nameInput.Name.CursorPosition - 1, 1);
                                nameInput.Name.CursorPosition--;
                            }
                            else
                            {
                                string input = GetTypedCharacter();
                                if (!string.IsNullOrEmpty(input) && nameInput.Name.Text.Length < 20)
                                {
                                    nameInput.Name.Text = nameInput.Name.Text.Insert(nameInput.Name.CursorPosition, input);
                                    nameInput.Name.CursorPosition++;
                                }
                            }
                            _pressedKeys.Remove(Key.Back);
                        }
                        else if (dialogFrameController.Frame is RecordsFrame recordsFrame)
                        {
                            //Обработка клавиш для экрана рекордов
                            if (IsKeyPressed(Key.A))
                            {
                                dialogFrameController.ChangeActiveButton((int)MyKey.A);
                                _pressedKeys.Remove(Key.A);
                            }
                            else if (IsKeyPressed(Key.D))
                            {
                                dialogFrameController.ChangeActiveButton((int)MyKey.D);
                                _pressedKeys.Remove(Key.D);
                            }
                            else if (IsKeyPressed(Key.Enter))
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
                                _pressedKeys.Remove(Key.Enter);
                            }
                        }
                        else if (dialogFrameController.Frame is RulesFrame rulesFrame)
                        {
                            //Обработка клавиш для экрана правил
                            if (IsKeyPressed(Key.Enter))
                            {
                                Controller = new DialogFrameController(new MainMenuFrame());
                                ControllerChanged?.Invoke(Controller);
                                _pressedKeys.Remove(Key.Enter);
                            }
                        }
                        else if (dialogFrameController.Frame is PauseFrame pauseFrame)
                        {
                            //Обработка клавиш для экрана паузы
                            if (IsKeyPressed(Key.A))
                            {
                                dialogFrameController.ChangeActiveButton((int)MyKey.A);
                                _pressedKeys.Remove(Key.A);
                            }
                            else if (IsKeyPressed(Key.D))
                            {
                                dialogFrameController.ChangeActiveButton((int)MyKey.D);
                                _pressedKeys.Remove(Key.D);
                            }
                            else if (IsKeyPressed(Key.Enter))
                            {
                                if (pauseFrame._activeButtonIndex == 2)
                                {
                                    LevelController.Clear();
                                    Controller = new DialogFrameController(new MainMenuFrame());
                                    ControllerChanged?.Invoke(Controller);
                                    _pressedKeys.Remove(Key.Enter);
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
                        break;
                    case LevelController levelController:
                        //Обработка клавиш для игрового уровня
                        if (IsKeyPressed(Key.S))
                        {
                            levelController.MovePlayerDown();
                        }
                        if (IsKeyPressed(Key.W))
                        {
                            levelController.MovePlayerUp();
                        }
                        if (IsKeyPressed(Key.A))
                        {
                            levelController.MovePlayerLeft();
                        }
                        if (IsKeyPressed(Key.D))
                        {
                            levelController.MovePlayerRight();
                        }
                        if (IsKeyPressed(Key.Escape))
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
                            _pressedKeys.Remove(Key.Escape);
                        }
                        break;
                    default:
                        break;
                }
            });
        }

        /// <summary>
        /// Получает символ, соответствующий нажатой клавише.
        /// </summary>
        /// <returns>Символ, соответствующий нажатой клавише.</returns>
        private string GetTypedCharacter()
        {
            foreach (Key key in _pressedKeys)
            {
                if (IsKeyPressed(key))
                {
                    _pressedKeys.Remove(key);
                    return ConvertKeyToLetter(key);
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Преобразует клавишу в символ.
        /// </summary>
        /// <param name="parKey">Клавиша для преобразования.</param>
        /// <returns>Символ, соответствующий клавише.</returns>
        private string ConvertKeyToLetter(Key parKey)
        {
            if (parKey >= Key.A && parKey <= Key.Z)
            {
                bool shift = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);
                bool capsLock = Console.CapsLock;

                char letter = (char)(parKey - Key.A + 'a');

                if (shift ^ capsLock)
                {
                    letter = char.ToUpper(letter);
                }

                return letter.ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// Обработчик события смерти героя.
        /// </summary>
        /// <param name="parHero">Герой, который умер.</param>
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
        /// Обработчик события улучшения героя.
        /// </summary>
        /// <param name="parHero">Герой, который получил улучшение.</param>
        private void OnHeroUpgraded(Hero parHero)
        {
            if (!(Controller.Frame is LevelFrame))
            {
                return;
            }
            if (Controller is LevelController)
            {
                LevelController.GameTimer.Pause();
            }
            _bufferedFrame = Controller.Frame;
            Controller.Frame = new UpgradeFrame(parHero);
            Controller = new DialogFrameController((UpgradeFrame)Controller.Frame);
        }
    }
}