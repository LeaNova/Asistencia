using System.Linq.Expressions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_asistecia;

[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("asistencia")]
public class AsistenciaController : ControllerBase {
    private readonly DataContext context;

    public AsistenciaController(DataContext context) {
        this.context = context;
    }

    [HttpPatch("marcar")]
    public async Task<ActionResult> alta([FromForm] Marca marca) {
        try {
            int userId = int.Parse(User.Claims.First(x => x.Type == "id").Value);
            var result = await context.asistencias.AsNoTracking()
                .Where(x => x.codIngreso == marca.codIngreso && x.idUsuario == userId)
                .FirstOrDefaultAsync();

            if(result != null && result.horaIngreso == null) {
                result.horaIngreso = marca.time;

                context.asistencias.Update(result);
                context.SaveChanges();

                return Ok();
            }

            if(result != null && result.horaIngreso != null && result.horaSalida == null) {
                result.horaSalida = marca.time;

                context.asistencias.Update(result);
                context.SaveChanges();

                return Ok();
            }

            return BadRequest();
        } catch(Exception ex) {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("get/{codIngreso}")]
    public async Task<ActionResult<Asistencia>> get(string codIngreso) {
        try {
            if(User.IsInRole("Admin")) {
                var result = await context.ingresos.FirstOrDefaultAsync(x => x.codIngreso == codIngreso);

                if(result != null) {
                    var lista = await context.asistencias
                        .Include(x => x.usuario)
                        .Select(x => new {
                            codIngreso = x.codIngreso,
                            horaIngreso = x.horaIngreso,
                            horaSalida = x.horaSalida,
                            usuario = new {
                                idUsuario = x.usuario.idUsuario,
                                nombre = x.usuario.nombre,
                                apellido = x.usuario.apellido
                            }
                        })
                        .Where(x => x.codIngreso == result.codIngreso)
                        .ToListAsync();

                    return Ok(lista);
                }
                return BadRequest();
            }
            return BadRequest();
        } catch(Exception ex) {
            return BadRequest(ex.Message);
        }
    }
}
