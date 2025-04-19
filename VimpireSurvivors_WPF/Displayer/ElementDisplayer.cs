using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using MvcModel.Elements;

namespace VimpireSurvivors_WPF.Displayer
{
    /// <summary>
    /// Класс ElementDisplayer предоставляет методы для отрисовки элементов интерфейса в WPF.
    /// </summary>
    internal class ElementDisplayer
    {
        /// <summary>
        /// Отрисовывает кнопку в WPF.
        /// </summary>
        /// <param name="parDrawingContext">Контекст отрисовки.</param>
        /// <param name="parbButton">Кнопка для отрисовки.</param>
        public static void DrawButton(DrawingContext parDrawingContext, Button parbButton)
        {
            Brush backgroundBrush = ConvertConsoleColorToBrush(parbButton.BackgroundColor);
            Brush textBrush = ConvertConsoleColorToBrush(parbButton.TextColor);

            double x = parbButton.Coordinates.X;
            double y = parbButton.Coordinates.Y;

            double width = parbButton.Width;
            double height = parbButton.Height;

            Rect buttonRect = new Rect(x, y, width, height);
            parDrawingContext.DrawRectangle(backgroundBrush, null, buttonRect);

            if (!string.IsNullOrEmpty(parbButton.Text))
            {
                Typeface typeface = new Typeface("Segoe UI");
                FormattedText formattedText = new FormattedText(
                    parbButton.Text,
                    System.Globalization.CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    typeface,
                    16,
                    textBrush,
                    VisualTreeHelper.GetDpi(Application.Current.MainWindow).PixelsPerDip);

                double textX = x + (width - formattedText.Width) / 2;
                double textY = y + (height - formattedText.Height) / 2;
                parDrawingContext.DrawText(formattedText, new Point(textX, textY));
            }
        }

        /// <summary>
        /// Отрисовывает текстовое поле в WPF.
        /// </summary>
        /// <param name="parDrawingContext">Контекст отрисовки.</param>
        /// <param name="parTextBox">Текстовое поле для отрисовки.</param>
        public static void DrawTextBox(DrawingContext parDrawingContext, TextBox parTextBox)
        {
            Rect rect = new Rect(parTextBox.Coordinates.X, parTextBox.Coordinates.Y, parTextBox.Width, parTextBox.Height);

            SolidColorBrush backgroundBrush = new SolidColorBrush(ConvertConsoleColorToMediaColor(parTextBox.BackgroundColor));
            parDrawingContext.DrawRectangle(backgroundBrush, null, rect);

            string text = parTextBox.Text ?? string.Empty;
            int maxTextWidth = parTextBox.Width - 2;
            string visibleText = text.Length > maxTextWidth ? text.Substring(0, maxTextWidth) : text;

            FormattedText formattedText = new FormattedText(
                visibleText,
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface("Arial"),
                26,
                new SolidColorBrush(ConvertConsoleColorToMediaColor(parTextBox.TextColor)),
                VisualTreeHelper.GetDpi(Application.Current.MainWindow).PixelsPerDip
            );

            double textX = rect.X + 5;
            double textY = rect.Y + (rect.Height - formattedText.Height) / 2;

            parDrawingContext.DrawText(formattedText, new Point(textX, textY));
        }

        /// <summary>
        /// Отрисовывает метку в WPF.
        /// </summary>
        /// <param name="parDrawingContext">Контекст отрисовки.</param>
        /// <param name="parLabel">Метка для отрисовки.</param>
        /// <param name="parAlignLeft">Флаг, указывающий, нужно ли выравнивать текст по левому краю.</param>
        public static void DrawLabel(DrawingContext parDrawingContext, Label parLabel, bool parAlignLeft = false)
        {
            Rect rect = new Rect(parLabel.Coordinates.X, parLabel.Coordinates.Y, parLabel.Width, parLabel.Height);

            SolidColorBrush backgroundBrush = new SolidColorBrush(ConvertConsoleColorToMediaColor(parLabel.BackgroundColor));
            parDrawingContext.DrawRectangle(backgroundBrush, null, rect);

            int maxCharsPerLine = parLabel.Width / 10;
            List<string> lines = new List<string>();
            for (int i = 0; i < parLabel.Text.Length; i += maxCharsPerLine)
            {
                lines.Add(parLabel.Text.Substring(i, Math.Min(maxCharsPerLine, parLabel.Text.Length - i)));
            }

            int maxLines = parLabel.Height / 15;
            lines = lines.Take(maxLines).ToList();

            double textY = rect.Y + 5;

            foreach (var line in lines)
            {
                FormattedText formattedText = new FormattedText(
                    line,
                    System.Globalization.CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface("Arial"),
                    22,
                    new SolidColorBrush(ConvertConsoleColorToMediaColor(parLabel.TextColor)),
                    VisualTreeHelper.GetDpi(Application.Current.MainWindow).PixelsPerDip
                );

                double textX;
                if (parAlignLeft)
                {
                    textX = rect.X + 5;
                }
                else
                {
                    textX = rect.X + (rect.Width - formattedText.Width) / 2;
                }

                parDrawingContext.DrawText(formattedText, new Point(textX, textY));

                textY += 20;
            }
        }

