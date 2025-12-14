//using System.Data;
//using System.Diagnostics;

////1
//using System;
//using System.Data.SqlClient;

//namespace SqlConnectionDemo
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            string connectionString = "Server=localhost;Database=master;Trusted_Connection=True;TrustServerCertificate=True;";

//            using (SqlConnection connection = new SqlConnection(connectionString))
//            {
//                try
//                {
//                    connection.Open();

//                    if (connection.State == System.Data.ConnectionState.Open)
//                    {
//                        Console.WriteLine("Подключение успешно установлено!");
//                        Console.WriteLine($"Сервер: {connection.ServerVersion}");
//                        Console.WriteLine($"База данных: {connection.Database}");
//                        Console.WriteLine($"Источник данных: {connection.DataSource}");
//                    }
//                }
//                catch (SqlException ex)
//                {
//                    Console.WriteLine("Ошибка SQL Server:");
//                    Console.WriteLine(ex.Message);
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine("Ошибка подключения:");
//                    Console.WriteLine(ex.Message);
//                }
//            }

//            Console.WriteLine("Соединение закрыто.");
//            Console.ReadKey();
//        }
//    }
//}

////2
//using System;
//using System.Data.SqlClient;

//namespace SqlSelectDemo
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            string connectionString = "Server=localhost;Database=YourDatabase;Trusted_Connection=True;TrustServerCertificate=True;";

//            using (SqlConnection connection = new SqlConnection(connectionString))
//            {
//                try
//                {
//                    connection.Open();

//                    string query = "SELECT id, name, email FROM Users";

//                    using (SqlCommand command = new SqlCommand(query, connection))
//                    {
//                        using (SqlDataReader reader = command.ExecuteReader())
//                        {
//                            if (!reader.HasRows)
//                            {
//                                Console.WriteLine("Таблица Users пуста.");
//                                return;
//                            }

//                            Console.WriteLine("| {0,-5} | {1,-20} | {2,-30} |", "ID", "Name", "Email");
//                            Console.WriteLine(new string('-', 65));

//                            while (reader.Read())
//                            {
//                                int id = reader.GetInt32(0);
//                                string name = reader.IsDBNull(1) ? "" : reader.GetString(1);
//                                string email = reader.IsDBNull(2) ? "" : reader.GetString(2);

//                                Console.WriteLine("| {0,-5} | {1,-20} | {2,-30} |", id, name, email);
//                            }
//                        }
//                    }
//                }
//                catch (SqlException ex)
//                {
//                    Console.WriteLine("Ошибка SQL: " + ex.Message);
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine("Ошибка: " + ex.Message);
//                }
//            }

//            Console.WriteLine("\nНажмите любую клавишу для завершения...");
//            Console.ReadKey();
//        }
//    }
//}

////3
//using System;
//using System.Data.SqlClient;

//namespace SqlDataReaderDemo
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            string connectionString = "Server=localhost;Database=YourDatabase;Trusted_Connection=True;TrustServerCertificate=True;";

//            using (SqlConnection connection = new SqlConnection(connectionString))
//            {
//                try
//                {
//                    connection.Open();

//                    string query = "SELECT id, name, email FROM Users";

//                    using (SqlCommand command = new SqlCommand(query, connection))
//                    {
//                        using (SqlDataReader reader = command.ExecuteReader())
//                        {
//                            if (!reader.HasRows)
//                            {
//                                Console.WriteLine("Таблица Users пуста.");
//                                Console.WriteLine("Прочитано записей: 0");
//                                Console.ReadKey();
//                                return;
//                            }

//                            Console.WriteLine("| {0,-5} | {1,-20} | {2,-30} |", "ID", "Name", "Email");
//                            Console.WriteLine(new string('-', 65));

//                            int rowCount = 0;

//                            while (reader.Read())
//                            {
//                                int id = reader.GetInt32(0);
//                                string name = reader.IsDBNull(1) ? "(null)" : reader.GetString(1);
//                                string email = reader.IsDBNull(2) ? "(null)" : reader.GetString(2);

//                                Console.WriteLine("| {0,-5} | {1,-20} | {2,-30} |", id, name, email);
//                                rowCount++;
//                            }

//                            Console.WriteLine(new string('-', 65));
//                            Console.WriteLine($"Прочитано записей: {rowCount}");
//                        }
//                    }
//                }
//                catch (SqlException ex)
//                {
//                    Console.WriteLine("Ошибка SQL: " + ex.Message);
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine("Ошибка: " + ex.Message);
//                }
//            }

//            Console.WriteLine("\nНажмите любую клавишу для завершения...");
//            Console.ReadKey();
//        }
//    }
//}

////4
//using System;
//using System.Data;
//using System.Data.SqlClient;

//namespace SqlInsertDemo
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            string connectionString = "Server=localhost;Database=YourDatabase;Trusted_Connection=True;TrustServerCertificate=True;";

//            Console.Write("Введите имя: ");
//            string name = Console.ReadLine();

//            Console.Write("Введите email: ");
//            string email = Console.ReadLine();

//            InsertUser(connectionString, name, email);

//            Console.WriteLine("\nНажмите любую клавишу для завершения...");
//            Console.ReadKey();
//        }

//        static void InsertUser(string connectionString, string name, string email)
//        {
//            string query = "INSERT INTO Users (name, email) VALUES (@name, @email)";

//            using (SqlConnection connection = new SqlConnection(connectionString))
//            {
//                using (SqlCommand command = new SqlCommand(query, connection))
//                {
//                    command.Parameters.Add("@name", SqlDbType.NVarChar, 100).Value = name ?? (object)DBNull.Value;
//                    command.Parameters.Add("@email", SqlDbType.NVarChar, 255).Value = email ?? (object)DBNull.Value;

//                    try
//                    {
//                        connection.Open();
//                        int rowsAffected = command.ExecuteNonQuery();

//                        if (rowsAffected > 0)
//                        {
//                            Console.WriteLine("Запись успешно добавлена в таблицу Users.");
//                        }
//                    }
//                    catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
//                    {
//                        Console.WriteLine("Ошибка: Нарушение ограничения уникальности (дубликат email или другого уникального поля).");
//                    }
//                    catch (SqlException ex) when (ex.Number == 547)
//                    {
//                        Console.WriteLine("Ошибка: Нарушение ограничения CHECK или внешнего ключа.");
//                    }
//                    catch (SqlException ex)
//                    {
//                        Console.WriteLine("Ошибка базы данных: " + ex.Message);
//                    }
//                    catch (Exception ex)
//                    {
//                        Console.WriteLine("Неизвестная ошибка: " + ex.Message);
//                    }
//                }
//            }
//        }
//    }
//}

////5
//using System;
//using System.Data;
//using System.Data.SqlClient;

//namespace SqlUpdateDemo
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            string connectionString = "Server=localhost;Database=YourDatabase;Trusted_Connection=True;TrustServerCertificate=True;";

//            Console.Write("Введите ID пользователя: ");
//            if (!int.TryParse(Console.ReadLine(), out int userId))
//            {
//                Console.WriteLine("Некорректный ID.");
//                Console.ReadKey();
//                return;
//            }

//            Console.Write("Введите новый email: ");
//            string newEmail = Console.ReadLine();

//            int updatedRows = UpdateUserEmail(connectionString, userId, newEmail);

//            if (updatedRows > 0)
//                Console.WriteLine($"Email успешно обновлён. Обновлено строк: {updatedRows}");
//            else
//                Console.WriteLine("Пользователь с указанным ID не найден.");

//            Console.WriteLine("\nНажмите любую клавишу для завершения...");
//            Console.ReadKey();
//        }

//        static int UpdateUserEmail(string connectionString, int userId, string newEmail)
//        {
//            string checkQuery = "SELECT COUNT(*) FROM Users WHERE id = @id";
//            string updateQuery = "UPDATE Users SET email = @email WHERE id = @id";

//            using (SqlConnection connection = new SqlConnection(connectionString))
//            {
//                connection.Open();

//                using (SqlCommand checkCmd = new SqlCommand(checkQuery, connection))
//                {
//                    checkCmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = userId });
//                    int count = (int)checkCmd.ExecuteScalar();

//                    if (count == 0)
//                        return 0;
//                }

//                using (SqlCommand updateCmd = new SqlCommand(updateQuery, connection))
//                {
//                    updateCmd.Parameters.Add(new SqlParameter("@email", SqlDbType.NVarChar, 255) { Value = newEmail ?? (object)DBNull.Value });
//                    updateCmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = userId });

//                    try
//                    {
//                        return updateCmd.ExecuteNonQuery();
//                    }
//                    catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
//                    {
//                        Console.WriteLine("Ошибка: Новый email уже существует (нарушение уникальности).");
//                        return 0;
//                    }
//                    catch (SqlException ex)
//                    {
//                        Console.WriteLine("Ошибка базы данных: " + ex.Message);
//                        return 0;
//                    }
//                }
//            }
//        }
//    }
//}

////6
//using System;
//using System.Data;
//using System.Data.SqlClient;

//namespace SqlDeleteDemo
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            string connectionString = "Server=localhost;Database=YourDatabase;Trusted_Connection=True;TrustServerCertificate=True;";

//            Console.Write("Введите ID пользователя для удаления: ");
//            if (!int.TryParse(Console.ReadLine(), out int userId) || userId <= 0)
//            {
//                Console.WriteLine("Некорректный ID.");
//                Console.ReadKey();
//                return;
//            }

//            bool deleted = DeleteUserById(connectionString, userId);

//            if (deleted)
//                Console.WriteLine("Пользователь успешно удалён.");
//            else
//                Console.WriteLine("Удаление не выполнено (пользователь не найден или есть связанные данные).");

//            Console.WriteLine("\nНажмите любую клавишу для завершения...");
//            Console.ReadKey();
//        }

//        static bool DeleteUserById(string connectionString, int userId)
//        {
//            string checkQuery = "SELECT COUNT(*) FROM Users WHERE id = @id";
//            string deleteQuery = "DELETE FROM Users WHERE id = @id";

//            using (SqlConnection connection = new SqlConnection(connectionString))
//            {
//                try
//                {
//                    connection.Open();

//                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, connection))
//                    {
//                        checkCmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = userId });
//                        int count = (int)checkCmd.ExecuteScalar();

//                        if (count == 0)
//                        {
//                            Console.WriteLine("Пользователь с указанным ID не найден.");
//                            return false;
//                        }
//                    }

//                    using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, connection))
//                    {
//                        deleteCmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = userId });
//                        int rowsAffected = deleteCmd.ExecuteNonQuery();

//                        if (rowsAffected > 0)
//                        {
//                            Console.WriteLine($"Подтверждение: пользователь с ID = {userId} удалён.");
//                            return true;
//                        }
//                    }
//                }
//                catch (SqlException ex) when (ex.Number == 547)
//                {
//                    Console.WriteLine("Ошибка: Невозможно удалить пользователя — на него ссылаются другие таблицы (нарушение внешнего ключа).");
//                    return false;
//                }
//                catch (SqlException ex)
//                {
//                    Console.WriteLine("Ошибка базы данных: " + ex.Message);
//                    return false;
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine("Неизвестная ошибка: " + ex.Message);
//                    return false;
//                }

//                return false;
//            }
//        }
//    }
//}

////7
//using System;
//using System.Data;
//using System.Data.SqlClient;

//namespace SqlInjectionDemo
//{
//    class Program
//    {
//        static string connectionString = "Server=localhost;Database=YourDatabase;Trusted_Connection=True;TrustServerCertificate=True;";

//        static void Main(string[] args)
//        {
//            Console.WriteLine("Демонстрация SQL Injection\n");

