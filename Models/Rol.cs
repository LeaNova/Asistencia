using System.ComponentModel.DataAnnotations;

namespace API_asistecia;

public class Rol {
    [Key]
    public int idRol { get; set; }
    public string nombre { get; set; }
    public bool disponible { get; set; }
}