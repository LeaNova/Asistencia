namespace API_asistecia;

public class UsuarioInfo {
    public UsuarioInfo(Usuario u) {
        this.idUsuario = u.idUsuario;
        this.nombre = u.nombre;
        this.apellido = u.apellido;
        this.fechaNac =  u.fechaNac;
        this.dni = u.dni;
        this.genero = u.genero.nombre;
        this.estCivil = u.estCivil.nombre;
        this.direccion = u.direccion;
        this.telefono = u.telefono;
        this.mail = u.mail;
        this.fechaIngreso = u.fechaIngreso.Value;
        this.rol = u.rol.nombre;
    }

    public int idUsuario { get; set; }
    public string nombre { get; set; }
    public string apellido { get; set; }
    public DateTime fechaNac { get; set; }
    public string dni { get; set; }
    public string genero { get; set; }
    public string estCivil { get; set; }
    public string direccion { get; set; }
    public string telefono { get; set; }
    public string mail { get; set; }
    public DateTime fechaIngreso { get; set; }
    public string rol { get; set; }
}