//            Console.Write("Введите email для поиска (безопасный метод): ");
//            string input = Console.ReadLine();

//            Console.WriteLine("\n1. Безопасный метод ( SqlParameter )");
//            SafeLoginSearch(input);

//            Console.WriteLine("\n2. Уязвимый метод (конкатенация строк)");
//            UnsafeLoginSearch(input);

//            Console.WriteLine("\nНажмите любую клавишу для завершения...");
//            Console.ReadKey();
//        }

//        // Безопасный метод — использует параметры
//        static void SafeLoginSearch(string email)
//        {
//            string query = "SELECT id, name, email FROM Users WHERE email = @email";

//            using (SqlConnection conn = new SqlConnection(connectionString))
//            {
//                using (SqlCommand cmd = new SqlCommand(query, conn))
//                {
//                    cmd.Parameters.Add(new SqlParameter("@email", SqlDbType.NVarChar, 255) { Value = email });

//                    try
//                    {
//                        conn.Open();
//                        using (SqlDataReader reader = cmd.ExecuteReader())
//                        {
//                            if (reader.Read())
//                                Console.WriteLine($"Найден: ID={reader["id"]}, Name={reader["name"]}, Email={reader["email"]}");
//                            else
//                                Console.WriteLine("Пользователь не найден.");
//                        }
//                    }
//                    catch (Exception ex) { Console.WriteLine("Ошибка: " + ex.Message); }
//                }
//            }

//            Console.WriteLine("Параметры полностью защищают от SQL-инъекций!");
//        }

//        static void UnsafeLoginSearch(string email)
//        {
//            string query = "SELECT id, name, email FROM Users WHERE email = '" + email + "'";

//            Console.WriteLine($"Выполняемый запрос: {query}");

//            using (SqlConnection conn = new SqlConnection(connectionString))
//            {
//                using (SqlCommand cmd = new SqlCommand(query, conn))
//                {
//                    try
//                    {
//                        conn.Open();
//                        using (SqlDataReader reader = cmd.ExecuteReader())
//                        {
//                            Console.WriteLine("Результаты (может быть много!):");
//                            while (reader.Read())
//                            {
//                                Console.WriteLine($"  → ID={reader["id"]}, Name={reader["name"]}, Email={reader["email"]}");
//                            }
//                        }
//                    }
//                    catch (Exception ex) { Console.WriteLine("Ошибка: " + ex.Message); }
//                }
//            }

//            Console.WriteLine("ВНИМАНИЕ: Если ввести ' OR '1'='1 — будут выведены ВСЕ пользователи!");
//            Console.WriteLine("Это и есть SQL Injection — злоумышленник может удалить данные, войти без пароля и т.д.");
//        }
//    }
//}

////8
//using System;
//using System.Data;
//using System.Data.SqlClient;

//namespace SqlDataAdapterDemo
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            string connectionString = "Server=localhost;Database=YourDatabase;Trusted_Connection=True;TrustServerCertificate=True;";

//            string query = "SELECT id, name, email FROM Users";

//            DataTable dataTable = new DataTable();

//            using (SqlConnection connection = new SqlConnection(connectionString))
//            {
//                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
//                {
//                    try
//                    {
//                        adapter.Fill(dataTable);

//                        if (dataTable.Rows.Count == 0)
//                        {
//                            Console.WriteLine("Таблица Users пуста.");
//                        }
//                        else
//                        {
//                            Console.WriteLine($"Загружено записей: {dataTable.Rows.Count}");
//                            Console.WriteLine();

//                            Console.WriteLine("| {0,-5} | {1,-20} | {2,-30} |", "ID", "Name", "Email");
//                            Console.WriteLine(new string('-', 65));

//                            foreach (DataRow row in dataTable.Rows)
//                            {
//                                int id = Convert.ToInt32(row["id"]);
//                                string name = row["name"] == DBNull.Value ? "(null)" : row["name"].ToString();
//                                string email = row["email"] == DBNull.Value ? "(null)" : row["email"].ToString();

//                                Console.WriteLine("| {0,-5} | {1,-20} | {2,-30} |", id, name, email);
//                            }

//                            Console.WriteLine(new string('-', 65));

//                            if (dataTable.Rows.Count > 0)
//                            {
//                                DataRow firstRow = dataTable.Rows[0];
//                                Console.WriteLine($"Первая строка: ID = {firstRow["id"]}, Name = {firstRow["name"]}");
//                            }
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        Console.WriteLine("Ошибка: " + ex.Message);
//                    }
//                }
//            }

//            Console.WriteLine("\nНажмите любую клавишу для завершения...");
//            Console.ReadKey();
//        }
//    }
//}

////9
//using System;
//using System.Data;
//using System.Data.SqlClient;

//namespace DataSetRelationsDemo
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            string connectionString = "Server=localhost;Database=YourDatabase;Trusted_Connection=True;TrustServerCertificate=True;";

//            DataSet dataSet = new DataSet("Shop");

//            using (SqlConnection connection = new SqlConnection(connectionString))
//            {
//                try
//                {
//                    connection.Open();

//                    string sqlUsers = "SELECT id, name, email FROM Users";
//                    using (SqlDataAdapter adapterUsers = new SqlDataAdapter(sqlUsers, connection))
//                    {
//                        adapterUsers.Fill(dataSet, "Users");
//                    }

//                    string sqlOrders = "SELECT id, user_id, product, amount FROM Orders";
//                    using (SqlDataAdapter adapterOrders = new SqlDataAdapter(sqlOrders, connection))
//                    {
//                        adapterOrders.Fill(dataSet, "Orders");
//                    }

//                    DataRelation relation = new DataRelation(
//                        "UserOrders",
//                        dataSet.Tables["Users"].Columns["id"],
//                        dataSet.Tables["Orders"].Columns["user_id"]
//                    );

//                    dataSet.Relations.Add(relation);

//                    Console.WriteLine("=== Пользователи и их заказы ===\n");

//                    foreach (DataRow userRow in dataSet.Tables["Users"].Rows)
//                    {
//                        Console.WriteLine($"Пользователь: {userRow["id"]}. {userRow["name"]} ({userRow["email"]})");

//                        DataRow[] childOrders = userRow.GetChildRows(relation);

//                        if (childOrders.Length == 0)
//                        {
//                            Console.WriteLine("   → Нет заказов");
//                        }
//                        else
//                        {
//                            Console.WriteLine($"   → Заказов: {childOrders.Length}");
//                            foreach (DataRow order in childOrders)
//                            {
//                                Console.WriteLine($"     • Заказ #{order["id"]}: {order["product"]}, сумма: {order["amount"]}");
//                            }
//                        }
//                        Console.WriteLine();
//                    }

//                    Console.WriteLine($"Всего пользователей: {dataSet.Tables["Users"].Rows.Count}");
//                    Console.WriteLine($"Всего заказов: {dataSet.Tables["Orders"].Rows.Count}");
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine("Ошибка: " + ex.Message);
//                }
//            }

//            Console.WriteLine("Нажмите любую клавишу для завершения...");
//            Console.ReadKey();
//        }
//    }
//}

////10
//using System;
//using System.Data;
//using System.Data.SqlClient;
//using System.Threading;

//namespace DatabaseExceptionHandlingDemo
//{
//    class Program
//    {
//        static string connectionString = "Server=localhost;Database=YourDatabase;Trusted_Connection=True;TrustServerCertificate=True;";

//        static void Main(string[] args)
//        {
//            Console.WriteLine("Демонстрация полной обработки исключений при работе с БД\n");

//            ExecuteWithFullExceptionHandling("SELECT * FROM Users");                       


//            Console.WriteLine("\nПрограмма завершена.");
//            Console.ReadKey();
//        }

//        static void ExecuteWithFullExceptionHandling(string query)
//        {
//            using (SqlConnection connection = new SqlConnection(connectionString))
//            {
//                SqlCommand command = new SqlCommand(query, connection);
//                command.CommandTimeout = 10; /

//                try
//                {
//                    Console.WriteLine($"Выполняется запрос: {query}");
//                    connection.Open();

//                    using (SqlDataReader reader = command.ExecuteReader())
//                    {
//                        int count = 0;
//                        while (reader.Read()) count++;
//                        Console.WriteLine($"Успешно получено строк: {count}");
//                    }
//                }
//                catch (SqlException sqlEx)
//                {
//                    LogError("SQL Server Error", sqlEx);
//                    HandleSqlException(sqlEx);
//                }
//                catch (TimeoutException timeoutEx)
//                {
//                    LogError("Timeout Error", timeoutEx);
//                    Console.WriteLine("Превышено время ожидания выполнения запроса (таймаут).");
//                    Console.WriteLine("Рекомендация: увеличьте CommandTimeout или оптимизируйте запрос.");
//                }
//                catch (InvalidOperationException invOpEx)
//                {
//                    LogError("Invalid Operation", invOpEx);
//                    Console.WriteLine("Недопустимая операция: возможно, соединение уже открыто/закрыто или объект использован неверно.");
//                }
//                catch (Exception ex)
//                {
//                    LogError("Unexpected Error", ex);
//                    Console.WriteLine($"Неизвестная ошибка: {ex.Message}");
//                }
//            }
//        }

//        static void HandleSqlException(SqlException sqlEx)
//        {
//            foreach (SqlError error in sqlEx.Errors)
//            {
//                Console.WriteLine($"  • Код ошибки: {error.Number}");
//                Console.WriteLine($"  • Уровень: {error.Class}");
//                Console.WriteLine($"  • Сообщение: {error.Message}");
//                Console.WriteLine($"  • Процедура: {error.Procedure}");
//                Console.WriteLine($"  • Сервер: {error.Server}");
//                Console.WriteLine($"  • Строка: {error.LineNumber}\n");
//            }

//            switch (sqlEx.Number)
//            {
//                case 2: Console.WriteLine("Сервер недоступен или неправильный адрес."); break;
//                case 53: Console.WriteLine("Сетевая ошибка подключения."); break;
//                case 18456: Console.WriteLine("Ошибка авторизации (неверный логин/пароль)."); break;
//                case 4060: Console.WriteLine("Не удалось открыть базу данных."); break;
//                case 207: Console.WriteLine("Неверное имя столбца."); break;
//                case 208: Console.WriteLine("Таблица не найдена."); break;
//                case 547: Console.WriteLine("Нарушение ограничения (внешний ключ, CHECK и т.д.)."); break;
//                case 2627: case 2601: Console.WriteLine("Нарушение уникальности (дубликат ключа)."); break;
//                case -2: Console.WriteLine("Таймаут подключения."); break;
//                default: Console.WriteLine("Другая ошибка SQL Server."); break;
//            }
//        }

//        static void LogError(string errorType, Exception ex)
//        {
//            string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {errorType} | {ex.GetType().Name} | {ex.Message}\n{ex.StackTrace}\n";
//            Console.WriteLine($"\n[ЛОГ] {logMessage}");
//        }
//    }
//}

////11
//using System;
//using System.Data;
//using System.Data.SqlClient;

//namespace StoredProcedureDemo
//{
//    class Program
//    {
//        static string connectionString = "Server=localhost;Database=YourDatabase;Trusted_Connection=True;TrustServerCertificate=True;";

//        static void Main(string[] args)
//        {
//            Console.Write("Введите ID пользователя: ");
//            if (!int.TryParse(Console.ReadLine(), out int userId) || userId <= 0)
//            {
//                Console.WriteLine("Некорректный ID.");
//                Console.ReadKey();
//                return;
//            }

//            GetUserOrders(userId);

//            Console.WriteLine("\nНажмите любую клавишу для завершения...");
//            Console.ReadKey();
//        }

