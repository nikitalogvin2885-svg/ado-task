using System;
using System.Collections.Generic;
using System.Data.SQLite;

class Program
{
    private const string CS = "Data Source=orders.db;Version=3;";

    static void Main()
    {
        InitDatabase();

        Console.WriteLine("=== Создаём успешный заказ ===");
        CreateOrder(1, new List<(int ProductId, int Quantity, decimal Price)>
        {
            (1, 2, 500m),
            (2, 1, 1200m),
            (3, 3, 300m)
        });

        Console.WriteLine("\n=== Создаём заказ с ошибкой (нет на складе) ===");
        CreateOrder(2, new List<(int ProductId, int Quantity, decimal Price)>
        {
            (1, 1, 500m),
            (2, 999, 1200m)  // ← Ошибка: наушников всего 5, а заказали 999
        });

        Console.WriteLine("\nГотово! Нажми любую клавишу...");
        Console.ReadKey();
    }

    static bool CreateOrder(int customerId, List<(int ProductId, int Quantity, decimal Price)> items)
    {
        using var con = new SQLiteConnection(CS);
        con.Open();
        using var transaction = con.BeginTransaction();

        try
        {
            // 1. Создаём заголовок заказа
            var cmdOrder = con.CreateCommand();
            cmdOrder.Transaction = transaction;
            cmdOrder.CommandText = "INSERT INTO Orders (CustomerId, OrderDate, TotalAmount) VALUES (@cust, @date, 0); SELECT last_insert_rowid();";
            cmdOrder.Parameters.AddWithValue("@cust", customerId);
            cmdOrder.Parameters.AddWithValue("@date", DateTime.Now);
            long orderId = (long)cmdOrder.ExecuteScalar()!;

            decimal total = 0m;

            // 2. Добавляем товары
            foreach (var item in items)
            {
                // Проверка остатка
                var check = con.CreateCommand();
                check.Transaction = transaction;
                check.CommandText = "SELECT Stock FROM Products WHERE Id = @id";
                check.Parameters.AddWithValue("@id", item.ProductId);
                object stockObj = check.ExecuteScalar();

                if (stockObj == null || Convert.ToInt32(stockObj) < item.Quantity)
                    throw new Exception($"Товара {item.ProductId} недостаточно на складе!");

                // Снимаем со склада
                var updateStock = con.CreateCommand();
                updateStock.Transaction = transaction;
                updateStock.CommandText = "UPDATE Products SET Stock = Stock - @qty WHERE Id = @id";
                updateStock.Parameters.AddWithValue("@qty", item.Quantity);
                updateStock.Parameters.AddWithValue("@id", item.ProductId);
                updateStock.ExecuteNonQuery();
                // Добавляем строку заказа
                var cmdLine = con.CreateCommand();
                cmdLine.Transaction = transaction;
                cmdLine.CommandText = @"
                    INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price, Amount)
                    VALUES (@orderId, @prodId, @qty, @price, @amount)";
                cmdLine.Parameters.AddWithValue("@orderId", orderId);
                cmdLine.Parameters.AddWithValue("@prodId", item.ProductId);
                cmdLine.Parameters.AddWithValue("@qty", item.Quantity);
                cmdLine.Parameters.AddWithValue("@price", item.Price);
                cmdLine.Parameters.AddWithValue("@amount", item.Quantity * item.Price); // ← ИСПРАВЛЕНО

                cmdLine.ExecuteNonQuery();

                total += item.Quantity * item.Price;
            }

            // 3. Обновляем итоговую сумму
            var updateTotal = con.CreateCommand();
            updateTotal.Transaction = transaction;
            updateTotal.CommandText = "UPDATE Orders SET TotalAmount = @total WHERE Id = @id";
            updateTotal.Parameters.AddWithValue("@total", total);
            updateTotal.Parameters.AddWithValue("@id", orderId);
            updateTotal.ExecuteNonQuery();

            transaction.Commit();
            Console.WriteLine($"Заказ {orderId} успешно создан! Сумма: {total} руб.");
            return true;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            Console.WriteLine($"ОШИБКА! Весь заказ откатился: {ex.Message}");
            return false;
        }
    }

    static void InitDatabase()
    {
        using var con = new SQLiteConnection(CS);
        con.Open();

        new SQLiteCommand("DROP TABLE IF EXISTS OrderItems", con).ExecuteNonQuery();
        new SQLiteCommand("DROP TABLE IF EXISTS Orders", con).ExecuteNonQuery();
        new SQLiteCommand("DROP TABLE IF EXISTS Products", con).ExecuteNonQuery();

        new SQLiteCommand(@"
            CREATE TABLE Products (
                Id INTEGER PRIMARY KEY,
                Name TEXT,
                Stock INTEGER
            )", con).ExecuteNonQuery();

        new SQLiteCommand(@"
            INSERT INTO Products (Id, Name, Stock) VALUES
            (1, 'Телефон', 10),
            (2, 'Наушники', 5),
            (3, 'Чехол', 50)", con).ExecuteNonQuery();

        new SQLiteCommand(@"
            CREATE TABLE Orders (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                CustomerId INTEGER,
                OrderDate TEXT,
                TotalAmount REAL
            )", con).ExecuteNonQuery();

        new SQLiteCommand(@"
            CREATE TABLE OrderItems (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                OrderId INTEGER,
                ProductId INTEGER,
                Quantity INTEGER,
                Price REAL,
                Amount REAL
            )", con).ExecuteNonQuery();
    }
}