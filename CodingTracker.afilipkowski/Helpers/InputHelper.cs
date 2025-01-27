using System.Globalization;
using CodingTracker.Models;
using Spectre.Console;

namespace CodingTracker.Helpers;

internal class InputHelper
{
    internal static DateTime GetDateInput(string message)
    {
        DateTime dateTime;
        string userInput;

        userInput = AnsiConsole.Ask<string>($"Enter the {message} time of the coding session (dd.mm.yyyy hh:mm) or type 'now' to use current time:");
        while (!DateTime.TryParseExact(userInput, "dd.MM.yyyy HH:mm", new CultureInfo("en-US"), DateTimeStyles.None, out dateTime))
        {
            if (userInput.ToLower() == "now")
            {
                dateTime = DateTime.Now;
                return dateTime;
            }
            AnsiConsole.MarkupLine("[red]Invalid input. Enter the date in (dd.mm.yyyy hh:mm) format or type 'now to enter the current time'.[/]");
            userInput = Console.ReadLine();
        }
        return dateTime;
    }

    internal static string CalculateDuration (DateTime t1, DateTime t2)
    {
        TimeSpan durationTs = t2 - t1;
        string duration = string.Format("{0:00}:{1:00}", durationTs.Hours, durationTs.Minutes);
        return duration;
    }
}