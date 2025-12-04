//using System.Data;
//using System.Diagnostics;
//using System.Reflection.Emit;
//using static System.Net.Mime.MediaTypeNames;

//1
//using System;
//using System.Data;
//using System.Diagnostics;
//using System.Linq;

//namespace DataViewAdvantagesDemo
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            Console.OutputEncoding = System.Text.Encoding.UTF8;
//            Console.WriteLine("=== Демонстрация преимуществ DataView над DataTable ===\n");

//            // 1. Создаём и заполняем DataTable
//            DataTable products = CreateProductsTable();
//            FillData(products);

//            Console.WriteLine($"Исходная таблица содержит {products.Rows.Count} товаров.\n");

//            // 2. Демонстрация: исходная таблица НЕ изменяется при работе с DataView
//            Console.WriteLine("1. Исходная таблица остаётся неизменной при фильтрации и сортировке через DataView\n");

//            DataView viewExpensive = new DataView(products)
//            {
//                RowFilter = "Цена > 500",
//                Sort = "Цена DESC"
//            };

//            Console.WriteLine("DataView: Товары дороже 500 руб, отсортированные по убыванию цены:");
//            foreach (DataRowView drv in viewExpensive)
//            {
//                Console.WriteLine($"  {drv["Название"],-25} | Цена: {drv["Цена"],4} | Категория: {drv["Категория"]}");
//            }
//            Console.WriteLine();

//            Console.WriteLine("Проверяем, что исходная таблица НЕ изменилась (первые 5 строк):");
//            for (int i = 0; i < Math.Min(5, products.Rows.Count); i++)
//            {
//                DataRow row = products.Rows[i];
//                Console.WriteLine($"  {row["Название"],-25} | Цена: {row["Цена"],4} | Кол-во: {row["Количество"],2}");
//            }
//            Console.WriteLine();

//            // 3. Сравнение с DataTable.Select() — он возвращает новый массив, исходная таблица тоже не меняется,
//            //     но каждый раз создаётся новый массив DataRow[]
//            Console.WriteLine("2. Сравнение DataTable.Select() vs DataView (фильтрация)\n");

//            Stopwatch sw = new Stopwatch();

//            // DataTable.Select() — каждый вызов создаёт новый массив
//            sw.Restart();
//            DataRow[] selected1 = products.Select("Цена > 1000");
//            sw.Stop();
//            long selectTime1 = sw.ElapsedTicks;

//            sw.Restart();
//            DataRow[] selected2 = products.Select("Цена > 1000");
//            sw.Stop();
//            long selectTime2 = sw.ElapsedTicks;

//            // DataView — фильтр меняется динамически, без пересоздания структуры
//            DataView fastView = new DataView(products);
//            sw.Restart();
//            fastView.RowFilter = "Цена > 1000";
//            sw.Stop();
//            long viewFilterTime1 = sw.ElapsedTicks;

//            sw.Restart();
//            fastView.RowFilter = "Категория = 'Электроника'";
//            sw.Stop();
//            long viewFilterTime2 = sw.ElapsedTicks;

//            Console.WriteLine("Производительность изменения фильтра (в тиках процессора):");
//            Console.WriteLine($"   DataTable.Select() (1-й вызов): {selectTime1,10}");
//            Console.WriteLine($"   DataTable.Select() (2-й вызов): {selectTime2,10}");
//            Console.WriteLine($"   DataView.RowFilter = ... (1-й): {viewFilterTime1,10}");
//            Console.WriteLine($"   DataView.RowFilter = ... (2-й): {viewFilterTime2,10}");
//            Console.WriteLine("   → DataView значительно быстрее при многократном изменении фильтра!\n");

//            // 4. Несколько представлений одновременно
//            Console.WriteLine("3. Несколько DataView для одной таблицы одновременно\n");

//            DataView viewElectronics = new DataView(products)
//            {
//                RowFilter = "Категория = 'Электроника'",
//                Sort = "Цена DESC"
//            };

//            DataView viewLowStock = new DataView(products)
//            {
//                RowFilter = "Количество < 5",
//                Sort = "Количество ASC, Название"
//            };

//            DataView viewCheapFood = new DataView(products)
//            {
//                RowFilter = "Категория = 'Продукты' AND Цена < 100",
//                Sort = "Название"
//            };

//            Console.WriteLine($"• Электроника (дорогие сначала): {viewElectronics.Count} шт.");
//            Console.WriteLine($"• Мало на складе (<5):           {viewLowStock.Count} шт.");
//            Console.WriteLine($"• Дешёвые продукты (<100 руб):   {viewCheapFood.Count} шт.\n");

//            // 5. Автоматическое обновление DataView при изменении DataTable
//            Console.WriteLine("4. Автоматическое обновление DataView при изменении данных\n");

//            Console.WriteLine("До изменения: товаров с Количество < 5: " + viewLowStock.Count);

//            // Изменяем данные в исходной таблице
//            products.Rows.Find(5)["Количество"] = 2;  // Ноутбук Lenovo
//            products.Rows.Find(12)["Количество"] = 3; // Хлеб

//            Console.WriteLine("После изменения двух строк в DataTable:");
//            Console.WriteLine("   → Товаров с Количество < 5 стало: " + viewLowStock.Count);
//            Console.WriteLine("   DataView обновился автоматически!\n");

//            // 6. Итоговый отчёт
//            Console.WriteLine("=== ИТОГОВЫЙ ОТЧЁТ О ПРЕИМУЩЕСТВАХ DataView ===\n");
//            Console.WriteLine("✓ Исходная DataTable остаётся неизменной при любой фильтрации/сортировке");
//            Console.WriteLine("✓ Многократное изменение фильтра и сортировки — ОЧЕНЬ быстро (миллионы операций в секунду)");
//            Console.WriteLine("✓ Поддержка нескольких независимых представлений одной таблицы одновременно");
//            Console.WriteLine("✓ Автоматическое отражение изменений в DataTable → в DataView (и наоборот при RowStateFilter)");
//            Console.WriteLine("✓ Значительно выше производительность при динамических фильтрах по сравнению с повторным вызовом DataTable.Select()");
//            Console.WriteLine("✓ Удобно для привязки к элементам UI (WinForms, WPF через BindingSource)");
//            Console.WriteLine("\nDataView — идеальный выбор, когда нужна динамическая, многопользовательская фильтрация и сортировка без мутации данных.");

//            Console.WriteLine("\nНажмите любую клавишу для выхода...");
//            Console.ReadKey();
//        }

//        static DataTable CreateProductsTable()
//        {
//            DataTable dt = new DataTable("Products");

//            dt.Columns.Add("ProductID", typeof(int));
//            dt.Columns.Add("Название", typeof(string));
//            dt.Columns.Add("Цена", typeof(decimal));
//            dt.Columns.Add("Количество", typeof(int));
//            dt.Columns.Add("Категория", typeof(string));

//            dt.PrimaryKey = new DataColumn[] { dt.Columns["ProductID"] };

//            return dt;
//        }

//        static void FillData(DataTable dt)
//        {
//            var data = new (int Id, string Name, decimal Price, int Qty, string Cat)[]
//            {
//                (1, "iPhone 15 Pro", 129900, 8, "Электроника"),
//                (2, "Samsung Galaxy S24", 109900, 12, "Электроника"),
//                (3, "Ноутбук Lenovo", 89900, 5, "Электроника"),
//                (4, "Наушники AirPods Pro", 24900, 25, "Электроника"),
//                (5, "Кофемашина Delonghi", 45900, 7, "Бытовая техника"),
//                (6, "Микроволновка LG", 12900, 15, "Бытовая техника"),
//                (7, "Холодильник Bosch", 89000, 3, "Бытовая техника"),
//                (8, "Пылесос Dyson", 54900, 6, "Бытовая техника"),
//                (9, "Книга 'Чистый код'", 2300, 40, "Книги"),
//                (10, "Книга 'Алгоритмы'", 3200, 18, "Книги"),
//                (11, "Молоко 1л", 85, 150, "Продукты"),
//                (12, "Хлеб", 45, 200, "Продукты"),
//                (13, "Яблоки 1кг", 129, 80, "Продукты"),
//                (14, "Куриное филе 1кг", 420, 35, "Продукты"),
//                (15, "Смартфон Xiaomi 14", 59900, 20, "Электроника"),
//                (16, "Футболка", 890, 120, "Одежда"),
//                (17, "Джинсы", 3490, 45, "Одежда"),
//                (18, "Кроссовки Nike", 12990, 30, "Одежда"),
//                (19, "Рюкзак", 2790, 60, "Аксессуары"),
//                (20, "Часы Casio", 4990, 50, "Аксессуары")
//            };

//            foreach (var item in data)
//            {
//                dt.Rows.Add(item.Id, item.Name, item.Price, item.Qty, item.Cat);
//            }
//        }
//    }
//}


//2
//using System;
//using System.Data;
//using System.Diagnostics;
//using System.Linq;

//namespace DataTableFindDemo
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            Console.OutputEncoding = System.Text.Encoding.UTF8;
//            Console.WriteLine("=== Демонстрация метода Find() для поиска по первичному ключу ===\n");

//            // 1. Создаём DataTable "Сотрудники"
//            DataTable employees = CreateEmployeesTable();

//            // 2. Заполняем 50 сотрудниками
//            FillData(employees);

//            Console.WriteLine($"Таблица содержит {employees.Rows.Count} сотрудников.\n");

//            // 3. Поиск одного сотрудника по ID с Find()
//            Console.WriteLine("1. Поиск существующего сотрудника по ID с Find()\n");
//            int existingId = 10;
//            DataRow foundRow = employees.Rows.Find(existingId);
//            if (foundRow != null)
//            {
//                PrintEmployeeInfo(foundRow);
//            }
//            else
//            {
//                Console.WriteLine($"Сотрудник с ID {existingId} не найден.");
//            }
//            Console.WriteLine();

//            // 4. Поиск несуществующего ID
//            Console.WriteLine("2. Поиск несуществующего ID и обработка null\n");
//            int nonExistingId = 999;
//            DataRow notFoundRow = employees.Rows.Find(nonExistingId);
//            if (notFoundRow == null)
//            {
//                Console.WriteLine($"Сотрудник с ID {nonExistingId} не найден (возвращено null).");
//            }
//            Console.WriteLine();

//            // 5. Сравнение производительности Find() vs Select()
//            Console.WriteLine("3. Сравнение производительности Find() vs Select() для одиночного поиска\n");
//            Stopwatch sw = new Stopwatch();
//            int testId = 25;
//            const int iterations = 100000; // Много итераций для точного замера

//            // Замер Find()
//            sw.Restart();
//            for (int i = 0; i < iterations; i++)
//            {
//                employees.Rows.Find(testId);
//            }
//            sw.Stop();
//            long findTime = sw.ElapsedTicks;

//            // Замер Select()
//            sw.Restart();
//            for (int i = 0; i < iterations; i++)
//            {
//                employees.Select($"EmployeeID = {testId}");
//            }
//            sw.Stop();
//            long selectTime = sw.ElapsedTicks;

//            Console.WriteLine($"Производительность ({iterations} итераций, в тиках процессора):");
//            Console.WriteLine($"   DataRow.Find():    {findTime,12}");
//            Console.WriteLine($"   DataTable.Select(): {selectTime,12}");
//            Console.WriteLine($"   → Find() быстрее в ~{Math.Round((double)selectTime / findTime, 1)} раз благодаря индексу PK!\n");

//            // 6. Поиск с DataView и BinarySearch/Find
//            Console.WriteLine("4. Поиск с использованием DataView и Find() (возвращает индекс в view)\n");
//            DataView view = new DataView(employees)
//            {
//                Sort = "EmployeeID ASC" // Необходимо для Find() в DataView
//            };
//            int viewIndex = view.Find(testId);
//            if (viewIndex >= 0)
//            {
//                DataRowView drv = view[viewIndex];
//                Console.WriteLine($"Найден в DataView по индексу {viewIndex}:");
//                PrintEmployeeInfo(drv.Row);
//            }
//            else
//            {
//                Console.WriteLine($"ID {testId} не найден в DataView.");
//            }
//            Console.WriteLine();

//            // 7. Метод для поиска в диапазоне ID (от X до Y)
//            Console.WriteLine("5. Поиск сотрудников в диапазоне ID (от 10 до 20)\n");
//            DataRow[] rangeRows = FindEmployeesInRange(employees, 10, 20);
//            Console.WriteLine($"Найдено {rangeRows.Length} сотрудников:");
//            foreach (DataRow row in rangeRows)
//            {
//                PrintEmployeeInfo(row, shortFormat: true);
//            }
//            Console.WriteLine();

//            // 8. Обработка исключений
//            Console.WriteLine("6. Обработка исключений при поиске\n");
//            try
//            {
//                // Неверный тип (string вместо int)
//                employees.Rows.Find("invalid");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Исключение при неверном типе ключа: {ex.Message}");
//            }

//            try
//            {
//                // Null ключ
//                employees.Rows.Find(null);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Исключение при null ключе: {ex.Message}");
//            }
//            Console.WriteLine();

//            // 9. Итоговый отчёт
//            Console.WriteLine("=== ИТОГОВЫЙ ОТЧЁТ О ПРЕИМУЩЕСТВАХ Find() ===\n");
//            Console.WriteLine("✓ Find() использует индекс первичного ключа → очень быстро для одиночного поиска");
//            Console.WriteLine("✓ Возвращает DataRow напрямую (или null при отсутствии)");
//            Console.WriteLine("✓ Значительно быстрее Select() для точного поиска по PK (особенно в больших таблицах)");
//            Console.WriteLine("✓ В DataView: Find() работает как BinarySearch по отсортированной колонке");
//            Console.WriteLine("✓ Обработка ошибок: проверка на null, catch для неверного типа/нулевого ключа");
//            Console.WriteLine("✓ Для диапазонов: используйте Select() с фильтром, а не Find()");

//            Console.WriteLine("\nНажмите любую клавишу для выхода...");
//            Console.ReadKey();
//        }

//        static DataTable CreateEmployeesTable()
//        {
//            DataTable dt = new DataTable("Employees");

//            dt.Columns.Add("EmployeeID", typeof(int));
//            dt.Columns.Add("ФИО", typeof(string));
//            dt.Columns.Add("Email", typeof(string));
//            dt.Columns.Add("Отдел", typeof(string));
//            dt.Columns.Add("Зарплата", typeof(decimal));

//            // Устанавливаем первичный ключ
//            dt.PrimaryKey = new DataColumn[] { dt.Columns["EmployeeID"] };

//            return dt;
//        }

//        static void FillData(DataTable dt)
//        {
//            string[] departments = { "IT", "HR", "Sales", "Marketing", "Finance" };
//            Random rnd = new Random();

//            for (int id = 1; id <= 50; id++)
//            {
//                string fio = $"Сотрудник {id} Иванович";
//                string email = $"employee{id}@company.com";
//                string dept = departments[rnd.Next(departments.Length)];
//                decimal salary = rnd.Next(50000, 200000) + rnd.Next(0, 99) / 100m;

//                dt.Rows.Add(id, fio, email, dept, salary);
//            }
//        }

//        static void PrintEmployeeInfo(DataRow row, bool shortFormat = false)
//        {
//            if (shortFormat)
//            {
//                Console.WriteLine($"  ID: {row["EmployeeID"],2} | ФИО: {row["ФИО"],-20} | Зарплата: {row["Зарплата"],8:C}");
//            }
//            else
//            {
//                Console.WriteLine($"ID: {row["EmployeeID"]}");
//                Console.WriteLine($"ФИО: {row["ФИО"]}");
//                Console.WriteLine($"Email: {row["Email"]}");
//                Console.WriteLine($"Отдел: {row["Отдел"]}");
//                Console.WriteLine($"Зарплата: {row["Зарплата"]:C}");
//            }
//        }

//        static DataRow[] FindEmployeesInRange(DataTable dt, int fromId, int toId)
//        {
//            if (fromId > toId) throw new ArgumentException("fromId должен быть меньше toId");

//            return dt.Select($"EmployeeID >= {fromId} AND EmployeeID <= {toId}", "EmployeeID ASC");
//        }
//    }
//}


//3
//using System;
//using System.Data;
//using System.Diagnostics;
//using System.Linq;

//class OrderSearchSystem
//{
//    static void Main()
//    {
//        // Создаём и заполняем DataTable "Заказы"
//        DataTable ordersTable = CreateOrdersTable();
//        FillTestData(ordersTable);

//        Console.WriteLine("=== СИСТЕМА ФИЛЬТРАЦИИ ЗАКАЗОВ ===\n");

//        // 1. Фильтрация по сумме заказа
//        Console.WriteLine("1. ФИЛЬТРАЦИЯ ПО СУММЕ ЗАКАЗА");
//        Console.WriteLine("═════════════════════════════════════════════════");
//        FilterByAmount(ordersTable, 5000m);

//        // 2. Фильтрация по статусу заказа
//        Console.WriteLine("\n2. ФИЛЬТРАЦИЯ ПО СТАТУСУ ЗАКАЗА");
//        Console.WriteLine("═════════════════════════════════════════════════");
//        FilterByStatus(ordersTable, "доставка");

//        // 3. Фильтрация по дате заказа
//        Console.WriteLine("\n3. ФИЛЬТРАЦИЯ ПО ДАТЕ ЗАКАЗА");
//        Console.WriteLine("═════════════════════════════════════════════════");
//        FilterByDate(ordersTable, new DateTime(2023, 1, 1), new DateTime(2023, 12, 31));

//        // 4. Комбинированные фильтры
//        Console.WriteLine("\n4. КОМБИНИРОВАННЫЕ ФИЛЬТРЫ");
//        Console.WriteLine("═════════════════════════════════════════════════");
//        CombinedFilters(ordersTable, "доставка", 3000m);

//        // 5. Фильтрация по клиенту
//        Console.WriteLine("\n5. ФИЛЬТРАЦИЯ ПО КЛИЕНТУ");
//        Console.WriteLine("═════════════════════════════════════════════════");
//        FilterByCustomer(ordersTable, 1003);

//        // 6. Фильтрация с использованием LIKE
//        Console.WriteLine("\n6. ФИЛЬТРАЦИЯ С ИСПОЛЬЗОВАНИЕМ LIKE");
//        Console.WriteLine("═════════════════════════════════════════════════");
//        FilterByAddress(ordersTable, "ул. Ленина");

//        // 7. Сортировка результатов
//        Console.WriteLine("\n7. СОРТИРОВКА РЕЗУЛЬТАТОВ");
//        Console.WriteLine("═════════════════════════════════════════════════");
//        SortResults(ordersTable, "Amount DESC");

//        // 8. Сравнение с LINQ
//        Console.WriteLine("\n8. СРАВНЕНИЕ С LINQ");
//        Console.WriteLine("═════════════════════════════════════════════════");
//        CompareWithLINQ(ordersTable);

//        // 9. Метод построения фильтра
//        Console.WriteLine("\n9. МЕТОД ПОСТРОЕНИЯ ФИЛЬТРА");
//        Console.WriteLine("═════════════════════════════════════════════════");
//        BuildFilterDemo(ordersTable);

//        // 10. Сравнение производительности
//        Console.WriteLine("\n10. СРАВНЕНИЕ ПРОИЗВОДИТЕЛЬНОСТИ");
//        Console.WriteLine("═════════════════════════════════════════════════");
//        ComparePerformance(ordersTable);
//    }

//    // Создание DataTable "Заказы"
//    static DataTable CreateOrdersTable()
//    {
//        DataTable table = new DataTable("Заказы");

//        table.Columns.Add("OrderID", typeof(int));
//        table.Columns.Add("CustomerID", typeof(int));
//        table.Columns.Add("OrderDate", typeof(DateTime));
//        table.Columns.Add("Amount", typeof(decimal));
//        table.Columns.Add("Status", typeof(string));
//        table.Columns.Add("ShippingAddress", typeof(string));

//        table.PrimaryKey = new DataColumn[] { table.Columns["OrderID"] };

//        return table;
//    }

//    // Заполнение таблицы тестовыми данными
//    static void FillTestData(DataTable table)
//    {
//        Random random = new Random();
//        string[] statuses = { "ожидание", "обработка", "отгрузка", "доставка" };
//        string[] addresses = { "ул. Ленина, 1", "ул. Пушкина, 5", "пр. Мира, 10", "ул. Гагарина, 15", "ул. Советская, 20" };

//        for (int i = 1; i <= 100; i++)
//        {
//            table.Rows.Add(
//                i,
//                1001 + random.Next(0, 10),
//                DateTime.Now.AddDays(-random.Next(0, 365)),
//                1000 + random.Next(0, 10000),
//                statuses[random.Next(0, statuses.Length)],
//                addresses[random.Next(0, addresses.Length)]
//            );
//        }
//    }

//    // 1. Фильтрация по сумме заказа
//    static void FilterByAmount(DataTable table, decimal minAmount)
//    {
//        string filter = $"Amount > {minAmount}";
//        DataRow[] rows = table.Select(filter);
//        Console.WriteLine($"Найдено заказов с суммой больше {minAmount}: {rows.Length}");
//        PrintRows(rows, 5);
//    }

//    // 2. Фильтрация по статусу заказа
//    static void FilterByStatus(DataTable table, string status)
//    {
//        string filter = $"Status = '{status}'";
//        DataRow[] rows = table.Select(filter);
//        Console.WriteLine($"Найдено заказов со статусом '{status}': {rows.Length}");
//        PrintRows(rows, 5);
//    }

//    // 3. Фильтрация по дате заказа
//    static void FilterByDate(DataTable table, DateTime startDate, DateTime endDate)
//    {
//        string filter = $"OrderDate >= #{startDate:yyyy-MM-dd}# AND OrderDate <= #{endDate:yyyy-MM-dd}#";
//        DataRow[] rows = table.Select(filter);
//        Console.WriteLine($"Найдено заказов с {startDate:d} по {endDate:d}: {rows.Length}");
//        PrintRows(rows, 5);
//    }

//    // 4. Комбинированные фильтры
//    static void CombinedFilters(DataTable table, string status, decimal minAmount)
//    {
//        string filter = $"Status = '{status}' AND Amount > {minAmount}";
//        DataRow[] rows = table.Select(filter);
//        Console.WriteLine($"Найдено заказов со статусом '{status}' и суммой больше {minAmount}: {rows.Length}");
//        PrintRows(rows, 5);
//    }

//    // 5. Фильтрация по клиенту
//    static void FilterByCustomer(DataTable table, int customerID)
//    {
//        string filter = $"CustomerID = {customerID}";
//        DataRow[] rows = table.Select(filter);
//        Console.WriteLine($"Найдено заказов для клиента {customerID}: {rows.Length}");
//        PrintRows(rows, 5);
//    }

//    // 6. Фильтрация с использованием LIKE
//    static void FilterByAddress(DataTable table, string addressPattern)
//    {
//        string filter = $"ShippingAddress LIKE '%{addressPattern}%'";
//        DataRow[] rows = table.Select(filter);
//        Console.WriteLine($"Найдено заказов с адресом, содержащим '{addressPattern}': {rows.Length}");
//        PrintRows(rows, 5);
//    }

//    // 7. Сортировка результатов
//    static void SortResults(DataTable table, string sortExpression)
//    {
//        DataRow[] rows = table.Select("", sortExpression);
//        Console.WriteLine($"Отсортировано по '{sortExpression}':");
//        PrintRows(rows, 5);
//    }

//    // 8. Сравнение с LINQ
//    static void CompareWithLINQ(DataTable table)
//    {
//        // Используем Select()
//        var stopwatch = Stopwatch.StartNew();
//        DataRow[] selectRows = table.Select("Status = 'доставка'");
//        stopwatch.Stop();
//        Console.WriteLine($"Select(): Найдено {selectRows.Length} записей за {stopwatch.ElapsedTicks} тиков");

//        // Используем LINQ
//        stopwatch.Restart();
//        var linqRows = table.AsEnumerable().Where(row => row.Field<string>("Status") == "доставка").ToArray();
//        stopwatch.Stop();
//        Console.WriteLine($"LINQ: Найдено {linqRows.Length} записей за {stopwatch.ElapsedTicks} тиков");
//    }

//    // 9. Метод построения фильтра
//    static void BuildFilterDemo(DataTable table)
//    {
//        string filter = BuildFilter(
//            customerID: 1003,
//            minAmount: 2000m,
//            status: "доставка",
//            addressPattern: "ул. Ленина"
//        );

//        Console.WriteLine($"Сгенерированный фильтр: {filter}");

//        DataRow[] rows = table.Select(filter);
//        Console.WriteLine($"Найдено записей: {rows.Length}");
//        PrintRows(rows, 5);
//    }

//    static string BuildFilter(int? customerID = null, decimal? minAmount = null,
//                             string status = null, string addressPattern = null)
//    {
//        var conditions = new System.Collections.Generic.List<string>();

//        if (customerID.HasValue)
//            conditions.Add($"CustomerID = {customerID.Value}");

//        if (minAmount.HasValue)
//            conditions.Add($"Amount > {minAmount.Value}");

//        if (!string.IsNullOrEmpty(status))
//            conditions.Add($"Status = '{status}'");

//        if (!string.IsNullOrEmpty(addressPattern))
//            conditions.Add($"ShippingAddress LIKE '%{addressPattern}%'");

//        return string.Join(" AND ", conditions);
//    }

//    // 10. Сравнение производительности
//    static void ComparePerformance(DataTable table)
//    {
//        int iterations = 10000;
//        Random random = new Random();

//        // Тест 1: Select() по статусу
//        var stopwatch = Stopwatch.StartNew();
//        for (int i = 0; i < iterations; i++)
//        {
//            string status = i % 4 == 0 ? "ожидание" : (i % 4 == 1 ? "обработка" : (i % 4 == 2 ? "отгрузка" : "доставка"));
//            var rows = table.Select($"Status = '{status}'");
//        }
//        stopwatch.Stop();
//        Console.WriteLine($"Select() по статусу: {stopwatch.ElapsedMilliseconds} мс за {iterations} итераций");

//        // Тест 2: LINQ по статусу
//        stopwatch.Restart();
//        for (int i = 0; i < iterations; i++)
//        {
//            string status = i % 4 == 0 ? "ожидание" : (i % 4 == 1 ? "обработка" : (i % 4 == 2 ? "отгрузка" : "доставка"));
//            var rows = table.AsEnumerable().Where(row => row.Field<string>("Status") == status).ToArray();
//        }
//        stopwatch.Stop();
//        Console.WriteLine($"LINQ по статусу: {stopwatch.ElapsedMilliseconds} мс за {iterations} итераций");
//    }

//    // Вспомогательный метод для вывода строк
//    static void PrintRows(DataRow[] rows, int maxRows)
//    {
//        for (int i = 0; i < Math.Min(maxRows, rows.Length); i++)
//        {
//            DataRow row = rows[i];
//            Console.WriteLine(
//                $"  Заказ {row["OrderID"]}: Клиент {row["CustomerID"]}, " +
//                $"Сумма {row["Amount"]:C}, Статус {row["Status"]}, " +
//                $"Адрес {row["ShippingAddress"]}"
//            );
//        }

//        if (rows.Length > maxRows)
//            Console.WriteLine($"  ... и ещё {rows.Length - maxRows} записей");
//    }
//}


//4
//using System;
//using System.Data;
//using System.Globalization;

//class StudentDataViewDemo
//{
//    static void Main()
//    {
//        // Устанавливаем культуру для корректного разбора чисел с плавающей запятой
//        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

//        // Создаём и заполняем DataTable "Студенты"
//        DataTable studentsTable = CreateStudentsTable();
//        FillTestData(studentsTable);

//        Console.WriteLine("=== ДЕМОНСТРАЦИЯ РАЗЛИЧНЫХ СПОСОБОВ СОЗДАНИЯ DataView ===\n");

//        // Способ 1: новый DataView(таблица)
//        Console.WriteLine("1. СПОСОБ: DataView(table)");
//        Console.WriteLine("============================================================");
//        DemoDataViewMethod1(studentsTable);

//        // Способ 2: new DataView(table, "GPA > 3.5", "Name ASC", DataViewRowState.CurrentRows)
//        Console.WriteLine("\n2. СПОСОБ: DataView(table, filter, sort, DataViewRowState)");
//        Console.WriteLine("============================================================");
//        DemoDataViewMethod2(studentsTable);

//        // Способ 3: Через таблицу.DefaultView
//        Console.WriteLine("\n3. СПОСОБ: table.DefaultView");
//        Console.WriteLine("============================================================");
//        DemoDataViewMethod3(studentsTable);

//        // Способ 4: Через конструктор с фильтром в объекте DataView после создания
//        Console.WriteLine("\n4. СПОСОБ: DataView с фильтром после создания");
//        Console.WriteLine("============================================================");
//        DemoDataViewMethod4(studentsTable);
//    }

//    static DataTable CreateStudentsTable()
//    {
//        DataTable table = new DataTable("Студенты");

//        table.Columns.Add("StudentID", typeof(int));
//        table.Columns.Add("Имя", typeof(string));
//        table.Columns.Add("Средний балл", typeof(double));
//        table.Columns.Add("Специальность", typeof(string));
//        table.Columns.Add("Год поступления", typeof(int));

//        table.PrimaryKey = new DataColumn[] { table.Columns["StudentID"] };

//        return table;
//    }

//    static void FillTestData(DataTable table)
//    {
//        Random random = new Random();
//        string[] names = { "Иван", "Мария", "Петр", "Анна", "Дмитрий", "Светлана", "Алексей", "Елена", "Николай", "Юлия" };
//        string[] specialties = { "Информатика", "Математика", "Физика", "Химия", "Биология" };

//        for (int i = 1; i <= 100; i++)
//        {
//            table.Rows.Add(
//                i,
//                $"{names[random.Next(names.Length)]} {i}",
//                Math.Round(2.0 + random.NextDouble() * 3, 2),
//                specialties[random.Next(specialties.Length)],
//                2018 + random.Next(0, 5)
//            );
//        }
//    }

//    static void DemoDataViewMethod1(DataTable table)
//    {
//        DataView view = new DataView(table);

//        Console.WriteLine($"Количество строк: {view.Count}");
//        Console.WriteLine("Первые 5 строк:");
//        PrintDataView(view, 5);
//        Console.WriteLine($"Текущий фильтр: {(string.IsNullOrEmpty(view.RowFilter) ? "нет" : view.RowFilter)}");
//        Console.WriteLine($"Текущая сортировка: {(string.IsNullOrEmpty(view.Sort) ? "нет" : view.Sort)}");
//    }

//    static void DemoDataViewMethod2(DataTable table)
//    {
//        DataView view = new DataView(table, "[Средний балл] > 3.5", "[Имя] ASC", DataViewRowState.CurrentRows);

//        Console.WriteLine($"Количество строк: {view.Count}");
//        Console.WriteLine("Первые 5 строк:");
//        PrintDataView(view, 5);
//        Console.WriteLine($"Текущий фильтр: {view.RowFilter}");
//        Console.WriteLine($"Текущая сортировка: {view.Sort}");
//    }

