////Задание 1: Создание простого отношения DataRelation между двумя таблицами.
//using System;
//using System.Data;
//using System.Collections.Generic;

//namespace DataRelationExample
//{
//    class Program
//    {
//        static void Main()
//        {
//            // Создаём DataSet и таблицы
//            DataSet ds = CreateDataSet();

//            // Заполняем таблицы тестовыми данными
//            FillTestData(ds);

//            // Создаём отношение один-ко-многим
//            CreateRelationship(ds);

//            Console.WriteLine("=== ДЕМОНСТРАЦИЯ НАВИГАЦИИ ПО ОТНОШЕНИЮ ОДИН-КО-МНОГИМ ===\n");

//            // 1. Информация о созданном отношении
//            Console.WriteLine("1. ИНФОРМАЦИЯ О СОЗДАННОМ ОТНОШЕНИИ:");
//            Console.WriteLine("=====================================");
//            PrintRelationInfo(ds);
//            Console.WriteLine();

//            // 2. Вывод иерархической структуры (категории → товары)
//            Console.WriteLine("2. ИЕРАРХИЧЕСКАЯ СТРУКТУРА (КАТЕГОРИИ → ТОВАРЫ):");
//            Console.WriteLine("=====================================");
//            PrintHierarchy(ds);
//            Console.WriteLine();

//            // 3. Поиск товаров в указанной категории
//            Console.WriteLine("3. ТОВАРЫ В КАТЕГОРИИ 'Электроника':");
//            Console.WriteLine("=====================================");
//            GetProductsByCategory(ds, "Электроника");
//            Console.WriteLine();

//            // 4. Вывод полной информации о товарах
//            Console.WriteLine("4. ПОЛНАЯ ИНФОРМАЦИЯ О ТОВАРАХ:");
//            Console.WriteLine("=====================================");
//            PrintAllProducts(ds);
//            Console.WriteLine();

//            // 5. Средняя цена товаров в каждой категории
//            Console.WriteLine("5. СРЕДНЯЯ ЦЕНА ТОВАРОВ В КАЖДОЙ КАТЕГОРИИ:");
//            Console.WriteLine("=====================================");
//            CalculateAveragePriceByCategory(ds);
//            Console.WriteLine();
//        }

//        // Создание DataSet с таблицами
//        static DataSet CreateDataSet()
//        {
//            DataSet ds = new DataSet("ProductDataSet");

//            // Таблица Категории
//            DataTable categories = new DataTable("Категории");
//            categories.Columns.Add("CategoryID", typeof(int));
//            categories.Columns.Add("CategoryName", typeof(string));
//            categories.Columns.Add("Описание", typeof(string));
//            categories.PrimaryKey = new DataColumn[] { categories.Columns["CategoryID"] };

//            // Таблица Товары
//            DataTable products = new DataTable("Товары");
//            products.Columns.Add("ProductID", typeof(int));
//            products.Columns.Add("ProductName", typeof(string));
//            products.Columns.Add("Price", typeof(decimal));
//            products.Columns.Add("CategoryID", typeof(int));
//            products.PrimaryKey = new DataColumn[] { products.Columns["ProductID"] };

//            ds.Tables.Add(categories);
//            ds.Tables.Add(products);

//            return ds;
//        }

//        // Заполнение тестовыми данными
//        static void FillTestData(DataSet ds)
//        {
//            DataTable categories = ds.Tables["Категории"];
//            DataTable products = ds.Tables["Товары"];

//            // Добавляем категории
//            categories.Rows.Add(1, "Электроника", "Электронные устройства");
//            categories.Rows.Add(2, "Одежда", "Одежда и обувь");
//            categories.Rows.Add(3, "Книги", "Книги и журналы");

//            // Добавляем товары
//            products.Rows.Add(1, "Смартфон", 29990.00m, 1);
//            products.Rows.Add(2, "Ноутбук", 59990.00m, 1);
//            products.Rows.Add(3, "Наушники", 2990.00m, 1);
//            products.Rows.Add(4, "Футболка", 990.00m, 2);
//            products.Rows.Add(5, "Джинсы", 2490.00m, 2);
//            products.Rows.Add(6, "Кроссовки", 3990.00m, 2);
//            products.Rows.Add(7, "Роман", 490.00m, 3);
//            products.Rows.Add(8, "Учебник", 1290.00m, 3);
//            products.Rows.Add(9, "Журнал", 190.00m, 3);
//        }

//        // Создание отношения один-ко-многим
//        static void CreateRelationship(DataSet ds)
//        {
//            DataTable categories = ds.Tables["Категории"];
//            DataTable products = ds.Tables["Товары"];

//            try
//            {
//                DataRelation relation = new DataRelation(
//                    "CategoryProducts",
//                    categories.Columns["CategoryID"],
//                    products.Columns["CategoryID"],
//                    true); // createConstraints = true

//                ds.Relations.Add(relation);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Ошибка при создании отношения: {ex.Message}");
//            }
//        }

//        // Вывод информации о созданном отношении
//        static void PrintRelationInfo(DataSet ds)
//        {
//            DataRelation relation = ds.Relations["CategoryProducts"];

//            Console.WriteLine($"Имя отношения: {relation.RelationName}");
//            Console.WriteLine($"Родительская таблица: {relation.ParentTable.TableName}");
//            Console.WriteLine($"Дочерняя таблица: {relation.ChildTable.TableName}");
//            Console.WriteLine($"Колонка связи в родительской таблице: {relation.ParentColumns[0].ColumnName}");
//            Console.WriteLine($"Колонка связи в дочерней таблице: {relation.ChildColumns[0].ColumnName}");
//        }

//        // Вывод иерархической структуры (категории → товары)
//        static void PrintHierarchy(DataSet ds)
//        {
//            DataTable categories = ds.Tables["Категории"];
//            DataRelation relation = ds.Relations["CategoryProducts"];

//            foreach (DataRow categoryRow in categories.Rows)
//            {
//                Console.WriteLine($"\nКатегория: {categoryRow["CategoryName"]} ({categoryRow["CategoryID"]}) - {categoryRow["Описание"]}");

//                DataRow[] productRows = categoryRow.GetChildRows(relation);

//                foreach (DataRow productRow in productRows)
//                {
//                    Console.WriteLine($"\tТовар: {productRow["ProductName"]}, Цена: {productRow["Price"]:C}");
//                }
//            }
//        }

//        // Поиск товаров в указанной категории
//        static void GetProductsByCategory(DataSet ds, string categoryName)
//        {
//            DataTable categories = ds.Tables["Категории"];
//            DataTable products = ds.Tables["Товары"];
//            DataRelation relation = ds.Relations["CategoryProducts"];

//            DataRow categoryRow = categories.Select($"CategoryName = '{categoryName}'")[0];

//            if (categoryRow != null)
//            {
//                Console.WriteLine($"Товары в категории: {categoryRow["CategoryName"]}");

//                DataRow[] productRows = categoryRow.GetChildRows(relation);

//                foreach (DataRow productRow in productRows)
//                {
//                    Console.WriteLine($"\tТовар: {productRow["ProductName"]}, Цена: {productRow["Price"]:C}");
//                }
//            }
//            else
//            {
//                Console.WriteLine($"Категория '{categoryName}' не найдена.");
//            }
//        }

//        // Вывод полной информации о товарах
//        static void PrintAllProducts(DataSet ds)
//        {
//            DataTable products = ds.Tables["Товары"];
//            DataTable categories = ds.Tables["Категории"];
//            DataRelation relation = ds.Relations["CategoryProducts"];

//            Console.WriteLine("ID | Название товара         | Цена     | Категория");
//            Console.WriteLine("─────────────────────────────────────────────────────────────────────────");

//            foreach (DataRow productRow in products.Rows)
//            {
//                int productID = (int)productRow["ProductID"];
//                string productName = (string)productRow["ProductName"];
//                decimal price = (decimal)productRow["Price"];
//                int categoryID = (int)productRow["CategoryID"];

//                DataRow categoryRow = productRow.GetParentRow(relation);
//                string categoryName = categoryRow != null ? (string)categoryRow["CategoryName"] : "Неизвестно";

//                Console.WriteLine($"{productID,2} | {productName,-22} | {price,8:C} | {categoryName}");
//            }
//        }

//        // Средняя цена товаров в каждой категории
//        static void CalculateAveragePriceByCategory(DataSet ds)
//        {
//            DataTable categories = ds.Tables["Категории"];
//            DataRelation relation = ds.Relations["CategoryProducts"];

//            Console.WriteLine("Категория               | Количество товаров | Средняя цена");
//            Console.WriteLine("─────────────────────────────────────────────────────────────────────────");

//            foreach (DataRow categoryRow in categories.Rows)
//            {
//                string categoryName = (string)categoryRow["CategoryName"];
//                DataRow[] productRows = categoryRow.GetChildRows(relation);

//                if (productRows.Length == 0)
//                {
//                    Console.WriteLine($"{categoryName,-22} | {0,17} | Нет товаров");
//                    continue;
//                }

//                decimal sumPrices = 0;
//                foreach (DataRow productRow in productRows)
//                {
//                    sumPrices += (decimal)productRow["Price"];
//                }

//                decimal averagePrice = sumPrices / productRows.Length;
//                Console.WriteLine($"{categoryName,-22} | {productRows.Length,17} | {averagePrice,13:C}");
//            }
//        }
//    }
//}


////Задание 2: Получение дочерних строк с помощью GetChildRows()
//using System;
//using System.Data;
//using System.Linq;

//namespace DataRelationExample
//{
//    class Program
//    {
//        static void Main()
//        {
//            // Создаём DataSet и таблицы
//            DataSet ds = CreateDataSet();

//            // Заполняем таблицы тестовыми данными
//            FillTestData(ds);

//            // Создаём отношение один-ко-многим
//            CreateRelationship(ds);

//            Console.WriteLine("=== РАБОТА С МЕТОДОМ GetChildRows() ===\n");

//            // 1. Вывод всех товаров для каждой категории
//            Console.WriteLine("1. ТОВАРЫ В КАЖДОЙ КАТЕГОРИИ:");
//            Console.WriteLine("=====================================");
//            PrintProductsByCategory(ds);
//            Console.WriteLine();

//            // 2. Вывод характеристик и формирование всех товаров в виде таблицы
//            Console.WriteLine("2. ХАРАКТЕРИСТИКИ И ФОРМИРОВАНИЕ ТОВАРОВ:");
//            Console.WriteLine("=====================================");
//            PrintProductsTable(ds);
//            Console.WriteLine();

//            // 3. Поиск товаров в конкретной категории
//            Console.WriteLine("3. ПОИСК ТОВАРОВ В КАТЕГОРИИ 'Электроника':");
//            Console.WriteLine("=====================================");
//            FindProductsInCategory(ds, "Электроника");
//            Console.WriteLine();

//            // 4. Подсчёт количества товаров в каждой категории
//            Console.WriteLine("4. КОЛИЧЕСТВО ТОВАРОВ В КАЖДОЙ КАТЕГОРИИ:");
//            Console.WriteLine("=====================================");
//            CountProductsByCategory(ds);
//            Console.WriteLine();

//            // 5. Стоимость товаров в каждой категории
//            Console.WriteLine("5. СТОИМОСТЬ ТОВАРОВ В КАЖДОЙ КАТЕГОРИИ:");
//            Console.WriteLine("=====================================");
//            CalculateTotalPriceByCategory(ds);
//            Console.WriteLine();

//            // 6. Фильтр категорий, содержащих товары выше определённой цены
//            Console.WriteLine("6. КАТЕГОРИИ С ТОВАРАМИ ДОРОЖЕ 3000:");
//            Console.WriteLine("=====================================");
//            FilterCategoriesByPrice(ds, 3000);
//            Console.WriteLine();
//        }

//        // Создание DataSet с таблицами
//        static DataSet CreateDataSet()
//        {
//            DataSet ds = new DataSet("ProductDataSet");

//            // Таблица Категории
//            DataTable categories = new DataTable("Категории");
//            categories.Columns.Add("CategoryID", typeof(int));
//            categories.Columns.Add("CategoryName", typeof(string));
//            categories.Columns.Add("Описание", typeof(string));
//            categories.PrimaryKey = new DataColumn[] { categories.Columns["CategoryID"] };

//            // Таблица Товары
//            DataTable products = new DataTable("Товары");
//            products.Columns.Add("ProductID", typeof(int));
//            products.Columns.Add("ProductName", typeof(string));
//            products.Columns.Add("Price", typeof(decimal));
//            products.Columns.Add("Quantity", typeof(int)); // Добавлено поле Количество
//            products.Columns.Add("CategoryID", typeof(int));
//            products.PrimaryKey = new DataColumn[] { products.Columns["ProductID"] };

//            ds.Tables.Add(categories);
//            ds.Tables.Add(products);

//            return ds;
//        }

//        // Заполнение тестовыми данными
//        static void FillTestData(DataSet ds)
//        {
//            DataTable categories = ds.Tables["Категории"];
//            DataTable products = ds.Tables["Товары"];

//            // Добавляем категории
//            categories.Rows.Add(1, "Электроника", "Электронные устройства");
//            categories.Rows.Add(2, "Одежда", "Одежда и обувь");
//            categories.Rows.Add(3, "Книги", "Книги и журналы");

//            // Добавляем товары
//            products.Rows.Add(1, "Смартфон", 29990.00m, 10, 1);
//            products.Rows.Add(2, "Ноутбук", 59990.00m, 5, 1);
//            products.Rows.Add(3, "Наушники", 2990.00m, 20, 1);
//            products.Rows.Add(4, "Футболка", 990.00m, 30, 2);
//            products.Rows.Add(5, "Джинсы", 2490.00m, 15, 2);
//            products.Rows.Add(6, "Кроссовки", 3990.00m, 10, 2);
//            products.Rows.Add(7, "Роман", 490.00m, 50, 3);
//            products.Rows.Add(8, "Учебник", 1290.00m, 20, 3);
//            products.Rows.Add(9, "Журнал", 190.00m, 100, 3);
//        }

//        // Создание отношения один-ко-многим
//        static void CreateRelationship(DataSet ds)
//        {
//            DataTable categories = ds.Tables["Категории"];
//            DataTable products = ds.Tables["Товары"];

//            try
//            {
//                DataRelation relation = new DataRelation(
//                    "CategoryProducts",
//                    categories.Columns["CategoryID"],
//                    products.Columns["CategoryID"],
//                    true); // createConstraints = true

//                ds.Relations.Add(relation);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Ошибка при создании отношения: {ex.Message}");
//            }
//        }

//        // 1. Вывод всех товаров для каждой категории
//        static void PrintProductsByCategory(DataSet ds)
//        {
//            DataTable categories = ds.Tables["Категории"];
//            DataRelation relation = ds.Relations["CategoryProducts"];

//            foreach (DataRow categoryRow in categories.Rows)
//            {
//                Console.WriteLine($"\nКатегория: {categoryRow["CategoryName"]} ({categoryRow["CategoryID"]}) - {categoryRow["Описание"]}");

//                DataRow[] productRows = categoryRow.GetChildRows(relation);

//                if (productRows.Length == 0)
//                {
//                    Console.WriteLine("\tВ этой категории нет товаров.");
//                    continue;
//                }

//                foreach (DataRow productRow in productRows)
//                {
//                    Console.WriteLine($"\tТовар: {productRow["ProductName"]}, Цена: {productRow["Price"]:C}, Количество: {productRow["Quantity"]}");
//                }
//            }
//        }

//        // 2. Вывод характеристик и формирование всех товаров в виде таблицы
//        static void PrintProductsTable(DataSet ds)
//        {
//            DataTable products = ds.Tables["Товары"];
//            DataTable categories = ds.Tables["Категории"];
//            DataRelation relation = ds.Relations["CategoryProducts"];

//            Console.WriteLine("ID | Название товара         | Цена     | Количество | Категория");
//            Console.WriteLine("─────────────────────────────────────────────────────────────────────────────────────────────────────────────────");

//            foreach (DataRow productRow in products.Rows)
//            {
//                int productID = (int)productRow["ProductID"];
//                string productName = (string)productRow["ProductName"];
//                decimal price = (decimal)productRow["Price"];
//                int quantity = (int)productRow["Quantity"];

//                DataRow categoryRow = productRow.GetParentRow(relation);
//                string categoryName = categoryRow != null ? (string)categoryRow["CategoryName"] : "Неизвестно";

//                Console.WriteLine($"{productID,2} | {productName,-22} | {price,8:C} | {quantity,11} | {categoryName}");
//            }
//        }

//        // 3. Поиск товаров в конкретной категории
//        static void FindProductsInCategory(DataSet ds, string categoryName)
//        {
//            DataTable categories = ds.Tables["Категории"];
//            DataRelation relation = ds.Relations["CategoryProducts"];

//            DataRow[] categoryRows = categories.Select($"CategoryName = '{categoryName}'");

//            if (categoryRows.Length == 0)
//            {
//                Console.WriteLine($"Категория '{categoryName}' не найдена.");
//                return;
//            }

//            DataRow categoryRow = categoryRows[0];
//            Console.WriteLine($"Товары в категории: {categoryRow["CategoryName"]}");

//            DataRow[] productRows = categoryRow.GetChildRows(relation);

//            if (productRows.Length == 0)
//            {
//                Console.WriteLine("\tВ этой категории нет товаров.");
//                return;
//            }

//            foreach (DataRow productRow in productRows)
//            {
//                Console.WriteLine($"\tТовар: {productRow["ProductName"]}, Цена: {productRow["Price"]:C}, Количество: {productRow["Quantity"]}");
//            }
//        }

//        // 4. Подсчёт количества товаров в каждой категории
//        static void CountProductsByCategory(DataSet ds)
//        {
//            DataTable categories = ds.Tables["Категории"];
//            DataRelation relation = ds.Relations["CategoryProducts"];

//            Console.WriteLine("Категория               | Количество товаров");
//            Console.WriteLine("─────────────────────────────────────────────────────────────────────────");

//            foreach (DataRow categoryRow in categories.Rows)
//            {
//                string categoryName = (string)categoryRow["CategoryName"];
//                DataRow[] productRows = categoryRow.GetChildRows(relation);

//                Console.WriteLine($"{categoryName,-22} | {productRows.Length,17}");
//            }
//        }

//        // 5. Стоимость товаров в каждой категории
//        static void CalculateTotalPriceByCategory(DataSet ds)
//        {
//            DataTable categories = ds.Tables["Категории"];
//            DataRelation relation = ds.Relations["CategoryProducts"];

//            Console.WriteLine("Категория               | Общая стоимость");
//            Console.WriteLine("─────────────────────────────────────────────────────────────────────────");

//            foreach (DataRow categoryRow in categories.Rows)
//            {
//                string categoryName = (string)categoryRow["CategoryName"];
//                DataRow[] productRows = categoryRow.GetChildRows(relation);

//                decimal totalPrice = 0;

//                foreach (DataRow productRow in productRows)
//                {
//                    decimal price = (decimal)productRow["Price"];
//                    int quantity = (int)productRow["Quantity"];
//                    totalPrice += price * quantity;
//                }

//                Console.WriteLine($"{categoryName,-22} | {totalPrice,17:C}");
//            }
//        }

//        // 6. Фильтр категорий, содержащих товары выше определённой цены
//        static void FilterCategoriesByPrice(DataSet ds, decimal minPrice)
//        {
//            DataTable categories = ds.Tables["Категории"];
//            DataRelation relation = ds.Relations["CategoryProducts"];

//            Console.WriteLine($"Категории с товарами дороже {minPrice:C}:");
//            Console.WriteLine("─────────────────────────────────────────────────────────────────────────");

//            foreach (DataRow categoryRow in categories.Rows)
//            {
//                string categoryName = (string)categoryRow["CategoryName"];
//                DataRow[] productRows = categoryRow.GetChildRows(relation);

//                bool hasExpensiveProducts = false;

//                foreach (DataRow productRow in productRows)
//                {
//                    decimal price = (decimal)productRow["Price"];
//                    if (price > minPrice)
//                    {
//                        hasExpensiveProducts = true;
//                        break;
//                    }
//                }

//                if (hasExpensiveProducts)
//                {
//                    Console.WriteLine($"\nКатегория: {categoryName}");

//                    foreach (DataRow productRow in productRows)
//                    {
//                        decimal price = (decimal)productRow["Price"];
//                        if (price > minPrice)
//                        {
//                            Console.WriteLine($"\tТовар: {productRow["ProductName"]}, Цена: {productRow["Price"]:C}");
//                        }
//                    }
//                }
//            }
//        }
//    }
//}


////Задание 3: Получение родительских строк с помощью GetParentRows()
//using System;
//using System.Data;

//namespace GetParentRowsExample
//{
//    class Program
//    {
//        static void Main()
//        {
//            // Создаём DataSet и таблицы
//            DataSet ds = CreateDataSet();

//            // Заполняем таблицы тестовыми данными
//            FillTestData(ds);

//            // Создаём отношение один-ко-многим
//            CreateRelationship(ds);

//            Console.WriteLine("=== РАБОТА С МЕТОДОМ GetParentRows() ===\n");

//            // 1. Получение родительских категорий для нескольких товаров
//            Console.WriteLine("1. РОДИТЕЛЬСКИЕ КАТЕГОРИИ ДЛЯ ТОВАРОВ:");
//            Console.WriteLine("=====================================");
//            GetParentCategoriesForProducts(ds, new int[] { 1, 3, 5 });
//            Console.WriteLine();

//            // 2. Вывод информации о товаре вместе с его категорией
//            Console.WriteLine("2. ИНФОРМАЦИЯ О ТОВАРЕ И ЕГО КАТЕГОРИИ:");
//            Console.WriteLine("=====================================");
//            PrintProductWithCategory(ds, 3);
//            Console.WriteLine();

//            // 3. Поиск по идентификатору товара и вывод полной информации
//            Console.WriteLine("3. ПОЛНАЯ ИНФОРМАЦИЯ О ТОВАРЕ ПО ID:");
//            Console.WriteLine("=====================================");
//            PrintFullProductInfo(ds, 5);
//            Console.WriteLine();

//            // 4. Отчёт: товар → категория → описание категории
//            Console.WriteLine("4. ОТЧЁТ: ТОВАР → КАТЕГОРИЯ → ОПИСАНИЕ КАТЕГОРИИ:");
//            Console.WriteLine("=====================================");
//            PrintProductCategoryReport(ds);
//            Console.WriteLine();

//            // 5. Обработка случаев, когда товар не имеет родительской категории
//            Console.WriteLine("5. ПРОВЕРКА НА ПОТЕРЯННЫЕ ЗАПИСИ:");
//            Console.WriteLine("=====================================");
//            CheckForOrphanedProducts(ds);
//            Console.WriteLine();

//            // 6. Демонстрация разницы между GetParentRows() для измененной и удаленной строки
//            Console.WriteLine("6. РАЗНИЦА МЕЖДУ GetParentRows() ДЛЯ ИЗМЕНЁННОЙ И УДАЛЁННОЙ СТРОКИ:");
//            Console.WriteLine("=====================================");
//            DemonstrateGetParentRowsDifference(ds);
//            Console.WriteLine();
//        }

//        // Создание DataSet с таблицами
//        static DataSet CreateDataSet()
//        {
//            DataSet ds = new DataSet("ProductDataSet");

//            // Таблица Категории
//            DataTable categories = new DataTable("Категории");
//            categories.Columns.Add("CategoryID", typeof(int));
//            categories.Columns.Add("CategoryName", typeof(string));
//            categories.Columns.Add("Описание", typeof(string));
//            categories.PrimaryKey = new DataColumn[] { categories.Columns["CategoryID"] };

//            // Таблица Товары
//            DataTable products = new DataTable("Товары");
//            products.Columns.Add("ProductID", typeof(int));
//            products.Columns.Add("ProductName", typeof(string));
//            products.Columns.Add("Price", typeof(decimal));
//            products.Columns.Add("CategoryID", typeof(int));
//            products.PrimaryKey = new DataColumn[] { products.Columns["ProductID"] };

//            ds.Tables.Add(categories);
//            ds.Tables.Add(products);

//            return ds;
//        }

//        // Заполнение тестовыми данными
//        static void FillTestData(DataSet ds)
//        {
//            DataTable categories = ds.Tables["Категории"];
//            DataTable products = ds.Tables["Товары"];

//            // Добавляем категории
//            categories.Rows.Add(1, "Электроника", "Электронные устройства");
//            categories.Rows.Add(2, "Одежда", "Одежда и обувь");
//            categories.Rows.Add(3, "Книги", "Книги и журналы");

//            // Добавляем товары
//            products.Rows.Add(1, "Смартфон", 29990.00m, 1);
//            products.Rows.Add(2, "Ноутбук", 59990.00m, 1);
//            products.Rows.Add(3, "Наушники", 2990.00m, 1);
//            products.Rows.Add(4, "Футболка", 990.00m, 2);
//            products.Rows.Add(5, "Джинсы", 2490.00m, 2);
//            products.Rows.Add(6, "Кроссовки", 3990.00m, 2);
//            products.Rows.Add(7, "Роман", 490.00m, 3);
//            products.Rows.Add(8, "Учебник", 1290.00m, 3);
//            products.Rows.Add(9, "Журнал", 190.00m, 3);
//            products.Rows.Add(10, "Потерянный товар", 100.00m, 99); // Товар без категории
//        }

//        // Создание отношения один-ко-многим
//        static void CreateRelationship(DataSet ds)
//        {
//            DataTable categories = ds.Tables["Категории"];
//            DataTable products = ds.Tables["Товары"];

//            try
//            {
//                DataRelation relation = new DataRelation(
//                    "CategoryProducts",
//                    categories.Columns["CategoryID"],
//                    products.Columns["CategoryID"],
//                    true); // createConstraints = true

//                ds.Relations.Add(relation);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Ошибка при создании отношения: {ex.Message}");
//            }
//        }

//        // 1. Получение родительских категорий для нескольких товаров
//        static void GetParentCategoriesForProducts(DataSet ds, int[] productIDs)
//        {
//            DataTable products = ds.Tables["Товары"];
//            DataRelation relation = ds.Relations["CategoryProducts"];

//            foreach (int productID in productIDs)
//            {
//                DataRow productRow = products.Rows.Find(productID);

//                if (productRow != null)
//                {
//                    Console.WriteLine($"\nТовар: {productRow["ProductName"]}");

//                    DataRow[] parentRows = productRow.GetParentRows(relation);

//                    if (parentRows.Length > 0)
//                    {
//                        DataRow categoryRow = parentRows[0];
//                        Console.WriteLine($"\tКатегория: {categoryRow["CategoryName"]}");
//                    }
//                    else
//                    {
//                        Console.WriteLine("\tНет родительской категории.");
//                    }
//                }
//                else
//                {
//                    Console.WriteLine($"\nТовар с ID {productID} не найден.");
//                }
//            }
//        }

//        // 2. Вывод информации о товаре вместе с его категорией
//        static void PrintProductWithCategory(DataSet ds, int productID)
//        {
//            DataTable products = ds.Tables["Товары"];
//            DataRelation relation = ds.Relations["CategoryProducts"];

//            DataRow productRow = products.Rows.Find(productID);

//            if (productRow != null)
//            {
//                Console.WriteLine($"Товар: {productRow["ProductName"]}, Цена: {productRow["Price"]:C}");

//                DataRow[] parentRows = productRow.GetParentRows(relation);

//                if (parentRows.Length > 0)
//                {
//                    DataRow categoryRow = parentRows[0];
//                    Console.WriteLine($"\tКатегория: {categoryRow["CategoryName"]}");
//                    Console.WriteLine($"\tОписание категории: {categoryRow["Описание"]}");
//                }
//                else
//                {
//                    Console.WriteLine("\tНет родительской категории.");
//                }
//            }
//            else
//            {
//                Console.WriteLine($"Товар с ID {productID} не найден.");
//            }
//        }

//        // 3. Поиск по идентификатору товара и вывод полной информации
//        static void PrintFullProductInfo(DataSet ds, int productID)
//        {
//            DataTable products = ds.Tables["Товары"];
//            DataRelation relation = ds.Relations["CategoryProducts"];

//            DataRow productRow = products.Rows.Find(productID);

//            if (productRow != null)
//            {
//                Console.WriteLine($"Полная информация о товаре:");
//                Console.WriteLine($"ID: {productRow["ProductID"]}");
//                Console.WriteLine($"Название: {productRow["ProductName"]}");
//                Console.WriteLine($"Цена: {productRow["Price"]:C}");

