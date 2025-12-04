////Задание 1: Отслеживание изменений состояния строки в DataTable через RowState
//using System;
//using System.Data;

//class Program
//{
//    static void Main()
//    {
//        // Создаём DataTable и добавляем столбцы
//        DataTable studentsTable = new DataTable("Students");
//        studentsTable.Columns.Add("ID", typeof(int));
//        studentsTable.Columns.Add("ФИО", typeof(string));
//        studentsTable.Columns.Add("ЭлектроннаяПочта", typeof(string));
//        studentsTable.Columns.Add("Класс", typeof(string));
//        studentsTable.Columns.Add("СредняяОценка", typeof(double));

//        // Заполняем DataTable начальными данными
//        studentsTable.Rows.Add(1, "Иванов Иван Иванович", "ivanov@example.com", "10А", 4.5);
//        studentsTable.Rows.Add(2, "Петров Петр Петрович", "petrov@example.com", "10Б", 4.2);
//        studentsTable.Rows.Add(3, "Сидорова Анна Сергеевна", "sidorova@example.com", "11А", 4.8);

//        // Выводим начальные данные
//        Console.WriteLine("Начальные данные:");
//        PrintDataTable(studentsTable);

//        // 1. Добавляем 3 новых учащихся
//        studentsTable.Rows.Add(4, "Новый Ученик 1", "new1@example.com", "9А", 4.0);
//        studentsTable.Rows.Add(5, "Новый Ученик 2", "new2@example.com", "9Б", 3.8);
//        studentsTable.Rows.Add(6, "Новый Ученик 3", "new3@example.com", "10А", 4.1);

//        // 2. Редактируем 2 текущих учащихся
//        studentsTable.Rows[0]["ЭлектроннаяПочта"] = "ivanov_new@example.com";
//        studentsTable.Rows[1]["СредняяОценка"] = 4.7;

//        // 3. Удаляем 1 учащегося
//        studentsTable.Rows[2].Delete();

//        // Выводим отчёт обо всех строках с указанием RowState и изменённых полей
//        Console.WriteLine("\nОтчёт обо всех строках:");
//        PrintRowStates(studentsTable);

//        // Получаем только изменённые строки
//        DataTable changedRows = studentsTable.GetChanges();
//        if (changedRows != null)
//        {
//            Console.WriteLine("\nОтчёт только по изменённым строкам:");
//            PrintRowStates(changedRows);
//        }
//        else
//        {
//            Console.WriteLine("\nНет изменённых строк.");
//        }
//    }

//    // Метод для вывода DataTable в консоль
//    static void PrintDataTable(DataTable table)
//    {
//        foreach (DataRow row in table.Rows)
//        {
//            if (row.RowState != DataRowState.Deleted)
//            {
//                Console.WriteLine($"ID: {row["ID"]}, ФИО: {row["ФИО"]}, " +
//                    $"ЭлектроннаяПочта: {row["ЭлектроннаяПочта"]}, Класс: {row["Класс"]}, " +
//                    $"СредняяОценка: {row["СредняяОценка"]}");
//            }
//        }
//    }

//    // Метод для вывода отчёта о строках с указанием RowState и изменённых полей
//    static void PrintRowStates(DataTable table)
//    {
//        foreach (DataRow row in table.Rows)
//        {
//            Console.WriteLine($"\nСтрока: {row.RowState}");
//            if (row.RowState != DataRowState.Unchanged && row.RowState != DataRowState.Detached)
//            {
//                Console.WriteLine("Изменённые поля:");
//                if (row.RowState == DataRowState.Added)
//                {
//                    foreach (DataColumn column in table.Columns)
//                    {
//                        Console.WriteLine($"{column.ColumnName}: {row[column]}");
//                    }
//                }
//                else if (row.RowState == DataRowState.Modified)
//                {
//                    DataRowVersion original = row.HasVersion(DataRowVersion.Original) ? DataRowVersion.Original : DataRowVersion.Current;
//                    foreach (DataColumn column in table.Columns)
//                    {
//                        if (!row[column, DataRowVersion.Current].Equals(row[column, original]))
//                        {
//                            Console.WriteLine($"{column.ColumnName}: было {row[column, original]}, стало {row[column, DataRowVersion.Current]}");
//                        }
//                    }
//                }
//                else if (row.RowState == DataRowState.Deleted)
//                {
//                    foreach (DataColumn column in table.Columns)
//                    {
//                        Console.WriteLine($"{column.ColumnName}: {row[column, DataRowVersion.Original]}");
//                    }
//                }
//            }
//        }
//    }
//}


