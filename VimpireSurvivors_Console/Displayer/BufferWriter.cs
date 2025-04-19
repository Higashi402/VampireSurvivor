using MvcModel;
using MvcModel.Elements;
using System.Globalization;

namespace VimpireSurvivors_Console.Displayer
{
    /// <summary>
    /// Класс BufferWriter предоставляет методы для отрисовки элементов интерфейса в буфере консоли.
    /// </summary>
    public class BufferWriter
    {
        /// <summary>
        /// Отрисовывает кнопку в буфере консоли.
        /// </summary>
        /// <param name="parButton">Кнопка для отрисовки.</param>
        /// <param name="parBuffer">Буфер консоли, в который производится отрисовка.</param>
        public static void WriteButton(Button parButton, ConsoleFastOutput.CharInfo[] parBuffer)
        {
            string buttonText = parButton.Text;
            short bgColor = (short)parButton.BackgroundColor;
            short textColor = (short)parButton.TextColor;

            int textStartX = (parButton.Width - buttonText.Length) / 2;
            int textStartY = parButton.Height / 2;

            for (int i = 0; i < parButton.Height; i++)
            {
                for (int j = 0; j < parButton.Width; j++)
                {
                    int globalX = parButton.Coordinates.X + j;
                    int globalY = parButton.Coordinates.Y + i;

                    if (globalX < 0 || globalX >= GameWindow.GetInstance().Width || globalY < 0 || globalY >= GameWindow.GetInstance().Height)
                        continue;

                    int bufferIndex = globalY * GameWindow.GetInstance().Width + globalX;

                    if (i == textStartY && j >= textStartX && j < textStartX + buttonText.Length)
                    {
                        parBuffer[bufferIndex].Char.UnicodeChar = buttonText[j - textStartX];
                        parBuffer[bufferIndex].Attributes = (short)(textColor | bgColor << 4);
                    }
                    else
                    {
                        parBuffer[bufferIndex].Char.UnicodeChar = ' ';
                        parBuffer[bufferIndex].Attributes = (short)(bgColor << 4);
                    }
                }
            }
        }

        /// <summary>
        /// Отрисовывает текстовое поле в буфере консоли.
        /// </summary>
        /// <param name="parTextBox">Текстовое поле для отрисовки.</param>
        /// <param name="parBuffer">Буфер консоли, в который производится отрисовка.</param>
        public static void WriteTextBox(TextBox parTextBox, ConsoleFastOutput.CharInfo[] parBuffer)
        {
            string text = parTextBox.Text ?? string.Empty;
            short bgColor = (short)parTextBox.BackgroundColor;
            short textColor = (short)parTextBox.TextColor;

            int textStartY = parTextBox.Height / 2;
            int maxTextWidth = parTextBox.Width - 2;
            string visibleText = text.Length > maxTextWidth ? text.Substring(0, maxTextWidth) : text;

            for (int i = 0; i < parTextBox.Height; i++)
            {
                for (int j = 0; j < parTextBox.Width; j++)
                {
                    int globalX = parTextBox.Coordinates.X + j;
                    int globalY = parTextBox.Coordinates.Y + i;

                    if (globalX < 0 || globalX >= GameWindow.GetInstance().Width || globalY < 0 || globalY >= GameWindow.GetInstance().Height)
                        continue;

                    int bufferIndex = globalY * GameWindow.GetInstance().Width + globalX;

                    if (i == textStartY && j > 0 && j <= visibleText.Length)
                    {
                        parBuffer[bufferIndex].Char.UnicodeChar = visibleText[j - 1];
                        parBuffer[bufferIndex].Attributes = (short)(textColor | bgColor << 4);
                    }
                    else
                    {
                        parBuffer[bufferIndex].Char.UnicodeChar = ' ';
                        parBuffer[bufferIndex].Attributes = (short)(bgColor << 4);
                    }
                }
            }
        }

        /// <summary>
        /// Отрисовывает метку (Label) в буфере консоли.
        /// </summary>
        /// <param name="parLabel">Метка для отрисовки.</param>
        /// <param name="parBuffer">Буфер консоли, в который производится отрисовка.</param>
        public static void WriteLabel(Label parLabel, ConsoleFastOutput.CharInfo[] parBuffer)
        {
            string text = parLabel.Text;
            short bgColor = (short)parLabel.BackgroundColor;
            short textColor = (short)parLabel.TextColor;

            int textStartX = 0;
            int maxCharsPerLine = parLabel.Width;

            List<string> lines = new List<string>();
            for (int i = 0; i < text.Length; i += maxCharsPerLine)
            {
                lines.Add(text.Substring(i, Math.Min(maxCharsPerLine, text.Length - i)));
            }

            lines = lines.Take(parLabel.Height).ToList();

            for (int i = 0; i < parLabel.Height; i++)
            {
                for (int j = 0; j < parLabel.Width; j++)
                {
                    int globalX = parLabel.Coordinates.X + j;
                    int globalY = parLabel.Coordinates.Y + i;

                    if (globalX < 0 || globalX >= GameWindow.GetInstance().Width || globalY < 0 || globalY >= GameWindow.GetInstance().Height)
                        continue;

                    int bufferIndex = globalY * GameWindow.GetInstance().Width + globalX;

                    if (i < lines.Count && j < lines[i].Length)
                    {
                        parBuffer[bufferIndex].Char.UnicodeChar = lines[i][j];
                        parBuffer[bufferIndex].Attributes = (short)(textColor | bgColor << 4);
                    }
                    else
                    {
                        parBuffer[bufferIndex].Char.UnicodeChar = ' ';
                        parBuffer[bufferIndex].Attributes = (short)(bgColor << 4);
                    }
                }
            }
        }

        /// <summary>
        /// Отрисовывает форму (Form) в буфере консоли.
        /// </summary>
        /// <param name="parForm">Форма для отрисовки.</param>
        /// <param name="parBuffer">Буфер консоли, в который производится отрисовка.</param>
        public static void WriteForm(Form parForm, ConsoleFastOutput.CharInfo[] parBuffer)
        {
            short bgColor = (short)parForm.BackGroundColor;

            for (int i = 0; i < parForm.Height; i++)
            {
                for (int j = 0; j < parForm.Width; j++)
                {
                    int globalX = parForm.Coordinates.X + j;
                    int globalY = parForm.Coordinates.Y + i;

                    if (globalX < 0 || globalX >= GameWindow.GetInstance().Width || globalY < 0 || globalY >= GameWindow.GetInstance().Height)
                        continue;

                    int bufferIndex = globalY * GameWindow.GetInstance().Width + globalX;
                    parBuffer[bufferIndex].Char.UnicodeChar = ' ';
                    parBuffer[bufferIndex].Attributes = (short)(bgColor << 4);
                }
            }
        }
    }
}