//                DataRow[] parentRows = productRow.GetParentRows(relation);

//                if (parentRows.Length > 0)
//                {
//                    DataRow categoryRow = parentRows[0];
//                    Console.WriteLine($"\nКатегория:");
//                    Console.WriteLine($"\tID: {categoryRow["CategoryID"]}");
//                    Console.WriteLine($"\tНазвание: {categoryRow["CategoryName"]}");
//                    Console.WriteLine($"\tОписание: {categoryRow["Описание"]}");
//                }
//                else
//                {
//                    Console.WriteLine("\nНет родительской категории.");
//                }
//            }
//            else
//            {
//                Console.WriteLine($"Товар с ID {productID} не найден.");
//            }
//        }

//        // 4. Отчёт: товар → категория → описание категории
//        static void PrintProductCategoryReport(DataSet ds)
//        {
//            DataTable products = ds.Tables["Товары"];
//            DataRelation relation = ds.Relations["CategoryProducts"];

//            Console.WriteLine("Товар                     | Категория         | Описание категории");
//            Console.WriteLine("──────────────────────────────────────────────────────────────────");

//            foreach (DataRow productRow in products.Rows)
//            {
//                string productName = (string)productRow["ProductName"];

//                DataRow[] parentRows = productRow.GetParentRows(relation);

//                if (parentRows.Length > 0)
//                {
//                    DataRow categoryRow = parentRows[0];
//                    string categoryName = (string)categoryRow["CategoryName"];
//                    string categoryDescription = (string)categoryRow["Описание"];

//                    Console.WriteLine($"{productName,-25} | {categoryName,-15} | {categoryDescription}");
//                }
//                else
//                {
//                    Console.WriteLine($"{productName,-25} | Нет категории     | Нет описания");
//                }
//            }
//        }

//        // 5. Обработка случаев, когда товар не имеет родительской категории
//        static void CheckForOrphanedProducts(DataSet ds)
//        {
//            DataTable products = ds.Tables["Товары"];
//            DataRelation relation = ds.Relations["CategoryProducts"];

//            Console.WriteLine("Потерянные товары (без родительской категории):");

//            foreach (DataRow productRow in products.Rows)
//            {
//                DataRow[] parentRows = productRow.GetParentRows(relation);

//                if (parentRows.Length == 0)
//                {
//                    Console.WriteLine($"\tТовар: {productRow["ProductName"]} (ID: {productRow["ProductID"]})");
//                }
//            }
//        }

//        // 6. Демонстрация разницы между GetParentRows() для измененной и удаленной строки
//        static void DemonstrateGetParentRowsDifference(DataSet ds)
//        {
//            DataTable products = ds.Tables["Товары"];
//            DataRelation relation = ds.Relations["CategoryProducts"];

//            // Изменение существующей строки
//            DataRow productRow = products.Rows.Find(1);
//            if (productRow != null)
//            {
//                Console.WriteLine("Изменённая строка:");
//                productRow["Price"] = 25990.00m;
//                DataRow[] parentRows = productRow.GetParentRows(relation);
//                if (parentRows.Length > 0)
//                {
//                    Console.WriteLine($"\tТовар: {productRow["ProductName"]}, Категория: {parentRows[0]["CategoryName"]}");
//                }
//            }

//            // Удаление строки
//            DataRow productRowToDelete = products.Rows.Find(10);
//            if (productRowToDelete != null)
//            {
//                Console.WriteLine("\nУдаленная строка:");
//                productRowToDelete.Delete();
//                DataRow[] parentRowsDeleted = productRowToDelete.GetParentRows(relation, DataRowVersion.Original);
//                if (parentRowsDeleted.Length > 0)
//                {
//                    Console.WriteLine($"\tТовар: {productRowToDelete["ProductName", DataRowVersion.Original]}, Категория: {parentRowsDeleted[0]["CategoryName"]}");
//                }
//                else
//                {
//                    Console.WriteLine("\tНет родительской категории для удаленной строки.");
//                }
//            }
//        }
//    }
//}


////Задание 4: Создание отношений "к себе" для иерархических данных
//using System;
//using System.Data;
//using System.Collections.Generic;

//namespace EmployeeHierarchy
//{
//    class Program
//    {
//        static void Main()
//        {
//            // Создаём DataSet и таблицу сотрудников
//            DataSet ds = CreateDataSet();

//            // Заполняем таблицу тестовыми данными
//            FillTestData(ds);

//            // Создаём самоссылающееся отношение
//            CreateSelfReferencingRelation(ds);

//            Console.WriteLine("=== ИЕРАРХИЯ СОТРУДНИКОВ ===\n");

//            // 1. Вывод иерархии сотрудников
//            Console.WriteLine("1. ИЕРАРХИЯ СОТРУДНИКОВ:");
//            Console.WriteLine("=====================================");
//            PrintEmployeeHierarchy(ds);
//            Console.WriteLine();

//            // 2. Вывод руководителя для каждого сотрудника
//            Console.WriteLine("2. РУКОВОДИТЕЛИ СОТРУДНИКОВ:");
//            Console.WriteLine("=====================================");
//            PrintEmployeeManagers(ds);
//            Console.WriteLine();

//            // 3. Вывод подчинённых для каждого менеджера
//            Console.WriteLine("3. ПОДЧИНЁННЫЕ МЕНЕДЖЕРОВ:");
//            Console.WriteLine("=====================================");
//            PrintManagerSubordinates(ds);
//            Console.WriteLine();

//            // 4. Подсчёт уровня иерархии для каждого сотрудника
//            Console.WriteLine("4. УРОВНИ ИЕРАРХИИ СОТРУДНИКОВ:");
//            Console.WriteLine("=====================================");
//            PrintEmployeeHierarchyLevels(ds);
//            Console.WriteLine();

//            // 5. Проверка на циклические ссылки
//            Console.WriteLine("5. ПРОВЕРКА НА ЦИКЛИЧЕСКИЕ ССЫЛКИ:");
//            Console.WriteLine("=====================================");
//            CheckForCyclicReferences(ds);
//        }

//        // Создание DataSet с таблицей сотрудников
//        static DataSet CreateDataSet()
//        {
//            DataSet ds = new DataSet("CompanyDataSet");

//            // Таблица Сотрудники
//            DataTable employees = new DataTable("Сотрудники");
//            employees.Columns.Add("EmployeeID", typeof(int));
//            employees.Columns.Add("ИмяСотрудника", typeof(string));
//            employees.Columns.Add("Отдел", typeof(string));
//            employees.Columns.Add("Зарплата", typeof(decimal));
//            employees.Columns.Add("ManagerID", typeof(int)).AllowDBNull = true; // Может быть NULL для главы компании

//            employees.PrimaryKey = new DataColumn[] { employees.Columns["EmployeeID"] };

//            ds.Tables.Add(employees);

//            return ds;
//        }

//        // Заполнение тестовыми данными
//        static void FillTestData(DataSet ds)
//        {
//            DataTable employees = ds.Tables["Сотрудники"];

//            // Добавляем сотрудников
//            employees.Rows.Add(1, "Иван Иванов", "Руководство", 150000, DBNull.Value); // Глава компании
//            employees.Rows.Add(2, "Мария Петрова", "IT", 120000, 1); // Руководитель IT
//            employees.Rows.Add(3, "Петр Сидоров", "HR", 110000, 1); // Руководитель HR
//            employees.Rows.Add(4, "Анна Кузнецова", "IT", 90000, 2); // Сотрудник IT
//            employees.Rows.Add(5, "Сергей Васильев", "IT", 85000, 2); // Сотрудник IT
//            employees.Rows.Add(6, "Ольга Николаева", "HR", 80000, 3); // Сотрудник HR
//            employees.Rows.Add(7, "Алексей Смирнов", "Финансы", 100000, 1); // Руководитель Финансов
//            employees.Rows.Add(8, "Елена Козлова", "Финансы", 75000, 7); // Сотрудник Финансов
//            employees.Rows.Add(9, "Дмитрий Морозов", "IT", 95000, 2); // Сотрудник IT
//            // employees.Rows.Add(10, "Тест Тестов", "IT", 95000, 10); // Циклическая ссылка (закомментировано)
//        }

//        // Создание самоссылающегося отношения
//        static void CreateSelfReferencingRelation(DataSet ds)
//        {
//            DataTable employees = ds.Tables["Сотрудники"];

//            try
//            {
//                DataRelation relation = new DataRelation(
//                    "EmployeeHierarchy",
//                    employees.Columns["EmployeeID"],
//                    employees.Columns["ManagerID"],
//                    false); // createConstraints = false, чтобы избежать ошибок при циклических ссылках

//                ds.Relations.Add(relation);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Ошибка при создании отношения: {ex.Message}");
//            }
//        }

//        // 1. Вывод иерархии сотрудников
//        static void PrintEmployeeHierarchy(DataSet ds)
//        {
//            DataTable employees = ds.Tables["Сотрудники"];
//            DataRelation relation = ds.Relations["EmployeeHierarchy"];

//            // Начинаем с главы компании (ManagerID = NULL)
//            DataRow[] ceoRows = employees.Select("ManagerID IS NULL");

//            if (ceoRows.Length == 0)
//            {
//                Console.WriteLine("Глава компании не найден.");
//                return;
//            }

//            DataRow ceoRow = ceoRows[0];
//            PrintEmployeeHierarchyRecursive(ceoRow, relation, 0);
//        }

//        // Рекурсивный метод для вывода иерархии
//        static void PrintEmployeeHierarchyRecursive(DataRow employeeRow, DataRelation relation, int level)
//        {
//            string indent = new string(' ', level * 4);
//            Console.WriteLine($"{indent}→ {employeeRow["ИмяСотрудника"]} ({employeeRow["Отдел"]}), Зарплата: {employeeRow["Зарплата"]:C}");

//            // Получаем подчинённых сотрудников
//            DataRow[] subordinateRows = employeeRow.GetChildRows(relation);

//            foreach (DataRow subordinateRow in subordinateRows)
//            {
//                PrintEmployeeHierarchyRecursive(subordinateRow, relation, level + 1);
//            }
//        }

//        // 2. Вывод руководителя для каждого сотрудника
//        static void PrintEmployeeManagers(DataSet ds)
//        {
//            DataTable employees = ds.Tables["Сотрудники"];
//            DataRelation relation = ds.Relations["EmployeeHierarchy"];

//            foreach (DataRow employeeRow in employees.Rows)
//            {
//                string employeeName = (string)employeeRow["ИмяСотрудника"];

//                // Получаем родительскую строку (руководителя)
//                DataRow[] managerRows = employeeRow.GetParentRows(relation);

//                if (managerRows.Length > 0)
//                {
//                    DataRow managerRow = managerRows[0];
//                    string managerName = (string)managerRow["ИмяСотрудника"];
//                    Console.WriteLine($"{employeeName} → Руководитель: {managerName}");
//                }
//                else
//                {
//                    Console.WriteLine($"{employeeName} → Глава компании (нет руководителя)");
//                }
//            }
//        }

//        // 3. Вывод подчинённых для каждого менеджера
//        static void PrintManagerSubordinates(DataSet ds)
//        {
//            DataTable employees = ds.Tables["Сотрудники"];
//            DataRelation relation = ds.Relations["EmployeeHierarchy"];

//            foreach (DataRow employeeRow in employees.Rows)
//            {
//                string employeeName = (string)employeeRow["ИмяСотрудника"];

//                // Получаем дочерние строки (подчинённые)
//                DataRow[] subordinateRows = employeeRow.GetChildRows(relation);

//                if (subordinateRows.Length > 0)
//                {
//                    Console.WriteLine($"{employeeName} (Подчинённые: {subordinateRows.Length})");
//                    foreach (DataRow subordinateRow in subordinateRows)
//                    {
//                        string subordinateName = (string)subordinateRow["ИмяСотрудника"];
//                        Console.WriteLine($"\t→ {subordinateName}");
//                    }
//                }
//            }
//        }

//        // 4. Подсчёт уровня иерархии для каждого сотрудника
//        static void PrintEmployeeHierarchyLevels(DataSet ds)
//        {
//            DataTable employees = ds.Tables["Сотрудники"];
//            DataRelation relation = ds.Relations["EmployeeHierarchy"];

//            foreach (DataRow employeeRow in employees.Rows)
//            {
//                string employeeName = (string)employeeRow["ИмяСотрудника"];
//                int level = GetEmployeeLevel(employeeRow, relation);
//                Console.WriteLine($"{employeeName} → Уровень: {level}");
//            }
//        }

//        // Рекурсивный метод для подсчёта уровня иерархии
//        static int GetEmployeeLevel(DataRow employeeRow, DataRelation relation)
//        {
//            // Получаем родительскую строку (руководителя)
//            DataRow[] managerRows = employeeRow.GetParentRows(relation);

//            if (managerRows.Length == 0)
//            {
//                return 0; // Глава компании
//            }

//            DataRow managerRow = managerRows[0];
//            return GetEmployeeLevel(managerRow, relation) + 1;
//        }

//        // 5. Проверка на циклические ссылки
//        static void CheckForCyclicReferences(DataSet ds)
//        {
//            DataTable employees = ds.Tables["Сотрудники"];
//            DataRelation relation = ds.Relations["EmployeeHierarchy"];

//            bool hasCyclicReferences = false;

//            foreach (DataRow employeeRow in employees.Rows)
//            {
//                if (IsCyclicReference(employeeRow, relation, new HashSet<int>()))
//                {
//                    hasCyclicReferences = true;
//                    string employeeName = (string)employeeRow["ИмяСотрудника"];
//                    Console.WriteLine($"Обнаружена циклическая ссылка для сотрудника: {employeeName}");
//                }
//            }

//            if (!hasCyclicReferences)
//            {
//                Console.WriteLine("Циклические ссылки не обнаружены.");
//            }
//        }

//        // Рекурсивный метод для проверки циклических ссылок
//        static bool IsCyclicReference(DataRow employeeRow, DataRelation relation, HashSet<int> visitedEmployees)
//        {
//            int employeeID = (int)employeeRow["EmployeeID"];

//            // Если сотрудник уже посещён, то это циклическая ссылка
//            if (visitedEmployees.Contains(employeeID))
//            {
//                return true;
//            }

//            visitedEmployees.Add(employeeID);

//            // Получаем родительскую строку (руководителя)
//            DataRow[] managerRows = employeeRow.GetParentRows(relation);

//            if (managerRows.Length > 0)
//            {
//                DataRow managerRow = managerRows[0];
//                return IsCyclicReference(managerRow, relation, visitedEmployees);
//            }

//            return false;
//        }
//    }
//}


////Задание 5: Получение данных из отношений "сам к себе" с фильтрацией
//using System;
//using System.Data;
//using System.Collections.Generic;
//using System.Linq;

//namespace EmployeeHierarchyAdvanced
//{
//    class Program
//    {
//        static void Main()
//        {
//            // Создаём DataSet и таблицу сотрудников
//            DataSet ds = CreateDataSet();

//            // Заполняем таблицу тестовыми данными
//            FillTestData(ds);

//            // Создаём самоссылающееся отношение
//            CreateSelfReferencingRelation(ds);

//            Console.WriteLine("=== РАБОТА С ФИЛЬТРАЦИЕЙ И ПОИСКОМ В ИЕРАРХИИ ===\n");

//            // 1. Поиск всех подчинённых конкретного сотрудника (прямые подчинённые)
//            Console.WriteLine("1. ПРЯМЫЕ ПОДЧИНЁННЫЕ СОТРУДНИКА 'Мария Петрова':");
//            Console.WriteLine("=====================================");
//            FindDirectSubordinates(ds, "Мария Петрова");
//            Console.WriteLine();

//            // 2. Получение всей цепочки руководства для сотрудника
//            Console.WriteLine("2. ЦЕПОЧКА РУКОВОДСТВА ДЛЯ СОТРУДНИКА 'Анна Кузнецова':");
//            Console.WriteLine("=====================================");
//            PrintManagementChain(ds, "Анна Кузнецова");
//            Console.WriteLine();

//            // 3. Поиск всех сотрудников определенного уровня иерархии
//            Console.WriteLine("3. ВСЕ СОТРУДНИКИ 2 УРОВНЯ (руководители отделов):");
//            Console.WriteLine("=====================================");
//            FindEmployeesByLevel(ds, 2);
//            Console.WriteLine();

//            // 4. Отчёт со статистикой по иерархии
//            Console.WriteLine("4. СТАТИСТИКА ПО ИЕРАРХИИ:");
//            Console.WriteLine("=====================================");
//            PrintHierarchyStatistics(ds);
//            Console.WriteLine();

//            // 5. Поиск "коллег" (сотрудников с одним руководителем)
//            Console.WriteLine("5. КОЛЛЕГИ СОТРУДНИКА 'Анна Кузнецова':");
//            Console.WriteLine("=====================================");
//            FindColleagues(ds, "Анна Кузнецова");
//            Console.WriteLine();

//            // 6. Получение информации о всех вышестоящих сотрудниках
//            Console.WriteLine("6. ВСЕ ВЫШЕСТОЯЩИЕ СОТРУДНИКИ ДЛЯ 'Анна Кузнецова':");
//            Console.WriteLine("=====================================");
//            PrintAllSuperiors(ds, "Анна Кузнецова");
//            Console.WriteLine();
//        }

//        // Создание DataSet с таблицей сотрудников
//        static DataSet CreateDataSet()
//        {
//            DataSet ds = new DataSet("CompanyDataSet");

//            // Таблица Сотрудники
//            DataTable employees = new DataTable("Сотрудники");
//            employees.Columns.Add("EmployeeID", typeof(int));
//            employees.Columns.Add("ИмяСотрудника", typeof(string));
//            employees.Columns.Add("Отдел", typeof(string));
//            employees.Columns.Add("Зарплата", typeof(decimal));
//            employees.Columns.Add("ManagerID", typeof(int)).AllowDBNull = true; // Может быть NULL для главы компании

//            employees.PrimaryKey = new DataColumn[] { employees.Columns["EmployeeID"] };

//            ds.Tables.Add(employees);

//            return ds;
//        }

//        // Заполнение тестовыми данными
//        static void FillTestData(DataSet ds)
//        {
//            DataTable employees = ds.Tables["Сотрудники"];

//            // Добавляем сотрудников
//            employees.Rows.Add(1, "Иван Иванов", "Руководство", 150000, DBNull.Value); // Глава компании
//            employees.Rows.Add(2, "Мария Петрова", "IT", 120000, 1); // Руководитель IT
//            employees.Rows.Add(3, "Петр Сидоров", "HR", 110000, 1); // Руководитель HR
//            employees.Rows.Add(4, "Анна Кузнецова", "IT", 90000, 2); // Сотрудник IT
//            employees.Rows.Add(5, "Сергей Васильев", "IT", 85000, 2); // Сотрудник IT
//            employees.Rows.Add(6, "Ольга Николаева", "HR", 80000, 3); // Сотрудник HR
//            employees.Rows.Add(7, "Алексей Смирнов", "Финансы", 100000, 1); // Руководитель Финансов
//            employees.Rows.Add(8, "Елена Козлова", "Финансы", 75000, 7); // Сотрудник Финансов
//            employees.Rows.Add(9, "Дмитрий Морозов", "IT", 95000, 2); // Сотрудник IT
//            employees.Rows.Add(10, "Ирина Волкова", "Маркетинг", 105000, 1); // Руководитель Маркетинга
//            employees.Rows.Add(11, "Андрей Захаров", "Маркетинг", 78000, 10); // Сотрудник Маркетинга
//        }

//        // Создание самоссылающегося отношения
//        static void CreateSelfReferencingRelation(DataSet ds)
//        {
//            DataTable employees = ds.Tables["Сотрудники"];

//            try
//            {
//                DataRelation relation = new DataRelation(
//                    "EmployeeHierarchy",
//                    employees.Columns["EmployeeID"],
//                    employees.Columns["ManagerID"],
//                    false); // createConstraints = false, чтобы избежать ошибок при циклических ссылках

//                ds.Relations.Add(relation);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Ошибка при создании отношения: {ex.Message}");
//            }
//        }

//        // 1. Поиск всех подчинённых конкретного сотрудника (прямые подчинённые)
//        static void FindDirectSubordinates(DataSet ds, string employeeName)
//        {
//            DataTable employees = ds.Tables["Сотрудники"];
//            DataRelation relation = ds.Relations["EmployeeHierarchy"];

//            // Находим сотрудника по имени
//            DataRow[] employeeRows = employees.Select($"ИмяСотрудника = '{employeeName}'");

//            if (employeeRows.Length == 0)
//            {
//                Console.WriteLine($"Сотрудник '{employeeName}' не найден.");
//                return;
//            }

//            DataRow employeeRow = employeeRows[0];
//            Console.WriteLine($"Подчинённые сотрудника: {employeeName}");

//            // Получаем дочерние строки (прямые подчинённые)
//            DataRow[] subordinateRows = employeeRow.GetChildRows(relation);

//            if (subordinateRows.Length == 0)
//            {
//                Console.WriteLine("\tНет прямых подчинённых.");
//                return;
//            }

//            foreach (DataRow subordinateRow in subordinateRows)
//            {
//                string subordinateName = (string)subordinateRow["ИмяСотрудника"];
//                Console.WriteLine($"\t→ {subordinateName}");
//            }
//        }

//        // 2. Получение всей цепочки руководства для сотрудника
//        static void PrintManagementChain(DataSet ds, string employeeName)
//        {
//            DataTable employees = ds.Tables["Сотрудники"];
//            DataRelation relation = ds.Relations["EmployeeHierarchy"];

//            // Находим сотрудника по имени
//            DataRow[] employeeRows = employees.Select($"ИмяСотрудника = '{employeeName}'");

//            if (employeeRows.Length == 0)
//            {
//                Console.WriteLine($"Сотрудник '{employeeName}' не найден.");
//                return;
//            }

//            DataRow employeeRow = employeeRows[0];
//            Console.WriteLine($"Цепочка руководства для сотрудника: {employeeName}");

//            // Получаем цепочку руководства
//            List<DataRow> managementChain = GetManagementChain(employeeRow, relation);

//            if (managementChain.Count == 0)
//            {
//                Console.WriteLine("\tСотрудник является главой компании.");
//                return;
//            }

//            for (int i = 0; i < managementChain.Count; i++)
//            {
//                string managerName = (string)managementChain[i]["ИмяСотрудника"];
//                Console.WriteLine($"\t{i + 1}. {managerName}");
//            }
//        }

//        // Рекурсивный метод для получения цепочки руководства
//        static List<DataRow> GetManagementChain(DataRow employeeRow, DataRelation relation)
//        {
//            List<DataRow> chain = new List<DataRow>();

//            // Получаем родительскую строку (руководителя)
//            DataRow[] managerRows = employeeRow.GetParentRows(relation);

//            if (managerRows.Length > 0)
//            {
//                DataRow managerRow = managerRows[0];
//                chain.Add(managerRow);
//                chain.AddRange(GetManagementChain(managerRow, relation));
//            }

//            return chain;
//        }

//        // 3. Поиск всех сотрудников определенного уровня иерархии
//        static void FindEmployeesByLevel(DataSet ds, int level)
//        {
//            DataTable employees = ds.Tables["Сотрудники"];
//            DataRelation relation = ds.Relations["EmployeeHierarchy"];

//            Console.WriteLine($"Сотрудники {level} уровня:");

//            foreach (DataRow employeeRow in employees.Rows)
//            {
//                int employeeLevel = GetEmployeeLevel(employeeRow, relation);

//                if (employeeLevel == level)
//                {
//                    string employeeName = (string)employeeRow["ИмяСотрудника"];
//                    Console.WriteLine($"\t{employeeName}");
//                }
//            }
//        }

//        // Рекурсивный метод для подсчёта уровня иерархии
//        static int GetEmployeeLevel(DataRow employeeRow, DataRelation relation)
//        {
//            // Получаем родительскую строку (руководителя)
//            DataRow[] managerRows = employeeRow.GetParentRows(relation);

//            if (managerRows.Length == 0)
//            {
//                return 0; // Глава компании
//            }

//            DataRow managerRow = managerRows[0];
//            return GetEmployeeLevel(managerRow, relation) + 1;
//        }

//        // 4. Отчёт со статистикой по иерархии
//        static void PrintHierarchyStatistics(DataSet ds)
//        {
//            DataTable employees = ds.Tables["Сотрудники"];
//            DataRelation relation = ds.Relations["EmployeeHierarchy"];

//            // Подсчёт количества уровней
//            int maxLevel = 0;
//            foreach (DataRow employeeRow in employees.Rows)
//            {
//                int level = GetEmployeeLevel(employeeRow, relation);
//                if (level > maxLevel)
//                {
//                    maxLevel = level;
//                }
//            }

//            Console.WriteLine($"Количество уровней в иерархии: {maxLevel + 1}");

//            // Подсчёт количества руководителей по уровням
//            Dictionary<int, int> managersByLevel = new Dictionary<int, int>();
//            Dictionary<int, int> employeesByLevel = new Dictionary<int, int>();
//            Dictionary<int, List<int>> subordinatesCountByLevel = new Dictionary<int, List<int>>();

//            for (int i = 0; i <= maxLevel; i++)
//            {
//                managersByLevel[i] = 0;
//                employeesByLevel[i] = 0;
//                subordinatesCountByLevel[i] = new List<int>();
//            }

//            foreach (DataRow employeeRow in employees.Rows)
//            {
//                int level = GetEmployeeLevel(employeeRow, relation);
//                employeesByLevel[level]++;

//                // Получаем дочерние строки (подчинённые)
//                DataRow[] subordinateRows = employeeRow.GetChildRows(relation);
//                subordinatesCountByLevel[level].Add(subordinateRows.Length);

//                if (subordinateRows.Length > 0)
//                {
//                    managersByLevel[level]++;
//                }
//            }

//            Console.WriteLine("\nСтатистика по уровням:");
//            Console.WriteLine("Уровень | Количество сотрудников | Количество руководителей | Среднее количество подчинённых");

//            for (int i = 0; i <= maxLevel; i++)
//            {
//                double avgSubordinates = subordinatesCountByLevel[i].Count > 0 ?
//                    subordinatesCountByLevel[i].Average() : 0;

//                Console.WriteLine($"{i,6} | {employeesByLevel[i],20} | {managersByLevel[i],22} | {avgSubordinates,28:F2}");
//            }
//        }

//        // 5. Поиск "коллег" (сотрудников с одним руководителем)
//        static void FindColleagues(DataSet ds, string employeeName)
//        {
//            DataTable employees = ds.Tables["Сотрудники"];
//            DataRelation relation = ds.Relations["EmployeeHierarchy"];

//            // Находим сотрудника по имени
//            DataRow[] employeeRows = employees.Select($"ИмяСотрудника = '{employeeName}'");

//            if (employeeRows.Length == 0)
//            {
//                Console.WriteLine($"Сотрудник '{employeeName}' не найден.");
//                return;
//            }

//            DataRow employeeRow = employeeRows[0];

//            // Получаем родительскую строку (руководителя)
//            DataRow[] managerRows = employeeRow.GetParentRows(relation);

//            if (managerRows.Length == 0)
//            {
//                Console.WriteLine($"{employeeName} является главой компании и не имеет коллег.");
//                return;
//            }

//            DataRow managerRow = managerRows[0];
//            string managerName = (string)managerRow["ИмяСотрудника"];

//            Console.WriteLine($"Коллеги сотрудника {employeeName} (руководитель: {managerName}):");

//            // Получаем дочерние строки руководителя (все подчинённые)
//            DataRow[] subordinateRows = managerRow.GetChildRows(relation);

//            foreach (DataRow subordinateRow in subordinateRows)
//            {
//                string subordinateName = (string)subordinateRow["ИмяСотрудника"];
//                if (subordinateName != employeeName)
//                {
//                    Console.WriteLine($"\t→ {subordinateName}");
//                }
//            }
//        }

//        // 6. Получение информации о всех вышестоящих сотрудниках
//        static void PrintAllSuperiors(DataSet ds, string employeeName)
//        {
//            DataTable employees = ds.Tables["Сотрудники"];
//            DataRelation relation = ds.Relations["EmployeeHierarchy"];

//            // Находим сотрудника по имени
//            DataRow[] employeeRows = employees.Select($"ИмяСотрудника = '{employeeName}'");

//            if (employeeRows.Length == 0)
//            {
//                Console.WriteLine($"Сотрудник '{employeeName}' не найден.");
//                return;
//            }

