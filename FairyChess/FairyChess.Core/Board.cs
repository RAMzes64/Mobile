using System;
using System.Collections.Generic;
using System.Text;

namespace FairyChess.Core
{
    /// <summary>
    /// Прямоугольная доска площадью до <see cref="MaxCells"/> клеток с произвольными
    /// целыми координатами. Разреженное хранение: только занятые клетки.
    /// </summary>
    public sealed class Board
    {
        public const int MaxCells = 1000;

        private readonly Dictionary<Position, Piece> _pieces = new();

        public Position Min { get; }
        public Position Max { get; }
        public int Width => Max.X - Min.X + 1;
        public int Height => Max.Y - Min.Y + 1;

        public Board(Position min, Position max)
        {
            if (max.X < min.X || max.Y < min.Y)
                throw new ArgumentException($"Некорректные границы: min {min} должен быть не больше max {max}.");

            long width = (long)max.X - min.X + 1;
            long height = (long)max.Y - min.Y + 1;
            long cells = width * height;
            if (cells > MaxCells)
                throw new ArgumentException(
                    $"Площадь доски {width}x{height} = {cells} превышает лимит {MaxCells} клеток.");

            Min = min;
            Max = max;
        }

        /// <summary>Фигура на клетке; null — если клетка пуста или вне доски.</summary>
        public Piece? this[Position position] =>
            Contains(position) ? _pieces.GetValueOrDefault(position) : null;

        public bool Contains(Position position) =>
            position.X >= Min.X && position.X <= Max.X &&
            position.Y >= Min.Y && position.Y <= Max.Y;

        public IEnumerable<KeyValuePair<Position, Piece>> Pieces => _pieces;

        public void Add(Piece piece, Position position)
        {
            ArgumentNullException.ThrowIfNull(piece);

            if (!Contains(position))
                throw new ArgumentException($"Клетка {position} вне доски [{Min}..{Max}].");
            if (_pieces.ContainsKey(position))
                throw new ArgumentException($"Клетка {position} уже занята фигурой '{_pieces[position].Type.Name}'.");

            _pieces.Add(position, piece);
        }

        public bool Remove(Position position) => _pieces.Remove(position);

        /// <summary>Независимая копия доски (фигуры иммутабельны — копируются только ссылки).</summary>
        public Board Clone()
        {
            var clone = new Board(Min, Max);
            foreach (var (position, piece) in _pieces)
                clone._pieces.Add(position, piece);
            return clone;
        }
    }
}
