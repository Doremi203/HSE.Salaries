namespace HSE.Salaries;

/// <summary>
/// Элемент меню приложения
/// </summary>
/// <param name="Input">Команда вызова</param>
/// <param name="Description">Описание</param>
/// <param name="Action">Действие</param>
internal record MenuItem(string Input, string Description, Action Action);

internal static class Program
{
    private static List<Employee>? _employees;
    private static bool _isCanceled;
    
    /// <summary>
    /// Меню приложения
    /// </summary>
    private static readonly MenuItem[] Menu = {
        new ("0", "выйти из программы", () => _isCanceled = true),
        new ("1", "загрузить файл", LoadFile),
        new ("2", "вывести информацию о сотрудниках в US", ShowUnitedStatesEmployees),
        new ("3", "вывести отсортированный список", ShowListSortedByResidentCountryAndSalaryInUsd),
        new ("4", "вывести информацию о сотрудниках не на полной занятости", ShowNotFullTimeEmployees),
        new ("5a", "вывести общее количество строк с данными о зарплатах", ShowEmployeeDataCount),
        new ("5b", "вывести количество записей о сотрудниках в каждой стране", ShowEmployeeDataCountByResidence),
        new ("5c", "вывести количество дата-инженеров в каждой стране", ShowDataEngineerCountByResidence),
        new ("5d", "вывести количетсво сотрудников из США, которые не работают в США", ShowUsEmployeeNotWorkingInUsCount),
    };

    private static void Main()
    {
        while (!_isCanceled)
        {
            ShowMenu();
            ProcessMenu();
        }
    }

    /// <summary>
    /// Вывод меню приложения
    /// </summary>
    private static void ShowMenu()
    {
        foreach (var item in Menu)
        {
            Console.WriteLine($"Введите \"{item.Input}\", чтобы {item.Description}");
        }
    }
    
    /// <summary>
    /// Обработка выбранного пользователем элемента меню приложения
    /// </summary>
    /// <exception cref="Exception"></exception>
    private static void ProcessMenu()
    {
        try
        {
            var input = Console.ReadLine();
            var menuItem = Menu.FirstOrDefault(i => i.Input == input);
            if (menuItem is null)
                throw new Exception("Несуществующая команда, повторите ввод");
            menuItem.Action();
        }
        catch (IOException)
        {
            Console.WriteLine("Закройте файл, над которым проводится операция");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
    
    /// <summary>
    /// Загрузка файла
    /// </summary>
    private static void LoadFile()
    {
        Console.WriteLine("Введите имя или адрес файла");
        var filepath = Console.ReadLine();
        _employees = Csv.Read(filepath!);
    }
    
    /// <summary>
    /// Выборка сотрудников из США
    /// </summary>
    private static void ShowUnitedStatesEmployees()
    {
        AssertEmployees();
            
        var usEmployees = _employees!.Where(e => e.Residence == "US").ToList();
        usEmployees.ForEach(e => Console.WriteLine(e.ToString()));
        Csv.Write("US_ds_salaries.csv", usEmployees);
    }

    /// <summary>
    /// Выборка отсортированного по стране резидентства и зарплате списка
    /// </summary>
    private static void ShowListSortedByResidentCountryAndSalaryInUsd()
    {
        AssertEmployees();
            
        var caEmployees = _employees!.Where(e => e.Residence == "CA").ToList();
        var sortedEmployees = _employees!.OrderBy(e => e.Residence)
            .ThenBy(e => e.SalaryInUsd)
            .ToList();
                
        Console.WriteLine(caEmployees.Max(e => e.SalaryInUsd) - caEmployees.Min(e => e.SalaryInUsd));
        sortedEmployees.ForEach(e => Console.WriteLine(e.ToString()));
        Csv.Write("sorted_ds_salaries.csv", sortedEmployees);
    }
    
    /// <summary>
    /// Выборка сотрудников не на полной занятости
    /// </summary>
    private static void ShowNotFullTimeEmployees()
    {
        AssertEmployees();
            
        var notFullTimeEmployees = _employees!.Where(e => e.EmploymentType != EmploymentType.FT).ToList();
                
        Console.WriteLine(notFullTimeEmployees.Average(e => e.SalaryInUsd));
        notFullTimeEmployees.ForEach(e => Console.WriteLine(e.ToString()));
                
        Console.WriteLine("Введите имя или адрес файла, в который вы хотите сохранить данные");
        var fileNameIsCorrect = false;
        while (!fileNameIsCorrect)
        {
            try
            {
                var filepath = Console.ReadLine();
                Csv.Write(filepath, notFullTimeEmployees);
                fileNameIsCorrect = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Введите другое имя файла");
            }
        }
    }
    
    /// <summary>
    /// Вывод числа строк в файле с данными о зарплатах
    /// </summary>
    private static void ShowEmployeeDataCount()
    {
        AssertEmployees();
            
        Console.WriteLine($"Общее количество строк с данными о зарплатах: {_employees!.Count}");
    }

    /// <summary>
    /// Выборка числа сотрудников по стране резидентства
    /// </summary>
    private static void ShowEmployeeDataCountByResidence()
    {
        AssertEmployees();
            
        var groupsByResidence = _employees!.GroupBy(e => e.Residence)
            .ToDictionary(grouping => grouping.Key, grouping => grouping.ToList());
        Console.WriteLine($"Количество групп по странам: {groupsByResidence.Count}");
        foreach (var group in groupsByResidence)
        {
            Console.WriteLine(
                $"{Dictionaries.Countries[group.Key]}: {group.Value.Count}");
        }
    }
    
    /// <summary>
    /// Выборка дата-инженеров по стране резидентства
    /// </summary>
    private static void ShowDataEngineerCountByResidence()
    {
        AssertEmployees();
            
        var groupsByResidence = _employees!.GroupBy(e => e.Residence);
        Console.WriteLine("Количество дата-инженеров в каждой стране:");
        foreach (var group in groupsByResidence)
        {
            Console.WriteLine(
                $"{Dictionaries.Countries[group.Key]}: {group.Count(employee => employee.JobTitle == "Data Engineer")}");
        }
            
    }

    /// <summary>
    /// Выборка работников, проживающих в США, которые работают не на компанию из США
    /// </summary>
    private static void ShowUsEmployeeNotWorkingInUsCount()
    {
        AssertEmployees();
            
        Console.WriteLine(
            $"Количество работников, проживающих в США, которые работают не на компанию из США: {_employees!.Count(employee => employee.Residence == "US" && employee.CompanyLocation != "US")}");
    }
    
    /// <summary>
    /// Контроль, что данные по зарплатам загружены
    /// </summary>
    /// <exception cref="Exception"></exception>
    private static void AssertEmployees()
    {
        if (_employees is null) 
            throw new Exception("Сначала необходимо загрузить файл");
    }
}