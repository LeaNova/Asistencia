using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API_asistecia;

[ApiController]
[AllowAnonymous]
[Route("usuario")]
public class UsuarioController : ControllerBase {
    private readonly DataContext context;
    private readonly IConfiguration configuration;

    public UsuarioController(DataContext context, IConfiguration configuration) {
        this.context = context;
        this.configuration = configuration;
    }


    [HttpPost("signin")]
    public async Task<IActionResult> alta([FromForm] UsuarioSign sign) {
        try {
            if(ModelState.IsValid) {
                string passHashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
					password: sign.pass.ToString(),
					salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
					prf: KeyDerivationPrf.HMACSHA1,
					iterationCount: 1000,
					numBytesRequested: 256 / 8));
                
                Usuario user = parseUsuario(sign);
                user.pass = passHashed;
                user.fechaIngreso = DateTime.Now;
                user.disponible = true;

                context.usuarios.Add(user);
                context.SaveChanges();

                //return CreatedAtAction(nameof(obtener), new { id = user.idUsuario }, user);
                return Ok();
            }

            return BadRequest();
        } catch(Exception ex) {
            return BadRequest(ex.Message);
        }
    }

    //Log in
    [HttpPost("login")]
    public async Task<IActionResult> login([FromForm] Login login) {
        try {
            Usuario user = await context.usuarios
                .Include(x => x.rol)
                .FirstOrDefaultAsync(x => x.mail == login.mail);

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
				password: login.pass,
				salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
				prf: KeyDerivationPrf.HMACSHA1,
				iterationCount: 1000,
				numBytesRequested: 256 / 8));

            if(user != null && user.pass == hashed) {
                var key = new SymmetricSecurityKey(
					System.Text.Encoding.ASCII.GetBytes(configuration["TokenAuthentication:SecretKey"]));
				var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

				var claims = new List<Claim> {
					new Claim(ClaimTypes.Name, user.nombre + " " + user.apellido),
					new Claim("id", user.idUsuario.ToString()),
					new Claim(ClaimTypes.Role, user.rol.nombre)
				};

				var token = new JwtSecurityToken(
					issuer: configuration["TokenAuthentication:Issuer"],
					audience: configuration["TokenAuthentication:Audience"],
					claims: claims,
					expires: DateTime.Now.AddDays(7),
					signingCredentials: credenciales
				);

				return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            }
            return BadRequest();
        } catch (Exception ex) {
            return BadRequest(ex.Message);
        }
    }
    
    //Busquedas
    [HttpGet("get")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<Usuario>> obtener() {
        try {
            int id = int.Parse(User.Claims.First(x => x.Type == "id").Value);
            Usuario u = context.usuarios
            .Include(x => x.genero).Include(x => x.estCivil).Include(x => x.rol)
            .FirstOrDefault(x => x.idUsuario == id);

            UsuarioInfo userInfo = new UsuarioInfo(u);
            return Ok(userInfo);
        } catch (Exception ex) {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("list")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<Usuario>> getList() {
        try {
            if(User.IsInRole("Admin")) {
                var listaEmpleados = await context.usuarios
                    .Include(x => x.rol).Where(x => !x.rol.nombre.Equals("Admin")).Where(x => x.disponible == true)
                    .Select(x => new {
                        idUsuario = x.idUsuario,
                        nombre = x.nombre,
                        apellido = x.apellido,
                        dni = x.dni,
                        puesto = x.rol.nombre
                    })
                    .ToListAsync();
                
                return Ok(listaEmpleados);
            }
            return BadRequest();
        } catch(Exception ex) {
            return BadRequest(ex.Message);
        }
    }

    //Funciones
    private Usuario parseUsuario(UsuarioSign sign) {
        Usuario usuario = new Usuario {
            nombre = sign.nombre,
            apellido = sign.apellido,
            fechaNac = DateTime.Parse(sign.fechaNac),
            dni = sign.dni,
            idGenero = int.Parse(sign.idGenero),
            idEstCivil = int.Parse(sign.idEstCivil),
            direccion = sign.direccion,
            telefono = sign.telefono,
            mail = sign.mail,
            pass = sign.pass,
            idRol = int.Parse(sign.idRol)
        };
        
        return usuario;
    }
}
