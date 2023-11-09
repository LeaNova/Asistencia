using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_asistecia;

public class Usuario {
    [Key]
    public int idUsuario { get; set; }
    public string nombre { get; set; }
    public string apellido { get; set; }
    public DateTime fechaNac { get; set; }
    public string dni { get; set; }
    public int idGenero { get; set; }
    public int idEstCivil { get; set; }
    public string direccion { get; set; }
    public string telefono { get; set; }
    public string mail { get; set; }
    public string pass { get; set; }
    public DateTime? fechaIngreso { get; set; }
    public int idRol { get; set; }
    public bool? disponible { get; set; }

    // Clases Foraneas
    [ForeignKey(nameof(idGenero))]
    public Genero? genero { get; set; }

    [ForeignKey(nameof(idEstCivil))]
    public EstCivil? estCivil { get; set; }

    [ForeignKey(nameof(idRol))]
    public Rol? rol { get; set; }
}