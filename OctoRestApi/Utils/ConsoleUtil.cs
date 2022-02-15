// OctoRestApi copyright 2022 Danny Glover.

namespace OctoRestApi.Utils;

public static class ConsoleUtil
{
	public static void WriteLine(string text, ConsoleColor textColor)
	{
		Console.ForegroundColor = textColor;

		Console.WriteLine(text);
		Console.ResetColor();
	}

	public static void WriteLine(string text, ConsoleColor textColor, ConsoleColor backgroundColor)
	{
		Console.BackgroundColor = backgroundColor;
		Console.ForegroundColor = textColor;

		Console.WriteLine(text);
		Console.ResetColor();
	}
}
