using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_asistecia;

[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("ingreso")]
public class IngresoController : ControllerBase {
    private readonly DataContext context;
    public IngresoController(DataContext context) {
        this.context = context;
    }

    [HttpPost("create")]
    public async Task<IActionResult> alta([FromForm] string fecha) {
        try {
            if(User.IsInRole("Admin")) {
                DateTime date = DateTime.Parse(fecha);
				Random random = new Random(Environment.TickCount);
				string rdmChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789";
				string code = "";

				for(int i = 0; i < 2; i++) {
					code += rdmChars[random.Next(0, rdmChars.Length)];
				}

                Ingreso ingreso = new Ingreso{
                    codIngreso = date.ToString("ddMMyyyy") + code,
                    fecha = date
                };

                context.ingresos.Add(ingreso);
                context.SaveChanges();

                return CreatedAtAction(nameof(get), new { codIngreso = ingreso.codIngreso }, ingreso);
            }
            return BadRequest();
        } catch(Exception ex) {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("get/{codIngreso}")]
    public async Task<ActionResult<Ingreso>> get(string codIngreso) {
        try {
            if(User.IsInRole("Admin")) {
                var result = context.ingresos.FirstOrDefault(x => x.codIngreso == codIngreso);

                return Ok(result);
            }
            return BadRequest();
        } catch(Exception ex) {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("get/list")]
    public async Task<ActionResult<Ingreso>> getAll() {
        try {
            if(User.IsInRole("Admin")) {
                var result = await context.ingresos
                    .OrderByDescending(x => x.fecha)
                    .ToListAsync();

                return Ok(result);
            }
            return BadRequest();
        } catch(Exception ex) {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("get/last")]
    public async Task<ActionResult<Ingreso>> getLast() {
        try {
            if(User.IsInRole("Admin")) {
                var result = context.ingresos
                    .OrderByDescending(x => x.fecha)
                    .FirstOrDefault();

                return Ok(result);
            }
            return BadRequest();
        } catch(Exception ex) {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("get/today/{fecha}")]
    public async Task<ActionResult<Ingreso>> getLast(string fecha) {
        try {
            if(User.IsInRole("Admin")) {
                var result = context.ingresos
                    .Where(x => x.fecha == DateTime.Parse(fecha))
                    .FirstOrDefault();

                return Ok(result);
            }
            return BadRequest();
        } catch(Exception ex) {
            return BadRequest(ex.Message);
        }
    }
}
