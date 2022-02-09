using System.Text;

namespace TestConsoleApp.Utils;

public static class InputUtil
{
    private static string ConsoleReadHidden()
    {
        var input = new StringBuilder();
        var finished = false;
    
        while (!finished)
        {
            var key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.Enter:
                    finished = true;
                    break;
                
                case ConsoleKey.Backspace:
                    if (input.Length > 0)
                    {
                        input.Remove(input.Length - 1, 1);
                    }
                    
                    break;
                
                default:
                    input.Append(key.KeyChar);
                    break;
            }
        }
    
        return input.ToString();
    }

    public static string? InputReadField(string prefix, ConsoleColor prefixColor = ConsoleColor.White)
    {
        Console.ForegroundColor = prefixColor;
        Console.Write(prefix);
        Console.ResetColor();
        
        return Console.ReadLine();
    }
    
    public static string InputReadFieldPassword(string prefix, ConsoleColor prefixColor = ConsoleColor.White)
    {
        Console.ForegroundColor = prefixColor;
        Console.Write(prefix);
        Console.ResetColor();
        
        return ConsoleReadHidden();
    }
}