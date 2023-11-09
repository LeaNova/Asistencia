using System.ComponentModel.DataAnnotations;

namespace API_asistecia;

public class Familiar {
    [Key]
    public int idFamiliar { get; set; }
    public string nombre { get; set; }
    public string apellido { get; set; }
    public string dni { get; set; }
    public int idUsuario { get; set; }
    public int idRelacion { get; set; }
}