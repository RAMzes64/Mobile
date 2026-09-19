using System;
using System.Collections.Generic;
using System.Text;

namespace FairyChess.Core
{
    /// <summary>
    /// /// Единый параметризованный класс фигуры: всё поведение определяется <see cref="PieceType"/>.
    /// Иммутабельность позволяет клонировать доску копированием ссылок.
    /// </summary>
    /// <param name="Type"></param>
    /// <param name="Color"></param>
    /// <param name="HasMoved"></param>
    public sealed record Piece(PieceType Type, Color Color, bool HasMoved = false)
    {
        /// <summary>
        /// Копия фигуры с отметкой «уже ходила».
        /// </summary>
        /// <returns></returns>
        public Piece AsMoved() => this with { HasMoved = true };
    }
}

