using CodingTracker.Helpers;
using CodingTracker.Models;
using Spectre.Console;

namespace CodingTracker.Controllers;

internal class CodingSessionController
{
    public CodingSession? CreateSession()
    {
        DateTime startTime = InputHelper.GetDateInput("start");
        DateTime endTime = InputHelper.GetDateInput("end");

        if (startTime > endTime)
        {
            return null;
        }
        else
        {
            return new CodingSession
            {
                StartTime = startTime.ToString("dd.MM.yyyy HH:mm"),
                EndTime = endTime.ToString("dd.MM.yyyy HH:mm"),
                Duration = InputHelper.CalculateDuration(startTime, endTime)
            };
        }
    }

    public void RecordSession(DatabaseController db)
    {
        var session = CreateSession();
        if (session != null)
        {
            db.AddSession(session);
            AnsiConsole.MarkupLine("[green]Session added successfully[/]");
        }
        else
        {
            AnsiConsole.MarkupLine("[red]End time cannot be earlier than start time.[/]");
        }
    }

    public void UpdateSession(DatabaseController db)
    {
        DisplaySessions(db);
        int id = AnsiConsole.Ask<int>("\nEnter the [green]ID[/] of the session you want to update or enter 0 to cancel:");
        if (id == 0)
            return;

        var session = CreateSession();
        if (session != null)
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
            AnsiConsole.MarkupLine("[red]End time cannot be earlier than start time.[/]");
        }
    }

    public void DisplaySessions(DatabaseController db)
    {
        List<CodingSession> sessions = db.GetSessions();
        if (sessions.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No sessions recorded yet.[/]");
            return;
        }
        var table = InputHelper.GenerateTable(sessions);
        AnsiConsole.Write(table);

        TimeSpan totalTime = CalculateTotalTime(sessions);
        TimeSpan averageTime = new(totalTime.Ticks / sessions.Count);
        AnsiConsole.MarkupLine($"Total time spent coding: [green]{totalTime.Hours} hours and {totalTime.Minutes} minutes[/]");
        AnsiConsole.MarkupLine($"Average time spent coding: [green]{averageTime.Hours} hours and {averageTime.Minutes} minutes[/]");
    }

    public void DeleteSession(DatabaseController db)
    {
        DisplaySessions(db);
        int id = AnsiConsole.Ask<int>("\nEnter the [green]ID[/] of the session you want to delete or enter 0 to cancel:");
        if (id == 0) 
            return;

        if (db.DeleteSession(id))
        {
            AnsiConsole.MarkupLine("[green]Session deleted successfully.[/]");
        }
        else
        {
            AnsiConsole.MarkupLine("[red]Session not found.[/]");
        }
    }

    private TimeSpan CalculateTotalTime(List<CodingSession> sessions)
    {
        TimeSpan totalTime = new TimeSpan();
        foreach (var session in sessions)
        {
            totalTime += TimeSpan.Parse(session.Duration);
        }
        return totalTime;
    }
}