////Задание 2: Работа с DataRowVersion при просмотре текущих и исходных результатов
//using System;
//using System.Data;

//class Program
//{
//    static void Main()
//    {
//        // Создаём DataTable для товаров
//        DataTable productsTable = new DataTable("Products");
//        productsTable.Columns.Add("ID", typeof(int));
//        productsTable.Columns.Add("Название", typeof(string));
//        productsTable.Columns.Add("Цена", typeof(decimal));
//        productsTable.Columns.Add("КоличествоНаСкладе", typeof(int));
//        productsTable.Columns.Add("СтатусДоступности", typeof(string));

//        // Заполняем DataTable начальными данными
//        productsTable.Rows.Add(1, "Ноутбук", 50000.00m, 10, "В наличии");
//        productsTable.Rows.Add(2, "Смартфон", 30000.00m, 20, "В наличии");
//        productsTable.Rows.Add(3, "Наушники", 5000.00m, 30, "В наличии");
//        productsTable.Rows.Add(4, "Клавиатура", 2000.00m, 15, "В наличии");

//        // Принимаем изменения, чтобы начальные строки имели состояние Unchanged
//        productsTable.AcceptChanges();

//        // Выводим начальные данные
//        Console.WriteLine("Начальные данные:");
//        PrintProductsTable(productsTable);

//        // Редактируем несколько товаров
//        productsTable.Rows[0]["Цена"] = 45000.00m; // Уменьшаем цену на ноутбук
//        productsTable.Rows[0]["КоличествоНаСкладе"] = 5; // Уменьшаем количество на складе

//        productsTable.Rows[1]["Цена"] = 35000.00m; // Увеличиваем цену на смартфон
//        productsTable.Rows[1]["КоличествоНаСкладе"] = 25; // Увеличиваем количество на складе

//        // Выводим отчёт по изменённым ценам
//        Console.WriteLine("\nОтчёт по изменённым ценам:");
//        PrintPriceChangesReport(productsTable);

//        // Выводим специальный отчёт только для товаров с изменённой ценой
//        Console.WriteLine("\nСпециальный отчёт (только товары с изменённой ценой):");
//        PrintChangedPriceProductsReport(productsTable);
//    }

//    // Метод для вывода DataTable в консоль
//    static void PrintProductsTable(DataTable table)
//    {
//        foreach (DataRow row in table.Rows)
//        {
//            if (row.RowState != DataRowState.Deleted)
//            {
//                Console.WriteLine(
//                    $"ID: {row["ID"]}, " +
//                    $"Название: {row["Название"]}, " +
//                    $"Цена: {row["Цена"]:C}, " +
//                    $"Количество: {row["КоличествоНаСкладе"]}, " +
//                    $"Статус: {row["СтатусДоступности"]}");
//            }
//        }
//    }

//    // Метод для вывода отчёта по изменённым ценам
//    static void PrintPriceChangesReport(DataTable table)
//    {
//        bool hasChanges = false;
//        foreach (DataRow row in table.Rows)
//        {
//            if (row.RowState == DataRowState.Modified)
//            {
//                decimal originalPrice = (decimal)row["Цена", DataRowVersion.Original];
//                decimal currentPrice = (decimal)row["Цена", DataRowVersion.Current];
//                decimal priceDifference = currentPrice - originalPrice;
//                decimal percentChange = priceDifference / originalPrice * 100;

//                Console.WriteLine(
//                    $"\nТовар: {row["Название"]}, " +
//                    $"Исходная цена: {originalPrice:C}, " +
//                    $"Текущая цена: {currentPrice:C}, " +
//                    $"Разница: {priceDifference:C}, " +
//                    $"Процент изменения: {percentChange:F2}%");
//                hasChanges = true;
//            }
//        }
//        if (!hasChanges)
//            Console.WriteLine("Нет изменённых товаров.");
//    }

//    // Метод для вывода специального отчёта только для товаров с изменённой ценой
//    static void PrintChangedPriceProductsReport(DataTable table)
//    {
//        bool hasPriceChanges = false;
//        foreach (DataRow row in table.Rows)
//        {
//            if (row.RowState == DataRowState.Modified)
//            {
//                try
//                {
//                    decimal originalPrice = (decimal)row["Цена", DataRowVersion.Original];
//                    decimal currentPrice = (decimal)row["Цена", DataRowVersion.Current];