//    static void DemoDataViewMethod3(DataTable table)
//    {
//        DataView view = table.DefaultView;

//        Console.WriteLine($"Количество строк: {view.Count}");
//        Console.WriteLine("Первые 5 строк:");
//        PrintDataView(view, 5);
//        Console.WriteLine($"Текущий фильтр: {(string.IsNullOrEmpty(view.RowFilter) ? "нет" : view.RowFilter)}");
//        Console.WriteLine($"Текущая сортировка: {(string.IsNullOrEmpty(view.Sort) ? "нет" : view.Sort)}");

//        // Пример изменения фильтра
//        view.RowFilter = "[Средний балл] > 4.0";
//        Console.WriteLine("\nПосле изменения фильтра:");
//        Console.WriteLine($"Количество строк: {view.Count}");
//        Console.WriteLine("Первые 5 строк:");
//        PrintDataView(view, 5);
//    }

//    static void DemoDataViewMethod4(DataTable table)
//    {
//        DataView view = new DataView(table);

//        Console.WriteLine("До применения фильтра:");
//        Console.WriteLine($"Количество строк: {view.Count}");
//        Console.WriteLine("Первые 5 строк:");
//        PrintDataView(view, 5);

//        // Применяем фильтр и сортировку
//        view.RowFilter = "[Специальность] = 'Информатика'";
//        view.Sort = "[Средний балл] DESC";

//        Console.WriteLine("\nПосле применения фильтра и сортировки:");
//        Console.WriteLine($"Количество строк: {view.Count}");
//        Console.WriteLine("Первые 5 строк:");
//        PrintDataView(view, 5);
//        Console.WriteLine($"Текущий фильтр: {view.RowFilter}");
//        Console.WriteLine($"Текущая сортировка: {view.Sort}");
//    }

//    static void PrintDataView(DataView view, int maxRows)
//    {
//        for (int i = 0; i < Math.Min(maxRows, view.Count); i++)
//        {
//            DataRowView rowView = view[i];
//            Console.WriteLine(
//                $"Студент {rowView["StudentID"]}: {rowView["Имя"]}, " +
//                $"Средний балл: {rowView["Средний балл"]}, " +
//                $"Специальность: {rowView["Специальность"]}, " +
//                $"Год поступления: {rowView["Год поступления"]}"
//            );
//        }

//        if (view.Count > maxRows)
//            Console.WriteLine($"... и ещё {view.Count - maxRows} записей");
//    }
//}


//5
//using System;
//using System.Data;
//using System.Globalization;

//class SalesDataViewDemo
//{
//    static void Main()
//    {
//        // Устанавливаем культуру для корректного разбора чисел с плавающей запятой
//        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

//        // Создаём и заполняем DataTable "Продажи"
//        DataTable salesTable = CreateSalesTable();
//        FillTestData(salesTable);

//        Console.WriteLine("=== ДЕМОНСТРАЦИЯ ФИЛЬТРАЦИИ И СОРТИРОВКИ В DataView ===\n");

//        // Создаём DataView
//        DataView salesView = new DataView(salesTable);

//        // Демонстрация фильтрации и сортировки
//        DemoFilterAndSort(salesView);

//        // Динамическое изменение сортировки
//        DemoDynamicSortChange(salesView);

//        // Сброс фильтров и сортировки
//        DemoResetFiltersAndSort(salesView);
//    }

//    static DataTable CreateSalesTable()
//    {
//        DataTable table = new DataTable("Продажи");

//        table.Columns.Add("SalesID", typeof(int));
//        table.Columns.Add("ProductName", typeof(string));
//        table.Columns.Add("SalesDate", typeof(DateTime));
//        table.Columns.Add("Quantity", typeof(int));
//        table.Columns.Add("Price", typeof(decimal));
//        table.Columns.Add("Salesperson", typeof(string));
//        table.Columns.Add("Region", typeof(string));

//        table.PrimaryKey = new DataColumn[] { table.Columns["SalesID"] };

//        return table;
//    }

//    static void FillTestData(DataTable table)
//    {
//        Random random = new Random();
//        string[] productNames = { "Ноутбук", "Смартфон", "Планшет", "Монитор", "Клавиатура" };
//        string[] salespersons = { "Иванов", "Петров", "Сидоров", "Кузнецов", "Васильев" };
//        string[] regions = { "Москва", "Санкт-Петербург", "Новосибирск", "Екатеринбург", "Казань" };

//        for (int i = 1; i <= 200; i++)
//        {
//            DateTime salesDate = DateTime.Now.AddDays(-random.Next(0, 365));
//            table.Rows.Add(
//                i,
//                productNames[random.Next(productNames.Length)],
//                salesDate,
//                1 + random.Next(10),
//                500 + random.Next(20000),
//                salespersons[random.Next(salespersons.Length)],
//                regions[random.Next(regions.Length)]
//            );
//        }
//    }

//    static void DemoFilterAndSort(DataView view)
//    {
//        // Фильтрация по регионам
//        Console.WriteLine("Фильтрация по региону 'Москва':");
//        view.RowFilter = "[Region] = 'Москва'";
//        Console.WriteLine($"Количество записей: {view.Count}");
//        PrintDataView(view, 5);

//        // Фильтрация по датам
//        Console.WriteLine("\nФильтрация по датам (последний месяц):");
//        DateTime oneMonthAgo = DateTime.Now.AddMonths(-1);
//        view.RowFilter = "[SalesDate] >= #" + oneMonthAgo.ToString("yyyy-MM-dd") + "#";
//        Console.WriteLine($"Количество записей: {view.Count}");
//        PrintDataView(view, 5);

//        // Фильтрация по минимальной сумме продажи
//        Console.WriteLine("\nФильтрация по минимальной сумме продажи (сумма > 5000):");
//        view.RowFilter = "[Quantity] * [Price] > 5000";
//        Console.WriteLine($"Количество записей: {view.Count}");
//        PrintDataView(view, 5);

//        // Комбинированные фильтры
//        Console.WriteLine("\nКомбинированные фильтры (регион 'Санкт-Петербург' и сумма > 10000):");
//        view.RowFilter = "[Region] = 'Санкт-Петербург' AND [Quantity] * [Price] > 10000";
//        Console.WriteLine($"Количество записей: {view.Count}");
//        PrintDataView(view, 5);

//        // Сортировка по дате (возрастание)
//        Console.WriteLine("\nСортировка по дате (возрастание):");
//        view.Sort = "[SalesDate] ASC";
//        PrintDataView(view, 5);

//        // Сортировка по дате (убывание)
//        Console.WriteLine("\nСортировка по дате (убывание):");
//        view.Sort = "[SalesDate] DESC";
//        PrintDataView(view, 5);

//        // Сортировка по продавцу
//        Console.WriteLine("\nСортировка по продавцу:");
//        view.Sort = "[Salesperson] ASC";
//        PrintDataView(view, 5);

//        // Многоуровневая сортировка (Region ASC, Date DESC)
//        Console.WriteLine("\nМногоуровневая сортировка (Region ASC, Date DESC):");
//        view.Sort = "[Region] ASC, [SalesDate] DESC";
//        PrintDataView(view, 5);
//    }

//    static void DemoDynamicSortChange(DataView view)
//    {
//        Console.WriteLine("\n=== ДИНАМИЧЕСКОЕ ИЗМЕНЕНИЕ СОРТИРОВКИ ===");

//        Console.WriteLine("\nВыберите вариант сортировки:");
//        Console.WriteLine("1. По дате (возрастание)");
//        Console.WriteLine("2. По дате (убывание)");
//        Console.WriteLine("3. По продавцу");
//        Console.WriteLine("4. По региону и дате");

//        Console.Write("Ваш выбор: ");
//        string choice = Console.ReadLine();

//        switch (choice)
//        {
//            case "1":
//                view.Sort = "[SalesDate] ASC";
//                break;
//            case "2":
//                view.Sort = "[SalesDate] DESC";
//                break;
//            case "3":
//                view.Sort = "[Salesperson] ASC";
//                break;
//            case "4":
//                view.Sort = "[Region] ASC, [SalesDate] DESC";
//                break;
//            default:
//                Console.WriteLine("Некорректный выбор.");
//                return;
//        }

//        Console.WriteLine("\nРезультат сортировки:");
//        PrintDataView(view, 5);
//    }

//    static void DemoResetFiltersAndSort(DataView view)
//    {
//        Console.WriteLine("\n=== СБРОС ФИЛЬТРОВ И СОРТИРОВКИ ===");

//        view.RowFilter = "";
//        view.Sort = "";

//        Console.WriteLine("Фильтры и сортировка сброшены.");
//        Console.WriteLine($"Количество записей: {view.Count}");
//        PrintDataView(view, 5);
//    }

//    static void PrintDataView(DataView view, int maxRows)
//    {
//        for (int i = 0; i < Math.Min(maxRows, view.Count); i++)
//        {
//            DataRowView rowView = view[i];
//            Console.WriteLine(
//                $"Продажа {rowView["SalesID"]}: {rowView["ProductName"]}, " +
//                $"Дата: {((DateTime)rowView["SalesDate"]).ToShortDateString()}, " +
//                $"Количество: {rowView["Quantity"]}, " +
//                $"Цена: {rowView["Price"]:C}, " +
//                $"Продавец: {rowView["Salesperson"]}, " +
//                $"Регион: {rowView["Region"]}"
//            );
//        }

//        if (view.Count > maxRows)
//            Console.WriteLine($"... и ещё {view.Count - maxRows} записей");
//    }
//}


//6
//using System;
//using System.Data;
//using System.Linq;

//class InvoiceDataViewRowStateDemo
//{
//    static void Main()
//    {
//        // Создаём и заполняем DataTable "Счета"
//        DataTable invoicesTable = CreateInvoicesTable();
//        FillTestData(invoicesTable);

//        Console.WriteLine("=== ДЕМОНСТРАЦИЯ DataViewRowState ===\n");

//        // Выводим исходные данные
//        Console.WriteLine("Исходные данные:");
//        PrintDataTable(invoicesTable);

//        // Добавляем 5 новых счетов
//        AddNewInvoices(invoicesTable, 5);

//        // Модифицируем 3 текущих счета
//        ModifyInvoices(invoicesTable, 3);

//        // Удаляем 2 счета
//        DeleteInvoices(invoicesTable, 2);

//        // Создаём DataView для каждого состояния строк
//        CreateDataViewByState(invoicesTable, "CurrentRows", DataViewRowState.CurrentRows);
//        CreateDataViewByState(invoicesTable, "OriginalRows", DataViewRowState.OriginalRows);
//        CreateDataViewByState(invoicesTable, "Added", DataViewRowState.Added);
//        CreateDataViewByState(invoicesTable, "ModifiedOriginal", DataViewRowState.ModifiedOriginal);
//        CreateDataViewByState(invoicesTable, "ModifiedCurrent", DataViewRowState.ModifiedCurrent);
//        CreateDataViewByState(invoicesTable, "Deleted", DataViewRowState.Deleted);

//        // Комбинированные состояния
//        CreateDataViewByState(invoicesTable, "Added | ModifiedCurrent", DataViewRowState.Added | DataViewRowState.ModifiedCurrent);

//        // Отчёт обо всех изменениях перед AcceptChanges
//        Console.WriteLine("\n=== ОТЧЁТ ОБ ИЗМЕНЕНИЯХ ПЕРЕД AcceptChanges ===");
//        ReportChanges(invoicesTable);

//        // Откат изменений
//        Console.WriteLine("\n=== ОТКАТ ИЗМЕНЕНИЙ ===");
//        invoicesTable.RejectChanges();
//        Console.WriteLine("Изменения отменены. Текущие данные:");
//        PrintDataTable(invoicesTable);
//    }

//    static DataTable CreateInvoicesTable()
//    {
//        DataTable table = new DataTable("Счета");

//        table.Columns.Add("InvoiceID", typeof(int));
//        table.Columns.Add("CustomerName", typeof(string));
//        table.Columns.Add("Amount", typeof(decimal));
//        table.Columns.Add("Status", typeof(string));
//        table.Columns.Add("DueDate", typeof(DateTime));

//        table.PrimaryKey = new DataColumn[] { table.Columns["InvoiceID"] };

//        return table;
//    }

//    static void FillTestData(DataTable table)
//    {
//        Random random = new Random();
//        string[] customerNames = { "ООО Ромашка", "ИП Иванов", "ЗАО Альфа", "ОАО Бета", "ООО Гамма" };
//        string[] statuses = { "Оплачен", "Не оплачен", "Просрочен" };

//        for (int i = 1; i <= 10; i++)
//        {
//            table.Rows.Add(
//                i,
//                customerNames[random.Next(customerNames.Length)],
//                1000 + random.Next(9000),
//                statuses[random.Next(statuses.Length)],
//                DateTime.Now.AddDays(random.Next(-30, 30))
//            );
//        }
//    }

//    static void AddNewInvoices(DataTable table, int count)
//    {
//        Random random = new Random();
//        string[] customerNames = { "Новый Клиент 1", "Новый Клиент 2", "Новый Клиент 3", "Новый Клиент 4", "Новый Клиент 5" };
//        string[] statuses = { "Не оплачен", "Просрочен" };

//        int nextId = table.Rows.Count > 0 ? table.Rows.Cast<DataRow>().Max(r => r.Field<int>("InvoiceID")) + 1 : 1;

//        for (int i = 0; i < count; i++)
//        {
//            table.Rows.Add(
//                nextId + i,
//                customerNames[i],
//                1000 + random.Next(9000),
//                statuses[random.Next(statuses.Length)],
//                DateTime.Now.AddDays(random.Next(30, 60))
//            );
//        }

//        Console.WriteLine($"\nДобавлено {count} новых счетов.");
//    }

//    static void ModifyInvoices(DataTable table, int count)
//    {
//        Random random = new Random();

//        for (int i = 0; i < count && i < table.Rows.Count; i++)
//        {
//            DataRow row = table.Rows[i];
//            row["Amount"] = 1000 + random.Next(9000);
//            row["Status"] = "Изменён";
//        }

//        Console.WriteLine($"\nИзменено {count} счетов.");
//    }

//    static void DeleteInvoices(DataTable table, int count)
//    {
//        for (int i = 0; i < count && i < table.Rows.Count; i++)
//        {
//            table.Rows[i].Delete();
//        }

//        Console.WriteLine($"\nУдалено {count} счетов.");
//    }

//    static void CreateDataViewByState(DataTable table, string stateName, DataViewRowState state)
//    {
//        DataView view = new DataView(table, "", "", state);

//        Console.WriteLine($"\n=== DataViewRowState.{stateName} ===");
//        Console.WriteLine($"Количество строк: {view.Count}");

//        if (view.Count > 0)
//        {
//            Console.WriteLine("Содержимое строк:");
//            foreach (DataRowView rowView in view)
//            {
//                Console.WriteLine(
//                    $"Счёт {rowView["InvoiceID"]}: {rowView["CustomerName"]}, " +
//                    $"Сумма: {rowView["Amount"]:C}, " +
//                    $"Статус: {rowView.Row["Status"]}, " +
//                    $"Дата оплаты: {((DateTime)rowView["DueDate"]).ToShortDateString()}, " +
//                    $"Состояние: {rowView.Row.RowState}"
//                );
//            }
//        }
//        else
//        {
//            Console.WriteLine("Нет строк в данном состоянии.");
//        }
//    }

//    static void ReportChanges(DataTable table)
//    {
//        DataRow[] addedRows = table.Select("", "", DataViewRowState.Added);
//        DataRow[] modifiedRows = table.Select("", "", DataViewRowState.ModifiedCurrent);
//        DataRow[] deletedRows = table.Select("", "", DataViewRowState.Deleted);

//        Console.WriteLine($"Добавлено счетов: {addedRows.Length}");
//        Console.WriteLine($"Изменено счетов: {modifiedRows.Length}");
//        Console.WriteLine($"Удалено счетов: {deletedRows.Length}");
//    }

//    static void PrintDataTable(DataTable table)
//    {
//        foreach (DataRow row in table.Rows)
//        {
//            if (row.RowState != DataRowState.Deleted)
//            {
//                Console.WriteLine(
//                    $"Счёт {row["InvoiceID"]}: {row["CustomerName"]}, " +
//                    $"Сумма: {row["Amount"]:C}, " +
//                    $"Статус: {row["Status"]}, " +
//                    $"Дата оплаты: {((DateTime)row["DueDate"]).ToShortDateString()}, " +
//                    $"Состояние: {row.RowState}"
//                );
//            }
//        }
//    }
//}


//7
//using System;
//using System.Data;
//using System.Diagnostics;

//class BookSearchDemo
//{
//    static void Main()
//    {
//        // Создаём и заполняем DataTable "Книги"
//        DataTable booksTable = CreateBooksTable();
//        FillTestData(booksTable);

//        // Устанавливаем первичный ключ на BookID
//        booksTable.PrimaryKey = new DataColumn[] { booksTable.Columns["BookID"] };

//        Console.WriteLine("=== ПОИСК ДАННЫХ В DataView ===\n");

//        // Создаём DataView с фильтром по категории "Фантастика"
//        DataView fantasyView = new DataView(booksTable, "[Категория] = 'Фантастика'", "", DataViewRowState.CurrentRows);

//        // Поиск книг по ISBN с использованием Find() в DataTable
//        Console.WriteLine("Поиск книг по ISBN в DataTable:");
//        SearchByISBNInDataTable(booksTable, "978-5-12345-678-9");
//        SearchByISBNInDataTable(booksTable, "978-5-00000-000-0"); // Несуществующий ISBN

//        // Поиск книг по ISBN с использованием FindRows() в DataTable
//        Console.WriteLine("\nПоиск книг по ISBN с использованием FindRows:");
//        SearchByISBNWithFindRows(booksTable, "978-5-12345-678-9");

//        // Поиск книг по ISBN в DataView
//        Console.WriteLine("\nПоиск книг по ISBN в DataView:");
//        SearchByISBNInDataView(fantasyView, "978-5-12345-678-9");
//        SearchByISBNInDataView(fantasyView, "978-5-00000-000-0"); // Несуществующий ISBN

//        // Сравнение производительности FindRows в DataTable и поиска в DataView
//        Console.WriteLine("\nСравнение производительности поиска в DataTable и DataView:");
//        CompareSearchPerformance(booksTable, fantasyView, "978-5-12345-678-9");

//        // Поиск по нескольким параметрам (автор и год)
//        Console.WriteLine("\nПоиск по нескольким параметрам (автор и год):");
//        SearchByAuthorAndYear(booksTable, "Иванов Иван", 2020);

//        // Поиск с использованием BinarySearch() в отсортированном DataView
//        Console.WriteLine("\nПоиск с использованием BinarySearch() в отсортированном DataView:");
//        SearchWithBinarySearch(fantasyView, "978-5-12345-678-9");
//    }

//    static DataTable CreateBooksTable()
//    {
//        DataTable table = new DataTable("Книги");

//        table.Columns.Add("BookID", typeof(int));
//        table.Columns.Add("Название", typeof(string));
//        table.Columns.Add("Автор", typeof(string));
//        table.Columns.Add("ISBN", typeof(string));
//        table.Columns.Add("Год", typeof(int));
//        table.Columns.Add("Цена", typeof(decimal));
//        table.Columns.Add("Категория", typeof(string));

//        return table;
//    }

//    static void FillTestData(DataTable table)
//    {
//        string[] titles = { "Война и мир", "Преступление и наказание", "Мастер и Маргарита", "1984", "Гарри Поттер" };
//        string[] authors = { "Толстой Лев", "Достоевский Федор", "Булгаков Михаил", "Оруэлл Джордж", "Роулинг Джоан" };
//        string[] categories = { "Классика", "Фантастика", "Детектив", "Приключения" };
//        Random random = new Random();

//        for (int i = 1; i <= 100; i++)
//        {
//            table.Rows.Add(
//                i,
//                titles[random.Next(titles.Length)] + " " + i,
//                authors[random.Next(authors.Length)],
//                $"978-5-{10000 + i:D5}-{random.Next(100, 999)}-{random.Next(0, 9)}",
//                1950 + random.Next(70),
//                Math.Round(100 + random.NextDouble() * 900, 2),
//                categories[random.Next(categories.Length)]
//            );
//        }
//    }

//    static void SearchByISBNInDataTable(DataTable table, string isbn)
//    {
//        DataRow foundRow = null;
//        foreach (DataRow row in table.Rows)
//        {
//            if (row["ISBN"].ToString() == isbn)
//            {
//                foundRow = row;
//                break;
//            }
//        }

//        if (foundRow != null)
//        {
//            Console.WriteLine($"Найдена книга по ISBN {isbn}:");
//            PrintBookInfo(foundRow);
//        }
//        else
//        {
//            Console.WriteLine($"Книга с ISBN {isbn} не найдена.");
//        }
//    }

//    static void SearchByISBNWithFindRows(DataTable table, string isbn)
//    {
//        try
//        {
//            DataRow[] foundRows = table.Select($"ISBN = '{isbn}'");

//            if (foundRows.Length > 0)
//            {
//                Console.WriteLine($"Найдена книга по ISBN {isbn}:");
//                foreach (DataRow row in foundRows)
//                {
//                    PrintBookInfo(row);
//                }
//            }
//            else
//            {
//                Console.WriteLine($"Книга с ISBN {isbn} не найдена.");
//            }
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Ошибка при поиске: {ex.Message}");
//        }
//    }

//    static void SearchByISBNInDataView(DataView view, string isbn)
//    {
//        int index = -1;
//        for (int i = 0; i < view.Count; i++)
//        {
//            if (view[i]["ISBN"].ToString() == isbn)
//            {
//                index = i;
//                break;
//            }
//        }

//        if (index != -1)
//        {
//            Console.WriteLine($"Найдена книга по ISBN {isbn}:");
//            PrintBookInfo(view[index].Row);
//        }
//        else
//        {
//            Console.WriteLine($"Книга с ISBN {isbn} не найдена.");
//        }
//    }

//    static void CompareSearchPerformance(DataTable table, DataView view, string isbn)
//    {
//        Stopwatch stopwatch = new Stopwatch();

//        // Поиск в DataTable
//        stopwatch.Start();
//        DataRow[] foundRowsInTable = table.Select($"ISBN = '{isbn}'");
//        stopwatch.Stop();
//        long tableTime = stopwatch.ElapsedTicks;

//        // Поиск в DataView
//        stopwatch.Restart();
//        int indexInView = -1;
//        for (int i = 0; i < view.Count; i++)
//        {
//            if (view[i]["ISBN"].ToString() == isbn)
//            {
//                indexInView = i;
//                break;
//            }
//        }
//        stopwatch.Stop();
//        long viewTime = stopwatch.ElapsedTicks;

//        Console.WriteLine($"Поиск в DataTable занял {tableTime} тиков.");
//        Console.WriteLine($"Поиск в DataView занял {viewTime} тиков.");
//    }

//    static void SearchByAuthorAndYear(DataTable table, string author, int year)
//    {
//        DataRow[] foundRows = table.Select($"[Автор] = '{author}' AND [Год] = {year}");

//        if (foundRows.Length > 0)
//        {
//            Console.WriteLine($"Найдено {foundRows.Length} книг автора {author} за {year} год:");
//            foreach (DataRow row in foundRows)
//            {
//                PrintBookInfo(row);
//            }
//        }
//        else
//        {
//            Console.WriteLine($"Книги автора {author} за {year} год не найдены.");
//        }
//    }

//    static void SearchWithBinarySearch(DataView view, string isbn)
//    {
//        // Сортируем DataView по ISBN
//        view.Sort = "ISBN ASC";

//        // Используем Find для поиска
//        int index = view.Find(isbn);

//        if (index != -1)
//        {
//            Console.WriteLine($"Найдена книга по ISBN {isbn} с использованием BinarySearch:");
//            PrintBookInfo(view[index].Row);
//        }
//        else
//        {
//            Console.WriteLine($"Книга с ISBN {isbn} не найдена с использованием BinarySearch.");
//        }
//    }

//    static void PrintBookInfo(DataRow row)
//    {
//        Console.WriteLine(
//            $"ID: {row["BookID"]}, " +
//            $"Название: {row["Название"]}, " +
//            $"Автор: {row["Автор"]}, " +
//            $"ISBN: {row["ISBN"]}, " +
//            $"Год: {row["Год"]}, " +
//            $"Цена: {row["Цена"]:C}, " +
//            $"Категория: {row["Категория"]}"
//        );
//    }
//}


//8
//using System;
//using System.Data;
//using System.Text.RegularExpressions;

//class CustomerDataViewDemo
//{
//    static void Main()
//    {
//        // Создаём и заполняем DataTable "Клиенты"
//        DataTable customersTable = CreateCustomersTable();

//        // Создаём DataView с фильтром по статусу "Active"
//        DataView activeCustomersView = new DataView(customersTable, "[Статус] = 'Active'", "", DataViewRowState.CurrentRows);

//        Console.WriteLine("=== ДОБАВЛЕНИЕ ДАННЫХ ЧЕРЕЗ DataView ===\n");

//        // Выводим исходные данные
//        Console.WriteLine("Исходные данные:");
//        PrintDataView(activeCustomersView);

//        // Добавление нового клиента через DataView.AddNew()
//        Console.WriteLine("\nДобавление нового клиента через DataView.AddNew():");
//        AddCustomerThroughDataView(activeCustomersView, 1, "Иван Иванов", "ivan@example.com", "1234567890", "Москва", "Active");

//        // Выводим данные после добавления
//        Console.WriteLine("\nДанные после добавления через DataView.AddNew():");
//        PrintDataView(activeCustomersView);

//        // Добавление новой строки в исходную таблицу и проверка её показа в DataView
//        Console.WriteLine("\nДобавление новой строки в исходную таблицу:");
//        AddCustomerToDataTable(customersTable, 2, "Петр Петров", "petr@example.com", "0987654321", "Санкт-Петербург", "Active");

//        // Выводим данные после добавления в исходную таблицу
//        Console.WriteLine("\nДанные после добавления в исходную таблицу:");
//        PrintDataView(activeCustomersView);

//        // Добавление клиента с нарушением фильтра
//        Console.WriteLine("\nДобавление клиента с нарушением фильтра:");
//        AddCustomerThroughDataView(activeCustomersView, 3, "Сидор Сидоров", "sidor@example.com", "5556667777", "Новосибирск", "Inactive");

//        // Выводим данные после добавления клиента с нарушением фильтра
//        Console.WriteLine("\nДанные после добавления клиента с нарушением фильтра:");
//        PrintDataView(activeCustomersView);

//        // Добавление клиента с валидацией
//        Console.WriteLine("\nДобавление клиента с валидацией:");
//        AddCustomerWithValidation(activeCustomersView, 4, "Алексей Алексеев", "invalid-email", "1112223333", "Екатеринбург", "Active");

//        // Выводим данные после добавления клиента с валидацией
//        Console.WriteLine("\nДанные после попытки добавления клиента с валидацией:");
//        PrintDataView(activeCustomersView);

//        // Обработка исключений при нарушении ограничения
//        Console.WriteLine("\nОбработка исключений при нарушении ограничения:");
//        AddCustomerWithExceptionHandling(customersTable, activeCustomersView, 1, "Иван Иванов", "ivan@example.com", "1234567890", "Москва", "Active");

//        // Выводим данные после обработки исключений
//        Console.WriteLine("\nДанные после обработки исключений:");
//        PrintDataView(activeCustomersView);

//        // Создаём ещё один DataView с другим фильтром
//        DataView allCustomersView = new DataView(customersTable, "", "", DataViewRowState.CurrentRows);

//        // Выводим данные из другого DataView
//        Console.WriteLine("\nДанные из другого DataView (все клиенты):");
//        PrintDataView(allCustomersView);
//    }

//    static DataTable CreateCustomersTable()
//    {
//        DataTable table = new DataTable("Клиенты");

//        table.Columns.Add("CustomerID", typeof(int));
//        table.Columns.Add("Имя", typeof(string));
//        table.Columns.Add("Электронная почта", typeof(string));
//        table.Columns.Add("Телефон", typeof(string));
//        table.Columns.Add("Город", typeof(string));
//        table.Columns.Add("Статус", typeof(string));

//        table.PrimaryKey = new DataColumn[] { table.Columns["CustomerID"] };

//        return table;
//    }

//    static void AddCustomerThroughDataView(DataView view, int customerID, string name, string email, string phone, string city, string status)
//    {
//        try
//        {
//            DataRowView newRow = view.AddNew();
//            newRow["CustomerID"] = customerID;
//            newRow["Имя"] = name;
//            newRow["Электронная почта"] = email;
//            newRow["Телефон"] = phone;
//            newRow["Город"] = city;
//            newRow["Статус"] = status;
//            newRow.EndEdit();

//            Console.WriteLine($"Клиент {name} успешно добавлен.");
//        }
//        catch (ConstraintException ex)
//        {
//            Console.WriteLine($"Ошибка при добавлении клиента: {ex.Message}");
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Неизвестная ошибка: {ex.Message}");
//        }
//    }

//    static void AddCustomerToDataTable(DataTable table, int customerID, string name, string email, string phone, string city, string status)
//    {
//        try
//        {
//            DataRow newRow = table.NewRow();
//            newRow["CustomerID"] = customerID;
//            newRow["Имя"] = name;
//            newRow["Электронная почта"] = email;
//            newRow["Телефон"] = phone;
//            newRow["Город"] = city;
//            newRow["Статус"] = status;
//            table.Rows.Add(newRow);

//            Console.WriteLine($"Клиент {name} успешно добавлен в исходную таблицу.");
//        }
//        catch (ConstraintException ex)
//        {
//            Console.WriteLine($"Ошибка при добавлении клиента: {ex.Message}");
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Неизвестная ошибка: {ex.Message}");
//        }
//    }

//    static void AddCustomerWithValidation(DataView view, int customerID, string name, string email, string phone, string city, string status)
//    {
//        try
//        {
//            if (!IsValidEmail(email))
//            {
//                throw new ArgumentException("Некорректный адрес электронной почты.");
//            }

//            if (name.Length < 3)
//            {
//                throw new ArgumentException("Имя должно содержать не менее 3 символов.");
//            }

//            DataRowView newRow = view.AddNew();
//            newRow["CustomerID"] = customerID;
//            newRow["Имя"] = name;
//            newRow["Электронная почта"] = email;
//            newRow["Телефон"] = phone;
//            newRow["Город"] = city;
//            newRow["Статус"] = status;
//            newRow.EndEdit();

//            Console.WriteLine($"Клиент {name} успешно добавлен.");
//        }
//        catch (ConstraintException ex)
//        {
//            Console.WriteLine($"Ошибка при добавлении клиента: {ex.Message}");
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Ошибка валидации: {ex.Message}");
//        }
//    }