//        static void GetUserOrders(int userId)
//        {
//            using (SqlConnection connection = new SqlConnection(connectionString))
//            {
//                using (SqlCommand command = new SqlCommand("GetUserOrders", connection))
//                {
//                    command.CommandType = CommandType.StoredProcedure;

//                    command.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int) { Value = userId });

//                    try
//                    {
//                        connection.Open();

//                        using (SqlDataReader reader = command.ExecuteReader())
//                        {
//                            if (!reader.HasRows)
//                            {
//                                Console.WriteLine($"У пользователя с ID = {userId} нет заказов.");
//                                return;
//                            }

//                            Console.WriteLine($"Заказы пользователя ID = {userId}:");
//                            Console.WriteLine("| {0,-5} | {1,-20} | {2,-12} |", "ID", "Товар", "Сумма");
//                            Console.WriteLine(new string('-', 50));

//                            while (reader.Read())
//                            {
//                                int orderId = reader.GetInt32("id");
//                                string product = reader.GetString("product");
//                                decimal amount = reader.GetDecimal("amount");

//                                Console.WriteLine("| {0,-5} | {1,-20} | {2,-12:C} |", orderId, product, amount);
//                            }
//                        }
//                    }
//                    catch (SqlException ex)
//                    {
//                        Console.WriteLine("Ошибка при выполнении хранимой процедуры:");
//                        Console.WriteLine(ex.Message);
//                    }
//                    catch (Exception ex)
//                    {
//                        Console.WriteLine("Ошибка: " + ex.Message);
//                    }
//                }
//            }
//        }
//    }
//}

////12
//using System;
//using System.Data;
//using System.Data.SqlClient;

//class Program
//{
//    static string connStr = "Server=localhost;Database=YourDatabase;Trusted_Connection=True;TrustServerCertificate=True;";

//    static void Main()
//    {
//        Console.Write("Введите ID пользователя: ");
//        int userId = int.Parse(Console.ReadLine());

//        using (SqlConnection conn = new SqlConnection(connStr))
//        {
//            SqlCommand cmd = new SqlCommand("GetUserOrdersCount", conn);
//            cmd.CommandType = CommandType.StoredProcedure;

//            cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
//            SqlParameter outputParam = new SqlParameter("@OrdersCount", SqlDbType.Int)
//            {
//                Direction = ParameterDirection.Output
//            };
//            cmd.Parameters.Add(outputParam);

//            conn.Open();
//            cmd.ExecuteNonQuery();

//            int count = outputParam.Value == DBNull.Value ? 0 : (int)outputParam.Value;
//            Console.WriteLine($"Количество заказов пользователя {userId}: {count}");
//        }
//        Console.ReadKey();
//    }
//}

////13
//using System;
//using System.Data.SqlClient;

//class Program
//{
//    static string connStr = "Server=localhost;Database=YourDatabase;Trusted_Connection=True;TrustServerCertificate=True;";

//    static void Main()
//    {
//        TransferMoney(1, 2, 300.00m);
//        Console.ReadKey();
//    }

//    static bool TransferMoney(int fromId, int toId, decimal amount)
//    {
//        using (SqlConnection conn = new SqlConnection(connStr))
//        {
//            conn.Open();
//            using (SqlTransaction tx = conn.BeginTransaction())
//            {
//                try
//                {
//                    using (SqlCommand cmd = new SqlCommand(
//                        "UPDATE Accounts SET Balance = Balance - @amount WHERE Id = @fromId;" +
//                        "UPDATE Accounts SET Balance = Balance + @amount WHERE Id = @toId;",
//                        conn, tx))
//                    {
//                        cmd.Parameters.AddWithValue("@amount", amount);
//                        cmd.Parameters.AddWithValue("@fromId", fromId);
//                        cmd.Parameters.AddWithValue("@toId", toId);

//                        cmd.ExecuteNonQuery();
//                    }
//                    tx.Commit();
//                    Console.WriteLine($"Перевод {amount:C} успешен!");
//                    return true;
//                }
//                catch (Exception ex)
//                {
//                    tx.Rollback();
//                    Console.WriteLine("Ошибка! Транзакция отменена: " + ex.Message);
//                    return false;
//                }
//            }
//        }
//    }
//}

////14
//using System;
//using System.Data;
//using System.Data.SqlClient;
//using System.Threading.Tasks;

//class Program
//{
//    static string connStr = "Server=localhost;Database=YourDatabase;Trusted_Connection=True;TrustServerCertificate=True;";

//    static void Main()
//    {
//        DemoIsolationLevels();
//        Console.ReadKey();
//    }

//    static void DemoIsolationLevels()
//    {
//        using (SqlConnection conn = new SqlConnection(connStr))
//        {
//            conn.Open();

//            new SqlCommand("IF OBJECT_ID('TestTable') IS NULL CREATE TABLE TestTable (Id INT, Value INT); TRUNCATE TABLE TestTable; INSERT INTO TestTable VALUES (1, 100)", conn).ExecuteNonQuery();

//            Console.WriteLine("1. ReadUncommitted (грязное чтение):");
//            Task.Run(() => DirtyReadDemo(conn));

//            Console.WriteLine("\n2. ReadCommitted (по умолчанию):");
//            Task.Run(() => ReadCommittedDemo(conn));

//            Console.WriteLine("\n3. Serializable (полная блокировка):");
//            Task.Run(() => SerializableDemo(conn));

//            Console.ReadKey();
//        }
//    }

//    static void DirtyReadDemo(SqlConnection conn)
//    {
//        using (var c = new SqlConnection(connStr)) c.Open();
//        using (var tx = c.BeginTransaction(IsolationLevel.ReadUncommitted))
//        using (var cmd = c.CreateCommand()) { cmd.Transaction = tx; cmd.CommandText = "SELECT Value FROM TestTable WHERE Id=1"; Console.WriteLine("ReadUncommitted видит: " + cmd.ExecuteScalar()); }
//    }

//    static void ReadCommittedDemo(SqlConnection conn)
//    {
//        using (var c = new SqlConnection(connStr)) c.Open();
//        using (var tx = c.BeginTransaction(IsolationLevel.ReadCommitted))
//        using (var cmd = c.CreateCommand()) { cmd.Transaction = tx; cmd.CommandText = "SELECT Value FROM TestTable WHERE Id=1"; Console.WriteLine("ReadCommitted видит: " + cmd.ExecuteScalar()); }
//    }

//    static void SerializableDemo(SqlConnection conn)
//    {
//        using (var c = new SqlConnection(connStr)) c.Open();
//        using (var tx = c.BeginTransaction(IsolationLevel.Serializable))
//        using (var cmd = c.CreateCommand()) { cmd.Transaction = tx; cmd.CommandText = "SELECT Value FROM TestTable WHERE Id=1"; Console.WriteLine("Serializable — ждёт завершения других транзакций..."); cmd.ExecuteScalar(); }
//    }
//}

////15
//using System;
//using System.Data;
//using System.Data.SqlClient;
//using System.Diagnostics;

//class Program
//{
//    static string connStr = "Server=localhost;Database=YourDatabase;Trusted_Connection=True;TrustServerCertificate=True;";

//    static void Main()
//    {
//        DataTable table = new DataTable();
//        table.Columns.Add("Name", typeof(string));
//        table.Columns.Add("Email", typeof(string));

//        for (int i = 1; i <= 10000; i++)
//            table.Rows.Add("User" + i, $"user{i}@test.com");

//        Console.WriteLine("Тестируем обычный INSERT...");
//        var sw1 = Stopwatch.StartNew();
//        InsertOneByOne(table);
//        sw1.Stop();

//        new SqlCommand("TRUNCATE TABLE Users", new SqlConnection(connStr) { }.Open()).ExecuteNonQuery();

//        Console.WriteLine($"Обычный INSERT: {sw1.ElapsedMilliseconds} мс");

//        Console.WriteLine("Тестируем SqlBulkCopy...");
//        var sw2 = Stopwatch.StartNew();
//        BulkInsert(table);
//        sw2.Stop();

//        Console.WriteLine($"SqlBulkCopy: {sw2.ElapsedMilliseconds} мс (в {sw1.ElapsedMilliseconds / (double)sw2.ElapsedMilliseconds:F1} раз быстрее!)");

//        Console.ReadKey();
//    }

//    static void InsertOneByOne(DataTable data)
//    {
//        using (SqlConnection conn = new SqlConnection(connStr))
//        {
//            conn.Open();
//            foreach (DataRow row in data.Rows)
//            {
//                using (SqlCommand cmd = new SqlCommand("INSERT INTO Users (name, email) VALUES (@n, @e)", conn))
//                {
//                    cmd.Parameters.AddWithValue("@n", row["Name"]);
//                    cmd.Parameters.AddWithValue("@e", row["Email"]);
//                    cmd.ExecuteNonQuery();
//                }
//            }
//        }
//    }

//    static void BulkInsert(DataTable data)
//    {
//        using (SqlBulkCopy bulk = new SqlBulkCopy(connStr))
//        {
//            bulk.DestinationTableName = "Users";
//            bulk.ColumnMappings.Add("Name", "name");
//            bulk.ColumnMappings.Add("Email", "email");
//            bulk.BulkCopyTimeout = 300;
//            bulk.WriteToServer(data);
//        }
//    }
//}

////16
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.Threading.Tasks;

//public class Employee
//{
//    public int EmployeeID { get; set; }
//    public string Name { get; set; }
//    public decimal Salary { get; set; }
//}

//public class EmployeeRepository
//{
//    private readonly string _connectionString;

//    public EmployeeRepository(string connectionString)
//    {
//        _connectionString = connectionString;
//    }

//    // Асинхронное чтение данных из БД
//    public async Task<List<Employee>> GetEmployeesAsync()
//    {
//        List<Employee> employees = new List<Employee>();

//        using (SqlConnection connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            string query = "SELECT EmployeeID, Name, Salary FROM Employees";

//            using (SqlCommand command = new SqlCommand(query, connection))
//            {
//                using (SqlDataReader reader = await command.ExecuteReaderAsync())
//                {
//                    while (await reader.ReadAsync())
//                    {
//                        employees.Add(new Employee
//                        {
//                            EmployeeID = reader.GetInt32(0),
//                            Name = reader.GetString(1),
//                            Salary = reader.GetDecimal(2)
//                        });
//                    }
//                }
//            }
//        }

//        return employees;
//    }
//}

//// Пример использования
//class Program
//{
//    static async Task Main(string[] args)
//    {
//        string connectionString = "Server=your_server;Database=your_database;User Id=your_user;Password=your_password;";
//        EmployeeRepository repository = new EmployeeRepository(connectionString);

//        Console.WriteLine("Загрузка данных из базы данных...");

//        var employees = await repository.GetEmployeesAsync();

//        Console.WriteLine("\nСписок сотрудников:");
//        foreach (var employee in employees)
//        {
//            Console.WriteLine($"{employee.EmployeeID}: {employee.Name}, Зарплата: {employee.Salary:C}");
//        }
//    }
//}

////17
//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Threading;
//using System.Threading.Tasks;

//public class Employee
//{
//    public int EmployeeID { get; set; }
//    public string Name { get; set; }
//    public decimal Salary { get; set; }
//}

//public class CachedEmployeeRepository
//{
//    private readonly string _connectionString;
//    private static Dictionary<string, CacheItem> _cache = new Dictionary<string, CacheItem>();
//    private static Timer _cacheCleanupTimer;

//    public CachedEmployeeRepository(string connectionString)
//    {
//        _connectionString = connectionString;
//        _cacheCleanupTimer = new Timer(CleanupCache, null, TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(10));
//    }

