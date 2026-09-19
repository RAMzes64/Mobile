using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using FairyChess.Core;
using FairyChess.Wpf.Views;
using Microsoft.Win32;

namespace FairyChess.Wpf.ViewModels;

/// <summary>
/// Единственная точка связи UI ↔ FairyChess.Core.
/// Два режима: расстановка (_game == null) и партия (_game != null).
/// </summary>
public sealed class GameViewModel : ObservableObject
{
    private readonly PieceTypeCatalog _catalog = new();
    private readonly Dictionary<Position, Piece> _setupPieces = new();
    private readonly Dictionary<Position, SquareViewModel> _squareIndex = new();

    private Game? _game;
    private SquareViewModel? _selected;
    private bool _isSetupMode = true;
    private PieceType? _selectedPieceType;
    private Color _selectedPieceColor = Color.White;
    private string _minXText = "0";
    private string _minYText = "0";
    private string _boardWidthText = "8";
    private string _boardHeightText = "8";
    private string _statusText = "";
    private string _turnText = "";
    private string _message = "";
    private int _boardRows = 8;
    private int _boardColumns = 8;
    private double _squareFontSize = 26;

    public GameViewModel()
    {
        _catalog.RegisterStandardChess();
        foreach (PieceType type in _catalog.All)
            PieceTypes.Add(type);
        _selectedPieceType = _catalog.Get("Король");

        SquareClickCommand = new RelayCommand(parameter => OnSquareClick(parameter as SquareViewModel));
        StartGameCommand = new RelayCommand(_ => StartGame(), _ => IsSetupMode);
        UndoCommand = new RelayCommand(_ => Undo(), _ => IsGameMode);
        ResignWhiteCommand = new RelayCommand(_ => Resign(Color.White), _ => IsGameMode);
        ResignBlackCommand = new RelayCommand(_ => Resign(Color.Black), _ => IsGameMode);
        NewGameCommand = new RelayCommand(_ => BackToSetup(), _ => IsGameMode);
        AddPieceTypeCommand = new RelayCommand(_ => AddPieceType());
        LoadTypesCommand = new RelayCommand(_ => LoadTypes());

        Refresh();
    }

    // --- Коллекции ---

    public ObservableCollection<SquareViewModel> Squares { get; } = new();

    public ObservableCollection<PieceType> PieceTypes { get; } = new();

    public ObservableCollection<string> MoveHistory { get; } = new();

    public IReadOnlyList<Color> PieceColors { get; } = new[] { Color.White, Color.Black };

    // --- Режимы ---

    public bool IsSetupMode
    {
        get => _isSetupMode;
        private set
        {
            if (Set(ref _isSetupMode, value))
                OnPropertyChanged(nameof(IsGameMode));
        }
    }

    public bool IsGameMode => !IsSetupMode;

    // --- Расстановка ---

    public PieceType? SelectedPieceType
    {
        get => _selectedPieceType;
        set => Set(ref _selectedPieceType, value);
    }

    public Color SelectedPieceColor
    {
        get => _selectedPieceColor;
        set => Set(ref _selectedPieceColor, value);
    }

    public string MinXText { get => _minXText; set { if (Set(ref _minXText, value) && IsSetupMode) Refresh(); } }
    public string MinYText { get => _minYText; set { if (Set(ref _minYText, value) && IsSetupMode) Refresh(); } }
    public string BoardWidthText { get => _boardWidthText; set { if (Set(ref _boardWidthText, value) && IsSetupMode) Refresh(); } }
    public string BoardHeightText { get => _boardHeightText; set { if (Set(ref _boardHeightText, value) && IsSetupMode) Refresh(); } }

    // --- Состояние ---

    public string StatusText { get => _statusText; private set => Set(ref _statusText, value); }
    public string TurnText { get => _turnText; private set => Set(ref _turnText, value); }
    public string Message { get => _message; private set => Set(ref _message, value); }
    public int BoardRows { get => _boardRows; private set => Set(ref _boardRows, value); }
    public int BoardColumns { get => _boardColumns; private set => Set(ref _boardColumns, value); }
    public double SquareFontSize { get => _squareFontSize; private set => Set(ref _squareFontSize, value); }

    // --- Команды ---

    public ICommand SquareClickCommand { get; }
    public ICommand StartGameCommand { get; }
    public ICommand UndoCommand { get; }
    public ICommand ResignWhiteCommand { get; }
    public ICommand ResignBlackCommand { get; }
    public ICommand NewGameCommand { get; }
    public ICommand AddPieceTypeCommand { get; }
    public ICommand LoadTypesCommand { get; }