//            DataRow employeeRow = employeeRows[0];
//            Console.WriteLine($"Все вышестоящие сотрудники для {employeeName}:");

//            // Получаем всех вышестоящих сотрудников
//            List<DataRow> superiors = GetAllSuperiors(employeeRow, relation);

//            if (superiors.Count == 0)
//            {
//                Console.WriteLine("\tСотрудник является главой компании.");
//                return;
//            }

//            for (int i = 0; i < superiors.Count; i++)
//            {
//                string superiorName = (string)superiors[i]["ИмяСотрудника"];
//                Console.WriteLine($"\t{i + 1}. {superiorName}");
//            }
//        }

//        // Рекурсивный метод для получения всех вышестоящих сотрудников
//        static List<DataRow> GetAllSuperiors(DataRow employeeRow, DataRelation relation)
//        {
//            List<DataRow> superiors = new List<DataRow>();

//            // Получаем родительскую строку (руководителя)
//            DataRow[] managerRows = employeeRow.GetParentRows(relation);

//            if (managerRows.Length > 0)
//            {
//                DataRow managerRow = managerRows[0];
//                superiors.Add(managerRow);
//                superiors.AddRange(GetAllSuperiors(managerRow, relation));
//            }

//            return superiors;
//        }
//    }
//}


////Задание 6: Реализация отношений многими-ко-многим через промежуточную таблицу
//using System;
//using System.Data;
//using System.Collections.Generic;

//namespace ManyToManyRelationship
//{
//    class Program
//    {
//        static void Main()
//        {
//            // Создаём DataSet и таблицы
//            DataSet ds = CreateDataSet();

//            // Заполняем таблицы тестовыми данными
//            FillTestData(ds);

//            // Создаём отношения DataRelation
//            CreateRelations(ds);

//            Console.WriteLine("=== РАБОТА С ОТНОШЕНИЕМ МНОГИЕ-КО-МНОГИМ ===\n");

//            // 1. Получение всех курсов для конкретного студента
//            Console.WriteLine("1. КУРСЫ СТУДЕНТА 'Иван Петров':");
//            Console.WriteLine("=====================================");
//            GetStudentCourses(ds, "Иван Петров");
//            Console.WriteLine();

//            // 2. Получение всех студентов на конкретном курсе
//            Console.WriteLine("2. СТУДЕНТЫ НА КУРСЕ 'C# Programming':");
//            Console.WriteLine("=====================================");
//            GetCourseStudents(ds, "C# Programming");
//            Console.WriteLine();

//            // 3. Статистика: количество студентов на каждом курсе
//            Console.WriteLine("3. КОЛИЧЕСТВО СТУДЕНТОВ НА КАЖДОМ КУРСЕ:");
//            Console.WriteLine("=====================================");
//            PrintStudentsPerCourseStatistics(ds);
//            Console.WriteLine();

//            // 4. Статистика: количество курсов для каждого студента
//            Console.WriteLine("4. КОЛИЧЕСТВО КУРСОВ ДЛЯ КАЖДОГО СТУДЕНТА:");
//            Console.WriteLine("=====================================");
//            PrintCoursesPerStudentStatistics(ds);
//            Console.WriteLine();

//            // 5. Полная информация о регистрациях
//            Console.WriteLine("5. ПОЛНАЯ ИНФОРМАЦИЯ О РЕГИСТРАЦИЯХ:");
//            Console.WriteLine("=====================================");
//            PrintAllRegistrations(ds);
//        }

//        // Создание DataSet с таблицами
//        static DataSet CreateDataSet()
//        {
//            DataSet ds = new DataSet("UniversityDB");

//            // Таблица Студенты
//            DataTable students = new DataTable("Студенты");
//            students.Columns.Add("StudentID", typeof(int));
//            students.Columns.Add("StudentName", typeof(string));
//            students.Columns.Add("Email", typeof(string));
//            students.PrimaryKey = new DataColumn[] { students.Columns["StudentID"] };

//            // Таблица Курсы
//            DataTable courses = new DataTable("Курсы");
//            courses.Columns.Add("CourseID", typeof(string));
//            courses.Columns.Add("CourseName", typeof(string));
//            courses.Columns.Add("Instructor", typeof(string));
//            courses.PrimaryKey = new DataColumn[] { courses.Columns["CourseID"] };

//            // Таблица Регистрация (промежуточная)
//            DataTable registration = new DataTable("Регистрация");
//            registration.Columns.Add("RegistrationID", typeof(int));
//            registration.Columns.Add("StudentID", typeof(int));
//            registration.Columns.Add("CourseID", typeof(string));
//            registration.Columns.Add("EnrollmentDate", typeof(DateTime));
//            registration.Columns.Add("Grade", typeof(double));
//            registration.PrimaryKey = new DataColumn[] { registration.Columns["RegistrationID"] };

//            ds.Tables.Add(students);
//            ds.Tables.Add(courses);
//            ds.Tables.Add(registration);

//            return ds;
//        }

//        // Заполнение тестовыми данными
//        static void FillTestData(DataSet ds)
//        {
//            DataTable students = ds.Tables["Студенты"];
//            DataTable courses = ds.Tables["Курсы"];
//            DataTable registration = ds.Tables["Регистрация"];

//            // Добавляем студентов
//            students.Rows.Add(101, "Иван Петров", "ivan@example.com");
//            students.Rows.Add(102, "Мария Сидорова", "maria@example.com");
//            students.Rows.Add(103, "Петр Иванов", "petr@example.com");
//            students.Rows.Add(104, "Анна Смирнова", "anna@example.com");

//            // Добавляем курсы
//            courses.Rows.Add("C001", "C# Programming", "Дмитрий Волков");
//            courses.Rows.Add("C002", "Database Design", "Светлана Морозова");
//            courses.Rows.Add("C003", "Web Development", "Алексей Новиков");
//            courses.Rows.Add("C004", "OOP Principles", "Петр Сергеев");

//            // Добавляем регистрации с оценками
//            registration.Rows.Add(1, 101, "C001", new DateTime(2024, 01, 15), 4.5);
//            registration.Rows.Add(2, 101, "C002", new DateTime(2024, 01, 20), 3.8);
//            registration.Rows.Add(3, 101, "C004", new DateTime(2024, 02, 10), 4.9);
//            registration.Rows.Add(4, 102, "C001", new DateTime(2024, 01, 15), 4.8);
//            registration.Rows.Add(5, 102, "C003", new DateTime(2024, 02, 05), 4.2);
//            registration.Rows.Add(6, 103, "C002", new DateTime(2024, 01, 20), 3.5);
//            registration.Rows.Add(7, 103, "C003", new DateTime(2024, 02, 05), 4.0);
//            registration.Rows.Add(8, 103, "C004", new DateTime(2024, 02, 10), 4.7);
//            registration.Rows.Add(9, 104, "C001", new DateTime(2024, 01, 15), 4.6);
//            registration.Rows.Add(10, 104, "C002", new DateTime(2024, 01, 20), 4.3);
//            registration.Rows.Add(11, 104, "C003", new DateTime(2024, 02, 05), 4.1);
//        }

//        // Создание отношений DataRelation
//        static void CreateRelations(DataSet ds)
//        {
//            DataTable students = ds.Tables["Студенты"];
//            DataTable courses = ds.Tables["Курсы"];
//            DataTable registration = ds.Tables["Регистрация"];

//            try
//            {
//                // Отношение: Студенты → Регистрация (один студент → много регистраций)
//                DataRelation studentRegistrationRelation = new DataRelation(
//                    "Students_Registrations",
//                    students.Columns["StudentID"],
//                    registration.Columns["StudentID"],
//                    true);

//                // Отношение: Курсы → Регистрация (один курс → много регистраций)
//                DataRelation courseRegistrationRelation = new DataRelation(
//                    "Courses_Registrations",
//                    courses.Columns["CourseID"],
//                    registration.Columns["CourseID"],
//                    true);

//                ds.Relations.Add(studentRegistrationRelation);
//                ds.Relations.Add(courseRegistrationRelation);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Ошибка при создании отношений: {ex.Message}");
//            }
//        }

//        // 1. Получение всех курсов для конкретного студента
//        static void GetStudentCourses(DataSet ds, string studentName)
//        {
//            DataTable students = ds.Tables["Студенты"];
//            DataTable courses = ds.Tables["Курсы"];
//            DataTable registration = ds.Tables["Регистрация"];
//            DataRelation studentRegistrationRelation = ds.Relations["Students_Registrations"];
//            DataRelation courseRegistrationRelation = ds.Relations["Courses_Registrations"];

//            // Находим студента по имени
//            DataRow[] studentRows = students.Select($"StudentName = '{studentName}'");

//            if (studentRows.Length == 0)
//            {
//                Console.WriteLine($"Студент '{studentName}' не найден.");
//                return;
//            }

//            DataRow studentRow = studentRows[0];
//            Console.WriteLine($"Курсы студента: {studentName}");

//            // Получаем все регистрации студента
//            DataRow[] registrationRows = studentRow.GetChildRows(studentRegistrationRelation);

//            if (registrationRows.Length == 0)
//            {
//                Console.WriteLine("\tСтудент не записан ни на один курс.");
//                return;
//            }

//            foreach (DataRow regRow in registrationRows)
//            {
//                // Получаем информацию о курсе
//                DataRow[] courseRows = regRow.GetParentRows(courseRegistrationRelation);

//                if (courseRows.Length > 0)
//                {
//                    DataRow courseRow = courseRows[0];
//                    DateTime enrollmentDate = (DateTime)regRow["EnrollmentDate"];
//                    double grade = (double)regRow["Grade"];

//                    Console.WriteLine($"\t• {courseRow["CourseName"]}");
//                    Console.WriteLine($"\t  Преподаватель: {courseRow["Instructor"]}");
//                    Console.WriteLine($"\t  Дата регистрации: {enrollmentDate:dd.MM.yyyy}");
//                    Console.WriteLine($"\t  Оценка: {grade:F1}");
//                    Console.WriteLine();
//                }
//            }
//        }

//        // 2. Получение всех студентов на конкретном курсе
//        static void GetCourseStudents(DataSet ds, string courseName)
//        {
//            DataTable students = ds.Tables["Студенты"];
//            DataTable courses = ds.Tables["Курсы"];
//            DataTable registration = ds.Tables["Регистрация"];
//            DataRelation studentRegistrationRelation = ds.Relations["Students_Registrations"];
//            DataRelation courseRegistrationRelation = ds.Relations["Courses_Registrations"];

//            // Находим курс по имени
//            DataRow[] courseRows = courses.Select($"CourseName = '{courseName}'");

//            if (courseRows.Length == 0)
//            {
//                Console.WriteLine($"Курс '{courseName}' не найден.");
//                return;
//            }

//            DataRow courseRow = courseRows[0];
//            Console.WriteLine($"Студенты на курсе: {courseName}");

//            // Получаем все регистрации курса
//            DataRow[] registrationRows = courseRow.GetChildRows(courseRegistrationRelation);

//            if (registrationRows.Length == 0)
//            {
//                Console.WriteLine("\tНа этом курсе нет студентов.");
//                return;
//            }

//            foreach (DataRow regRow in registrationRows)
//            {
//                // Получаем информацию о студенте
//                DataRow[] studentRows = regRow.GetParentRows(studentRegistrationRelation);

//                if (studentRows.Length > 0)
//                {
//                    DataRow studentRow = studentRows[0];
//                    DateTime enrollmentDate = (DateTime)regRow["EnrollmentDate"];
//                    double grade = (double)regRow["Grade"];

//                    Console.WriteLine($"\t• {studentRow["StudentName"]}");
//                    Console.WriteLine($"\t  Email: {studentRow["Email"]}");
//                    Console.WriteLine($"\t  Дата регистрации: {enrollmentDate:dd.MM.yyyy}");
//                    Console.WriteLine($"\t  Оценка: {grade:F1}");
//                    Console.WriteLine();
//                }
//            }
//        }

//        // 3. Статистика: количество студентов на каждом курсе
//        static void PrintStudentsPerCourseStatistics(DataSet ds)
//        {
//            DataTable courses = ds.Tables["Курсы"];
//            DataTable registration = ds.Tables["Регистрация"];
//            DataRelation courseRegistrationRelation = ds.Relations["Courses_Registrations"];

//            Console.WriteLine("Курс                  | Количество студентов");
//            Console.WriteLine("─────────────────────────────────────────────");

//            foreach (DataRow courseRow in courses.Rows)
//            {
//                string courseName = (string)courseRow["CourseName"];

//                // Получаем все регистрации курса
//                DataRow[] registrationRows = courseRow.GetChildRows(courseRegistrationRelation);

//                Console.WriteLine($"{courseName,-20} | {registrationRows.Length,17}");
//            }
//        }

//        // 4. Статистика: количество курсов для каждого студента
//        static void PrintCoursesPerStudentStatistics(DataSet ds)
//        {
//            DataTable students = ds.Tables["Студенты"];
//            DataTable registration = ds.Tables["Регистрация"];
//            DataRelation studentRegistrationRelation = ds.Relations["Students_Registrations"];

//            Console.WriteLine("Студент                | Количество курсов");
//            Console.WriteLine("──────────────────────────────────────────");

//            foreach (DataRow studentRow in students.Rows)
//            {
//                string studentName = (string)studentRow["StudentName"];

//                // Получаем все регистрации студента
//                DataRow[] registrationRows = studentRow.GetChildRows(studentRegistrationRelation);

//                Console.WriteLine($"{studentName,-20} | {registrationRows.Length,17}");
//            }
//        }

//        // 5. Полная информация о регистрациях
//        static void PrintAllRegistrations(DataSet ds)
//        {
//            DataTable students = ds.Tables["Студенты"];
//            DataTable courses = ds.Tables["Курсы"];
//            DataTable registration = ds.Tables["Регистрация"];
//            DataRelation studentRegistrationRelation = ds.Relations["Students_Registrations"];
//            DataRelation courseRegistrationRelation = ds.Relations["Courses_Registrations"];

//            Console.WriteLine("ID | Студент              | Курс             | Дата         | Оценка");
//            Console.WriteLine("────────────────────────────────────────────────────────────────────");

//            foreach (DataRow regRow in registration.Rows)
//            {
//                int registrationID = (int)regRow["RegistrationID"];
//                DateTime enrollmentDate = (DateTime)regRow["EnrollmentDate"];
//                double grade = (double)regRow["Grade"];

//                // Получаем информацию о студенте
//                DataRow[] studentRows = regRow.GetParentRows(studentRegistrationRelation);
//                string studentName = studentRows.Length > 0 ?
//                    (string)studentRows[0]["StudentName"] : "Неизвестен";

//                // Получаем информацию о курсе
//                DataRow[] courseRows = regRow.GetParentRows(courseRegistrationRelation);
//                string courseName = courseRows.Length > 0 ?
//                    (string)courseRows[0]["CourseName"] : "Неизвестен";

//                Console.WriteLine($"{registrationID,2} | {studentName,-20} | {courseName,-15} | {enrollmentDate:dd.MM.yyyy} | {grade,6:F1}");
//            }
//        }
//    }
//}


////Задание 7: Навигация по изображениям многими людьми в обе стороны.
//using System;
//using System.Data;
//using System.Collections.Generic;
//using System.Linq;

//namespace ManyToManyNavigation
//{
//    class Program
//    {
//        static void Main()
//        {
//            // Создаём DataSet и таблицы
//            DataSet ds = CreateDataSet();

//            // Заполняем таблицы тестовыми данными
//            FillTestData(ds);

//            // Создаём отношения DataRelation
//            CreateRelations(ds);

//            Console.WriteLine("=== НАВИГАЦИЯ ПО ОТНОШЕНИЯМ МНОГИЕ-КО-МНОГИМ ===\n");

//            // 1. Получение всех курсов и оценок для студента
//            Console.WriteLine("1. КУРСЫ И ОЦЕНКИ СТУДЕНТА 'Иван Петров':");
//            Console.WriteLine("=====================================");
//            GetStudentCoursesAndGrades(ds, "Иван Петров");
//            Console.WriteLine();

//            // 2. Получение всех студентов и их оценок для курса
//            Console.WriteLine("2. СТУДЕНТЫ И ОЦЕНКИ НА КУРСЕ 'C# Programming':");
//            Console.WriteLine("=====================================");
//            GetCourseStudentsAndGrades(ds, "C# Programming");
//            Console.WriteLine();

//            // 3. Поиск студентов, учащихся на одних и тех же курсах
//            Console.WriteLine("3. СТУДЕНТЫ, УЧАЩИЕСЯ НА ОДНИХ И ТЕХ ЖЕ КУРСАХ, ЧТО И 'Иван Петров':");
//            Console.WriteLine("=====================================");
//            FindStudentsWithSameCourses(ds, "Иван Петров");
//            Console.WriteLine();

//            // 4. Полная информация о регистрациях
//            Console.WriteLine("4. ПОЛНАЯ ИНФОРМАЦИЯ О РЕГИСТРАЦИЯХ:");
//            Console.WriteLine("=====================================");
//            PrintFullRegistrationInfo(ds);
//            Console.WriteLine();

//            // 5. Средняя оценка для каждого студента
//            Console.WriteLine("5. СРЕДНЯЯ ОЦЕНКА ДЛЯ КАЖДОГО СТУДЕНТА:");
//            Console.WriteLine("=====================================");
//            CalculateStudentAverageGrades(ds);
//            Console.WriteLine();

//            // 6. Средняя оценка студентов по каждому курсу
//            Console.WriteLine("6. СРЕДНЯЯ ОЦЕНКА ПО КУРСАМ:");
//            Console.WriteLine("=====================================");
//            CalculateCourseAverageGrades(ds);
//            Console.WriteLine();

//            // 7. Лучшие студенты (средний рейтинг выше 4.5)
//            Console.WriteLine("7. ЛУЧШИЕ СТУДЕНТЫ (средний рейтинг > 4.5):");
//            Console.WriteLine("=====================================");
//            FindTopStudents(ds, 4.5);
//            Console.WriteLine();
//        }

//        // Создание DataSet с таблицами
//        static DataSet CreateDataSet()
//        {
//            DataSet ds = new DataSet("UniversityDB");

//            // Таблица Студенты
//            DataTable students = new DataTable("Студенты");
//            students.Columns.Add("StudentID", typeof(int));
//            students.Columns.Add("StudentName", typeof(string));
//            students.Columns.Add("Email", typeof(string));
//            students.PrimaryKey = new DataColumn[] { students.Columns["StudentID"] };

//            // Таблица Курсы
//            DataTable courses = new DataTable("Курсы");
//            courses.Columns.Add("CourseID", typeof(string));
//            courses.Columns.Add("CourseName", typeof(string));
//            courses.Columns.Add("Instructor", typeof(string));
//            courses.PrimaryKey = new DataColumn[] { courses.Columns["CourseID"] };

//            // Таблица Регистрация (промежуточная)
//            DataTable registration = new DataTable("Регистрация");
//            registration.Columns.Add("RegistrationID", typeof(int));
//            registration.Columns.Add("StudentID", typeof(int));
//            registration.Columns.Add("CourseID", typeof(string));
//            registration.Columns.Add("EnrollmentDate", typeof(DateTime));
//            registration.Columns.Add("Grade", typeof(double)).AllowDBNull = true; // Оценка может быть NULL
//            registration.PrimaryKey = new DataColumn[] { registration.Columns["RegistrationID"] };

//            ds.Tables.Add(students);
//            ds.Tables.Add(courses);
//            ds.Tables.Add(registration);

//            return ds;
//        }

//        // Заполнение тестовыми данными
//        static void FillTestData(DataSet ds)
//        {
//            DataTable students = ds.Tables["Студенты"];
//            DataTable courses = ds.Tables["Курсы"];
//            DataTable registration = ds.Tables["Регистрация"];

//            // Добавляем студентов
//            students.Rows.Add(101, "Иван Петров", "ivan@example.com");
//            students.Rows.Add(102, "Мария Сидорова", "maria@example.com");
//            students.Rows.Add(103, "Петр Иванов", "petr@example.com");
//            students.Rows.Add(104, "Анна Смирнова", "anna@example.com");

//            // Добавляем курсы
//            courses.Rows.Add("C001", "C# Programming", "Дмитрий Волков");
//            courses.Rows.Add("C002", "Database Design", "Светлана Морозова");
//            courses.Rows.Add("C003", "Web Development", "Алексей Новиков");
//            courses.Rows.Add("C004", "OOP Principles", "Петр Сергеев");

//            // Добавляем регистрации с оценками
//            registration.Rows.Add(1, 101, "C001", new DateTime(2025, 11, 15), 4.5);
//            registration.Rows.Add(2, 101, "C002", new DateTime(2025, 11, 20), 3.8);
//            registration.Rows.Add(3, 101, "C004", new DateTime(2025, 12, 10), 4.9);
//            registration.Rows.Add(4, 102, "C001", new DateTime(2025, 11, 15), 4.8);
//            registration.Rows.Add(5, 102, "C003", new DateTime(2025, 12, 05), 4.2);
//            registration.Rows.Add(6, 103, "C002", new DateTime(2025, 11, 20), 3.5);
//            registration.Rows.Add(7, 103, "C003", new DateTime(2025, 12, 05), 4.0);
//            registration.Rows.Add(8, 103, "C004", new DateTime(2025, 12, 10), 4.7);
//            registration.Rows.Add(9, 104, "C001", new DateTime(2025, 11, 15), 4.6);
//            registration.Rows.Add(10, 104, "C002", new DateTime(2025, 11, 20), 4.3);
//            registration.Rows.Add(11, 104, "C003", new DateTime(2025, 12, 05), 4.1);
//            registration.Rows.Add(12, 104, "C004", new DateTime(2025, 12, 10), DBNull.Value); // Оценка отсутствует
//        }

//        // Создание отношений DataRelation
//        static void CreateRelations(DataSet ds)
//        {
//            DataTable students = ds.Tables["Студенты"];
//            DataTable courses = ds.Tables["Курсы"];
//            DataTable registration = ds.Tables["Регистрация"];

//            try
//            {
//                // Отношение: Студенты → Регистрация (один студент → много регистраций)
//                DataRelation studentRegistrationRelation = new DataRelation(
//                    "Students_Registrations",
//                    students.Columns["StudentID"],
//                    registration.Columns["StudentID"],
//                    true);

//                // Отношение: Курсы → Регистрация (один курс → много регистраций)
//                DataRelation courseRegistrationRelation = new DataRelation(
//                    "Courses_Registrations",
//                    courses.Columns["CourseID"],
//                    registration.Columns["CourseID"],
//                    true);

//                ds.Relations.Add(studentRegistrationRelation);
//                ds.Relations.Add(courseRegistrationRelation);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Ошибка при создании отношений: {ex.Message}");
//            }
//        }

//        // 1. Получение всех курсов и оценок для студента
//        static void GetStudentCoursesAndGrades(DataSet ds, string studentName)
//        {
//            DataTable students = ds.Tables["Студенты"];
//            DataTable courses = ds.Tables["Курсы"];
//            DataTable registration = ds.Tables["Регистрация"];
//            DataRelation studentRegistrationRelation = ds.Relations["Students_Registrations"];
//            DataRelation courseRegistrationRelation = ds.Relations["Courses_Registrations"];

//            // Находим студента по имени
//            DataRow[] studentRows = students.Select($"StudentName = '{studentName}'");

//            if (studentRows.Length == 0)
//            {
//                Console.WriteLine($"Студент '{studentName}' не найден.");
//                return;
//            }

//            DataRow studentRow = studentRows[0];
//            Console.WriteLine($"Курсы и оценки студента: {studentName}");

//            // Получаем все регистрации студента
//            DataRow[] registrationRows = studentRow.GetChildRows(studentRegistrationRelation);

//            if (registrationRows.Length == 0)
//            {
//                Console.WriteLine("\tСтудент не записан ни на один курс.");
//                return;
//            }

//            foreach (DataRow regRow in registrationRows)
//            {
//                // Получаем информацию о курсе
//                DataRow[] courseRows = regRow.GetParentRows(courseRegistrationRelation);

//                if (courseRows.Length > 0)
//                {
//                    DataRow courseRow = courseRows[0];
//                    DateTime enrollmentDate = (DateTime)regRow["EnrollmentDate"];
//                    double? grade = regRow["Grade"] as double?;

//                    Console.WriteLine($"\t• {courseRow["CourseName"]}");
//                    Console.WriteLine($"\t  Преподаватель: {courseRow["Instructor"]}");
//                    Console.WriteLine($"\t  Дата регистрации: {enrollmentDate:dd.MM.yyyy}");
//                    Console.WriteLine($"\t  Оценка: {(grade.HasValue ? grade.Value.ToString("F1") : "Нет оценки")}");
//                    Console.WriteLine();
//                }
//            }
//        }

//        // 2. Получение всех студентов и их оценок для курса
//        static void GetCourseStudentsAndGrades(DataSet ds, string courseName)
//        {
//            DataTable students = ds.Tables["Студенты"];
//            DataTable courses = ds.Tables["Курсы"];
//            DataTable registration = ds.Tables["Регистрация"];
//            DataRelation studentRegistrationRelation = ds.Relations["Students_Registrations"];
//            DataRelation courseRegistrationRelation = ds.Relations["Courses_Registrations"];

//            // Находим курс по имени
//            DataRow[] courseRows = courses.Select($"CourseName = '{courseName}'");

//            if (courseRows.Length == 0)
//            {
//                Console.WriteLine($"Курс '{courseName}' не найден.");
//                return;
//            }

//            DataRow courseRow = courseRows[0];
//            Console.WriteLine($"Студенты и их оценки на курсе: {courseName}");

//            // Получаем все регистрации курса
//            DataRow[] registrationRows = courseRow.GetChildRows(courseRegistrationRelation);

//            if (registrationRows.Length == 0)
//            {
//                Console.WriteLine("\tНа этом курсе нет студентов.");
//                return;
//            }

//            foreach (DataRow regRow in registrationRows)
//            {
//                // Получаем информацию о студенте
//                DataRow[] studentRows = regRow.GetParentRows(studentRegistrationRelation);

//                if (studentRows.Length > 0)
//                {
//                    DataRow studentRow = studentRows[0];
//                    DateTime enrollmentDate = (DateTime)regRow["EnrollmentDate"];
//                    double? grade = regRow["Grade"] as double?;

//                    Console.WriteLine($"\t• {studentRow["StudentName"]}");
//                    Console.WriteLine($"\t  Email: {studentRow["Email"]}");
//                    Console.WriteLine($"\t  Дата регистрации: {enrollmentDate:dd.MM.yyyy}");
//                    Console.WriteLine($"\t  Оценка: {(grade.HasValue ? grade.Value.ToString("F1") : "Нет оценки")}");
//                    Console.WriteLine();
//                }
//            }
//        }

//        // 3. Поиск студентов, учащихся на одних и тех же курсах
//        static void FindStudentsWithSameCourses(DataSet ds, string studentName)
//        {
//            DataTable students = ds.Tables["Студенты"];
//            DataTable registration = ds.Tables["Регистрация"];
//            DataRelation studentRegistrationRelation = ds.Relations["Students_Registrations"];

//            // Находим студента по имени
//            DataRow[] studentRows = students.Select($"StudentName = '{studentName}'");

//            if (studentRows.Length == 0)
//            {
//                Console.WriteLine($"Студент '{studentName}' не найден.");
//                return;
//            }

//            DataRow studentRow = studentRows[0];

//            // Получаем все курсы студента
//            DataRow[] studentRegistrations = studentRow.GetChildRows(studentRegistrationRelation);
//            var studentCourseIDs = new HashSet<string>();

//            foreach (DataRow regRow in studentRegistrations)
//            {
//                studentCourseIDs.Add((string)regRow["CourseID"]);
//            }

//            Console.WriteLine($"Студенты, учащиеся на тех же курсах, что и {studentName}:");

//            // Находим других студентов, учащихся на тех же курсах
//            foreach (DataRow otherStudentRow in students.Rows)
//            {
//                if ((int)otherStudentRow["StudentID"] == (int)studentRow["StudentID"])
//                    continue;

//                DataRow[] otherStudentRegistrations = otherStudentRow.GetChildRows(studentRegistrationRelation);
//                var otherCourseIDs = new HashSet<string>();

//                foreach (DataRow regRow in otherStudentRegistrations)
//                {
//                    otherCourseIDs.Add((string)regRow["CourseID"]);
//                }

//                // Находим пересечение курсов
//                var commonCourses = studentCourseIDs.Intersect(otherCourseIDs).ToList();