//    public async Task<List<Employee>> GetEmployeesAsync()
//    {
//        string cacheKey = "AllEmployees";

//        if (_cache.TryGetValue(cacheKey, out CacheItem cacheItem) && !cacheItem.IsExpired)
//        {
//            Console.WriteLine("Данные получены из кэша.");
//            return cacheItem.Data;
//        }

//        List<Employee> employees = new List<Employee>();

//        using (SqlConnection connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            string query = "SELECT EmployeeID, Name, Salary FROM Employees";
//            using (SqlCommand command = new SqlCommand(query, connection))
//            {
//                using (SqlDataReader reader = await command.ExecuteReaderAsync())
//                {
//                    while (await reader.ReadAsync())
//                    {
//                        employees.Add(new Employee
//                        {
//                            EmployeeID = reader.GetInt32(0),
//                            Name = reader.GetString(1),
//                            Salary = reader.GetDecimal(2)
//                        });
//                    }
//                }
//            }
//        }

//        _cache[cacheKey] = new CacheItem(employees);
//        Console.WriteLine("Данные загружены из базы данных.");
//        return employees;
//    }

//    public void InvalidateCache()
//    {
//        _cache.Clear();
//        Console.WriteLine("Кэш очищен.");
//    }

//    private static void CleanupCache(object state)
//    {
//        List<string> expiredKeys = new List<string>();
//        foreach (var kvp in _cache)
//        {
//            if (kvp.Value.IsExpired)
//                expiredKeys.Add(kvp.Key);
//        }

//        foreach (string key in expiredKeys)
//            _cache.Remove(key);

//        Console.WriteLine($"Очистка кэша: удалено {expiredKeys.Count} записей.");
//    }

//    private class CacheItem
//    {
//        public List<Employee> Data { get; }
//        public DateTime ExpirationTime { get; }

//        public CacheItem(List<Employee> data, int cacheDurationMinutes = 5)
//        {
//            Data = data;
//            ExpirationTime = DateTime.Now.AddMinutes(cacheDurationMinutes);
//        }

//        public bool IsExpired => DateTime.Now > ExpirationTime;
//    }
//}

//// Пример использования
//class Program
//{
//    static async Task Main(string[] args)
//    {
//        string connectionString = "Server=your_server;Database=your_database;User Id=your_user;Password=your_password;";
//        CachedEmployeeRepository repository = new CachedEmployeeRepository(connectionString);

//        // Первая загрузка данных
//        Console.WriteLine("Первая загрузка данных...");
//        var employees1 = await repository.GetEmployeesAsync();

//        // Вторая загрузка данных (из кэша)
//        Console.WriteLine("\nВторая загрузка данных...");
//        var employees2 = await repository.GetEmployeesAsync();

//        // Инвалидация кэша
//        repository.InvalidateCache();

//        // Третья загрузка данных (из базы данных)
//        Console.WriteLine("\nТретья загрузка данных...");
//        var employees3 = await repository.GetEmployeesAsync();
//    }
//}

////18
//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Threading.Tasks;

//public class Employee
//{
//    public int EmployeeID { get; set; }
//    public string Name { get; set; }
//    public decimal Salary { get; set; }
//}

//public class EmployeeSearchRepository
//{
//    private readonly string _connectionString;

//    public EmployeeSearchRepository(string connectionString)
//    {
//        _connectionString = connectionString;
//    }

//    public async Task<List<Employee>> SearchEmployeesAsync(string keyword)
//    {
//        List<Employee> employees = new List<Employee>();

//        using (SqlConnection connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            string query = "SELECT EmployeeID, Name, Salary FROM Employees WHERE Name LIKE @Keyword";
//            using (SqlCommand command = new SqlCommand(query, connection))
//            {
//                command.Parameters.AddWithValue("@Keyword", $"%{keyword}%");
//                using (SqlDataReader reader = await command.ExecuteReaderAsync())
//                {
//                    while (await reader.ReadAsync())
//                    {
//                        employees.Add(new Employee
//                        {
//                            EmployeeID = reader.GetInt32(0),
//                            Name = reader.GetString(1),
//                            Salary = reader.GetDecimal(2)
//                        });
//                    }
//                }
//            }
//        }

//        return employees;
//    }
//}

//// Пример использования
//class Program
//{
//    static async Task Main(string[] args)
//    {
//        string connectionString = "Server=your_server;Database=your_database;User Id=your_user;Password=your_password;";
//        EmployeeSearchRepository repository = new EmployeeSearchRepository(connectionString);

//        Console.WriteLine("Введите ключевое слово для поиска:");
//        string keyword = Console.ReadLine();

//        var employees = await repository.SearchEmployeesAsync(keyword);

//        Console.WriteLine("\nРезультаты поиска:");
//        foreach (var employee in employees)
//        {
//            Console.WriteLine($"{employee.EmployeeID}: {employee.Name}, {employee.Salary:C}");
//        }
//    }
//}

////19
//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Threading.Tasks;

//public class EmployeeNullable
//{
//    public int EmployeeID { get; set; }
//    public string Name { get; set; }
//    public decimal Salary { get; set; }
//    public string MiddleName { get; set; }
//    public decimal? Bonus { get; set; }
//}

//public class EmployeeNullableRepository
//{
//    private readonly string _connectionString;

//    public EmployeeNullableRepository(string connectionString)
//    {
//        _connectionString = connectionString;
//    }

//    public async Task<List<EmployeeNullable>> GetEmployeesWithNullableFieldsAsync()
//    {
//        List<EmployeeNullable> employees = new List<EmployeeNullable>();

//        using (SqlConnection connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            string query = "SELECT EmployeeID, Name, Salary, MiddleName, Bonus FROM Employees";
//            using (SqlCommand command = new SqlCommand(query, connection))
//            {
//                using (SqlDataReader reader = await command.ExecuteReaderAsync())
//                {
//                    while (await reader.ReadAsync())
//                    {
//                        employees.Add(new EmployeeNullable
//                        {
//                            EmployeeID = reader.GetInt32(0),
//                            Name = reader.GetString(1),
//                            Salary = reader.GetDecimal(2),
//                            MiddleName = reader.IsDBNull(3) ? null : reader.GetString(3),
//                            Bonus = reader.IsDBNull(4) ? (decimal?)null : reader.GetDecimal(4)
//                        });
//                    }
//                }
//            }
//        }

//        return employees;
//    }

//    public async Task AddEmployeeWithNullableFieldsAsync(EmployeeNullable employee)
//    {
//        using (SqlConnection connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            string query = "INSERT INTO Employees (Name, Salary, MiddleName, Bonus) VALUES (@Name, @Salary, @MiddleName, @Bonus)";
//            using (SqlCommand command = new SqlCommand(query, connection))
//            {
//                command.Parameters.AddWithValue("@Name", employee.Name);
//                command.Parameters.AddWithValue("@Salary", employee.Salary);
//                command.Parameters.AddWithValue("@MiddleName", employee.MiddleName ?? (object)DBNull.Value);
//                command.Parameters.AddWithValue("@Bonus", employee.Bonus ?? (object)DBNull.Value);

//                await command.ExecuteNonQueryAsync();
//            }
//        }
//    }
//}

//// Пример использования
//class Program
//{
//    static async Task Main(string[] args)
//    {
//        string connectionString = "Server=your_server;Database=your_database;User Id=your_user;Password=your_password;";
//        EmployeeNullableRepository repository = new EmployeeNullableRepository(connectionString);

//        // Чтение данных с NULL значениями
//        var employees = await repository.GetEmployeesWithNullableFieldsAsync();

//        Console.WriteLine("Список сотрудников с NULL значениями:");
//        foreach (var employee in employees)
//        {
//            Console.WriteLine($"{employee.EmployeeID}: {employee.Name}, {employee.Salary:C}, " +
//                $"Отчество: {(employee.MiddleName ?? "нет")}, Бонус: {(employee.Bonus.HasValue ? employee.Bonus.Value.ToString("C") : "нет")}");
//        }

//        // Добавление записи с NULL значениями
//        EmployeeNullable newEmployee = new EmployeeNullable
//        {
//            Name = "Новый сотрудник",
//            Salary = 50000,
//            MiddleName = null,
//            Bonus = null
//        };

//        await repository.AddEmployeeWithNullableFieldsAsync(newEmployee);
//        Console.WriteLine("Новый сотрудник добавлен.");
//    }
//}

////20
//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Diagnostics;
//using System.IO;
//using System.Threading.Tasks;

//public class Employee
//{
//    public int EmployeeID { get; set; }
//    public string Name { get; set; }
//    public decimal Salary { get; set; }
//}

//public class LoggedEmployeeRepository
//{
//    private readonly string _connectionString;
//    private readonly string _logFilePath = "sql_queries.log";

//    public LoggedEmployeeRepository(string connectionString)
//    {
//        _connectionString = connectionString;
//    }

//    public async Task<List<Employee>> GetEmployeesAsync()
//    {
//        List<Employee> employees = new List<Employee>();
//        Stopwatch stopwatch = Stopwatch.StartNew();

//        try
//        {
//            using (SqlConnection connection = new SqlConnection(_connectionString))
//            {
//                await connection.OpenAsync();
//                string query = "SELECT EmployeeID, Name, Salary FROM Employees";

//                LogQuery(query, "Start");

//                using (SqlCommand command = new SqlCommand(query, connection))
//                {
//                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
//                    {
//                        while (await reader.ReadAsync())
//                        {
//                            employees.Add(new Employee
//                            {
//                                EmployeeID = reader.GetInt32(0),
//                                Name = reader.GetString(1),
//                                Salary = reader.GetDecimal(2)
//                            });
//                        }
//                    }
//                }
//            }

//            stopwatch.Stop();
//            LogQuery(query, "Success", stopwatch.ElapsedMilliseconds, employees.Count);
//            return employees;
//        }
//        catch (Exception ex)
//        {
//            stopwatch.Stop();
//            LogQuery("SELECT EmployeeID, Name, Salary FROM Employees", "Error", stopwatch.ElapsedMilliseconds, 0, ex.Message);
//            throw;
//        }
//    }

//    private void LogQuery(string query, string status, long executionTime = 0, int resultCount = 0, string errorMessage = null)
//    {
//        string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {status} | Query: {query} | " +
//            $"Execution Time: {executionTime} ms | Result Count: {resultCount}";

//        if (!string.IsNullOrEmpty(errorMessage))
//            logMessage += $" | Error: {errorMessage}";

//        File.AppendAllText(_logFilePath, logMessage + Environment.NewLine);
//    }
//}

//// Пример использования
//class Program
//{
//    static async Task Main(string[] args)
//    {
//        string connectionString = "Server=your_server;Database=your_database;User Id=your_user;Password=your_password;";
//        LoggedEmployeeRepository repository = new LoggedEmployeeRepository(connectionString);

//        try
//        {
//            Console.WriteLine("Загрузка данных из базы данных...");
//            var employees = await repository.GetEmployeesAsync();

//            Console.WriteLine("\nСписок сотрудников:");
//            foreach (var employee in employees)
//            {
//                Console.WriteLine($"{employee.EmployeeID}: {employee.Name}, Зарплата: {employee.Salary:C}");
//            }
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Ошибка: {ex.Message}");
//        }
//    }
//}

////21
//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Threading.Tasks;
//using Microsoft.Extensions.DependencyInjection;

//// Интерфейсы
//public interface IRepository<T> where T : class
//{
//    Task<IEnumerable<T>> GetAllAsync();
//    Task<T> GetByIdAsync(int id);
//    Task AddAsync(T entity);
//    Task UpdateAsync(T entity);
//    Task DeleteAsync(int id);
//}