        /// <summary>
        /// Отрисовывает форму в WPF.
        /// </summary>
        /// <param name="parDrawingContext">Контекст отрисовки.</param>
        /// <param name="parForm">Форма для отрисовки.</param>
        public static void DrawForm(DrawingContext parDrawingContext, Form parForm)
        {
            double x = parForm.Coordinates.X;
            double y = parForm.Coordinates.Y;
            double width = parForm.Width;
            double height = parForm.Height;

            SolidColorBrush backgroundBrush = new SolidColorBrush(ConvertConsoleColorToMediaColor(parForm.BackGroundColor));

            Rect rect = new Rect(x, y, width, height);
            parDrawingContext.DrawRectangle(backgroundBrush, null, rect);
        }

        /// <summary>
        /// Преобразует ConsoleColor в Brush.
        /// </summary>
        /// <param name="parConsoleColor">Цвет консоли.</param>
        /// <returns>Соответствующий Brush.</returns>
        private static Brush ConvertConsoleColorToBrush(ConsoleColor parConsoleColor)
        {
            switch (parConsoleColor)
            {
                case ConsoleColor.Black: return Brushes.Black;
                case ConsoleColor.DarkBlue: return Brushes.DarkBlue;
                case ConsoleColor.DarkGreen: return Brushes.DarkGreen;
                case ConsoleColor.DarkCyan: return Brushes.DarkCyan;
                case ConsoleColor.DarkRed: return Brushes.DarkRed;
                case ConsoleColor.DarkMagenta: return Brushes.DarkMagenta;
                case ConsoleColor.DarkYellow: return Brushes.Goldenrod;
                case ConsoleColor.Gray: return Brushes.Gray;
                case ConsoleColor.DarkGray: return Brushes.DarkGray;
                case ConsoleColor.Blue: return Brushes.Blue;
                case ConsoleColor.Green: return Brushes.Green;
                case ConsoleColor.Cyan: return Brushes.Cyan;
                case ConsoleColor.Red: return Brushes.Red;
                case ConsoleColor.Magenta: return Brushes.Magenta;
                case ConsoleColor.Yellow: return Brushes.Yellow;
                case ConsoleColor.White: return Brushes.White;
                default: return Brushes.Transparent;
            }
        }

        /// <summary>
        /// Преобразует ConsoleColor в Color.
        /// </summary>
        /// <param name="parConsoleColor">Цвет консоли.</param>
        /// <returns>Соответствующий Color.</returns>
        private static Color ConvertConsoleColorToMediaColor(ConsoleColor parConsoleColor)
        {
            return parConsoleColor switch
            {
                ConsoleColor.Black => Colors.Black,
                ConsoleColor.DarkBlue => Colors.DarkBlue,
                ConsoleColor.DarkGreen => Colors.DarkGreen,
                ConsoleColor.DarkCyan => Colors.DarkCyan,
                ConsoleColor.DarkRed => Colors.DarkRed,
                ConsoleColor.DarkMagenta => Colors.DarkMagenta,
                ConsoleColor.DarkYellow => Colors.Olive,
                ConsoleColor.Gray => Colors.Gray,
                ConsoleColor.DarkGray => Colors.DarkGray,
                ConsoleColor.Blue => Colors.Blue,
                ConsoleColor.Green => Colors.Green,
                ConsoleColor.Cyan => Colors.Cyan,
                ConsoleColor.Red => Colors.Red,
                ConsoleColor.Magenta => Colors.Magenta,
                ConsoleColor.Yellow => Colors.Yellow,
                ConsoleColor.White => Colors.White,
                _ => Colors.Transparent,
            };
        }
    }
}