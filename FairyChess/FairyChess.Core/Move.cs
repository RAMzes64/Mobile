using System;
using System.Collections.Generic;
using System.Text;

namespace FairyChess.Core
{
    /// <summary>Ход: откуда и куда.</summary>
    public readonly record struct Move(Position From, Position To);

    /// <summary>
    /// Снимок выполненного хода для Undo.
    /// В v1 взятая фигура всегда стояла на Move.To (взятие на проходе добавило бы отдельное поле).
    /// </summary>
    public sealed record MoveRecord(Move Move, Piece MovedBefore, Piece? Captured);
}
