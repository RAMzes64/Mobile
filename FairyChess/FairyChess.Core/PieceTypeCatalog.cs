using System;
using System.Collections.Generic;
using System.Text;

namespace FairyChess.Core
{
    /// <summary>Реестр типов фигур, доступных в рантайме. Единственная точка входа новых типов.</summary>
    public sealed class PieceTypeCatalog
    {
        private readonly Dictionary<string, PieceType> _types = new(StringComparer.Ordinal);

        public IEnumerable<PieceType> All => _types.Values;

        public void Register(PieceType type)
        {
            ArgumentNullException.ThrowIfNull(type);
            type.Validate();

            if (_types.ContainsKey(type.Name))
                throw new ArgumentException($"Тип фигуры '{type.Name}' уже зарегистрирован.");

            _types.Add(type.Name, type);
        }

        public bool Contains(string name) => _types.ContainsKey(name);

        public PieceType Get(string name) =>
            _types.TryGetValue(name, out var type)
                ? type
                : throw new KeyNotFoundException($"Тип фигуры '{name}' не зарегистрирован в каталоге.");

        /// <summary>Регистрирует стандартный набор: король, ферзь, ладья, слон, конь, пешка.</summary>
        public void RegisterStandardChess()
        {
            Register(StandardPieces.King);
            Register(StandardPieces.Queen);
            Register(StandardPieces.Rook);
            Register(StandardPieces.Bishop);
            Register(StandardPieces.Knight);
            Register(StandardPieces.Pawn);
        }
    }
}
