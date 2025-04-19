using MvcModel.Elements;
using MvcModel;
using MvcModel.Frames;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcModel.Upgrades;
using MvcModel.Сreatures.Heros;
using MvcModel.Сreatures;
using System.Numerics;
using MvcModel.Json;

namespace VimpireSurvivors_Console
{
    /// <summary>
    /// Класс для инициализации различных игровых фреймов в консольной версии игры.
    /// </summary>
    /// <remarks>
    /// Класс отвечает за создание и настройку элементов интерфейса для различных экранов игры,
    /// таких как главное меню, меню уровней, экран паузы, экран завершения игры и другие.
    /// </remarks>
    public class FrameInitializer
    {
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <remarks>
        /// Инициализирует обработчики событий для различных фреймов игры.
        /// </remarks>
        public FrameInitializer()
        {
            MainMenuFrame.MainMenuInitialized += OnMainMenuInitialized;
            LevelsMenuFrame.LevelsMenuInitialized += OnLevelsMenuInitialized;
            LevelFrame.LevelInitialized += OnLevelInitialized;
            NameInputFrame.NameInputInitialized += OnNameInputInitialized;
            PauseFrame.PauseFrameInitialized += OnPauseFrameInitialized;
            RecordsFrame.RecordsFrameInitialized += OnRecordsFrameInitialized;
            RulesFrame.RulesFrameInitialized += OnRulesFrameInitialized;
            UpgradeFrame.UpgradeFrameInitialized += OnUpgradeFrameInitialized;
            GameOverFrame.GameOverInitialized += OnGameOverInitialized;
        }

        /// <summary>
        /// Инициализирует главное меню игры.
        /// </summary>
        /// <param name="parMenu">Фрейм главного меню.</param>
        /// <remarks>
        /// Метод создает кнопки и метки для главного меню, такие как "Play", "Records", "Rules" и "Exit".
        /// </remarks>
        public static void OnMainMenuInitialized(MainMenuFrame parMenu)
        {
            Button buttonStartGame = new Button("Play", new Point(GameWindow.GetInstance().Width / 2 - 15, GameWindow.GetInstance().Height / 2 - 13), 5, 30);
            buttonStartGame.TextColor = ConsoleColor.Blue;
            buttonStartGame.ChangeStatus(true);

            Button buttonRecords = new Button("Records", new Point(GameWindow.GetInstance().Width / 2 - 15, GameWindow.GetInstance().Height / 2 - 7), 5, 30);
            buttonRecords.TextColor = ConsoleColor.Blue;

            Button buttonRules = new Button("Rules", new Point(GameWindow.GetInstance().Width / 2 - 15, GameWindow.GetInstance().Height / 2 - 1), 5, 30);
            buttonRules.TextColor = ConsoleColor.Blue;

            Button buttonExit = new Button("Exit", new Point(GameWindow.GetInstance().Width / 2 - 15, GameWindow.GetInstance().Height / 2 + 5), 5, 30);
            buttonExit.TextColor = ConsoleColor.Blue;

            Label gameTitle = new Label("VampireSurvivors", new Point(GameWindow.GetInstance().Width / 2 - 8, 12), 5, 30);
            gameTitle.TextColor = ConsoleColor.Blue;

            parMenu.Buttons = new List<Button>() { buttonStartGame, buttonRecords, buttonRules, buttonExit };
            parMenu.Labels.Add(gameTitle);
        }

        /// <summary>
        /// Инициализирует меню выбора уровней.
        /// </summary>
        /// <param name="parlLevelsMenu">Фрейм меню уровней.</param>
        /// <remarks>
        /// Метод создает кнопки для выбора уровней и кнопку "Exit".
        /// </remarks>
        public static void OnLevelsMenuInitialized(LevelsMenuFrame parlLevelsMenu)
        {
            Button buttonLevel_1 = new Button("Level 1", new Point(GameWindow.GetInstance().Width / 2 - 15, GameWindow.GetInstance().Height / 2 - 13), 5, 30);
            buttonLevel_1.BackgroundColor = ConsoleColor.Green;
            buttonLevel_1.TextColor = ConsoleColor.Blue;
            buttonLevel_1.ChangeStatus(true);

            Button buttonLevel_2 = new Button("Level 2", new Point(GameWindow.GetInstance().Width / 2 - 15, GameWindow.GetInstance().Height / 2 - 7), 5, 30);
            buttonLevel_2.BackgroundColor = ConsoleColor.Green;
            buttonLevel_2.TextColor = ConsoleColor.Blue;

            Button buttonLevel_3 = new Button("Level 3", new Point(GameWindow.GetInstance().Width / 2 - 15, GameWindow.GetInstance().Height / 2 - 1), 5, 30);
            buttonLevel_3.BackgroundColor = ConsoleColor.Green;
            buttonLevel_3.TextColor = ConsoleColor.Blue;

            Button buttonExit = new Button("Exit", new Point(GameWindow.GetInstance().Width / 2 - 15, GameWindow.GetInstance().Height / 2 + 5), 5, 30);
            buttonExit.BackgroundColor = ConsoleColor.Green;
            buttonExit.TextColor = ConsoleColor.Blue;

            parlLevelsMenu.Buttons = new List<Button>() { buttonLevel_1, buttonLevel_2, buttonLevel_3, buttonExit };
        }