//public interface IEmployeeRepository : IRepository<Employee>
//{
//    Task<IEnumerable<Employee>> GetByDepartmentAsync(int departmentId);
//}

//public interface IDepartmentRepository : IRepository<Department>
//{
//    Task<IEnumerable<Department>> GetByCompanyAsync(int companyId);
//}

//// Модели
//public class Employee
//{
//    public int Id { get; set; }
//    public string Name { get; set; }
//    public decimal Salary { get; set; }
//    public int DepartmentId { get; set; }
//}

//public class Department
//{
//    public int Id { get; set; }
//    public string Name { get; set; }
//    public int CompanyId { get; set; }
//}

//// Репозитории
//public class EmployeeRepository : IEmployeeRepository
//{
//    private readonly string _connectionString;

//    public EmployeeRepository(string connectionString)
//    {
//        _connectionString = connectionString;
//    }

//    public async Task<IEnumerable<Employee>> GetAllAsync()
//    {
//        var employees = new List<Employee>();
//        using (var connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            var command = new SqlCommand("SELECT * FROM Employees", connection);
//            using (var reader = await command.ExecuteReaderAsync())
//            {
//                while (await reader.ReadAsync())
//                {
//                    employees.Add(new Employee
//                    {
//                        Id = reader.GetInt32(0),
//                        Name = reader.GetString(1),
//                        Salary = reader.GetDecimal(2),
//                        DepartmentId = reader.GetInt32(3)
//                    });
//                }
//            }
//        }
//        return employees;
//    }

//    public async Task<Employee> GetByIdAsync(int id)
//    {
//        using (var connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            var command = new SqlCommand("SELECT * FROM Employees WHERE Id = @Id", connection);
//            command.Parameters.AddWithValue("@Id", id);
//            using (var reader = await command.ExecuteReaderAsync())
//            {
//                if (await reader.ReadAsync())
//                {
//                    return new Employee
//                    {
//                        Id = reader.GetInt32(0),
//                        Name = reader.GetString(1),
//                        Salary = reader.GetDecimal(2),
//                        DepartmentId = reader.GetInt32(3)
//                    };
//                }
//            }
//        }
//        return null;
//    }

//    public async Task AddAsync(Employee entity)
//    {
//        using (var connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            var command = new SqlCommand(
//                "INSERT INTO Employees (Name, Salary, DepartmentId) VALUES (@Name, @Salary, @DepartmentId)",
//                connection);
//            command.Parameters.AddWithValue("@Name", entity.Name);
//            command.Parameters.AddWithValue("@Salary", entity.Salary);
//            command.Parameters.AddWithValue("@DepartmentId", entity.DepartmentId);
//            await command.ExecuteNonQueryAsync();
//        }
//    }

//    public async Task UpdateAsync(Employee entity)
//    {
//        using (var connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            var command = new SqlCommand(
//                "UPDATE Employees SET Name = @Name, Salary = @Salary, DepartmentId = @DepartmentId WHERE Id = @Id",
//                connection);
//            command.Parameters.AddWithValue("@Id", entity.Id);
//            command.Parameters.AddWithValue("@Name", entity.Name);
//            command.Parameters.AddWithValue("@Salary", entity.Salary);
//            command.Parameters.AddWithValue("@DepartmentId", entity.DepartmentId);
//            await command.ExecuteNonQueryAsync();
//        }
//    }

//    public async Task DeleteAsync(int id)
//    {
//        using (var connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            var command = new SqlCommand("DELETE FROM Employees WHERE Id = @Id", connection);
//            command.Parameters.AddWithValue("@Id", id);
//            await command.ExecuteNonQueryAsync();
//        }
//    }

//    public async Task<IEnumerable<Employee>> GetByDepartmentAsync(int departmentId)
//    {
//        var employees = new List<Employee>();
//        using (var connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            var command = new SqlCommand("SELECT * FROM Employees WHERE DepartmentId = @DepartmentId", connection);
//            command.Parameters.AddWithValue("@DepartmentId", departmentId);
//            using (var reader = await command.ExecuteReaderAsync())
//            {
//                while (await reader.ReadAsync())
//                {
//                    employees.Add(new Employee
//                    {
//                        Id = reader.GetInt32(0),
//                        Name = reader.GetString(1),
//                        Salary = reader.GetDecimal(2),
//                        DepartmentId = reader.GetInt32(3)
//                    });
//                }
//            }
//        }
//        return employees;
//    }
//}

//public class DepartmentRepository : IDepartmentRepository
//{
//    private readonly string _connectionString;

//    public DepartmentRepository(string connectionString)
//    {
//        _connectionString = connectionString;
//    }

//    public async Task<IEnumerable<Department>> GetAllAsync()
//    {
//        var departments = new List<Department>();
//        using (var connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            var command = new SqlCommand("SELECT * FROM Departments", connection);
//            using (var reader = await command.ExecuteReaderAsync())
//            {
//                while (await reader.ReadAsync())
//                {
//                    departments.Add(new Department
//                    {
//                        Id = reader.GetInt32(0),
//                        Name = reader.GetString(1),
//                        CompanyId = reader.GetInt32(2)
//                    });
//                }
//            }
//        }
//        return departments;
//    }

//    public async Task<Department> GetByIdAsync(int id)
//    {
//        using (var connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            var command = new SqlCommand("SELECT * FROM Departments WHERE Id = @Id", connection);
//            command.Parameters.AddWithValue("@Id", id);
//            using (var reader = await command.ExecuteReaderAsync())
//            {
//                if (await reader.ReadAsync())
//                {
//                    return new Department
//                    {
//                        Id = reader.GetInt32(0),
//                        Name = reader.GetString(1),
//                        CompanyId = reader.GetInt32(2)
//                    };
//                }
//            }
//        }
//        return null;
//    }

//    public async Task AddAsync(Department entity)
//    {
//        using (var connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            var command = new SqlCommand(
//                "INSERT INTO Departments (Name, CompanyId) VALUES (@Name, @CompanyId)",
//                connection);
//            command.Parameters.AddWithValue("@Name", entity.Name);
//            command.Parameters.AddWithValue("@CompanyId", entity.CompanyId);
//            await command.ExecuteNonQueryAsync();
//        }
//    }

//    public async Task UpdateAsync(Department entity)
//    {
//        using (var connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            var command = new SqlCommand(
//                "UPDATE Departments SET Name = @Name, CompanyId = @CompanyId WHERE Id = @Id",
//                connection);
//            command.Parameters.AddWithValue("@Id", entity.Id);
//            command.Parameters.AddWithValue("@Name", entity.Name);
//            command.Parameters.AddWithValue("@CompanyId", entity.CompanyId);
//            await command.ExecuteNonQueryAsync();
//        }
//    }

//    public async Task DeleteAsync(int id)
//    {
//        using (var connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            var command = new SqlCommand("DELETE FROM Departments WHERE Id = @Id", connection);
//            command.Parameters.AddWithValue("@Id", id);
//            await command.ExecuteNonQueryAsync();
//        }
//    }

//    public async Task<IEnumerable<Department>> GetByCompanyAsync(int companyId)
//    {
//        var departments = new List<Department>();
//        using (var connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            var command = new SqlCommand("SELECT * FROM Departments WHERE CompanyId = @CompanyId", connection);
//            command.Parameters.AddWithValue("@CompanyId", companyId);
//            using (var reader = await command.ExecuteReaderAsync())
//            {
//                while (await reader.ReadAsync())
//                {
//                    departments.Add(new Department
//                    {
//                        Id = reader.GetInt32(0),
//                        Name = reader.GetString(1),
//                        CompanyId = reader.GetInt32(2)
//                    });
//                }
//            }
//        }
//        return departments;
//    }
//}

//// Внедрение зависимостей
//class Program
//{
//    static async Task Main(string[] args)
//    {
//        var services = new ServiceCollection();
//        services.AddTransient<IEmployeeRepository>(provider =>
//            new EmployeeRepository("Server=your_server;Database=your_database;User Id=your_user;Password=your_password;"));
//        services.AddTransient<IDepartmentRepository>(provider =>
//            new DepartmentRepository("Server=your_server;Database=your_database;User Id=your_user;Password=your_password;"));

//        var serviceProvider = services.BuildServiceProvider();

//        var employeeRepository = serviceProvider.GetService<IEmployeeRepository>();
//        var departmentRepository = serviceProvider.GetService<IDepartmentRepository>();

//        // Пример использования
//        var employees = await employeeRepository.GetAllAsync();
//        foreach (var employee in employees)
//        {
//            Console.WriteLine($"{employee.Id}: {employee.Name}, {employee.Salary:C}");
//        }
//    }
//}


////22
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.Reflection;
//using System.Threading.Tasks;

//public static class DataMapper<T> where T : new()
//{
//    public static async Task<IEnumerable<T>> MapAsync(SqlDataReader reader)
//    {
//        var results = new List<T>();
//        var properties = typeof(T).GetProperties();

//        while (await reader.ReadAsync())
//        {
//            var entity = new T();
//            foreach (var property in properties)
//            {
//                if (reader.HasColumn(property.Name) && !reader.IsDBNull(reader.GetOrdinal(property.Name)))
//                {
//                    var value = reader[property.Name];
//                    property.SetValue(entity, value);
//                }
//            }
//            results.Add(entity);
//        }

//        return results;
//    }

//    public static async Task<T> MapSingleAsync(SqlDataReader reader)
//    {
//        if (await reader.ReadAsync())
//        {
//            var entity = new T();
//            var properties = typeof(T).GetProperties();

//            foreach (var property in properties)
//            {
//                if (reader.HasColumn(property.Name) && !reader.IsDBNull(reader.GetOrdinal(property.Name)))
//                {
//                    var value = reader[property.Name];
//                    property.SetValue(entity, value);
//                }
//            }

//            return entity;
//        }

//        return default;
//    }

//    private static bool HasColumn(this IDataRecord reader, string columnName)
//    {
//        for (int i = 0; i < reader.FieldCount; i++)
//        {
//            if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
//                return true;
//        }
//        return false;
//    }
//}

//// Пример использования
//public class EmployeeRepositoryWithMapper
//{
//    private readonly string _connectionString;

//    public EmployeeRepositoryWithMapper(string connectionString)
//    {
//        _connectionString = connectionString;
//    }

//    public async Task<IEnumerable<Employee>> GetAllAsync()
//    {
//        using (var connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            var command = new SqlCommand("SELECT * FROM Employees", connection);
//            using (var reader = await command.ExecuteReaderAsync())
//            {
//                return await DataMapper<Employee>.MapAsync(reader);
//            }
//        }
//    }

//    public async Task<Employee> GetByIdAsync(int id)
//    {
//        using (var connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            var command = new SqlCommand("SELECT * FROM Employees WHERE Id = @Id", connection);
//            command.Parameters.AddWithValue("@Id", id);
//            using (var reader = await command.ExecuteReaderAsync())
//            {
//                return await DataMapper<Employee>.MapSingleAsync(reader);
//            }
//        }
//    }
//}

//// Пример использования
//class Program
//{
//    static async Task Main(string[] args)
//    {
//        var repository = new EmployeeRepositoryWithMapper("Server=your_server;Database=your_database;User Id=your_user;Password=your_password;");

//        var employees = await repository.GetAllAsync();
//        foreach (var employee in employees)
//        {
//            Console.WriteLine($"{employee.Id}: {employee.Name}, {employee.Salary:C}");
//        }
//    }
//}

////23
//using System;
//using System.Data;
//using System.Data.SqlClient;
//using System.Threading.Tasks;

