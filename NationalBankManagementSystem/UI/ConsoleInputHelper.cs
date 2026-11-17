using System.Globalization;

namespace NationalBankManagementSystem.UI;

public static class ConsoleInputHelper
{
    public static string ReadRequiredString(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(input))
            {
                return input;
            }
            PrintError("Input cannot be empty. Please try again.");
        }
    }

    public static int ReadInt(string prompt, int? min = null, int? max = null)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine()?.Trim();
            if (int.TryParse(input, out int result))
            {
                if (min.HasValue && result < min.Value)
                {
                    PrintError($"Value must be at least {min.Value}. Please try again.");
                    continue;
                }
                if (max.HasValue && result > max.Value)
                {
                    PrintError($"Value must be at most {max.Value}. Please try again.");
                    continue;
                }
                return result;
            }
            PrintError("Invalid number format. Please enter a valid integer.");
        }
    }

    public static DateOnly ReadDateOnly(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine()?.Trim();
            if (DateOnly.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            {
                return date;
            }
            if (DateOnly.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out var genericDate))
            {
                return genericDate;
            }
            PrintError("Invalid date format. Please enter date as yyyy-MM-dd (e.g. 2000-01-25).");
        }
    }

    public static string ReadEmail(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(input) && input.Contains('@') && input.Contains('.'))
            {
                return input;
            }
            PrintError("Invalid email format. Must contain '@' and domain (e.g. name@domain.com).");
        }
    }

    public static int ReadMenuChoice(int minChoice, int maxChoice)
    {
        while (true)
        {
            Console.Write("\nEnter choice: ");
            var input = Console.ReadLine()?.Trim();
            if (int.TryParse(input, out int choice) && choice >= minChoice && choice <= maxChoice)
            {
                return choice;
            }
            PrintError($"Invalid choice. Please enter a number between {minChoice} and {maxChoice}.");
        }
    }

    public static void Pause()
    {
        Console.WriteLine("\nPress any key to return to the menu...");
        if (!Console.IsInputRedirected)
        {
            Console.ReadKey(intercept: true);
        }
    }

    public static void PrintSuccess(string message)
    {
        var prev = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(message);
        Console.ForegroundColor = prev;
    }

    public static void PrintInfo(string message)
    {
        var prev = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(message);
        Console.ForegroundColor = prev;
    }

    public static void PrintError(string message)
    {
        var prev = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[Error] {message}");
        Console.ForegroundColor = prev;
    }
}
