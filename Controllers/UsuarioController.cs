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
					password: sign.dni.ToString(),
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
    
    [HttpPatch("edit/disponible/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> updateDisponible(int id) {
        try {
            if(User.IsInRole("Admin")) {
                Usuario u = context.usuarios.AsNoTracking()
                .FirstOrDefault(x => x.idUsuario == id);

                if(u != null) {
                    u.disponible = !u.disponible;

                    context.usuarios.Update(u);
                    context.SaveChanges();

                    return Ok();
                }
                return BadRequest();
            }
            return BadRequest();
        } catch(Exception ex) {
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("edit/pass")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> updatePass([FromForm] UsuarioPass usuarioPass) {
        try {
            int id = int.Parse(User.Claims.First(x => x.Type == "id").Value);
            Usuario u = await context.usuarios.AsNoTracking()
                .FirstOrDefaultAsync(x => x.idUsuario == id);

            if(u == null) {
                return BadRequest();
            }
            
            string oldHashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
				password: usuarioPass.oldPass.ToString(),
				salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
				prf: KeyDerivationPrf.HMACSHA1,
				iterationCount: 1000,
				numBytesRequested: 256 / 8));

            if(u.pass != oldHashed) {
                return BadRequest();
            }

            string newHashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
				password: usuarioPass.newPass.ToString(),
				salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
				prf: KeyDerivationPrf.HMACSHA1,
				iterationCount: 1000,
				numBytesRequested: 256 / 8));

            u.pass = newHashed;

            context.usuarios.Update(u);
            await context.SaveChangesAsync();

            return Ok();
        } catch(Exception ex) {
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
    
    //Busquedas
    [HttpGet("get/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<Usuario>> obtenerById(int id) {
        try {
            if(User.IsInRole("Admin")) {
                Usuario u = context.usuarios
                .Include(x => x.genero).Include(x => x.estCivil).Include(x => x.rol)
                .FirstOrDefault(x => x.idUsuario == id);

                UsuarioInfo userInfo = new UsuarioInfo(u);
                return Ok(userInfo);
            }
            
            return BadRequest();
        } catch (Exception ex) {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("list/all")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<Usuario>> getListAll() {
        try {
            if(User.IsInRole("Admin")) {
                var listaEmpleados = await context.usuarios
                    .Include(x => x.rol).Where(x => !x.rol.nombre.Equals("Admin"))
                    .Select(x => new {
                        idUsuario = x.idUsuario,
                        nombre = x.nombre,
                        apellido = x.apellido,
                        dni = x.dni,
                        puesto = x.rol.nombre,
                        disponible = x.disponible
                    })
                    .ToListAsync();
                
                return Ok(listaEmpleados);
            }
            return BadRequest();
        } catch(Exception ex) {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("list/disponibles")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<Usuario>> getListDisponibles() {
        try {
            if(User.IsInRole("Admin")) {
                var listaEmpleados = await context.usuarios
                    .Include(x => x.rol).Where(x => !x.rol.nombre.Equals("Admin")).Where(x => x.disponible == true)
                    .Select(x => new {
                        idUsuario = x.idUsuario,
                        nombre = x.nombre,
                        apellido = x.apellido,
                        dni = x.dni,
                        puesto = x.rol.nombre,
                        disponible = x.disponible
                    })
                    .ToListAsync();
                
                return Ok(listaEmpleados);
            }
            return BadRequest();
        } catch(Exception ex) {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("list/nodisponibles")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<Usuario>> getListNoDisponibles() {
        try {
            if(User.IsInRole("Admin")) {
                var listaEmpleados = await context.usuarios
                    .Include(x => x.rol).Where(x => !x.rol.nombre.Equals("Admin")).Where(x => x.disponible == false)
                    .Select(x => new {
                        idUsuario = x.idUsuario,
                        nombre = x.nombre,
                        apellido = x.apellido,
                        dni = x.dni,
                        puesto = x.rol.nombre,
                        disponible = x.disponible
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
            idRol = int.Parse(sign.idRol)
        };
        
        return usuario;
    }
}
