using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_asistecia;

[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[AllowAnonymous]
[Route("rol")]
public class RolController : ControllerBase {
    private readonly DataContext context;

    public RolController(DataContext context) {
        this.context = context;
    }

    //Obtener
    [HttpGet("get")]
    public async Task<ActionResult<Rol>> obtener() {
        try {
            var listaRoles = await context.roles
                .Where(x => x.disponible)
                .ToListAsync();

            return Ok(listaRoles);
        } catch (Exception ex) {
            return BadRequest(ex.Message);
        }
    }
}
