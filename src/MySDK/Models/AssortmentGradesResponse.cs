namespace MySDK.Models;

/// <summary>
/// Response containing assortment grades
/// </summary>
public class AssortmentGradesResponse
{
    public List<AssortmentGrade> AssortmentGrades { get; set; } = new();
}