//public interface IUnitOfWork : IDisposable
//{
//    IEmployeeRepository Employees { get; }
//    IDepartmentRepository Departments { get; }
//    Task<int> CommitAsync();
//    Task RollbackAsync();
//}

//public class UnitOfWork : IUnitOfWork
//{
//    private readonly string _connectionString;
//    private SqlConnection _connection;
//    private SqlTransaction _transaction;
//    private IEmployeeRepository _employeeRepository;
//    private IDepartmentRepository _departmentRepository;

//    public UnitOfWork(string connectionString)
//    {
//        _connectionString = connectionString;
//    }

//    public IEmployeeRepository Employees => _employeeRepository ??= new EmployeeRepository(_connection, _transaction);
//    public IDepartmentRepository Departments => _departmentRepository ??= new DepartmentRepository(_connection, _transaction);

//    public async Task<int> CommitAsync()
//    {
//        if (_transaction == null)
//            throw new InvalidOperationException("Транзакция не начата.");

//        try
//        {
//            await _transaction.CommitAsync();
//            return 1;
//        }
//        catch
//        {
//            await _transaction.RollbackAsync();
//            throw;
//        }
//        finally
//        {
//            _transaction.Dispose();
//            _transaction = null;
//            _connection.Close();
//        }
//    }

//    public async Task RollbackAsync()
//    {
//        if (_transaction != null)
//        {
//            await _transaction.RollbackAsync();
//            _transaction.Dispose();
//            _transaction = null;
//        }
//        _connection.Close();
//    }

//    public void Dispose()
//    {
//        if (_transaction != null)
//        {
//            _transaction.Rollback();
//            _transaction.Dispose();
//        }
//        if (_connection != null)
//        {
//            _connection.Close();
//            _connection.Dispose();
//        }
//    }

//    private SqlConnection Connection
//    {
//        get
//        {
//            if (_connection == null)
//            {
//                _connection = new SqlConnection(_connectionString);
//                _connection.Open();
//                _transaction = _connection.BeginTransaction();
//            }
//            return _connection;
//        }
//    }
//}

//// Репозитории с поддержкой транзакций
//public class EmployeeRepositoryWithTransaction : IEmployeeRepository
//{
//    private readonly SqlConnection _connection;
//    private readonly SqlTransaction _transaction;

//    public EmployeeRepositoryWithTransaction(SqlConnection connection, SqlTransaction transaction)
//    {
//        _connection = connection;
//        _transaction = transaction;
//    }

//    public async Task<IEnumerable<Employee>> GetAllAsync()
//    {
//        var employees = new List<Employee>();
//        var command = new SqlCommand("SELECT * FROM Employees", _connection, _transaction);
//        using (var reader = await command.ExecuteReaderAsync())
//        {
//            while (await reader.ReadAsync())
//            {
//                employees.Add(new Employee
//                {
//                    Id = reader.GetInt32(0),
//                    Name = reader.GetString(1),
//                    Salary = reader.GetDecimal(2),
//                    DepartmentId = reader.GetInt32(3)
//                });
//            }
//        }
//        return employees;
//    }

//    public async Task AddAsync(Employee entity)
//    {
//        var command = new SqlCommand(
//            "INSERT INTO Employees (Name, Salary, DepartmentId) VALUES (@Name, @Salary, @DepartmentId)",
//            _connection, _transaction);
//        command.Parameters.AddWithValue("@Name", entity.Name);
//        command.Parameters.AddWithValue("@Salary", entity.Salary);
//        command.Parameters.AddWithValue("@DepartmentId", entity.DepartmentId);
//        await command.ExecuteNonQueryAsync();
//    }

//    public async Task UpdateAsync(Employee entity)
//    {
//        var command = new SqlCommand(
//            "UPDATE Employees SET Name = @Name, Salary = @Salary, DepartmentId = @DepartmentId WHERE Id = @Id",
//            _connection, _transaction);
//        command.Parameters.AddWithValue("@Id", entity.Id);
//        command.Parameters.AddWithValue("@Name", entity.Name);
//        command.Parameters.AddWithValue("@Salary", entity.Salary);
//        command.Parameters.AddWithValue("@DepartmentId", entity.DepartmentId);
//        await command.ExecuteNonQueryAsync();
//    }

//    public async Task DeleteAsync(int id)
//    {
//        var command = new SqlCommand("DELETE FROM Employees WHERE Id = @Id", _connection, _transaction);
//        command.Parameters.AddWithValue("@Id", id);
//        await command.ExecuteNonQueryAsync();
//    }

//    public async Task<Employee> GetByIdAsync(int id)
//    {
//        var command = new SqlCommand("SELECT * FROM Employees WHERE Id = @Id", _connection, _transaction);
//        command.Parameters.AddWithValue("@Id", id);
//        using (var reader = await command.ExecuteReaderAsync())
//        {
//            if (await reader.ReadAsync())
//            {
//                return new Employee
//                {
//                    Id = reader.GetInt32(0),
//                    Name = reader.GetString(1),
//                    Salary = reader.GetDecimal(2),
//                    DepartmentId = reader.GetInt32(3)
//                };
//            }
//        }
//        return null;
//    }

//    public async Task<IEnumerable<Employee>> GetByDepartmentAsync(int departmentId)
//    {
//        var employees = new List<Employee>();
//        var command = new SqlCommand("SELECT * FROM Employees WHERE DepartmentId = @DepartmentId", _connection, _transaction);
//        command.Parameters.AddWithValue("@DepartmentId", departmentId);
//        using (var reader = await command.ExecuteReaderAsync())
//        {
//            while (await reader.ReadAsync())
//            {
//                employees.Add(new Employee
//                {
//                    Id = reader.GetInt32(0),
//                    Name = reader.GetString(1),
//                    Salary = reader.GetDecimal(2),
//                    DepartmentId = reader.GetInt32(3)
//                });
//            }
//        }
//        return employees;
//    }

//    public Task<IEnumerable<Employee>> FindByPredicateAsync(Expression<Func<Employee, bool>> predicate)
//    {
//        throw new NotImplementedException();
//    }

//    public Task<IEnumerable<Employee>> GetPagedAsync(int pageNumber, int pageSize)
//    {
//        throw new NotImplementedException();
//    }
//}

//// Пример использования
//class Program
//{
//    static async Task Main(string[] args)
//    {
//        using (var unitOfWork = new UnitOfWork("Server=your_server;Database=your_database;User Id=your_user;Password=your_password;"))
//        {
//            try
//            {
//                var employee = new Employee { Name = "Иван Иванов", Salary = 50000, DepartmentId = 1 };
//                await unitOfWork.Employees.AddAsync(employee);

//                var department = new Department { Name = "IT", CompanyId = 1 };
//                await unitOfWork.Departments.AddAsync(department);

//                await unitOfWork.CommitAsync();
//                Console.WriteLine("Транзакция успешно завершена.");
//            }
//            catch (Exception ex)
//            {
//                await unitOfWork.RollbackAsync();
//                Console.WriteLine($"Ошибка: {ex.Message}");
//            }
//        }
//    }
//}

////24
//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Threading.Tasks;

//public interface IRepository<T> where T : class
//{
//    Task<IEnumerable<T>> GetAllAsync();
//    Task<T> GetByIdAsync(int id);
//    Task<IEnumerable<T>> FindByPredicateAsync(Expression<Func<T, bool>> predicate);
//    Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize);
//    Task AddAsync(T entity);
//    Task UpdateAsync(T entity);
//    Task DeleteAsync(int id);
//}

//public class EmployeeRepository : IRepository<Employee>
//{
//    private readonly string _connectionString;

//    public EmployeeRepository(string connectionString)
//    {
//        _connectionString = connectionString;
//    }

//    public async Task<IEnumerable<Employee>> GetAllAsync()
//    {
//        var employees = new List<Employee>();
//        using (var connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            var command = new SqlCommand("SELECT * FROM Employees", connection);
//            using (var reader = await command.ExecuteReaderAsync())
//            {
//                while (await reader.ReadAsync())
//                {
//                    employees.Add(new Employee
//                    {
//                        Id = reader.GetInt32(0),
//                        Name = reader.GetString(1),
//                        Salary = reader.GetDecimal(2),
//                        DepartmentId = reader.GetInt32(3)
//                    });
//                }
//            }
//        }
//        return employees;
//    }

//    public async Task<Employee> GetByIdAsync(int id)
//    {
//        using (var connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            var command = new SqlCommand("SELECT * FROM Employees WHERE Id = @Id", connection);
//            command.Parameters.AddWithValue("@Id", id);
//            using (var reader = await command.ExecuteReaderAsync())
//            {
//                if (await reader.ReadAsync())
//                {
//                    return new Employee
//                    {
//                        Id = reader.GetInt32(0),
//                        Name = reader.GetString(1),
//                        Salary = reader.GetDecimal(2),
//                        DepartmentId = reader.GetInt32(3)
//                    };
//                }
//            }
//        }
//        return null;
//    }

//    public async Task<IEnumerable<Employee>> FindByPredicateAsync(Expression<Func<Employee, bool>> predicate)
//    {
//        var employees = (await GetAllAsync()).AsQueryable();
//        return employees.Where(predicate).ToList();
//    }

//    public async Task<IEnumerable<Employee>> GetPagedAsync(int pageNumber, int pageSize)
//    {
//        var employees = (await GetAllAsync()).AsQueryable();
//        return employees.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
//    }

//    public async Task AddAsync(Employee entity)
//    {
//        using (var connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            var command = new SqlCommand(
//                "INSERT INTO Employees (Name, Salary, DepartmentId) VALUES (@Name, @Salary, @DepartmentId)",
//                connection);
//            command.Parameters.AddWithValue("@Name", entity.Name);
//            command.Parameters.AddWithValue("@Salary", entity.Salary);
//            command.Parameters.AddWithValue("@DepartmentId", entity.DepartmentId);
//            await command.ExecuteNonQueryAsync();
//        }
//    }

//    public async Task UpdateAsync(Employee entity)
//    {
//        using (var connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            var command = new SqlCommand(
//                "UPDATE Employees SET Name = @Name, Salary = @Salary, DepartmentId = @DepartmentId WHERE Id = @Id",
//                connection);
//            command.Parameters.AddWithValue("@Id", entity.Id);
//            command.Parameters.AddWithValue("@Name", entity.Name);
//            command.Parameters.AddWithValue("@Salary", entity.Salary);
//            command.Parameters.AddWithValue("@DepartmentId", entity.DepartmentId);
//            await command.ExecuteNonQueryAsync();
//        }
//    }

//    public async Task DeleteAsync(int id)
//    {
//        using (var connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            var command = new SqlCommand("DELETE FROM Employees WHERE Id = @Id", connection);
//            command.Parameters.AddWithValue("@Id", id);
//            await command.ExecuteNonQueryAsync();
//        }
//    }
//}

//// Пример использования
//class Program
//{
//    static async Task Main(string[] args)
//    {
//        var repository = new EmployeeRepository("Server=your_server;Database=your_database;User Id=your_user;Password=your_password;");

//        // Поиск по предикату
//        var highSalaryEmployees = await repository.FindByPredicateAsync(e => e.Salary > 50000);
//        Console.WriteLine("Сотрудники с зарплатой больше 50000:");
//        foreach (var employee in highSalaryEmployees)
//        {
//            Console.WriteLine($"{employee.Id}: {employee.Name}, {employee.Salary:C}");
//        }

//        // Пагинация
//        var pagedEmployees = await repository.GetPagedAsync(1, 10);
//        Console.WriteLine("\nПервая страница сотрудников:");
//        foreach (var employee in pagedEmployees)
//        {
//            Console.WriteLine($"{employee.Id}: {employee.Name}, {employee.Salary:C}");
//        }
//    }
//}

