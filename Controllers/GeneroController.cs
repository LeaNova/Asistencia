using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_asistecia;

[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[AllowAnonymous]
[Route("genero")]
public class GeneroController : ControllerBase {
    private readonly DataContext context;

    public GeneroController(DataContext context) {
        this.context = context;
    }

    //Obtener
    [HttpGet("get")]
    public async Task<ActionResult<Genero>> obtener() {
        try {
            var listaRoles = await context.generos.ToListAsync();

            return Ok(listaRoles);
        } catch (Exception ex) {
            return BadRequest(ex.Message);
        }
    }
}
