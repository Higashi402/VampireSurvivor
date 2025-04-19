using MvcModel.Elements;
using MvcModel.Frames;
using MvcModel.Upgrades;
using MvcModel.Сreatures.Heros;
using MvcModel.Сreatures;
using MvcModel;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcModel.Json;

namespace VimpireSurvivors_WPF
{
    /// <summary>
    /// Класс для инициализации различных игровых фреймов.
    /// </summary>
    /// <remarks>
    /// Класс отвечает за создание и настройку элементов интерфейса для различных экранов игры,
    /// таких как главное меню, меню уровней, экран паузы, экран завершения игры и другие.
    /// </remarks>
    public class FrameInitializer
    {
        /// <summary>
        /// Ширина столбца для отображения номера в таблице рекордов.
        /// </summary>
        public int NUMBER_WIDTH = (int)(GameWindow.GetInstance().Width * 0.1);

        /// <summary>
        /// Ширина столбца для отображения имени игрока в таблице рекордов.
        /// </summary>
        public int NAME_WIDTH = (int)(GameWindow.GetInstance().Width * 0.3);

        /// <summary>
        /// Ширина столбца для отображения времени в таблице рекордов.
        /// </summary>
        public int TIME_WIDTH = (int)(GameWindow.GetInstance().Width * 0.2);

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
            int buttonWidth = 600;
            int buttonHeight = 100;

            Button buttonStartGame = new Button("Play", new Point(GameWindow.GetInstance().Width / 2 - buttonWidth / 2, (int)(GameWindow.GetInstance().Height * 0.2)), buttonHeight, buttonWidth);
            buttonStartGame.TextColor = ConsoleColor.Blue;
            buttonStartGame.ChangeStatus(true);

            Button buttonRecords = new Button("Records", new Point(GameWindow.GetInstance().Width / 2 - buttonWidth / 2, (int)(GameWindow.GetInstance().Height * 0.35)), buttonHeight, buttonWidth);
            buttonRecords.TextColor = ConsoleColor.Blue;

            Button buttonRules = new Button("Rules", new Point(GameWindow.GetInstance().Width / 2 - buttonWidth / 2, (int)(GameWindow.GetInstance().Height * 0.5)), buttonHeight, buttonWidth);
            buttonRules.TextColor = ConsoleColor.Blue;

            Button buttonExit = new Button("Exit", new Point(GameWindow.GetInstance().Width / 2 - buttonWidth / 2, (int)(GameWindow.GetInstance().Height * 0.65)), buttonHeight, buttonWidth);
            buttonExit.TextColor = ConsoleColor.Blue;

            Label gameTitle = new Label("VampireSurvivors", new Point(GameWindow.GetInstance().Width / 2 - buttonWidth / 2, (int)(GameWindow.GetInstance().Height * 0.1)), buttonHeight, buttonWidth);
            gameTitle.TextColor = ConsoleColor.Blue;

            parMenu.Buttons = new List<Button>() { buttonStartGame, buttonRecords, buttonRules, buttonExit };
            parMenu.Labels.Add(gameTitle);
        }

