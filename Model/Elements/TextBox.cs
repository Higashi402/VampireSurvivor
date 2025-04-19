using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcModel.Elements
{
    public class TextBox : Element
    {
        public string Text { get; set; }

        public int CursorPosition { get; set; }

        public ConsoleColor BackgroundColor { get; set; }

        public ConsoleColor TextColor { get; set; }

        public TextBox(string parText, Point parCoordinates, int parHeight, int parWidth)
        {
            this.Text = parText;
            this.Coordinates = parCoordinates;
            this.Height = parHeight;
            this.Width = parWidth;
            CursorPosition = 0;
        }
    }
}
