using System.Configuration;
using CodingTracker.Models;
using Dapper;
using Microsoft.Data.Sqlite;

namespace CodingTracker.Controllers;

internal class DatabaseController
{
    private string connectionString = ConfigurationManager.ConnectionStrings["databaseConn"].ConnectionString;
    private string tableName = ConfigurationManager.AppSettings["tableName"];

    internal DatabaseController()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            var command = $"CREATE TABLE IF NOT EXISTS {tableName}(Id INTEGER PRIMARY KEY, StartTime TEXT, EndTime TEXT, Duration TEXT)";
            connection.Execute(command);
        }
    }

    internal void AddSession(CodingSession session)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            var command = $"INSERT INTO {tableName} (StartTime, EndTime, Duration) VALUES (@start_time, @end_time, @duration)";
            connection.Execute(command, new { start_time = session.StartTime, end_time = session.EndTime, duration = session.Duration });
        }
    }

    internal List<CodingSession> GetSessions()
    {
        List<CodingSession> sessions;
        using (var connection = new SqliteConnection(connectionString))
        {
            var command = $"SELECT * FROM {tableName}";
            sessions = connection.Query<CodingSession>(command).ToList();
        }
        return sessions;
    }

    internal bool DeleteSession(int id)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            var command = $"DELETE FROM {tableName} WHERE Id = @id";
            return connection.Execute(command, new { id }) > 0;
        }
    }

    internal bool UpdateSession(int id, CodingSession session)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            var command = $"UPDATE {tableName} SET StartTime=@StartTime, EndTime=@EndTime, Duration=@Duration WHERE Id = @Id";
            return connection.Execute(command, new { session.StartTime, session.EndTime, session.Duration, Id = id }) > 0;
        }
    }
}