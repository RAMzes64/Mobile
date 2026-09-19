using System;
using System.Collections.Generic;
using System.Text;

namespace FairyChess.Core
{
    /// <summary>Геометрия ходов и атак. Ничего не знает о шахах, мате и очерёдности.</summary>
    public static class MoveGenerator
    {
        /// <summary>
        /// Псевдолегальные ходы фигуры (без учёта шаха).
        /// Пусто, если клетка пуста. Паттерны Dy инвертируются для чёрных.
        /// </summary>
        public static IEnumerable<Move> PseudoLegal(Board board, Position from)
        {
            ArgumentNullException.ThrowIfNull(board);

            var piece = board[from];
            if (piece is null)
                yield break;

            int flip = piece.Color == Color.White ? 1 : -1;

            foreach (var pattern in piece.Type.Patterns)
                foreach (var (vx, vy) in Variants(pattern))
                    foreach (var move in RayMoves(board, from, piece, vx, vy * flip, pattern))
                        yield return move;

            // Двойной первый ход «вперёд»: (0, +1) для белых, (0, -1) для чёрных; обе клетки свободны.
            if (piece.Type.DoubleFirstStep && !piece.HasMoved)
            {
                Position mid = from + new Position(0, flip);
                Position dest = from + new Position(0, flip * 2);
                if (board.Contains(mid) && board[mid] is null &&
                    board.Contains(dest) && board[dest] is null)
                    yield return new Move(from, dest);
            }
        }

        /// <summary>
        /// Бьётся ли клетка target фигурами стороны bySide. Цель рассматривается как «взятая»
        /// независимо от её содержимого; тихие шаблоны (QuietOnly) атак не создают.
        /// </summary>
        public static bool IsAttacked(Board board, Position target, Color bySide)
        {
            ArgumentNullException.ThrowIfNull(board);

            int flip = bySide == Color.White ? 1 : -1;

            foreach (var (from, piece) in board.Pieces)
            {
                if (piece.Color != bySide)
                    continue;

                Position delta = target - from;

                foreach (var pattern in piece.Type.Patterns)
                {
                    if (pattern.QuietOnly)
                        continue;

                    foreach (var (vx, vy) in Variants(pattern))
                    {
                        (int ex, int ey) = (vx, vy * flip);
                        if (Reaches(delta, ex, ey, pattern.MaxSteps, out int steps) &&
                            PathIsClear(board, from, ex, ey, steps))
                            return true;
                    }
                }
            }

            return false;
        }

        private static IEnumerable<Move> RayMoves(
            Board board, Position from, Piece piece, int vx, int vy, MovePattern pattern)
        {
            Position current = from;
            for (int step = 1; step <= pattern.MaxSteps; step++)
            {
                current += new Position(vx, vy);
                if (!board.Contains(current))
                    yield break; // вышли за доску

                Piece? occupant = board[current];
                if (occupant is null)
                {
                    if (!pattern.CaptureOnly)
                        yield return new Move(from, current);
                    continue;
                }

                if (occupant.Color != piece.Color && !pattern.QuietOnly)
                    yield return new Move(from, current);
                yield break; // луч заблокирован любой фигурой
            }
        }

        /// <summary>Лежит ли delta на луче (vx, vy) на расстоянии 1..maxSteps шагов.</summary>
        private static bool Reaches(Position delta, int vx, int vy, int maxSteps, out int steps)
        {
            steps = 0;
            if (vx == 0 && vy == 0)
                return false;

            if (vx == 0)
            {
                if (delta.X != 0 || delta.Y % vy != 0) return false;
                steps = delta.Y / vy;
            }
            else if (vy == 0)
            {
                if (delta.Y != 0 || delta.X % vx != 0) return false;
                steps = delta.X / vx;
            }
            else
            {
                if (delta.X % vx != 0 || delta.Y % vy != 0) return false;
                int kx = delta.X / vx;
                int ky = delta.Y / vy;
                if (kx != ky) return false;
                steps = kx;
            }

            return steps >= 1 && steps <= maxSteps;
        }

        /// <summary>Свободны ли клетки строго между from и целью (шаги 1..steps-1).</summary>
        private static bool PathIsClear(Board board, Position from, int vx, int vy, int steps)
        {
            for (int step = 1; step < steps; step++)
                if (board[from + new Position(vx * step, vy * step)] != null)
                    return false;
            return true;
        }

        /// <summary>
        /// Разворот шаблона в набор векторов: без Mirror — только базовый вектор,
        /// с Mirror — все 8 отражений/поворотов (дубликаты исключаются).
        /// </summary>
        private static IEnumerable<(int X, int Y)> Variants(MovePattern pattern)
        {
            if (!pattern.Mirror)
            {
                yield return (pattern.Dx, pattern.Dy);
                yield break;
            }

            (int dx, int dy) = (pattern.Dx, pattern.Dy);
            var seen = new HashSet<(int, int)>();
            foreach (var v in new[]
                     {
                     (dx, dy), (-dx, dy), (dx, -dy), (-dx, -dy),
                     (dy, dx), (-dy, dx), (dy, -dx), (-dy, -dx)
                 })
                if (seen.Add(v))
                    yield return v;
        }
    }
}
