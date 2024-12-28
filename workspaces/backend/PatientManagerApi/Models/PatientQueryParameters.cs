namespace PatientManagerApi.Models;

public class PatientQueryParameters
{
    public string? GivenName { get; set; }
    public string? FamilyName { get; set; }
    public string? SortBy { get; set; }
    public string? SortDirection { get; set; } // "asc" o "desc"

    // Validazione della direzione di ordinamento
    public bool IsValidSortDirection()
    {
        if (string.IsNullOrEmpty(SortDirection)) return true;
        return SortDirection.ToLower() == "asc" || SortDirection.ToLower() == "desc";
    }

    // Validazione del campo di ordinamento
    public bool IsValidSortField()
    {
        if (string.IsNullOrEmpty(SortBy)) return true;
        
        var validFields = new[] { "id", "givenname", "familyname", "birthdate", "sex" };
        return validFields.Contains(SortBy.ToLower());
    }
} 