//    static void AddCustomerWithExceptionHandling(DataTable table, DataView view, int customerID, string name, string email, string phone, string city, string status)
//    {
//        try
//        {
//            DataRow newRow = table.NewRow();
//            newRow["CustomerID"] = customerID;
//            newRow["Имя"] = name;
//            newRow["Электронная почта"] = email;
//            newRow["Телефон"] = phone;
//            newRow["Город"] = city;
//            newRow["Статус"] = status;
//            table.Rows.Add(newRow);

//            Console.WriteLine($"Клиент {name} успешно добавлен.");
//        }
//        catch (ConstraintException ex)
//        {
//            Console.WriteLine($"Ошибка ограничения: {ex.Message}");
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Неизвестная ошибка: {ex.Message}");
//        }
//    }

//    static bool IsValidEmail(string email)
//    {
//        try
//        {
//            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
//            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
//            return regex.IsMatch(email);
//        }
//        catch (RegexMatchTimeoutException)
//        {
//            return false;
//        }
//    }

//    static void PrintDataView(DataView view)
//    {
//        if (view.Count == 0)
//        {
//            Console.WriteLine("Нет данных для отображения.");
//            return;
//        }

//        foreach (DataRowView rowView in view)
//        {
//            Console.WriteLine(
//                $"ID: {rowView["CustomerID"]}, " +
//                $"Имя: {rowView["Имя"]}, " +
//                $"Электронная почта: {rowView["Электронная почта"]}, " +
//                $"Телефон: {rowView["Телефон"]}, " +
//                $"Город: {rowView["Город"]}, " +
//                $"Статус: {rowView["Статус"]}"
//            );
//        }
//    }
//}


//9
//using System;
//using System.Data;
//using System.Collections.Generic;

//class EmployeeDataViewDemo
//{
//    static void Main()
//    {
//        // Создаём и заполняем DataTable "Сотрудники"
//        DataTable employeesTable = CreateEmployeesTable();
//        FillTestData(employeesTable);

//        // Создаём DataView с фильтром по отделу "IT"
//        DataView itEmployeesView = new DataView(employeesTable, "[Отдел] = 'IT'", "", DataViewRowState.CurrentRows);

//        Console.WriteLine("=== РЕДАКТИРОВАНИЕ ДАННЫХ ЧЕРЕЗ DataView ===\n");

//        // Выводим исходные данные
//        Console.WriteLine("Исходные данные (IT отдел):");
//        PrintDataView(itEmployeesView);

//        // Поиск сотрудника через Find()
//        Console.WriteLine("\nПоиск сотрудника с EmployeeID = 1:");
//        FindAndEditEmployee(itEmployeesView, 1, 50000, "Активен");

//        // Выводим данные после редактирования
//        Console.WriteLine("\nДанные после редактирования:");
//        PrintDataView(itEmployeesView);

//        // Массовое редактирование (повышение зарплат на 10% для всех в отделе)
//        Console.WriteLine("\nМассовое редактирование (повышение зарплат на 10% для всех в IT отделе):");
//        MassEditSalaries(itEmployeesView, 10);

//        // Выводим данные после массового редактирования
//        Console.WriteLine("\nДанные после массового редактирования:");
//        PrintDataView(itEmployeesView);

//        // Редактирование с валидацией
//        Console.WriteLine("\nРедактирование с валидацией:");
//        EditEmployeeWithValidation(itEmployeesView, 2, -1000, "Неактивен");

//        // Выводим данные после редактирования с валидацией
//        Console.WriteLine("\nДанные после редактирования с валидацией:");
//        PrintDataView(itEmployeesView);

//        // Проверка отражения изменений в исходной таблице
//        Console.WriteLine("\nПроверка отражения изменений в исходной таблице:");
//        PrintDataTable(employeesTable);

//        // Создаём ещё один DataView с другим фильтром
//        DataView activeEmployeesView = new DataView(employeesTable, "[Статус] = 'Активен'", "", DataViewRowState.CurrentRows);

//        // Выводим данные из другого DataView
//        Console.WriteLine("\nДанные из другого DataView (все активные сотрудники):");
//        PrintDataView(activeEmployeesView);

//        // Отчёт обо всех изменениях
//        Console.WriteLine("\nОтчёт обо всех изменениях:");
//        ReportChanges(employeesTable);
//    }

//    static DataTable CreateEmployeesTable()
//    {
//        DataTable table = new DataTable("Сотрудники");

//        table.Columns.Add("EmployeeID", typeof(int));
//        table.Columns.Add("Имя", typeof(string));
//        table.Columns.Add("Зарплата", typeof(decimal));
//        table.Columns.Add("Отдел", typeof(string));
//        table.Columns.Add("Дата Найма", typeof(DateTime));
//        table.Columns.Add("Статус", typeof(string));

//        table.PrimaryKey = new DataColumn[] { table.Columns["EmployeeID"] };

//        return table;
//    }

//    static void FillTestData(DataTable table)
//    {
//        Random random = new Random();
//        string[] names = { "Иванов", "Петров", "Сидоров", "Кузнецов", "Васильев", "Смирнов", "Новиков", "Федоров", "Морозов", "Волков" };
//        string[] departments = { "IT", "HR", "Finance", "Marketing", "Sales" };
//        string[] statuses = { "Активен", "Неактивен", "В отпуске" };

//        for (int i = 1; i <= 50; i++)
//        {
//            table.Rows.Add(
//                i,
//                $"{names[random.Next(names.Length)]} {names[random.Next(names.Length)]}",
//                30000 + random.Next(10) * 10000,
//                departments[random.Next(departments.Length)],
//                DateTime.Now.AddDays(-random.Next(365 * 5)),
//                statuses[random.Next(statuses.Length)]
//            );
//        }
//    }

//    static void FindAndEditEmployee(DataView view, int employeeID, decimal newSalary, string newStatus)
//    {
//        try
//        {
//            // Сортируем DataView по EmployeeID для использования Find()
//            view.Sort = "EmployeeID ASC";

//            // Ищем сотрудника
//            int index = view.Find(employeeID);

//            if (index != -1)
//            {
//                Console.WriteLine($"Найден сотрудник с EmployeeID = {employeeID}.");

//                // Редактируем данные сотрудника
//                DataRowView rowView = view[index];
//                Console.WriteLine("До редактирования:");
//                PrintEmployeeInfo(rowView.Row);

//                rowView["Зарплата"] = newSalary;
//                rowView["Статус"] = newStatus;

//                Console.WriteLine("После редактирования:");
//                PrintEmployeeInfo(rowView.Row);
//            }
//            else
//            {
//                Console.WriteLine($"Сотрудник с EmployeeID = {employeeID} не найден.");
//            }
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Ошибка при поиске или редактировании сотрудника: {ex.Message}");
//        }
//    }

//    static void MassEditSalaries(DataView view, int percentage)
//    {
//        try
//        {
//            Console.WriteLine($"Повышение зарплат на {percentage}% для всех сотрудников в отделе.");

//            foreach (DataRowView rowView in view)
//            {
//                decimal currentSalary = (decimal)rowView["Зарплата"];
//                decimal newSalary = currentSalary * (1 + percentage / 100.0m);
//                rowView["Зарплата"] = Math.Round(newSalary, 2);
//            }

//            Console.WriteLine("Зарплаты успешно повышены.");
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Ошибка при массовом редактировании зарплат: {ex.Message}");
//        }
//    }

//    static void EditEmployeeWithValidation(DataView view, int employeeID, decimal newSalary, string newStatus)
//    {
//        try
//        {
//            // Сортируем DataView по EmployeeID для использования Find()
//            view.Sort = "EmployeeID ASC";

//            // Ищем сотрудника
//            int index = view.Find(employeeID);

//            if (index != -1)
//            {
//                Console.WriteLine($"Найден сотрудник с EmployeeID = {employeeID}.");

//                // Проверяем валидацию
//                if (newSalary <= 0)
//                {
//                    throw new ArgumentException("Зарплата должна быть больше 0.");
//                }

//                List<string> validStatuses = new List<string> { "Активен", "Неактивен", "В отпуске" };
//                if (!validStatuses.Contains(newStatus))
//                {
//                    throw new ArgumentException("Некорректный статус.");
//                }

//                // Редактируем данные сотрудника
//                DataRowView rowView = view[index];
//                Console.WriteLine("До редактирования:");
//                PrintEmployeeInfo(rowView.Row);

//                rowView["Зарплата"] = newSalary;
//                rowView["Статус"] = newStatus;

//                Console.WriteLine("После редактирования:");
//                PrintEmployeeInfo(rowView.Row);
//            }
//            else
//            {
//                Console.WriteLine($"Сотрудник с EmployeeID = {employeeID} не найден.");
//            }
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Ошибка при редактировании сотрудника: {ex.Message}");
//        }
//    }

//    static void PrintDataView(DataView view)
//    {
//        if (view.Count == 0)
//        {
//            Console.WriteLine("Нет данных для отображения.");
//            return;
//        }

//        foreach (DataRowView rowView in view)
//        {
//            PrintEmployeeInfo(rowView.Row);
//        }
//    }

//    static void PrintDataTable(DataTable table)
//    {
//        foreach (DataRow row in table.Rows)
//        {
//            if (row.RowState != DataRowState.Deleted)
//            {
//                PrintEmployeeInfo(row);
//            }
//        }
//    }

//    static void PrintEmployeeInfo(DataRow row)
//    {
//        Console.WriteLine(
//            $"ID: {row["EmployeeID"]}, " +
//            $"Имя: {row["Имя"]}, " +
//            $"Зарплата: {row["Зарплата"]:C}, " +
//            $"Отдел: {row["Отдел"]}, " +
//            $"Дата Найма: {((DateTime)row["Дата Найма"]).ToShortDateString()}, " +
//            $"Статус: {row["Статус"]}"
//        );
//    }

//    static void ReportChanges(DataTable table)
//    {
//        DataRow[] addedRows = table.Select("", "", DataViewRowState.Added);
//        DataRow[] modifiedRows = table.Select("", "", DataViewRowState.ModifiedCurrent);
//        DataRow[] deletedRows = table.Select("", "", DataViewRowState.Deleted);

//        Console.WriteLine($"Добавлено сотрудников: {addedRows.Length}");
//        Console.WriteLine($"Изменено сотрудников: {modifiedRows.Length}");
//        Console.WriteLine($"Удалено сотрудников: {deletedRows.Length}");
//    }
//}


//10
//using System;
//using System.Data;
//using System.Collections.Generic;

//class OrderDataViewDemo
//{
//    static void Main()
//    {
//        // Создаём и заполняем DataTable "Заказы"
//        DataTable ordersTable = CreateOrdersTable();
//        FillTestData(ordersTable);

//        // Создаём DataView с фильтром по статусу "Отменено"
//        DataView cancelledOrdersView = new DataView(ordersTable, "[Статус] = 'Отменено'", "", DataViewRowState.CurrentRows);

//        Console.WriteLine("=== УДАЛЕНИЕ ДАННЫХ ЧЕРЕЗ DataView ===\n");

//        // Выводим исходные данные
//        Console.WriteLine("Исходные данные (заказы со статусом 'Отменено'):");
//        PrintDataView(cancelledOrdersView);

//        // Удаление заказа через Delete()
//        Console.WriteLine("\nУдаление заказа с OrderID = 1:");
//        DeleteOrderByID(cancelledOrdersView, 1);

//        // Выводим данные после удаления
//        Console.WriteLine("\nДанные после удаления заказа с OrderID = 1:");
//        PrintDataView(cancelledOrdersView);

//        // Массовое удаление (все заказы со статусом «Отменено»)
//        Console.WriteLine("\nМассовое удаление всех заказов со статусом 'Отменено':");
//        MassDeleteOrders(cancelledOrdersView);

//        // Выводим данные после массового удаления
//        Console.WriteLine("\nДанные после массового удаления:");
//        PrintDataView(cancelledOrdersView);

//        // Удаление с подтверждением
//        Console.WriteLine("\nУдаление с подтверждением заказа с OrderID = 2:");
//        DeleteOrderWithConfirmation(ordersTable, 2);

//        // Выводим данные после удаления с подтверждением
//        Console.WriteLine("\nДанные после удаления с подтверждением:");
//        PrintDataView(new DataView(ordersTable, "[Статус] = 'Отменено'", "", DataViewRowState.CurrentRows));

//        // Откат удаления
//        Console.WriteLine("\nОткат удаления:");
//        RollbackDeletion(ordersTable);

//        // Выводим данные после отката удаления
//        Console.WriteLine("\nДанные после отката удаления:");
//        PrintDataView(new DataView(ordersTable, "[Статус] = 'Отменено'", "", DataViewRowState.CurrentRows));

//        // Отчёт об удалённых заказах
//        Console.WriteLine("\nОтчёт об удалённых заказах:");
//        ReportDeletedOrders(ordersTable);

//        // Статистика перед AcceptChanges
//        Console.WriteLine("\nСтатистика перед AcceptChanges:");
//        StatisticsBeforeAcceptChanges(ordersTable);
//    }

//    static DataTable CreateOrdersTable()
//    {
//        DataTable table = new DataTable("Заказы");

//        table.Columns.Add("OrderID", typeof(int));
//        table.Columns.Add("CustomerID", typeof(int));
//        table.Columns.Add("Сумма", typeof(decimal));
//        table.Columns.Add("Статус", typeof(string));
//        table.Columns.Add("Дата создания", typeof(DateTime));

//        table.PrimaryKey = new DataColumn[] { table.Columns["OrderID"] };

//        return table;
//    }

//    static void FillTestData(DataTable table)
//    {
//        Random random = new Random();
//        string[] statuses = { "Новый", "В обработке", "Отменено", "Выполнено", "Оплачено" };

//        for (int i = 1; i <= 100; i++)
//        {
//            table.Rows.Add(
//                i,
//                1000 + i,
//                Math.Round(100 + random.NextDouble() * 9900, 2),
//                statuses[random.Next(statuses.Length)],
//                DateTime.Now.AddDays(-random.Next(365))
//            );
//        }
//    }

//    static void DeleteOrderByID(DataView view, int orderID)
//    {
//        try
//        {
//            // Сортируем DataView по OrderID для использования Find()
//            view.Sort = "OrderID ASC";

//            // Ищем заказ
//            int index = view.Find(orderID);

//            if (index != -1)
//            {
//                Console.WriteLine($"Найден заказ с OrderID = {orderID}.");

//                // Удаляем заказ
//                view[index].Delete();

//                Console.WriteLine($"Заказ с OrderID = {orderID} удалён.");
//            }
//            else
//            {
//                Console.WriteLine($"Заказ с OrderID = {orderID} не найден.");
//            }
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Ошибка при удалении заказа: {ex.Message}");
//        }
//    }

//    static void MassDeleteOrders(DataView view)
//    {
//        try
//        {
//            Console.WriteLine("Удаление всех заказов со статусом 'Отменено'...");

//            int deletedCount = 0;
//            foreach (DataRowView rowView in view)
//            {
//                rowView.Delete();
//                deletedCount++;
//            }

//            Console.WriteLine($"Удалено {deletedCount} заказов.");
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Ошибка при массовом удалении заказов: {ex.Message}");
//        }
//    }

//    static void DeleteOrderWithConfirmation(DataTable table, int orderID)
//    {
//        try
//        {
//            DataRow orderRow = table.Rows.Find(orderID);

//            if (orderRow != null)
//            {
//                Console.WriteLine($"Найден заказ с OrderID = {orderID}.");
//                PrintOrderInfo(orderRow);

//                Console.Write("Подтвердите удаление (y/n): ");
//                string confirmation = Console.ReadLine();

//                if (confirmation.ToLower() == "y")
//                {
//                    orderRow.Delete();
//                    Console.WriteLine($"Заказ с OrderID = {orderID} удалён.");
//                }
//                else
//                {
//                    Console.WriteLine("Удаление отменено.");
//                }
//            }
//            else
//            {
//                Console.WriteLine($"Заказ с OrderID = {orderID} не найден.");
//            }
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Ошибка при удалении заказа: {ex.Message}");
//        }
//    }

//    static void RollbackDeletion(DataTable table)
//    {
//        try
//        {
//            // Находим все удалённые строки
//            DataRow[] deletedRows = table.Select("", "", DataViewRowState.Deleted);

//            if (deletedRows.Length > 0)
//            {
//                Console.WriteLine($"Найдено {deletedRows.Length} удалённых заказов. Откат изменений...");

//                // Откат удаления для каждой строки
//                foreach (DataRow row in deletedRows)
//                {
//                    row.RejectChanges();
//                }

//                Console.WriteLine("Откат удаления выполнен.");
//            }
//            else
//            {
//                Console.WriteLine("Нет удалённых заказов для отката.");
//            }
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Ошибка при откате удаления: {ex.Message}");
//        }
//    }

//    static void ReportDeletedOrders(DataTable table)
//    {
//        try
//        {
//            DataRow[] deletedRows = table.Select("", "", DataViewRowState.Deleted);

//            if (deletedRows.Length > 0)
//            {
//                Console.WriteLine($"Найдено {deletedRows.Length} удалённых заказов:");

//                foreach (DataRow row in deletedRows)
//                {
//                    PrintOrderInfo(row);
//                }
//            }
//            else
//            {
//                Console.WriteLine("Нет удалённых заказов.");
//            }
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Ошибка при составлении отчёта об удалённых заказах: {ex.Message}");
//        }
//    }

//    static void StatisticsBeforeAcceptChanges(DataTable table)
//    {
//        try
//        {
//            DataRow[] addedRows = table.Select("", "", DataViewRowState.Added);
//            DataRow[] modifiedRows = table.Select("", "", DataViewRowState.ModifiedCurrent);
//            DataRow[] deletedRows = table.Select("", "", DataViewRowState.Deleted);

//            Console.WriteLine($"Добавлено заказов: {addedRows.Length}");
//            Console.WriteLine($"Изменено заказов: {modifiedRows.Length}");
//            Console.WriteLine($"Удалено заказов: {deletedRows.Length}");
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Ошибка при получении статистики: {ex.Message}");
//        }
//    }

//    static void PrintDataView(DataView view)
//    {
//        if (view.Count == 0)
//        {
//            Console.WriteLine("Нет данных для отображения.");
//            return;
//        }

//        foreach (DataRowView rowView in view)
//        {
//            PrintOrderInfo(rowView.Row);
//        }
//    }

//    static void PrintOrderInfo(DataRow row)
//    {
//        Console.WriteLine(
//            $"OrderID: {row["OrderID"]}, " +
//            $"CustomerID: {row["CustomerID"]}, " +
//            $"Сумма: {row["Сумма"]:C}, " +
//            $"Статус: {row["Статус"]}, " +
//            $"Дата создания: {((DateTime)row["Дата создания"]).ToShortDateString()}, " +
//            $"Состояние: {row.RowState}"
//        );
//    }
//}


//11
//using System;
//using System.Data;

//class CreateDataTableFromDataViewDemo
//{
//    static void Main()
//    {
//        // Создаём и заполняем DataTable "Продукты"
//        DataTable productsTable = CreateProductsTable();
//        FillTestData(productsTable);

//        // Создаём DataView с фильтром и сортировкой
//        DataView expensiveProductsView = new DataView(productsTable, "[Цена] > 1000", "[Цена] DESC", DataViewRowState.CurrentRows);

//        Console.WriteLine("=== СОЗДАНИЕ НОВОЙ DATATABLE ИЗ DATAVIEW ===\n");

//        // Выводим исходные данные
//        Console.WriteLine("Исходные данные (продукты дороже 1000):");
//        PrintDataView(expensiveProductsView);

//        // Способ 1: Использование DataView.ToTable()
//        Console.WriteLine("\nСпособ 1: Использование DataView.ToTable()");
//        DataTable tableFromToTable = CreateTableFromToTable(expensiveProductsView);
//        Console.WriteLine("Новая таблица из DataView.ToTable():");
//        PrintDataTable(tableFromToTable);

//        // Способ 2: Ручное копирование данных через цикл
//        Console.WriteLine("\nСпособ 2: Ручное копирование данных через цикл");
//        DataTable tableFromManualCopy = CreateTableFromManualCopy(expensiveProductsView);
//        Console.WriteLine("Новая таблица из ручного копирования:");
//        PrintDataTable(tableFromManualCopy);

//        // Способ 3: Копирование только нескольких колонок
//        Console.WriteLine("\nСпособ 3: Копирование только нескольких колонок");
//        DataTable tableFromSelectedColumns = CreateTableFromSelectedColumns(expensiveProductsView);
//        Console.WriteLine("Новая таблица из выбранных колонок:");
//        PrintDataTable(tableFromSelectedColumns);

//        // Способ 4: Копирование с изменением имён колонок
//        Console.WriteLine("\nСпособ 4: Копирование с изменением имён колонок");
//        DataTable tableWithRenamedColumns = CreateTableWithRenamedColumns(expensiveProductsView);
//        Console.WriteLine("Новая таблица с переименованными колонками:");
//        PrintDataTable(tableWithRenamedColumns);

//        // Создание таблиц с преобразованием данных (цена в другой валюте)
//        Console.WriteLine("\nСоздание таблиц с преобразованием данных (цена в другой валюте)");
//        DataTable tableWithConvertedPrice = CreateTableWithConvertedPrice(expensiveProductsView);
//        Console.WriteLine("Новая таблица с конвертированной ценой:");
//        PrintDataTable(tableWithConvertedPrice);

//        // Создание с добавлением новых расчетных колонок
//        Console.WriteLine("\nСоздание с добавлением новых расчетных колонок");
//        DataTable tableWithCalculatedColumns = CreateTableWithCalculatedColumns(expensiveProductsView);
//        Console.WriteLine("Новая таблица с расчетными колонками:");
//        PrintDataTable(tableWithCalculatedColumns);

//        // Сравнение исходной таблицы и созданной
//        Console.WriteLine("\nСравнение исходной таблицы и созданной:");
//        CompareTables(productsTable, tableWithCalculatedColumns);
//    }

//    static DataTable CreateProductsTable()
//    {
//        DataTable table = new DataTable("Продукты");

//        table.Columns.Add("ProductID", typeof(int));
//        table.Columns.Add("Название", typeof(string));
//        table.Columns.Add("Категория", typeof(string));
//        table.Columns.Add("Цена", typeof(decimal));
//        table.Columns.Add("В наличии", typeof(int));
//        table.Columns.Add("Поставщик", typeof(string));

//        table.PrimaryKey = new DataColumn[] { table.Columns["ProductID"] };

//        return table;
//    }

//    static void FillTestData(DataTable table)
//    {
//        Random random = new Random();
//        string[] names = { "Ноутбук", "Смартфон", "Планшет", "Монитор", "Клавиатура", "Мышь", "Наушники", "Колонка", "Микрофон", "Веб-камера" };
//        string[] categories = { "Электроника", "Аксессуары", "Периферия" };
//        string[] suppliers = { "Поставщик1", "Поставщик2", "Поставщик3" };

//        for (int i = 1; i <= 100; i++)
//        {
//            table.Rows.Add(
//                i,
//                $"{names[random.Next(names.Length)]} {i}",
//                categories[random.Next(categories.Length)],
//                Math.Round(500 + random.NextDouble() * 9500, 2),
//                random.Next(100),
//                suppliers[random.Next(suppliers.Length)]
//            );
//        }
//    }

//    static DataTable CreateTableFromToTable(DataView view)
//    {
//        return view.ToTable();
//    }

//    static DataTable CreateTableFromManualCopy(DataView view)
//    {
//        DataTable newTable = view.Table.Clone();

//        foreach (DataRowView rowView in view)
//        {
//            DataRow newRow = newTable.NewRow();
//            newRow.ItemArray = rowView.Row.ItemArray;
//            newTable.Rows.Add(newRow);
//        }

//        return newTable;
//    }

//    static DataTable CreateTableFromSelectedColumns(DataView view)
//    {
//        DataTable newTable = new DataTable("SelectedColumns");

//        // Добавляем только нужные колонки
//        newTable.Columns.Add("ProductID", typeof(int));
//        newTable.Columns.Add("Название", typeof(string));
//        newTable.Columns.Add("Цена", typeof(decimal));

//        foreach (DataRowView rowView in view)
//        {
//            DataRow newRow = newTable.NewRow();
//            newRow["ProductID"] = rowView["ProductID"];
//            newRow["Название"] = rowView["Название"];
//            newRow["Цена"] = rowView["Цена"];
//            newTable.Rows.Add(newRow);
//        }

//        return newTable;
//    }

//    static DataTable CreateTableWithRenamedColumns(DataView view)
//    {
//        DataTable newTable = new DataTable("RenamedColumns");

//        // Добавляем колонки с новыми именами
//        newTable.Columns.Add("ID", typeof(int));
//        newTable.Columns.Add("ProductName", typeof(string));
//        newTable.Columns.Add("Category", typeof(string));
//        newTable.Columns.Add("Price", typeof(decimal));
//        newTable.Columns.Add("InStock", typeof(int));
//        newTable.Columns.Add("Supplier", typeof(string));

//        foreach (DataRowView rowView in view)
//        {
//            DataRow newRow = newTable.NewRow();
//            newRow["ID"] = rowView["ProductID"];
//            newRow["ProductName"] = rowView["Название"];
//            newRow["Category"] = rowView["Категория"];
//            newRow["Price"] = rowView["Цена"];
//            newRow["InStock"] = rowView["В наличии"];
//            newRow["Supplier"] = rowView["Поставщик"];
//            newTable.Rows.Add(newRow);
//        }

//        return newTable;
//    }

//    static DataTable CreateTableWithConvertedPrice(DataView view)
//    {
//        DataTable newTable = view.Table.Clone();

//        // Добавляем колонку для цены в другой валюте
//        newTable.Columns.Add("ЦенаUSD", typeof(decimal));

//        foreach (DataRowView rowView in view)
//        {
//            DataRow newRow = newTable.NewRow();
//            newRow.ItemArray = rowView.Row.ItemArray;

//            // Конвертация цены в доллары (например, курс 0.01)
//            decimal priceInRubles = (decimal)rowView["Цена"];
//            newRow["ЦенаUSD"] = Math.Round(priceInRubles * 0.01m, 2);

//            newTable.Rows.Add(newRow);
//        }

//        return newTable;
//    }

//    static DataTable CreateTableWithCalculatedColumns(DataView view)
//    {
//        DataTable newTable = view.Table.Clone();

//        // Добавляем расчетную колонку для общей стоимости (Цена * В наличии)
//        newTable.Columns.Add("ОбщаяСтоимость", typeof(decimal));

//        foreach (DataRowView rowView in view)
//        {
//            DataRow newRow = newTable.NewRow();
//            newRow.ItemArray = rowView.Row.ItemArray;

//            // Рассчитываем общую стоимость
//            decimal price = (decimal)rowView["Цена"];
//            int inStock = (int)rowView["В наличии"];
//            newRow["ОбщаяСтоимость"] = price * inStock;

//            newTable.Rows.Add(newRow);
//        }

//        return newTable;
//    }

//    static void CompareTables(DataTable originalTable, DataTable newTable)
//    {
//        Console.WriteLine($"Исходная таблица содержит {originalTable.Rows.Count} строк.");
//        Console.WriteLine($"Новая таблица содержит {newTable.Rows.Count} строк.");

//        Console.WriteLine("\nСтруктура исходной таблицы:");
//        foreach (DataColumn column in originalTable.Columns)
//        {
//            Console.WriteLine($"- {column.ColumnName} ({column.DataType.Name})");
//        }

//        Console.WriteLine("\nСтруктура новой таблицы:");
//        foreach (DataColumn column in newTable.Columns)
//        {
//            Console.WriteLine($"- {column.ColumnName} ({column.DataType.Name})");
//        }
//    }

//    static void PrintDataView(DataView view)
//    {
//        if (view.Count == 0)
//        {
//            Console.WriteLine("Нет данных для отображения.");
//            return;
//        }

//        foreach (DataRowView rowView in view)
//        {
//            PrintProductInfo(rowView.Row);
//        }
//    }

//    static void PrintDataTable(DataTable table)
//    {
//        if (table.Rows.Count == 0)
//        {
//            Console.WriteLine("Нет данных для отображения.");
//            return;
//        }

//        foreach (DataRow row in table.Rows)
//        {
//            PrintProductInfo(row);
//        }
//    }

//    static void PrintProductInfo(DataRow row)
//    {
//        Console.WriteLine(
//            $"ProductID: {GetValueOrDefault(row, "ProductID", "N/A")}, " +
//            $"Название: {GetValueOrDefault(row, "Название", "N/A")}, " +
//            $"Категория: {GetValueOrDefault(row, "Категория", "N/A")}, " +
//            $"Цена: {GetValueOrDefault(row, "Цена", "N/A")}, " +
//            $"В наличии: {GetValueOrDefault(row, "В наличии", "N/A")}, " +
//            $"Поставщик: {GetValueOrDefault(row, "Поставщик", "N/A")}, " +
//            $"ЦенаUSD: {GetValueOrDefault(row, "ЦенаUSD", "N/A")}, " +
//            $"ОбщаяСтоимость: {GetValueOrDefault(row, "ОбщаяСтоимость", "N/A")}, " +
//            $"ID: {GetValueOrDefault(row, "ID", "N/A")}, " +
//            $"ProductName: {GetValueOrDefault(row, "ProductName", "N/A")}"
//        );
//    }

//    static string GetValueOrDefault(DataRow row, string columnName, string defaultValue)
//    {
//        if (row.Table.Columns.Contains(columnName))
//        {
//            return row[columnName].ToString();
//        }
//        else
//        {
//            return defaultValue;
//        }
//    }
//}


//12
//using System;
//using System.Data;
//using System.Diagnostics;
//using System.Linq;

//class BankAccountSearchDemo
//{
//    static void Main()
//    {
//        // Создаём и заполняем DataTable "Счета в банке"
//        DataTable accountsTable = CreateAccountsTable();
//        FillTestData(accountsTable);

//        Console.WriteLine("=== КОМПЛЕКСНЫЙ ПОИСК В DATATABLE ===\n");

//        // Поиск по PK с использованием Find()
//        Console.WriteLine("Поиск по PK (AccountID = 1001):");
//        SearchByPrimaryKey(accountsTable, 1001);

//        // Поиск с использованием Select()
//        Console.WriteLine("\nПоиск с использованием Select() (Баланс > 50000, Статус = Активный):");
//        SearchByCriteria(accountsTable, "Balance > 50000 AND Status = 'Активный'");

//        // Поиск с использованием DataView
//        Console.WriteLine("\nПоиск с использованием DataView (Баланс > 50000, Статус = Активный, Тип счёта = Сберегательный):");
//        SearchByDataView(accountsTable, "Balance > 50000 AND Status = 'Активный' AND AccountType = 'Сберегательный'");

//        // Умный поиск
//        Console.WriteLine("\nУмный поиск:");
//        SmartSearch(accountsTable, "AccountID", "1001");
//        SmartSearch(accountsTable, "Balance", "50000");
//        SmartSearch(accountsTable, "Status,Balance,AccountType", "Активный,50000,Сберегательный");

//        // Поиск с автодополнением
//        Console.WriteLine("\nПоиск с автодополнением (по первым буквам имени):");
//        AutoCompleteSearch(accountsTable, "Иван");