        /// <summary>
        /// Инициализирует игровой уровень.
        /// </summary>
        /// <param name="parLevel">Фрейм игрового уровня.</param>
        /// <remarks>
        /// Метод создает игрока, размещает камни на уровне и настраивает оружие игрока.
        /// </remarks>
        public static void OnLevelInitialized(LevelFrame parLevel)
        {
            parLevel.Player = new Hero("Vlad", 100, GameWindow.GetInstance().Width / 2, GameWindow.GetInstance().Height / 2, 3, 1);
            parLevel.Player.IsStopped = false;
            parLevel.Rocks.Add(new Rock(34, 10));
            parLevel.Rocks.Add(new Rock(10, 20));
            parLevel.Rocks.Add(new Rock(16, 14));
            parLevel.Rocks.Add(new Rock(23, 30));

            foreach (Upgrade upgrade in parLevel.Player.Upgrades)
            {
                if (upgrade is Weapon weapon)
                {
                    weapon.WeaponAttack(parLevel.Player, parLevel.Enemies);
                    weapon._CTS.TryReset();
                }
            }
        }

        /// <summary>
        /// Инициализирует экран ввода имени игрока.
        /// </summary>
        /// <param name="parnNameInput">Фрейм ввода имени.</param>
        /// <remarks>
        /// Метод создает текстовое поле для ввода имени и метки для отображения сообщений.
        /// </remarks>
        public static void OnNameInputInitialized(NameInputFrame parnNameInput)
        {
            parnNameInput.Name = new TextBox("", new Point(GameWindow.GetInstance().Width / 2 - 15, GameWindow.GetInstance().Height / 2 - 2), 3, 30);
            parnNameInput.Name.TextColor = ConsoleColor.Blue;
            parnNameInput.Name.BackgroundColor = ConsoleColor.Green;

            Label errorLabel = new Label("Name is empty!", new Point(GameWindow.GetInstance().Width / 2 - 14,
                GameWindow.GetInstance().Height / 2 + 2), 3, 30);
            errorLabel.TextColor = ConsoleColor.Blue;

            Label printLabel = new Label("Print hero name", new Point(GameWindow.GetInstance().Width / 2 - 8,
                GameWindow.GetInstance().Height / 2 - 5), 3, 30);
            printLabel.TextColor = ConsoleColor.Blue;

            parnNameInput.Labels.Add(errorLabel);
            parnNameInput.Labels.Add(printLabel);
        }

        /// <summary>
        /// Инициализирует экран паузы.
        /// </summary>
        /// <param name="parPauseFrame">Фрейм паузы.</param>
        /// <remarks>
        /// Метод создает форму паузы, кнопки "Continue game" и "Main menu", а также метку "Pause".
        /// </remarks>
        public static void OnPauseFrameInitialized(PauseFrame parPauseFrame)
        {
            Form pauseForm = new Form(new Point(GameWindow.GetInstance().Width / 2 - 40, GameWindow.GetInstance().Height / 2 - 10),
                80, 20, ConsoleColor.Black);

            Button buttonBackToGame = new Button("Continue game", new Point(pauseForm.Coordinates.X + 2,
                pauseForm.Coordinates.Y + pauseForm.Height / 2 - 2), 5, 30);
            buttonBackToGame.TextColor = ConsoleColor.Blue;
            buttonBackToGame.ChangeStatus(true);

            Button buttonMainMenu = new Button("Main menu", new Point(pauseForm.Coordinates.X + pauseForm.Width - 32,
                pauseForm.Coordinates.Y + +pauseForm.Height / 2 - 2), 5, 30);
            buttonMainMenu.TextColor = ConsoleColor.Blue;

            Label pauseLabel = new Label("Pause", new Point(GameWindow.GetInstance().Width / 2 - 3, pauseForm.Coordinates.Y + 2), 3, 6);
            pauseLabel.TextColor = ConsoleColor.Blue;

            parPauseFrame.Buttons = new List<Button>() { buttonBackToGame, buttonMainMenu };
            parPauseFrame.Forms = new List<Form>() { pauseForm };
            parPauseFrame.Labels.Add(pauseLabel);
        }

