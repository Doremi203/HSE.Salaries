namespace HSE.Salaries;

/// <summary>
/// Класс для работы с файлом зарплатных данных
/// </summary>
internal static class Csv
{
    /// <summary>
    /// Заголовки файла
    /// </summary>
    private static readonly string[] Headings =
    {
        "", 
        "work_year", 
        "experience_level", 
        "employment_type", 
        "job_title", 
        "salary", 
        "salary_currency", 
        "salary_in_usd", 
        "employee_residence", 
        "remote_ratio", 
        "company_location", 
        "company_size" 
    };

    private const string Delimeter = ",";

    /// <summary>
    /// Чтение файла зарплатных данных
    /// </summary>
    /// <param name="filepath"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static List<Employee> Read(string filepath)
    {
        if (!filepath.EndsWith(".csv")) 
            throw new Exception("Некорректное имя файла, требуется расширение csv.");
        if (!File.Exists(filepath))
            throw new Exception("Файл с указанным именем не существует");
        var employees = new List<Employee>();

        using var reader = new StreamReader(filepath);
        var lineNumber = 1;
        while (reader.ReadLine() is { } line)
        {
            var fields = line.Split(Delimeter);
            if (lineNumber == 1)
            {
                if (fields == null || !fields.SequenceEqual(Headings))
                {
                    throw new Exception("Некорректные заголовки в файле");
                }
            }
            else
            {
                employees.Add(ParseEmployee(fields, lineNumber));
            }

            lineNumber++;
        }
        reader.Close();
        
        if (employees.Count == 0)
        {
            throw new Exception("Считанный файл не содержит информации");
        }
        
        return employees;
    }

    private static Employee ParseEmployee(string[] fields, long lineNumber)
    {
        if (fields is null || fields.Length != Headings.Length)
            throw new Exception($"Некорректный формат данных в строке {lineNumber}.");
        try
        {
            //Проверка кодов стран
            //Проверка кодов валют
            if (!Dictionaries.Countries.ContainsKey(fields[8]) 
                || !Dictionaries.Countries.ContainsKey(fields[10]) 
                || !Dictionaries.Currencies.Contains(fields[6]))
                throw new Exception();
            
            //Проверка полей, что они представляют из себя названия enum
            var experienceLevel = Enum.Parse<ExperienceLevel>(fields[2]);
            var employmentType = Enum.Parse<EmploymentType>(fields[3]);
            var remoteRatio = Enum.Parse<RemoteRatio>(fields[9]);
            var companySize = Enum.Parse<CompanySize>(fields[11]);
            
            if (!Enum.IsDefined(typeof(ExperienceLevel), experienceLevel) 
                || !Enum.IsDefined(typeof(EmploymentType), employmentType) 
                || !Enum.IsDefined(typeof(RemoteRatio), remoteRatio) 
                || !Enum.IsDefined(typeof(CompanySize), companySize))
                throw new Exception();
            
            return new Employee
            (
                Number: int.Parse(fields[0]),
                WorkYear: int.Parse(fields[1]),
                ExperienceLevel: experienceLevel,
                EmploymentType: employmentType,
                JobTitle: fields[4],
                Salary: int.Parse(fields[5]),
                SalaryCurrency: fields[6],
                SalaryInUsd: int.Parse(fields[7]),
                Residence: fields[8],
                RemoteRatio: remoteRatio,
                CompanyLocation: fields[10],
                CompanySize: companySize
            );
        }
        catch (Exception ex)
        {
            throw new Exception($"Некорректный формат данных в строке {lineNumber}.", ex);
        }
    }
    
    /// <summary>
    /// Сохранение данных по зарплатам в файл
    /// </summary>
    /// <param name="filepath"></param>
    /// <param name="employees"></param>
    /// <exception cref="Exception"></exception>
    public static void Write(string? filepath, List<Employee> employees)
    {
        if (filepath is null || !filepath.EndsWith(".csv")) 
            throw new Exception("Некорректное имя файла, требуется расширение csv.");
        using var sw = new StreamWriter(filepath);
        sw.WriteLine(string.Join(Delimeter, Headings));
        employees.ForEach(e =>
        {
            sw.WriteLine(e.ToString(Delimeter));
        });
    }
}