//        // Отчёт о найденных счётах
//        Console.WriteLine("\nОтчёт о найденных счётах:");
//        ReportFoundAccounts(accountsTable, "Balance > 50000 AND Status = 'Активный'");

//        // Измерение производительности каждого метода
//        Console.WriteLine("\nИзмерение производительности:");
//        MeasurePerformance(accountsTable);
//    }

//    static DataTable CreateAccountsTable()
//    {
//        DataTable table = new DataTable("Счета в банке");

//        table.Columns.Add("AccountID", typeof(int));
//        table.Columns.Add("CustomerName", typeof(string));
//        table.Columns.Add("Balance", typeof(decimal));
//        table.Columns.Add("AccountType", typeof(string));
//        table.Columns.Add("OpenDate", typeof(DateTime));
//        table.Columns.Add("Status", typeof(string));

//        table.PrimaryKey = new DataColumn[] { table.Columns["AccountID"] };

//        return table;
//    }

//    static void FillTestData(DataTable table)
//    {
//        Random random = new Random();
//        string[] customerNames = { "Иванов Иван", "Петров Петр", "Сидоров Сидор", "Кузнецов Алексей", "Васильев Василий" };
//        string[] accountTypes = { "Сберегательный", "Текущий", "Зарплатный" };
//        string[] statuses = { "Активный", "Замороженный", "Закрытый" };

//        for (int i = 1; i <= 200; i++)
//        {
//            table.Rows.Add(
//                1000 + i,
//                customerNames[random.Next(customerNames.Length)] + " " + i,
//                Math.Round(1000 + random.NextDouble() * 99000, 2),
//                accountTypes[random.Next(accountTypes.Length)],
//                DateTime.Now.AddDays(-random.Next(365 * 5)),
//                statuses[random.Next(statuses.Length)]
//            );
//        }
//    }

//    static void SearchByPrimaryKey(DataTable table, int accountID)
//    {
//        Stopwatch stopwatch = Stopwatch.StartNew();
//        DataRow row = table.Rows.Find(accountID);
//        stopwatch.Stop();

//        if (row != null)
//        {
//            Console.WriteLine($"Найден счёт с AccountID = {accountID}:");
//            PrintAccountInfo(row);
//        }
//        else
//        {
//            Console.WriteLine($"Счёт с AccountID = {accountID} не найден.");
//        }

//        Console.WriteLine($"Время выполнения: {stopwatch.ElapsedTicks} тиков");
//    }

//    static void SearchByCriteria(DataTable table, string criteria)
//    {
//        Stopwatch stopwatch = Stopwatch.StartNew();
//        DataRow[] rows = table.Select(criteria);
//        stopwatch.Stop();

//        Console.WriteLine($"Найдено {rows.Length} счётов по критерию '{criteria}':");
//        foreach (DataRow row in rows.Take(5))
//        {
//            PrintAccountInfo(row);
//        }

//        Console.WriteLine($"Время выполнения: {stopwatch.ElapsedTicks} тиков");
//    }

//    static void SearchByDataView(DataTable table, string criteria)
//    {
//        Stopwatch stopwatch = Stopwatch.StartNew();
//        DataView view = new DataView(table, criteria, "", DataViewRowState.CurrentRows);
//        stopwatch.Stop();

//        Console.WriteLine($"Найдено {view.Count} счётов по критерию '{criteria}':");
//        for (int i = 0; i < Math.Min(5, view.Count); i++)
//        {
//            PrintAccountInfo(view[i].Row);
//        }

//        Console.WriteLine($"Время выполнения: {stopwatch.ElapsedTicks} тиков");
//    }

//    static void SmartSearch(DataTable table, string fields, string values)
//    {
//        Stopwatch stopwatch = Stopwatch.StartNew();
//        string[] fieldList = fields.Split(',');
//        string[] valueList = values.Split(',');

//        DataRow[] resultRows = null;
//        string methodUsed = "";

//        if (fieldList.Length == 1 && fieldList[0] == "AccountID")
//        {
//            int accountID = int.Parse(valueList[0]);
//            DataRow row = table.Rows.Find(accountID);
//            resultRows = row != null ? new DataRow[] { row } : new DataRow[0];
//            methodUsed = "Find()";
//        }
//        else if (fieldList.Length == 1)
//        {
//            string criteria = $"{fieldList[0]} = '{valueList[0]}'";
//            if (fieldList[0] == "Balance")
//            {
//                criteria = $"{fieldList[0]} > {valueList[0]}";
//            }
//            resultRows = table.Select(criteria);
//            methodUsed = "Select()";
//        }
//        else
//        {
//            string criteria = "";
//            for (int i = 0; i < fieldList.Length; i++)
//            {
//                if (i > 0) criteria += " AND ";
//                if (fieldList[i] == "Balance")
//                {
//                    criteria += $"{fieldList[i]} > {valueList[i]}";
//                }
//                else
//                {
//                    criteria += $"{fieldList[i]} = '{valueList[i]}'";
//                }
//            }
//            DataView view = new DataView(table, criteria, "", DataViewRowState.CurrentRows);
//            resultRows = view.Table.Select(criteria);
//            methodUsed = "DataView";
//        }

//        stopwatch.Stop();

//        Console.WriteLine($"Умный поиск с использованием {methodUsed}:");
//        Console.WriteLine($"Найдено {resultRows.Length} счётов:");
//        foreach (DataRow row in resultRows.Take(5))
//        {
//            PrintAccountInfo(row);
//        }

//        Console.WriteLine($"Время выполнения: {stopwatch.ElapsedTicks} тиков");
//    }

//    static void AutoCompleteSearch(DataTable table, string namePrefix)
//    {
//        Stopwatch stopwatch = Stopwatch.StartNew();
//        DataRow[] rows = table.Select($"CustomerName LIKE '{namePrefix}%'");
//        stopwatch.Stop();

//        Console.WriteLine($"Найдено {rows.Length} счётов с именем, начинающимся на '{namePrefix}':");
//        foreach (DataRow row in rows.Take(5))
//        {
//            PrintAccountInfo(row);
//        }

//        Console.WriteLine($"Время выполнения: {stopwatch.ElapsedTicks} тиков");
//    }

//    static void ReportFoundAccounts(DataTable table, string criteria)
//    {
//        DataRow[] rows = table.Select(criteria);

//        Console.WriteLine($"Отчёт о найденных счётах по критерию '{criteria}':");
//        Console.WriteLine($"Всего найдено: {rows.Length} счётов");

//        decimal totalBalance = 0;
//        foreach (DataRow row in rows)
//        {
//            totalBalance += (decimal)row["Balance"];
//        }

//        Console.WriteLine($"Общий баланс: {totalBalance:C}");
//    }

//    static void MeasurePerformance(DataTable table)
//    {
//        int iterations = 1000;
//        Random random = new Random();

//        // Измерение производительности Find()
//        Stopwatch stopwatch = Stopwatch.StartNew();
//        for (int i = 0; i < iterations; i++)
//        {
//            int accountID = 1001 + random.Next(199);
//            DataRow row = table.Rows.Find(accountID);
//        }
//        stopwatch.Stop();
//        Console.WriteLine($"Find(): {stopwatch.ElapsedMilliseconds} мс за {iterations} итераций");

//        // Измерение производительности Select()
//        stopwatch.Restart();
//        for (int i = 0; i < iterations; i++)
//        {
//            string status = random.Next(3) == 0 ? "Активный" : (random.Next(2) == 0 ? "Замороженный" : "Закрытый");
//            DataRow[] rows = table.Select($"Status = '{status}'");
//        }
//        stopwatch.Stop();
//        Console.WriteLine($"Select(): {stopwatch.ElapsedMilliseconds} мс за {iterations} итераций");

//        // Измерение производительности DataView
//        stopwatch.Restart();
//        for (int i = 0; i < iterations; i++)
//        {
//            string status = random.Next(3) == 0 ? "Активный" : (random.Next(2) == 0 ? "Замороженный" : "Закрытый");
//            DataView view = new DataView(table, $"Status = '{status}'", "", DataViewRowState.CurrentRows);
//        }
//        stopwatch.Stop();
//        Console.WriteLine($"DataView: {stopwatch.ElapsedMilliseconds} мс за {iterations} итераций");
//    }

//    static void PrintAccountInfo(DataRow row)
//    {
//        Console.WriteLine(
//            $"AccountID: {row["AccountID"]}, " +
//            $"CustomerName: {row["CustomerName"]}, " +
//            $"Balance: {row["Balance"]:C}, " +
//            $"AccountType: {row["AccountType"]}, " +
//            $"OpenDate: {((DateTime)row["OpenDate"]).ToShortDateString()}, " +
//            $"Status: {row["Status"]}"
//        );
//    }
//}


//13
//using System;
//using System.Data;
//using System.IO;
//using System.Collections.Generic;

//class EventFilteringDemo
//{
//    static void Main()
//    {
//        // Создаём и заполняем DataTable "События"
//        DataTable eventsTable = CreateEventsTable();
//        FillTestData(eventsTable);

//        // Создаём DataView
//        DataView eventsView = new DataView(eventsTable);

//        Console.WriteLine("=== МНОГОУРОВНЕВАЯ ФИЛЬТРАЦИЯ СОБЫТИЙ ===\n");

//        // Уровень 1: Фильтр по категории
//        Console.WriteLine("Уровень 1: Фильтр по категории 'Конференция'");
//        ApplyCategoryFilter(eventsView, "Категория = 'Конференция'");

//        // Уровень 2: Добавление фильтра по приоритету
//        Console.WriteLine("\nУровень 2: Добавление фильтра по приоритету 'Высокий'");
//        ApplyPriorityFilter(eventsView, "Приоритет = 'Высокий'");

//        // Уровень 3: Добавление фильтра по дате
//        Console.WriteLine("\nУровень 3: Добавление фильтра по дате (будущие события)");
//        ApplyDateFilter(eventsView, DateTime.Now);

//        // Уровень 4: Добавление фильтра по количеству участников
//        Console.WriteLine("\nУровень 4: Добавление фильтра по количеству участников (> 50)");
//        ApplyParticipantsFilter(eventsView, 50);

//        // Удаление фильтра по дате
//        Console.WriteLine("\nУдаление фильтра по дате");
//        RemoveDateFilter(eventsView);

//        // Сброс всех фильтров
//        Console.WriteLine("\nСброс всех фильтров");
//        ResetAllFilters(eventsView);

//        // Сохранение конфигурации фильтров
//        Console.WriteLine("\nСохранение конфигурации фильтров");
//        SaveFilterConfiguration(eventsView, "filters.cfg");

//        // Загрузка конфигурации фильтров
//        Console.WriteLine("\nЗагрузка конфигурации фильтров");
//        LoadFilterConfiguration(eventsView, "filters.cfg");
//    }

//    static DataTable CreateEventsTable()
//    {
//        DataTable table = new DataTable("События");

//        table.Columns.Add("EventID", typeof(int));
//        table.Columns.Add("EventName", typeof(string));
//        table.Columns.Add("Дата", typeof(DateTime));
//        table.Columns.Add("Место", typeof(string));
//        table.Columns.Add("Категория", typeof(string));
//        table.Columns.Add("Приоритет", typeof(string));
//        table.Columns.Add("Статус", typeof(string));
//        table.Columns.Add("Участники", typeof(int));

//        table.PrimaryKey = new DataColumn[] { table.Columns["EventID"] };

//        return table;
//    }

//    static void FillTestData(DataTable table)
//    {
//        Random random = new Random();
//        string[] eventNames = { "Конференция по IT", "Выставка технологий", "Семинар по маркетингу", "Встреча партнеров", "Тренинг по продажам" };
//        string[] places = { "Москва", "Санкт-Петербург", "Новосибирск", "Екатеринбург", "Казань" };
//        string[] categories = { "Конференция", "Выставка", "Семинар", "Встреча", "Тренинг" };
//        string[] priorities = { "Низкий", "Средний", "Высокий" };
//        string[] statuses = { "Планируется", "Идет", "Завершено" };

//        for (int i = 1; i <= 300; i++)
//        {
//            table.Rows.Add(
//                i,
//                eventNames[random.Next(eventNames.Length)] + " " + i,
//                DateTime.Now.AddDays(random.Next(-30, 90)),
//                places[random.Next(places.Length)],
//                categories[random.Next(categories.Length)],
//                priorities[random.Next(priorities.Length)],
//                statuses[random.Next(statuses.Length)],
//                random.Next(100)
//            );
//        }
//    }

//    static void ApplyCategoryFilter(DataView view, string category)
//    {
//        view.RowFilter = category;
//        Console.WriteLine($"Применён фильтр по категории. Найдено событий: {view.Count}");
//        PrintDataViewInfo(view);
//    }

//    static void ApplyPriorityFilter(DataView view, string priority)
//    {
//        if (string.IsNullOrEmpty(view.RowFilter))
//        {
//            view.RowFilter = priority;
//        }
//        else
//        {
//            view.RowFilter += $" AND {priority}";
//        }
//        Console.WriteLine($"Добавлен фильтр по приоритету. Найдено событий: {view.Count}");
//        PrintDataViewInfo(view);
//    }

//    static void ApplyDateFilter(DataView view, DateTime currentDate)
//    {
//        string dateFilter = $"Дата > #{currentDate:yyyy-MM-dd}#";
//        if (string.IsNullOrEmpty(view.RowFilter))
//        {
//            view.RowFilter = dateFilter;
//        }
//        else
//        {
//            view.RowFilter += $" AND {dateFilter}";
//        }
//        Console.WriteLine($"Добавлен фильтр по дате. Найдено событий: {view.Count}");
//        PrintDataViewInfo(view);
//    }

//    static void ApplyParticipantsFilter(DataView view, int minParticipants)
//    {
//        string participantsFilter = $"Участники > {minParticipants}";
//        if (string.IsNullOrEmpty(view.RowFilter))
//        {
//            view.RowFilter = participantsFilter;
//        }
//        else
//        {
//            view.RowFilter += $" AND {participantsFilter}";
//        }
//        Console.WriteLine($"Добавлен фильтр по количеству участников. Найдено событий: {view.Count}");
//        PrintDataViewInfo(view);
//    }

//    static void RemoveDateFilter(DataView view)
//    {
//        if (!string.IsNullOrEmpty(view.RowFilter))
//        {
//            string[] filters = view.RowFilter.Split(new[] { " AND " }, StringSplitOptions.None);
//            List<string> newFilters = new List<string>();

//            foreach (string filter in filters)
//            {
//                if (!filter.Contains("Дата > #"))
//                {
//                    newFilters.Add(filter);
//                }
//            }

//            view.RowFilter = string.Join(" AND ", newFilters);
//        }
//        Console.WriteLine($"Фильтр по дате удалён. Найдено событий: {view.Count}");
//        PrintDataViewInfo(view);
//    }

//    static void ResetAllFilters(DataView view)
//    {
//        view.RowFilter = "";
//        Console.WriteLine($"Все фильтры сброшены. Найдено событий: {view.Count}");
//        PrintDataViewInfo(view);
//    }

//    static void SaveFilterConfiguration(DataView view, string filePath)
//    {
//        File.WriteAllText(filePath, view.RowFilter);
//        Console.WriteLine($"Конфигурация фильтров сохранена в файл {filePath}");
//    }

//    static void LoadFilterConfiguration(DataView view, string filePath)
//    {
//        if (File.Exists(filePath))
//        {
//            string filter = File.ReadAllText(filePath);
//            view.RowFilter = filter;
//            Console.WriteLine($"Конфигурация фильтров загружена из файла {filePath}. Найдено событий: {view.Count}");
//            PrintDataViewInfo(view);
//        }
//        else
//        {
//            Console.WriteLine($"Файл {filePath} не найден.");
//        }
//    }

//    static void PrintDataViewInfo(DataView view)
//    {
//        if (view.Count == 0)
//        {
//            Console.WriteLine("Нет данных для отображения.");
//            return;
//        }

//        Console.WriteLine("Первые 5 событий:");
//        for (int i = 0; i < Math.Min(5, view.Count); i++)
//        {
//            DataRowView rowView = view[i];
//            Console.WriteLine(
//                $"EventID: {rowView["EventID"]}, " +
//                $"EventName: {rowView["EventName"]}, " +
//                $"Дата: {((DateTime)rowView["Дата"]).ToShortDateString()}, " +
//                $"Категория: {rowView["Категория"]}, " +
//                $"Приоритет: {rowView["Приоритет"]}, " +
//                $"Участники: {rowView["Участники"]}"
//            );
//        }
//    }
//}


////14
//using System;
//using System.Data;
//using System.Diagnostics;
//using System.Collections.Generic;

//class SalesSortingDemo
//{
//    static void Main()
//    {
//        // Создаём и заполняем DataTable "Продажи сотрудников"
//        DataTable salesTable = CreateSalesTable();
//        FillTestData(salesTable);

//        // Создаём DataView
//        DataView salesView = new DataView(salesTable);

//        Console.WriteLine("=== МНОГОУРОВНЕВАЯ СОРТИРОВКА В DATAVIEW ===\n");

//        // Сортировка по одной колонке (Сумма DESC)
//        Console.WriteLine("Сортировка по одной колонке (Сумма DESC):");
//        SortBySingleColumn(salesView, "Сумма DESC");

//        // Сортировка по соседним колонкам (Отдел ASC, Сумма DESC)
//        Console.WriteLine("\nСортировка по соседним колонкам (Отдел ASC, Сумма DESC):");
//        SortByMultipleColumns(salesView, "Отдел ASC, Сумма DESC");

//        // Сортировка по трём колонкам (Регион ASC, Отдел ASC, Дата DESC)
//        Console.WriteLine("\nСортировка по трём колонкам (Регион ASC, Отдел ASC, Дата DESC):");
//        SortByThreeColumns(salesView, "Регион ASC, Отдел ASC, Дата DESC");

//        // Динамическая сортировка на основе пользовательского выбора
//        Console.WriteLine("\nДинамическая сортировка на основе пользовательского выбора:");
//        DynamicSorting(salesView);

//        // Переключение между возрастанием и убыванием по одной колонке
//        Console.WriteLine("\nПереключение между возрастанием и убыванием по колонке 'Сумма':");
//        ToggleSortDirection(salesView, "Сумма");

//        // Сравнение сортировки в DataView с сортировкой через Select()
//        Console.WriteLine("\nСравнение сортировки в DataView с сортировкой через Select():");
//        CompareSortingPerformance(salesTable);
//    }

//    static DataTable CreateSalesTable()
//    {
//        DataTable table = new DataTable("Продажи сотрудников");

//        table.Columns.Add("SalesID", typeof(int));
//        table.Columns.Add("Имя Сотрудника", typeof(string));
//        table.Columns.Add("Отдел", typeof(string));
//        table.Columns.Add("Сумма", typeof(decimal));
//        table.Columns.Add("Дата", typeof(DateTime));
//        table.Columns.Add("Квартал", typeof(int));
//        table.Columns.Add("Регион", typeof(string));

//        table.PrimaryKey = new DataColumn[] { table.Columns["SalesID"] };

//        return table;
//    }

//    static void FillTestData(DataTable table)
//    {
//        Random random = new Random();
//        string[] names = { "Иванов", "Петров", "Сидоров", "Кузнецов", "Васильев", "Смирнов", "Новиков", "Федоров", "Морозов", "Волков" };
//        string[] departments = { "Продажи", "Маркетинг", "IT", "Финансы", "HR" };
//        string[] regions = { "Москва", "Санкт-Петербург", "Новосибирск", "Екатеринбург", "Казань" };

//        for (int i = 1; i <= 500; i++)
//        {
//            table.Rows.Add(
//                i,
//                $"{names[random.Next(names.Length)]} {names[random.Next(names.Length)]}",
//                departments[random.Next(departments.Length)],
//                Math.Round(1000 + random.NextDouble() * 99000, 2),
//                DateTime.Now.AddDays(-random.Next(365)),
//                random.Next(1, 4),
//                regions[random.Next(regions.Length)]
//            );
//        }
//    }

//    static void SortBySingleColumn(DataView view, string sortExpression)
//    {
//        view.Sort = sortExpression;
//        Console.WriteLine($"Применена сортировка: {sortExpression}");
//        PrintDataViewInfo(view);
//    }

//    static void SortByMultipleColumns(DataView view, string sortExpression)
//    {
//        view.Sort = sortExpression;
//        Console.WriteLine($"Применена сортировка: {sortExpression}");
//        PrintDataViewInfo(view);
//    }

//    static void SortByThreeColumns(DataView view, string sortExpression)
//    {
//        view.Sort = sortExpression;
//        Console.WriteLine($"Применена сортировка: {sortExpression}");
//        PrintDataViewInfo(view);
//    }

//    static void DynamicSorting(DataView view)
//    {
//        Console.WriteLine("Выберите колонку для сортировки:");
//        Console.WriteLine("1. Имя Сотрудника");
//        Console.WriteLine("2. Отдел");
//        Console.WriteLine("3. Сумма");
//        Console.WriteLine("4. Дата");
//        Console.WriteLine("5. Квартал");
//        Console.WriteLine("6. Регион");

//        Console.Write("Введите номер колонки: ");
//        int columnChoice = int.Parse(Console.ReadLine());

//        Console.WriteLine("Выберите направление сортировки:");
//        Console.WriteLine("1. По возрастанию (ASC)");
//        Console.WriteLine("2. По убыванию (DESC)");

//        Console.Write("Введите номер направления: ");
//        int directionChoice = int.Parse(Console.ReadLine());

//        string columnName = "";
//        switch (columnChoice)
//        {
//            case 1:
//                columnName = "Имя Сотрудника";
//                break;
//            case 2:
//                columnName = "Отдел";
//                break;
//            case 3:
//                columnName = "Сумма";
//                break;
//            case 4:
//                columnName = "Дата";
//                break;
//            case 5:
//                columnName = "Квартал";
//                break;
//            case 6:
//                columnName = "Регион";
//                break;
//            default:
//                Console.WriteLine("Некорректный выбор.");
//                return;
//        }

//        string direction = directionChoice == 1 ? "ASC" : "DESC";
//        view.Sort = $"{columnName} {direction}";
//        Console.WriteLine($"Применена динамическая сортировка: {columnName} {direction}");
//        PrintDataViewInfo(view);
//    }

//    static void ToggleSortDirection(DataView view, string columnName)
//    {
//        if (view.Sort.Contains(columnName))
//        {
//            if (view.Sort.Contains("DESC"))
//            {
//                view.Sort = view.Sort.Replace("DESC", "ASC");
//                Console.WriteLine($"Сортировка изменена на {columnName} ASC");
//            }
//            else if (view.Sort.Contains("ASC"))
//            {
//                view.Sort = view.Sort.Replace("ASC", "DESC");
//                Console.WriteLine($"Сортировка изменена на {columnName} DESC");
//            }
//            else
//            {
//                view.Sort = $"{columnName} DESC";
//                Console.WriteLine($"Сортировка установлена на {columnName} DESC");
//            }
//        }
//        else
//        {
//            view.Sort = $"{columnName} DESC";
//            Console.WriteLine($"Сортировка установлена на {columnName} DESC");
//        }
//        PrintDataViewInfo(view);
//    }

//    static void CompareSortingPerformance(DataTable table)
//    {
//        int iterations = 1000;
//        Random random = new Random();

//        // Измерение производительности сортировки в DataView
//        Stopwatch stopwatch = Stopwatch.StartNew();
//        for (int i = 0; i < iterations; i++)
//        {
//            DataView view = new DataView(table, "", "Сумма DESC", DataViewRowState.CurrentRows);
//        }
//        stopwatch.Stop();
//        Console.WriteLine($"Сортировка в DataView: {stopwatch.ElapsedMilliseconds} мс за {iterations} итераций");

//        // Измерение производительности сортировки через Select()
//        stopwatch.Restart();
//        for (int i = 0; i < iterations; i++)
//        {
//            DataRow[] rows = table.Select("", "Сумма DESC");
//        }
//        stopwatch.Stop();
//        Console.WriteLine($"Сортировка через Select(): {stopwatch.ElapsedMilliseconds} мс за {iterations} итераций");
//    }

//    static void PrintDataViewInfo(DataView view)
//    {
//        if (view.Count == 0)
//        {
//            Console.WriteLine("Нет данных для отображения.");
//            return;
//        }

//        Console.WriteLine($"Найдено записей: {view.Count}");
//        Console.WriteLine("Первые 5 записей:");
//        for (int i = 0; i < Math.Min(5, view.Count); i++)
//        {
//            DataRowView rowView = view[i];
//            Console.WriteLine(
//                $"SalesID: {rowView["SalesID"]}, " +
//                $"Имя Сотрудника: {rowView["Имя Сотрудника"]}, " +
//                $"Отдел: {rowView["Отдел"]}, " +
//                $"Сумма: {rowView["Сумма"]:C}, " +
//                $"Дата: {((DateTime)rowView["Дата"]).ToShortDateString()}, " +
//                $"Регион: {rowView["Регион"]}"
//            );
//        }
//    }
//}


//15
//using System;
//using System.Data;
//using System.Diagnostics;
//using System.Linq;

//class StudentDataViewDemo
//{
//    static void Main()
//    {
//        // Создаём и заполняем DataTable "Студенты"
//        DataTable studentsTable = CreateStudentsTable();
//        FillTestData(studentsTable);

//        Console.WriteLine("=== РАБОТА С НЕСКОЛЬКИМИ DATAVIEW ===\n");

//        // Создаём несколько DataView
//        DataView topStudentsView = new DataView(studentsTable, "СреднийБалл > 4.0", "", DataViewRowState.CurrentRows);
//        DataView firstYearStudentsView = new DataView(studentsTable, "Год = 1", "", DataViewRowState.CurrentRows);
//        DataView academicLeaveStudentsView = new DataView(studentsTable, "Статус = 'Академический отпуск'", "", DataViewRowState.CurrentRows);
//        DataView allStudentsByFacultyView = new DataView(studentsTable, "", "Факультет ASC, Имя ASC", DataViewRowState.CurrentRows);

//        // Анализ каждого DataView
//        Console.WriteLine("Анализ DataView 1: Лучшие студенты (GPA > 4.0)");
//        AnalyzeDataView(topStudentsView);

//        Console.WriteLine("\nАнализ DataView 2: Студенты первого курса");
//        AnalyzeDataView(firstYearStudentsView);

//        Console.WriteLine("\nАнализ DataView 3: Студенты в академическом отпуске");
//        AnalyzeDataView(academicLeaveStudentsView);

//        Console.WriteLine("\nАнализ DataView 4: Все студенты, отсортированные по факультетам");
//        AnalyzeDataView(allStudentsByFacultyView);

//        // Поиск студента одновременно во всех DataView
//        Console.WriteLine("\nПоиск студента одновременно во всех DataView (StudentID = 1001):");
//        SearchStudentInAllViews(new DataView[] { topStudentsView, firstYearStudentsView, academicLeaveStudentsView, allStudentsByFacultyView }, 1001);

//        // Добавление нового студента
//        Console.WriteLine("\nДобавление нового студента:");
//        AddNewStudent(studentsTable, 201, "Новый Студент", 4.5m, "Информатика", 1, "Активен");

//        // Проверка влияния на все DataView
//        Console.WriteLine("\nПроверка влияния на все DataView после добавления нового студента:");
//        AnalyzeDataView(topStudentsView);
//        AnalyzeDataView(firstYearStudentsView);
//        AnalyzeDataView(academicLeaveStudentsView);
//        AnalyzeDataView(allStudentsByFacultyView);

//        // Отчёт о всех представлениях
//        Console.WriteLine("\nОтчёт о всех представлениях:");
//        ReportAllViews(new DataView[] { topStudentsView, firstYearStudentsView, academicLeaveStudentsView, allStudentsByFacultyView });

//        // Измерение использования памяти
//        Console.WriteLine("\nИзмерение использования памяти:");
//        MeasureMemoryUsage(studentsTable, new DataView[] { topStudentsView, firstYearStudentsView, academicLeaveStudentsView, allStudentsByFacultyView });
//    }

//    static DataTable CreateStudentsTable()
//    {
//        DataTable table = new DataTable("Студенты");

//        table.Columns.Add("StudentID", typeof(int));
//        table.Columns.Add("Имя", typeof(string));
//        table.Columns.Add("СреднийБалл", typeof(decimal));
//        table.Columns.Add("Факультет", typeof(string));
//        table.Columns.Add("Год", typeof(int));
//        table.Columns.Add("Статус", typeof(string));

//        table.PrimaryKey = new DataColumn[] { table.Columns["StudentID"] };

//        return table;
//    }

//    static void FillTestData(DataTable table)
//    {
//        Random random = new Random();
//        string[] names = { "Иванов", "Петров", "Сидоров", "Кузнецов", "Васильев", "Смирнов", "Новиков", "Федоров", "Морозов", "Волков" };
//        string[] faculties = { "Информатика", "Математика", "Физика", "Химия", "Биология" };
//        string[] statuses = { "Активен", "Академический отпуск", "Отчислен" };

//        for (int i = 1; i <= 200; i++)
//        {
//            table.Rows.Add(
//                1000 + i,
//                $"{names[random.Next(names.Length)]} {names[random.Next(names.Length)]}",
//                Math.Round(2.0 + random.NextDouble() * 3, 2),
//                faculties[random.Next(faculties.Length)],
//                random.Next(1, 5),
//                statuses[random.Next(statuses.Length)]
//            );
//        }
//    }

//    static void AnalyzeDataView(DataView view)
//    {
//        Console.WriteLine($"Количество студентов: {view.Count}");

//        if (view.Count > 0)
//        {
//            Console.WriteLine("Первые 5 студентов:");
//            for (int i = 0; i < Math.Min(5, view.Count); i++)
//            {
//                DataRowView rowView = view[i];
//                Console.WriteLine(
//                    $"StudentID: {rowView["StudentID"]}, " +
//                    $"Имя: {rowView["Имя"]}, " +
//                    $"СреднийБалл: {rowView["СреднийБалл"]}, " +
//                    $"Факультет: {rowView["Факультет"]}, " +
//                    $"Год: {rowView["Год"]}, " +
//                    $"Статус: {rowView["Статус"]}"
//                );
//            }

//            decimal totalGPA = 0;
//            foreach (DataRowView rowView in view)
//            {
//                totalGPA += Convert.ToDecimal(rowView["СреднийБалл"]);
//            }
//            decimal averageGPA = totalGPA / view.Count;
//            Console.WriteLine($"Среднее GPA: {averageGPA:F2}");
//        }
//        else
//        {
//            Console.WriteLine("Нет данных для отображения.");
//        }
//    }