        /// <summary>
        /// Инициализирует экран рекордов.
        /// </summary>
        /// <param name="parRecordsFrame">Фрейм рекордов.</param>
        /// <remarks>
        /// Метод создает таблицу рекордов, кнопки "Back" и "Clear records", а также отображает список рекордов.
        /// </remarks>
        public static void OnRecordsFrameInitialized(RecordsFrame parRecordsFrame)
        {
            Button buttonBack = new Button("Back", new Point(10, GameWindow.GetInstance().Height - 10), 5, 30);
            buttonBack.TextColor = ConsoleColor.Blue;
            buttonBack.ChangeStatus(true);

            Button buttonClearRecords = new Button("Clear records", new Point(GameWindow.GetInstance().Width - 40, GameWindow.GetInstance().Height - 10), 5, 30);
            buttonClearRecords.TextColor = ConsoleColor.Blue;

            Label header = new Label($"|{parRecordsFrame.CenterText("Top", RecordsFrame.NUMBER_WIDTH)}|{parRecordsFrame.CenterText("Player Name", RecordsFrame.NAME_WIDTH)}|{parRecordsFrame.CenterText("Time (s)", RecordsFrame.TIME_WIDTH)}|",
                new Point(GameWindow.GetInstance().Width / 2 - 20, 10), 3, 100);
            header.TextColor = ConsoleColor.Blue;
            parRecordsFrame.Labels.Add(header);

            parRecordsFrame.Records = parRecordsFrame.Records.OrderByDescending(record => record.Time).ToList();

            byte number = 1;
            int yOffset = 3;
            foreach (JsonRecord record in parRecordsFrame.Records)
            {
                string text = $"|{parRecordsFrame.CenterText(number.ToString(), RecordsFrame.NUMBER_WIDTH)}|{parRecordsFrame.CenterText(record.HeroName, RecordsFrame.NAME_WIDTH)}|{parRecordsFrame.CenterText(record.Time.ToString(), RecordsFrame.TIME_WIDTH)}|";
                Label recordLabel = new Label(text, new Point(GameWindow.GetInstance().Width / 2 - 20, 10 + yOffset), 3, 100);
                recordLabel.TextColor = ConsoleColor.Blue;
                parRecordsFrame.Labels.Add(recordLabel);
                yOffset += 3;
                number++;
            }
            parRecordsFrame.Buttons = new List<Button>() { buttonBack, buttonClearRecords };
        }

        /// <summary>
        /// Инициализирует экран правил игры.
        /// </summary>
        /// <param name="parRulesFrame">Фрейм правил игры.</param>
        /// <remarks>
        /// Метод создает метки с текстом правил игры и кнопку "Back".
        /// </remarks>
        public static void OnRulesFrameInitialized(RulesFrame parRulesFrame)
        {
            Label rulesLabel = new Label("Rules", new Point(GameWindow.GetInstance().Width / 2 - 5, 10), 3, 10);
            rulesLabel.TextColor = ConsoleColor.Blue;

            Label textLabel = new Label("In Vampire Survivors, your goal is to survive for as long as possible while battling waves of enemies. " +
                "Experience points are gained by defeating foes, allowing your character to level up. Each time you level up, you can select one " +
                "of three randomly offered upgrades to enhance your abilities. The available upgrades include powerful weapons like the Whip, " +
                "which targets the nearest enemy, and the Laser, which strikes a random enemy within the visible area. " +
                "You can also choose enhancements like the Oak Root, which increases your maximum health and replenishes it, or the Whetstone, " +
                "which boosts the damage of all your weapons.Your survival time is tracked by a timer, and the game continues until you fall in battle. " +
                "At the start, you’ll input your name, which will be recorded alongside your survival time in the Hall of Fame. " +
                "Push your limits, improve your strategy, and aim for the top spot in the leaderboard! Controls: WASD keys.", new Point(GameWindow.GetInstance().Width / 2 - 50, 15), 10, 100);
            textLabel.TextColor = ConsoleColor.Blue;

            Button buttonBack = new Button("Back", new Point(10, GameWindow.GetInstance().Height - 10), 5, 30);
            buttonBack.TextColor = ConsoleColor.Blue;
            buttonBack.ChangeStatus(true);

            parRulesFrame.Labels.Add(rulesLabel);
            parRulesFrame.Labels.Add(textLabel);

            parRulesFrame.Buttons = new List<Button>() { buttonBack };
        }

