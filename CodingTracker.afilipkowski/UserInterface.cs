using CodingTracker.Controllers;
using Spectre.Console;
using static CodingTracker.Enums;

namespace CodingTracker;

internal class UserInterface
{
    private readonly DatabaseController db = new();
    private readonly CodingSessionController _codingSessionController = new();
    internal void MainMenu()
    {
        while (true)
        {
            Console.Clear();

            var menuChoice = AnsiConsole.Prompt(
                new SelectionPrompt<MenuOption>()
                .Title("What do you want to do?")
                .AddChoices(Enum.GetValues<MenuOption>()));

            switch (menuChoice)
            {
                case MenuOption.AddSession:
                    _codingSessionController.RecordSession(db);
                    break;
                case MenuOption.ViewSessions:
                    _codingSessionController.DisplaySessions(db);
                    break;
                case MenuOption.DeleteSession:
                    _codingSessionController.DeleteSession(db);
                    break;
                case MenuOption.UpdateSession:
                    _codingSessionController.RecordSession(db, update: true);
                    break;
            }

            AnsiConsole.MarkupLine("\nPress any key to continue");
            Console.ReadKey();
        }
    }
}