    // --- Клик по клетке: расстановка или ход ---

    private void OnSquareClick(SquareViewModel? square)
    {
        if (square is null)
            return;

        if (_game is null)
        {
            // Режим расстановки: клик по клетке — поставить/убрать фигуру.
            if (_setupPieces.ContainsKey(square.Position))
                _setupPieces.Remove(square.Position);
            else if (SelectedPieceType is { } type)
                _setupPieces.Add(square.Position, new Piece(type, SelectedPieceColor));

            ShowMessage("");
            Refresh();
            return;
        }

        // Партия: клик по подсвеченной цели — ход.
        if (_selected is not null && square.IsLegalTarget)
        {
            TryMove(_selected, square);
            return;
        }

        if (_game.Status != GameStatus.InProgress)
        {
            ClearSelection();
            ShowMessage("Игра окончена — нажмите «Новая партия».");
            return;
        }

        Piece? piece = _game.Board[square.Position];
        if (piece is not null && piece.Color == _game.Turn)
            Select(square);
        else
        {
            ClearSelection();
            if (piece is not null)
                ShowMessage($"Сейчас ход стороны: {SideName(_game.Turn)}.");
        }
    }

    private void Select(SquareViewModel square)
    {
        IReadOnlyList<Position> targets = _game!.LegalTargets(square.Position);
        if (targets.Count == 0)
        {
            ShowMessage("У этой фигуры нет допустимых ходов.");
            return;
        }

        ClearSelection();
        _selected = square;
        square.IsSelected = true;
        foreach (Position target in targets)
            if (_squareIndex.TryGetValue(target, out SquareViewModel? vm))
                vm.IsLegalTarget = true;
        ShowMessage("");
    }

    private void TryMove(SquareViewModel from, SquareViewModel to)
    {
        if (_game!.TryMakeMove(from.Position, to.Position, out string? error))
        {
            _selected = null;
            ShowMessage("");
            Refresh();
        }
        else
        {
            ClearSelection();
            ShowMessage(error ?? "Недопустимый ход.");
        }
    }

    private void ClearSelection()
    {
        if (_selected is not null)
        {
            _selected.IsSelected = false;
            _selected = null;
        }

        foreach (SquareViewModel square in Squares)
            square.IsLegalTarget = false;
    }

    // --- Управление партией ---

    private void StartGame()
    {
        if (!int.TryParse(MinXText, out int minX)) { ShowMessage("«Мин. X» должен быть целым числом."); return; }
        if (!int.TryParse(MinYText, out int minY)) { ShowMessage("«Мин. Y» должен быть целым числом."); return; }
        if (!int.TryParse(BoardWidthText, out int width) || width < 1) { ShowMessage("«Ширина» должна быть целым числом ≥ 1."); return; }
        if (!int.TryParse(BoardHeightText, out int height) || height < 1) { ShowMessage("«Высота» должна быть целым числом ≥ 1."); return; }

        if (!_setupPieces.Values.Any(piece => piece.Color == Color.White)) { ShowMessage("У белых должна быть хотя бы одна фигура."); return; }
        if (!_setupPieces.Values.Any(piece => piece.Color == Color.Black)) { ShowMessage("У чёрных должна быть хотя бы одна фигура."); return; }

        try
        {
            var board = new Board(new Position(minX, minY), new Position(minX + width - 1, minY + height - 1));
            foreach (var (position, piece) in _setupPieces)
                board.Add(piece, position);

            _game = new Game(board, _catalog); // Game клонирует доску сам
        }
        catch (ArgumentException ex)
        {
            ShowMessage(ex.Message); // площадь > 1000, фигура вне доски и т.п.
            return;
        }

        _selected = null;
        IsSetupMode = false;
        ShowMessage("");
        Refresh();
    }

    private void Undo()
    {
        try
        {
            _game!.Undo();
        }
        catch (InvalidOperationException ex)
        {
            ShowMessage(ex.Message);
            return;
        }

        _selected = null;
        ShowMessage("");
        Refresh();
    }

    private void Resign(Color side)
    {
        try
        {
            _game!.Resign(side);
        }
        catch (InvalidOperationException ex)
        {
            ShowMessage(ex.Message);
            return;
        }

        _selected = null;
        Refresh();
    }

