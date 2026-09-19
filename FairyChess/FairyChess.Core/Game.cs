using System;
using System.Collections.Generic;
using System.Text;

namespace FairyChess.Core
{
    public enum GameStatus
    {
        InProgress,
        WhiteWin,
        BlackWin,
        Draw
    }

    /// <summary>
    /// Партия: очерёдность ходов, шах/мат/пат, история и Undo.
    /// В партии доской управляет только Game (входная доска клонируется).
    /// </summary>
    public sealed class Game
    {
        private readonly PieceTypeCatalog _catalog;
        private readonly List<MoveRecord> _history = new();

        public Game(Board board, PieceTypeCatalog catalog, Color startingSide = Color.White)
        {
            ArgumentNullException.ThrowIfNull(board);
            ArgumentNullException.ThrowIfNull(catalog);

            _catalog = catalog;
            Board = board.Clone();
            Turn = startingSide;

            ValidatePromotions();
            Status = RecalculateStatus();
        }

        public Board Board { get; }
        public Color Turn { get; private set; }
        public GameStatus Status { get; private set; }

        /// <summary>История выполненных ходов (только чтение, для списка ходов в UI).</summary>
        public IReadOnlyList<MoveRecord> History => _history;

        public bool IsInCheck(Color side) => IsRoyalAttacked(Board, side);

        /// <summary>Легальные ходы фигуры. Пусто, если фигуры нет, она не стороны хода или игра окончена.</summary>
        public IReadOnlyList<Move> LegalMoves(Position from)
        {
            var moves = new List<Move>();
            if (Status != GameStatus.InProgress)
                return moves;

            Piece? piece = Board[from];
            if (piece is null || piece.Color != Turn)
                return moves;

            foreach (var move in MoveGenerator.PseudoLegal(Board, from))
                if (IsLegal(move))
                    moves.Add(move);
            return moves;
        }

        /// <summary>Клетки, доступные фигуре (для подсветки в UI).</summary>
        public IReadOnlyList<Position> LegalTargets(Position from) =>
            LegalMoves(from).Select(move => move.To).ToList();

        public bool TryMakeMove(Position from, Position to, out string? error)
        {
            if (Status != GameStatus.InProgress)
            {
                error = "Игра уже окончена.";
                return false;
            }

            Piece? piece = Board[from];
            if (piece is null)
            {
                error = $"На клетке {from} нет фигуры.";
                return false;
            }
            if (piece.Color != Turn)
            {
                error = $"Сейчас ход стороны {Turn}.";
                return false;
            }

            var move = new Move(from, to);
            if (!LegalMoves(from).Contains(move))
            {
                error = "Недопустимый ход.";
                return false;
            }

            MakeMove(move);
            error = null;
            return true;
        }

        /// <summary>
        /// Отменяет последний ход и возвращает игру в InProgress.
        /// Восстанавливает взятую фигуру, флаг HasMoved и отменяет превращение.
        /// </summary>
        public void Undo()
        {
            if (_history.Count == 0)
                throw new InvalidOperationException("История ходов пуста — нечего отменять.");

            MoveRecord record = _history[^1];
            _history.RemoveAt(_history.Count - 1);

            Board.Remove(record.Move.To);
            Board.Add(record.MovedBefore, record.Move.From);
            if (record.Captured is not null)
                Board.Add(record.Captured, record.Move.To);

            Turn = Turn.Opponent();
            Status = GameStatus.InProgress;
        }

        public void Resign(Color side)
        {
            if (Status != GameStatus.InProgress)
                throw new InvalidOperationException("Игра уже окончена.");

            Status = side == Color.White ? GameStatus.BlackWin : GameStatus.WhiteWin;
        }

        private void MakeMove(Move move)
        {
            Piece piece = Board[move.From]!;
            Piece? captured = Board[move.To];

            _history.Add(new MoveRecord(move, piece, captured));

            Board.Remove(move.From);
            if (captured is not null)
                Board.Remove(move.To);
            Board.Add(PromoteIfDue(piece.AsMoved(), move.To), move.To);

            Turn = Turn.Opponent();
            Status = RecalculateStatus();
        }

        /// <summary>Легальность: после применения хода к копии ни один король стороны не под атакой.</summary>
        private bool IsLegal(Move move)
        {
            Piece piece = Board[move.From]!;

            var simulation = Board.Clone();
            simulation.Remove(move.From);
            if (simulation[move.To] is not null)
                simulation.Remove(move.To);
            simulation.Add(piece.AsMoved(), move.To);

            return !IsRoyalAttacked(simulation, piece.Color);
        }

        /// <summary>Превращение на дальней горизонтали: белые — Max.Y, чёрные — Min.Y.</summary>
        private Piece PromoteIfDue(Piece piece, Position to)
        {
            string? promotesTo = piece.Type.PromotesTo;
            if (promotesTo is null)
                return piece;

            bool reached = piece.Color == Color.White ? to.Y == Board.Max.Y : to.Y == Board.Min.Y;
            return reached
                ? new Piece(_catalog.Get(promotesTo), piece.Color, HasMoved: true)
                : piece;
        }

        private GameStatus RecalculateStatus()
        {
            if (HasAnyLegalMove(Turn))
                return GameStatus.InProgress;

            return IsInCheck(Turn)                                                  // мат
                ? Turn == Color.White ? GameStatus.BlackWin : GameStatus.WhiteWin
                : GameStatus.Draw;                                                  // пат
        }

        private bool HasAnyLegalMove(Color side)
        {
            foreach (var (from, piece) in Board.Pieces)
            {
                if (piece.Color != side)
                    continue;
                foreach (var move in MoveGenerator.PseudoLegal(Board, from))
                    if (IsLegal(move))
                        return true;
            }
            return false;
        }

        private static bool IsRoyalAttacked(Board board, Color side)
        {
            Color opponent = side.Opponent();
            foreach (var (position, piece) in board.Pieces)
                if (piece.Color == side && piece.Type.IsRoyal &&
                    MoveGenerator.IsAttacked(board, position, opponent))
                    return true;
            return false;
        }

        private void ValidatePromotions()
        {
            foreach (var (_, piece) in Board.Pieces)
                if (piece.Type.PromotesTo is { } target && !_catalog.Contains(target))
                    throw new ArgumentException(
                        $"Фигура '{piece.Type.Name}' превращается в '{target}', но этот тип не зарегистрирован в каталоге.");
        }
    }
}
