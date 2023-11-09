using System.ComponentModel.DataAnnotations;

namespace API_asistecia;

public class Login {
    [Required]
    public string mail { get; set; }
    [Required]
    public string pass { get; set; }
}