    private void BackToSetup()
    {
        _game = null;
        _selected = null;
        IsSetupMode = true;
        ShowMessage("Внесите изменения в расстановку и начните новую партию.");
        Refresh();
    }

    // --- Динамические типы фигур ---

    // Прагматичное исключение из чистого MVVM: единственное View, известное VM.
    private void AddPieceType()
    {
        var dialog = new PieceTypeEditorDialog(PieceTypes.Select(type => type.Name).ToList())
        {
            Owner = Application.Current.MainWindow
        };

        if (dialog.ShowDialog() != true || dialog.Result is null)
            return;

        try
        {
            _catalog.Register(dialog.Result);
            PieceTypes.Add(dialog.Result);
            ShowMessage($"Тип «{dialog.Result.Name}» добавлен в каталог.");
        }
        catch (ArgumentException ex)
        {
            ShowMessage(ex.Message); // дубликат имени, некорректные шаблоны
        }
    }

    private void LoadTypes()
    {
        var dialog = new OpenFileDialog
        {
            Title = "Загрузка типов фигур",
            Filter = "JSON-файлы (*.json)|*.json|Все файлы (*.*)|*.*"
        };
        if (dialog.ShowDialog() != true)
            return;

        try
        {
            PieceTypeLoader.LoadFromFile(_catalog, dialog.FileName);
        }
        catch (Exception ex) when (ex is JsonException or ArgumentException or IOException)
        {
            ShowMessage($"Не удалось загрузить типы: {ex.Message}");
            return;
        }

        foreach (PieceType type in _catalog.All)
            if (!PieceTypes.Contains(type))
                PieceTypes.Add(type);

        ShowMessage($"Типы из «{Path.GetFileName(dialog.FileName)}» загружены.");
    }

    // --- Полная перерисовка состояния (для пошаговой игры это дёшево) ---

    private void Refresh()
    {
        int minX, minY, width, height;
        IReadOnlyDictionary<Position, Piece> pieces;

        if (_game is null)
        {
            minX = ParseOr(MinXText, 0);
            minY = ParseOr(MinYText, 0);
            width = Math.Max(1, ParseOr(BoardWidthText, 8));
            height = Math.Max(1, ParseOr(BoardHeightText, 8));
            pieces = _setupPieces;
        }
        else
        {
            minX = _game.Board.Min.X;
            minY = _game.Board.Min.Y;
            width = _game.Board.Width;
            height = _game.Board.Height;
            pieces = _game.Board.Pieces.ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        Squares.Clear();
        _squareIndex.Clear();
        for (int y = minY + height - 1; y >= minY; y--)          // верхняя строка — максимальный Y
        {
            for (int x = minX; x < minX + width; x++)
            {
                var square = new SquareViewModel(x, y, Math.Abs((x + y) % 2) == 1);
                square.SetPiece(pieces.GetValueOrDefault(new Position(x, y)));
                Squares.Add(square);
                _squareIndex[new Position(x, y)] = square;
            }
        }

        BoardRows = height;
        BoardColumns = width;
        SquareFontSize = Math.Clamp((int)(320.0 / Math.Max(width, height)), 8, 26);

        if (_game is null)
        {
            StatusText = "Режим расстановки";
            TurnText = $"Фигур на доске: {_setupPieces.Count}";
            MoveHistory.Clear();
        }
        else
        {
            StatusText = _game.Status switch
            {
                GameStatus.InProgress => "Партия идёт",
                GameStatus.WhiteWin => "Победа белых (мат или сдача)",
                GameStatus.BlackWin => "Победа чёрных (мат или сдача)",
                _ => "Ничья (пат)"
            };

            bool check = _game.Status == GameStatus.InProgress && _game.IsInCheck(_game.Turn);
            TurnText = _game.Status == GameStatus.InProgress
                ? $"Ход: {SideName(_game.Turn)}{(check ? " — ШАХ!" : "")}"
                : "Партия завершена";

            MoveHistory.Clear();
            foreach (MoveRecord record in _game.History)
                MoveHistory.Add(
                    $"{record.MovedBefore.Type.Symbol} {record.Move.From} → {record.Move.To}" +
                    (record.Captured is null ? "" : $" ×{record.Captured.Type.Symbol}"));
        }

        CommandManager.InvalidateRequerySuggested();
    }

    private void ShowMessage(string message) => Message = message;

    private static int ParseOr(string? text, int fallback) =>
        int.TryParse(text, out int value) ? value : fallback;

    private static string SideName(Color side) => side == Color.White ? "белые" : "чёрные";
}