//                    if (originalPrice != currentPrice)
//                    {
//                        Console.WriteLine(
//                            $"\nТовар: {row["Название"]}, " +
//                            $"Исходная цена: {originalPrice:C}, " +
//                            $"Текущая цена: {currentPrice:C}");
//                        hasPriceChanges = true;
//                    }
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"Ошибка при обработке товара {row["Название"]}: {ex.Message}");
//                }
//            }
//        }
//        if (!hasPriceChanges)
//            Console.WriteLine("Нет товаров с изменённой ценой.");
//    }
//}


////Задание 3: Фильтрация и поиск данных в DataSet с использованием DataView
//using System;
//using System.Data;
//using System.Linq;

//class Program
//{
//    static void Main()
//    {
//        // Создаём DataSet и таблицы
//        DataSet companyDataSet = new DataSet("Company");

//        // Таблица "Сотрудники"
//        DataTable employeesTable = new DataTable("Сотрудники");
//        employeesTable.Columns.Add("ID", typeof(int));
//        employeesTable.Columns.Add("ФИО", typeof(string));
//        employeesTable.Columns.Add("Отдел", typeof(string));
//        employeesTable.Columns.Add("Зарплата", typeof(decimal));
//        employeesTable.Columns.Add("ДатаНайма", typeof(DateTime));

//        // Таблица "Проекты"
//        DataTable projectsTable = new DataTable("Проекты");
//        projectsTable.Columns.Add("ID", typeof(int));
//        projectsTable.Columns.Add("Название", typeof(string));
//        projectsTable.Columns.Add("Отдел", typeof(string));
//        projectsTable.Columns.Add("БюджетПроекта", typeof(decimal));
//        projectsTable.Columns.Add("ДатаНачала", typeof(DateTime));

//        // Добавляем таблицы в DataSet
//        companyDataSet.Tables.Add(employeesTable);
//        companyDataSet.Tables.Add(projectsTable);

//        // Заполняем таблицу "Сотрудники"
//        employeesTable.Rows.Add(1, "Иванов Иван Иванович", "IT", 60000, new DateTime(2020, 1, 15));
//        employeesTable.Rows.Add(2, "Петров Петр Петрович", "HR", 45000, new DateTime(2019, 5, 10));
//        employeesTable.Rows.Add(3, "Сидорова Анна Сергеевна", "IT", 70000, new DateTime(2018, 3, 22));
//        employeesTable.Rows.Add(4, "Алексеев Алексей Алексеевич", "Finance", 55000, new DateTime(2021, 7, 5));
//        employeesTable.Rows.Add(5, "Кузнецова Мария Ивановна", "IT", 80000, new DateTime(2017, 11, 18));

//        // Заполняем таблицу "Проекты"
//        projectsTable.Rows.Add(1, "Разработка ПО", "IT", 500000, new DateTime(2022, 1, 1));
//        projectsTable.Rows.Add(2, "Оптимизация бизнес-процессов", "HR", 300000, new DateTime(2021, 10, 15));
//        projectsTable.Rows.Add(3, "Анализ рынка", "Finance", 400000, new DateTime(2023, 2, 20));

//        // Принимаем изменения
//        companyDataSet.AcceptChanges();

//        // Поиск сотрудников по фамилиям (частичный поиск)
//        Console.WriteLine("Поиск сотрудников по фамилии 'ов':");
//        SearchEmployeesByLastName(employeesTable, "ов");

//        // Фильтрация сотрудников по отделу
//        Console.WriteLine("\nСотрудники IT-отдела:");
//        FilterEmployeesByDepartment(employeesTable, "IT");

//        // Фильтрация сотрудников с зарплатой выше определенного значения
//        Console.WriteLine("\nСотрудники с зарплатой выше 60000:");
//        FilterEmployeesBySalary(employeesTable, 60000);

//        // Сортировка сотрудников по дате найма (по возрастанию)
//        Console.WriteLine("\nСотрудники, отсортированные по дате найма (по возрастанию):");
//        SortEmployeesByHireDate(employeesTable, "ASC");

//        // Сортировка сотрудников по дате найма (по убыванию)
//        Console.WriteLine("\nСотрудники, отсортированные по дате найма (по убыванию):");
//        SortEmployeesByHireDate(employeesTable, "DESC");

