using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientManagerApi.Models;

public class Patient
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }                    // Chiave primaria auto-incrementante
    public string FamilyName { get; set; } = "";   // Cognome
    public string GivenName { get; set; } = "";    // Nome
    public DateTime BirthDate { get; set; }        // Data di nascita
    public string Sex { get; set; } = "";          // Sesso

    // Proprietà di navigazione per i parametri
    public ICollection<Parameter> Parameters { get; set; } = new List<Parameter>();
}