//                if (commonCourses.Count > 0)
//                {
//                    Console.WriteLine($"\t• {(string)otherStudentRow["StudentName"]}");
//                    Console.WriteLine($"\t  Общие курсы: {string.Join(", ", commonCourses)}");
//                    Console.WriteLine();
//                }
//            }
//        }

//        // 4. Полная информация о регистрациях
//        static void PrintFullRegistrationInfo(DataSet ds)
//        {
//            DataTable students = ds.Tables["Студенты"];
//            DataTable courses = ds.Tables["Курсы"];
//            DataTable registration = ds.Tables["Регистрация"];
//            DataRelation studentRegistrationRelation = ds.Relations["Students_Registrations"];
//            DataRelation courseRegistrationRelation = ds.Relations["Courses_Registrations"];

//            Console.WriteLine("ID | Студент              | Курс             | Дата         | Оценка");
//            Console.WriteLine("────────────────────────────────────────────────────────────────────");

//            foreach (DataRow regRow in registration.Rows)
//            {
//                int registrationID = (int)regRow["RegistrationID"];
//                DateTime enrollmentDate = (DateTime)regRow["EnrollmentDate"];
//                double? grade = regRow["Grade"] as double?;

//                // Получаем информацию о студенте
//                DataRow[] studentRows = regRow.GetParentRows(studentRegistrationRelation);
//                string studentName = studentRows.Length > 0 ?
//                    (string)studentRows[0]["StudentName"] : "Неизвестен";

//                // Получаем информацию о курсе
//                DataRow[] courseRows = regRow.GetParentRows(courseRegistrationRelation);
//                string courseName = courseRows.Length > 0 ?
//                    (string)courseRows[0]["CourseName"] : "Неизвестен";

//                Console.WriteLine($"{registrationID,2} | {studentName,-20} | {courseName,-15} | {enrollmentDate:dd.MM.yyyy} | {(grade.HasValue ? grade.Value.ToString("F1") : "Нет")}");
//            }
//        }

//        // 5. Средняя оценка для каждого студента
//        static void CalculateStudentAverageGrades(DataSet ds)
//        {
//            DataTable students = ds.Tables["Студенты"];
//            DataTable registration = ds.Tables["Регистрация"];
//            DataRelation studentRegistrationRelation = ds.Relations["Students_Registrations"];

//            Console.WriteLine("Студент              | Количество курсов | Средняя оценка");
//            Console.WriteLine("─────────────────────────────────────────────────────────");

//            foreach (DataRow studentRow in students.Rows)
//            {
//                string studentName = (string)studentRow["StudentName"];
//                DataRow[] registrationRows = studentRow.GetChildRows(studentRegistrationRelation);

//                if (registrationRows.Length == 0)
//                {
//                    Console.WriteLine($"{studentName,-20} | {0,16} | Нет оценок");
//                    continue;
//                }

//                double sumGrades = 0;
//                int gradeCount = 0;

//                foreach (DataRow regRow in registrationRows)
//                {
//                    if (regRow["Grade"] != DBNull.Value)
//                    {
//                        sumGrades += (double)regRow["Grade"];
//                        gradeCount++;
//                    }
//                }

//                double averageGrade = gradeCount > 0 ? sumGrades / gradeCount : 0;
//                Console.WriteLine($"{studentName,-20} | {registrationRows.Length,16} | {(gradeCount > 0 ? averageGrade.ToString("F2") : "Нет оценок")}");
//            }
//        }

//        // 6. Средняя оценка студентов по каждому курсу
//        static void CalculateCourseAverageGrades(DataSet ds)
//        {
//            DataTable courses = ds.Tables["Курсы"];
//            DataTable registration = ds.Tables["Регистрация"];
//            DataRelation courseRegistrationRelation = ds.Relations["Courses_Registrations"];

//            Console.WriteLine("Курс                  | Количество студентов | Средняя оценка");
//            Console.WriteLine("─────────────────────────────────────────────────────────────");

//            foreach (DataRow courseRow in courses.Rows)
//            {
//                string courseName = (string)courseRow["CourseName"];
//                DataRow[] registrationRows = courseRow.GetChildRows(courseRegistrationRelation);

//                if (registrationRows.Length == 0)
//                {
//                    Console.WriteLine($"{courseName,-20} | {0,20} | Нет оценок");
//                    continue;
//                }

//                double sumGrades = 0;
//                int gradeCount = 0;

//                foreach (DataRow regRow in registrationRows)
//                {
//                    if (regRow["Grade"] != DBNull.Value)
//                    {
//                        sumGrades += (double)regRow["Grade"];
//                        gradeCount++;
//                    }
//                }

//                double averageGrade = gradeCount > 0 ? sumGrades / gradeCount : 0;
//                Console.WriteLine($"{courseName,-20} | {registrationRows.Length,20} | {(gradeCount > 0 ? averageGrade.ToString("F2") : "Нет оценок")}");
//            }
//        }

//        // 7. Лучшие студенты (средний рейтинг выше 4.5)
//        static void FindTopStudents(DataSet ds, double minAverageGrade)
//        {
//            DataTable students = ds.Tables["Студенты"];
//            DataTable registration = ds.Tables["Регистрация"];
//            DataRelation studentRegistrationRelation = ds.Relations["Students_Registrations"];

//            Console.WriteLine("Студент              | Средняя оценка | Курсы");
//            Console.WriteLine("─────────────────────────────────────────────");

//            bool foundStudents = false;

//            foreach (DataRow studentRow in students.Rows)
//            {
//                string studentName = (string)studentRow["StudentName"];
//                DataRow[] registrationRows = studentRow.GetChildRows(studentRegistrationRelation);

//                if (registrationRows.Length == 0)
//                    continue;

//                double sumGrades = 0;
//                int gradeCount = 0;

//                foreach (DataRow regRow in registrationRows)
//                {
//                    if (regRow["Grade"] != DBNull.Value)
//                    {
//                        sumGrades += (double)regRow["Grade"];
//                        gradeCount++;
//                    }
//                }

//                if (gradeCount == 0)
//                    continue;

//                double averageGrade = sumGrades / gradeCount;

//                if (averageGrade >= minAverageGrade)
//                {
//                    foundStudents = true;

//                    // Получаем информацию о курсах студента
//                    var courseNames = new List<string>();
//                    foreach (DataRow regRow in registrationRows)
//                    {
//                        DataRow[] courseRows = regRow.GetParentRows(ds.Relations["Courses_Registrations"]);
//                        if (courseRows.Length > 0)
//                        {
//                            courseNames.Add((string)courseRows[0]["CourseName"]);
//                        }
//                    }

//                    string courses = string.Join(", ", courseNames);
//                    Console.WriteLine($"{studentName,-20} | {averageGrade,14:F2} | {courses}");
//                }
//            }

//            if (!foundStudents)
//            {
//                Console.WriteLine($"Студентов со средней оценкой ≥ {minAverageGrade} не найдено");
//            }
//        }
//    }
//}


////Задание 8: Использование DataRelation для создания расчетных полей
//using System;
//using System.Data;

//namespace CalculatedFieldsWithDataRelation
//{
//    class Program
//    {
//        static void Main()
//        {
//            // Создаём DataSet и таблицы
//            DataSet ds = CreateDataSet();

//            // Заполняем таблицы тестовыми данными
//            FillTestData(ds);

//            // Создаём отношение между таблицами
//            CreateRelation(ds);

//            // Добавляем расчетные поля
//            AddCalculatedColumns(ds);

//            Console.WriteLine("=== РАСЧЁТНЫЕ ПОЛЯ С ИСПОЛЬЗОВАНИЕМ DATARELATION ===\n");

//            // Выводим информацию о категориях с расчетными полями
//            Console.WriteLine("1. ИНФОРМАЦИЯ О КАТЕГОРИЯХ С РАСЧЁТНЫМИ ПОЛЯМИ:");
//            Console.WriteLine("=====================================");
//            PrintCategoriesWithCalculations(ds);
//            Console.WriteLine();

//            // Демонстрация обновления расчетных полей при добавлении товара
//            Console.WriteLine("2. ДОБАВЛЕНИЕ НОВОГО ТОВАРА:");
//            Console.WriteLine("=====================================");
//            AddNewProduct(ds, "Новый товар", 1999.99m, 1, 10);
//            PrintCategoriesWithCalculations(ds);
//            Console.WriteLine();

//            // Демонстрация обновления расчетных полей при удалении товара
//            Console.WriteLine("3. УДАЛЕНИЕ ТОВАРА:");
//            Console.WriteLine("=====================================");
//            RemoveProduct(ds, 1);
//            PrintCategoriesWithCalculations(ds);
//            Console.WriteLine();

//            // Демонстрация обновления расчетных полей при изменении цены товара
//            Console.WriteLine("4. ИЗМЕНЕНИЕ ЦЕНЫ ТОВАРА:");
//            Console.WriteLine("=====================================");
//            UpdateProductPrice(ds, 2, 69990.00m);
//            PrintCategoriesWithCalculations(ds);
//        }

//        // Создание DataSet с таблицами
//        static DataSet CreateDataSet()
//        {
//            DataSet ds = new DataSet("ProductDataSet");

//            // Таблица Категории
//            DataTable categories = new DataTable("Категории");
//            categories.Columns.Add("CategoryID", typeof(int));
//            categories.Columns.Add("CategoryName", typeof(string));
//            categories.Columns.Add("Описание", typeof(string));
//            categories.PrimaryKey = new DataColumn[] { categories.Columns["CategoryID"] };

//            // Таблица Товары
//            DataTable products = new DataTable("Товары");
//            products.Columns.Add("ProductID", typeof(int));
//            products.Columns.Add("ProductName", typeof(string));
//            products.Columns.Add("Price", typeof(decimal));
//            products.Columns.Add("Quantity", typeof(int));
//            products.Columns.Add("CategoryID", typeof(int));
//            products.PrimaryKey = new DataColumn[] { products.Columns["ProductID"] };

//            ds.Tables.Add(categories);
//            ds.Tables.Add(products);

//            return ds;
//        }

//        // Заполнение тестовыми данными
//        static void FillTestData(DataSet ds)
//        {
//            DataTable categories = ds.Tables["Категории"];
//            DataTable products = ds.Tables["Товары"];

//            // Добавляем категории
//            categories.Rows.Add(1, "Электроника", "Электронные устройства");
//            categories.Rows.Add(2, "Одежда", "Одежда и обувь");
//            categories.Rows.Add(3, "Книги", "Книги и журналы");

//            // Добавляем товары
//            products.Rows.Add(1, "Смартфон", 29990.00m, 10, 1);
//            products.Rows.Add(2, "Ноутбук", 59990.00m, 5, 1);
//            products.Rows.Add(3, "Наушники", 2990.00m, 20, 1);
//            products.Rows.Add(4, "Футболка", 990.00m, 30, 2);
//            products.Rows.Add(5, "Джинсы", 2490.00m, 15, 2);
//            products.Rows.Add(6, "Кроссовки", 3990.00m, 10, 2);
//            products.Rows.Add(7, "Роман", 490.00m, 50, 3);
//            products.Rows.Add(8, "Учебник", 1290.00m, 20, 3);
//            products.Rows.Add(9, "Журнал", 190.00m, 100, 3);
//        }

//        // Создание отношения между таблицами
//        static void CreateRelation(DataSet ds)
//        {
//            DataTable categories = ds.Tables["Категории"];
//            DataTable products = ds.Tables["Товары"];

//            try
//            {
//                DataRelation relation = new DataRelation(
//                    "CategoryProducts",
//                    categories.Columns["CategoryID"],
//                    products.Columns["CategoryID"],
//                    true); // createConstraints = true

//                ds.Relations.Add(relation);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Ошибка при создании отношения: {ex.Message}");
//            }
//        }

//        // Добавление расчетных полей
//        static void AddCalculatedColumns(DataSet ds)
//        {
//            DataTable categories = ds.Tables["Категории"];
//            DataTable products = ds.Tables["Товары"];
//            DataRelation relation = ds.Relations["CategoryProducts"];

//            // Добавление колонки для хранения количества товаров в категории
//            DataColumn totalProductCountColumn = new DataColumn("TotalProductCount", typeof(int));
//            totalProductCountColumn.Expression = "Count(Child.CategoryID)";
//            categories.Columns.Add(totalProductCountColumn);

//            // Добавление колонки для хранения общей стоимости товаров в категории
//            DataColumn totalCategoryValueColumn = new DataColumn("TotalCategoryValue", typeof(decimal));
//            categories.Columns.Add(totalCategoryValueColumn);

//            // Пересчет общей стоимости для всех категорий
//            UpdateCategoryValues(ds);
//        }

//        // Пересчет общей стоимости товаров в категориях
//        static void UpdateCategoryValues(DataSet ds)
//        {
//            DataTable categories = ds.Tables["Категории"];
//            DataTable products = ds.Tables["Товары"];
//            DataRelation relation = ds.Relations["CategoryProducts"];

//            foreach (DataRow categoryRow in categories.Rows)
//            {
//                int categoryID = (int)categoryRow["CategoryID"];
//                decimal totalValue = 0;

//                // Получаем все товары в категории
//                DataRow[] productRows = categoryRow.GetChildRows(relation);

//                foreach (DataRow productRow in productRows)
//                {
//                    decimal price = (decimal)productRow["Price"];
//                    int quantity = (int)productRow["Quantity"];
//                    totalValue += price * quantity;
//                }

//                // Обновляем значение общей стоимости
//                categoryRow["TotalCategoryValue"] = totalValue;
//            }
//        }

//        // Вывод информации о категориях с расчетными полями
//        static void PrintCategoriesWithCalculations(DataSet ds)
//        {
//            DataTable categories = ds.Tables["Категории"];

//            Console.WriteLine("ID | Категория          | Описание               | Количество товаров | Общая стоимость");
//            Console.WriteLine("───────────────────────────────────────────────────────────────────────────────────────");

//            foreach (DataRow categoryRow in categories.Rows)
//            {
//                int categoryID = (int)categoryRow["CategoryID"];
//                string categoryName = (string)categoryRow["CategoryName"];
//                string description = (string)categoryRow["Описание"];
//                int productCount = (int)categoryRow["TotalProductCount"];
//                decimal totalValue = (decimal)categoryRow["TotalCategoryValue"];

//                Console.WriteLine($"{categoryID,2} | {categoryName,-18} | {description,-20} | {productCount,17} | {totalValue,16:C}");
//            }
//        }

//        // Добавление нового товара
//        static void AddNewProduct(DataSet ds, string productName, decimal price, int quantity, int categoryID)
//        {
//            DataTable products = ds.Tables["Товары"];

//            // Находим максимальный ProductID
//            int maxProductID = 0;
//            foreach (DataRow row in products.Rows)
//            {
//                int currentID = (int)row["ProductID"];
//                if (currentID > maxProductID)
//                {
//                    maxProductID = currentID;
//                }
//            }

//            // Добавляем новый товар
//            products.Rows.Add(maxProductID + 1, productName, price, quantity, categoryID);

//            Console.WriteLine($"Добавлен новый товар: {productName}");

//            // Обновляем расчетные поля
//            UpdateCategoryValues(ds);
//        }

//        // Удаление товара
//        static void RemoveProduct(DataSet ds, int productID)
//        {
//            DataTable products = ds.Tables["Товары"];

//            DataRow productRow = products.Rows.Find(productID);

//            if (productRow != null)
//            {
//                string productName = (string)productRow["ProductName"];
//                productRow.Delete();

//                Console.WriteLine($"Удален товар: {productName}");

//                // Обновляем расчетные поля
//                UpdateCategoryValues(ds);
//            }
//            else
//            {
//                Console.WriteLine($"Товар с ID {productID} не найден.");
//            }
//        }

//        // Изменение цены товара
//        static void UpdateProductPrice(DataSet ds, int productID, decimal newPrice)
//        {
//            DataTable products = ds.Tables["Товары"];

//            DataRow productRow = products.Rows.Find(productID);

//            if (productRow != null)
//            {
//                string productName = (string)productRow["ProductName"];
//                decimal oldPrice = (decimal)productRow["Price"];

//                productRow["Price"] = newPrice;

//                Console.WriteLine($"Цена товара '{productName}' изменена с {oldPrice:C} на {newPrice:C}");

//                // Обновляем расчетные поля
//                UpdateCategoryValues(ds);
//            }
//            else
//            {
//                Console.WriteLine($"Товар с ID {productID} не найден.");
//            }
//        }
//    }
//}


////Задание 9: Использование DeleteRule для определения поведения при удалении родительской записи
//using System;
//using System.Data;

//namespace DeleteRuleExample
//{
//    class Program
//    {
//        static void Main()
//        {
//            Console.WriteLine("=== ДЕМОНСТРАЦИЯ DELETE RULE ===\n");

//            // Вариант 1: DeleteRule.Cascade
//            Console.WriteLine("ВАРИАНТ 1: DELETE RULE CASCADE");
//            Console.WriteLine("=====================================");
//            DemonstrateDeleteRuleCascade();
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // Вариант 2: DeleteRule.SetNull
//            Console.WriteLine("ВАРИАНТ 2: DELETE RULE SET NULL");
//            Console.WriteLine("=====================================");
//            DemonstrateDeleteRuleSetNull();
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // Вариант 3: DeleteRule.None
//            Console.WriteLine("ВАРИАНТ 3: DELETE RULE NONE");
//            Console.WriteLine("=====================================");
//            DemonstrateDeleteRuleNone();
//        }

//        // Создание DataSet с таблицами
//        static DataSet CreateDataSet()
//        {
//            DataSet ds = new DataSet("CompanyDataSet");

//            // Таблица Отделы
//            DataTable departments = new DataTable("Отделы");
//            departments.Columns.Add("DepartmentID", typeof(int));
//            departments.Columns.Add("DepartmentName", typeof(string));
//            departments.PrimaryKey = new DataColumn[] { departments.Columns["DepartmentID"] };

//            // Таблица Сотрудники
//            DataTable employees = new DataTable("Сотрудники");
//            employees.Columns.Add("EmployeeID", typeof(int));
//            employees.Columns.Add("EmployeeName", typeof(string));
//            employees.Columns.Add("DepartmentID", typeof(int));
//            employees.Columns.Add("Salary", typeof(decimal));
//            employees.PrimaryKey = new DataColumn[] { employees.Columns["EmployeeID"] };

//            ds.Tables.Add(departments);
//            ds.Tables.Add(employees);

//            return ds;
//        }

//        // Заполнение тестовыми данными
//        static void FillTestData(DataSet ds)
//        {
//            DataTable departments = ds.Tables["Отделы"];
//            DataTable employees = ds.Tables["Сотрудники"];

//            // Добавляем отделы
//            departments.Rows.Add(1, "IT");
//            departments.Rows.Add(2, "HR");
//            departments.Rows.Add(3, "Finance");

//            // Добавляем сотрудников
//            employees.Rows.Add(101, "Иван Иванов", 1, 100000);
//            employees.Rows.Add(102, "Мария Петрова", 1, 95000);
//            employees.Rows.Add(103, "Петр Сидоров", 2, 85000);
//            employees.Rows.Add(104, "Анна Кузнецова", 2, 80000);
//            employees.Rows.Add(105, "Сергей Васильев", 3, 90000);
//        }

//        // Демонстрация DeleteRule.Cascade
//        static void DemonstrateDeleteRuleCascade()
//        {
//            DataSet ds = CreateDataSet();
//            FillTestData(ds);

//            // Создаём отношение с DeleteRule.Cascade
//            DataTable departments = ds.Tables["Отделы"];
//            DataTable employees = ds.Tables["Сотрудники"];

//            // Создаем ограничение внешнего ключа
//            ForeignKeyConstraint fkConstraint = new ForeignKeyConstraint(
//                "FK_Employees_Departments",
//                departments.Columns["DepartmentID"],
//                employees.Columns["DepartmentID"]);

//            // Устанавливаем DeleteRule.Cascade
//            fkConstraint.DeleteRule = Rule.Cascade;

//            // Добавляем ограничение в таблицу сотрудников
//            employees.Constraints.Add(fkConstraint);

//            // Выводим состояние до удаления
//            Console.WriteLine("СОСТОЯНИЕ ДО УДАЛЕНИЯ:");
//            PrintDataSetState(ds);

//            // Удаляем отдел IT
//            DataRow itDepartment = departments.Rows.Find(1);
//            if (itDepartment != null)
//            {
//                Console.WriteLine($"\nУдаляем отдел: {itDepartment["DepartmentName"]}");
//                itDepartment.Delete();
//            }

//            // Выводим состояние после удаления
//            Console.WriteLine("\nСОСТОЯНИЕ ПОСЛЕ УДАЛЕНИЯ:");
//            PrintDataSetState(ds);

//            Console.WriteLine("\nРЕЗУЛЬТАТ: Отдел и все его сотрудники были удалены (CASCADE).");
//        }

//        // Демонстрация DeleteRule.SetNull
//        static void DemonstrateDeleteRuleSetNull()
//        {
//            DataSet ds = CreateDataSet();
//            FillTestData(ds);

//            // Создаём отношение с DeleteRule.SetNull
//            DataTable departments = ds.Tables["Отделы"];
//            DataTable employees = ds.Tables["Сотрудники"];

//            // Разрешаем NULL значения в DepartmentID
//            employees.Columns["DepartmentID"].AllowDBNull = true;

//            // Создаем ограничение внешнего ключа
//            ForeignKeyConstraint fkConstraint = new ForeignKeyConstraint(
//                "FK_Employees_Departments",
//                departments.Columns["DepartmentID"],
//                employees.Columns["DepartmentID"]);

//            // Устанавливаем DeleteRule.SetNull
//            fkConstraint.DeleteRule = Rule.SetNull;

//            // Добавляем ограничение в таблицу сотрудников
//            employees.Constraints.Add(fkConstraint);

//            // Выводим состояние до удаления
//            Console.WriteLine("СОСТОЯНИЕ ДО УДАЛЕНИЯ:");
//            PrintDataSetState(ds);

//            // Удаляем отдел IT
//            DataRow itDepartment = departments.Rows.Find(1);
//            if (itDepartment != null)
//            {
//                Console.WriteLine($"\nУдаляем отдел: {itDepartment["DepartmentName"]}");
//                itDepartment.Delete();
//            }

//            // Выводим состояние после удаления
//            Console.WriteLine("\nСОСТОЯНИЕ ПОСЛЕ УДАЛЕНИЯ:");
//            PrintDataSetState(ds);

//            Console.WriteLine("\nРЕЗУЛЬТАТ: Отдел удалён, у сотрудников DepartmentID установлен в NULL (SET NULL).");
//        }

//        // Демонстрация DeleteRule.None
//        static void DemonstrateDeleteRuleNone()
//        {
//            DataSet ds = CreateDataSet();
//            FillTestData(ds);

//            // Создаём отношение с DeleteRule.None
//            DataTable departments = ds.Tables["Отделы"];
//            DataTable employees = ds.Tables["Сотрудники"];

//            // Создаем ограничение внешнего ключа
//            ForeignKeyConstraint fkConstraint = new ForeignKeyConstraint(
//                "FK_Employees_Departments",
//                departments.Columns["DepartmentID"],
//                employees.Columns["DepartmentID"]);

//            // Устанавливаем DeleteRule.None
//            fkConstraint.DeleteRule = Rule.None;

//            // Добавляем ограничение в таблицу сотрудников
//            employees.Constraints.Add(fkConstraint);

//            // Выводим состояние до удаления
//            Console.WriteLine("СОСТОЯНИЕ ДО УДАЛЕНИЯ:");
//            PrintDataSetState(ds);

//            // Пробуем удалить отдел IT
//            try
//            {
//                DataRow itDepartment = departments.Rows.Find(1);
//                if (itDepartment != null)
//                {
//                    Console.WriteLine($"\nПытаемся удалить отдел: {itDepartment["DepartmentName"]}");
//                    itDepartment.Delete();
//                    Console.WriteLine("Удаление прошло успешно.");
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"\nОШИБКА: {ex.Message}");
//                Console.WriteLine("Удаление отменено, так как есть связанные сотрудники (NONE).");
//            }

//            // Выводим состояние после попытки удаления
//            Console.WriteLine("\nСОСТОЯНИЕ ПОСЛЕ ПОПЫТКИ УДАЛЕНИЯ:");
//            PrintDataSetState(ds);
//        }

//        // Вывод состояния DataSet
//        static void PrintDataSetState(DataSet ds)
//        {
//            DataTable departments = ds.Tables["Отделы"];
//            DataTable employees = ds.Tables["Сотрудники"];

//            Console.WriteLine("\nОТДЕЛЫ:");
//            foreach (DataRow row in departments.Rows)
//            {
//                if (row.RowState != DataRowState.Deleted)
//                {
//                    Console.WriteLine($"{row["DepartmentID"]}: {row["DepartmentName"]}");
//                }
//            }

//            Console.WriteLine("\nСОТРУДНИКИ:");
//            foreach (DataRow row in employees.Rows)
//            {
//                if (row.RowState != DataRowState.Deleted)
//                {
//                    int deptID = row["DepartmentID"] != DBNull.Value ? (int)row["DepartmentID"] : -1;
//                    string deptName = deptID != -1 ? deptID.ToString() : "NULL";
//                    Console.WriteLine($"{row["EmployeeID"]}: {row["EmployeeName"]}, Отдел: {deptName}, Зарплата: {row["Salary"]}");
//                }
//            }
//        }
//    }
//}


////Задание 10: Использование UpdateRule для определения поведения при добавлении дополнительного переключателя
//using System;
//using System.Data;

//namespace UpdateRuleExample
//{
//    class Program
//    {
//        static void Main()
//        {
//            Console.WriteLine("=== ДЕМОНСТРАЦИЯ UPDATE RULE ===\n");

//            // Вариант 1: UpdateRule.Cascade
//            Console.WriteLine("ВАРИАНТ 1: UPDATE RULE CASCADE");
//            Console.WriteLine("=====================================");
//            DemonstrateUpdateRuleCascade();
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // Вариант 2: UpdateRule.SetNull
//            Console.WriteLine("ВАРИАНТ 2: UPDATE RULE SET NULL");
//            Console.WriteLine("=====================================");
//            DemonstrateUpdateRuleSetNull();
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // Вариант 3: UpdateRule.None
//            Console.WriteLine("ВАРИАНТ 3: UPDATE RULE NONE");
//            Console.WriteLine("=====================================");
//            DemonstrateUpdateRuleNone();
//        }

//        // Создание DataSet с таблицами
//        static DataSet CreateDataSet()
//        {
//            DataSet ds = new DataSet("CompanyDataSet");

//            // Таблица Отделы
//            DataTable departments = new DataTable("Отделы");
//            departments.Columns.Add("DepartmentID", typeof(int));
//            departments.Columns.Add("DepartmentName", typeof(string));
//            departments.PrimaryKey = new DataColumn[] { departments.Columns["DepartmentID"] };

//            // Таблица Сотрудники
//            DataTable employees = new DataTable("Сотрудники");
//            employees.Columns.Add("EmployeeID", typeof(int));
//            employees.Columns.Add("EmployeeName", typeof(string));
//            employees.Columns.Add("DepartmentID", typeof(int));
//            employees.Columns.Add("Salary", typeof(decimal));
//            employees.PrimaryKey = new DataColumn[] { employees.Columns["EmployeeID"] };

//            ds.Tables.Add(departments);
//            ds.Tables.Add(employees);

//            return ds;
//        }

//        // Заполнение тестовыми данными
//        static void FillTestData(DataSet ds)
//        {
//            DataTable departments = ds.Tables["Отделы"];
//            DataTable employees = ds.Tables["Сотрудники"];

//            // Добавляем отделы
//            departments.Rows.Add(1, "IT");
//            departments.Rows.Add(2, "HR");
//            departments.Rows.Add(3, "Finance");

//            // Добавляем сотрудников
//            employees.Rows.Add(101, "Иван Иванов", 1, 100000);
//            employees.Rows.Add(102, "Мария Петрова", 1, 95000);
//            employees.Rows.Add(103, "Петр Сидоров", 2, 85000);
//            employees.Rows.Add(104, "Анна Кузнецова", 2, 80000);
//            employees.Rows.Add(105, "Сергей Васильев", 3, 90000);
//        }

//        // Демонстрация UpdateRule.Cascade
//        static void DemonstrateUpdateRuleCascade()
//        {
//            DataSet ds = CreateDataSet();
//            FillTestData(ds);

//            // Создаём ограничение внешнего ключа с UpdateRule.Cascade
//            DataTable departments = ds.Tables["Отделы"];
//            DataTable employees = ds.Tables["Сотрудники"];

