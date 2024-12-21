namespace PatientManagerApi.Models;

public class Parameter
{
    public int ID { get; set; }
    public string Name { get; set; } = "";
    public string Value { get; set; } = "";
    public bool Alarm { get; set; }
    public int PatientID { get; set; }
} 