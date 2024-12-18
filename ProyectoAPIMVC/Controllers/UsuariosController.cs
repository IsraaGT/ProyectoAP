using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoAPIMVC.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoAPIMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ProyectoApiContext _context;

        public UsuariosController(ProyectoApiContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // POST: api/Usuarios
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                // Encriptar la contraseña con SHA-256
                usuario.Contraseña = EncriptarConSHA256(usuario.Contraseña);

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Idusuario }, usuario);
            }

            return BadRequest(ModelState);
        }

        // PUT: api/Usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.Idusuario)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Si la contraseña fue modificada, encriptarla nuevamente
                    if (!string.IsNullOrEmpty(usuario.Contraseña))
                    {
                        usuario.Contraseña = EncriptarConSHA256(usuario.Contraseña);
                    }
                    else
                    {
                        // Si no se pasa una nueva contraseña, mantener la antigua
                        var existingUsuario = await _context.Usuarios.FindAsync(id);
                        usuario.Contraseña = existingUsuario?.Contraseña; // Mantener la misma contraseña si no se proporciona una nueva
                    }

                    _context.Entry(usuario).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }

            return BadRequest(ModelState);
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Idusuario == id);
        }

        // Método para encriptar la contraseña con SHA-256
        private string EncriptarConSHA256(string contraseña)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Convertir la contraseña en bytes y encriptarla
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(contraseña));

                // Convertir los bytes a un string hexadecimal
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest.Correo) || string.IsNullOrEmpty(loginRequest.Contraseña))
            {
                return BadRequest(new { error = "Correo y contraseña son obligatorios" });
            }

            // Buscar el usuario por correo
            var usuario = await _context.Usuarios
                                         .FirstOrDefaultAsync(u => u.Correo == loginRequest.Correo);

            if (usuario == null)
            {
                return Unauthorized(new { error = "Correo o contraseña incorrectos." });
            }

            // Comparar la contraseña proporcionada con la almacenada en la base de datos (encriptada)
            var contraseñaEncriptada = EncriptarConSHA256(loginRequest.Contraseña);

            if (usuario.Contraseña != contraseñaEncriptada)
            {
                return Unauthorized(new { error = "Correo o contraseña incorrectos." });
            }

            // Generar el token JWT (opcional, si quieres devolver uno en lugar de solo un mensaje)
            // Aquí puedes agregar tu lógica de generación de token JWT

            return Ok(new { message = "Login exitoso", usuario = usuario.NombreUsuario });
        }


    }
}