//            ForeignKeyConstraint fkConstraint = new ForeignKeyConstraint(
//                "FK_Employees_Departments",
//                departments.Columns["DepartmentID"],
//                employees.Columns["DepartmentID"]);

//            // Устанавливаем UpdateRule.Cascade
//            fkConstraint.UpdateRule = Rule.Cascade;

//            // Добавляем ограничение в таблицу сотрудников
//            employees.Constraints.Add(fkConstraint);

//            // Выводим состояние до обновления
//            Console.WriteLine("СОСТОЯНИЕ ДО ОБНОВЛЕНИЯ:");
//            PrintDataSetState(ds);

//            // Обновляем ID отдела IT с 1 на 101
//            DataRow itDepartment = departments.Rows.Find(1);
//            if (itDepartment != null)
//            {
//                Console.WriteLine($"\nОбновляем ID отдела '{itDepartment["DepartmentName"]}' с 1 на 101");
//                itDepartment.BeginEdit();
//                itDepartment["DepartmentID"] = 101;
//                itDepartment.EndEdit();
//            }

//            // Выводим состояние после обновления
//            Console.WriteLine("\nСОСТОЯНИЕ ПОСЛЕ ОБНОВЛЕНИЯ:");
//            PrintDataSetState(ds);

//            Console.WriteLine("\nРЕЗУЛЬТАТ: ID отдела обновлен, и все связанные сотрудники также получили новый DepartmentID (CASCADE).");
//        }

//        // Демонстрация UpdateRule.SetNull
//        static void DemonstrateUpdateRuleSetNull()
//        {
//            DataSet ds = CreateDataSet();
//            FillTestData(ds);

//            // Создаём ограничение внешнего ключа с UpdateRule.SetNull
//            DataTable departments = ds.Tables["Отделы"];
//            DataTable employees = ds.Tables["Сотрудники"];

//            // Разрешаем NULL значения в DepartmentID
//            employees.Columns["DepartmentID"].AllowDBNull = true;

//            ForeignKeyConstraint fkConstraint = new ForeignKeyConstraint(
//                "FK_Employees_Departments",
//                departments.Columns["DepartmentID"],
//                employees.Columns["DepartmentID"]);

//            // Устанавливаем UpdateRule.SetNull
//            fkConstraint.UpdateRule = Rule.SetNull;

//            // Добавляем ограничение в таблицу сотрудников
//            employees.Constraints.Add(fkConstraint);

//            // Выводим состояние до обновления
//            Console.WriteLine("СОСТОЯНИЕ ДО ОБНОВЛЕНИЯ:");
//            PrintDataSetState(ds);

//            // Обновляем ID отдела IT с 1 на 101
//            DataRow itDepartment = departments.Rows.Find(1);
//            if (itDepartment != null)
//            {
//                Console.WriteLine($"\nОбновляем ID отдела '{itDepartment["DepartmentName"]}' с 1 на 101");
//                itDepartment.BeginEdit();
//                itDepartment["DepartmentID"] = 101;
//                itDepartment.EndEdit();
//            }

//            // Выводим состояние после обновления
//            Console.WriteLine("\nСОСТОЯНИЕ ПОСЛЕ ОБНОВЛЕНИЯ:");
//            PrintDataSetState(ds);

//            Console.WriteLine("\nРЕЗУЛЬТАТ: ID отдела обновлен, у сотрудников DepartmentID установлен в NULL (SET NULL).");
//        }

//        // Демонстрация UpdateRule.None
//        static void DemonstrateUpdateRuleNone()
//        {
//            DataSet ds = CreateDataSet();
//            FillTestData(ds);

//            // Создаём ограничение внешнего ключа с UpdateRule.None
//            DataTable departments = ds.Tables["Отделы"];
//            DataTable employees = ds.Tables["Сотрудники"];

//            ForeignKeyConstraint fkConstraint = new ForeignKeyConstraint(
//                "FK_Employees_Departments",
//                departments.Columns["DepartmentID"],
//                employees.Columns["DepartmentID"]);

//            // Устанавливаем UpdateRule.None
//            fkConstraint.UpdateRule = Rule.None;

//            // Добавляем ограничение в таблицу сотрудников
//            employees.Constraints.Add(fkConstraint);

//            // Выводим состояние до обновления
//            Console.WriteLine("СОСТОЯНИЕ ДО ОБНОВЛЕНИЯ:");
//            PrintDataSetState(ds);

//            // Пробуем обновить ID отдела IT с 1 на 101
//            try
//            {
//                DataRow itDepartment = departments.Rows.Find(1);
//                if (itDepartment != null)
//                {
//                    Console.WriteLine($"\nПытаемся обновить ID отдела '{itDepartment["DepartmentName"]}' с 1 на 101");
//                    itDepartment.BeginEdit();
//                    itDepartment["DepartmentID"] = 101;
//                    itDepartment.EndEdit();
//                    Console.WriteLine("Обновление прошло успешно.");
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"\nОШИБКА: {ex.Message}");
//                Console.WriteLine("Обновление отменено, так как есть связанные сотрудники (NONE).");
//            }

//            // Выводим состояние после попытки обновления
//            Console.WriteLine("\nСОСТОЯНИЕ ПОСЛЕ ПОПЫТКИ ОБНОВЛЕНИЯ:");
//            PrintDataSetState(ds);
//        }

//        // Вывод состояния DataSet
//        static void PrintDataSetState(DataSet ds)
//        {
//            DataTable departments = ds.Tables["Отделы"];
//            DataTable employees = ds.Tables["Сотрудники"];

//            Console.WriteLine("\nОТДЕЛЫ:");
//            foreach (DataRow row in departments.Rows)
//            {
//                if (row.RowState != DataRowState.Deleted)
//                {
//                    Console.WriteLine($"{row["DepartmentID"]}: {row["DepartmentName"]}");
//                }
//            }

//            Console.WriteLine("\nСОТРУДНИКИ:");
//            foreach (DataRow row in employees.Rows)
//            {
//                if (row.RowState != DataRowState.Deleted)
//                {
//                    int deptID = row["DepartmentID"] != DBNull.Value ? (int)row["DepartmentID"] : -1;
//                    string deptName = deptID != -1 ? deptID.ToString() : "NULL";
//                    Console.WriteLine($"{row["EmployeeID"]}: {row["EmployeeName"]}, Отдел: {deptName}, Зарплата: {row["Salary"]}");
//                }
//            }
//        }
//    }
//}


////Задание 11: Комбинирование DeleteRule и UpdateRule в одном приложении
//using System;
//using System.Data;

//namespace OrderManagementSystem
//{
//    class Program
//    {
//        static void Main()
//        {
//            // Создаём DataSet и таблицы
//            DataSet ds = CreateDataSet();

//            // Заполняем таблицы тестовыми данными
//            FillTestData(ds);

//            // Создаём отношения с правилами DeleteRule и UpdateRule
//            CreateRelations(ds);

//            Console.WriteLine("=== СИСТЕМА УПРАВЛЕНИЯ ЗАКАЗАМИ ===\n");

//            // Выводим начальное состояние данных
//            Console.WriteLine("НАЧАЛЬНОЕ СОСТОЯНИЕ ДАННЫХ:");
//            PrintDataSetState(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // 1. Добавление нового клиента
//            Console.WriteLine("1. ДОБАВЛЕНИЕ НОВОГО КЛИЕНТА:");
//            Console.WriteLine("=====================================");
//            AddNewCustomer(ds, "Новый Клиент", "newclient@example.com");
//            PrintDataSetState(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // 2. Удаление заказчика
//            Console.WriteLine("2. УДАЛЕНИЕ ЗАКАЗЧИКА (ID: 1):");
//            Console.WriteLine("=====================================");
//            DeleteCustomer(ds, 1);
//            PrintDataSetState(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // 3. Изменение идентификатора заказчика
//            Console.WriteLine("3. ИЗМЕНЕНИЕ ID ЗАКАЗЧИКА (с 2 на 102):");
//            Console.WriteLine("=====================================");
//            UpdateCustomerID(ds, 2, 102);
//            PrintDataSetState(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // 4. Добавление заказа
//            Console.WriteLine("4. ДОБАВЛЕНИЕ НОВОГО ЗАКАЗА:");
//            Console.WriteLine("=====================================");
//            AddNewOrder(ds, 102, new DateTime(2023, 12, 15), 1500.00m);
//            PrintDataSetState(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // 5. Удаление заказа
//            Console.WriteLine("5. УДАЛЕНИЕ ЗАКАЗА (ID: 2):");
//            Console.WriteLine("=====================================");
//            DeleteOrder(ds, 2);
//            PrintDataSetState(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // 6. Изменение ID заказа
//            Console.WriteLine("6. ИЗМЕНЕНИЕ ID ЗАКАЗА (с 3 на 103):");
//            Console.WriteLine("=====================================");
//            UpdateOrderID(ds, 3, 103);
//            PrintDataSetState(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // 7. Отчёт о безопасности операций
//            Console.WriteLine("7. ОТЧЁТ О БЕЗОПАСНОСТИ ОПЕРАЦИЙ:");
//            Console.WriteLine("=====================================");
//            PrintOperationReport(ds);
//        }

//        // Создание DataSet с таблицами
//        static DataSet CreateDataSet()
//        {
//            DataSet ds = new DataSet("OrderManagement");

//            // Таблица Заказчики
//            DataTable customers = new DataTable("Заказчики");
//            customers.Columns.Add("CustomerID", typeof(int));
//            customers.Columns.Add("CustomerName", typeof(string));
//            customers.Columns.Add("Email", typeof(string));
//            customers.PrimaryKey = new DataColumn[] { customers.Columns["CustomerID"] };

//            // Таблица Заказы
//            DataTable orders = new DataTable("Заказы");
//            orders.Columns.Add("OrderID", typeof(int));
//            orders.Columns.Add("OrderDate", typeof(DateTime));
//            orders.Columns.Add("CustomerID", typeof(int));
//            orders.Columns.Add("Total", typeof(decimal));
//            orders.PrimaryKey = new DataColumn[] { orders.Columns["OrderID"] };

//            // Таблица ОрдеротовыеДетали
//            DataTable orderDetails = new DataTable("ОрдеротовыеДетали");
//            orderDetails.Columns.Add("DetailID", typeof(int));
//            orderDetails.Columns.Add("OrderID", typeof(int));
//            orderDetails.Columns.Add("ProductID", typeof(int));
//            orderDetails.Columns.Add("Quantity", typeof(int));
//            orderDetails.Columns.Add("Price", typeof(decimal));
//            orderDetails.PrimaryKey = new DataColumn[] { orderDetails.Columns["DetailID"] };

//            ds.Tables.Add(customers);
//            ds.Tables.Add(orders);
//            ds.Tables.Add(orderDetails);

//            return ds;
//        }

//        // Заполнение тестовыми данными
//        static void FillTestData(DataSet ds)
//        {
//            DataTable customers = ds.Tables["Заказчики"];
//            DataTable orders = ds.Tables["Заказы"];
//            DataTable orderDetails = ds.Tables["ОрдеротовыеДетали"];

//            // Добавляем заказчиков
//            customers.Rows.Add(1, "Иван Иванов", "ivan@example.com");
//            customers.Rows.Add(2, "Мария Петрова", "maria@example.com");

//            // Добавляем заказы
//            orders.Rows.Add(1, new DateTime(2025, 10, 15), 1, 1000.00m);
//            orders.Rows.Add(2, new DateTime(2025, 11, 20), 1, 1500.00m);
//            orders.Rows.Add(3, new DateTime(2025, 12, 05), 2, 2000.00m);

//            // Добавляем детали заказов
//            orderDetails.Rows.Add(1, 1, 101, 2, 500.00m);
//            orderDetails.Rows.Add(2, 1, 102, 1, 500.00m);
//            orderDetails.Rows.Add(3, 2, 103, 3, 500.00m);
//            orderDetails.Rows.Add(4, 2, 104, 1, 1000.00m);
//            orderDetails.Rows.Add(5, 3, 105, 1, 2000.00m);
//        }

//        // Создание отношений с правилами DeleteRule и UpdateRule
//        static void CreateRelations(DataSet ds)
//        {
//            DataTable customers = ds.Tables["Заказчики"];
//            DataTable orders = ds.Tables["Заказы"];
//            DataTable orderDetails = ds.Tables["ОрдеротовыеДетали"];

//            try
//            {
//                // Отношение: Заказчики → Заказы (DeleteRule=Cascade, UpdateRule=Cascade)
//                ForeignKeyConstraint customerOrderConstraint = new ForeignKeyConstraint(
//                    "FK_Customers_Orders",
//                    customers.Columns["CustomerID"],
//                    orders.Columns["CustomerID"]);

//                customerOrderConstraint.DeleteRule = Rule.Cascade;
//                customerOrderConstraint.UpdateRule = Rule.Cascade;

//                orders.Constraints.Add(customerOrderConstraint);

//                // Отношение: Заказы → ОрдеротовыеДетали (DeleteRule=Cascade, UpdateRule=Cascade)
//                ForeignKeyConstraint orderDetailConstraint = new ForeignKeyConstraint(
//                    "FK_Orders_OrderDetails",
//                    orders.Columns["OrderID"],
//                    orderDetails.Columns["OrderID"]);

//                orderDetailConstraint.DeleteRule = Rule.Cascade;
//                orderDetailConstraint.UpdateRule = Rule.Cascade;

//                orderDetails.Constraints.Add(orderDetailConstraint);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Ошибка при создании отношений: {ex.Message}");
//            }
//        }

//        // Вывод состояния DataSet
//        static void PrintDataSetState(DataSet ds)
//        {
//            DataTable customers = ds.Tables["Заказчики"];
//            DataTable orders = ds.Tables["Заказы"];
//            DataTable orderDetails = ds.Tables["ОрдеротовыеДетали"];

//            Console.WriteLine("\nЗАКАЗЧИКИ:");
//            foreach (DataRow row in customers.Rows)
//            {
//                if (row.RowState != DataRowState.Deleted)
//                {
//                    Console.WriteLine($"{row["CustomerID"]}: {row["CustomerName"]}, Email: {row["Email"]}");
//                }
//            }

//            Console.WriteLine("\nЗАКАЗЫ:");
//            foreach (DataRow row in orders.Rows)
//            {
//                if (row.RowState != DataRowState.Deleted)
//                {
//                    Console.WriteLine($"{row["OrderID"]}: Дата: {((DateTime)row["OrderDate"]).ToShortDateString()}, Заказчик: {row["CustomerID"]}, Сумма: {row["Total"]:C}");
//                }
//            }

//            Console.WriteLine("\nДЕТАЛИ ЗАКАЗОВ:");
//            foreach (DataRow row in orderDetails.Rows)
//            {
//                if (row.RowState != DataRowState.Deleted)
//                {
//                    Console.WriteLine($"{row["DetailID"]}: Заказ: {row["OrderID"]}, Продукт: {row["ProductID"]}, Кол-во: {row["Quantity"]}, Цена: {row["Price"]:C}");
//                }
//            }
//        }

//        // 1. Добавление нового клиента
//        static void AddNewCustomer(DataSet ds, string customerName, string email)
//        {
//            DataTable customers = ds.Tables["Заказчики"];

//            // Находим максимальный CustomerID
//            int maxCustomerID = 0;
//            foreach (DataRow row in customers.Rows)
//            {
//                int currentID = (int)row["CustomerID"];
//                if (currentID > maxCustomerID)
//                {
//                    maxCustomerID = currentID;
//                }
//            }

//            // Добавляем нового клиента
//            customers.Rows.Add(maxCustomerID + 1, customerName, email);

//            Console.WriteLine($"Добавлен новый клиент: {customerName}");
//        }

//        // 2. Удаление заказчика
//        static void DeleteCustomer(DataSet ds, int customerID)
//        {
//            DataTable customers = ds.Tables["Заказчики"];

//            DataRow customerRow = customers.Rows.Find(customerID);

//            if (customerRow != null)
//            {
//                string customerName = (string)customerRow["CustomerName"];
//                customerRow.Delete();

//                Console.WriteLine($"Удален заказчик: {customerName} (ID: {customerID})");
//            }
//            else
//            {
//                Console.WriteLine($"Заказчик с ID {customerID} не найден.");
//            }
//        }

//        // 3. Изменение идентификатора заказчика
//        static void UpdateCustomerID(DataSet ds, int oldCustomerID, int newCustomerID)
//        {
//            DataTable customers = ds.Tables["Заказчики"];

//            DataRow customerRow = customers.Rows.Find(oldCustomerID);

//            if (customerRow != null)
//            {
//                string customerName = (string)customerRow["CustomerName"];

//                try
//                {
//                    customerRow.BeginEdit();
//                    customerRow["CustomerID"] = newCustomerID;
//                    customerRow.EndEdit();

//                    Console.WriteLine($"ID заказчика '{customerName}' изменен с {oldCustomerID} на {newCustomerID}");
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"Ошибка при изменении ID заказчика: {ex.Message}");
//                }
//            }
//            else
//            {
//                Console.WriteLine($"Заказчик с ID {oldCustomerID} не найден.");
//            }
//        }

//        // 4. Добавление заказа
//        static void AddNewOrder(DataSet ds, int customerID, DateTime orderDate, decimal total)
//        {
//            DataTable orders = ds.Tables["Заказы"];

//            // Находим максимальный OrderID
//            int maxOrderID = 0;
//            foreach (DataRow row in orders.Rows)
//            {
//                int currentID = (int)row["OrderID"];
//                if (currentID > maxOrderID)
//                {
//                    maxOrderID = currentID;
//                }
//            }

//            // Добавляем новый заказ
//            orders.Rows.Add(maxOrderID + 1, orderDate, customerID, total);

//            Console.WriteLine($"Добавлен новый заказ для клиента с ID: {customerID}");
//        }

//        // 5. Удаление заказа
//        static void DeleteOrder(DataSet ds, int orderID)
//        {
//            DataTable orders = ds.Tables["Заказы"];

//            DataRow orderRow = orders.Rows.Find(orderID);

//            if (orderRow != null)
//            {
//                DateTime orderDate = (DateTime)orderRow["OrderDate"];
//                orderRow.Delete();

//                Console.WriteLine($"Удален заказ с ID: {orderID} от {orderDate.ToShortDateString()}");
//            }
//            else
//            {
//                Console.WriteLine($"Заказ с ID {orderID} не найден.");
//            }
//        }

//        // 6. Изменение ID заказа
//        static void UpdateOrderID(DataSet ds, int oldOrderID, int newOrderID)
//        {
//            DataTable orders = ds.Tables["Заказы"];

//            DataRow orderRow = orders.Rows.Find(oldOrderID);

//            if (orderRow != null)
//            {
//                DateTime orderDate = (DateTime)orderRow["OrderDate"];

//                try
//                {
//                    orderRow.BeginEdit();
//                    orderRow["OrderID"] = newOrderID;
//                    orderRow.EndEdit();

//                    Console.WriteLine($"ID заказа от {orderDate.ToShortDateString()} изменен с {oldOrderID} на {newOrderID}");
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"Ошибка при изменении ID заказа: {ex.Message}");
//                }
//            }
//            else
//            {
//                Console.WriteLine($"Заказ с ID {oldOrderID} не найден.");
//            }
//        }

//        // 7. Отчёт о безопасности операций
//        static void PrintOperationReport(DataSet ds)
//        {
//            DataTable customers = ds.Tables["Заказчики"];
//            DataTable orders = ds.Tables["Заказы"];
//            DataTable orderDetails = ds.Tables["ОрдеротовыеДетали"];

//            Console.WriteLine("ОТЧЁТ О БЕЗОПАСНОСТИ ОПЕРАЦИЙ:");
//            Console.WriteLine("-------------------------------------");
//            Console.WriteLine($"Общее количество заказчиков: {customers.Rows.Count}");
//            Console.WriteLine($"Общее количество заказов: {orders.Rows.Count}");
//            Console.WriteLine($"Общее количество деталей заказов: {orderDetails.Rows.Count}");

//            // Подсчёт количества заказов для каждого заказчика
//            var customerOrderCounts = new Dictionary<int, int>();
//            foreach (DataRow row in orders.Rows)
//            {
//                if (row.RowState != DataRowState.Deleted)
//                {
//                    int customerID = (int)row["CustomerID"];
//                    if (customerOrderCounts.ContainsKey(customerID))
//                    {
//                        customerOrderCounts[customerID]++;
//                    }
//                    else
//                    {
//                        customerOrderCounts[customerID] = 1;
//                    }
//                }
//            }

//            Console.WriteLine("\nКоличество заказов на заказчика:");
//            foreach (var kvp in customerOrderCounts)
//            {
//                DataRow customerRow = customers.Rows.Find(kvp.Key);
//                if (customerRow != null)
//                {
//                    string customerName = (string)customerRow["CustomerName"];
//                    Console.WriteLine($"{customerName}: {kvp.Value}");
//                }
//            }

//            // Подсчёт количества деталей заказа для каждого заказа
//            var orderDetailCounts = new Dictionary<int, int>();
//            foreach (DataRow row in orderDetails.Rows)
//            {
//                if (row.RowState != DataRowState.Deleted)
//                {
//                    int orderID = (int)row["OrderID"];
//                    if (orderDetailCounts.ContainsKey(orderID))
//                    {
//                        orderDetailCounts[orderID]++;
//                    }
//                    else
//                    {
//                        orderDetailCounts[orderID] = 1;
//                    }
//                }
//            }

//            Console.WriteLine("\nКоличество деталей заказа на заказ:");
//            foreach (var kvp in orderDetailCounts)
//            {
//                DataRow orderRow = orders.Rows.Find(kvp.Key);
//                if (orderRow != null)
//                {
//                    DateTime orderDate = (DateTime)orderRow["OrderDate"];
//                    Console.WriteLine($"Заказ от {orderDate.ToShortDateString()}: {kvp.Value}");
//                }
//            }
//        }
//    }
//}


////Задание 12: Использование RowState для получения информации о удаляемых строках.
//using System;
//using System.Data;
//using System.Collections.Generic;

//namespace RowStateExample
//{
//    class Program
//    {
//        static void Main()
//        {
//            // Создаём DataSet и таблицы
//            DataSet ds = CreateDataSet();

//            // Заполняем таблицы тестовыми данными
//            FillTestData(ds);

//            // Создаём отношение между таблицами
//            CreateRelation(ds);

//            Console.WriteLine("=== РАБОТА С ROWSTATE ДЛЯ УДАЛЯЕМЫХ СТРОК ===\n");

//            // Выводим начальное состояние данных
//            Console.WriteLine("НАЧАЛЬНОЕ СОСТОЯНИЕ ДАННЫХ:");
//            PrintDataSetState(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // 1. Добавление 2 новых товаров
//            Console.WriteLine("1. ДОБАВЛЕНИЕ 2 НОВЫХ ТОВАРОВ:");
//            Console.WriteLine("=====================================");
//            AddNewProduct(ds, "Новый товар 1", 1999.99m, 1, 10);
//            AddNewProduct(ds, "Новый товар 2", 2999.99m, 2, 5);
//            PrintDataSetState(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // 2. Модификация 2 товаров
//            Console.WriteLine("2. МОДИФИКАЦИЯ 2 ТОВАРОВ:");
//            Console.WriteLine("=====================================");
//            UpdateProduct(ds, 1, "Обновленный смартфон", 25990.00m, 15);
//            UpdateProduct(ds, 2, "Обновленный ноутбук", 65990.00m, 3);
//            PrintDataSetState(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // 3. Пометка 3 товаров на удаление
//            Console.WriteLine("3. ПОМЕТКА 3 ТОВАРОВ НА УДАЛЕНИЕ:");
//            Console.WriteLine("=====================================");
//            DeleteProduct(ds, 3);
//            DeleteProduct(ds, 4);
//            DeleteProduct(ds, 5);
//            PrintDataSetState(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // 4. Получение всех строк, помеченных на удаление
//            Console.WriteLine("4. ТОВАРЫ, ПОМЕЧЕННЫЕ НА УДАЛЕНИЕ:");
//            Console.WriteLine("=====================================");
//            PrintDeletedProducts(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // 5. Отчёт перед удалением
//            Console.WriteLine("5. ОТЧЁТ ПЕРЕД УДАЛЕНИЕМ:");
//            Console.WriteLine("=====================================");
//            PrintDeletionReport(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // 6. Отмена удаления для отдельных строк
//            Console.WriteLine("6. ОТМЕНА УДАЛЕНИЯ ДЛЯ ТОВАРА С ID 3:");
//            Console.WriteLine("=====================================");
//            CancelProductDeletion(ds, 3);
//            PrintDataSetState(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // 7. Вывод итогового состояния
//            Console.WriteLine("7. ИТОГОВОЕ СОСТОЯНИЕ:");
//            Console.WriteLine("=====================================");
//            PrintDataSetState(ds);
//        }

//        // Создание DataSet с таблицами
//        static DataSet CreateDataSet()
//        {
//            DataSet ds = new DataSet("ProductDataSet");

//            // Таблица Категории
//            DataTable categories = new DataTable("Категории");
//            categories.Columns.Add("CategoryID", typeof(int));
//            categories.Columns.Add("CategoryName", typeof(string));
//            categories.Columns.Add("Описание", typeof(string));
//            categories.PrimaryKey = new DataColumn[] { categories.Columns["CategoryID"] };

//            // Таблица Товары
//            DataTable products = new DataTable("Товары");
//            products.Columns.Add("ProductID", typeof(int));
//            products.Columns.Add("ProductName", typeof(string));
//            products.Columns.Add("Price", typeof(decimal));
//            products.Columns.Add("Quantity", typeof(int));
//            products.Columns.Add("CategoryID", typeof(int));
//            products.PrimaryKey = new DataColumn[] { products.Columns["ProductID"] };

//            ds.Tables.Add(categories);
//            ds.Tables.Add(products);

//            return ds;
//        }

//        // Заполнение тестовыми данными
//        static void FillTestData(DataSet ds)
//        {
//            DataTable categories = ds.Tables["Категории"];
//            DataTable products = ds.Tables["Товары"];

//            // Добавляем категории
//            categories.Rows.Add(1, "Электроника", "Электронные устройства");
//            categories.Rows.Add(2, "Одежда", "Одежда и обувь");
//            categories.Rows.Add(3, "Книги", "Книги и журналы");

//            // Добавляем товары
//            products.Rows.Add(1, "Смартфон", 29990.00m, 10, 1);
//            products.Rows.Add(2, "Ноутбук", 59990.00m, 5, 1);
//            products.Rows.Add(3, "Наушники", 2990.00m, 20, 1);
//            products.Rows.Add(4, "Футболка", 990.00m, 30, 2);
//            products.Rows.Add(5, "Джинсы", 2490.00m, 15, 2);
//            products.Rows.Add(6, "Кроссовки", 3990.00m, 10, 2);
//            products.Rows.Add(7, "Роман", 490.00m, 50, 3);
//            products.Rows.Add(8, "Учебник", 1290.00m, 20, 3);
//            products.Rows.Add(9, "Журнал", 190.00m, 100, 3);
//        }

//        // Создание отношения между таблицами
//        static void CreateRelation(DataSet ds)
//        {
//            DataTable categories = ds.Tables["Категории"];
//            DataTable products = ds.Tables["Товары"];

//            try
//            {
//                DataRelation relation = new DataRelation(
//                    "CategoryProducts",
//                    categories.Columns["CategoryID"],
//                    products.Columns["CategoryID"],
//                    true); // createConstraints = true

//                ds.Relations.Add(relation);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Ошибка при создании отношения: {ex.Message}");
//            }
//        }

//        // Вывод состояния DataSet
//        static void PrintDataSetState(DataSet ds)
//        {
//            DataTable categories = ds.Tables["Категории"];
//            DataTable products = ds.Tables["Товары"];

//            Console.WriteLine("\nКАТЕГОРИИ:");
//            foreach (DataRow row in categories.Rows)
//            {
//                if (row.RowState != DataRowState.Deleted)
//                {
//                    Console.WriteLine($"{row["CategoryID"]}: {row["CategoryName"]} ({row["Описание"]})");
//                }
//            }

