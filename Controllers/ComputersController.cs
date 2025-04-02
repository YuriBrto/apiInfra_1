using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apiInfra_1.Models;
using apiInfra_1.Data;

namespace apiInfra_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComputersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ComputersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/computers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Computer>>> GetComputers()
        {
            return Ok(await _context.Computers.AsNoTracking().ToListAsync());
        }

        // GET: api/computers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Computer>> GetComputer(int id)
        {
            var computer = await _context.Computers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (computer == null)
            {
                return NotFound();
            }
            return Ok(computer);
        }

        // POST: api/computers
        [HttpPost]
        public async Task<ActionResult<Computer>> PostComputer([FromBody] Computer computer)
        {
            if (computer == null)
            {
                return BadRequest("Dados inválidos.");
            }

            _context.Computers.Add(computer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetComputer), new { id = computer.Id }, computer);
        }

        // PUT: api/computers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComputer(int id, [FromBody] Computer computer)
        {
            if (computer == null || id != computer.Id)
            {
                return BadRequest("ID inválido ou dados inconsistentes.");
            }

            if (!await _context.Computers.AnyAsync(e => e.Id == id))
            {
                return NotFound();
            }

            _context.Entry(computer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Erro ao atualizar o registro.");
            }

            return NoContent();
        }

        // DELETE: api/computers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComputer(int id)
        {
            var computer = await _context.Computers.FindAsync(id);
            if (computer == null)
            {
                return NotFound();
            }

            _context.Computers.Remove(computer);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}