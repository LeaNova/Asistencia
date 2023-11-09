using System.ComponentModel.DataAnnotations;

namespace API_asistecia;

public class Ingreso {
    [Key]
    public string codIngreso { get; set; }
    public DateTime fecha { get; set; }
}