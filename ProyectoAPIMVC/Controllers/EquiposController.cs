using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoAPIMVC.Models;

[ApiController]
[Route("api/[controller]")]
public class EquiposController : ControllerBase
{
    private readonly ProyectoApiContext _context;

    public EquiposController(ProyectoApiContext context)
    {
        _context = context;
    }

    // GET: api/Equipos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Equipo>>> GetEquipos()
    {
        // Filtrar equipos con Status = true
        return await _context.Equipos
                             .Include(e => e.Pilotos)
                             .Where(e => e.Status == true)
                             .ToListAsync();
    }

    // GET: api/Equipos/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Equipo>> GetEquipo(int id)
    {
        var equipo = await _context.Equipos
                                   .Include(e => e.Pilotos)
                                   .FirstOrDefaultAsync(e => e.Idequipo == id && e.Status == true);

        if (equipo == null)
        {
            return NotFound();
        }

        return equipo;
    }

    // POST: api/Equipos
    [HttpPost]
    public async Task<ActionResult<Equipo>> PostEquipo(Equipo equipo)
    {
        _context.Equipos.Add(equipo);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEquipo), new { id = equipo.Idequipo }, equipo);
    }

    // PUT: api/Equipos/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutEquipo(int id, Equipo equipo)
    {
        if (id != equipo.Idequipo)
        {
            return BadRequest();
        }

        _context.Entry(equipo).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EquipoExists(id))
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

    // DELETE: api/Equipos/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEquipo(int id)
    {
        var equipo = await _context.Equipos.FindAsync(id);
        if (equipo == null)
        {
            return NotFound();
        }

        // Cambiar el estado a inactivo (Status = false)
        equipo.Status = false;

        _context.Entry(equipo).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EquipoExists(id))
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

    private bool EquipoExists(int id)
    {
        return _context.Equipos.Any(e => e.Idequipo == id);
    }
}
