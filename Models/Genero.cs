using System.ComponentModel.DataAnnotations;

namespace API_asistecia;

public class Genero {
    [Key]
    public int idGenero { get; set; }
    public string nombre { get; set; }
}