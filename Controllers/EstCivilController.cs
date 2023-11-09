using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_asistecia;

[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[AllowAnonymous]
[Route("estcivil")]
public class EstCivilController : ControllerBase {
    private readonly DataContext context;

    public EstCivilController(DataContext context) {
        this.context = context;
    }

    //Obtener
    [HttpGet("get")]
    public async Task<ActionResult<EstCivil>> obtener() {
        try {
            var listaRoles = await context.estCiviles.ToListAsync();

            return Ok(listaRoles);
        } catch (Exception ex) {
            return BadRequest(ex.Message);
        }
    }
}
