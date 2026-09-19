using System;
using System.Collections.Generic;
using System.Text;

namespace FairyChess.Core
{
    public enum Color
    {
        White,
        Black
    }

    public static class ColorExtensions
    {
        public static Color Opponent(this Color color) =>
            color == Color.White ? Color.Black : Color.White;
    }
}
