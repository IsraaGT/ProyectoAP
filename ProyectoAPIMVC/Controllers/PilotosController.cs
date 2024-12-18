using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoAPIMVC.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoAPIMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PilotosController : ControllerBase
    {
        private readonly ProyectoApiContext _context;

        public PilotosController(ProyectoApiContext context)
        {
            _context = context;
        }

        // GET: api/Pilotos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Piloto>>> GetPilotos()
        {
            try
            {
                var pilotos = await _context.Pilotos
                    .Include(p => p.IdequipoNavigation) // Incluir los detalles del equipo al que pertenece el piloto
                    .ToListAsync();

                return Ok(pilotos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al obtener los pilotos: " + ex.Message);
            }
        }

        // GET: api/Pilotos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Piloto>> GetPiloto(int id)
        {
            try
            {
                var piloto = await _context.Pilotos
                    .Include(p => p.IdequipoNavigation) // Incluir detalles del equipo
                    .FirstOrDefaultAsync(p => p.Idpiloto == id);

                if (piloto == null)
                {
                    return NotFound("Piloto no encontrado.");
                }

                return Ok(piloto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al obtener el piloto: " + ex.Message);
            }
        }

        // POST: api/Pilotos
        [HttpPost]
        public async Task<ActionResult<Piloto>> PostPiloto(Piloto piloto)
        {
            // Verificar si el equipo relacionado existe
            var equipo = await _context.Equipos.FindAsync(piloto.Idequipo);
            if (equipo == null)
            {
                return BadRequest("El equipo especificado no existe.");
            }

            // Validar el modelo
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Devolver los errores de validación
            }

            try
            {
                _context.Pilotos.Add(piloto);
                await _context.SaveChangesAsync();

                // Devolver el recurso recién creado
                return CreatedAtAction("GetPiloto", new { id = piloto.Idpiloto }, piloto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al agregar el piloto: " + ex.Message);
            }
        }

        // PUT: api/Pilotos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPiloto(int id, Piloto piloto)
        {
            if (id != piloto.Idpiloto)
            {
                return BadRequest("El ID del piloto no coincide.");
            }

            // Verificar si el equipo relacionado existe
            var equipo = await _context.Equipos.FindAsync(piloto.Idequipo);
            if (equipo == null)
            {
                return BadRequest("El equipo especificado no existe.");
            }

            // Validar el modelo
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Devolver los errores de validación
            }

            _context.Entry(piloto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PilotoExists(id))
                {
                    return NotFound("Piloto no encontrado.");
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al actualizar el piloto: " + ex.Message);
            }

            return NoContent();
        }

        // DELETE: api/Pilotos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePiloto(int id)
        {
            var piloto = await _context.Pilotos.FindAsync(id);
            if (piloto == null)
            {
                return NotFound("Piloto no encontrado.");
            }

            try
            {
                _context.Pilotos.Remove(piloto);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al eliminar el piloto: " + ex.Message);
            }

            return NoContent();
        }

        private bool PilotoExists(int id)
        {
            return _context.Pilotos.Any(p => p.Idpiloto == id);
        }
    }
}