        /// <summary>
        /// Инициализирует меню выбора уровней.
        /// </summary>
        /// <param name="parLevelsMenu">Фрейм меню уровней.</param>
        /// <remarks>
        /// Метод создает кнопки для выбора уровней и кнопку "Exit".
        /// </remarks>
        public static void OnLevelsMenuInitialized(LevelsMenuFrame parLevelsMenu)
        {
            int buttonWidth = 600;
            int buttonHeight = 100;

            Button buttonLevel_1 = new Button("Level 1", new Point(GameWindow.GetInstance().Width / 2 - buttonWidth / 2, (int)(GameWindow.GetInstance().Height * 0.2)), buttonHeight, buttonWidth);
            buttonLevel_1.BackgroundColor = ConsoleColor.Green;
            buttonLevel_1.TextColor = ConsoleColor.Blue;
            buttonLevel_1.ChangeStatus(true);

            Button buttonLevel_2 = new Button("Level 2", new Point(GameWindow.GetInstance().Width / 2 - buttonWidth / 2, (int)(GameWindow.GetInstance().Height * 0.35)), buttonHeight, buttonWidth);
            buttonLevel_2.BackgroundColor = ConsoleColor.Green;
            buttonLevel_2.TextColor = ConsoleColor.Blue;

            Button buttonLevel_3 = new Button("Level 3", new Point(GameWindow.GetInstance().Width / 2 - buttonWidth / 2, (int)(GameWindow.GetInstance().Height * 0.5)), buttonHeight, buttonWidth);
            buttonLevel_3.BackgroundColor = ConsoleColor.Green;
            buttonLevel_3.TextColor = ConsoleColor.Blue;

            Button buttonExit = new Button("Exit", new Point(GameWindow.GetInstance().Width / 2 - buttonWidth / 2, (int)(GameWindow.GetInstance().Height * 0.65)), buttonHeight, buttonWidth);
            buttonExit.BackgroundColor = ConsoleColor.Green;
            buttonExit.TextColor = ConsoleColor.Blue;

            parLevelsMenu.Buttons = new List<Button>() { buttonLevel_1, buttonLevel_2, buttonLevel_3, buttonExit };
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
            //int heroWidth = GameWindow.GetInstance().Width / 160;
            //int heroHeight = GameWindow.GetInstance().Height / 62 * 3;
            int heroWidth = GameWindow.GetInstance().Width / 62;
            int heroHeight = GameWindow.GetInstance().Height / 20;
            parLevel.Player = new Hero("Vlad", 100, GameWindow.GetInstance().Width / 2 - heroWidth / 2, GameWindow.GetInstance().Height / 2 - heroHeight / 2, heroHeight, heroWidth);
            parLevel.Player.IsStopped = false;
            parLevel.Rocks.Add(new Rock(100, 150));
            parLevel.Rocks.Add(new Rock(40, 500));
            parLevel.Rocks.Add(new Rock(600, 50));
            parLevel.Rocks.Add(new Rock(400, 200));

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
        /// <param name="parNameInput">Фрейм ввода имени.</param>
        /// <remarks>
        /// Метод создает текстовое поле для ввода имени и метки для отображения сообщений.
        /// </remarks>
        public static void OnNameInputInitialized(NameInputFrame parNameInput)
        {
            int textBoxWidth = (int)(GameWindow.GetInstance().Width * 0.3);
            int labelWidth = (int)(GameWindow.GetInstance().Width * 0.2);
            int labelHeight = (int)(GameWindow.GetInstance().Width * 0.05);

            parNameInput.Name = new TextBox("", new Point(GameWindow.GetInstance().Width / 2 - textBoxWidth / 2, (int)(GameWindow.GetInstance().Height * 0.45)), (int)(GameWindow.GetInstance().Height * 0.1), textBoxWidth);
            parNameInput.Name.TextColor = ConsoleColor.Blue;
            parNameInput.Name.BackgroundColor = ConsoleColor.Green;

            Label errorLabel = new Label("Name is empty!", new Point(GameWindow.GetInstance().Width / 2 - labelWidth / 2,
                (int)(GameWindow.GetInstance().Height * 0.55)), labelHeight, labelWidth);
            errorLabel.TextColor = ConsoleColor.Blue;

            Label printLabel = new Label("Print hero name", new Point(GameWindow.GetInstance().Width / 2 - labelWidth / 2,
                (int)(GameWindow.GetInstance().Height * 0.35)), labelHeight, labelWidth);
            printLabel.TextColor = ConsoleColor.Blue;

            parNameInput.Labels.Add(errorLabel);
            parNameInput.Labels.Add(printLabel);
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
            int pauseFormWidth = (int)(GameWindow.GetInstance().Width * 0.5);
            int pauseFormHeight = (int)(GameWindow.GetInstance().Height * 0.3);

            int buttonWidth = (int)(GameWindow.GetInstance().Width * 0.12);
            int buttonHeight = (int)(GameWindow.GetInstance().Height * 0.05);

            int labelWidth = (int)(GameWindow.GetInstance().Width * 0.2);
            int labelHeight = (int)(GameWindow.GetInstance().Height * 0.1);

            Form pauseForm = new Form(new Point(GameWindow.GetInstance().Width / 2 - pauseFormWidth / 2, GameWindow.GetInstance().Height / 2 - pauseFormHeight / 2),
                pauseFormWidth, pauseFormHeight, ConsoleColor.DarkGray);

            Button buttonBackToGame = new Button("Continue game", new Point(pauseForm.Coordinates.X + (int)(pauseForm.Width * 0.1),
                pauseForm.Coordinates.Y + (int)(pauseForm.Height * 0.5)), buttonHeight, buttonWidth);
            buttonBackToGame.TextColor = ConsoleColor.Blue;
            buttonBackToGame.ChangeStatus(true);

            Button buttonMainMenu = new Button("Main menu", new Point(pauseForm.Coordinates.X + pauseForm.Width - buttonWidth - (int)(pauseForm.Width * 0.1),
                pauseForm.Coordinates.Y + (int)(pauseForm.Height * 0.5)), buttonHeight, buttonWidth);
            buttonMainMenu.TextColor = ConsoleColor.Blue;

            Label pauseLabel = new Label("Pause", new Point(pauseForm.Coordinates.X + pauseForm.Width / 2 - labelWidth / 2, (int)(pauseForm.Coordinates.Y + pauseForm.Height * 0.1)), labelHeight, labelWidth);
            pauseLabel.TextColor = ConsoleColor.Blue;
            pauseLabel.BackgroundColor = ConsoleColor.DarkGray;

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
            int buttonWidth = 400;
            int buttonHeight = 100;

            int rowWidth = (int)(GameWindow.GetInstance().Width * 0.3);
            int rowHeight = (int)(GameWindow.GetInstance().Height * 0.06);

            Button buttonBack = new Button("Back", new Point((int)(GameWindow.GetInstance().Width * 0.02), (int)(GameWindow.GetInstance().Height * 0.75)), buttonHeight, buttonWidth);
            buttonBack.TextColor = ConsoleColor.Blue;
            buttonBack.ChangeStatus(true);

            Button buttonClearRecords = new Button("Clear records", new Point((int)(GameWindow.GetInstance().Width * 0.95 - buttonWidth), (int)(GameWindow.GetInstance().Height * 0.75)), buttonHeight, buttonWidth);
            buttonClearRecords.TextColor = ConsoleColor.Blue;

            Label header = new Label($"|{parRecordsFrame.CenterText("Top", RecordsFrame.NUMBER_WIDTH)}|{parRecordsFrame.CenterText("Player Name", RecordsFrame.NAME_WIDTH)}|{parRecordsFrame.CenterText("Time (s)", RecordsFrame.TIME_WIDTH)}|",
                new Point(GameWindow.GetInstance().Width / 2 - rowWidth / 2, (int)(GameWindow.GetInstance().Height * 0.15)), rowHeight, rowWidth);
            header.TextColor = ConsoleColor.Blue;
            parRecordsFrame.Labels.Add(header);

            parRecordsFrame.Records = parRecordsFrame.Records.OrderByDescending(record => record.Time).ToList();

            byte number = 1;
            int yOffset = (int)(GameWindow.GetInstance().Height * 0.20);
            foreach (JsonRecord record in parRecordsFrame.Records)
            {
                string text = $"|{parRecordsFrame.CenterText(number.ToString(), RecordsFrame.NUMBER_WIDTH)}|{parRecordsFrame.CenterText(record.HeroName, RecordsFrame.NAME_WIDTH)}|{parRecordsFrame.CenterText(record.Time.ToString(), RecordsFrame.TIME_WIDTH)}|";
                Label recordLabel = new Label(text, new Point(GameWindow.GetInstance().Width / 2 - rowWidth / 2, yOffset), rowHeight, rowWidth);
                recordLabel.TextColor = ConsoleColor.Blue;
                parRecordsFrame.Labels.Add(recordLabel);
                yOffset += 30;
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
            int buttonWidth = 400;
            int buttonHeight = 100;
            int rulesLabelWidth = (int)(GameWindow.GetInstance().Height * 0.7);

            Label rulesLabel = new Label("Rules", new Point(GameWindow.GetInstance().Width / 2 - 50, (int)(GameWindow.GetInstance().Height * 0.1)), buttonHeight, buttonWidth);
            rulesLabel.TextColor = ConsoleColor.Blue;

            Label textLabel = new Label("In Vampire Survivors, your goal is to survive for as long as possible while battling waves of enemies. " +
                "Experience points are gained by defeating foes, allowing your character to level up. Each time you level up, you can select one of three randomly " +
                "offered upgrades to enhance your abilities. The available upgrades include powerful weapons like the Whip, which targets the nearest enemy, and " +
                "the Laser, which strikes a random enemy within the visible area. You can also choose enhancements like the Oak Root, which increases your maximum " +
                "health and replenishes it, or the Whetstone, which boosts the damage of all your weapons.Your survival time is tracked by a timer, and the game " +
                "continues until you fall in battle. At the start, you’ll input your name, which will be recorded alongside your survival time in the Hall of Fame. " +
                "Push your limits, improve your strategy, and aim for the top spot in the leaderboard! Controls: WASD keys.",
                new Point(GameWindow.GetInstance().Width / 2 - rulesLabelWidth / 2, (int)(GameWindow.GetInstance().Height * 0.25)), (int)(GameWindow.GetInstance().Height * 0.5), rulesLabelWidth);
            textLabel.TextColor = ConsoleColor.Blue;

            Button buttonBack = new Button("Back", new Point((int)(GameWindow.GetInstance().Width * 0.05), (int)(GameWindow.GetInstance().Height * 0.75)), buttonHeight, buttonWidth);
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
            int upgradeFormWidth = (int)(GameWindow.GetInstance().Width * 0.7);
            int upgradeFormHeight = (int)(GameWindow.GetInstance().Height * 0.7);

            int buttonWidth = (int)(GameWindow.GetInstance().Width * 0.12);
            int buttonHeight = (int)(GameWindow.GetInstance().Height * 0.05);

            Form upgradeForm = new Form(new Point(GameWindow.GetInstance().Width / 2 - upgradeFormWidth / 2, GameWindow.GetInstance().Height / 2 - upgradeFormHeight / 2),
                upgradeFormWidth, upgradeFormHeight, ConsoleColor.DarkGray);

            int space = (upgradeForm.Width - 3 * buttonWidth) / 4;
            int offset = space;
            parUpgradeFrame.Buttons = new List<Button>();
            foreach (UpgradeType upgradeType in parUpgradeFrame.GenerateRandomUpgrades(parHero))
            {
                if (upgradeType == UpgradeType.Time)
                {
                    Button timeUpgradeButton = new Button("Time",
                    new Point(upgradeForm.Coordinates.X + offset, upgradeForm.Coordinates.Y + (int)(upgradeForm.Height * 0.8)), buttonHeight, buttonWidth);
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
                    new Point(upgradeForm.Coordinates.X + offset, upgradeForm.Coordinates.Y + (int)(upgradeForm.Height * 0.8)), buttonHeight, buttonWidth);
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

            int labelWidth = (int)(GameWindow.GetInstance().Width * 0.3);
            int labelHeight = (int)(GameWindow.GetInstance().Height * 0.05);

            int offsetY = 0;
            foreach (Upgrade heroUpgrade in parHero.Upgrades)
            {
                string description = heroUpgrade.Type.ToString() + "(lvl." + heroUpgrade.Level + ")";
                if (heroUpgrade is Weapon weapon)
                {
                    description += " Damage: " + weapon.Damage + "(+" + (weapon.OverallDamage - weapon.Damage) + ")";
                }
                Label upgradeLabel = new Label(description, new Point(upgradeForm.Coordinates.X + (int)(upgradeForm.Width * 0.05),
                    upgradeForm.Coordinates.Y + (int)(upgradeForm.Height * 0.05) + offsetY), labelHeight, labelWidth);
                upgradeLabel.TextColor = ConsoleColor.Blue;
                upgradeLabel.BackgroundColor = ConsoleColor.Gray;
                parUpgradeFrame.Labels.Add(upgradeLabel);
                offsetY += (int)(GameWindow.GetInstance().Height * 0.05);
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
            int gameOverFormWidth = (int)(GameWindow.GetInstance().Width * 0.5);
            int gameOverFormHeight = (int)(GameWindow.GetInstance().Height * 0.3);

            int buttonWidth = (int)(GameWindow.GetInstance().Width * 0.12);
            int buttonHeight = (int)(GameWindow.GetInstance().Height * 0.05);

            Form gameOverForm = new Form(new Point(GameWindow.GetInstance().Width / 2 - gameOverFormWidth / 2, GameWindow.GetInstance().Height / 2 - gameOverFormHeight / 2),
                gameOverFormWidth, gameOverFormHeight, ConsoleColor.DarkGray);

            Button buttonToMainMenu = new Button("To main menu", new Point(gameOverForm.Coordinates.X + (int)(gameOverFormWidth / 2) - buttonWidth / 2,
                gameOverForm.Coordinates.Y + (int)(gameOverForm.Height * 0.5)), buttonHeight, buttonWidth);
            buttonToMainMenu.TextColor = ConsoleColor.Blue;
            buttonToMainMenu.ChangeStatus(true);

            Label gameOverLabel = new Label("Game Over", new Point(gameOverForm.Coordinates.X + gameOverForm.Width / 2 - buttonWidth / 2, gameOverForm.Coordinates.Y + (int)(gameOverFormHeight * 0.1)), buttonHeight, buttonWidth);
            gameOverLabel.TextColor = ConsoleColor.Blue;
            gameOverLabel.BackgroundColor = ConsoleColor.DarkGray;

            parGameOver.Buttons = new List<Button>() { buttonToMainMenu };
            parGameOver.Forms = new List<Form>() { gameOverForm };
            parGameOver.Labels.Add(gameOverLabel);
        }
    }
}