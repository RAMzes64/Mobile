using System.Collections.ObjectModel;
using System.Windows;
using FairyChess.Core;

namespace FairyChess.Wpf.Views;

public partial class PieceTypeEditorDialog : Window
{
    private const string NoPromotion = "(нет)";

    /// <summary>Редактируемые шаблоны ходов.</summary>
    public ObservableCollection<PatternRow> Patterns { get; } = new();

    /// <summary>Тип, собранный по кнопке OK; null — если диалог отменён.</summary>
    public PieceType? Result { get; private set; }

    public PieceTypeEditorDialog(IReadOnlyList<string> knownTypeNames)
    {
        InitializeComponent();

        PromotesToBox.ItemsSource = new[] { NoPromotion }.Concat(knownTypeNames).ToArray();
        PromotesToBox.SelectedIndex = 0;

        Patterns.Add(new PatternRow { Dx = 1, Dy = 2, Mirror = true }); // стартовый пример — «конь»
        DataContext = this;
    }

    private void AddPattern(object sender, RoutedEventArgs e) => Patterns.Add(new PatternRow());

    private void RemovePattern(object sender, RoutedEventArgs e)
    {
        if (PatternsGrid.SelectedItem is PatternRow row)
            Patterns.Remove(row);
    }

    private void OkClick(object sender, RoutedEventArgs e)
    {
        string name = NameBox.Text.Trim();
        string symbol = SymbolBox.Text.Trim();

        if (name.Length == 0) { Warn("Укажите имя типа фигуры."); return; }
        if (symbol.Length == 0) { Warn("Укажите символ фигуры (один знак)."); return; }

        string? promotesTo = PromotesToBox.SelectedItem as string;
        promotesTo = promotesTo == NoPromotion ? null : promotesTo;

        var type = new PieceType(
            name,
            symbol[0],
            Patterns.Select(p => new MovePattern(p.Dx, p.Dy, p.MaxSteps, p.Mirror, p.CaptureOnly, p.QuietOnly)).ToList(),
            IsRoyalCheck.IsChecked == true,
            DoubleFirstCheck.IsChecked == true,
            promotesTo);

        try
        {
            type.Validate(); // (0,0), MaxSteps < 1, QuietOnly+CaptureOnly и т.п.
        }
        catch (ArgumentException ex)
        {
            Warn(ex.Message);
            return;
        }

        Result = type;
        DialogResult = true;
    }

    private static void Warn(string message) =>
        MessageBox.Show(message, "Некорректный тип фигуры", MessageBoxButton.OK, MessageBoxImage.Warning);
}

/// <summary>Редактируемая строка таблицы шаблонов (привязана к DataGrid напрямую).</summary>
public sealed class PatternRow
{
    public int Dx { get; set; } = 1;
    public int Dy { get; set; } = 1;
    public int MaxSteps { get; set; } = 1;
    public bool Mirror { get; set; }
    public bool CaptureOnly { get; set; }
    public bool QuietOnly { get; set; }
}