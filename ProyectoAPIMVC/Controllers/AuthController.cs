using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoAPIMVC.Models;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoAPIMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ProyectoApiContext _context;

        public AuthController(ProyectoApiContext context)
        {
            _context = context;
        }

        // Login
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest login)
        {
            if (login == null)
            {
                return BadRequest("Los datos del login son incorrectos.");
            }

            // Buscar el usuario por correo
            var usuario = await _context.Usuarios
                                        .Where(u => u.Correo == login.Correo)
                                        .FirstOrDefaultAsync();

            if (usuario == null)
            {
                return Unauthorized("Usuario no encontrado.");
            }

            // Verificar la contraseña (comparando la encriptada)
            if (usuario.Contraseña != EncriptarConSHA256(login.Contraseña))
            {
                return Unauthorized("Contraseña incorrecta.");
            }

            // Si el login es correcto, retornar un código 200 OK
            // El frontend puede manejar la redirección a home.html usando esta respuesta.
            return Ok(new { message = "Login exitoso", userId = usuario.Idusuario, userName = usuario.NombreUsuario });
        }

        // Método para encriptar la contraseña con SHA-256
        private string EncriptarConSHA256(string contraseña)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(contraseña));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
