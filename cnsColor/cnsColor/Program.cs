
Console.ForegroundColor = ConsoleColor.Red;
Console.BackgroundColor = ConsoleColor.Green;
Console.WriteLine("Красный текст, на зелённом фоне");
Console.ResetColor();
Console.WriteLine("Usual text");
Console.WriteLine();
Console.WriteLine(new string('-', 80));

// Q: Как распечатать все доступные цвета в столбик

Console.WriteLine("ConsoleColor:");
foreach (var i in Enum.GetValues<ConsoleColor>())
{
    Console.Write($"{i:X} | {i,2:D} | ");
    Console.BackgroundColor = i;
    Console.WriteLine(i);
    Console.ResetColor();
}