//    static void SearchStudentInAllViews(DataView[] views, int studentID)
//    {
//        foreach (DataView view in views)
//        {
//            bool found = false;
//            foreach (DataRowView rowView in view)
//            {
//                if (Convert.ToInt32(rowView["StudentID"]) == studentID)
//                {
//                    Console.WriteLine($"Студент найден в представлении с фильтром '{view.RowFilter}'");
//                    Console.WriteLine(
//                        $"StudentID: {rowView["StudentID"]}, " +
//                        $"Имя: {rowView["Имя"]}, " +
//                        $"СреднийБалл: {rowView["СреднийБалл"]}, " +
//                        $"Факультет: {rowView["Факультет"]}"
//                    );
//                    found = true;
//                    break;
//                }
//            }
//            if (!found)
//            {
//                Console.WriteLine($"Студент не найден в представлении с фильтром '{view.RowFilter}'");
//            }
//        }
//    }

//    static void AddNewStudent(DataTable table, int studentID, string name, decimal gpa, string faculty, int year, string status)
//    {
//        DataRow newRow = table.NewRow();
//        newRow["StudentID"] = studentID;
//        newRow["Имя"] = name;
//        newRow["СреднийБалл"] = gpa;
//        newRow["Факультет"] = faculty;
//        newRow["Год"] = year;
//        newRow["Статус"] = status;
//        table.Rows.Add(newRow);

//        Console.WriteLine($"Студент {name} добавлен.");
//    }

//    static void ReportAllViews(DataView[] views)
//    {
//        foreach (DataView view in views)
//        {
//            Console.WriteLine($"\nФильтр: {view.RowFilter}, Сортировка: {view.Sort}");
//            Console.WriteLine($"Количество студентов: {view.Count}");
//        }
//    }

//    static void MeasureMemoryUsage(DataTable table, DataView[] views)
//    {
//        long beforeMemory = GC.GetTotalMemory(true);
//        Console.WriteLine($"Память до создания DataView: {beforeMemory / 1024} KB");

//        long afterMemory = GC.GetTotalMemory(true);
//        Console.WriteLine($"Память после создания DataView: {afterMemory / 1024} KB");

//        Console.WriteLine($"Использование памяти для DataView: {(afterMemory - beforeMemory) / 1024} KB");
//    }
//}


//15
//using System;
//using System.Data;

//class Program
//{
//    static void Main()
//    {
//        DataTable dt = new DataTable("Студенты");
//        dt.Columns.Add("StudentID", typeof(int));
//        dt.Columns.Add("Name", typeof(string));
//        dt.Columns.Add("GPA", typeof(double));
//        dt.Columns.Add("Faculty", typeof(string));
//        dt.Columns.Add("Year", typeof(int));
//        dt.Columns.Add("Status", typeof(string));

//        string[] faculties = { "Математический", "Физический", "Биологический", "Химический", "Информатика" };
//        string[] statuses = { "Активен", "Академический отпуск", "Отчислен" };
//        Random rnd = new Random();

//        for (int i = 1; i <= 200; i++)
//        {
//            dt.Rows.Add(i, $"Студент{i}", Math.Round(2.0 + rnd.NextDouble() * 3.0, 2),
//                faculties[rnd.Next(faculties.Length)], rnd.Next(1, 6), statuses[rnd.Next(statuses.Length)]);
//        }

//        long memBefore = GC.GetTotalMemory(true);

//        DataView dv1 = new DataView(dt) { RowFilter = "GPA > 4.0" };
//        DataView dv2 = new DataView(dt) { RowFilter = "Year = 1" };
//        DataView dv3 = new DataView(dt) { RowFilter = "Status = 'Академический отпуск'" };
//        DataView dv4 = new DataView(dt) { Sort = "Faculty" };

//        long memAfter = GC.GetTotalMemory(true);
//        Console.WriteLine($"Потребление памяти на 4 DataView: {(memAfter - memBefore)} байт\n");

//        DataView[] views = { dv1, dv2, dv3, dv4 };
//        string[] names = { "Лучшие студенты (GPA > 4.0)", "Студенты 1-го курса", "Студенты в академическом отпуске", "Все студенты (отсортированы по факультету)" };

//        Console.WriteLine("=== Начальный отчёт ===\n");
//        for (int i = 0; i < views.Length; i++)
//        {
//            DataView dv = views[i];
//            string filter = i == 3 ? "" : dv.RowFilter;
//            int count = dv.Count;
//            double avg = filter == "" ? (double)dt.Compute("AVG(GPA)", null) : (double)dt.Compute("AVG(GPA)", filter);

//            Console.WriteLine($"Представление: {names[i]}");
//            Console.WriteLine($"Количество студентов: {count}");
//            Console.WriteLine($"Средний GPA: {avg:F2}\n");
//            Console.WriteLine("Список студентов:");
//            foreach (DataRowView row in dv)
//                Console.WriteLine($"{row["StudentID"]}\t{row["Name"]}\t{row["GPA"]}\t{row["Faculty"]}\t{row["Year"]}\t{row["Status"]}");
//            Console.WriteLine(new string('-', 50));
//        }

//        Console.WriteLine("\n=== Поиск студента 'Студент100' во всех представлениях ===");
//        string search = "Студент100";
//        for (int i = 0; i < views.Length; i++)
//        {
//            bool found = false;
//            foreach (DataRowView row in views[i])
//                if (row["Name"].ToString() == search) { found = true; break; }
//            Console.WriteLine($"В представлении \"{names[i]}\": {(found ? "найден" : "не найден")}");
//        }

//        Console.WriteLine("\n=== Добавляем нового студента ===");
//        dt.Rows.Add(201, "НовыйСтудент", 4.8, "Информатика", 1, "Активен");

//        Console.WriteLine("Количество студентов после добавления:");
//        for (int i = 0; i < views.Length; i++)
//            Console.WriteLine($"{names[i]} → {views[i].Count} студентов");

//        Console.WriteLine("\n=== Обновлённый отчёт (после добавления) ===");
//        for (int i = 0; i < views.Length; i++)
//        {
//            DataView dv = views[i];
//            string filter = i == 3 ? "" : dv.RowFilter;
//            int count = dv.Count;
//            double avg = filter == "" ? (double)dt.Compute("AVG(GPA)", null) : (double)dt.Compute("AVG(GPA)", filter);

//            Console.WriteLine($"\nПредставление: {names[i]}");
//            Console.WriteLine($"Количество: {count}");
//            Console.WriteLine($"Средний GPA: {avg:F2}");
//        }
//    }
//}

//16
//using System;
//using System.Data;
//using System.Linq;

//class Program
//{
//    static DataTable dt = new DataTable("Фильмы");
//    static DataView dv;

//    static string текущийФильтр = "";
//    static string текущаяСортировка = "Year DESC";

//    static void Main()
//    {
//        СоздатьТаблицуИДанные();
//        dv = new DataView(dt);

//        Console.WriteLine("=== Динамический фильтр фильмов ===\n");
//        Console.WriteLine("Доступные команды:");
//        Console.WriteLine("  g <жанр>     — фильтр по жанру (или g all — снять)");
//        Console.WriteLine("  t <текст>    — поиск по названию (или t — снять)");
//        Console.WriteLine("  r <мин> <макс> — диапазон рейтинга (4.0–10.0)");
//        Console.WriteLine("  s year | rating | box — сортировка");
//        Console.WriteLine("  reset        — сброс всех фильтров");
//        Console.WriteLine("  show         — показать текущие результаты");
//        Console.WriteLine("  count        — показать количество");
//        Console.WriteLine("  exit         — выход\n");

//        ПрименитьФильтр();
//        ПоказатьКоличество();

//        while (true)
//        {
//            Console.Write("\n> ");
//            string input = Console.ReadLine().Trim();
//            if (string.IsNullOrEmpty(input)) continue;

//            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
//            string cmd = parts[0].ToLower();

//            try
//            {
//                switch (cmd)
//                {
//                    case "g":
//                        if (parts.Length == 1) { Console.WriteLine("Укажите жанр или 'all'"); break; }
//                        string жанр = string.Join(" ", parts.Skip(1));
//                        if (жанр.Equals("all", StringComparison.OrdinalIgnoreCase))
//                            текущийФильтр = текущийФильтр.Replace($"Genre = '{текущийФильтр.Split('\'')[1]}'", "").Trim();
//                        else
//                            текущийФильтр = ДобавитьУсловие(текущийФильтр, $"Genre = '{жанр}'");
//                        ПрименитьФильтр();
//                        break;

//                    case "t":
//                        string текст = parts.Length > 1 ? string.Join(" ", parts.Skip(1)) : "";
//                        текущийФильтр = УбратьУсловие(текущийФильтр, "Title LIKE");
//                        if (!string.IsNullOrEmpty(текст))
//                            текущийФильтр = ДобавитьУсловие(текущийФильтр, $"Title LIKE '%{текст.Replace("'", "''")}%'");
//                        ПрименитьФильтр();
//                        break;

//                    case "r":
//                        if (parts.Length != 3) { Console.WriteLine("Использование: r 4.5 9.0"); break; }
//                        double min = double.Parse(parts[1]);
//                        double max = double.Parse(parts[2]);
//                        текущийФильтр = УбратьУсловие(текущийФильтр, "Rating >=");
//                        текущийФильтр = ДобавитьУсловие(текущийФильтр, $"Rating >= {min} AND Rating <= {max}");
//                        ПрименитьФильтр();
//                        break;

//                    case "s":
//                        if (parts.Length != 2) { Console.WriteLine("s year | rating | box"); break; }
//                        switch (parts[1].ToLower())
//                        {
//                            case "year": текущаяСортировка = "Year DESC"; break;
//                            case "rating": текущаяСортировка = "Rating DESC"; break;
//                            case "box": текущаяСортировка = "BoxOffice DESC"; break;
//                            default: Console.WriteLine("Неизвестный тип сортировки"); break;
//                        }
//                        ПрименитьФильтр();
//                        break;

//                    case "reset":
//                        текущийФильтр = "";
//                        текущаяСортировка = "Year DESC";
//                        ПрименитьФильтр();
//                        Console.WriteLine("Все фильтры сброшены");
//                        break;

//                    case "show":
//                        ПоказатьРезультаты();
//                        break;

//                    case "count":
//                        ПоказатьКоличество();
//                        break;

//                    case "exit":
//                        Console.WriteLine($"Последний фильтр сохранён:\nФильтр: {dv.RowFilter}\nСортировка: {dv.Sort}");
//                        return;

//                    default:
//                        Console.WriteLine("Неизвестная команда");
//                        break;
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Ошибка: " + ex.Message);
//            }
//        }
//    }

//    static void СоздатьТаблицуИДанные()
//    {
//        dt.Columns.Add("MovieID", typeof(int));
//        dt.Columns.Add("Title", typeof(string));
//        dt.Columns.Add("Director", typeof(string));
//        dt.Columns.Add("Year", typeof(int));
//        dt.Columns.Add("Genre", typeof(string));
//        dt.Columns.Add("Rating", typeof(double));
//        dt.Columns.Add("BoxOffice", typeof(decimal));

//        string[] жанры = { "Боевик", "Комедия", "Драма", "Фантастика", "Ужасы", "Триллер", "Мелодрама", "Документальный" };
//        string[] режиссеры = { "Нолан", "Спилберг", "Тарантино", "Кэмерон", "Скорсезе", "Финчер", "Вильнёв" };
//        Random rnd = new Random();

//        for (int i = 1; i <= 300; i++)
//        {
//            string жанр = жанры[rnd.Next(жанры.Length)];
//            dt.Rows.Add(
//                i,
//                $"Фильм {i}: {жанр}",
//                режиссеры[rnd.Next(режиссеры.Length)],
//                1980 + rnd.Next(45),
//                жанр,
//                Math.Round(4.0 + rnd.NextDouble() * 6.0, 1),
//                rnd.Next(10, 2000) * 1000000M
//            );
//        }
//    }

//    static string ДобавитьУсловие(string текущий, string новое)
//    {
//        if (string.IsNullOrEmpty(текущий) || текущий.Trim() == "1=1")
//            return новое;
//        return $"{текущий} AND {новое}";
//    }

//    static string УбратьУсловие(string фильтр, string ключевое)
//    {
//        if (string.IsNullOrEmpty(фильтр)) return "";
//        var части = фильтр.Split(new[] { " AND " }, StringSplitOptions.None)
//            .Where(p => !p.Contains(ключевое)).ToArray();
//        return части.Length == 0 ? "" : string.Join(" AND ", части);
//    }

//    static void ПрименитьФильтр()
//    {
//        string filter = string.IsNullOrEmpty(текущийФильтр) ? "1=1" : текущийФильтр;
//        dv.RowFilter = filter;
//        dv.Sort = текущаяСортировка;
//        ПоказатьКоличество();
//    }

//    static void ПоказатьКоличество()
//    {
//        Console.WriteLine($"\nНайдено фильмов: {dv.Count} из {dt.Rows.Count}");
//        Console.WriteLine($"Текущий фильтр: {(dv.RowFilter == "1=1" ? "нет" : dv.RowFilter)}");
//        Console.WriteLine($"Сортировка: {dv.Sort}");
//    }

//    static void ПоказатьРезультаты()
//    {
//        Console.WriteLine("\n" + new string('=', 80));
//        Console.WriteLine($"{"ID",-4} {"Название",-35} {"Режиссёр",-15} {"Год",-5} {"Жанр",-12} {"Рейтинг",-7} {"Касса, млн$",-10}");
//        Console.WriteLine(new string('-', 80));

//        int показывать = Math.Min(20, dv.Count);
//        for (int i = 0; i < показывать; i++)
//        {
//            var row = dv[i].Row;
//            Console.WriteLine($"{row["MovieID"],-4} {row["Title"].ToString().Truncate(34),-35} " +
//                $"{row["Director"].ToString().Truncate(14),-15} {row["Year"],-5} " +
//                $"{row["Genre"].ToString().Truncate(11),-12} {row["Rating"],-7} {(decimal)row["BoxOffice"] / 1000000M,-10:F0}");
//        }

//        if (dv.Count > 20)
//            Console.WriteLine($"... и ещё {dv.Count - 20} фильмов (всего {dv.Count})");
//        Console.WriteLine(new string('=', 80));
//    }
//}

//static class Ext { public static string Truncate(this string s, int len) => s.Length > len ? s.Substring(0, len - 1) + "…" : s; }

//using System.Data;

//17
//using System;
//using System.Data;
//using System.Linq;
//using System.Collections.Generic;
//using System.Text;

//class Program
//{
//    static DataTable dt = new DataTable("Продажи по месяцам");
//    static DataView dvRegions, dvProducts, dvMonths;
//    static string currentFilter = "";

//    static void Main()
//    {
//        СоздатьТаблицуИДанные();
//        СоздатьПредставления();

//        Console.WriteLine("=== Визуализация продаж ===\n");
//        Console.WriteLine("Доступные команды:");
//        Console.WriteLine("  filter region <регион>  — фильтр по региону (или region all — снять)");
//        Console.WriteLine("  filter product <продукт> — фильтр по продукту (или product all — снять)");
//        Console.WriteLine("  show months             — график продаж по месяцам");
//        Console.WriteLine("  show regions            — диаграмма продаж по регионам");
//        Console.WriteLine("  show products           — диаграмма продаж по продуктам");
//        Console.WriteLine("  totals                  — итоговые значения");
//        Console.WriteLine("  reset                   — сброс фильтров");
//        Console.WriteLine("  exit                    — выход\n");

//        ОбновитьПредставления();
//        ПоказатьИтоговые();

//        while (true)
//        {
//            Console.Write("\n> ");
//            string input = Console.ReadLine().Trim();
//            if (string.IsNullOrEmpty(input)) continue;

//            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
//            string cmd = parts[0].ToLower();

//            try
//            {
//                if (cmd == "filter" && parts.Length >= 3)
//                {
//                    string type = parts[1].ToLower();
//                    string value = string.Join(" ", parts.Skip(2));
//                    if (type == "region")
//                    {
//                        currentFilter = УбратьУсловие(currentFilter, "Region =");
//                        if (!value.Equals("all", StringComparison.OrdinalIgnoreCase))
//                            currentFilter = ДобавитьУсловие(currentFilter, $"Region = '{value.Replace("'", "''")}'");
//                    }
//                    else if (type == "product")
//                    {
//                        currentFilter = УбратьУсловие(currentFilter, "Product =");
//                        if (!value.Equals("all", StringComparison.OrdinalIgnoreCase))
//                            currentFilter = ДобавитьУсловие(currentFilter, $"Product = '{value.Replace("'", "''")}'");
//                    }
//                    ОбновитьПредставления();
//                    Console.WriteLine($"Фильтр применён: {(string.IsNullOrEmpty(currentFilter) ? "нет" : currentFilter)}");
//                }
//                else if (cmd == "show")
//                {
//                    if (parts.Length < 2) { Console.WriteLine("Укажите: months, regions или products"); continue; }
//                    string chartType = parts[1].ToLower();
//                    switch (chartType)
//                    {
//                        case "months": ПоказатьГрафикПоМесяцам(); break;
//                        case "regions": ПоказатьДиаграммуПоРегионам(); break;
//                        case "products": ПоказатьДиаграммуПоПродуктам(); break;
//                        default: Console.WriteLine("Неизвестный тип: months, regions или products"); break;
//                    }
//                }
//                else if (cmd == "totals")
//                {
//                    ПоказатьИтоговые();
//                }
//                else if (cmd == "reset")
//                {
//                    currentFilter = "";
//                    ОбновитьПредставления();
//                    Console.WriteLine("Фильтры сброшены");
//                }
//                else if (cmd == "exit")
//                {
//                    Console.WriteLine($"Последний фильтр: {currentFilter}");
//                    return;
//                }
//                else
//                {
//                    Console.WriteLine("Неизвестная команда");
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Ошибка: " + ex.Message);
//            }
//        }
//    }

//    static void СоздатьТаблицуИДанные()
//    {
//        dt.Columns.Add("SalesID", typeof(int));
//        dt.Columns.Add("Month", typeof(string));
//        dt.Columns.Add("Product", typeof(string));
//        dt.Columns.Add("Amount", typeof(decimal));
//        dt.Columns.Add("Region", typeof(string));

//        string[] месяцы = { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };
//        string[] продукты = { "Товар A", "Товар B", "Товар C", "Товар D" };
//        string[] регионы = { "Север", "Юг", "Восток", "Запад" };
//        Random rnd = new Random();

//        for (int i = 1; i <= 120; i++) // 10 записей на месяц
//        {
//            string месяц = месяцы[(i - 1) % 12];
//            dt.Rows.Add(i, месяц, продукты[rnd.Next(продукты.Length)], (decimal)rnd.Next(100, 5000), регионы[rnd.Next(регионы.Length)]);
//        }
//    }

//    static void СоздатьПредставления()
//    {
//        dvRegions = new DataView(dt) { Sort = "Region" };
//        dvProducts = new DataView(dt) { Sort = "Product" };
//        dvMonths = new DataView(dt) { Sort = "Month" };
//    }

//    static void ОбновитьПредставления()
//    {
//        string filter = string.IsNullOrEmpty(currentFilter) ? null : currentFilter;
//        dvRegions.RowFilter = filter;
//        dvProducts.RowFilter = filter;
//        dvMonths.RowFilter = filter;
//    }

//    static string ДобавитьУсловие(string текущий, string новое)
//    {
//        return string.IsNullOrEmpty(текущий) ? новое : $"{текущий} AND {новое}";
//    }

//    static string УбратьУсловие(string фильтр, string ключевое)
//    {
//        if (string.IsNullOrEmpty(фильтр)) return "";
//        var части = фильтр.Split(new[] { " AND " }, StringSplitOptions.None)
//            .Where(p => !p.StartsWith(ключевое)).ToArray();
//        return string.Join(" AND ", части);
//    }

//    static void ПоказатьИтоговые()
//    {
//        decimal total = dvMonths.Table.Compute("SUM(Amount)", dvMonths.RowFilter) is DBNull ? 0 : (decimal)dvMonths.Table.Compute("SUM(Amount)", dvMonths.RowFilter);
//        Console.WriteLine($"\nИтоговые значения:");
//        Console.WriteLine($"Общие продажи: {total:C}");
//        Console.WriteLine($"По регионам: {dvRegions.Count} записей");
//        Console.WriteLine($"По продуктам: {dvProducts.Count} записей");
//        Console.WriteLine($"По месяцам: {dvMonths.Count} записей");
//    }

//    static void ПоказатьГрафикПоМесяцам()
//    {
//        var data = dvMonths.Table.AsEnumerable()
//            .Where(r => dvMonths.RowFilter == null || dvMonths.Table.Select(dvMonths.RowFilter).Contains(r))
//            .GroupBy(r => r["Month"])
//            .Select(g => new { Month = g.Key, Sum = g.Sum(r => (decimal)r["Amount"]) })
//            .OrderBy(x => Array.IndexOf(new[] { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" }, x.Month));

//        decimal max = data.Max(x => x.Sum);
//        Console.WriteLine("\nГрафик продаж по месяцам (линейный, ASCII):");
//        Console.WriteLine("Месяц     | Сумма          | График (масштаб: * = 100 руб)");
//        Console.WriteLine(new string('-', 50));

//        foreach (var item in data)
//        {
//            int bars = (int)(item.Sum / 100);
//            Console.WriteLine($"{item.Month,-10}| {item.Sum,-14:C} | {new string('*', bars)}");
//        }

//        Console.WriteLine("\nЛегенда: * представляет 100 руб. Ось X: месяцы, Ось Y: сумма продаж.");
//    }

//    static void ПоказатьДиаграммуПоРегионам()
//    {
//        var data = dvRegions.ToTable(true, "Region", "Amount").AsEnumerable()
//            .GroupBy(r => r["Region"])
//            .Select(g => new { Region = g.Key, Sum = g.Sum(r => (decimal)r["Amount"]) });

//        decimal max = data.Max(x => x.Sum);
//        Console.WriteLine("\nДиаграмма продаж по регионам (столбчатая, ASCII):");
//        Console.WriteLine("Регион    | Сумма          | Диаграмма (масштаб: # = 500 руб)");
//        Console.WriteLine(new string('-', 50));

//        foreach (var item in data)
//        {
//            int bars = (int)(item.Sum / 500);
//            Console.WriteLine($"{item.Region,-10}| {item.Sum,-14:C} | {new string('#', bars)}");
//        }

//        Console.WriteLine("\nЛегенда: # представляет 500 руб. Ось X: регионы, Ось Y: сумма продаж.");
//    }

//    static void ПоказатьДиаграммуПоПродуктам()
//    {
//        var data = dvProducts.ToTable(true, "Product", "Amount").AsEnumerable()
//            .GroupBy(r => r["Product"])
//            .Select(g => new { Product = g.Key, Sum = g.Sum(r => (decimal)r["Amount"]) });

//        decimal max = data.Max(x => x.Sum);
//        Console.WriteLine("\nДиаграмма продаж по продуктам (столбчатая, ASCII):");
//        Console.WriteLine("Продукт   | Сумма          | Диаграмма (масштаб: @ = 1000 руб)");
//        Console.WriteLine(new string('-', 50));

//        foreach (var item in data)
//        {
//            int bars = (int)(item.Sum / 1000);
//            Console.WriteLine($"{item.Product,-10}| {item.Sum,-14:C} | {new string('@', bars)}");
//        }

//        Console.WriteLine("\nЛегенда: @ представляет 1000 руб. Ось X: продукты, Ось Y: сумма продаж.");
//    }
//}

//18
//using System;
//using System.Data;
//using System.Diagnostics;
//using System.Collections.Generic;

//class Program
//{
//    static void Main()
//    {
//        DataTable dt = new DataTable("Цены на акции");
//        dt.Columns.Add("StockID", typeof(int));
//        dt.Columns.Add("CompanyName", typeof(string));
//        dt.Columns.Add("Price", typeof(double));
//        dt.Columns.Add("Date", typeof(DateTime));
//        dt.Columns.Add("Volume", typeof(int));

//        List<string> companies = new List<string>();
//        for (int i = 1; i <= 20; i++) companies.Add($"Компания {i}");
//        DateTime startDate = new DateTime(2020, 1, 1);
//        Random rnd = new Random(42);

//        for (int i = 1; i <= 1500; i++)
//        {
//            dt.Rows.Add(i, companies[rnd.Next(companies.Count)], Math.Round(50 + rnd.NextDouble() * 450, 2), startDate.AddDays(rnd.Next(1000)), rnd.Next(1000, 100000));
//        }

//        DataView dvPrice = new DataView(dt) { Sort = "Price ASC" };
//        DataView dvDate = new DataView(dt) { Sort = "CompanyName ASC, Date ASC" };

//        double testPrice = 300.0;
//        double rangeLow = 200.0;
//        double rangeHigh = 300.0;

//        Console.WriteLine("Пример поиска ближайшей цены ниже {0}:", testPrice);
//        var below = GetNearestBelow(dvPrice, testPrice);
//        if (below != null) ВывестиСтроку(below);
//        else Console.WriteLine("Нет цены ниже.");

//        Console.WriteLine("\nПример поиска ближайшей цены выше {0}:", testPrice);
//        var above = GetNearestAbove(dvPrice, testPrice);
//        if (above != null) ВывестиСтроку(above);
//        else Console.WriteLine("Нет цены выше.");

//        Console.WriteLine("\nКомпании в диапазоне цен {0} - {1}:", rangeLow, rangeHigh);
//        var range = GetRange(dvPrice, rangeLow, rangeHigh);
//        foreach (var row in range) ВывестиСтроку(row);

//        Console.WriteLine("\nАкция с минимальной ценой в диапазоне:");
//        var minRange = GetMinInRange(dvPrice, rangeLow, rangeHigh);
//        if (minRange != null) ВывестиСтроку(minRange);
//        else Console.WriteLine("Диапазон пуст.");

//        Console.WriteLine("\nАкция с максимальной ценой в диапазоне:");
//        var maxRange = GetMaxInRange(dvPrice, rangeLow, rangeHigh);
//        if (maxRange != null) ВывестиСтроку(maxRange);
//        else Console.WriteLine("Диапазон пуст.");

//        Console.WriteLine("\nСкачки цены > 10%:");
//        var jumps = FindPriceJumps(dvDate);
//        foreach (var jump in jumps)
//        {
//            Console.WriteLine($"Компания: {jump.Company}, Дата: {jump.Date:yyyy-MM-dd}, Предыдущая: {jump.PrevPrice:F2}, Текущая: {jump.CurrPrice:F2}, Изменение: {jump.Change:F2}%");
//        }

//        Граничные случаи
//        Console.WriteLine("\nГраничные случаи:");
//        double lowEdge = (double)dvPrice[0]["Price"] - 1;
//        Console.WriteLine("Ближайшая ниже {0}: Нет", lowEdge);
//        var aboveEdge = GetNearestAbove(dvPrice, lowEdge);
//        if (aboveEdge != null) ВывестиСтроку(aboveEdge);

//        double highEdge = (double)dvPrice[dvPrice.Count - 1]["Price"] + 1;
//        var belowEdge = GetNearestBelow(dvPrice, highEdge);
//        if (belowEdge != null) ВывестиСтроку(belowEdge);
//        Console.WriteLine("Ближайшая выше {0}: Нет", highEdge);

//        Производительность
//       Stopwatch sw = new Stopwatch();
//        sw.Start();
//        for (int i = 0; i < 1000; i++)
//        {
//            GetNearestBelow(dvPrice, testPrice);
//        }
//        sw.Stop();
//        Console.WriteLine("\nПроизводительность бинарного поиска (1000 раз): {0} мс", sw.ElapsedMilliseconds);

//        sw.Reset();
//        sw.Start();
//        for (int i = 0; i < 1000; i++)
//        {
//            LinearNearestBelow(dt, testPrice);
//        }
//        sw.Stop();
//        Console.WriteLine("Производительность линейного поиска (1000 раз): {0} мс", sw.ElapsedMilliseconds);
//    }

//    static int LowerBound(DataView dv, double val)
//    {
//        int low = 0;
//        int high = dv.Count;
//        while (low < high)
//        {
//            int mid = (low + high) / 2;
//            double p = (double)dv[mid]["Price"];
//            if (p < val) low = mid + 1;
//            else high = mid;
//        }
//        return low;
//    }

//    static int UpperBound(DataView dv, double val)
//    {
//        int low = 0;
//        int high = dv.Count;
//        while (low < high)
//        {
//            int mid = (low + high) / 2;
//            double p = (double)dv[mid]["Price"];
//            if (p <= val) low = mid + 1;
//            else high = mid;
//        }
//        return low;
//    }

//    static DataRowView GetNearestBelow(DataView dv, double val)
//    {
//        int idx = LowerBound(dv, val);
//        if (idx > 0) return dv[idx - 1];
//        return null;
//    }

//    static DataRowView GetNearestAbove(DataView dv, double val)
//    {
//        int idx = UpperBound(dv, val);
//        if (idx < dv.Count) return dv[idx];
//        return null;
//    }

//    static List<DataRowView> GetRange(DataView dv, double lowVal, double highVal)
//    {
//        int start = LowerBound(dv, lowVal);
//        int end = UpperBound(dv, highVal);
//        List<DataRowView> result = new List<DataRowView>();
//        for (int i = start; i < end; i++) result.Add(dv[i]);
//        return result;
//    }

//    static DataRowView GetMinInRange(DataView dv, double lowVal, double highVal)
//    {
//        int start = LowerBound(dv, lowVal);
//        int end = UpperBound(dv, highVal);
//        if (start >= end) return null;
//        return dv[start];
//    }

//    static DataRowView GetMaxInRange(DataView dv, double lowVal, double highVal)
//    {
//        int start = LowerBound(dv, lowVal);
//        int end = UpperBound(dv, highVal);
//        if (start >= end) return null;
//        return dv[end - 1];
//    }

//    static List<PriceJump> FindPriceJumps(DataView dv)
//    {
//        List<PriceJump> jumps = new List<PriceJump>();
//        if (dv.Count < 2) return jumps;

//        string prevCompany = (string)dv[0]["CompanyName"];
//        double prevPrice = (double)dv[0]["Price"];
//        DateTime prevDate = (DateTime)dv[0]["Date"];

//        for (int i = 1; i < dv.Count; i++)
//        {
//            string company = (string)dv[i]["CompanyName"];
//            if (company != prevCompany)
//            {
//                prevCompany = company;
//                prevPrice = (double)dv[i]["Price"];
//                prevDate = (DateTime)dv[i]["Date"];
//                continue;
//            }

//            double currPrice = (double)dv[i]["Price"];
//            DateTime currDate = (DateTime)dv[i]["Date"];
//            double change = (currPrice - prevPrice) / prevPrice;
//            if (Math.Abs(change) > 0.1)
//            {
//                jumps.Add(new PriceJump { Company = company, Date = currDate, PrevPrice = prevPrice, CurrPrice = currPrice, Change = change * 100 });
//            }
//            prevPrice = currPrice;
//            prevDate = currDate;
//        }
//        return jumps;
//    }

//    static DataRow LinearNearestBelow(DataTable dt, double val)
//    {
//        DataRow below = null;
//        foreach (DataRow row in dt.Rows)
//        {
//            double p = (double)row["Price"];
//            if (p < val)
//            {
//                if (below == null || (double)below["Price"] < p)
//                    below = row;
//            }
//        }
//        return below;
//    }

