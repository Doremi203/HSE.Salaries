namespace HSE.Salaries;

/// <summary>
/// Процент удаленной работы
/// </summary>
public enum RemoteRatio
{
    /// <summary>
    /// Нет удаленной работы
    /// </summary>
    NR = 0,
    
    /// <summary>
    /// Частично удаленная
    /// </summary>
    PR = 50,
    
    /// <summary>
    /// Полностью удаленная
    /// </summary>
    FR = 100,
}