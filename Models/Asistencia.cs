using System.ComponentModel.DataAnnotations.Schema;

namespace API_asistecia;

public class Asistencia {
    public string codIngreso { get; set; }
    public int idUsuario { get; set; }
    public string? horaIngreso { get; set; }
    public string? horaSalida { get; set; }

    [ForeignKey(nameof(codIngreso))]
    public Ingreso? ingreso { get; set; }
    [ForeignKey(nameof(idUsuario))]
    public Usuario? usuario { get; set; }
}