//            Console.WriteLine("\nТОВАРЫ:");
//            foreach (DataRow row in products.Rows)
//            {
//                string state = "";
//                switch (row.RowState)
//                {
//                    case DataRowState.Added:
//                        state = " [Добавлен]";
//                        break;
//                    case DataRowState.Modified:
//                        state = " [Изменен]";
//                        break;
//                    case DataRowState.Deleted:
//                        state = " [Удален]";
//                        break;
//                }
//                if (row.RowState != DataRowState.Deleted)
//                {
//                    Console.WriteLine($"{row["ProductID"]}: {row["ProductName"]}, Цена: {row["Price"]:C}, Кол-во: {row["Quantity"]}, Категория: {row["CategoryID"]}{state}");
//                }
//                else
//                {
//                    Console.WriteLine($"{row["ProductID", DataRowVersion.Original]}: {row["ProductName", DataRowVersion.Original]} [Удален]");
//                }
//            }
//        }

//        // 1. Добавление нового товара
//        static void AddNewProduct(DataSet ds, string productName, decimal price, int categoryID, int quantity)
//        {
//            DataTable products = ds.Tables["Товары"];

//            // Находим максимальный ProductID
//            int maxProductID = 0;
//            foreach (DataRow row in products.Rows)
//            {
//                if (row.RowState != DataRowState.Deleted)
//                {
//                    int currentID = (int)row["ProductID"];
//                    if (currentID > maxProductID)
//                    {
//                        maxProductID = currentID;
//                    }
//                }
//            }

//            // Добавляем новый товар
//            products.Rows.Add(maxProductID + 1, productName, price, quantity, categoryID);

//            Console.WriteLine($"Добавлен новый товар: {productName}");
//        }

//        // 2. Модификация товара
//        static void UpdateProduct(DataSet ds, int productID, string productName, decimal price, int quantity)
//        {
//            DataTable products = ds.Tables["Товары"];

//            DataRow productRow = products.Rows.Find(productID);

//            if (productRow != null && productRow.RowState != DataRowState.Deleted)
//            {
//                productRow.BeginEdit();
//                productRow["ProductName"] = productName;
//                productRow["Price"] = price;
//                productRow["Quantity"] = quantity;
//                productRow.EndEdit();

//                Console.WriteLine($"Товар с ID {productID} обновлен.");
//            }
//            else
//            {
//                Console.WriteLine($"Товар с ID {productID} не найден или уже удален.");
//            }
//        }

//        // 3. Пометка товара на удаление
//        static void DeleteProduct(DataSet ds, int productID)
//        {
//            DataTable products = ds.Tables["Товары"];

//            DataRow productRow = products.Rows.Find(productID);

//            if (productRow != null && productRow.RowState != DataRowState.Deleted)
//            {
//                productRow.Delete();

//                Console.WriteLine($"Товар с ID {productID} помечен на удаление.");
//            }
//            else
//            {
//                Console.WriteLine($"Товар с ID {productID} не найден или уже удален.");
//            }
//        }

//        // 4. Получение всех строк, помеченных на удаление
//        static void PrintDeletedProducts(DataSet ds)
//        {
//            DataTable products = ds.Tables["Товары"];
//            DataRelation relation = ds.Relations["CategoryProducts"];

//            Console.WriteLine("Список товаров, помеченных на удаление:");

//            foreach (DataRow row in products.Rows)
//            {
//                if (row.RowState == DataRowState.Deleted)
//                {
//                    int productID = (int)row["ProductID", DataRowVersion.Original];
//                    string productName = (string)row["ProductName", DataRowVersion.Original];

//                    // Получаем родительскую строку (категорию)
//                    DataRow[] categoryRows = row.GetParentRows(relation, DataRowVersion.Original);

//                    if (categoryRows.Length > 0)
//                    {
//                        DataRow categoryRow = categoryRows[0];
//                        string categoryName = (string)categoryRow["CategoryName"];

//                        Console.WriteLine($"\nТовар: {productName} (ID: {productID})");
//                        Console.WriteLine($"\tКатегория: {categoryName}");
//                    }
//                    else
//                    {
//                        Console.WriteLine($"\nТовар: {productName} (ID: {productID})");
//                        Console.WriteLine("\tКатегория: Неизвестно");
//                    }
//                }
//            }
//        }

//        // 5. Отчёт перед удалением
//        static void PrintDeletionReport(DataSet ds)
//        {
//            DataTable products = ds.Tables["Товары"];
//            DataTable categories = ds.Tables["Категории"];
//            DataRelation relation = ds.Relations["CategoryProducts"];

//            Console.WriteLine("ОТЧЁТ ПЕРЕД УДАЛЕНИЕМ:");

//            // Подсчёт количества товаров на удаление по категориям
//            Dictionary<int, List<DataRow>> deletedProductsByCategory = new Dictionary<int, List<DataRow>>();

//            foreach (DataRow row in products.Rows)
//            {
//                if (row.RowState == DataRowState.Deleted)
//                {
//                    int categoryID = (int)row["CategoryID", DataRowVersion.Original];

//                    if (!deletedProductsByCategory.ContainsKey(categoryID))
//                    {
//                        deletedProductsByCategory[categoryID] = new List<DataRow>();
//                    }

//                    deletedProductsByCategory[categoryID].Add(row);
//                }
//            }

//            Console.WriteLine("\nТовары, помеченные на удаление:");
//            foreach (var kvp in deletedProductsByCategory)
//            {
//                int categoryID = kvp.Key;
//                DataRow categoryRow = categories.Rows.Find(categoryID);

//                if (categoryRow != null)
//                {
//                    string categoryName = (string)categoryRow["CategoryName"];
//                    Console.WriteLine($"\nКатегория: {categoryName}");

//                    foreach (DataRow productRow in kvp.Value)
//                    {
//                        string productName = (string)productRow["ProductName", DataRowVersion.Original];
//                        Console.WriteLine($"\tТовар: {productName}");
//                    }
//                }
//            }

//            Console.WriteLine("\nСтатистика по категориям:");
//            foreach (var kvp in deletedProductsByCategory)
//            {
//                int categoryID = kvp.Key;
//                DataRow categoryRow = categories.Rows.Find(categoryID);

//                if (categoryRow != null)
//                {
//                    string categoryName = (string)categoryRow["CategoryName"];
//                    Console.WriteLine($"{categoryName}: {kvp.Value.Count} товаров на удаление");
//                }
//            }
//        }

//        // 6. Отмена удаления для отдельных строк
//        static void CancelProductDeletion(DataSet ds, int productID)
//        {
//            DataTable products = ds.Tables["Товары"];

//            DataRow productRow = products.Rows.Find(productID);

//            if (productRow != null && productRow.RowState == DataRowState.Deleted)
//            {
//                productRow.RejectChanges();

//                Console.WriteLine($"Удаление товара с ID {productID} отменено.");
//            }
//            else
//            {
//                Console.WriteLine($"Товар с ID {productID} не найден или не помечен на удаление.");
//            }
//        }
//    }
//}


////Задание 13: Получение связанной информации для строки со статусом Добавлено
//using System;
//using System.Data;
//using System.Collections.Generic;

//namespace NewRegistrationsAnalysis
//{
//    class Program
//    {
//        static void Main()
//        {
//            // Создаём DataSet и таблицы
//            DataSet ds = CreateDataSet();

//            // Заполняем таблицы тестовыми данными
//            FillTestData(ds);

//            // Создаём отношения DataRelation
//            CreateRelations(ds);

//            Console.WriteLine("=== АНАЛИЗ НОВЫХ РЕГИСТРАЦИЙ ===\n");

//            // Выводим начальное состояние данных
//            Console.WriteLine("НАЧАЛЬНОЕ СОСТОЯНИЕ ДАННЫХ:");
//            PrintDataSetState(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // Добавление новых регистраций
//            Console.WriteLine("ДОБАВЛЕНИЕ НОВЫХ РЕГИСТРАЦИЙ:");
//            Console.WriteLine("=====================================");
//            AddNewRegistration(ds, 101, "C003", new DateTime(2024, 01, 25), 4.0);
//            AddNewRegistration(ds, 102, "C004", new DateTime(2024, 02, 10), 4.5);
//            AddNewRegistration(ds, 103, "C001", new DateTime(2024, 02, 15), 3.8);
//            AddNewRegistration(ds, 104, "C002", new DateTime(2024, 02, 20), 4.2);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // Получение всех новых регистраций
//            Console.WriteLine("НОВЫЕ РЕГИСТРАЦИИ:");
//            Console.WriteLine("=====================================");
//            PrintNewRegistrations(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // Отчёт о новых регистрациях
//            Console.WriteLine("ОТЧЁТ О НОВЫХ РЕГИСТРАЦИЯХ:");
//            Console.WriteLine("=====================================");
//            PrintRegistrationReport(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // Статистика по новым регистрациям
//            Console.WriteLine("СТАТИСТИКА ПО НОВЫМ РЕГИСТРАЦИЯМ:");
//            Console.WriteLine("=====================================");
//            PrintRegistrationStatistics(ds);
//        }

//        // Создание DataSet с таблицами
//        static DataSet CreateDataSet()
//        {
//            DataSet ds = new DataSet("UniversityDB");

//            // Таблица Студенты
//            DataTable students = new DataTable("Студенты");
//            students.Columns.Add("StudentID", typeof(int));
//            students.Columns.Add("StudentName", typeof(string));
//            students.Columns.Add("Email", typeof(string));
//            students.PrimaryKey = new DataColumn[] { students.Columns["StudentID"] };

//            // Таблица Курсы
//            DataTable courses = new DataTable("Курсы");
//            courses.Columns.Add("CourseID", typeof(string));
//            courses.Columns.Add("CourseName", typeof(string));
//            courses.Columns.Add("Instructor", typeof(string));
//            courses.PrimaryKey = new DataColumn[] { courses.Columns["CourseID"] };

//            // Таблица Регистрация (промежуточная)
//            DataTable registration = new DataTable("Регистрация");
//            registration.Columns.Add("RegistrationID", typeof(int));
//            registration.Columns.Add("StudentID", typeof(int));
//            registration.Columns.Add("CourseID", typeof(string));
//            registration.Columns.Add("EnrollmentDate", typeof(DateTime));
//            registration.Columns.Add("Grade", typeof(double));
//            registration.PrimaryKey = new DataColumn[] { registration.Columns["RegistrationID"] };

//            ds.Tables.Add(students);
//            ds.Tables.Add(courses);
//            ds.Tables.Add(registration);

//            return ds;
//        }

//        // Заполнение тестовыми данными
//        static void FillTestData(DataSet ds)
//        {
//            DataTable students = ds.Tables["Студенты"];
//            DataTable courses = ds.Tables["Курсы"];
//            DataTable registration = ds.Tables["Регистрация"];

//            // Добавляем студентов
//            students.Rows.Add(101, "Иван Петров", "ivan@example.com");
//            students.Rows.Add(102, "Мария Сидорова", "maria@example.com");
//            students.Rows.Add(103, "Петр Иванов", "petr@example.com");
//            students.Rows.Add(104, "Анна Смирнова", "anna@example.com");

//            // Добавляем курсы
//            courses.Rows.Add("C001", "C# Programming", "Дмитрий Волков");
//            courses.Rows.Add("C002", "Database Design", "Светлана Морозова");
//            courses.Rows.Add("C003", "Web Development", "Алексей Новиков");
//            courses.Rows.Add("C004", "OOP Principles", "Петр Сергеев");

//            // Добавляем регистрации с оценками
//            registration.Rows.Add(1, 101, "C001", new DateTime(2025, 11, 15), 4.5);
//            registration.Rows.Add(2, 101, "C002", new DateTime(2025, 11, 20), 3.8);
//            registration.Rows.Add(3, 101, "C004", new DateTime(2025, 12, 10), 4.9);
//            registration.Rows.Add(4, 102, "C001", new DateTime(2025, 11, 15), 4.8);
//            registration.Rows.Add(5, 102, "C003", new DateTime(2025, 12, 05), 4.2);
//            registration.Rows.Add(6, 103, "C002", new DateTime(2025, 11, 20), 3.5);
//            registration.Rows.Add(7, 103, "C003", new DateTime(2025, 12, 05), 4.0);
//            registration.Rows.Add(8, 103, "C004", new DateTime(2025, 12, 10), 4.7);
//            registration.Rows.Add(9, 104, "C001", new DateTime(2025, 11, 15), 4.6);
//            registration.Rows.Add(10, 104, "C002", new DateTime(2025, 11, 20), 4.3);
//            registration.Rows.Add(11, 104, "C003", new DateTime(2025, 12, 05), 4.1);
//        }

//        // Создание отношений DataRelation
//        static void CreateRelations(DataSet ds)
//        {
//            DataTable students = ds.Tables["Студенты"];
//            DataTable courses = ds.Tables["Курсы"];
//            DataTable registration = ds.Tables["Регистрация"];

//            try
//            {
//                // Отношение: Студенты → Регистрация (один студент → много регистраций)
//                DataRelation studentRegistrationRelation = new DataRelation(
//                    "Students_Registrations",
//                    students.Columns["StudentID"],
//                    registration.Columns["StudentID"],
//                    true);

//                // Отношение: Курсы → Регистрация (один курс → много регистраций)
//                DataRelation courseRegistrationRelation = new DataRelation(
//                    "Courses_Registrations",
//                    courses.Columns["CourseID"],
//                    registration.Columns["CourseID"],
//                    true);

//                ds.Relations.Add(studentRegistrationRelation);
//                ds.Relations.Add(courseRegistrationRelation);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Ошибка при создании отношений: {ex.Message}");
//            }
//        }

//        // Вывод состояния DataSet
//        static void PrintDataSetState(DataSet ds)
//        {
//            DataTable students = ds.Tables["Студенты"];
//            DataTable courses = ds.Tables["Курсы"];
//            DataTable registration = ds.Tables["Регистрация"];

//            Console.WriteLine("\nСТУДЕНТЫ:");
//            foreach (DataRow row in students.Rows)
//            {
//                Console.WriteLine($"{row["StudentID"]}: {row["StudentName"]}, Email: {row["Email"]}");
//            }

//            Console.WriteLine("\nКУРСЫ:");
//            foreach (DataRow row in courses.Rows)
//            {
//                Console.WriteLine($"{row["CourseID"]}: {row["CourseName"]}, Преподаватель: {row["Instructor"]}");
//            }

//            Console.WriteLine("\nРЕГИСТРАЦИИ:");
//            foreach (DataRow row in registration.Rows)
//            {
//                Console.WriteLine($"{row["RegistrationID"]}: Студент: {row["StudentID"]}, Курс: {row["CourseID"]}, Дата: {((DateTime)row["EnrollmentDate"]).ToShortDateString()}, Оценка: {row["Grade"]}");
//            }
//        }

//        // Добавление новой регистрации
//        static void AddNewRegistration(DataSet ds, int studentID, string courseID, DateTime enrollmentDate, double grade)
//        {
//            DataTable registration = ds.Tables["Регистрация"];

//            // Находим максимальный RegistrationID
//            int maxRegistrationID = 0;
//            foreach (DataRow row in registration.Rows)
//            {
//                int currentID = (int)row["RegistrationID"];
//                if (currentID > maxRegistrationID)
//                {
//                    maxRegistrationID = currentID;
//                }
//            }

//            // Проверяем существование студента и курса
//            DataTable students = ds.Tables["Студенты"];
//            DataTable courses = ds.Tables["Курсы"];

//            DataRow studentRow = students.Rows.Find(studentID);
//            DataRow courseRow = courses.Rows.Find(courseID);

//            if (studentRow == null)
//            {
//                Console.WriteLine($"Ошибка: Студент с ID {studentID} не найден.");
//                return;
//            }

//            if (courseRow == null)
//            {
//                Console.WriteLine($"Ошибка: Курс с ID {courseID} не найден.");
//                return;
//            }

//            // Добавляем новую регистрацию
//            registration.Rows.Add(maxRegistrationID + 1, studentID, courseID, enrollmentDate, grade);

//            Console.WriteLine($"Добавлена новая регистрация: Студент {studentID}, Курс {courseID}");
//        }

//        // Получение всех новых регистраций
//        static void PrintNewRegistrations(DataSet ds)
//        {
//            DataTable registration = ds.Tables["Регистрация"];
//            DataRelation studentRegistrationRelation = ds.Relations["Students_Registrations"];
//            DataRelation courseRegistrationRelation = ds.Relations["Courses_Registrations"];

//            Console.WriteLine("Список новых регистраций:");

//            foreach (DataRow row in registration.Rows)
//            {
//                if (row.RowState == DataRowState.Added)
//                {
//                    int registrationID = (int)row["RegistrationID"];
//                    int studentID = (int)row["StudentID"];
//                    string courseID = (string)row["CourseID"];
//                    DateTime enrollmentDate = (DateTime)row["EnrollmentDate"];
//                    double grade = (double)row["Grade"];

//                    // Получаем информацию о студенте
//                    DataRow[] studentRows = row.GetParentRows(studentRegistrationRelation);
//                    string studentName = studentRows.Length > 0 ? (string)studentRows[0]["StudentName"] : "Неизвестен";

//                    // Получаем информацию о курсе
//                    DataRow[] courseRows = row.GetParentRows(courseRegistrationRelation);
//                    string courseName = courseRows.Length > 0 ? (string)courseRows[0]["CourseName"] : "Неизвестен";

//                    Console.WriteLine($"\nРегистрация ID: {registrationID}");
//                    Console.WriteLine($"\tСтудент: {studentName} (ID: {studentID})");
//                    Console.WriteLine($"\tКурс: {courseName} (ID: {courseID})");
//                    Console.WriteLine($"\tДата регистрации: {enrollmentDate:dd.MM.yyyy}");
//                    Console.WriteLine($"\tОценка: {grade:F1}");
//                }
//            }
//        }

//        // Отчёт о новых регистрациях
//        static void PrintRegistrationReport(DataSet ds)
//        {
//            DataTable registration = ds.Tables["Регистрация"];
//            DataRelation studentRegistrationRelation = ds.Relations["Students_Registrations"];
//            DataRelation courseRegistrationRelation = ds.Relations["Courses_Registrations"];

//            Console.WriteLine("Отчёт о новых регистрациях:");

//            foreach (DataRow row in registration.Rows)
//            {
//                if (row.RowState == DataRowState.Added)
//                {
//                    int registrationID = (int)row["RegistrationID"];
//                    int studentID = (int)row["StudentID"];
//                    string courseID = (string)row["CourseID"];
//                    DateTime enrollmentDate = (DateTime)row["EnrollmentDate"];
//                    double grade = (double)row["Grade"];

//                    // Получаем информацию о студенте
//                    DataRow[] studentRows = row.GetParentRows(studentRegistrationRelation);
//                    string studentName = studentRows.Length > 0 ? (string)studentRows[0]["StudentName"] : "Неизвестен";
//                    string studentEmail = studentRows.Length > 0 ? (string)studentRows[0]["Email"] : "Неизвестен";

//                    // Получаем информацию о курсе
//                    DataRow[] courseRows = row.GetParentRows(courseRegistrationRelation);
//                    string courseName = courseRows.Length > 0 ? (string)courseRows[0]["CourseName"] : "Неизвестен";
//                    string instructor = courseRows.Length > 0 ? (string)courseRows[0]["Instructor"] : "Неизвестен";

//                    Console.WriteLine($"\nРегистрация ID: {registrationID}");
//                    Console.WriteLine($"\tСтудент: {studentName} (ID: {studentID}, Email: {studentEmail})");
//                    Console.WriteLine($"\tКурс: {courseName} (ID: {courseID}, Преподаватель: {instructor})");
//                    Console.WriteLine($"\tДата регистрации: {enrollmentDate:dd.MM.yyyy}");
//                    Console.WriteLine($"\tОценка: {grade:F1}");
//                }
//            }
//        }

//        // Статистика по новым регистрациям
//        static void PrintRegistrationStatistics(DataSet ds)
//        {
//            DataTable registration = ds.Tables["Регистрация"];
//            DataRelation studentRegistrationRelation = ds.Relations["Students_Registrations"];
//            DataRelation courseRegistrationRelation = ds.Relations["Courses_Registrations"];

//            // Подсчёт количества новых регистраций для каждого студента
//            Dictionary<int, int> newRegistrationsByStudent = new Dictionary<int, int>();
//            // Подсчёт количества новых регистраций для каждого курса
//            Dictionary<string, int> newRegistrationsByCourse = new Dictionary<string, int>();

//            foreach (DataRow row in registration.Rows)
//            {
//                if (row.RowState == DataRowState.Added)
//                {
//                    int studentID = (int)row["StudentID"];
//                    string courseID = (string)row["CourseID"];

//                    // Подсчёт для студентов
//                    if (newRegistrationsByStudent.ContainsKey(studentID))
//                    {
//                        newRegistrationsByStudent[studentID]++;
//                    }
//                    else
//                    {
//                        newRegistrationsByStudent[studentID] = 1;
//                    }

//                    // Подсчёт для курсов
//                    if (newRegistrationsByCourse.ContainsKey(courseID))
//                    {
//                        newRegistrationsByCourse[courseID]++;
//                    }
//                    else
//                    {
//                        newRegistrationsByCourse[courseID] = 1;
//                    }
//                }
//            }

//            // Вывод статистики по студентам
//            Console.WriteLine("Количество новых регистраций по студентам:");
//            DataTable students = ds.Tables["Студенты"];
//            foreach (var kvp in newRegistrationsByStudent)
//            {
//                DataRow studentRow = students.Rows.Find(kvp.Key);
//                if (studentRow != null)
//                {
//                    string studentName = (string)studentRow["StudentName"];
//                    Console.WriteLine($"\t{studentName}: {kvp.Value}");
//                }
//            }

//            // Вывод статистики по курсам
//            Console.WriteLine("\nКоличество новых регистраций по курсам:");
//            DataTable courses = ds.Tables["Курсы"];
//            foreach (var kvp in newRegistrationsByCourse)
//            {
//                DataRow courseRow = courses.Rows.Find(kvp.Key);
//                if (courseRow != null)
//                {
//                    string courseName = (string)courseRow["CourseName"];
//                    Console.WriteLine($"\t{courseName}: {kvp.Value}");
//                }
//            }

//            // Общее количество новых регистраций
//            int totalNewRegistrations = newRegistrationsByStudent.Sum(kvp => kvp.Value);
//            Console.WriteLine($"\nОбщее количество новых регистраций: {totalNewRegistrations}");
//        }
//    }
//}


////Задание 14: Получение связанной информации для строки со статусом Modified
//using System;
//using System.Data;
//using System.Collections.Generic;

//namespace ModifiedRegistrationsAnalysis
//{
//    class Program
//    {
//        static void Main()
//        {
//            // Создаём DataSet и таблицы
//            DataSet ds = CreateDataSet();

//            // Заполняем таблицы тестовыми данными
//            FillTestData(ds);

//            // Создаём отношения DataRelation
//            CreateRelations(ds);

//            Console.WriteLine("=== АНАЛИЗ ИЗМЕНЁННЫХ РЕГИСТРАЦИЙ ===\n");

//            // Выводим начальное состояние данных
//            Console.WriteLine("НАЧАЛЬНОЕ СОСТОЯНИЕ ДАННЫХ:");
//            PrintDataSetState(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // Модификация нескольких регистраций
//            Console.WriteLine("МОДИФИКАЦИЯ РЕГИСТРАЦИЙ:");
//            Console.WriteLine("=====================================");
//            ModifyRegistrationGrade(ds, 1, 5.0);  // Повышаем оценку
//            ModifyRegistrationGrade(ds, 2, 3.0);  // Понижаем оценку
//            ModifyRegistrationGrade(ds, 3, 4.9);  // Оставляем почти без изменений
//            ModifyRegistrationGrade(ds, 4, 4.8);  // Повышаем оценку
//            ModifyRegistrationGrade(ds, 5, 3.5);  // Понижаем оценку
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // Отчёт об изменённых регистрациях
//            Console.WriteLine("ОТЧЁТ ОБ ИЗМЕНЁННЫХ РЕГИСТРАЦИЯХ:");
//            Console.WriteLine("=====================================");
//            PrintModifiedRegistrations(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // Дельта изменений
//            Console.WriteLine("ДЕЛЬТА ИЗМЕНЕНИЙ:");
//            Console.WriteLine("=====================================");
//            PrintGradeChangeDelta(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // Валидация перед сохранением
//            Console.WriteLine("ВАЛИДАЦИЯ ПЕРЕД СОХРАНЕНИЕМ:");
//            Console.WriteLine("=====================================");
//            bool isValid = ValidateGrades(ds);
//            Console.WriteLine(isValid ? "Все оценки корректны." : "Обнаружены некорректные оценки.");
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // Статистика изменений оценок
//            Console.WriteLine("СТАТИСТИКА ИЗМЕНЕНИЙ ОЦЕНОК:");
//            Console.WriteLine("=====================================");
//            PrintGradeChangeStatistics(ds);
//        }

//        // Создание DataSet с таблицами
//        static DataSet CreateDataSet()
//        {
//            DataSet ds = new DataSet("UniversityDB");

//            // Таблица Студенты
//            DataTable students = new DataTable("Студенты");
//            students.Columns.Add("StudentID", typeof(int));
//            students.Columns.Add("StudentName", typeof(string));
//            students.Columns.Add("Email", typeof(string));
//            students.PrimaryKey = new DataColumn[] { students.Columns["StudentID"] };

//            // Таблица Курсы
//            DataTable courses = new DataTable("Курсы");
//            courses.Columns.Add("CourseID", typeof(string));
//            courses.Columns.Add("CourseName", typeof(string));
//            courses.Columns.Add("Instructor", typeof(string));
//            courses.PrimaryKey = new DataColumn[] { courses.Columns["CourseID"] };

//            // Таблица Регистрация (промежуточная)
//            DataTable registration = new DataTable("Регистрация");
//            registration.Columns.Add("RegistrationID", typeof(int));
//            registration.Columns.Add("StudentID", typeof(int));
//            registration.Columns.Add("CourseID", typeof(string));
//            registration.Columns.Add("EnrollmentDate", typeof(DateTime));
//            registration.Columns.Add("Grade", typeof(double));
//            registration.PrimaryKey = new DataColumn[] { registration.Columns["RegistrationID"] };

//            ds.Tables.Add(students);
//            ds.Tables.Add(courses);
//            ds.Tables.Add(registration);

//            return ds;
//        }

//        // Заполнение тестовыми данными
//        static void FillTestData(DataSet ds)
//        {
//            DataTable students = ds.Tables["Студенты"];
//            DataTable courses = ds.Tables["Курсы"];
//            DataTable registration = ds.Tables["Регистрация"];

//            // Добавляем студентов
//            students.Rows.Add(101, "Иван Петров", "ivan@example.com");
//            students.Rows.Add(102, "Мария Сидорова", "maria@example.com");
//            students.Rows.Add(103, "Петр Иванов", "petr@example.com");
//            students.Rows.Add(104, "Анна Смирнова", "anna@example.com");

//            // Добавляем курсы
//            courses.Rows.Add("C001", "C# Programming", "Дмитрий Волков");
//            courses.Rows.Add("C002", "Database Design", "Светлана Морозова");
//            courses.Rows.Add("C003", "Web Development", "Алексей Новиков");
//            courses.Rows.Add("C004", "OOP Principles", "Петр Сергеев");

//            // Добавляем регистрации с оценками
//            registration.Rows.Add(1, 101, "C001", new DateTime(2025, 11, 15), 4.5);
//            registration.Rows.Add(2, 101, "C002", new DateTime(2025, 11, 20), 3.8);
//            registration.Rows.Add(3, 101, "C004", new DateTime(2025, 12, 10), 4.9);
//            registration.Rows.Add(4, 102, "C001", new DateTime(2025, 11, 15), 4.8);
//            registration.Rows.Add(5, 102, "C003", new DateTime(2025, 12, 05), 4.2);
//            registration.Rows.Add(6, 103, "C002", new DateTime(2025, 11, 20), 3.5);
//            registration.Rows.Add(7, 103, "C003", new DateTime(2025, 12, 05), 4.0);
//            registration.Rows.Add(8, 103, "C004", new DateTime(2025, 12, 10), 4.7);
//            registration.Rows.Add(9, 104, "C001", new DateTime(2025, 11, 15), 4.6);
//            registration.Rows.Add(10, 104, "C002", new DateTime(2025, 11, 20), 4.3);
//            registration.Rows.Add(11, 104, "C003", new DateTime(2025, 12, 05), 4.1);
//        }

//        // Создание отношений DataRelation
//        static void CreateRelations(DataSet ds)
//        {
//            DataTable students = ds.Tables["Студенты"];
//            DataTable courses = ds.Tables["Курсы"];
//            DataTable registration = ds.Tables["Регистрация"];