//        // Комбинированный поиск
//        Console.WriteLine("\nСотрудники IT-отдела с зарплатой более 50000, отсортированные по ФИО:");
//        CombinedSearch(employeesTable, "IT", 50000);

//        // Получение статистики по фильтрованным данным
//        Console.WriteLine("\nСтатистика по сотрудникам IT-отдела:");
//        GetStatisticsByDepartment(employeesTable, "IT");
//    }

//    // Поиск сотрудников по фамилии (частичный поиск)
//    static void SearchEmployeesByLastName(DataTable employeesTable, string lastNamePart)
//    {
//        string filterExpression = $"ФИО LIKE '%{lastNamePart}%'";
//        DataRow[] foundRows = employeesTable.Select(filterExpression);

//        if (foundRows.Length == 0)
//            Console.WriteLine("Сотрудники не найдены.");
//        else
//            foreach (DataRow row in foundRows)
//                Console.WriteLine($"ID: {row["ID"]}, ФИО: {row["ФИО"]}, Отдел: {row["Отдел"]}");
//    }

//    // Фильтрация сотрудников по отделу
//    static void FilterEmployeesByDepartment(DataTable employeesTable, string department)
//    {
//        DataView view = new DataView(employeesTable);
//        view.RowFilter = $"Отдел = '{department}'";

//        if (view.Count == 0)
//            Console.WriteLine("Сотрудники не найдены.");
//        else
//            foreach (DataRowView row in view)
//                Console.WriteLine($"ID: {row["ID"]}, ФИО: {row["ФИО"]}, Отдел: {row["Отдел"]}");
//    }

//    // Фильтрация сотрудников с зарплатой выше определенного значения
//    static void FilterEmployeesBySalary(DataTable employeesTable, decimal minSalary)
//    {
//        DataView view = new DataView(employeesTable);
//        view.RowFilter = $"Зарплата > {minSalary}";

//        if (view.Count == 0)
//            Console.WriteLine("Сотрудники не найдены.");
//        else
//            foreach (DataRowView row in view)
//                Console.WriteLine($"ID: {row["ID"]}, ФИО: {row["ФИО"]}, Зарплата: {row["Зарплата"]}");
//    }

//    // Сортировка сотрудников по дате найма
//    static void SortEmployeesByHireDate(DataTable employeesTable, string sortDirection)
//    {
//        DataView view = new DataView(employeesTable);
//        view.Sort = $"ДатаНайма {sortDirection}";

//        foreach (DataRowView row in view)
//            Console.WriteLine($"ID: {row["ID"]}, ФИО: {row["ФИО"]}, Дата найма: {row["ДатаНайма"]:d}");
//    }

//    // Комбинированный поиск
//    static void CombinedSearch(DataTable employeesTable, string department, decimal minSalary)
//    {
//        DataView view = new DataView(employeesTable);
//        view.RowFilter = $"Отдел = '{department}' AND Зарплата > {minSalary}";
//        view.Sort = "ФИО ASC";

//        if (view.Count == 0)
//            Console.WriteLine("Сотрудники не найдены.");
//        else
//            foreach (DataRowView row in view)
//                Console.WriteLine($"ID: {row["ID"]}, ФИО: {row["ФИО"]}, Отдел: {row["Отдел"]}, Зарплата: {row["Зарплата"]}");
//    }

//    // Получение статистики по фильтрованным данным
//    static void GetStatisticsByDepartment(DataTable employeesTable, string department)
//    {
//        DataView view = new DataView(employeesTable);
//        view.RowFilter = $"Отдел = '{department}'";

//        if (view.Count == 0)
//        {
//            Console.WriteLine("Сотрудники не найдены.");
//            return;
//        }

//        var salaries = view.Cast<DataRowView>()
//                          .Select(row => (decimal)row["Зарплата"])
//                          .ToList();

//        decimal averageSalary = salaries.Average();
//        decimal maxSalary = salaries.Max();
//        decimal minSalary = salaries.Min();

//        Console.WriteLine(
//            $"Количество сотрудников: {view.Count}\n" +
//            $"Средняя зарплата: {averageSalary:C}\n" +
//            $"Максимальная зарплата: {maxSalary:C}\n" +
//            $"Минимальная зарплата: {minSalary:C}");
//    }
//}
