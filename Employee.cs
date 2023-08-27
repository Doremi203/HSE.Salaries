namespace HSE.Salaries;

/// <summary>
/// Информация о сотруднике
/// </summary>
/// <param name="Number"></param>
/// <param name="WorkYear"></param>
/// <param name="ExperienceLevel"></param>
/// <param name="EmploymentType"></param>
/// <param name="JobTitle"></param>
/// <param name="Salary"></param>
/// <param name="SalaryCurrency"></param>
/// <param name="SalaryInUsd"></param>
/// <param name="Residence"></param>
/// <param name="RemoteRatio"></param>
/// <param name="CompanyLocation"></param>
/// <param name="CompanySize"></param>
public record Employee
(
    int Number,
    
    int WorkYear,

    ExperienceLevel ExperienceLevel,

    EmploymentType EmploymentType,

    string JobTitle,
    
    int Salary,

    string SalaryCurrency,

    int SalaryInUsd,
    
    string Residence,

    RemoteRatio RemoteRatio,

    string CompanyLocation,

    CompanySize CompanySize
)
{
    ///<inheritdoc/>
    public override string ToString() => string.Join(", ",
        $"Number: {Number}",
        $"WorkYear: {WorkYear}",
        $"ExperienceLevel: {Enum.GetName(ExperienceLevel)}",
        $"EmploymentType: {Enum.GetName(EmploymentType)}",
        $"JobTitle: {JobTitle}",
        $"Salary: {Salary}",
        $"SalaryCurrency: {SalaryCurrency}",
        $"SalaryInUsd: {SalaryInUsd}",
        $"EmployeeResidence: {Residence}",
        $"RemoteRatio: {(int)RemoteRatio}",
        $"CompanyLocation: {CompanyLocation}",
        $"CompanySize: {Enum.GetName(CompanySize)}");
    
    /// <summary>
    /// Преобразование в строку с использованием разделителя для атрибутов
    /// </summary>
    /// <param name="delimeter"></param>
    /// <returns></returns>
    public string ToString(string delimeter) => string.Join(delimeter,
        $"{Number}",
        $"{WorkYear}",
        $"{Enum.GetName(ExperienceLevel)}",
        $"{Enum.GetName(EmploymentType)}",
        $"{JobTitle}",
        $"{Salary}",
        $"{SalaryCurrency}",
        $"{SalaryInUsd}",
        $"{Residence}",
        $"{RemoteRatio}",
        $"{CompanyLocation}",
        $"{Enum.GetName(CompanySize)}");
}