//    static void ВывестиСтроку(DataRowView drv)
//    {
//        if (drv == null) return;
//        Console.WriteLine($"ID: {drv["StockID"]}, Компания: {drv["CompanyName"]}, Цена: {drv["Price"]:F2}, Дата: {((DateTime)drv["Date"]):yyyy-MM-dd}, Объём: {drv["Volume"]}");
//    }
//}

//class PriceJump
//{
//    public string Company { get; set; }
//    public DateTime Date { get; set; }
//    public double PrevPrice { get; set; }
//    public double CurrPrice { get; set; }
//    public double Change { get; set; }
//}

//19
//using System;
//using System.Data;
//using System.Text.RegularExpressions;
//using System.Collections.Generic;

//class Program
//{
//    static DateTime currentDate = new DateTime(2025, 12, 4);
//    static Dictionary<string, int> errorStats = new Dictionary<string, int>
//    {
//        { "Name", 0 }, { "Email", 0 }, { "Phone", 0 }, { "Address", 0 }, { "BirthDate", 0 }
//    };
//    static List<string> editLogs = new List<string>();

//    static void Main()
//    {
//        DataTable dt = new DataTable("Контакты");
//        dt.Columns.Add("ContactID", typeof(int));
//        dt.Columns.Add("Name", typeof(string));
//        dt.Columns.Add("Email", typeof(string));
//        dt.Columns.Add("Phone", typeof(string));
//        dt.Columns.Add("Address", typeof(string));
//        dt.Columns.Add("BirthDate", typeof(DateTime));

//        Random rnd = new Random();
//        for (int i = 1; i <= 100; i++)
//        {
//            string name = $"Имя {i}";
//            string email = $"email{i}@example.com";
//            string phone = $"({rnd.Next(100, 1000):D3}){rnd.Next(0, 1000):D3}-{rnd.Next(0, 10000):D4}";
//            string address = $"Адрес {i}";
//            DateTime birth = currentDate.AddYears(-rnd.Next(18, 60)).AddDays(rnd.Next(-365, 365));
//            dt.Rows.Add(i, name, email, phone, address, birth);
//        }

//        dt.ColumnChanging += ВалидацияПриИзменении;

//        DataView dv = new DataView(dt);

//        Console.WriteLine("Демонстрация редактирования с валидацией (через DataView):");

//        ПопыткаРедактирования(dv, 1, "Name", "Новое имя");
//        ПопыткаРедактирования(dv, 1, "Name", "");
//        ПопыткаРедактирования(dv, 2, "Email", "valid@email.com");
//        ПопыткаРедактирования(dv, 2, "Email", "invalid-email");
//        ПопыткаРедактирования(dv, 3, "Phone", "1234567890");
//        ПопыткаРедактирования(dv, 3, "Phone", "invalid");
//        ПопыткаРедактирования(dv, 4, "Address", "Новый адрес");
//        ПопыткаРедактирования(dv, 4, "Address", "");
//        ПопыткаРедактирования(dv, 5, "BirthDate", currentDate.AddYears(-30));
//        ПопыткаРедактирования(dv, 5, "BirthDate", currentDate.AddDays(1));

//        Console.WriteLine("\nЖурнал редактирований:");
//        foreach (string log in editLogs)
//        {
//            Console.WriteLine(log);
//        }

//        Console.WriteLine("\nСтатистика ошибок валидации:");
//        foreach (var kvp in errorStats)
//        {
//            Console.WriteLine($"{kvp.Key}: {kvp.Value} ошибок");
//        }

//        Console.WriteLine("\nПроверка, что исходная таблица не содержит ошибок (пример первых 5 записей):");
//        for (int i = 0; i < 5; i++)
//        {
//            DataRow row = dt.Rows[i];
//            Console.WriteLine($"ID: {row["ContactID"]}, Имя: {row["Name"]}, Email: {row["Email"]}, Телефон: {row["Phone"]}, Адрес: {row["Address"]}, Дата рождения: {((DateTime)row["BirthDate"]):yyyy-MM-dd}");
//        }
//    }

//    static void ВалидацияПриИзменении(object sender, DataColumnChangeEventArgs e)
//    {
//        string column = e.Column.ColumnName;
//        object value = e.ProposedValue;
//        string error = null;

//        switch (column)
//        {
//            case "Name":
//                if (string.IsNullOrEmpty(value as string))
//                    error = "Имя не может быть пустым";
//                break;
//            case "Email":
//                string email = value as string;
//                if (string.IsNullOrEmpty(email) || !email.Contains("@"))
//                    error = "Email должен содержать '@'";
//                break;
//            case "Phone":
//                string phone = value as string;
//                if (!Regex.IsMatch(phone, @"^\(\d{3}\)\d{3}-\d{4}$"))
//                    error = "Телефон должен быть в формате (XXX)XXX-XXXX";
//                break;
//            case "Address":
//                if (string.IsNullOrEmpty(value as string))
//                    error = "Адрес не может быть пустым";
//                break;
//            case "BirthDate":
//                DateTime birth = (DateTime)value;
//                if (birth > currentDate)
//                    error = "Дата рождения не может быть в будущем";
//                break;
//        }

//        if (error != null)
//        {
//            errorStats[column]++;
//            throw new ArgumentException(error);
//        }
//    }

//    static void ПопыткаРедактирования(DataView dv, int contactId, string column, object newValue)
//    {
//        DataRowView rowView = null;
//        foreach (DataRowView drv in dv)
//        {
//            if ((int)drv["ContactID"] == contactId)
//            {
//                rowView = drv;
//                break;
//            }
//        }

//        if (rowView == null) return;

//        string log = $"Попытка редактирования ID {contactId}, столбец {column} на '{newValue}'";
//        editLogs.Add(log);

//        try
//        {
//            if (column == "Phone" && newValue is string phoneInput)
//            {
//                string digits = new string(phoneInput.Where(char.IsDigit).ToArray());
//                if (digits.Length == 10)
//                {
//                    newValue = $"({digits.Substring(0, 3)}){digits.Substring(3, 3)}-{digits.Substring(6)}";
//                    editLogs.Add("Автоформатирование телефона применено");
//                }
//            }
//            else if (column == "BirthDate" && newValue is string dateStr)
//            {
//                newValue = DateTime.Parse(dateStr);
//            }

//            rowView[column] = newValue;
//            editLogs.Add("Успех");
//        }
//        catch (ArgumentException ex)
//        {
//            Console.WriteLine($"Ошибка валидации: {ex.Message}");
//            editLogs.Add($"Неудача: {ex.Message}");
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Общая ошибка: {ex.Message}");
//            editLogs.Add($"Неудача: {ex.Message}");
//        }
//    }
//}

//20
//using System;
//using System.Data;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Text.Json;
//using System.Text.RegularExpressions;
//using System.Xml;

//class Program
//{
//    static void Main()
//    {
//        DataTable dt = new DataTable("Отчёт о продажах");
//        dt.Columns.Add("ReportID", typeof(int));
//        dt.Columns.Add("Date", typeof(DateTime));
//        dt.Columns.Add("Product", typeof(string));
//        dt.Columns.Add("Quantity", typeof(int));
//        dt.Columns.Add("Price", typeof(double));
//        dt.Columns.Add("Salesperson", typeof(string));
//        dt.Columns.Add("Region", typeof(string));

//        string[] products = { "Товар A", "Товар B", "Товар C", "Товар D", "Товар E" };
//        string[] salespersons = { "Продавец 1", "Продавец 2", "Продавец 3" };
//        string[] regions = { "Север", "Юг", "Восток", "Запад" };
//        Random rnd = new Random();
//        DateTime startDate = new DateTime(2023, 1, 1);

//        for (int i = 1; i <= 500; i++)
//        {
//            dt.Rows.Add(i, startDate.AddDays(rnd.Next(365)), products[rnd.Next(products.Length)], rnd.Next(1, 100), Math.Round(rnd.NextDouble() * 1000, 2), salespersons[rnd.Next(salespersons.Length)], regions[rnd.Next(regions.Length)]);
//        }

//        DataView dv = new DataView(dt);
//        dv.RowFilter = "Quantity > 10";
//        dv.Sort = "Date DESC";

//        string exportInfo = $"Дата экспорта: {DateTime.Now:yyyy-MM-dd HH:mm:ss}, Количество строк: {dv.Count}, Фильтр: {dv.RowFilter ?? "нет"}, Сортировка: {dv.Sort ?? "нет"}";

//        Экспорт
//        ExportToCsv(dv, "export.csv", exportInfo);
//        ExportToXml(dv, "export.xml", exportInfo);
//        ExportToJson(dv, "export.json", exportInfo);
//        ExportToHtml(dv, "export.html", exportInfo);

//        Варианты для CSV(например, с точкой с запятой)
//        ExportToCsv(dv, "export_semicolon.csv", exportInfo, ';');

//        Импорт и проверка
//       DataTable importedCsv = ImportFromCsv("export.csv");
//        Console.WriteLine($"Импорт из CSV: {importedCsv.Rows.Count} строк");

//        DataTable importedXml = ImportFromXml("export.xml");
//        Console.WriteLine($"Импорт из XML: {importedXml.Rows.Count} строк");

//        DataTable importedJson = ImportFromJson("export.json");
//        Console.WriteLine($"Импорт из JSON: {importedJson.Rows.Count} строк");

//        DataTable importedHtml = ImportFromHtml("export.html");
//        Console.WriteLine($"Импорт из HTML: {importedHtml.Rows.Count} строк");

//        Для Excel не реализовано, так как требует внешней библиотеки(EPPlus), но можно использовать CSV для открытия в Excel
//        Console.WriteLine("Экспорт в Excel не реализован без EPPlus, используйте CSV.");
//    }

//    static void ExportToCsv(DataView dv, string filePath, string exportInfo, char separator = ',')
//    {
//        StringBuilder sb = new StringBuilder();
//        sb.AppendLine(exportInfo.Replace(separator, ' ')); // Заголовок

//        Заголовки столбцов
//        foreach (DataColumn col in dv.Table.Columns)
//        {
//            sb.Append(EscapeCsv(col.ColumnName, separator) + separator);
//        }
//        sb.Length--; // Удалить последний separator
//        sb.AppendLine();

//        Данные
//        foreach (DataRowView drv in dv)
//        {
//            foreach (DataColumn col in dv.Table.Columns)
//            {
//                string val = drv[col].ToString();
//                if (col.DataType == typeof(DateTime))
//                    val = ((DateTime)drv[col]).ToString("yyyy-MM-dd");
//                sb.Append(EscapeCsv(val, separator) + separator);
//            }
//            sb.Length--;
//            sb.AppendLine();
//        }

//        File.WriteAllText(filePath, sb.ToString());
//        Console.WriteLine($"Экспортировано в CSV: {filePath}");
//    }

//    static string EscapeCsv(string value, char separator)
//    {
//        if (value.Contains(separator) || value.Contains('"') || value.Contains('\n'))
//        {
//            return '"' + value.Replace("\"", "\"\"") + '"';
//        }
//        return value;
//    }

//    static void ExportToXml(DataView dv, string filePath, string exportInfo)
//    {
//        DataTable exportDt = dv.ToTable();
//        using (XmlWriter writer = XmlWriter.Create(filePath))
//        {
//            writer.WriteStartDocument();
//            writer.WriteStartElement("Export");
//            writer.WriteElementString("Info", exportInfo);
//            writer.WriteStartElement("Data");
//            exportDt.WriteXml(writer, XmlWriteMode.WriteSchema);
//            writer.WriteEndElement();
//            writer.WriteEndElement();
//            writer.WriteEndDocument();
//        }
//        Console.WriteLine($"Экспортировано в XML: {filePath}");
//    }

//    static void ExportToJson(DataView dv, string filePath, string exportInfo)
//    {
//        var data = new List<Dictionary<string, object>>();
//        foreach (DataRowView drv in dv)
//        {
//            var dict = new Dictionary<string, object>();
//            foreach (DataColumn col in dv.Table.Columns)
//            {
//                dict[col.ColumnName] = drv[col];
//            }
//            data.Add(dict);
//        }

//        var jsonObj = new { Info = exportInfo, Data = data };
//        string json = JsonSerializer.Serialize(jsonObj, new JsonSerializerOptions { WriteIndented = true });
//        File.WriteAllText(filePath, json);
//        Console.WriteLine($"Экспортировано в JSON: {filePath}");
//    }

//    static void ExportToHtml(DataView dv, string filePath, string exportInfo)
//    {
//        StringBuilder sb = new StringBuilder();
//        sb.AppendLine("<html><body>");
//        sb.AppendLine($"<p>{exportInfo.Replace("<", "&lt;").Replace(">", "&gt;")}</p>");
//        sb.AppendLine("<table border='1'>");

//        Заголовки
//        sb.Append("<tr>");
//        foreach (DataColumn col in dv.Table.Columns)
//        {
//            sb.Append($"<th>{col.ColumnName}</th>");
//        }
//        sb.AppendLine("</tr>");

//        Данные
//        foreach (DataRowView drv in dv)
//        {
//            sb.Append("<tr>");
//            foreach (DataColumn col in dv.Table.Columns)
//            {
//                string val = drv[col].ToString();
//                if (col.DataType == typeof(DateTime))
//                    val = ((DateTime)drv[col]).ToString("yyyy-MM-dd");
//                sb.Append($"<td>{val.Replace("<", "&lt;").Replace(">", "&gt;")}</td>");
//            }
//            sb.AppendLine("</tr>");
//        }

//        sb.AppendLine("</table></body></html>");
//        File.WriteAllText(filePath, sb.ToString());
//        Console.WriteLine($"Экспортировано в HTML: {filePath}");
//    }

//    static DataTable ImportFromCsv(string filePath, char separator = ',')
//    {
//        DataTable dt = new DataTable();
//        string[] lines = File.ReadAllLines(filePath);
//        if (lines.Length < 2) return dt;

//        Пропустить заголовок экспорта(первая строка)
//        string[] headers = ParseCsvLine(lines[1], separator);
//        foreach (string header in headers)
//        {
//            dt.Columns.Add(header);
//        }

//        for (int i = 2; i < lines.Length; i++)
//        {
//            string[] values = ParseCsvLine(lines[i], separator);
//            if (values.Length == headers.Length)
//            {
//                dt.Rows.Add(values);
//            }
//        }
//        return dt;
//    }

//    static string[] ParseCsvLine(string line, char separator)
//    {
//        List<string> values = new List<string>();
//        StringBuilder sb = new StringBuilder();
//        bool inQuotes = false;
//        foreach (char c in line)
//        {
//            if (c == '"' && !inQuotes)
//            {
//                inQuotes = true;
//            }
//            else if (c == '"' && inQuotes)
//            {
//                if (sb.Length > 0 && sb[sb.Length - 1] == '"')
//                {
//                    sb.Length--; // Удалить предыдущий ", если удвоенный
//                    values.Add(sb.ToString());
//                    sb.Clear();
//                    inQuotes = false;
//                }
//            }
//            else if (c == separator && !inQuotes)
//            {
//                values.Add(sb.ToString());
//                sb.Clear();
//            }
//            else
//            {
//                sb.Append(c);
//            }
//        }
//        if (sb.Length > 0) values.Add(sb.ToString());
//        return values.ToArray();
//    }

//    static DataTable ImportFromXml(string filePath)
//    {
//        DataSet ds = new DataSet();
//        ds.ReadXml(filePath);
//        return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
//    }

//    static DataTable ImportFromJson(string filePath)
//    {
//        string json = File.ReadAllText(filePath);
//        var jsonObj = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
//        if (jsonObj.ContainsKey("Data") && jsonObj["Data"] is JsonElement dataElem && dataElem.ValueKind == JsonValueKind.Array)
//        {
//            DataTable dt = new DataTable();
//            bool first = true;
//            foreach (JsonElement rowElem in dataElem.EnumerateArray())
//            {
//                if (first)
//                {
//                    foreach (JsonProperty prop in rowElem.EnumerateObject())
//                    {
//                        dt.Columns.Add(prop.Name);
//                    }
//                    first = false;
//                }
//                var row = dt.NewRow();
//                foreach (JsonProperty prop in rowElem.EnumerateObject())
//                {
//                    row[prop.Name] = prop.Value.ToString(); // Простая конверсия, для типов нужно парсить
//                }
//                dt.Rows.Add(row);
//            }
//            return dt;
//        }
//        return new DataTable();
//    }

//    static DataTable ImportFromHtml(string filePath)
//    {
//        string html = File.ReadAllText(filePath);
//        DataTable dt = new DataTable();

//        Простой парсер для<table>
//       Match tableMatch = Regex.Match(html, @"<table[^>]*>(.*?)</table>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
//        if (!tableMatch.Success) return dt;

//        string tableContent = tableMatch.Groups[1].Value;

//        Заголовки<th>
//       MatchCollection thMatches = Regex.Matches(tableContent, @"<th[^>]*>(.*?)</th>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
//        foreach (Match th in thMatches)
//        {
//            dt.Columns.Add(th.Groups[1].Value.Trim());
//        }

//        Строки<tr>
//       MatchCollection trMatches = Regex.Matches(tableContent, @"<tr[^>]*>(.*?)</tr>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
//        foreach (Match tr in trMatches)
//        {
//            string rowContent = tr.Groups[1].Value;
//            MatchCollection tdMatches = Regex.Matches(rowContent, @"<td[^>]*>(.*?)</td>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
//            if (tdMatches.Count == dt.Columns.Count)
//            {
//                object[] values = new object[dt.Columns.Count];
//                for (int i = 0; i < tdMatches.Count; i++)
//                {
//                    values[i] = tdMatches[i].Groups[1].Value.Trim();
//                }
//                dt.Rows.Add(values);
//            }
//        }

//        return dt;
//    }
//}

//21
//using System;
//using System.Data;
//using System.Collections.Generic;

//class Program
//{
//    static DataTable categories = new DataTable("Категории");
//    static DataTable products = new DataTable("Товары");
//    static Dictionary<int, bool> expandedStates = new Dictionary<int, bool>();

//    static void Main()
//    {
//        SetupTables();
//        FillData();

//        Console.WriteLine("=== Иерархическая структура категорий и товаров (консольное приложение) ===\n");
//        Console.WriteLine("Команды:");
//        Console.WriteLine("  show                 — показать иерархию");
//        Console.WriteLine("  expand <CategoryID>  — развернуть категорию");
//        Console.WriteLine("  collapse <CategoryID>— свернуть категорию");
//        Console.WriteLine("  search <ProductName> — поиск товара (частичное совпадение)");
//        Console.WriteLine("  exit                 — выход\n");

//        while (true)
//        {
//            Console.Write("> ");
//            string input = Console.ReadLine()?.Trim();
//            if (string.IsNullOrEmpty(input)) continue;

//            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
//            string cmd = parts[0].ToLower();

//            try
//            {
//                switch (cmd)
//                {
//                    case "show":
//                        ShowHierarchy();
//                        break;
//                    case "expand" when parts.Length == 2:
//                        int expId = int.Parse(parts[1]);
//                        Expand(expId);
//                        Console.WriteLine($"Категория {expId} развернута.");
//                        break;
//                    case "collapse" when parts.Length == 2:
//                        int colId = int.Parse(parts[1]);
//                        Collapse(colId);
//                        Console.WriteLine($"Категория {colId} свернута.");
//                        break;
//                    case "search" when parts.Length > 1:
//                        string searchName = string.Join(" ", parts.AsSpan(1));
//                        SearchProduct(searchName);
//                        break;
//                    case "exit":
//                        return;
//                    default:
//                        Console.WriteLine("Неизвестная команда или неверные аргументы.");
//                        break;
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Ошибка: {ex.Message}");
//            }
//        }
//    }

//    static void SetupTables()
//    {
//        categories.Columns.Add("CategoryID", typeof(int));
//        categories.Columns.Add("CategoryName", typeof(string));
//        categories.Columns.Add("ParentCategoryID", typeof(int));
//        categories.Columns.Add("Description", typeof(string));
//        categories.PrimaryKey = new[] { categories.Columns["CategoryID"] };

//        products.Columns.Add("ProductID", typeof(int));
//        products.Columns.Add("ProductName", typeof(string));
//        products.Columns.Add("CategoryID", typeof(int));
//        products.Columns.Add("Price", typeof(double));
//        products.PrimaryKey = new[] { products.Columns["ProductID"] };
//    }

//    static void FillData()
//    {
//        Корневые категории
//        categories.Rows.Add(1, "Электроника", DBNull.Value, "Электронные устройства");
//        categories.Rows.Add(6, "Одежда", DBNull.Value, "Одежда и аксессуары");

//        Подкатегории Электроника
//        categories.Rows.Add(2, "Смартфоны", 1, "Мобильные телефоны");
//        categories.Rows.Add(3, "Ноутбуки", 1, "Портативные компьютеры");
//        categories.Rows.Add(4, "Аксессуары", 1, "Дополнительные гаджеты");
//        categories.Rows.Add(5, "Чехлы", 4, "Защитные чехлы");

//        Подкатегории Одежда
//        categories.Rows.Add(7, "Мужская", 6, "Мужская одежда");
//        categories.Rows.Add(8, "Женская", 6, "Женская одежда");
//        categories.Rows.Add(9, "Обувь", 6, "Обувь для всех");
//        categories.Rows.Add(10, "Кроссовки", 9, "Спортивная обувь");

//        Random rnd = new Random();
//        for (int i = 1; i <= 100; i++)
//        {
//            int catId = rnd.Next(1, 11);
//            products.Rows.Add(i, $"Товар {i} ({categories.Rows.Find(catId)["CategoryName"]})", catId, Math.Round(rnd.NextDouble() * 900 + 100, 2));
//            expandedStates[i] = false; // Для категорий, но инициализируем для всех ID категорий
//        }

//        Инициализация состояний для категорий
//        foreach (DataRow cat in categories.Rows)
//        {
//            expandedStates[(int)cat["CategoryID"]] = false;
//        }
//    }

//    static void ShowHierarchy()
//    {
//        Console.WriteLine("\nТекущая иерархия:");
//        DataView topViews = new DataView(categories) { RowFilter = "ParentCategoryID IS NULL", Sort = "CategoryName" };
//        foreach (DataRowView top in topViews)
//        {
//            PrintCategory(top.Row, 0);
//        }
//    }

//    static void PrintCategory(DataRow catRow, int level)
//    {
//        int catId = (int)catRow["CategoryID"];
//        bool isExpanded = expandedStates.GetValueOrDefault(catId, false);
//        string indent = new string(' ', level * 2);
//        string expander = isExpanded ? "-" : "+";
//        Console.WriteLine($"{indent}{expander} {catRow["CategoryName"]} (ID: {catId}, Описание: {catRow["Description"]})");

//        if (!isExpanded) return;

//        Подкатегории
//       DataView subCatView = new DataView(categories) { RowFilter = $"ParentCategoryID = {catId}", Sort = "CategoryName" };
//        foreach (DataRowView sub in subCatView)
//        {
//            PrintCategory(sub.Row, level + 1);
//        }

//        Товары в этой категории
//        DataView prodView = new DataView(products) { RowFilter = $"CategoryID = {catId}", Sort = "ProductName" };
//        foreach (DataRowView prod in prodView)
//        {
//            Console.WriteLine($"{indent}  * {prod["ProductName"]} (ID: {prod["ProductID"]}, Цена: {prod["Price"]:F2})");
//        }
//    }

//    static void Expand(int catId)
//    {
//        if (categories.Rows.Find(catId) == null)
//        {
//            throw new Exception("Категория не найдена");
//        }
//        expandedStates[catId] = true;
//    }

//    static void Collapse(int catId)
//    {
//        if (categories.Rows.Find(catId) == null)
//        {
//            throw new Exception("Категория не найдена");
//        }
//        expandedStates[catId] = false;
//    }

//    static void SearchProduct(string name)
//    {
//        DataView searchView = new DataView(products) { RowFilter = $"ProductName LIKE '%{name.Replace("'", "''")}%'", Sort = "ProductName" };
//        if (searchView.Count == 0)
//        {
//            Console.WriteLine("\nТовары не найдены.");
//            return;
//        }

//        Console.WriteLine("\nНайденные товары:");
//        foreach (DataRowView prod in searchView)
//        {
//            int catId = (int)prod["CategoryID"];
//            string path = GetParentPath(catId);
//            Console.WriteLine($"- {prod["ProductName"]} (ID: {prod["ProductID"]}, Цена: {prod["Price"]:F2}, Путь: {path})");
//        }
//    }

//    static string GetParentPath(int catId)
//    {
//        List<string> pathList = new List<string>();
//        DataRow currentCat = categories.Rows.Find(catId);
//        while (currentCat != null)
//        {
//            pathList.Add((string)currentCat["CategoryName"]);
//            object parentId = currentCat["ParentCategoryID"];
//            currentCat = parentId is DBNull ? null : categories.Rows.Find((int)parentId);
//        }
//        pathList.Reverse();
//        return string.Join(" > ", pathList);
//    }

//    Получение всех товаров в категории и подкатегориях(для примера, но не используется напрямую)
//    static DataView GetAllProductsInSubtree(int catId)
//    {
//        List<int> allCatIds = GetAllSubCatIds(catId);
//        string idList = string.Join(",", allCatIds);
//        DataView allProds = new DataView(products) { RowFilter = $"CategoryID IN ({idList})" };
//        return allProds;
//    }

//    static List<int> GetAllSubCatIds(int catId)
//    {
//        List<int> ids = new List<int> { catId };
//        DataView subs = new DataView(categories) { RowFilter = $"ParentCategoryID = {catId}" };
//        foreach (DataRowView sub in subs)
//        {
//            ids.AddRange(GetAllSubCatIds((int)sub["CategoryID"]));
//        }
//        return ids;
//    }

//    Получение родительской категории
//    static DataRow GetParentCategory(int catId)
//    {
//        DataRow cat = categories.Rows.Find(catId);
//        if (cat == null || cat["ParentCategoryID"] is DBNull) return null;
//        return categories.Rows.Find((int)cat["ParentCategoryID"]);
//    }

//    Получение подкатегорий
//    static DataView GetSubCategories(int catId)
//    {
//        DataView subs = new DataView(categories);
//        subs.RowFilter = $"ParentCategoryID = {catId}";
//        subs.Sort = "CategoryName";
//        return subs;
//    }
//}

//using System.Data;
//using System.Diagnostics;

//22
//using System;
//using System.Data;
//using System.Diagnostics;
//using System.Collections.Generic;
//using System.Linq;

//class Program
//{
//    static DataTable dt = new DataTable("Документы");
//    static DataView dv;
//    static List<string> searchLogs = new List<string>();
//    static string currentFilter = "";

//    static void Main()
//    {
//        СоздатьТаблицуИДанные();
//        dv = new DataView(dt) { Sort = "Date DESC" };

//        Console.WriteLine("=== Продвинутый поиск документов ===\n");
//        Console.WriteLine("Доступные команды:");
//        Console.WriteLine("  filter <строка_фильтра>  — установить фильтр (используйте SQL-like синтаксис)");
//        Console.WriteLine("    Пример: Title LIKE '%отчет%' AND Date BETWEEN #2020-01-01# AND #2023-12-31#");
//        Console.WriteLine("    Для дат используйте #MM/DD/YYYY#");
//        Console.WriteLine("    Для IN: Type IN ('Отчет', 'Письмо')");
//        Console.WriteLine("    Комбинируйте с AND/OR/NOT");
//        Console.WriteLine("  search                   — выполнить поиск и показать результаты (топ 20)");
//        Console.WriteLine("  count                    — показать количество результатов");
//        Console.WriteLine("  performance <фильтр>     — измерить производительность указанного фильтра");
//        Console.WriteLine("  suggest <столбец> <начало> — автодополнение для значений столбца");
//        Console.WriteLine("    Столбцы: Author, Type, Status");
//        Console.WriteLine("  report                   — отчёт о сложных поисках");
//        Console.WriteLine("  reset                    — сброс фильтра");
//        Console.WriteLine("  exit                     — выход\n");

//        while (true)
//        {
//            Console.Write("> ");
//            string input = Console.ReadLine()?.Trim();
//            if (string.IsNullOrEmpty(input)) continue;

//            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
//            string cmd = parts[0].ToLower();

//            try
//            {
//                switch (cmd)
//                {
//                    case "filter" when parts.Length > 1:
//                        currentFilter = string.Join(" ", parts.Skip(1));
//                        dv.RowFilter = currentFilter;
//                        searchLogs.Add($"Сложный фильтр: {currentFilter} (результатов: {dv.Count})");
//                        Console.WriteLine($"Фильтр установлен: {currentFilter}");
//                        break;
//                    case "search":
//                        ПоказатьРезультаты();
//                        break;
//                    case "count":
//                        Console.WriteLine($"Количество документов по фильтру: {dv.Count}");
//                        break;
//                    case "performance" when parts.Length > 1:
//                        string perfFilter = string.Join(" ", parts.Skip(1));
//                        ИзмеритьПроизводительность(perfFilter);
//                        break;
//                    case "suggest" when parts.Length == 3:
//                        string column = parts[1];
//                        string prefix = parts[2];
//                        ПредложитьАвтодополнение(column, prefix);
//                        break;
//                    case "report":
//                        ПоказатьОтчёт();
//                        break;
//                    case "reset":
//                        currentFilter = "";
//                        dv.RowFilter = "";
//                        Console.WriteLine("Фильтр сброшен");
//                        break;
//                    case "exit":
//                        return;
//                    default:
//                        Console.WriteLine("Неизвестная команда");
//                        break;
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Ошибка: {ex.Message} (проверьте синтаксис фильтра)");
//            }
//        }
//    }

//    static void СоздатьТаблицуИДанные()
//    {
//        dt.Columns.Add("DocumentID", typeof(int));
//        dt.Columns.Add("Title", typeof(string));
//        dt.Columns.Add("Content", typeof(string));
//        dt.Columns.Add("Author", typeof(string));
//        dt.Columns.Add("Date", typeof(DateTime));
//        dt.Columns.Add("Type", typeof(string));
//        dt.Columns.Add("Status", typeof(string));

//        string[] types = { "Отчет", "Меморандум", "Письмо", "Контракт", "Инструкция" };
//        string[] statuses = { "Черновик", "Утвержден", "Архив", "Отклонен" };
//        string[] authors = { "Иванов", "Петров", "Сидоров", "Козлов", "Смирнов" };
//        Random rnd = new Random();
//        DateTime baseDate = new DateTime(2010, 1, 1);

//        for (int i = 1; i <= 1000; i++)
//        {
//            string title = $"Документ {i}: {types[rnd.Next(types.Length)]}";
//            string content = new string('a', rnd.Next(100, 500)); // Случайный текст
//            string author = authors[rnd.Next(authors.Length)];
//            DateTime date = baseDate.AddDays(rnd.Next(5000));
//            string type = types[rnd.Next(types.Length)];
//            string status = statuses[rnd.Next(statuses.Length)];

//            dt.Rows.Add(i, title, content, author, date, type, status);
//        }
//    }