////25
//using System;
//using System.Data;
//using System.Data.SqlClient;
//using System.Threading.Tasks;

//// Пример перечисления
//public enum EmployeeStatus
//{
//    Active = 1,
//    OnLeave = 2,
//    Terminated = 3
//}

//public class EmployeeWithStatus
//{
//    public int Id { get; set; }
//    public string Name { get; set; }
//    public decimal Salary { get; set; }
//    public EmployeeStatus Status { get; set; }
//}

//public class EmployeeStatusRepository
//{
//    private readonly string _connectionString;

//    public EmployeeStatusRepository(string connectionString)
//    {
//        _connectionString = connectionString;
//    }

//    public async Task<IEnumerable<EmployeeWithStatus>> GetAllAsync()
//    {
//        var employees = new List<EmployeeWithStatus>();
//        using (var connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            var command = new SqlCommand("SELECT * FROM EmployeesWithStatus", connection);
//            using (var reader = await command.ExecuteReaderAsync())
//            {
//                while (await reader.ReadAsync())
//                {
//                    employees.Add(new EmployeeWithStatus
//                    {
//                        Id = reader.GetInt32(0),
//                        Name = reader.GetString(1),
//                        Salary = reader.GetDecimal(2),
//                        Status = (EmployeeStatus)reader.GetInt32(3)
//                    });
//                }
//            }
//        }
//        return employees;
//    }

//    public async Task AddAsync(EmployeeWithStatus entity)
//    {
//        using (var connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            var command = new SqlCommand(
//                "INSERT INTO EmployeesWithStatus (Name, Salary, Status) VALUES (@Name, @Salary, @Status)",
//                connection);
//            command.Parameters.AddWithValue("@Name", entity.Name);
//            command.Parameters.AddWithValue("@Salary", entity.Salary);
//            command.Parameters.AddWithValue("@Status", (int)entity.Status);
//            await command.ExecuteNonQueryAsync();
//        }
//    }

//    public async Task UpdateAsync(EmployeeWithStatus entity)
//    {
//        using (var connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            var command = new SqlCommand(
//                "UPDATE EmployeesWithStatus SET Name = @Name, Salary = @Salary, Status = @Status WHERE Id = @Id",
//                connection);
//            command.Parameters.AddWithValue("@Id", entity.Id);
//            command.Parameters.AddWithValue("@Name", entity.Name);
//            command.Parameters.AddWithValue("@Salary", entity.Salary);
//            command.Parameters.AddWithValue("@Status", (int)entity.Status);
//            await command.ExecuteNonQueryAsync();
//        }
//    }
//}

//// Пример использования
//class Program
//{
//    static async Task Main(string[] args)
//    {
//        var repository = new EmployeeStatusRepository("Server=your_server;Database=your_database;User Id=your_user;Password=your_password;");

//        // Добавление сотрудника с перечислением
//        var employee = new EmployeeWithStatus
//        {
//            Name = "Иван Иванов",
//            Salary = 50000,
//            Status = EmployeeStatus.Active
//        };

//        await repository.AddAsync(employee);
//        Console.WriteLine("Сотрудник добавлен.");

//        // Получение сотрудников
//        var employees = await repository.GetAllAsync();
//        Console.WriteLine("\nСписок сотрудников:");
//        foreach (var emp in employees)
//        {
//            Console.WriteLine($"{emp.Id}: {emp.Name}, {emp.Salary:C}, Статус: {emp.Status}");
//        }
//    }
//}

////26
//using System;
//using System.Data;
//using System.Data.SqlClient;
//using System.Threading.Tasks;
//using Newtonsoft.Json;

//public class Employee
//{
//    public int Id { get; set; }
//    public string Name { get; set; }
//    public decimal Salary { get; set; }
//    public string AddressJson { get; set; } // JSON поле
//}

//public class Address
//{
//    public string City { get; set; }
//    public string Street { get; set; }
//    public string PostalCode { get; set; }
//}

//public class EmployeeJsonRepository
//{
//    private readonly string _connectionString;

//    public EmployeeJsonRepository(string connectionString)
//    {
//        _connectionString = connectionString;
//    }

//    // Сохранение объекта с сериализацией JSON
//    public async Task AddEmployeeWithAddressAsync(Employee employee, Address address)
//    {
//        employee.AddressJson = JsonConvert.SerializeObject(address);

//        using (var connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            var command = new SqlCommand(
//                "INSERT INTO Employees (Name, Salary, AddressJson) VALUES (@Name, @Salary, @AddressJson)",
//                connection);
//            command.Parameters.AddWithValue("@Name", employee.Name);
//            command.Parameters.AddWithValue("@Salary", employee.Salary);
//            command.Parameters.AddWithValue("@AddressJson", employee.AddressJson);
//            await command.ExecuteNonQueryAsync();
//        }
//    }

//    // Получение объекта с десериализацией JSON
//    public async Task<Employee> GetEmployeeWithAddressAsync(int id)
//    {
//        using (var connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            var command = new SqlCommand(
//                "SELECT Id, Name, Salary, AddressJson FROM Employees WHERE Id = @Id",
//                connection);
//            command.Parameters.AddWithValue("@Id", id);

//            using (var reader = await command.ExecuteReaderAsync())
//            {
//                if (await reader.ReadAsync())
//                {
//                    var employee = new Employee
//                    {
//                        Id = reader.GetInt32(0),
//                        Name = reader.GetString(1),
//                        Salary = reader.GetDecimal(2),
//                        AddressJson = reader.GetString(3)
//                    };

//                    return employee;
//                }
//            }
//        }
//        return null;
//    }

//    // Использование JSON_VALUE и JSON_QUERY
//    public async Task<string> GetEmployeeCityAsync(int id)
//    {
//        using (var connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            var command = new SqlCommand(
//                "SELECT JSON_VALUE(AddressJson, '$.City') FROM Employees WHERE Id = @Id",
//                connection);
//            command.Parameters.AddWithValue("@Id", id);

//            return (await command.ExecuteScalarAsync())?.ToString();
//        }
//    }

//    // Получение адреса как JSON
//    public async Task<string> GetEmployeeAddressJsonAsync(int id)
//    {
//        using (var connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            var command = new SqlCommand(
//                "SELECT JSON_QUERY(AddressJson) FROM Employees WHERE Id = @Id",
//                connection);
//            command.Parameters.AddWithValue("@Id", id);

//            return (await command.ExecuteScalarAsync())?.ToString();
//        }
//    }
//}

//// Пример использования
//class Program
//{
//    static async Task Main(string[] args)
//    {
//        var connectionString = "Server=your_server;Database=your_database;User Id=your_user;Password=your_password;";
//        var repository = new EmployeeJsonRepository(connectionString);

//        // Добавление сотрудника с адресом
//        var employee = new Employee { Name = "Иван Иванов", Salary = 50000 };
//        var address = new Address { City = "Москва", Street = "Ленина", PostalCode = "123456" };

//        await repository.AddEmployeeWithAddressAsync(employee, address);
//        Console.WriteLine("Сотрудник с адресом добавлен.");

//        // Получение сотрудника с адресом
//        var savedEmployee = await repository.GetEmployeeWithAddressAsync(1);
//        var deserializedAddress = JsonConvert.DeserializeObject<Address>(savedEmployee.AddressJson);
//        Console.WriteLine($"\nСотрудник: {savedEmployee.Name}, Город: {deserializedAddress.City}");

//        // Получение города сотрудника
//        var city = await repository.GetEmployeeCityAsync(1);
//        Console.WriteLine($"\nГород сотрудника: {city}");

//        // Получение адреса как JSON
//        var addressJson = await repository.GetEmployeeAddressJsonAsync(1);
//        Console.WriteLine($"\nАдрес сотрудника (JSON): {addressJson}");
//    }
//}

////27
//using System;
//using System.Data.SqlClient;
//using System.Diagnostics;
//using System.Threading.Tasks;

//public class QueryOptimizer
//{
//    private readonly string _connectionString;

//    public QueryOptimizer(string connectionString)
//    {
//        _connectionString = connectionString;
//    }

//    // Исходный медленный запрос
//    public async Task<TimeSpan> ExecuteSlowQueryAsync()
//    {
//        var stopwatch = Stopwatch.StartNew();

//        using (var connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            var command = new SqlCommand(
//                "SELECT * FROM Employees WHERE Salary > 50000 ORDER BY Name", connection);
//            using (var reader = await command.ExecuteReaderAsync())
//            {
//                while (await reader.ReadAsync()) { }
//            }
//        }

//        stopwatch.Stop();
//        return stopwatch.Elapsed;
//    }

//    // Оптимизированный запрос с индексами
//    public async Task<TimeSpan> ExecuteOptimizedQueryAsync()
//    {
//        var stopwatch = Stopwatch.StartNew();

//        using (var connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            var command = new SqlCommand(
//                "SELECT Id, Name, Salary FROM Employees WHERE Salary > 50000 ORDER BY Name", connection);
//            using (var reader = await command.ExecuteReaderAsync())
//            {
//                while (await reader.ReadAsync()) { }
//            }
//        }

//        stopwatch.Stop();
//        return stopwatch.Elapsed;
//    }

//    // Создание индексов
//    public async Task CreateIndexesAsync()
//    {
//        using (var connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();

//            // Индекс для поля Salary
//            var command = new SqlCommand(
//                "IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Employees_Salary') " +
//                "CREATE INDEX IX_Employees_Salary ON Employees(Salary)", connection);
//            await command.ExecuteNonQueryAsync();

//            // Индекс для поля Name
//            command = new SqlCommand(
//                "IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Employees_Name') " +
//                "CREATE INDEX IX_Employees_Name ON Employees(Name)", connection);
//            await command.ExecuteNonQueryAsync();
//        }
//    }
//}

//// Пример использования
//class Program
//{
//    static async Task Main(string[] args)
//    {
//        var connectionString = "Server=your_server;Database=your_database;User Id=your_user;Password=your_password;";
//        var optimizer = new QueryOptimizer(connectionString);

//        // Создание индексов
//        await optimizer.CreateIndexesAsync();
//        Console.WriteLine("Индексы созданы.");

//        // Выполнение медленного запроса
//        var slowTime = await optimizer.ExecuteSlowQueryAsync();
//        Console.WriteLine($"Время выполнения медленного запроса: {slowTime.TotalMilliseconds} мс");

//        // Выполнение оптимизированного запроса
//        var optimizedTime = await optimizer.ExecuteOptimizedQueryAsync();
//        Console.WriteLine($"Время выполнения оптимизированного запроса: {optimizedTime.TotalMilliseconds} мс");
//    }
//}

////28
//using System;
//using System.Data.SqlClient;
//using System.Threading.Tasks;

//public class ConnectionPoolManager
//{
//    private readonly string _connectionString;

//    public ConnectionPoolManager(string connectionString)
//    {
//        _connectionString = connectionString;
//    }

//    // Настройка пула соединений
//    public string GetConnectionStringWithPoolSettings()
//    {
//        var builder = new SqlConnectionStringBuilder(_connectionString)
//        {
//            MaxPoolSize = 100,  // Максимальное количество соединений в пуле
//            MinPoolSize = 5,    // Минимальное количество соединений в пуле
//            Pooling = true,     // Включение пула соединений
//            ConnectTimeout = 15 // Таймаут подключения
//        };

//        return builder.ConnectionString;
//    }

//    // Пример использования пула соединений
//    public async Task ExecuteQueriesInParallelAsync(int queryCount)
//    {
//        var connectionString = GetConnectionStringWithPoolSettings();
//        var tasks = new Task[queryCount];

