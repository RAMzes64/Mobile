using FairyChess.Core;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Каталог: стандарт + кастомный «Кентавр» (конь + ладья) + тип из JSON в рантайме
            var catalog = new PieceTypeCatalog();
            catalog.RegisterStandardChess();
            catalog.Register(new PieceType("Кентавр", 'C', new[]
            {
                new MovePattern(1, 2, Mirror: true),
                new MovePattern(1, 0, int.MaxValue, Mirror: true)
            }));
            PieceTypeLoader.Load(catalog, "[ { \"name\": \"Лучник\", \"symbol\": \"A\", " +
                "\"patterns\": [ { \"dx\": 0, \"dy\": 1, \"maxSteps\": 3, \"quietOnly\": true } ] } ]");

            // Доска 12×8 и произвольная расстановка
            var board = new Board(new Position(0, 0), new Position(11, 7));
            board.Add(new Piece(catalog.Get("Король"), Color.White), new Position(5, 0));
            board.Add(new Piece(catalog.Get("Кентавр"), Color.White), new Position(2, 3));
            board.Add(new Piece(catalog.Get("Король"), Color.Black), new Position(6, 7));
            board.Add(new Piece(catalog.Get("Ладья"), Color.Black), new Position(11, 7));

            var game = new Game(board, catalog);

            Console.WriteLine(string.Join(" ", game.LegalTargets(new Position(2, 3)))); // (1,5) (2,5) ... (2,0)

            if (!game.TryMakeMove(new Position(2, 3), new Position(11, 3), out var error))
                Console.WriteLine(error);

            Console.WriteLine($"{game.Status}, ход: {game.Turn}, шах чёрным: {game.IsInCheck(Color.Black)}");
            game.Undo(); // позиция полностью восстановлена
        }
    }
}
