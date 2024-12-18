using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoAPIMVC.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoAPIMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultadoCarrerasController : ControllerBase
    {
        private readonly ProyectoApiContext _context;

        public ResultadoCarrerasController(ProyectoApiContext context)
        {
            _context = context;
        }

        // GET: api/ResultadoCarreras
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResultadoCarrera>>> GetResultadoCarreras()
        {
            var resultadoCarreras = await _context.ResultadoCarreras
                .Include(r => r.IdcarreraNavigation)
                .Include(r => r.IdpilotoNavigation)
                .ToListAsync();
            return Ok(resultadoCarreras);
        }

        // GET: api/ResultadoCarreras/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ResultadoCarrera>> GetResultadoCarrera(int id)
        {
            var resultadoCarrera = await _context.ResultadoCarreras
                .Include(r => r.IdcarreraNavigation)
                .Include(r => r.IdpilotoNavigation)
                .FirstOrDefaultAsync(r => r.Idresultado == id);

            if (resultadoCarrera == null)
            {
                return NotFound();
            }

            return Ok(resultadoCarrera);
        }

        // POST: api/ResultadoCarreras
        [HttpPost]
        public async Task<ActionResult<ResultadoCarrera>> PostResultadoCarrera([FromBody] ResultadoCarrera resultadoCarrera)
        {
            if (ModelState.IsValid)
            {
                _context.ResultadoCarreras.Add(resultadoCarrera);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetResultadoCarrera), new { id = resultadoCarrera.Idresultado }, resultadoCarrera);
            }

            return BadRequest(ModelState);
        }

        // PUT: api/ResultadoCarreras/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutResultadoCarrera(int id, [FromBody] ResultadoCarrera resultadoCarrera)
        {
            if (id != resultadoCarrera.Idresultado)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(resultadoCarrera).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResultadoCarreraExists(id))
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

        // DELETE: api/ResultadoCarreras/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResultadoCarrera(int id)
        {
            var resultadoCarrera = await _context.ResultadoCarreras.FindAsync(id);
            if (resultadoCarrera == null)
            {
                return NotFound();
            }

            _context.ResultadoCarreras.Remove(resultadoCarrera);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ResultadoCarreraExists(int id)
        {
            return _context.ResultadoCarreras.Any(e => e.Idresultado == id);
        }
    }
}
