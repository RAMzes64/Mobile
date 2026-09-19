using System;
using System.Collections.Generic;
using System.Text;

namespace FairyChess.Core
{
    /// <summary>
    /// Дескриптор типа фигуры — чистые данные. Новые типы создаются в рантайме
    /// без изменения движка: движок читает только Patterns и флаги.
    /// </summary>
    public sealed record PieceType(
        string Name,
        char Symbol,
        IReadOnlyList<MovePattern> Patterns,
        bool IsRoyal = false,
        bool DoubleFirstStep = false,
        string? PromotesTo = null)
    {
        /// <summary>Проверка корректности; вызывается каталогом и JSON-загрузчиком.</summary>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentException("Имя типа фигуры не должно быть пустым.", nameof(Name));

            foreach (var pattern in Patterns)
                pattern.Validate(Name);
        }
    }
}
