# Coding Tracker

Coding Tracker is a CLI app written in C# that allows user to record coding sessions and store their details in a SQLite database.

## Tech stack
- C#
- Dapper
- [Spectre.Console](https://github.com/spectreconsole/spectre.console)
- SQLite

## Features
- The app uses SQLite database to store data.
- The user can record their coding session by inserting date and time of start and end of the session. The app takes care of calculating the duration and total coding time as well as average coding time per session.
- The user can delete and update existing records.
- The user can type “now” instead of the date to use the current date, so the app can be used as a timer for an ongoing session.

## Running the app
To run the application you need to have .NET installed on your machine.

1. Clone the repository:
    ```
    git clone https://github.com/afilipkowski/CodeReviews.Console.CodingTracker
    cd CodeReviews.Console.CodingTracker/CodingTracker.afilipkowski
    ```
2. Build the project:
    ```
    dotnet build
    ```
3. Run the application:
    ```
    dotnet run
    ```