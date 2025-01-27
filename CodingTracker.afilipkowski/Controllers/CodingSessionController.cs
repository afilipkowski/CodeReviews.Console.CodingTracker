using CodingTracker.Helpers;
using CodingTracker.Models;
using Spectre.Console;

namespace CodingTracker.Controllers;

internal class CodingSessionController
{
    public void RecordSession(DatabaseController db, bool update = false)
    {
        DateTime startTime;
        DateTime endTime;
        int id = 0;

        if (update)
        {
            DisplaySessions(db);
            id = AnsiConsole.Ask<int>("Enter the [green]ID[/] of the session you want to update:");
        }

        startTime = InputHelper.GetDateInput("start");
        endTime = InputHelper.GetDateInput("end");

        if (startTime > endTime)
        {
            AnsiConsole.MarkupLine("[red]End time cannot be before start time.[/]");
            return;
        }

        var session = new CodingSession
        {
            StartTime = startTime.ToString("dd.MM.yyyy HH:mm"),
            EndTime = endTime.ToString("dd.MM.yyyy HH:mm"),
            Duration = InputHelper.CalculateDuration(startTime, endTime)
        };

        if (update)
        {
            if (db.UpdateSession(id, session))
            {
                AnsiConsole.MarkupLine("[green]Session updated successfully.[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Session not found.[/]");
            }
        }
        else
        {
            db.AddSession(session);
            AnsiConsole.MarkupLine("[green]Session recorded successfully.[/]");
        }
    }

    public void DisplaySessions(DatabaseController db)
    {
        List<CodingSession> sessions = db.GetSessions();
        TimeSpan totalTime = new TimeSpan();

        if (sessions.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No sessions recorded yet.[/]");
            return;
        }

        var table = new Table();
        table.Border = TableBorder.Rounded;
        table.AddColumn("[green]Session ID[/]");
        table.AddColumn("[green]Start Time[/]");
        table.AddColumn("[green]End Time[/]");
        table.AddColumn("[green]Duration (hh:mm)[/]");

        foreach (var session in sessions)
        {
            table.AddRow(
               session.Id.ToString(),
               session.StartTime,
               session.EndTime,
               session.Duration
               );

            totalTime += TimeSpan.Parse(session.Duration);
        }

        var averageTime = new TimeSpan(totalTime.Ticks / sessions.Count);

        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine($"Total time spent coding: [green]{totalTime.Hours} hours and {totalTime.Minutes} minutes[/]");
        AnsiConsole.MarkupLine($"Average time spent coding: [green]{averageTime.Hours} hours and {averageTime.Minutes} minutes[/]");
    }

    public void DeleteSession(DatabaseController db)
    {
        DisplaySessions(db);
        int id = AnsiConsole.Ask<int>("Enter the [green]ID[/] of the session you want to delete:");
        if (db.DeleteSession(id))
        {
            AnsiConsole.MarkupLine("[green]Session deleted successfully.[/]");
        }
        else
        {
            AnsiConsole.MarkupLine("[red]Session not found.[/]");
        }
    }
}