Console.ForegroundColor = ConsoleColor.Cyan;
Console.BackgroundColor = ConsoleColor.White;
Console.WriteLine("Зеленый текст на белом фоне");
Console.ResetColor();
Console.WriteLine("Обычный");

Console.WriteLine(new string('_', 30));

Console.WriteLine("ConsoleColor:");
foreach (var i in Enum.GetValues<ConsoleColor>())
{
    Console.Write($"{i:X} \t {i:D} \t");
    Console.BackgroundColor = i;
    Console.WriteLine(i);
    Console.ResetColor();
}