//    static void ПоказатьРезультаты(int max = 20)
//    {
//        Console.WriteLine("\nРезультаты поиска (топ 20):");
//        Console.WriteLine($"{"ID",-5} {"Заголовок",-30} {"Автор",-10} {"Дата",-10} {"Тип",-12} {"Статус",-10}");
//        Console.WriteLine(new string('-', 80));

//        int count = Math.Min(max, dv.Count);
//        for (int i = 0; i < count; i++)
//        {
//            var row = dv[i];
//            Console.WriteLine($"{row["DocumentID"],-5} {row["Title"].ToString().Truncate(29),-30} {row["Author"],-10} {((DateTime)row["Date"]).ToString("yyyy-MM-dd"),-10} {row["Type"],-12} {row["Status"],-10}");
//        }
//        if (dv.Count > max) Console.WriteLine($"... и ещё {dv.Count - max} документов");
//    }

//    static void ИзмеритьПроизводительность(string filter)
//    {
//        Stopwatch sw = new Stopwatch();
//        sw.Start();
//        for (int i = 0; i < 100; i++)
//        {
//            dv.RowFilter = filter;
//            int count = dv.Count;
//        }
//        sw.Stop();
//        Console.WriteLine($"Производительность для '{filter}' (100 применений): {sw.ElapsedMilliseconds} мс, результатов: {dv.Count}");
//    }

//    static void ПредложитьАвтодополнение(string column, string prefix)
//    {
//        if (!new[] { "Author", "Type", "Status" }.Contains(column))
//        {
//            Console.WriteLine("Поддерживаемые столбцы: Author, Type, Status");
//            return;
//        }

//        var uniqueValues = dt.AsEnumerable()
//            .Select(r => r.Field<string>(column))
//            .Distinct()
//            .Where(v => v.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
//            .OrderBy(v => v)
//            .ToList();

//        if (uniqueValues.Count == 0)
//        {
//            Console.WriteLine("Нет совпадений");
//            return;
//        }

//        Console.WriteLine("\nВозможные варианты:");
//        foreach (string val in uniqueValues)
//        {
//            Console.WriteLine(val);
//        }
//    }

//    static void ПоказатьОтчёт()
//    {
//        Console.WriteLine("\nОтчёт о сложных поисках:");
//        foreach (string log in searchLogs)
//        {
//            Console.WriteLine(log);
//        }
//        if (searchLogs.Count == 0) Console.WriteLine("Нет записей");
//    }
//}

//static class Extensions
//{
//    public static string Truncate(this string value, int maxLength)
//    {
//        if (string.IsNullOrEmpty(value)) return value;
//        return value.Length <= maxLength ? value : value.Substring(0, maxLength - 1) + "…";
//    }
//}

//23
//using System;
//using System.Data;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;

//class Program
//{
//    static DataTable dt = new DataTable("Заказы");

//    static void Main()
//    {
//        НастроитьТаблицуИДанные();

//        DataView dv = new DataView(dt);
//        dv.Sort = "Date DESC";

//        Console.WriteLine("=== Отчёты с группировкой (консольная версия) ===\n");
//        Console.WriteLine("Доступные команды:");
//        Console.WriteLine("  filter <фильтр>  — установить фильтр (пример: Status = 'Успех')");
//        Console.WriteLine("  report region    — отчёт с группировкой по регионам");
//        Console.WriteLine("  report status    — отчёт с группировкой по статусу");
//        Console.WriteLine("  report month     — отчёт с группировкой по месяцам");
//        Console.WriteLine("  report multi     — многоуровневая группировка (регион → месяц → статус)");
//        Console.WriteLine("  export <тип> <файл> — экспорт отчёта в файл (тип: region, status, month, multi)");
//        Console.WriteLine("  reset            — сброс фильтра");
//        Console.WriteLine("  exit             — выход\n");

//        while (true)
//        {
//            Console.Write("> ");
//            string input = Console.ReadLine()?.Trim();
//            if (string.IsNullOrEmpty(input)) continue;

//            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
//            string cmd = parts[0].ToLower();

//            try
//            {
//                switch (cmd)
//                {
//                    case "filter" when parts.Length > 1:
//                        string filter = string.Join(" ", parts.Skip(1));
//                        dv.RowFilter = filter;
//                        Console.WriteLine($"Фильтр установлен: {filter} (результатов: {dv.Count})");
//                        break;
//                    case "report" when parts.Length == 2:
//                        string type = parts[1].ToLower();
//                        switch (type)
//                        {
//                            case "region": ПоказатьОтчётПоРегионам(dv); break;
//                            case "status": ПоказатьОтчётПоСтатусу(dv); break;
//                            case "month": ПоказатьОтчётПоМесяцам(dv); break;
//                            case "multi": ПоказатьМногоуровневыйОтчёт(dv); break;
//                            default: Console.WriteLine("Неизвестный тип отчёта"); break;
//                        }
//                        break;
//                    case "export" when parts.Length == 3:
//                        string expType = parts[1].ToLower();
//                        string file = parts[2];
//                        ЭкспортироватьОтчёт(dv, expType, file);
//                        break;
//                    case "reset":
//                        dv.RowFilter = "";
//                        Console.WriteLine("Фильтр сброшен");
//                        break;
//                    case "exit":
//                        return;
//                    default:
//                        Console.WriteLine("Неизвестная команда");
//                        break;
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Ошибка: {ex.Message}");
//            }
//        }
//    }

//    static void НастроитьТаблицуИДанные()
//    {
//        dt.Columns.Add("OrderID", typeof(int));
//        dt.Columns.Add("CustomerName", typeof(string));
//        dt.Columns.Add("Product", typeof(string));
//        dt.Columns.Add("Amount", typeof(double));
//        dt.Columns.Add("Date", typeof(DateTime));
//        dt.Columns.Add("Region", typeof(string));
//        dt.Columns.Add("Status", typeof(string));

//        string[] регионы = { "Север", "Юг", "Восток", "Запад" };
//        string[] статусы = { "Успех", "В ожидании", "Отменен" };
//        string[] продукты = { "Товар A", "Товар B", "Товар C" };
//        Random rnd = new Random();
//        DateTime начало = new DateTime(2023, 1, 1);

//        for (int i = 1; i <= 1000; i++)
//        {
//            dt.Rows.Add(
//                i,
//                $"Клиент {i}",
//                продукты[rnd.Next(продукты.Length)],
//                Math.Round(rnd.NextDouble() * 10000, 2),
//                начало.AddDays(rnd.Next(730)),
//                регионы[rnd.Next(регионы.Length)],
//                статусы[rnd.Next(статусы.Length)]
//            );
//        }
//    }

//    Метод для создания таблицы с результатами группировки(общий)
//    static DataTable СоздатьТаблицуГруппировки(string[] столбцыГруппировки, string столбецЗначения)
//    {
//        DataTable таблица = new DataTable();
//        foreach (string col in столбцыГруппировки)
//            таблица.Columns.Add(col, typeof(string));
//        таблица.Columns.Add("Сумма", typeof(double));
//        таблица.Columns.Add("Количество", typeof(int));
//        return таблица;
//    }

//    Группировка по регионам
//    static DataTable ГруппировкаПоРегионам(DataView dv)
//    {
//        DataTable результат = СоздатьТаблицуГруппировки(new[] { "Region" }, "Amount");
//        var группы = new Dictionary<string, (double сумма, int кол)>();

//        foreach (DataRowView drv in dv)
//        {
//            string регион = (string)drv["Region"];
//            double сумма = (double)drv["Amount"];

//            if (!группы.ContainsKey(регион))
//                группы[регион] = (0, 0);

//            var (текСумма, текКол) = группы[регион];
//            группы[регион] = (текСумма + сумма, текКол + 1);
//        }

//        foreach (var kvp in группы.OrderBy(k => k.Key))
//        {
//            результат.Rows.Add(kvp.Key, kvp.Value.сумма, kvp.Value.кол);
//        }

//        return результат;
//    }

//    static void ПоказатьОтчётПоРегионам(DataView dv)
//    {
//        var таблица = ГруппировкаПоРегионам(dv);
//        double итогоСумма = (double)таблица.Compute("SUM(Сумма)", null);
//        int итогоКол = (int)таблица.Compute("SUM(Количество)", null);

//        Console.WriteLine("\nОтчёт по регионам:");
//        foreach (DataRow row in таблица.Rows)
//        {
//            Console.WriteLine($"{row["Region"]}: Сумма = {row["Сумма"]:F2}, Количество = {row["Количество"]}");
//        }
//        Console.WriteLine($"Итого: Сумма = {итогоСумма:F2}, Количество = {итогоКол}");
//    }

//    Группировка по статусу
//    static DataTable ГруппировкаПоСтатусу(DataView dv)
//    {
//        DataTable результат = СоздатьТаблицуГруппировки(new[] { "Status" }, "Amount");
//        var группы = new Dictionary<string, (double сумма, int кол)>();

//        foreach (DataRowView drv in dv)
//        {
//            string статус = (string)drv["Status"];
//            double сумма = (double)drv["Amount"];

//            if (!группы.ContainsKey(статус))
//                группы[статус] = (0, 0);

//            var (текСумма, текКол) = группы[статус];
//            группы[статус] = (текСумма + сумма, текКол + 1);
//        }

//        foreach (var kvp in группы.OrderBy(k => k.Key))
//        {
//            результат.Rows.Add(kvp.Key, kvp.Value.сумма, kvp.Value.кол);
//        }

//        return результат;
//    }

//    static void ПоказатьОтчётПоСтатусу(DataView dv)
//    {
//        var таблица = ГруппировкаПоСтатусу(dv);
//        double итогоСумма = (double)таблица.Compute("SUM(Сумма)", null);
//        int итогоКол = (int)таблица.Compute("SUM(Количество)", null);

//        Console.WriteLine("\nОтчёт по статусам:");
//        foreach (DataRow row in таблица.Rows)
//        {
//            Console.WriteLine($"{row["Status"]}: Сумма = {row["Сумма"]:F2}, Количество = {row["Количество"]}");
//        }
//        Console.WriteLine($"Итого: Сумма = {итогоСумма:F2}, Количество = {итогоКол}");
//    }

//    Группировка по месяцам
//    static DataTable ГруппировкаПоМесяцам(DataView dv)
//    {
//        DataTable результат = СоздатьТаблицуГруппировки(new[] { "Месяц" }, "Amount");
//        var группы = new Dictionary<string, (double сумма, int кол)>();

//        foreach (DataRowView drv in dv)
//        {
//            DateTime дата = (DateTime)drv["Date"];
//            string месяц = дата.ToString("yyyy-MM");
//            double сумма = (double)drv["Amount"];

//            if (!группы.ContainsKey(месяц))
//                группы[месяц] = (0, 0);

//            var (текСумма, текКол) = группы[месяц];
//            группы[месяц] = (текСумма + сумма, текКол + 1);
//        }

//        foreach (var kvp in группы.OrderBy(k => k.Key))
//        {
//            результат.Rows.Add(kvp.Key, kvp.Value.сумма, kvp.Value.кол);
//        }

//        return результат;
//    }

//    static void ПоказатьОтчётПоМесяцам(DataView dv)
//    {
//        var таблица = ГруппировкаПоМесяцам(dv);
//        double итогоСумма = (double)таблица.Compute("SUM(Сумма)", null);
//        int итогоКол = (int)таблица.Compute("SUM(Количество)", null);

//        Console.WriteLine("\nОтчёт по месяцам:");
//        foreach (DataRow row in таблица.Rows)
//        {
//            Console.WriteLine($"{row["Месяц"]}: Сумма = {row["Сумма"]:F2}, Количество = {row["Количество"]}");
//        }
//        Console.WriteLine($"Итого: Сумма = {итогоСумма:F2}, Количество = {итогоКол}");
//    }

//    Многоуровневая группировка
//    static Dictionary<string, Dictionary<string, Dictionary<string, (double сумма, int кол)>>> МногоуровневаяГруппировка(DataView dv)
//    {
//        var группы = new Dictionary<string, Dictionary<string, Dictionary<string, (double, int)>>>();

//        foreach (DataRowView drv in dv)
//        {
//            string регион = (string)drv["Region"];
//            DateTime дата = (DateTime)drv["Date"];
//            string месяц = дата.ToString("yyyy-MM");
//            string статус = (string)drv["Status"];
//            double сумма = (double)drv["Amount"];

//            if (!группы.ContainsKey(регион))
//                группы[регион] = new Dictionary<string, Dictionary<string, (double, int)>>();

//            var месяцы = группы[регион];
//            if (!месяцы.ContainsKey(месяц))
//                месяцы[месяц] = new Dictionary<string, (double, int)>();

//            var статусы = месяцы[месяц];
//            if (!статусы.ContainsKey(статус))
//                статусы[статус] = (0, 0);

//            var (текСумма, текКол) = статусы[статус];
//            статусы[статус] = (текСумма + сумма, текКол + 1);
//        }

//        return группы;
//    }

//    static void ПоказатьМногоуровневыйОтчёт(DataView dv)
//    {
//        var группы = МногоуровневаяГруппировка(dv);
//        double глобИтогоСумма = 0;
//        int глобИтогоКол = 0;

//        Console.WriteLine("\nМногоуровневый отчёт (регион → месяц → статус):");

//        foreach (var регионKvp in группы.OrderBy(k => k.Key))
//        {
//            Console.WriteLine(регионKvp.Key);
//            double регионСумма = 0;
//            int регионКол = 0;

//            foreach (var месяцKvp in регионKvp.Value.OrderBy(k => k.Key))
//            {
//                Console.WriteLine($"  {месяцKvp.Key}");
//                double месяцСумма = 0;
//                int месяцКол = 0;

//                foreach (var статусKvp in месяцKvp.Value.OrderBy(k => k.Key))
//                {
//                    Console.WriteLine($"    {статусKvp.Key}: Сумма = {статусKvp.Value.сумма:F2}, Количество = {статусKvp.Value.кол}");
//                    месяцСумма += статусKvp.Value.сумма;
//                    месяцКол += статусKvp.Value.кол;
//                }

//                Console.WriteLine($"    Итого по месяцу: Сумма = {месяцСумма:F2}, Количество = {месяцКол}");
//                регионСумма += месяцСумма;
//                регионКол += месяцКол;
//            }

//            Console.WriteLine($"  Итого по региону: Сумма = {регионСумма:F2}, Количество = {регионКол}");
//            глобИтогоСумма += регионСумма;
//            глобИтогоКол += регионКол;
//        }

//        Console.WriteLine($"Глобальный итог: Сумма = {глобИтогоСумма:F2}, Количество = {глобИтогоКол}");
//    }

//    static void ЭкспортироватьОтчёт(DataView dv, string тип, string файл)
//    {
//        using (StreamWriter writer = new StreamWriter(файл))
//        {
//            Console.SetOut(writer);
//            switch (тип)
//            {
//                case "region": ПоказатьОтчётПоРегионам(dv); break;
//                case "status": ПоказатьОтчётПоСтатусу(dv); break;
//                case "month": ПоказатьОтчётПоМесяцам(dv); break;
//                case "multi": ПоказатьМногоуровневыйОтчёт(dv); break;
//                default: Console.WriteLine("Неизвестный тип"); return;
//            }
//            Console.SetOut(Console.Out);
//            Console.WriteLine($"Отчёт экспортирован в {файл}");
//        }
//    }
//}

//24
//using System;
//using System.Data;
//using System.Diagnostics;

//class Program
//{
//    static DataTable dt = new DataTable("Заказы товаров");
//    static DataView dv;

//    static void Main()
//    {
//        НастроитьТаблицу();
//        ЗаполнитьДанные();

//        dv = new DataView(dt);
//        dv.Sort = "DiscountedTotal DESC";

//        Console.WriteLine("=== DataView с вычисляемыми столбцами ===\n");

//        ПоказатьПример(10);

//        Console.WriteLine("\nЗаказы с итоговой суммой после скидки > 1000:");
//        dv.RowFilter = "DiscountedTotal > 1000";
//        ПоказатьПример(8);
//        Console.WriteLine($"Найдено заказов: {dv.Count}");

//        dv.Sort = "DiscountedTotal DESC";
//        Console.WriteLine("\nТоп-5 самых дорогих заказов после скидки:");
//        ПоказатьПример(5);

//        double макс = (double)dt.Compute("MAX(DiscountedTotal)", "");
//        double мин = (double)dt.Compute("MIN(DiscountedTotal)", "");
//        double сумма = (double)dt.Compute("SUM(DiscountedTotal)", "");
//        double среднее = (double)dt.Compute("AVG(DiscountedTotal)", "");

//        Console.WriteLine($"\nСтатистика DiscountedTotal:");
//        Console.WriteLine($"  Максимум: {макс:F2}");
//        Console.WriteLine($"  Минимум:  {мин:F2}");
//        Console.WriteLine($"  Сумма:    {сумма:F2}");
//        Console.WriteLine($"  Среднее:  {среднее:F2}");

//        Console.WriteLine("\nДемонстрация обновления вычисляемых полей:");
//        Console.WriteLine("До изменения (строка с OrderDetailID = 5):");
//        dv.RowFilter = "OrderDetailID = 5";
//        ПоказатьПример(1);

//        DataRow ряд = dt.Rows.Find(5);
//        if (ряд != null)
//        {
//            ряд["Quantity"] = 20;
//            ряд["Discount"] = 0.3;
//            Console.WriteLine("Изменено: Quantity = 20, Discount = 30%");
//        }

//        Console.WriteLine("После изменения:");
//        ПоказатьПример(1);
//        dv.RowFilter = "";

//        Console.WriteLine("\nСравнение производительности фильтрации (1000 применений):");

//        Stopwatch sw = new Stopwatch();

//        sw.Restart();
//        for (int i = 0; i < 1000; i++)
//        {
//            dv.RowFilter = "UnitPrice > 50";
//            int count = dv.Count;
//        }
//        sw.Stop();
//        Console.WriteLine($"По обычному полю (UnitPrice > 50): {sw.ElapsedMilliseconds} мс");

//        sw.Restart();
//        for (int i = 0; i < 1000; i++)
//        {
//            dv.RowFilter = "DiscountedTotal > 1000";
//            int count = dv.Count;
//        }
//        sw.Stop();
//        Console.WriteLine($"По вычисляемому полю (DiscountedTotal > 1000): {sw.ElapsedMilliseconds} мс");

//        dv.RowFilter = "";
//    }

//    static void НастроитьТаблицу()
//    {
//        dt.Columns.Add("OrderDetailID", typeof(int));
//        dt.Columns.Add("OrderID", typeof(int));
//        dt.Columns.Add("ProductName", typeof(string));
//        dt.Columns.Add("Quantity", typeof(int));
//        dt.Columns.Add("UnitPrice", typeof(double));
//        dt.Columns.Add("Discount", typeof(double));
//        var totalCol = dt.Columns.Add("Total", typeof(double));
//        totalCol.Expression = "Quantity * UnitPrice";

//        var discountedCol = dt.Columns.Add("DiscountedTotal", typeof(double));
//        discountedCol.Expression = "Total * (1 - Discount)";

//        dt.PrimaryKey = new[] { dt.Columns["OrderDetailID"] };
//    }

//    static void ЗаполнитьДанные()
//    {
//        Random rnd = new Random();
//        string[] продукты = { "Ноутбук", "Монитор", "Клавиатура", "Мышь", "Принтер", "Сканер", "Веб-камера" };

//        for (int i = 1; i <= 250; i++)
//        {
//            dt.Rows.Add(
//                i,
//                rnd.Next(1, 100),
//                продукты[rnd.Next(продукты.Length)],
//                rnd.Next(1, 30),
//                Math.Round(rnd.NextDouble() * 200 + 20, 2),
//                Math.Round(rnd.NextDouble() * 0.4, 2)
//            );
//        }
//    }

//    static void ПоказатьПример(int количество)
//    {
//        int макс = Math.Min(количество, dv.Count);
//        Console.WriteLine($"{"ID",-4} {"Товар",-15} {"Кол-во",-6} {"Цена",-8} {"Скидка",-7} {"Total",-10} {"DiscountedTotal",-15}");
//        Console.WriteLine(new string('-', 80));

//        for (int i = 0; i < макс; i++)
//        {
//            DataRowView row = dv[i];
//            Console.WriteLine(
//                $"{row["OrderDetailID"],-4} " +
//                $"{row["ProductName"].ToString().Truncate(14),-15} " +
//                $"{row["Quantity"],-6} " +
//                $"{row["UnitPrice"],-8:F2} " +
//                $"{row["Discount"],-7:P0} " +
//                $"{row["Total"],-10:F2} " +
//                $"{row["DiscountedTotal"],-15:F2}"
//            );
//        }
//        if (dv.Count > макс)
//            Console.WriteLine($"... и ещё {dv.Count - макс} строк");
//        Console.WriteLine();
//    }
//}

//static class StringExt
//{
//    public static string Truncate(this string s, int len) =>
//        s.Length <= len ? s : s.Substring(0, len - 1) + "…";
//}

//25
//using System;
//using System.Data;
//using System.IO;
//using System.Collections.Generic;

//class Program
//{
//    static DataTable dt = new DataTable("Электронные письма");

//    static void Main()
//    {
//        НастроитьТаблицу();
//        ЗаполнитьДанные();
//        ОтметитьТочныеДубликаты();
//        ОтметитьПохожиеДубликаты();

//        DataView dvВсе = new DataView(dt) { Sort = "EmailID" };
//        DataView dvДубликаты = new DataView(dt) { RowFilter = "Duplicates > 0", Sort = "EmailAddress" };
//        DataView dvУникальные = new DataView(dt) { RowFilter = "Duplicates = 0", Sort = "EmailID" };

//        int колДубликатов = dvДубликаты.Count;
//        int колУникальных = dvУникальные.Count;
//        Console.WriteLine($"Статистика: Дубликатов: {колДубликатов}, Уникальных: {колУникальных}, Всего: {dt.Rows.Count}");

//        Console.WriteLine("\nПример дубликатов:");
//        ПоказатьСтроки(dvДубликаты, 20);

//        ОбъединитьДубликаты();

//        dvДубликаты.RowFilter = "Duplicates > 0";
//        dvУникальные.RowFilter = "Duplicates = 0";
//        колДубликатов = dvДубликаты.Count;
//        колУникальных = dvУникальные.Count;
//        Console.WriteLine($"\nПосле объединения: Дубликатов: {колДубликатов}, Уникальных: {колУникальных}, Всего: {dt.Rows.Count}");

//        СоздатьОтчёт(dvДубликаты);
//    }

//    static void НастроитьТаблицу()
//    {
//        dt.Columns.Add("EmailID", typeof(int));
//        dt.Columns.Add("EmailAddress", typeof(string));
//        dt.Columns.Add("Name", typeof(string));
//        dt.Columns.Add("Domain", typeof(string));
//        dt.Columns.Add("Duplicates", typeof(int));

//        dt.PrimaryKey = new[] { dt.Columns["EmailID"] };
//    }

//    static void ЗаполнитьДанные()
//    {
//        Random rnd = new Random(42);
//        string[] домены = { "gmail.com", "yandex.ru", "mail.ru", "outlook.com", "protonmail.com" };

//        HashSet<string> emails = new HashSet<string>();

//        for (int i = 1; i <= 500; i++)
//        {
//            string имя = СлучайноеИмя(rnd, rnd.Next(5, 11));
//            string домен = домены[rnd.Next(домены.Length)];
//            string email = $"{имя}@{домен}";

//            if (rnd.NextDouble() < 0.2 && emails.Count > 0)
//            {
//                email = emails.ElementAt(rnd.Next(emails.Count));
//            }
//            else if (rnd.NextDouble() < 0.1 && emails.Count > 0)
//            {
//                string существующий = emails.ElementAt(rnd.Next(emails.Count));
//                string староеИмя = существующий.Split('@')[0];
//                int поз = rnd.Next(староеИмя.Length);
//                char новыйСимвол = (char)rnd.Next('a', 'z' + 1);
//                имя = староеИмя.Substring(0, поз) + новыйСимвол + староеИмя.Substring(поз + 1);
//                домен = домены[rnd.Next(домены.Length)];
//                email = $"{имя}@{домен}";
//            }

//            emails.Add(email);
//            dt.Rows.Add(i, email, имя, домен, 0);
//        }
//    }

//    static string СлучайноеИмя(Random rnd, int длина)
//    {
//        string символы = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
//        char[] имя = new char[длина];
//        for (int j = 0; j < длина; j++)
//            имя[j] = символы[rnd.Next(символы.Length)];
//        return new string(имя);
//    }

//    static void ОтметитьТочныеДубликаты()
//    {
//        DataView dv = new DataView(dt) { Sort = "EmailAddress" };
//        string предыдущийEmail = null;
//        int счётчик = 0;

//        for (int i = 0; i < dv.Count; i++)
//        {
//            string текущийEmail = (string)dv[i]["EmailAddress"];

//            if (текущийEmail == предыдущийEmail)
//            {
//                счётчик++;
//                dv[i]["Duplicates"] = счётчик;

//                for (int j = i - 1; j >= 0; j--)
//                {
//                    if ((string)dv[j]["EmailAddress"] == текущийEmail)
//                        dv[j]["Duplicates"] = счётчик + 1;
//                    else
//                        break;
//                }
//            }
//            else
//            {
//                счётчик = 0;
//            }

//            предыдущийEmail = текущийEmail;
//        }
//    }

//    static void ОтметитьПохожиеДубликаты()
//    {
//        DataView dv = new DataView(dt) { Sort = "Name" };

//        for (int i = 0; i < dv.Count - 1; i++)
//        {
//            for (int j = i + 1; j < dv.Count; j++)
//            {
//                string имя1 = (string)dv[i]["Name"];
//                string имя2 = (string)dv[j]["Name"];
//                string домен1 = (string)dv[i]["Domain"];
//                string домен2 = (string)dv[j]["Domain"];

//                if (домен1 != домен2 && РасстояниеЛевенштейна(имя1, имя2) <= 1)
//                {
//                    dv[i]["Duplicates"] = (int)dv[i]["Duplicates"] + 1;
//                    dv[j]["Duplicates"] = (int)dv[j]["Duplicates"] + 1;
//                }

//                if (Math.Abs(имя1.Length - имя2.Length) > 1 || имя1[0] != имя2[0])
//                    break;
//            }
//        }
//    }

//    static int РасстояниеЛевенштейна(string s1, string s2)
//    {
//        int len1 = s1.Length;
//        int len2 = s2.Length;
//        int[,] матрица = new int[len1 + 1, len2 + 1];

//        for (int i = 0; i <= len1; i++) матрица[i, 0] = i;
//        for (int j = 0; j <= len2; j++) матрица[0, j] = j;

//        for (int i = 1; i <= len1; i++)
//        {
//            for (int j = 1; j <= len2; j++)
//            {
//                int стоимость = (s1[i - 1] == s2[j - 1]) ? 0 : 1;
//                матрица[i, j] = Math.Min(
//                    Math.Min(матрица[i - 1, j] + 1, матрица[i, j - 1] + 1),
//                    матрица[i - 1, j - 1] + стоимость
//                );
//            }
//        }

//        return матрица[len1, len2];
//    }

//    static void ПоказатьСтроки(DataView dv, int макс)
//    {
//        int кол = Math.Min(макс, dv.Count);
//        Console.WriteLine($"{"ID",-4} {"Email",-30} {"Имя",-15} {"Домен",-15} {"Дубликатов",-10}");
//        Console.WriteLine(new string('-', 80));

//        for (int i = 0; i < кол; i++)
//        {
//            DataRowView row = dv[i];
//            Console.WriteLine(
//                $"{row["EmailID"],-4} " +
//                $"{row["EmailAddress"],-30} " +
//                $"{row["Name"],-15} " +
//                $"{row["Domain"],-15} " +
//                $"{row["Duplicates"],-10}"
//            );
//        }
//    }

//    static void ОбъединитьДубликаты()
//    {
//        DataView dv = new DataView(dt) { Sort = "EmailAddress, EmailID" };
//        List<DataRow> кУдалению = new List<DataRow>();

//        string предыдущийEmail = null;
//        DataRow главная = null;

//        for (int i = 0; i < dv.Count; i++)
//        {
//            DataRow row = dv[i].Row;
//            string email = (string)row["EmailAddress"];

//            if (email != предыдущийEmail)
//            {
//                главная = row;
//                предыдущийEmail = email;
//            }
//            else
//            {
//                кУдалению.Add(row);
//            }
//        }

//        foreach (DataRow row in кУдалению)
//        {
//            dt.Rows.Remove(row);
//        }

//        foreach (DataRow row in dt.Rows)
//        {
//            row["Duplicates"] = 0;
//        }
//    }

//    static void СоздатьОтчёт(DataView dvДубликаты)
//    {
//        using (StreamWriter writer = new StreamWriter("дубликаты_отчёт.txt"))
//        {
//            writer.WriteLine("Отчёт о дубликатах электронных писем");
//            writer.WriteLine($"Дата: {DateTime.Now}");
//            writer.WriteLine($"Всего дубликатов: {dvДубликаты.Count}");
//            writer.WriteLine("\nСписок дубликатов:");

//            string предыдущийEmail = null;
//            foreach (DataRowView drv in dvДубликаты)
//            {
//                string email = (string)drv["EmailAddress"];
//                if (email != предыдущийEmail)
//                {
//                    writer.WriteLine($"\nГруппа дубликатов для {email}:");
//                    предыдущийEmail = email;
//                }
//                writer.WriteLine($"  ID: {drv["EmailID"]}, Имя: {drv["Name"]}, Домен: {drv["Domain"]}, Дубликатов: {drv["Duplicates"]}");
//            }

//            writer.WriteLine("\nРекомендации:");
//            writer.WriteLine("- Объедините дубликаты, выбрав главную запись с наиболее полной информацией.");
//            writer.WriteLine("- Для похожих дубликатов проверьте, являются ли они одним и тем же пользователем.");
//            writer.WriteLine("- Регулярно очищайте базу от дубликатов для оптимизации.");
//        }

//        Console.WriteLine("\nОтчёт создан: дубликаты_отчёт.txt");
//    }
//}

//using System.Data;

//26
//using System;
//using System.Data;

//class Program
//{
//    static DataTable products = new DataTable("Товары");
//    static DataTable categories = new DataTable("Категории");
//    static DataTable discounts = new DataTable("Скидки");
//    static DataSet ds = new DataSet();

//    static DataView dv1;
//    static DataView dv2;
//    static DataView dv3;

//    static int adds = 0, updates = 0, deletes = 0;
//    static string log = "";

//    static void Main()
//    {
//        НастроитьТаблицы();
//        ЗаполнитьДанные();

//        dv1 = new DataView(products) { Sort = "Category" };
//        dv2 = new DataView(products) { RowFilter = "Price > 1000" };
//        dv3 = new DataView(products) { Sort = "Price DESC" };

//        products.RowChanged += ЛогироватьИзменение;
//        products.RowDeleting += ЛогироватьУдаление;
//        products.RowDeleted += ЛогироватьУдаление;

