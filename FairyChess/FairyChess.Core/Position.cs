namespace FairyChess.Core
{
    /// <summary>
    /// Координата клетки на доске; компоненты — любые целые числа.
    /// </summary>
    /// <param name="X"></param>
    /// <param name="Y"></param>
    public readonly record struct Position(int X, int Y)
    {
        public static Position operator +(Position a, Position b) => new(a.X + b.X, a.Y + b.Y);

        public static Position operator -(Position a, Position b) => new(a.X - b.X, a.Y - b.Y);

        public override string ToString() => $"({X}, {Y})";
    }
}
