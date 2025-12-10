using System;
using System.Data.SQLite;

class Program
{
    private const string CS = "Data Source=bank.db;Version=3;";

    static void Main()
    {
        InitDb();

        Console.WriteLine("До перевода:");
        Show();

        bool ok = Transfer(1, 2, 600m);   // попробуй 600 или 1500 — увидишь откат

        Console.WriteLine(ok ? "Перевод прошёл" : "Перевод откатился");

        Console.WriteLine("После перевода:");
        Show();

        Console.ReadKey();
    }

    static void InitDb()
    {
        using var con = new SQLiteConnection(CS);
        con.Open();
        new SQLiteCommand("CREATE TABLE IF NOT EXISTS Acc(Id INTEGER PRIMARY KEY, Balance REAL NOT NULL DEFAULT 1000)", con).ExecuteNonQuery();
        new SQLiteCommand("INSERT OR IGNORE INTO Acc(Id, Balance) VALUES(1,1000),(2,500)", con).ExecuteNonQuery();
    }

    static bool Transfer(int from, int to, decimal sum)
    {
        using var con = new SQLiteConnection(CS);
        con.Open();
        using var tr = con.BeginTransaction();
        try
        {
            // списание
            var cmd1 = con.CreateCommand();
            cmd1.Transaction = tr;
            cmd1.CommandText = "UPDATE Acc SET Balance = Balance - @sum WHERE Id = @id AND Balance >= @sum";
            cmd1.Parameters.AddWithValue("@sum", sum);
            cmd1.Parameters.AddWithValue("@id", from);
            if (cmd1.ExecuteNonQuery() == 0)
            {
                tr.Rollback();
                Console.WriteLine("Не хватает денег → откат");
                return false;
            }

            // зачисление
            var cmd2 = con.CreateCommand();
            cmd2.Transaction = tr;
            cmd2.CommandText = "UPDATE Acc SET Balance = Balance + @sum WHERE Id = @id";
            cmd2.Parameters.AddWithValue("@sum", sum);
            cmd2.Parameters.AddWithValue("@id", to);
            cmd2.ExecuteNonQuery();

            tr.Commit();
            return true;
        }
        catch
        {
            tr.Rollback();
            Console.WriteLine("Ошибка → полный откат");
            return false;
        }
    }

    static void Show()
    {
        using var con = new SQLiteConnection(CS);
        con.Open();
        var cmd = con.CreateCommand();
        cmd.CommandText = "SELECT Id, Balance FROM Acc";
        using var r = cmd.ExecuteReader();
        while (r.Read())
            Console.WriteLine($"Счёт {r.GetInt32(0)} → {r.GetDecimal(1)} руб.");
        Console.WriteLine();
    }
}
