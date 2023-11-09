using System.ComponentModel.DataAnnotations;

namespace API_asistecia;

public class EstCivil {
    [Key]
    public int idEstCivil { get; set; }
    public string nombre { get; set; }
}