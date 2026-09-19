using System.Windows.Media;
using FairyChess.Core;
using Color = FairyChess.Core.Color;

namespace FairyChess.Wpf.ViewModels;

/// <summary>Одна клетка доски: координаты, содержимое и подсветка.</summary>
public sealed class SquareViewModel : ObservableObject
{
    private string _symbol = "";
    private Brush _pieceBrush = Brushes.Transparent;
    private bool _isSelected;
    private bool _isLegalTarget;

    public SquareViewModel(int x, int y, bool isDark) => (X, Y, IsDark) = (x, y, isDark);

    public int X { get; }
    public int Y { get; }
    public bool IsDark { get; }

    public Position Position => new(X, Y);
    public string Coordinates => $"{X}, {Y}";

    public string Symbol { get => _symbol; private set => Set(ref _symbol, value); }

    public Brush PieceBrush { get => _pieceBrush; private set => Set(ref _pieceBrush, value); }

    public bool IsSelected { get => _isSelected; set => Set(ref _isSelected, value); }

    public bool IsLegalTarget { get => _isLegalTarget; set => Set(ref _isLegalTarget, value); }

    public void SetPiece(Piece? piece)
    {
        Symbol = piece is null ? "" : piece.Type.Symbol.ToString();
        PieceBrush = piece is null
            ? Brushes.Transparent
            : piece.Color == Color.White ? Brushes.White : Brushes.Black;
    }
}