//        for (int i = 0; i < queryCount; i++)
//        {
//            int queryId = i;
//            tasks[i] = Task.Run(async () =>
//            {
//                using (var connection = new SqlConnection(connectionString))
//                {
//                    await connection.OpenAsync();
//                    var command = new SqlCommand(
//                        $"SELECT 1 AS QueryId, GETDATE() AS ExecutionTime WHERE 1 = {queryId % 2}",
//                        connection);
//                    using (var reader = await command.ExecuteReaderAsync())
//                    {
//                        while (await reader.ReadAsync())
//                        {
//                            Console.WriteLine($"Query {queryId}: {reader["ExecutionTime"]}");
//                        }
//                    }
//                }
//            });
//        }

//        await Task.WhenAll(tasks);
//    }
//}

//// Пример использования
//class Program
//{
//    static async Task Main(string[] args)
//    {
//        var connectionString = "Server=your_server;Database=your_database;User Id=your_user;Password=your_password;";
//        var poolManager = new ConnectionPoolManager(connectionString);

//        // Настройка пула соединений
//        var optimizedConnectionString = poolManager.GetConnectionStringWithPoolSettings();
//        Console.WriteLine($"Оптимизированная строка подключения: {optimizedConnectionString}");

//        // Параллельное выполнение запросов
//        await poolManager.ExecuteQueriesInParallelAsync(10);
//    }
//}

////29
//using System;
//using System.Data.SqlClient;
//using System.IO;
//using System.Threading.Tasks;

//public class MigrationManager
//{
//    private readonly string _connectionString;
//    private readonly string _migrationsDirectory = "Migrations";

//    public MigrationManager(string connectionString)
//    {
//        _connectionString = connectionString;
//    }

//    // Применение миграций
//    public async Task ApplyMigrationsAsync()
//    {
//        // Создание таблицы версий, если она не существует
//        await EnsureVersionTableExistsAsync();

//        // Получение текущей версии
//        var currentVersion = await GetCurrentVersionAsync();

//        // Получение списка файлов миграций
//        var migrationFiles = Directory.GetFiles(_migrationsDirectory, "*.sql");
//        Array.Sort(migrationFiles);

//        // Применение миграций
//        foreach (var file in migrationFiles)
//        {
//            var fileName = Path.GetFileNameWithoutExtension(file);
//            var version = int.Parse(fileName.Split('_')[0]);

//            if (version > currentVersion)
//            {
//                var sql = await File.ReadAllTextAsync(file);
//                await ExecuteMigrationAsync(version, sql);
//                Console.WriteLine($"Применена миграция {version}");
//            }
//        }
//    }

//    // Откат миграции
//    public async Task RollbackMigrationAsync(int version)
//    {
//        var rollbackScript = Path.Combine(_migrationsDirectory, $"{version}_rollback.sql");
//        if (File.Exists(rollbackScript))
//        {
//            var sql = await File.ReadAllTextAsync(rollbackScript);
//            await ExecuteMigrationAsync(version, sql, isRollback: true);
//            Console.WriteLine($"Откат миграции {version}");
//        }
//        else
//        {
//            Console.WriteLine($"Файл отката для миграции {version} не найден.");
//        }
//    }

//    // Создание таблицы версий
//    private async Task EnsureVersionTableExistsAsync()
//    {
//        using (var connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            var command = new SqlCommand(
//                "IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SchemaVersions') " +
//                "CREATE TABLE SchemaVersions (Version INT PRIMARY KEY, AppliedDate DATETIME)",
//                connection);
//            await command.ExecuteNonQueryAsync();
//        }
//    }

//    // Получение текущей версии
//    private async Task<int> GetCurrentVersionAsync()
//    {
//        using (var connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            var command = new SqlCommand(
//                "SELECT ISNULL(MAX(Version), 0) FROM SchemaVersions",
//                connection);
//            return (int)await command.ExecuteScalarAsync();
//        }
//    }

//    // Выполнение миграции
//    private async Task ExecuteMigrationAsync(int version, string sql, bool isRollback = false)
//    {
//        using (var connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            using (var transaction = connection.BeginTransaction())
//            {
//                try
//                {
//                    // Выполнение SQL скрипта
//                    var commands = sql.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);
//                    foreach (var cmd in commands)
//                    {
//                        using (var command = new SqlCommand(cmd, connection, transaction))
//                        {
//                            await command.ExecuteNonQueryAsync();
//                        }
//                    }

//                    // Обновление версии
//                    var versionCommand = isRollback
//                        ? new SqlCommand("DELETE FROM SchemaVersions WHERE Version = @Version", connection, transaction)
//                        : new SqlCommand("INSERT INTO SchemaVersions (Version, AppliedDate) VALUES (@Version, GETDATE())", connection, transaction);

//                    versionCommand.Parameters.AddWithValue("@Version", version);
//                    await versionCommand.ExecuteNonQueryAsync();

//                    transaction.Commit();
//                }
//                catch
//                {
//                    transaction.Rollback();
//                    throw;
//                }
//            }
//        }
//    }
//}

//// Пример использования
//class Program
//{
//    static async Task Main(string[] args)
//    {
//        var connectionString = "Server=your_server;Database=your_database;User Id=your_user;Password=your_password;";
//        var migrationManager = new MigrationManager(connectionString);

//        // Применение миграций
//        await migrationManager.ApplyMigrationsAsync();

//        // Откат миграции
//        await migrationManager.RollbackMigrationAsync(1);
//    }
//}

////30
//using System;
//using System.Data.SqlClient;
//using System.Threading.Tasks;
//using Xunit;

//public class EmployeeRepositoryTests : IDisposable
//{
//    private readonly string _connectionString;
//    private readonly SqlConnection _connection;

//    public EmployeeRepositoryTests()
//    {
//        // Использование in-memory базы данных или тестовой базы данных
//        _connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=TestDatabase;Integrated Security=True;";
//        _connection = new SqlConnection(_connectionString);
//        _connection.Open();

//        // Создание тестовой таблицы
//        var command = new SqlCommand(
//            "IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Employees') " +
//            "CREATE TABLE Employees (Id INT IDENTITY(1,1) PRIMARY KEY, Name NVARCHAR(100), Salary DECIMAL(18,2))",
//            _connection);
//        command.ExecuteNonQuery();
//    }

//    public void Dispose()
//    {
//        // Очистка тестовой таблицы
//        var command = new SqlCommand("DROP TABLE Employees", _connection);
//        command.ExecuteNonQuery();
//        _connection.Close();
//        _connection.Dispose();
//    }

//    [Fact]
//    public async Task AddEmployeeAsync_ShouldAddEmployee()
//    {
//        // Arrange
//        var repository = new EmployeeRepository(_connectionString);
//        var employee = new Employee { Name = "Иван Иванов", Salary = 50000 };

//        // Act
//        await repository.AddAsync(employee);

//        // Assert
//        var savedEmployee = await repository.GetByIdAsync(1);
//        Assert.NotNull(savedEmployee);
//        Assert.Equal("Иван Иванов", savedEmployee.Name);
//        Assert.Equal(50000, savedEmployee.Salary);
//    }

//    [Fact]
//    public async Task UpdateEmployeeAsync_ShouldUpdateEmployee()
//    {
//        // Arrange
//        var repository = new EmployeeRepository(_connectionString);
//        var employee = new Employee { Name = "Иван Иванов", Salary = 50000 };
//        await repository.AddAsync(employee);

//        // Act
//        employee.Name = "Петр Петров";
//        employee.Salary = 60000;
//        await repository.UpdateAsync(employee);

//        // Assert
//        var updatedEmployee = await repository.GetByIdAsync(1);
//        Assert.Equal("Петр Петров", updatedEmployee.Name);
//        Assert.Equal(60000, updatedEmployee.Salary);
//    }

//    [Fact]
//    public async Task DeleteEmployeeAsync_ShouldDeleteEmployee()
//    {
//        // Arrange
//        var repository = new EmployeeRepository(_connectionString);
//        var employee = new Employee { Name = "Иван Иванов", Salary = 50000 };
//        await repository.AddAsync(employee);

//        // Act
//        await repository.DeleteAsync(1);

//        // Assert
//        var deletedEmployee = await repository.GetByIdAsync(1);
//        Assert.Null(deletedEmployee);
//    }

//    [Fact]
//    public async Task GetAllEmployeesAsync_ShouldReturnAllEmployees()
//    {
//        // Arrange
//        var repository = new EmployeeRepository(_connectionString);
//        await repository.AddAsync(new Employee { Name = "Иван Иванов", Salary = 50000 });
//        await repository.AddAsync(new Employee { Name = "Петр Петров", Salary = 60000 });

//        // Act
//        var employees = await repository.GetAllAsync();

//        // Assert
//        Assert.Equal(2, employees.Count);
//    }
//}

//public class EmployeeRepository
//{
//    private readonly string _connectionString;

//    public EmployeeRepository(string connectionString)
//    {
//        _connectionString = connectionString;
//    }

//    public async Task<IEnumerable<Employee>> GetAllAsync()
//    {
//        var employees = new List<Employee>();
//        using (var connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            var command = new SqlCommand("SELECT * FROM Employees", connection);
//            using (var reader = await command.ExecuteReaderAsync())
//            {
//                while (await reader.ReadAsync())
//                {
//                    employees.Add(new Employee
//                    {
//                        Id = reader.GetInt32(0),
//                        Name = reader.GetString(1),
//                        Salary = reader.GetDecimal(2)
//                    });
//                }
//            }
//        }
//        return employees;
//    }

//    public async Task<Employee> GetByIdAsync(int id)
//    {
//        using (var connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            var command = new SqlCommand("SELECT * FROM Employees WHERE Id = @Id", connection);
//            command.Parameters.AddWithValue("@Id", id);
//            using (var reader = await command.ExecuteReaderAsync())
//            {
//                if (await reader.ReadAsync())
//                {
//                    return new Employee
//                    {
//                        Id = reader.GetInt32(0),
//                        Name = reader.GetString(1),
//                        Salary = reader.GetDecimal(2)
//                    };
//                }
//            }
//        }
//        return null;
//    }

//    public async Task AddAsync(Employee entity)
//    {
//        using (var connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            var command = new SqlCommand(
//                "INSERT INTO Employees (Name, Salary) VALUES (@Name, @Salary)",
//                connection);
//            command.Parameters.AddWithValue("@Name", entity.Name);
//            command.Parameters.AddWithValue("@Salary", entity.Salary);
//            await command.ExecuteNonQueryAsync();
//        }
//    }

//    public async Task UpdateAsync(Employee entity)
//    {
//        using (var connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            var command = new SqlCommand(
//                "UPDATE Employees SET Name = @Name, Salary = @Salary WHERE Id = @Id",
//                connection);
//            command.Parameters.AddWithValue("@Id", entity.Id);
//            command.Parameters.AddWithValue("@Name", entity.Name);
//            command.Parameters.AddWithValue("@Salary", entity.Salary);
//            await command.ExecuteNonQueryAsync();
//        }
//    }

//    public async Task DeleteAsync(int id)
//    {
//        using (var connection = new SqlConnection(_connectionString))
//        {
//            await connection.OpenAsync();
//            var command = new SqlCommand("DELETE FROM Employees WHERE Id = @Id", connection);
//            command.Parameters.AddWithValue("@Id", id);
//            await command.ExecuteNonQueryAsync();
//        }
//    }
//}

//public class Employee
//{
//    public int Id { get; set; }
//    public string Name { get; set; }
//    public decimal Salary { get; set; }
//}