        /// <summary>
        /// Инициализирует экран улучшений.
        /// </summary>
        /// <param name="parUpgradeFrame">Фрейм улучшений.</param>
        /// <param name="parHero">Игрок, для которого применяются улучшения.</param>
        /// <remarks>
        /// Метод создает форму улучшений, кнопки для выбора улучшений и отображает текущие улучшения игрока.
        /// </remarks>
        public static void OnUpgradeFrameInitialized(UpgradeFrame parUpgradeFrame, Hero parHero)
        {
            Form upgradeForm = new Form(new Point(GameWindow.GetInstance().Width / 2 - 50, GameWindow.GetInstance().Height / 2 - 30),
                100, 56, ConsoleColor.Black);
            int buttonWidth = 20;
            int space = (upgradeForm.Width - 3 * buttonWidth) / 4;
            int offset = space;
            parUpgradeFrame.Buttons = new List<Button>();
            foreach (UpgradeType upgradeType in parUpgradeFrame.GenerateRandomUpgrades(parHero))
            {
                if (upgradeType == UpgradeType.Time)
                {
                    Button timeUpgradeButton = new Button("Time",
                    new Point(upgradeForm.Coordinates.X + offset, upgradeForm.Coordinates.Y + upgradeForm.Height - 15), 6, buttonWidth);
                    timeUpgradeButton.InternalUpgrade = upgradeType;
                    timeUpgradeButton.Discription = "Additional 5 seconds";

                    parUpgradeFrame.Buttons.Add(timeUpgradeButton);
                    continue;
                }
                var existingUpgrade = parHero.Upgrades.FirstOrDefault(upg => upg.Type == upgradeType);
                int nextLevelUpgrade = 0;
                if (existingUpgrade != null)
                {
                    nextLevelUpgrade = existingUpgrade.Level + 1;
                }
                else
                {
                    nextLevelUpgrade = 1;
                }
                Button upgradeButton = new Button(upgradeType.ToString() + "(lvl." + nextLevelUpgrade + ")",
                    new Point(upgradeForm.Coordinates.X + offset, upgradeForm.Coordinates.Y + upgradeForm.Height - 15), 6, buttonWidth);
                upgradeButton.InternalUpgrade = upgradeType;
                upgradeButton.NextLevelUpgrade = nextLevelUpgrade;
                List<JsonUpgrade> upgrades = JsonWriter.ReadJsonUpgradeList();
                var upgrade = upgrades.FirstOrDefault(u => u.Upgrade == upgradeButton.InternalUpgrade.ToString() && u.Level == upgradeButton.NextLevelUpgrade);
                string description = upgrade?.Discription ?? "Description not found";
                upgradeButton.Discription = description;
                parUpgradeFrame.Buttons.Add(upgradeButton);
                offset += upgradeButton.Width + space;
            }

            parUpgradeFrame.Buttons[0].BackgroundColor = ConsoleColor.Red;

            int offsetY = 0;
            foreach (Upgrade heroUpgrade in parHero.Upgrades)
            {
                string description = heroUpgrade.Type.ToString() + "(lvl." + heroUpgrade.Level + ")";
                if (heroUpgrade is Weapon weapon)
                {
                    description += " Damage: " + weapon.Damage + "(+" + (weapon.OverallDamage - weapon.Damage) + ")";
                }
                Label upgradeLabel = new Label(description, new Point(GameWindow.GetInstance().Width / 2 - 48,
                    GameWindow.GetInstance().Height / 2 - 28 + offsetY), 3, 50);
                upgradeLabel.TextColor = ConsoleColor.Blue;
                parUpgradeFrame.Labels.Add(upgradeLabel);
                offsetY += 2;
            }

            parUpgradeFrame.Forms = new List<Form>() { upgradeForm };
        }

        /// <summary>
        /// Инициализирует экран завершения игры.
        /// </summary>
        /// <param name="parGameOver">Фрейм завершения игры.</param>
        /// <remarks>
        /// Метод создает форму завершения игры, кнопку "To main menu" и метку "Game Over".
        /// </remarks>
        public static void OnGameOverInitialized(GameOverFrame parGameOver)
        {
            Form gameOverForm = new Form(new Point(GameWindow.GetInstance().Width / 2 - 40, GameWindow.GetInstance().Height / 2 - 10),
                80, 20, ConsoleColor.Black);

            Button buttonToMainMenu = new Button("To main menu", new Point(gameOverForm.Coordinates.X + 25,
                gameOverForm.Coordinates.Y + gameOverForm.Height / 2 - 2), 5, 30);
            buttonToMainMenu.TextColor = ConsoleColor.Blue;
            buttonToMainMenu.ChangeStatus(true);

            Label gameOverLabel = new Label("Game Over", new Point(GameWindow.GetInstance().Width / 2 - 5, gameOverForm.Coordinates.Y + 2), 3, 10);
            gameOverLabel.TextColor = ConsoleColor.Blue;

            parGameOver.Buttons = new List<Button>() { buttonToMainMenu };
            parGameOver.Forms = new List<Form>() { gameOverForm };
            parGameOver.Labels.Add(gameOverLabel);
        }
    }
}