//        Console.WriteLine("=== Синхронизация DataView (консольная версия) ===\n");

//        ПоказатьПредставления("Начальное состояние");

//        DataRow newRow = products.NewRow();
//        newRow["ProductID"] = 21;
//        newRow["Name"] = "Новый Товар";
//        newRow["Category"] = "Электроника";
//        newRow["Price"] = 1500.0;
//        newRow["Stock"] = 50;
//        products.Rows.Add(newRow);
//        ПоказатьПредставления("После добавления");

//        DataRow rowToUpdate = products.Rows.Find(1);
//        if (rowToUpdate != null)
//        {
//            rowToUpdate["Price"] = 1200.0;
//        }
//        ПоказатьПредставления("После изменения цены");

//        DataRow rowToDelete = products.Rows.Find(21);
//        if (rowToDelete != null)
//        {
//            rowToDelete.Delete();
//        }
//        ПоказатьПредставления("После удаления");

//        Console.WriteLine("\nЛог изменений:");
//        Console.WriteLine(log);
//        Console.WriteLine("\nСтатистика синхронизации:");
//        Console.WriteLine($"Добавлений: {adds}");
//        Console.WriteLine($"Изменений: {updates}");
//        Console.WriteLine($"Удалений: {deletes}");
//    }

//    static void НастроитьТаблицы()
//    {
//        categories.Columns.Add("CategoryID", typeof(int));
//        categories.Columns.Add("CategoryName", typeof(string));
//        categories.PrimaryKey = new[] { categories.Columns["CategoryID"] };

//        discounts.Columns.Add("DiscountID", typeof(int));
//        discounts.Columns.Add("ProductID", typeof(int));
//        discounts.Columns.Add("DiscountPercent", typeof(double));
//        discounts.PrimaryKey = new[] { discounts.Columns["DiscountID"] };

//        products.Columns.Add("ProductID", typeof(int));
//        products.Columns.Add("Name", typeof(string));
//        products.Columns.Add("Category", typeof(string));
//        products.Columns.Add("Price", typeof(double));
//        products.Columns.Add("Stock", typeof(int));
//        products.PrimaryKey = new[] { products.Columns["ProductID"] };

//        ds.Tables.Add(categories);
//        ds.Tables.Add(discounts);
//        ds.Tables.Add(products);

//        ds.Relations.Add("ProductDiscounts", products.Columns["ProductID"], discounts.Columns["ProductID"]);
//    }

//    static void ЗаполнитьДанные()
//    {
//        Random rnd = new Random();

//        categories.Rows.Add(1, "Электроника");
//        categories.Rows.Add(2, "Одежда");
//        categories.Rows.Add(3, "Книги");

//        string[] категории = { "Электроника", "Одежда", "Книги" };
//        for (int i = 1; i <= 20; i++)
//        {
//            products.Rows.Add(i, $"Товар {i}", категории[rnd.Next(3)], rnd.Next(500, 2000), rnd.Next(10, 100));
//        }

//        for (int i = 1; i <= 10; i++)
//        {
//            discounts.Rows.Add(i, rnd.Next(1, 21), rnd.NextDouble() * 0.3);
//        }
//    }

//    static void ПоказатьПредставления(string заголовок)
//    {
//        Console.WriteLine($"\n{заголовок}:");
//        Console.WriteLine("DV1 (сортировка по Category):");
//        ПоказатьТаблица(dv1, 5);

//        Console.WriteLine("DV2 (Price > 1000):");
//        ПоказатьТаблица(dv2, 5);

//        Console.WriteLine("DV3 (сортировка по Price DESC):");
//        ПоказатьТаблица(dv3, 5);
//    }

//    static void ПоказатьТаблица(DataView dv, int макс)
//    {
//        int кол = Math.Min(макс, dv.Count);
//        Console.WriteLine($"{"ID",-4} {"Имя",-15} {"Категория",-12} {"Цена",-8} {"Запас",-6}");
//        Console.WriteLine(new string('-', 50));

//        for (int i = 0; i < кол; i++)
//        {
//            DataRowView row = dv[i];
//            Console.WriteLine($"{row["ProductID"],-4} {row["Name"].ToString().Truncate(14),-15} {row["Category"],-12} {row["Price"],-8:F2} {row["Stock"],-6}");
//        }
//        if (dv.Count > макс) Console.WriteLine($"... + {dv.Count - макс} строк");
//        Console.WriteLine();
//    }

//    static void ЛогироватьИзменение(object sender, DataRowChangeEventArgs e)
//    {
//        if (e.Action == DataRowAction.Add)
//        {
//            adds++;
//            log += $"Добавлен: ID={e.Row["ProductID"]}, Имя={e.Row["Name"]}\n";
//        }
//        else if (e.Action == DataRowAction.Change)
//        {
//            updates++;
//            log += $"Изменён: ID={e.Row["ProductID"]}, Новые значения: Цена={e.Row["Price"]}, Запас={e.Row["Stock"]}\n";
//        }
//    }

//    static void ЛогироватьУдаление(object sender, DataRowChangeEventArgs e)
//    {
//        if (e.Action == DataRowAction.Delete)
//        {
//            deletes++;
//            log += $"Удалён: ID={e.Row["ProductID"]}\n";
//        }
//    }
//}

//static class Ext
//{
//    public static string Truncate(this string s, int len) => s.Length > len ? s.Substring(0, len - 1) + "…" : s;
//}

//27
//using System;
//using System.Data;
//using System.Diagnostics;

//class Program
//{
//    static void Main()
//    {
//        DataTable dt = new DataTable("Логи");
//        dt.Columns.Add("LogID", typeof(int));
//        dt.Columns.Add("Timestamp", typeof(DateTime));
//        dt.Columns.Add("Level", typeof(string));
//        dt.Columns.Add("Message", typeof(string));
//        dt.Columns.Add("Source", typeof(string));
//        dt.Columns.Add("EventID", typeof(int));

//        dt.PrimaryKey = new[] { dt.Columns["LogID"] };

//        Random rnd = new Random();
//        string[] levels = { "Info", "Warning", "Error", "Debug" };
//        string[] sources = { "App1", "App2", "Service", "API" };
//        DateTime baseTime = DateTime.Now.AddYears(-1);

//        Console.WriteLine("Генерация 1,000,000 записей...");
//        long memBefore = GC.GetTotalMemory(true);
//        Stopwatch sw = Stopwatch.StartNew();

//        for (int i = 1; i <= 1000000; i++)
//        {
//            dt.Rows.Add(
//                i,
//                baseTime.AddSeconds(rnd.Next(31536000)),
//                levels[rnd.Next(levels.Length)],
//                $"Сообщение {i}",
//                sources[rnd.Next(sources.Length)],
//                rnd.Next(1000)
//            );
//        }

//        sw.Stop();
//        long memAfter = GC.GetTotalMemory(true);
//        Console.WriteLine($"Генерация завершена. Время: {sw.ElapsedMilliseconds} мс, Память: {(memAfter - memBefore) / 1024 / 1024} МБ");

//        string filter = "Level = 'Error'";

//        Console.WriteLine("\nСценарий 1: Select() в DataTable");
//        long mem1Before = GC.GetTotalMemory(true);
//        sw.Restart();
//        DataRow[] res1First = dt.Select(filter);
//        sw.Stop();
//        long mem1After = GC.GetTotalMemory(true);
//        Console.WriteLine($"Первое выполнение: Время = {sw.ElapsedMilliseconds} мс, Результатов = {res1First.Length}, Память = {(mem1After - mem1Before) / 1024} КБ");

//        sw.Restart();
//        DataRow[] res1Repeat = dt.Select(filter);
//        sw.Stop();
//        Console.WriteLine($"Повторное: Время = {sw.ElapsedMilliseconds} мс");

//        Console.WriteLine("\nСценарий 2: Создание DataView с фильтром");
//        long mem2Before = GC.GetTotalMemory(true);
//        sw.Restart();
//        DataView dvFirst = new DataView(dt) { RowFilter = filter };
//        int count2First = dvFirst.Count;
//        sw.Stop();
//        long mem2After = GC.GetTotalMemory(true);
//        Console.WriteLine($"Первое выполнение: Время = {sw.ElapsedMilliseconds} мс, Результатов = {count2First}, Память = {(mem2After - mem2Before) / 1024} КБ");

//        Console.WriteLine("\nСценарий 3: Повторный поиск в DataView");
//        sw.Restart();
//        int count3Repeat = dvFirst.Count;
//        sw.Stop();
//        Console.WriteLine($"Повторное: Время = {sw.ElapsedMilliseconds} мс, Результатов = {count3Repeat}");

//        Console.WriteLine("\nСценарий 4: Поиск через Find() по PK");
//        int searchId = 500000;
//        long mem4Before = GC.GetTotalMemory(true);
//        sw.Restart();
//        DataRow row4First = dt.Rows.Find(searchId);
//        sw.Stop();
//        long mem4After = GC.GetTotalMemory(true);
//        Console.WriteLine($"Первое выполнение: Время = {sw.ElapsedMilliseconds} мс, Найдено = {(row4First != null ? "Да" : "Нет")}, Память = {(mem4After - mem4Before) / 1024} КБ");

//        sw.Restart();
//        DataRow row4Repeat = dt.Rows.Find(searchId);
//        sw.Stop();
//        Console.WriteLine($"Повторное: Время = {sw.ElapsedMilliseconds} мс");

//        Console.WriteLine("\nКэширование результатов Select:");
//        DataRow[] cachedRes = dt.Select(filter);
//        sw.Restart();
//        int cachedCount = cachedRes.Length;
//        sw.Stop();
//        Console.WriteLine($"Доступ к кэшу: Время = {sw.ElapsedMilliseconds} мс, Результатов = {cachedCount}");

//        GC.Collect();
//        long baseMem = GC.GetTotalMemory(true);

//        DataRow[] selectRes = dt.Select(filter);
//        GC.Collect();
//        long selectMem = GC.GetTotalMemory(true) - baseMem;

//        DataView dvMem = new DataView(dt) { RowFilter = filter };
//        GC.Collect();
//        long dvMemUsage = GC.GetTotalMemory(true) - baseMem - selectMem;

//        Console.WriteLine("\nСравнение памяти:");
//        Console.WriteLine($"Select(): ~{selectMem / 1024} КБ");
//        Console.WriteLine($"DataView: ~{dvMemUsage / 1024} КБ");

//        Console.WriteLine("\nОтчёт о производительности:");
//        Console.WriteLine("| Сценарий | Первое (мс) | Повтор (мс) | Память (КБ) |");
//        Console.WriteLine("|----------|-------------|-------------|-------------|");
//        Console.WriteLine("| 1. Select | ... | ... | ... |");  
//        Console.WriteLine("\nРекомендации по использованию DataView:");
//        Console.WriteLine("- Для повторяющихся запросов создавайте DataView один раз — повторные фильтры быстрые благодаря индексу.");
//        Console.WriteLine("- DataView оптимизирован для больших данных, где Select() сканирует таблицу каждый раз.");
//        Console.WriteLine("- Кэшируйте результаты для статичных данных, чтобы минимизировать вычисления.");
//        Console.WriteLine("- DataView использует больше памяти на индекс, но ускоряет операции.");
//        Console.WriteLine("- Комбинируйте с PrimaryKey для быстрого Find() по ключу.");
//    }
//}

//28
//using System;
//using System.Data;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;

//class Program
//{
//    static void Main()
//    {
//        DataTable library = new DataTable("Библиотека");
//        library.Columns.Add("BookID", typeof(int));
//        library.Columns.Add("Title", typeof(string));
//        library.Columns.Add("Author", typeof(string));
//        library.Columns.Add("Year", typeof(int));
//        library.Columns.Add("ISBN", typeof(string));
//        library.Columns.Add("Genre", typeof(string));
//        library.Columns.Add("Rating", typeof(double));
//        library.Columns.Add("Pages", typeof(int));

//        ЗаполнитьБиблиотеку(library);

//        SearchEngine engine = new SearchEngine(library);

//        Console.WriteLine("=== Библиотека с продвинутым поиском (консольный интерфейс) ===\n");
//        Console.WriteLine("Доступные команды:");
//        Console.WriteLine("  author <автор>          — поиск по автору (частичное совпадение)");
//        Console.WriteLine("  title <название>        — поиск по названию");
//        Console.WriteLine("  genre <жанр>            — поиск по жанру");
//        Console.WriteLine("  year <от> <до>          — поиск по году (диапазон)");
//        Console.WriteLine("  rating <мин>            — поиск по рейтингу (минимум)");
//        Console.WriteLine("  combined                — комбинированный поиск (вводите параметры по запросу)");
//        Console.WriteLine("  history                 — показать историю поисков");
//        Console.WriteLine("  recommend               — рекомендации на основе истории");
//        Console.WriteLine("  save <файл>             — сохранить последние результаты в CSV");
//        Console.WriteLine("  exit                    — выход\n");

//        DataView lastResults = null;

//        while (true)
//        {
//            Console.Write("> ");
//            string input = Console.ReadLine()?.Trim();
//            if (string.IsNullOrEmpty(input)) continue;

//            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
//            string cmd = parts[0].ToLower();

//            try
//            {
//                switch (cmd)
//                {
//                    case "author" when parts.Length > 1:
//                        string author = string.Join(" ", parts.Skip(1));
//                        lastResults = engine.SearchByAuthor(author);
//                        break;
//                    case "title" when parts.Length > 1:
//                        string title = string.Join(" ", parts.Skip(1));
//                        lastResults = engine.SearchByTitle(title);
//                        break;
//                    case "genre" when parts.Length > 1:
//                        string genre = string.Join(" ", parts.Skip(1));
//                        lastResults = engine.SearchByGenre(genre);
//                        break;
//                    case "year" when parts.Length == 3:
//                        int fromYear = int.Parse(parts[1]);
//                        int toYear = int.Parse(parts[2]);
//                        lastResults = engine.SearchByYear(fromYear, toYear);
//                        break;
//                    case "rating" when parts.Length == 2:
//                        double minRating = double.Parse(parts[1]);
//                        lastResults = engine.SearchByRating(minRating);
//                        break;
//                    case "combined":
//                        lastResults = КомбинированныйПоиск(engine);
//                        break;
//                    case "history":
//                        engine.ShowHistory();
//                        continue;
//                    case "recommend":
//                        engine.GetRecommendations();
//                        continue;
//                    case "save" when parts.Length == 2 && lastResults != null:
//                        СохранитьРезультаты(lastResults, parts[1]);
//                        continue;
//                    case "exit":
//                        return;
//                    default:
//                        Console.WriteLine("Неизвестная команда или неверные аргументы.");
//                        continue;
//                }

//                if (lastResults != null)
//                {
//                    ПоказатьРезультаты(lastResults);
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Ошибка: {ex.Message}");
//            }
//        }
//    }

//    static void ЗаполнитьБиблиотеку(DataTable table)
//    {
//        string[] authors = { "Лев Толстой", "Фёдор Достоевский", "Джейн Остин", "Джон Толкин", "Джоан Роулинг", "Стивен Кинг", "Агата Кристи", "Марк Твен", "Джордж Орвелл", "Рэй Брэдбери" };
//        string[] genres = { "Фантастика", "Классика", "Детектив", "Фэнтези", "Ужасы", "Приключения", "Роман" };
//        Random rnd = new Random();

//        for (int i = 1; i <= 500; i++)
//        {
//            table.Rows.Add(
//                i,
//                $"Книга {i}: {genres[rnd.Next(genres.Length)]}",
//                authors[rnd.Next(authors.Length)],
//                rnd.Next(1800, 2024),
//                $"978-{rnd.Next(1000000000, 2000000000)}",
//                genres[rnd.Next(genres.Length)],
//                Math.Round(rnd.NextDouble() * 5, 1),
//                rnd.Next(100, 1000)
//            );
//        }
//    }

//    static DataView КомбинированныйПоиск(SearchEngine engine)
//    {
//        Console.Write("Автор (или пусто): ");
//        string author = Console.ReadLine()?.Trim();

//        Console.Write("Название (или пусто): ");
//        string title = Console.ReadLine()?.Trim();

//        Console.Write("Жанр (или пусто): ");
//        string genre = Console.ReadLine()?.Trim();

//        Console.Write("Год от (или 0): ");
//        int fromYear = int.TryParse(Console.ReadLine(), out int fy) ? fy : 0;

//        Console.Write("Год до (или 0): ");
//        int toYear = int.TryParse(Console.ReadLine(), out int ty) ? ty : 0;

//        Console.Write("Мин. рейтинг (или 0): ");
//        double minRating = double.TryParse(Console.ReadLine(), out double mr) ? mr : 0;

//        return engine.CombinedSearch(author, title, genre, fromYear > 0 ? fromYear : (int?)null, toYear > 0 ? toYear : (int?)null, minRating > 0 ? minRating : (double?)null);
//    }

//    static void ПоказатьРезультаты(DataView results)
//    {
//        Console.WriteLine("\nРезультаты поиска:");
//        Console.WriteLine($"{"ID",-4} {"Название",-30} {"Автор",-20} {"Год",-5} {"ISBN",-15} {"Жанр",-15} {"Рейтинг",-8} {"Страниц",-7}");
//        Console.WriteLine(new string('-', 110));

//        foreach (DataRowView row in results)
//        {
//            Console.WriteLine(
//                $"{row["BookID"],-4} " +
//                $"{row["Title"].ToString().Truncate(29),-30} " +
//                $"{row["Author"].ToString().Truncate(19),-20} " +
//                $"{row["Year"],-5} " +
//                $"{row["ISBN"],-15} " +
//                $"{row["Genre"],-15} " +
//                $"{row["Rating"],-8:F1} " +
//                $"{row["Pages"],-7}"
//            );
//        }

//        Console.WriteLine($"\nНайдено: {results.Count} книг\n");
//    }

//    static void СохранитьРезультаты(DataView results, string filePath)
//    {
//        using (StreamWriter writer = new StreamWriter(filePath))
//        {
//            writer.WriteLine("BookID,Title,Author,Year,ISBN,Genre,Rating,Pages");

//            foreach (DataRowView row in results)
//            {
//                writer.WriteLine(
//                    $"{row["BookID"]}," +
//                    $"\"{row["Title"].ToString().Replace("\"", "\"\"")}\"," +
//                    $"\"{row["Author"].ToString().Replace("\"", "\"\"")}\"," +
//                    $"{row["Year"]}," +
//                    $"{row["ISBN"]}," +
//                    $"{row["Genre"]}," +
//                    $"{row["Rating"]}," +
//                    $"{row["Pages"]}"
//                );
//            }
//        }

//        Console.WriteLine($"Результаты сохранены в {filePath}\n");
//    }
//}

//class SearchEngine
//{
//    private DataTable _library;
//    private List<string> _history = new List<string>();

//    public SearchEngine(DataTable library)
//    {
//        _library = library;
//    }

//    public DataView SearchByAuthor(string author)
//    {
//        string filter = $"Author LIKE '%{author.Replace("'", "''")}%'";
//        return СоздатьView(filter, "По автору: " + author);
//    }

//    public DataView SearchByTitle(string title)
//    {
//        string filter = $"Title LIKE '%{title.Replace("'", "''")}%'";
//        return СоздатьView(filter, "По названию: " + title);
//    }

//    public DataView SearchByGenre(string genre)
//    {
//        string filter = $"Genre = '{genre.Replace("'", "''")}'";
//        return СоздатьView(filter, "По жанру: " + genre);
//    }

//    public DataView SearchByYear(int from, int to)
//    {
//        string filter = $"Year >= {from} AND Year <= {to}";
//        return СоздатьView(filter, $"По году: {from}-{to}");
//    }

//    public DataView SearchByRating(double min)
//    {
//        string filter = $"Rating >= {min}";
//        return СоздатьView(filter, $"По рейтингу: >= {min}");
//    }

//    public DataView CombinedSearch(string author = null, string title = null, string genre = null, int? fromYear = null, int? toYear = null, double? minRating = null)
//    {
//        List<string> filters = new List<string>();
//        string desc = "Комбинированный: ";

//        if (!string.IsNullOrEmpty(author))
//        {
//            filters.Add($"Author LIKE '%{author.Replace("'", "''")}%'");
//            desc += $"Автор={author}; ";
//        }
//        if (!string.IsNullOrEmpty(title))
//        {
//            filters.Add($"Title LIKE '%{title.Replace("'", "''")}%'");
//            desc += $"Название={title}; ";
//        }
//        if (!string.IsNullOrEmpty(genre))
//        {
//            filters.Add($"Genre = '{genre.Replace("'", "''")}'");
//            desc += $"Жанр={genre}; ";
//        }
//        if (fromYear.HasValue && toYear.HasValue)
//        {
//            filters.Add($"Year >= {fromYear} AND Year <= {toYear}");
//            desc += $"Год={fromYear}-{toYear}; ";
//        }
//        if (minRating.HasValue)
//        {
//            filters.Add($"Rating >= {minRating}");
//            desc += $"Рейтинг>={minRating}; ";
//        }

//        string filter = string.Join(" AND ", filters);
//        return СоздатьView(filter, desc);
//    }

//    private DataView СоздатьView(string filter, string description)
//    {
//        DataView view = new DataView(_library)
//        {
//            RowFilter = filter,
//            Sort = "Rating DESC"
//        };

//        _history.Add($"{DateTime.Now:yyyy-MM-dd HH:mm}: {description} (найдено: {view.Count})");

//        return view;
//    }

//    public void ShowHistory()
//    {
//        Console.WriteLine("\nИстория поисков:");
//        foreach (string entry in _history)
//        {
//            Console.WriteLine(entry);
//        }
//        Console.WriteLine();
//    }

//    public void GetRecommendations()
//    {
//        if (_history.Count == 0)
//        {
//            Console.WriteLine("Нет истории для рекомендаций.");
//            return;
//        }

//        Dictionary<string, int> genreCount = new Dictionary<string, int>();
//        Dictionary<string, int> authorCount = new Dictionary<string, int>();

//        foreach (string entry in _history)
//        {
//            if (entry.Contains("Жанр="))
//            {
//                string genre = entry.Split("Жанр=")[1].Split(';')[0].Trim();
//                genreCount[genre] = genreCount.GetValueOrDefault(genre, 0) + 1;
//            }
//            if (entry.Contains("Автор="))
//            {
//                string author = entry.Split("Автор=")[1].Split(';')[0].Trim();
//                authorCount[author] = authorCount.GetValueOrDefault(author, 0) + 1;
//            }
//        }

//        string recGenre = genreCount.OrderByDescending(kv => kv.Value).FirstOrDefault().Key;
//        string recAuthor = authorCount.OrderByDescending(kv => kv.Value).FirstOrDefault().Key;

//        DataView recView = new DataView(_library);
//        string recFilter = "";
//        if (!string.IsNullOrEmpty(recGenre))
//            recFilter += $"Genre = '{recGenre}'";
//        if (!string.IsNullOrEmpty(recAuthor))
//            recFilter += string.IsNullOrEmpty(recFilter) ? "" : " OR " + $"Author LIKE '%{recAuthor}%'";

//        recView.RowFilter = recFilter;
//        recView.Sort = "Rating DESC";

//        Console.WriteLine("\nРекомендации на основе истории:");
//        if (!string.IsNullOrEmpty(recGenre)) Console.WriteLine($"Частый жанр: {recGenre}");
//        if (!string.IsNullOrEmpty(recAuthor)) Console.WriteLine($"Частый автор: {recAuthor}");
//        ПоказатьРезультаты(recView, 5);
//    }

//    private void ПоказатьРезультаты(DataView results, int max = int.MaxValue)
//    {
//        int count = Math.Min(max, results.Count);
//        Console.WriteLine($"{"ID",-4} {"Название",-30} {"Автор",-20} {"Год",-5} {"ISBN",-15} {"Жанр",-15} {"Рейтинг",-8} {"Страниц",-7}");
//        Console.WriteLine(new string('-', 110));

//        for (int i = 0; i < count; i++)
//        {
//            DataRowView row = results[i];
//            Console.WriteLine(
//                $"{row["BookID"],-4} " +
//                $"{row["Title"].ToString().Truncate(29),-30} " +
//                $"{row["Author"].ToString().Truncate(19),-20} " +
//                $"{row["Year"],-5} " +
//                $"{row["ISBN"],-15} " +
//                $"{row["Genre"],-15} " +
//                $"{row["Rating"],-8:F1} " +
//                $"{row["Pages"],-7}"
//            );
//        }

//        if (results.Count > max) Console.WriteLine($"... и ещё {results.Count - max} книг");
//        Console.WriteLine();
//    }
//}

//static class StringExtensions
//{
//    public static string Truncate(this string value, int maxLength)
//    {
//        if (string.IsNullOrEmpty(value)) return value;
//        return value.Length <= maxLength ? value : value.Substring(0, maxLength - 1) + "…";
//    }
//}

//29
//using System;
//using System.Data;
//using System.Windows.Forms;
//using System.IO;
//using System.Collections.Generic;

//namespace EmployeeApp
//{
//    public class MainForm : Form
//    {
//        private DataTable employees = new DataTable("Сотрудники");
//        private List<string> accessLogs = new List<string>();
//        private List<string> changeLogs = new List<string>();
//        private DataGridView dgv;
//        private ComboBox cmbRole;
//        private TextBox txtDepartment;
//        private Button btnReport, btnExport;

//        public MainForm()
//        {
//            InitializeData();
//            InitializeUI();
//            employees.TableNewRow += (s, e) => LogChange("Добавлена новая строка");
//            employees.RowChanged += (s, e) => LogChange($"Изменена строка ID={e.Row["EmployeeID"]}, Действие={e.Action}");
//            employees.RowDeleted += (s, e) => LogChange($"Удалена строка ID={e.Row["EmployeeID"]}, Действие={e.Action}");
//        }

//        private void InitializeData()
//        {
//            employees.Columns.Add("EmployeeID", typeof(int));
//            employees.Columns.Add("Name", typeof(string));
//            employees.Columns.Add("Department", typeof(string));
//            employees.Columns.Add("Salary", typeof(double));
//            employees.Columns.Add("HireDate", typeof(DateTime));
//            employees.Columns.Add("Status", typeof(string));

//            employees.PrimaryKey = new[] { employees.Columns["EmployeeID"] };

//            Random rnd = new Random();
//            string[] departments = { "HR", "IT", "Finance", "Sales" };
//            string[] statuses = { "Active", "Inactive" };
//            DateTime baseDate = new DateTime(2010, 1, 1);

//            for (int i = 1; i <= 100; i++)
//            {
//                employees.Rows.Add(i, $"Employee {i}", departments[rnd.Next(departments.Length)], rnd.Next(3000, 10000), baseDate.AddDays(rnd.Next(5000)), statuses[rnd.Next(statuses.Length)]);
//            }
//        }

//        private void InitializeUI()
//        {
//            this.Text = "Система просмотра сотрудников";
//            this.Size = new System.Drawing.Size(800, 600);

//            Label lblRole = new Label { Text = "Роль:", Left = 20, Top = 20 };
//            cmbRole = new ComboBox { Left = 80, Top = 17, Width = 150 };
//            cmbRole.Items.AddRange(new[] { "HR Manager", "Department Head", "Finance", "General" });
//            cmbRole.SelectedIndexChanged += UpdateView;

//            Label lblDept = new Label { Text = "Отдел (для Head):", Left = 250, Top = 20 };
//            txtDepartment = new TextBox { Left = 380, Top = 17, Width = 150 };

//            dgv = new DataGridView { Left = 20, Top = 60, Width = 750, Height = 450, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };

//            btnReport = new Button { Text = "Отчёт о доступе", Left = 20, Top = 520, Width = 150 };
//            btnReport.Click += (s, e) => ShowReport();

//            btnExport = new Button { Text = "Экспорт в CSV", Left = 180, Top = 520, Width = 150 };
//            btnExport.Click += (s, e) => ExportToCsv();

//            this.Controls.AddRange(new Control[] { lblRole, cmbRole, lblDept, txtDepartment, dgv, btnReport, btnExport });
//        }

//        private void UpdateView(object sender, EventArgs e)
//        {
//            string role = cmbRole.SelectedItem?.ToString();
//            if (string.IsNullOrEmpty(role)) return;

//            LogAccess(role);

//            DataView view = new DataView(employees);
//            bool canEdit = false;

//            switch (role)
//            {
//                case "HR Manager":
//                    view.Sort = "Salary DESC";
//                    canEdit = true;
//                    break;
//                case "Department Head":
//                    string dept = txtDepartment.Text.Trim();
//                    if (string.IsNullOrEmpty(dept))
//                    {
//                        MessageBox.Show("Введите отдел");
//                        return;
//                    }
//                    view.RowFilter = $"Department = '{dept.Replace("'", "''")}'";
//                    canEdit = true;
//                    break;
//                case "Finance":
//                    view.RowFilter = "Status = 'Active' AND Salary > 5000";
//                    break;
//                case "General":
//                    view = new DataView(employees);
//                    dgv.DataSource = view;
//                    dgv.Columns["EmployeeID"].Visible = false;
//                    dgv.Columns["Salary"].Visible = false;
//                    dgv.Columns["HireDate"].Visible = false;
//                    dgv.Columns["Status"].Visible = false;
//                    return; 
//            }

//            dgv.DataSource = view;
//            dgv.ReadOnly = !canEdit;
//        }

//        private void LogAccess(string role)
//        {
//            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Роль: {role} просмотрел данные (строк: {dgv.RowCount})";
//            accessLogs.Add(logEntry);
//        }

//        private void LogChange(string message)
//        {
//            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Изменение: {message}";
//            changeLogs.Add(logEntry);
//        }

//        private void ShowReport()
//        {
//            string report = "Отчёт о доступе:\n" + string.Join("\n", accessLogs) +
//                            "\n\nАудит изменений:\n" + string.Join("\n", changeLogs);
//            MessageBox.Show(report);
//        }

//        private void ExportToCsv()
//        {
//            if (dgv.DataSource is not DataView view) return;

//            using (SaveFileDialog sfd = new SaveFileDialog { Filter = "CSV|*.csv" })
//            {
//                if (sfd.ShowDialog() == DialogResult.OK)
//                {
//                    using (StreamWriter sw = new StreamWriter(sfd.FileName))
//                    {
//                        foreach (DataGridViewColumn col in dgv.Columns)
//                        {
//                            if (col.Visible) sw.Write(col.HeaderText + ",");
//                        }
//                        sw.WriteLine();

//                        foreach (DataRowView row in view)
//                        {
//                            foreach (DataGridViewColumn col in dgv.Columns)
//                            {
//                                if (col.Visible) sw.Write(row[col.DataPropertyName] + ",");
//                            }
//                            sw.WriteLine();
//                        }
//                    }
//                    MessageBox.Show("Экспорт завершён");
//                }
//            }
//        }
//    }

//    static void Main()
//        {
//            Application.EnableVisualStyles();
//            Application.SetCompatibleTextRenderingDefault(false);
//            Application.Run(new MainForm());
//        }
//    }