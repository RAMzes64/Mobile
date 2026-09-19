using System;
using System.Collections.Generic;
using System.Text;

namespace FairyChess.Core
{
    /// <summary>Стандартные шахматные фигуры, описанные шаблонами MovePattern.</summary>
    public static class StandardPieces
    {
        public static readonly PieceType King = new(
            "Король", 'K',
            new[]
            {
            new MovePattern(1, 0, Mirror: true),
            new MovePattern(1, 1, Mirror: true)
            },
            IsRoyal: true);

        public static readonly PieceType Queen = new(
            "Ферзь", 'Q',
            new[]
            {
            new MovePattern(1, 0, int.MaxValue, Mirror: true),
            new MovePattern(1, 1, int.MaxValue, Mirror: true)
            });

        public static readonly PieceType Rook = new(
            "Ладья", 'R',
            new[] { new MovePattern(1, 0, int.MaxValue, Mirror: true) });

        public static readonly PieceType Bishop = new(
            "Слон", 'B',
            new[] { new MovePattern(1, 1, int.MaxValue, Mirror: true) });

        public static readonly PieceType Knight = new(
            "Конь", 'N',
            new[] { new MovePattern(1, 2, Mirror: true) });

        public static readonly PieceType Pawn = new(
            "Пешка", 'P',
            new[]
            {
            new MovePattern(0, 1, QuietOnly: true),          // вперёд на свободную клетку
            new MovePattern(1, 1, CaptureOnly: true),        // взятие по диагонали
            new MovePattern(-1, 1, CaptureOnly: true)
            },
            DoubleFirstStep: true,
            PromotesTo: "Ферзь");
    }
}