//            try
//            {
//                // Отношение: Студенты → Регистрация (один студент → много регистраций)
//                DataRelation studentRegistrationRelation = new DataRelation(
//                    "Students_Registrations",
//                    students.Columns["StudentID"],
//                    registration.Columns["StudentID"],
//                    true);

//                // Отношение: Курсы → Регистрация (один курс → много регистраций)
//                DataRelation courseRegistrationRelation = new DataRelation(
//                    "Courses_Registrations",
//                    courses.Columns["CourseID"],
//                    registration.Columns["CourseID"],
//                    true);

//                ds.Relations.Add(studentRegistrationRelation);
//                ds.Relations.Add(courseRegistrationRelation);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Ошибка при создании отношений: {ex.Message}");
//            }
//        }

//        // Вывод состояния DataSet
//        static void PrintDataSetState(DataSet ds)
//        {
//            DataTable registration = ds.Tables["Регистрация"];

//            Console.WriteLine("\nРЕГИСТРАЦИИ:");
//            foreach (DataRow row in registration.Rows)
//            {
//                string state = "";
//                switch (row.RowState)
//                {
//                    case DataRowState.Added:
//                        state = " [Добавлена]";
//                        break;
//                    case DataRowState.Modified:
//                        state = " [Изменена]";
//                        break;
//                    case DataRowState.Deleted:
//                        state = " [Удалена]";
//                        break;
//                }
//                if (row.RowState != DataRowState.Deleted)
//                {
//                    Console.WriteLine($"{row["RegistrationID"]}: Студент: {row["StudentID"]}, Курс: {row["CourseID"]}, Дата: {((DateTime)row["EnrollmentDate"]).ToShortDateString()}, Оценка: {row["Grade"]}{state}");
//                }
//                else
//                {
//                    Console.WriteLine($"{row["RegistrationID", DataRowVersion.Original]}: Студент: {row["StudentID", DataRowVersion.Original]}, Курс: {row["CourseID", DataRowVersion.Original]}, Оценка: {row["Grade", DataRowVersion.Original]} [Удалена]");
//                }
//            }
//        }

//        // Модификация оценки регистрации
//        static void ModifyRegistrationGrade(DataSet ds, int registrationID, double newGrade)
//        {
//            DataTable registration = ds.Tables["Регистрация"];

//            DataRow registrationRow = registration.Rows.Find(registrationID);

//            if (registrationRow != null)
//            {
//                double oldGrade = (double)registrationRow["Grade"];
//                registrationRow.BeginEdit();
//                registrationRow["Grade"] = newGrade;
//                registrationRow.EndEdit();

//                Console.WriteLine($"Оценка регистрации {registrationID} изменена с {oldGrade:F1} на {newGrade:F1}");
//            }
//            else
//            {
//                Console.WriteLine($"Регистрация с ID {registrationID} не найдена.");
//            }
//        }

//        // Отчёт об изменённых регистрациях
//        static void PrintModifiedRegistrations(DataSet ds)
//        {
//            DataTable registration = ds.Tables["Регистрация"];
//            DataRelation studentRegistrationRelation = ds.Relations["Students_Registrations"];
//            DataRelation courseRegistrationRelation = ds.Relations["Courses_Registrations"];

//            Console.WriteLine("Список изменённых регистраций:");

//            bool hasModified = false;

//            foreach (DataRow row in registration.Rows)
//            {
//                if (row.RowState == DataRowState.Modified)
//                {
//                    hasModified = true;
//                    int registrationID = (int)row["RegistrationID"];
//                    double oldGrade = (double)row["Grade", DataRowVersion.Original];
//                    double newGrade = (double)row["Grade", DataRowVersion.Current];

//                    // Получаем информацию о студенте
//                    DataRow[] studentRows = row.GetParentRows(studentRegistrationRelation);
//                    string studentName = studentRows.Length > 0 ? (string)studentRows[0]["StudentName"] : "Неизвестен";

//                    // Получаем информацию о курсе
//                    DataRow[] courseRows = row.GetParentRows(courseRegistrationRelation);
//                    string courseName = courseRows.Length > 0 ? (string)courseRows[0]["CourseName"] : "Неизвестен";

//                    Console.WriteLine($"Регистрация ID: {registrationID}");
//                    Console.WriteLine($"\tСтудент: {studentName}");
//                    Console.WriteLine($"\tКурс: {courseName}");
//                    Console.WriteLine($"\tСтарая оценка: {oldGrade:F1}");
//                    Console.WriteLine($"\tНовая оценка: {newGrade:F1}");
//                    Console.WriteLine();
//                }
//            }

//            if (!hasModified)
//            {
//                Console.WriteLine("Нет изменённых регистраций.");
//            }
//        }

//        // Дельта изменений оценок
//        static void PrintGradeChangeDelta(DataSet ds)
//        {
//            DataTable registration = ds.Tables["Регистрация"];
//            DataRelation studentRegistrationRelation = ds.Relations["Students_Registrations"];
//            DataRelation courseRegistrationRelation = ds.Relations["Courses_Registrations"];

//            Console.WriteLine("Дельта изменений оценок:");

//            bool hasChanges = false;

//            foreach (DataRow row in registration.Rows)
//            {
//                if (row.RowState == DataRowState.Modified)
//                {
//                    hasChanges = true;
//                    int registrationID = (int)row["RegistrationID"];
//                    double oldGrade = (double)row["Grade", DataRowVersion.Original];
//                    double newGrade = (double)row["Grade", DataRowVersion.Current];
//                    double delta = newGrade - oldGrade;
//                    string deltaSign = delta >= 0 ? "+" : "";

//                    // Получаем информацию о студенте
//                    DataRow[] studentRows = row.GetParentRows(studentRegistrationRelation);
//                    string studentName = studentRows.Length > 0 ? (string)studentRows[0]["StudentName"] : "Неизвестен";

//                    // Получаем информацию о курсе
//                    DataRow[] courseRows = row.GetParentRows(courseRegistrationRelation);
//                    string courseName = courseRows.Length > 0 ? (string)courseRows[0]["CourseName"] : "Неизвестен";

//                    Console.WriteLine($"Регистрация ID: {registrationID}");
//                    Console.WriteLine($"\tСтудент: {studentName}");
//                    Console.WriteLine($"\tКурс: {courseName}");
//                    Console.WriteLine($"\tИзменение оценки: {oldGrade:F1} → {newGrade:F1} ({deltaSign}{delta:F1})");
//                    Console.WriteLine();
//                }
//            }

//            if (!hasChanges)
//            {
//                Console.WriteLine("Нет изменённых оценок.");
//            }
//        }

//        // Валидация оценок перед сохранением
//        static bool ValidateGrades(DataSet ds)
//        {
//            DataTable registration = ds.Tables["Регистрация"];
//            bool isValid = true;

//            foreach (DataRow row in registration.Rows)
//            {
//                if (row.RowState == DataRowState.Modified || row.RowState == DataRowState.Added)
//                {
//                    double grade = (double)row["Grade", DataRowVersion.Current];

//                    if (grade < 2.0 || grade > 5.0)
//                    {
//                        int registrationID = (int)row["RegistrationID"];
//                        Console.WriteLine($"Ошибка валидации: Оценка {grade:F1} для регистрации {registrationID} вне диапазона [2.0, 5.0]");
//                        isValid = false;
//                    }
//                }
//            }

//            return isValid;
//        }

//        // Статистика изменений оценок
//        static void PrintGradeChangeStatistics(DataSet ds)
//        {
//            DataTable registration = ds.Tables["Регистрация"];
//            int increasedCount = 0;
//            int decreasedCount = 0;
//            int unchangedCount = 0;

//            foreach (DataRow row in registration.Rows)
//            {
//                if (row.RowState == DataRowState.Modified)
//                {
//                    double oldGrade = (double)row["Grade", DataRowVersion.Original];
//                    double newGrade = (double)row["Grade", DataRowVersion.Current];

//                    if (newGrade > oldGrade)
//                    {
//                        increasedCount++;
//                    }
//                    else if (newGrade < oldGrade)
//                    {
//                        decreasedCount++;
//                    }
//                    else
//                    {
//                        unchangedCount++;
//                    }
//                }
//            }

//            Console.WriteLine($"Повышено оценок: {increasedCount}");
//            Console.WriteLine($"Понижено оценок: {decreasedCount}");
//            Console.WriteLine($"Без изменений: {unchangedCount}");
//            Console.WriteLine($"Всего изменений: {increasedCount + decreasedCount + unchangedCount}");
//        }
//    }
//}


////Задание 15: Каскадное удаление с отслеживанием изменений во всех таблицах
//using System;
//using System.Data;
//using System.Collections.Generic;

//namespace CascadeDeletionAnalysis
//{
//    class Program
//    {
//        static void Main()
//        {
//            // Создаём DataSet и таблицы
//            DataSet ds = CreateDataSet();

//            // Заполняем таблицы тестовыми данными
//            FillTestData(ds);

//            // Создаём отношения с DeleteRule.Cascade
//            CreateRelations(ds);

//            Console.WriteLine("=== КАСКАДНОЕ УДАЛЕНИЕ С ОТСЛЕЖИВАНИЕМ ===\n");

//            // Выводим начальное состояние данных
//            Console.WriteLine("НАЧАЛЬНОЕ СОСТОЯНИЕ ДАННЫХ:");
//            PrintDataSetState(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // Удаляем заказчика с ID 1
//            Console.WriteLine("УДАЛЕНИЕ ЗАКАЗЧИКА С ID 1:");
//            Console.WriteLine("=====================================");
//            DeleteCustomer(ds, 1);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // Анализ удалённых строк
//            Console.WriteLine("АНАЛИЗ УДАЛЁННЫХ СТРОК:");
//            Console.WriteLine("=====================================");
//            AnalyzeDeletedRows(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // Отчёт об удалённых данных
//            Console.WriteLine("ОТЧЁТ ОБ УДАЛЁННЫХ ДАННЫХ:");
//            Console.WriteLine("=====================================");
//            PrintDeletionReport(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // Полный откат изменений
//            Console.WriteLine("ПОЛНЫЙ ОТКАТ ИЗМЕНЕНИЙ:");
//            Console.WriteLine("=====================================");
//            RejectAllChanges(ds);
//            PrintDataSetState(ds);
//        }

//        // Создание DataSet с таблицами
//        static DataSet CreateDataSet()
//        {
//            DataSet ds = new DataSet("OrderManagement");

//            // Таблица Заказчики
//            DataTable customers = new DataTable("Заказчики");
//            customers.Columns.Add("CustomerID", typeof(int));
//            customers.Columns.Add("CustomerName", typeof(string));
//            customers.Columns.Add("Email", typeof(string));
//            customers.PrimaryKey = new DataColumn[] { customers.Columns["CustomerID"] };

//            // Таблица Заказы
//            DataTable orders = new DataTable("Заказы");
//            orders.Columns.Add("OrderID", typeof(int));
//            orders.Columns.Add("OrderDate", typeof(DateTime));
//            orders.Columns.Add("CustomerID", typeof(int));
//            orders.Columns.Add("Total", typeof(decimal));
//            orders.PrimaryKey = new DataColumn[] { orders.Columns["OrderID"] };

//            // Таблица ДеталиЗаказов
//            DataTable orderDetails = new DataTable("ДеталиЗаказов");
//            orderDetails.Columns.Add("DetailID", typeof(int));
//            orderDetails.Columns.Add("OrderID", typeof(int));
//            orderDetails.Columns.Add("ProductID", typeof(int));
//            orderDetails.Columns.Add("Quantity", typeof(int));
//            orderDetails.Columns.Add("Price", typeof(decimal));
//            orderDetails.PrimaryKey = new DataColumn[] { orderDetails.Columns["DetailID"] };

//            ds.Tables.Add(customers);
//            ds.Tables.Add(orders);
//            ds.Tables.Add(orderDetails);

//            return ds;
//        }

//        // Заполнение тестовыми данными
//        static void FillTestData(DataSet ds)
//        {
//            DataTable customers = ds.Tables["Заказчики"];
//            DataTable orders = ds.Tables["Заказы"];
//            DataTable orderDetails = ds.Tables["ДеталиЗаказов"];

//            // Добавляем заказчиков
//            customers.Rows.Add(1, "Иван Иванов", "ivan@example.com");
//            customers.Rows.Add(2, "Мария Петрова", "maria@example.com");

//            // Добавляем заказы
//            orders.Rows.Add(1, new DateTime(2023, 10, 15), 1, 1000.00m);
//            orders.Rows.Add(2, new DateTime(2023, 11, 20), 1, 1500.00m);
//            orders.Rows.Add(3, new DateTime(2023, 12, 05), 2, 2000.00m);

//            // Добавляем детали заказов
//            orderDetails.Rows.Add(1, 1, 101, 2, 500.00m);
//            orderDetails.Rows.Add(2, 1, 102, 1, 500.00m);
//            orderDetails.Rows.Add(3, 2, 103, 3, 500.00m);
//            orderDetails.Rows.Add(4, 2, 104, 1, 1000.00m);
//            orderDetails.Rows.Add(5, 3, 105, 1, 2000.00m);
//        }

//        // Создание отношений с DeleteRule.Cascade
//        static void CreateRelations(DataSet ds)
//        {
//            DataTable customers = ds.Tables["Заказчики"];
//            DataTable orders = ds.Tables["Заказы"];
//            DataTable orderDetails = ds.Tables["ДеталиЗаказов"];

//            try
//            {
//                // Отношение: Заказчики → Заказы (DeleteRule=Cascade)
//                ForeignKeyConstraint customerOrderConstraint = new ForeignKeyConstraint(
//                    "FK_Customers_Orders",
//                    customers.Columns["CustomerID"],
//                    orders.Columns["CustomerID"]);

//                customerOrderConstraint.DeleteRule = Rule.Cascade;
//                orders.Constraints.Add(customerOrderConstraint);

//                // Отношение: Заказы → ДеталиЗаказов (DeleteRule=Cascade)
//                ForeignKeyConstraint orderDetailConstraint = new ForeignKeyConstraint(
//                    "FK_Orders_OrderDetails",
//                    orders.Columns["OrderID"],
//                    orderDetails.Columns["OrderID"]);

//                orderDetailConstraint.DeleteRule = Rule.Cascade;
//                orderDetails.Constraints.Add(orderDetailConstraint);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Ошибка при создании отношений: {ex.Message}");
//            }
//        }

//        // Вывод состояния DataSet
//        static void PrintDataSetState(DataSet ds)
//        {
//            DataTable customers = ds.Tables["Заказчики"];
//            DataTable orders = ds.Tables["Заказы"];
//            DataTable orderDetails = ds.Tables["ДеталиЗаказов"];

//            Console.WriteLine("\nЗАКАЗЧИКИ:");
//            foreach (DataRow row in customers.Rows)
//            {
//                if (row.RowState != DataRowState.Deleted)
//                {
//                    Console.WriteLine($"{row["CustomerID"]}: {row["CustomerName"]}, Email: {row["Email"]}");
//                }
//                else
//                {
//                    Console.WriteLine($"{row["CustomerID", DataRowVersion.Original]}: {row["CustomerName", DataRowVersion.Original]} [Удален]");
//                }
//            }

//            Console.WriteLine("\nЗАКАЗЫ:");
//            foreach (DataRow row in orders.Rows)
//            {
//                if (row.RowState != DataRowState.Deleted)
//                {
//                    Console.WriteLine($"{row["OrderID"]}: Дата: {((DateTime)row["OrderDate"]).ToShortDateString()}, Заказчик: {row["CustomerID"]}, Сумма: {row["Total"]:C}");
//                }
//                else
//                {
//                    Console.WriteLine($"{row["OrderID", DataRowVersion.Original]}: Дата: {((DateTime)row["OrderDate", DataRowVersion.Original]).ToShortDateString()}, Заказчик: {row["CustomerID", DataRowVersion.Original]}, Сумма: {row["Total", DataRowVersion.Original]:C} [Удален]");
//                }
//            }

//            Console.WriteLine("\nДЕТАЛИ ЗАКАЗОВ:");
//            foreach (DataRow row in orderDetails.Rows)
//            {
//                if (row.RowState != DataRowState.Deleted)
//                {
//                    Console.WriteLine($"{row["DetailID"]}: Заказ: {row["OrderID"]}, Продукт: {row["ProductID"]}, Кол-во: {row["Quantity"]}, Цена: {row["Price"]:C}");
//                }
//                else
//                {
//                    Console.WriteLine($"{row["DetailID", DataRowVersion.Original]}: Заказ: {row["OrderID", DataRowVersion.Original]}, Продукт: {row["ProductID", DataRowVersion.Original]}, Кол-во: {row["Quantity", DataRowVersion.Original]}, Цена: {row["Price", DataRowVersion.Original]:C} [Удалена]");
//                }
//            }
//        }

//        // Удаление заказчика
//        static void DeleteCustomer(DataSet ds, int customerID)
//        {
//            DataTable customers = ds.Tables["Заказчики"];

//            DataRow customerRow = customers.Rows.Find(customerID);

//            if (customerRow != null)
//            {
//                string customerName = (string)customerRow["CustomerName"];
//                Console.WriteLine($"Удаляем заказчика: {customerName} (ID: {customerID})");
//                customerRow.Delete();
//            }
//            else
//            {
//                Console.WriteLine($"Заказчик с ID {customerID} не найден.");
//            }
//        }

//        // Анализ удалённых строк
//        static void AnalyzeDeletedRows(DataSet ds)
//        {
//            DataTable customers = ds.Tables["Заказчики"];
//            DataTable orders = ds.Tables["Заказы"];
//            DataTable orderDetails = ds.Tables["ДеталиЗаказов"];

//            Console.WriteLine("Удалённые заказчики:");
//            foreach (DataRow row in customers.Rows)
//            {
//                if (row.RowState == DataRowState.Deleted)
//                {
//                    string customerName = (string)row["CustomerName", DataRowVersion.Original];
//                    Console.WriteLine($"\t{customerName} (ID: {row["CustomerID", DataRowVersion.Original]})");
//                }
//            }

//            Console.WriteLine("\nУдалённые заказы:");
//            foreach (DataRow row in orders.Rows)
//            {
//                if (row.RowState == DataRowState.Deleted)
//                {
//                    int orderID = (int)row["OrderID", DataRowVersion.Original];
//                    DateTime orderDate = (DateTime)row["OrderDate", DataRowVersion.Original];
//                    Console.WriteLine($"\tЗаказ {orderID} от {orderDate:dd.MM.yyyy}");
//                }
//            }

//            Console.WriteLine("\nУдалённые детали заказов:");
//            foreach (DataRow row in orderDetails.Rows)
//            {
//                if (row.RowState == DataRowState.Deleted)
//                {
//                    int detailID = (int)row["DetailID", DataRowVersion.Original];
//                    int orderID = (int)row["OrderID", DataRowVersion.Original];
//                    Console.WriteLine($"\tДеталь {detailID} для заказа {orderID}");
//                }
//            }
//        }

//        // Отчёт об удалённых данных
//        static void PrintDeletionReport(DataSet ds)
//        {
//            DataTable customers = ds.Tables["Заказчики"];
//            DataTable orders = ds.Tables["Заказы"];
//            DataTable orderDetails = ds.Tables["ДеталиЗаказов"];

//            Console.WriteLine("ПОЛНЫЙ ОТЧЁТ ОБ УДАЛЁННЫХ ДАННЫХ:");

//            // Собираем информацию об удалённых заказчиках
//            List<DataRow> deletedCustomers = new List<DataRow>();
//            foreach (DataRow row in customers.Rows)
//            {
//                if (row.RowState == DataRowState.Deleted)
//                {
//                    deletedCustomers.Add(row);
//                }
//            }

//            // Для каждого удалённого заказчика собираем информацию о его заказах и деталях
//            foreach (DataRow customerRow in deletedCustomers)
//            {
//                int customerID = (int)customerRow["CustomerID", DataRowVersion.Original];
//                string customerName = (string)customerRow["CustomerName", DataRowVersion.Original];

//                Console.WriteLine($"\nУдалённый заказчик: {customerName} (ID: {customerID})");

//                // Собираем информацию об удалённых заказах этого заказчика
//                List<DataRow> deletedOrders = new List<DataRow>();
//                foreach (DataRow orderRow in orders.Rows)
//                {
//                    if (orderRow.RowState == DataRowState.Deleted &&
//                        (int)orderRow["CustomerID", DataRowVersion.Original] == customerID)
//                    {
//                        deletedOrders.Add(orderRow);
//                    }
//                }

//                Console.WriteLine($"\tКоличество удалённых заказов: {deletedOrders.Count}");

//                // Для каждого удалённого заказа собираем информацию о его деталях
//                foreach (DataRow orderRow in deletedOrders)
//                {
//                    int orderID = (int)orderRow["OrderID", DataRowVersion.Original];
//                    DateTime orderDate = (DateTime)orderRow["OrderDate", DataRowVersion.Original];
//                    decimal total = (decimal)orderRow["Total", DataRowVersion.Original];

//                    Console.WriteLine($"\t\tУдалённый заказ {orderID} от {orderDate:dd.MM.yyyy}, сумма: {total:C}");

//                    // Собираем информацию об удалённых деталях этого заказа
//                    List<DataRow> deletedDetails = new List<DataRow>();
//                    foreach (DataRow detailRow in orderDetails.Rows)
//                    {
//                        if (detailRow.RowState == DataRowState.Deleted &&
//                            (int)detailRow["OrderID", DataRowVersion.Original] == orderID)
//                        {
//                            deletedDetails.Add(detailRow);
//                        }
//                    }

//                    Console.WriteLine($"\t\tКоличество удалённых деталей заказа: {deletedDetails.Count}");

//                    // Выводим информацию о деталях заказа
//                    foreach (DataRow detailRow in deletedDetails)
//                    {
//                        int productID = (int)detailRow["ProductID", DataRowVersion.Original];
//                        int quantity = (int)detailRow["Quantity", DataRowVersion.Original];
//                        decimal price = (decimal)detailRow["Price", DataRowVersion.Original];

//                        Console.WriteLine($"\t\t\tДеталь: Продукт {productID}, Кол-во: {quantity}, Цена: {price:C}");
//                    }
//                }
//            }

//            // Подсчёт общего количества удалённых записей
//            int totalDeletedCustomers = customers.Select(null, null, DataViewRowState.Deleted).Length;
//            int totalDeletedOrders = orders.Select(null, null, DataViewRowState.Deleted).Length;
//            int totalDeletedDetails = orderDetails.Select(null, null, DataViewRowState.Deleted).Length;

//            Console.WriteLine($"\nОБЩАЯ СТАТИСТИКА:");
//            Console.WriteLine($"\tУдалённых заказчиков: {totalDeletedCustomers}");
//            Console.WriteLine($"\tУдалённых заказов: {totalDeletedOrders}");
//            Console.WriteLine($"\tУдалённых деталей заказов: {totalDeletedDetails}");
//        }

//        // Полный откат изменений
//        static void RejectAllChanges(DataSet ds)
//        {
//            Console.WriteLine("Откатываем все изменения...");
//            ds.RejectChanges();
//            Console.WriteLine("Все изменения отменены.");
//        }
//    }
//}


////Задание 16: Проверка ссылочной надежности перед сохранением
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Text;

//class Program
//{
//    static void Main()
//    {
//        // Создаём DataSet и таблицы
//        DataSet ds = CreateDataSet();

//        // Заполняем таблицы тестовыми данными
//        FillTestData(ds);

//        // Создаём отношения
//        CreateRelationships(ds);

//        Console.WriteLine("=== ДЕМОНСТРАЦИЯ ПРОВЕРКИ ССЫЛОЧНОЙ ЦЕЛОСТНОСТИ ===\n");

//        // 1. Попытка добавить товар с несуществующей CategoryID (нарушение целостности)
//        Console.WriteLine("1. ПОПЫТКА ДОБАВИТЬ ТОВАР С НЕСУЩЕСТВУЮЩЕЙ CATEGORYID:");
//        Console.WriteLine("=====================================");
//        TryAddInvalidProduct(ds);
//        Console.WriteLine();

//        // 2. Временно отключим принуждение ограничений для введения нарушений
//        Console.WriteLine("2. ВВЕДЕНИЕ НАРУШЕНИЙ (EnforceConstraints = false):");
//        Console.WriteLine("=====================================");
//        IntroduceViolations(ds);
//        Console.WriteLine();

//        // 3. Проверка целостности и вывод отчета
//        Console.WriteLine("3. ПРОВЕРКА ЦЕЛОСТНОСТИ (CheckReferentialIntegrity):");
//        Console.WriteLine("=====================================");
//        CheckReferentialIntegrity(ds);
//        Console.WriteLine();

//        // 4. Исправление нарушений (удаление осиротевших записей)
//        Console.WriteLine("4. ИСПРАВЛЕНИЕ НАРУШЕНИЙ (удаление осиротевших):");
//        Console.WriteLine("=====================================");
//        CheckReferentialIntegrity(ds, true, true); // fix with delete
//        CheckReferentialIntegrity(ds); // check again
//        Console.WriteLine();

//        // 5. Введение нарушений снова для демонстрации установки NULL
//        IntroduceViolations(ds);

//        // 6. Исправление нарушений (установка NULL для осиротевших)
//        Console.WriteLine("5. ИСПРАВЛЕНИЕ НАРУШЕНИЙ (установка NULL):");
//        Console.WriteLine("=====================================");
//        CheckReferentialIntegrity(ds, true, false); // fix with set null
//        CheckReferentialIntegrity(ds); // check again
//        Console.WriteLine();

//        // 6. Симуляция отчета перед сохранением в БД
//        Console.WriteLine("6. ОТЧЕТ ПЕРЕД СОХРАНЕНИЕМ В БД:");
//        Console.WriteLine("=====================================");
//        SimulateSaveToDB(ds);
//        Console.WriteLine();
//    }

//    // Создание DataSet с таблицами
//    static DataSet CreateDataSet()
//    {
//        DataSet ds = new DataSet("ShopDB");

//        // Таблица Категории
//        DataTable categories = new DataTable("Категории");
//        categories.Columns.Add("CategoryID", typeof(int));
//        categories.Columns.Add("CategoryName", typeof(string));
//        categories.PrimaryKey = new DataColumn[] { categories.Columns["CategoryID"] };

//        // Таблица Товары
//        DataTable products = new DataTable("Товары");
//        products.Columns.Add("ProductID", typeof(int));
//        products.Columns.Add("ProductName", typeof(string));
//        products.Columns.Add("CategoryID", typeof(int));
//        products.Columns["CategoryID"].AllowDBNull = true; // Разрешаем NULL для демонстрации
//        products.PrimaryKey = new DataColumn[] { products.Columns["ProductID"] };

//        ds.Tables.Add(categories);
//        ds.Tables.Add(products);

//        return ds;
//    }

//    // Заполнение тестовыми данными
//    static void FillTestData(DataSet ds)
//    {
//        DataTable categories = ds.Tables["Категории"];
//        DataTable products = ds.Tables["Товары"];

//        // Добавляем категории
//        categories.Rows.Add(1, "Электроника");
//        categories.Rows.Add(2, "Книги");

//        // Добавляем товары
//        products.Rows.Add(1, "Ноутбук", 1);
//        products.Rows.Add(2, "Книга по C#", 2);
//    }

//    // Создание отношений
//    static void CreateRelationships(DataSet ds)
//    {
//        DataTable categories = ds.Tables["Категории"];
//        DataTable products = ds.Tables["Товары"];

//        // Отношение: Категории → Товары (один ко многим)
//        DataRelation rel = new DataRelation(
//            "FK_Products_Categories",
//            categories.Columns["CategoryID"],
//            products.Columns["CategoryID"],
//            true // Создаём ForeignKeyConstraint
//        );
//        ds.Relations.Add(rel);

//        // Настраиваем ForeignKeyConstraint
//        ForeignKeyConstraint fk = (ForeignKeyConstraint)products.Constraints["FK_Products_Categories"];
//        fk.DeleteRule = Rule.Cascade; // Для демонстрации, но не обязательно
//        fk.UpdateRule = Rule.Cascade;
//    }

//    // 1. Попытка добавить invalid товар
//    static void TryAddInvalidProduct(DataSet ds)
//    {
//        DataTable products = ds.Tables["Товары"];
//        try
//        {
//            products.Rows.Add(3, "Неверный товар", 99); // Несуществующая категория
//            Console.WriteLine("Товар добавлен без ошибки (не должно произойти)");
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine("Нарушение целостности поймано:");
//            Console.WriteLine(ex.Message);
//        }
//    }

