using System;
using System.Collections.Generic;
using System.Text;

namespace FairyChess.Core
{
    // <summary>
    /// Декларативный шаблон хода в ориентации белых (для чёрных Dy инвертируется движком).
    /// MaxSteps = 1 — прыжок; N / int.MaxValue — скольжение. Луч останавливается краем доски
    /// или любой фигурой; чужая фигура в конце луча может быть взята (если не QuietOnly).
    /// </summary>
    public sealed record MovePattern(
        int Dx,
        int Dy,
        int MaxSteps = 1,
        bool Mirror = false,
        bool CaptureOnly = false,
        bool QuietOnly = false)
    {
        public void Validate(string? ownerName = null)
        {
            string where = ownerName is null ? "" : $" (тип фигуры '{ownerName}')";

            if (Dx == 0 && Dy == 0)
                throw new ArgumentException($"Смещение (Dx, Dy) не может быть (0, 0){where}.");
            if (MaxSteps < 1)
                throw new ArgumentException($"MaxSteps должен быть не меньше 1{where}.");
            if (QuietOnly && CaptureOnly)
                throw new ArgumentException($"QuietOnly и CaptureOnly несовместимы{where}.");
        }
    }
}