//    // 2. Введение нарушений (EnforceConstraints = false)
//    static void IntroduceViolations(DataSet ds)
//    {
//        ds.EnforceConstraints = false;
//        DataTable products = ds.Tables["Товары"];

//        // Добавляем осиротевший товар
//        products.Rows.Add(4, "Осиротевший товар", 100); // Несуществующая категория

//        // Добавляем товар с NULL в CategoryID
//        products.Rows.Add(5, "Товар с NULL", null);

//        Console.WriteLine("Нарушения введены (orphan и NULL)");
//    }

//    // 3. Метод проверки целостности
//    static void CheckReferentialIntegrity(DataSet ds, bool fix = false, bool deleteOrphans = true)
//    {
//        StringBuilder report = new StringBuilder();
//        report.AppendLine("Отчет о нарушениях ссылочной целостности:");
//        report.AppendLine("=========================================");

//        List<DataRow> rowsToDelete = new List<DataRow>();
//        List<(DataRow, string)> rowsToSetNull = new List<(DataRow, string)>();

//        bool hasViolations = false;

//        foreach (DataRelation rel in ds.Relations)
//        {
//            DataTable child = rel.ChildTable;
//            string fkColumnName = rel.ChildColumns[0].ColumnName;
//            string relationName = rel.RelationName;

//            report.AppendLine($"Отношение: {relationName} (Родитель: {rel.ParentTable.TableName}, Дитя: {child.TableName})");

//            foreach (DataRow row in child.Rows)
//            {
//                if (row.RowState == DataRowState.Deleted) continue;

//                object fkValue = row[fkColumnName];

//                if (fkValue == DBNull.Value || fkValue == null)
//                {
//                    hasViolations = true;
//                    report.AppendLine($" - NULL в колонке ограничения: Таблица {child.TableName}, PK {row[child.PrimaryKey[0].ColumnName]}, FK {fkColumnName} = NULL");
//                }
//                else
//                {
//                    DataRow[] parents = row.GetParentRows(relationName);
//                    if (parents.Length == 0)
//                    {
//                        hasViolations = true;
//                        report.AppendLine($" - Осиротевшая запись: Таблица {child.TableName}, PK {row[child.PrimaryKey[0].ColumnName]}, FK {fkColumnName} = {fkValue}");

//                        if (fix)
//                        {
//                            if (deleteOrphans)
//                            {
//                                rowsToDelete.Add(row);
//                            }
//                            else if (child.Columns[fkColumnName].AllowDBNull)
//                            {
//                                rowsToSetNull.Add((row, fkColumnName));
//                            }
//                            else
//                            {
//                                report.AppendLine($"   ! Невозможно установить NULL: колонка {fkColumnName} не позволяет NULL");
//                            }
//                        }
//                    }
//                }
//            }
//        }

//        if (!hasViolations)
//        {
//            report.AppendLine("Нарушений не найдено.");
//        }

//        Console.WriteLine(report.ToString());

//        if (fix)
//        {
//            // Исправляем
//            foreach (DataRow row in rowsToDelete)
//            {
//                row.Delete();
//                Console.WriteLine($"Удалена осиротевшая запись: Таблица {row.Table.TableName}, PK {row[row.Table.PrimaryKey[0].ColumnName, DataRowVersion.Original]}");
//            }

//            foreach (var (row, colName) in rowsToSetNull)
//            {
//                row[colName] = DBNull.Value;
//                Console.WriteLine($"Установлен NULL для осиротевшей записи: Таблица {row.Table.TableName}, PK {row[row.Table.PrimaryKey[0].ColumnName]}");
//            }
//        }
//    }

//    // 6. Симуляция сохранения в БД с проверкой
//    static void SimulateSaveToDB(DataSet ds)
//    {
//        // Перед "сохранением" проверяем
//        CheckReferentialIntegrity(ds);

//        // Симулируем сохранение (в реальности - адаптеры и т.д.)
//        try
//        {
//            ds.EnforceConstraints = true; // Включаем обратно для симуляции
//            ds.AcceptChanges();
//            Console.WriteLine("Данные 'сохранены' в БД без нарушений.");
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine("Ошибка при 'сохранении' в БД:");
//            Console.WriteLine(ex.Message);
//        }
//        finally
//        {
//            ds.EnforceConstraints = false; // Сбрасываем для дальнейших тестов, если нужно
//        }
//    }
//}


////Задание 17: Использование DataRelation для фильтрации данных в DataView
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;

//class Program
//{
//    static void Main()
//    {
//        // Создаём DataSet и таблицы
//        DataSet ds = CreateDataSet();

//        // Заполняем таблицы тестовыми данными
//        FillTestData(ds);

//        // Создаём отношение
//        CreateRelationships(ds);

//        Console.WriteLine("=== ДЕМОНСТРАЦИЯ ФИЛЬТРАЦИИ ДАННЫХ ЧЕРЕЗ ОТНОШЕНИЯ ===\n");

//        // 1. Все заказы конкретного заказчика
//        Console.WriteLine("1. ВСЕ ЗАКАЗЫ КОНКРЕТНОГО ЗАКАЗЧИКА:");
//        Console.WriteLine("=====================================");
//        GetOrdersForCustomer(ds, 1);
//        Console.WriteLine();

//        // 2. Заказы, сделанные после определенной даты
//        Console.WriteLine("2. ЗАКАЗЫ ПОСЛЕ ОПРЕДЕЛЕННОЙ ДАТЫ:");
//        Console.WriteLine("=====================================");
//        DateTime afterDate = new DateTime(2023, 6, 1);
//        GetOrdersAfterDate(ds, afterDate);
//        Console.WriteLine();

//        // 3. Заказы на сумму больше указанной
//        Console.WriteLine("3. ЗАКАЗЫ НА СУММУ БОЛЬШЕ УКАЗАННОЙ:");
//        Console.WriteLine("=====================================");
//        decimal minAmount = 150.00m;
//        GetOrdersAboveAmount(ds, minAmount);
//        Console.WriteLine();

//        // 4. Комбинированные фильтры (заказы клиента X после даты Y с суммой > Z)
//        Console.WriteLine("4. КОМБИНИРОВАННЫЕ ФИЛЬТРЫ:");
//        Console.WriteLine("=====================================");
//        int customerId = 1;
//        DateTime dateY = new DateTime(2023, 4, 1);
//        decimal amountZ = 100.00m;
//        GetFilteredOrders(ds, customerId, dateY, amountZ);
//        Console.WriteLine();

//        // 5. Сортировка результатов (например, заказы по дате descending)
//        Console.WriteLine("5. ЗАКАЗЫ С СОРТИРОВКОЙ ПО ДАТЕ (DESC):");
//        Console.WriteLine("=====================================");
//        GetSortedOrders(ds, "OrderDate DESC");
//        Console.WriteLine();

//        // 6. Обработка пустых результатов
//        Console.WriteLine("6. ПРИМЕР ПУСТОГО РЕЗУЛЬТАТА:");
//        Console.WriteLine("=====================================");
//        GetOrdersForCustomer(ds, 999); // Несуществующий заказчик
//        Console.WriteLine();

//        // Для отображения в DataGridView (пример, как привязать DataView к grid)
//        // В реальном Windows Forms приложении:
//        // DataGridView grid = new DataGridView();
//        // DataView dv = new DataView(ds.Tables["Заказы"]);
//        // grid.DataSource = dv;
//    }

//    // Создание DataSet с таблицами
//    static DataSet CreateDataSet()
//    {
//        DataSet ds = new DataSet("ShopDB");

//        // Таблица Заказчики
//        DataTable customers = new DataTable("Заказчики");
//        customers.Columns.Add("CustomerID", typeof(int));
//        customers.Columns.Add("Name", typeof(string));
//        customers.Columns.Add("Email", typeof(string));
//        customers.PrimaryKey = new DataColumn[] { customers.Columns["CustomerID"] };

//        // Таблица Заказы
//        DataTable orders = new DataTable("Заказы");
//        orders.Columns.Add("OrderID", typeof(int));
//        orders.Columns.Add("CustomerID", typeof(int));
//        orders.Columns.Add("OrderDate", typeof(DateTime));
//        orders.Columns.Add("Amount", typeof(decimal));
//        orders.PrimaryKey = new DataColumn[] { orders.Columns["OrderID"] };

//        ds.Tables.Add(customers);
//        ds.Tables.Add(orders);

//        return ds;
//    }

//    // Заполнение тестовыми данными
//    static void FillTestData(DataSet ds)
//    {
//        DataTable customers = ds.Tables["Заказчики"];
//        DataTable orders = ds.Tables["Заказы"];

//        // Добавляем заказчиков
//        customers.Rows.Add(1, "Иван Иванов", "ivan@example.com");
//        customers.Rows.Add(2, "Мария Петрова", "maria@example.com");
//        customers.Rows.Add(3, "Петр Сидоров", "petr@example.com");

//        // Добавляем заказы
//        orders.Rows.Add(1, 1, new DateTime(2025, 1, 15), 100.00m);
//        orders.Rows.Add(2, 1, new DateTime(2025, 5, 20), 200.00m);
//        orders.Rows.Add(3, 2, new DateTime(2025, 3, 10), 150.00m);
//        orders.Rows.Add(4, 2, new DateTime(2025, 7, 5), 300.00m);
//        orders.Rows.Add(5, 3, new DateTime(2025, 2, 25), 120.00m);
//        orders.Rows.Add(6, 3, new DateTime(2025, 8, 15), 250.00m);
//    }

//    // Создание отношения
//    static void CreateRelationships(DataSet ds)
//    {
//        DataTable customers = ds.Tables["Заказчики"];
//        DataTable orders = ds.Tables["Заказы"];

//        // Отношение: Заказчики → Заказы (один заказчик → много заказов)
//        DataRelation rel = new DataRelation(
//            "FK_Customers_Orders",
//            customers.Columns["CustomerID"],
//            orders.Columns["CustomerID"],
//            true
//        );
//        ds.Relations.Add(rel);
//    }

//    // 1. Все заказы конкретного заказчика (используя GetChildRows)
//    static void GetOrdersForCustomer(DataSet ds, int customerID)
//    {
//        DataTable customers = ds.Tables["Заказчики"];
//        DataTable orders = ds.Tables["Заказы"];

//        DataRow customerRow = customers.Rows.Find(customerID);

//        if (customerRow == null)
//        {
//            Console.WriteLine($"Заказчик с ID {customerID} не найден.");
//            return;
//        }

//        Console.WriteLine($"Заказчик: {customerRow["Name"]}");
//        Console.WriteLine($"Email: {customerRow["Email"]}");
//        Console.WriteLine("\nЗаказы:");
//        Console.WriteLine("─────────────────────────────────────────────────");

//        DataRow[] orderRows = customerRow.GetChildRows("FK_Customers_Orders");

//        if (orderRows.Length == 0)
//        {
//            Console.WriteLine("У заказчика нет заказов.");
//            return;
//        }

//        foreach (DataRow orderRow in orderRows)
//        {
//            Console.WriteLine($" • Заказ ID: {orderRow["OrderID"]}");
//            Console.WriteLine($"   Дата: {(DateTime)orderRow["OrderDate"]:dd.MM.yyyy}");
//            Console.WriteLine($"   Сумма: {orderRow["Amount"]:F2}");
//            Console.WriteLine();
//        }
//    }

//    // 2. Заказы после определенной даты (используя DataView)
//    static void GetOrdersAfterDate(DataSet ds, DateTime afterDate)
//    {
//        DataTable orders = ds.Tables["Заказы"];

//        DataView dv = new DataView(orders);
//        dv.RowFilter = $"OrderDate > '#{afterDate:MM/dd/yyyy}#'"; // Формат для RowFilter

//        Console.WriteLine($"Заказы после {afterDate:dd.MM.yyyy}:");
//        Console.WriteLine("─────────────────────────────────────────────────");

//        if (dv.Count == 0)
//        {
//            Console.WriteLine("Нет заказов после указанной даты.");
//            return;
//        }

//        foreach (DataRowView rowView in dv)
//        {
//            DataRow orderRow = rowView.Row;
//            Console.WriteLine($" • Заказ ID: {orderRow["OrderID"]}");
//            Console.WriteLine($"   Дата: {(DateTime)orderRow["OrderDate"]:dd.MM.yyyy}");
//            Console.WriteLine($"   Сумма: {orderRow["Amount"]:F2}");
//            Console.WriteLine($"   Заказчик ID: {orderRow["CustomerID"]}");
//            Console.WriteLine();
//        }
//    }

//    // 3. Заказы на сумму больше указанной (используя DataView)
//    static void GetOrdersAboveAmount(DataSet ds, decimal minAmount)
//    {
//        DataTable orders = ds.Tables["Заказы"];

//        DataView dv = new DataView(orders);
//        dv.RowFilter = $"Amount > {minAmount.ToString(System.Globalization.CultureInfo.InvariantCulture)}";

//        Console.WriteLine($"Заказы с суммой > {minAmount:F2}:");
//        Console.WriteLine("─────────────────────────────────────────────────");

//        if (dv.Count == 0)
//        {
//            Console.WriteLine("Нет заказов с суммой больше указанной.");
//            return;
//        }

//        foreach (DataRowView rowView in dv)
//        {
//            DataRow orderRow = rowView.Row;
//            Console.WriteLine($" • Заказ ID: {orderRow["OrderID"]}");
//            Console.WriteLine($"   Дата: {(DateTime)orderRow["OrderDate"]:dd.MM.yyyy}");
//            Console.WriteLine($"   Сумма: {orderRow["Amount"]:F2}");
//            Console.WriteLine($"   Заказчик ID: {orderRow["CustomerID"]}");
//            Console.WriteLine();
//        }
//    }

//    // 4. Комбинированные фильтры (заказы клиента X после даты Y с суммой > Z)
//    static void GetFilteredOrders(DataSet ds, int customerID, DateTime dateY, decimal amountZ)
//    {
//        DataTable orders = ds.Tables["Заказы"];

//        DataView dv = new DataView(orders);
//        dv.RowFilter = $"CustomerID = {customerID} AND OrderDate > '#{dateY:MM/dd/yyyy}#' AND Amount > {amountZ.ToString(System.Globalization.CultureInfo.InvariantCulture)}";

//        Console.WriteLine($"Заказы клиента {customerID} после {dateY:dd.MM.yyyy} с суммой > {amountZ:F2}:");
//        Console.WriteLine("─────────────────────────────────────────────────");

//        if (dv.Count == 0)
//        {
//            Console.WriteLine("Нет заказов, удовлетворяющих условиям.");
//            return;
//        }

//        foreach (DataRowView rowView in dv)
//        {
//            DataRow orderRow = rowView.Row;
//            Console.WriteLine($" • Заказ ID: {orderRow["OrderID"]}");
//            Console.WriteLine($"   Дата: {(DateTime)orderRow["OrderDate"]:dd.MM.yyyy}");
//            Console.WriteLine($"   Сумма: {orderRow["Amount"]:F2}");
//            Console.WriteLine();
//        }
//    }

//    // 5. Заказы с сортировкой (используя DataView.Sort)
//    static void GetSortedOrders(DataSet ds, string sortExpression)
//    {
//        DataTable orders = ds.Tables["Заказы"];

//        DataView dv = new DataView(orders);
//        dv.Sort = sortExpression;

//        Console.WriteLine($"Все заказы, отсортированные по {sortExpression}:");
//        Console.WriteLine("─────────────────────────────────────────────────");

//        if (dv.Count == 0)
//        {
//            Console.WriteLine("Нет заказов.");
//            return;
//        }

//        foreach (DataRowView rowView in dv)
//        {
//            DataRow orderRow = rowView.Row;
//            Console.WriteLine($" • Заказ ID: {orderRow["OrderID"]}");
//            Console.WriteLine($"   Дата: {(DateTime)orderRow["OrderDate"]:dd.MM.yyyy}");
//            Console.WriteLine($"   Сумма: {orderRow["Amount"]:F2}");
//            Console.WriteLine($"   Заказчик ID: {orderRow["CustomerID"]}");
//            Console.WriteLine();
//        }
//    }
//}


////Задание 18: Экспорт иерархических данных с использованием DataRelation
//using System;
//using System.Data;
//using System.IO;
//using System.Text;

//class Program
//{
//    static void Main()
//    {
//        // Создаём DataSet и таблицы
//        DataSet ds = CreateDataSet();

//        // Заполняем таблицы тестовыми данными
//        FillTestData(ds);

//        // Создаём отношения
//        CreateRelationships(ds);

//        Console.WriteLine("=== ДЕМОНСТРАЦИЯ ЭКСПОРТА ИЕРАРХИЧЕСКИХ ДАННЫХ ===\n");

//        // 1. Экспорт в XML с сохранением иерархии
//        Console.WriteLine("1. ЭКСПОРТ В XML:");
//        Console.WriteLine("=====================================");
//        ExportToXml(ds, "hierarchy.xml");
//        Console.WriteLine();

//        // 2. Рекурсивный обход иерархии и вывод в консоль
//        Console.WriteLine("2. РЕКУРСИВНЫЙ ОБХОД ИЕРАРХИИ:");
//        Console.WriteLine("=====================================");
//        TraverseHierarchy(ds.Tables["Заказчики"], 0);
//        Console.WriteLine();

//        // 3. Экспорт в CSV с иерархией (индентированный)
//        Console.WriteLine("3. ЭКСПОРТ В CSV С ИЕРАРХИЕЙ:");
//        Console.WriteLine("=====================================");
//        ExportToHierarchicalCsv(ds.Tables["Заказчики"], "hierarchy.csv");
//        Console.WriteLine();

//        // 4. Экспорт в JSON с иерархией
//        Console.WriteLine("4. ЭКСПОРТ В JSON:");
//        Console.WriteLine("=====================================");
//        string json = ExportToJson(ds.Tables["Заказчики"]);
//        File.WriteAllText("hierarchy.json", json);
//        Console.WriteLine("Экспортировано в hierarchy.json");
//        Console.WriteLine("Фрагмент JSON:");
//        Console.WriteLine(json.Substring(0, Math.Min(200, json.Length)) + "...");
//        Console.WriteLine();

//        // 5. Импорт из XML обратно в DataSet
//        Console.WriteLine("5. ИМПОРТ ИЗ XML:");
//        Console.WriteLine("=====================================");
//        DataSet importedDs = ImportFromXml("hierarchy.xml");
//        if (importedDs != null)
//        {
//            Console.WriteLine("Импорт успешен. Проверка данных:");
//            TraverseHierarchy(importedDs.Tables["Заказчики"], 0);
//        }
//        Console.WriteLine();

//        // 6. Обработка ошибок (пример с несуществующим файлом)
//        Console.WriteLine("6. ОБРАБОТКА ОШИБОК:");
//        Console.WriteLine("=====================================");
//        ImportFromXml("nonexistent.xml");
//        Console.WriteLine();
//    }

//    // Создание DataSet с таблицами
//    static DataSet CreateDataSet()
//    {
//        DataSet ds = new DataSet("ShopDB");

//        // Таблица Заказчики
//        DataTable customers = new DataTable("Заказчики");
//        customers.Columns.Add("CustomerID", typeof(int));
//        customers.Columns.Add("Name", typeof(string));
//        customers.Columns.Add("Email", typeof(string));
//        customers.PrimaryKey = new DataColumn[] { customers.Columns["CustomerID"] };

//        // Таблица Заказы
//        DataTable orders = new DataTable("Заказы");
//        orders.Columns.Add("OrderID", typeof(int));
//        orders.Columns.Add("CustomerID", typeof(int));
//        orders.Columns.Add("OrderDate", typeof(DateTime));
//        orders.Columns.Add("Amount", typeof(decimal));
//        orders.PrimaryKey = new DataColumn[] { orders.Columns["OrderID"] };

//        // Таблица Детали Заказов
//        DataTable orderDetails = new DataTable("ДеталиЗаказов");
//        orderDetails.Columns.Add("DetailID", typeof(int));
//        orderDetails.Columns.Add("OrderID", typeof(int));
//        orderDetails.Columns.Add("ProductName", typeof(string));
//        orderDetails.Columns.Add("Quantity", typeof(int));
//        orderDetails.Columns.Add("Price", typeof(decimal));
//        orderDetails.PrimaryKey = new DataColumn[] { orderDetails.Columns["DetailID"] };

//        ds.Tables.Add(customers);
//        ds.Tables.Add(orders);
//        ds.Tables.Add(orderDetails);

//        return ds;
//    }

//    // Заполнение тестовыми данными
//    static void FillTestData(DataSet ds)
//    {
//        DataTable customers = ds.Tables["Заказчики"];
//        DataTable orders = ds.Tables["Заказы"];
//        DataTable orderDetails = ds.Tables["ДеталиЗаказов"];

//        // Добавляем заказчиков
//        customers.Rows.Add(1, "Иван Иванов", "ivan@example.com");
//        customers.Rows.Add(2, "Мария Петрова", "maria@example.com");

//        // Добавляем заказы
//        orders.Rows.Add(1, 1, new DateTime(2025, 1, 15), 100.00m);
//        orders.Rows.Add(2, 1, new DateTime(2025, 5, 20), 200.00m);
//        orders.Rows.Add(3, 2, new DateTime(2025, 3, 10), 150.00m);

//        // Добавляем детали заказов
//        orderDetails.Rows.Add(1, 1, "Товар A", 2, 30.00m);
//        orderDetails.Rows.Add(2, 1, "Товар B", 1, 40.00m);
//        orderDetails.Rows.Add(3, 2, "Товар C", 3, 50.00m);
//        orderDetails.Rows.Add(4, 3, "Товар D", 1, 150.00m);
//    }

//    // Создание отношений
//    static void CreateRelationships(DataSet ds)
//    {
//        DataTable customers = ds.Tables["Заказчики"];
//        DataTable orders = ds.Tables["Заказы"];
//        DataTable orderDetails = ds.Tables["ДеталиЗаказов"];

//        // Отношение: Заказчики → Заказы
//        DataRelation rel1 = new DataRelation(
//            "FK_Customers_Orders",
//            customers.Columns["CustomerID"],
//            orders.Columns["CustomerID"],
//            true
//        );
//        ds.Relations.Add(rel1);

//        // Отношение: Заказы → ДеталиЗаказов
//        DataRelation rel2 = new DataRelation(
//            "FK_Orders_OrderDetails",
//            orders.Columns["OrderID"],
//            orderDetails.Columns["OrderID"],
//            true
//        );
//        ds.Relations.Add(rel2);
//    }

//    // Экспорт в XML с сохранением схемы и отношений
//    static void ExportToXml(DataSet ds, string filePath)
//    {
//        try
//        {
//            ds.WriteXml(filePath, XmlWriteMode.WriteSchema);
//            Console.WriteLine($"Данные экспортированы в XML: {filePath}");
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Ошибка экспорта в XML: {ex.Message}");
//        }
//    }

//    // Рекурсивный метод для обхода иерархии (для консольного вывода)
//    static void TraverseHierarchy(DataTable table, int level)
//    {
//        foreach (DataRow row in table.Rows)
//        {
//            string indent = new string(' ', level * 2);
//            Console.WriteLine($"{indent}Таблица: {table.TableName}");
//            foreach (DataColumn col in table.Columns)
//            {
//                Console.WriteLine($"{indent}  {col.ColumnName}: {row[col]}");
//            }
//            Console.WriteLine();

//            // Получаем дочерние отношения
//            foreach (DataRelation rel in table.ChildRelations)
//            {
//                DataRow[] childRows = row.GetChildRows(rel);
//                foreach (DataRow childRow in childRows)
//                {
//                    TraverseHierarchyRecursive(childRow, level + 1, rel.ChildTable);
//                }
//            }
//        }
//    }

//    static void TraverseHierarchyRecursive(DataRow row, int level, DataTable table)
//    {
//        string indent = new string(' ', level * 2);
//        Console.WriteLine($"{indent}Таблица: {table.TableName}");
//        foreach (DataColumn col in table.Columns)
//        {
//            Console.WriteLine($"{indent}  {col.ColumnName}: {row[col]}");
//        }
//        Console.WriteLine();

//        // Рекурсия для следующих уровней
//        foreach (DataRelation rel in table.ChildRelations)
//        {
//            DataRow[] childRows = row.GetChildRows(rel);
//            foreach (DataRow childRow in childRows)
//            {
//                TraverseHierarchyRecursive(childRow, level + 1, rel.ChildTable);
//            }
//        }
//    }

//    // Экспорт в CSV с иерархией (индентированный текст)
//    static void ExportToHierarchicalCsv(DataTable table, string filePath)
//    {
//        try
//        {
//            StringBuilder sb = new StringBuilder();
//            BuildHierarchicalCsv(table, 0, sb);
//            File.WriteAllText(filePath, sb.ToString());
//            Console.WriteLine($"Данные экспортированы в CSV: {filePath}");
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Ошибка экспорта в CSV: {ex.Message}");
//        }
//    }

//    static void BuildHierarchicalCsv(DataTable table, int level, StringBuilder sb)
//    {
//        foreach (DataRow row in table.Rows)
//        {
//            string indent = new string(' ', level * 2);
//            sb.AppendLine($"{indent}Таблица: {table.TableName}");
//            foreach (DataColumn col in table.Columns)
//            {
//                sb.AppendLine($"{indent}  {col.ColumnName},{row[col]}");
//            }
//            sb.AppendLine();

//            // Дочерние
//            foreach (DataRelation rel in table.ChildRelations)
//            {
//                DataRow[] childRows = row.GetChildRows(rel);
//                foreach (DataRow childRow in childRows)
//                {
//                    BuildHierarchicalCsvRecursive(childRow, level + 1, rel.ChildTable, sb);
//                }
//            }
//        }
//    }

//    static void BuildHierarchicalCsvRecursive(DataRow row, int level, DataTable table, StringBuilder sb)
//    {
//        string indent = new string(' ', level * 2);
//        sb.AppendLine($"{indent}Таблица: {table.TableName}");
//        foreach (DataColumn col in table.Columns)
//        {
//            sb.AppendLine($"{indent}  {col.ColumnName},{row[col]}");
//        }
//        sb.AppendLine();

//        // Рекурсия
//        foreach (DataRelation rel in table.ChildRelations)
//        {
//            DataRow[] childRows = row.GetChildRows(rel);
//            foreach (DataRow childRow in childRows)
//            {
//                BuildHierarchicalCsvRecursive(childRow, level + 1, rel.ChildTable, sb);
//            }
//        }
//    }

//    // Экспорт в JSON (рекурсивный)
//    static string ExportToJson(DataTable table)
//    {
//        StringBuilder sb = new StringBuilder();
//        sb.AppendLine("[");
//        bool first = true;
//        foreach (DataRow row in table.Rows)
//        {
//            if (!first) sb.AppendLine(",");
//            first = false;
//            BuildJsonRecursive(row, table, sb, 0);
//        }
//        sb.AppendLine("]");
//        return sb.ToString();
//    }

//    static void BuildJsonRecursive(DataRow row, DataTable table, StringBuilder sb, int level)
//    {
//        string indent = new string(' ', level * 2);
//        sb.Append($"{indent}{{");
//        bool firstCol = true;
//        foreach (DataColumn col in table.Columns)
//        {
//            if (!firstCol) sb.Append(",");
//            firstCol = false;
//            string value = row[col].ToString().Replace("\"", "\\\"");
//            sb.Append($"\"{col.ColumnName}\":\"{value}\"");
//        }

//        // Дочерние таблицы
//        foreach (DataRelation rel in table.ChildRelations)
//        {
//            DataRow[] childRows = row.GetChildRows(rel);
//            if (childRows.Length > 0)
//            {
//                sb.Append($",\"{rel.ChildTable.TableName}\":[");
//                bool firstChild = true;
//                foreach (DataRow childRow in childRows)
//                {
//                    if (!firstChild) sb.Append(",");
//                    firstChild = false;
//                    BuildJsonRecursive(childRow, rel.ChildTable, sb, level + 1);
//                }
//                sb.Append("]");
//            }
//        }
//        sb.Append("}");
//    }

//    // Импорт из XML
//    static DataSet ImportFromXml(string filePath)
//    {
//        try
//        {
//            DataSet ds = new DataSet();
//            ds.ReadXml(filePath, XmlReadMode.ReadSchema);
//            Console.WriteLine($"Данные импортированы из XML: {filePath}");
//            return ds;
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Ошибка импорта из XML: {ex.Message}");
//            return null;
//        }
//    }
//}