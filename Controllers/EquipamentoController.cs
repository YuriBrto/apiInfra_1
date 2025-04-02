using apiInfra_1.Data;
using apiInfra_1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apiInfra_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipamentosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EquipamentosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/equipamentos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Equipamento>>> GetEquipamentos()
        {
            return Ok(await _context.Equipamentos.AsNoTracking().ToListAsync());
        }

        // GET: api/equipamentos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Equipamento>> GetEquipamento(int id)
        {
            var equipamento = await _context.Equipamentos.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
            if (equipamento == null)
            {
                return NotFound();
            }
            return Ok(equipamento);
        }

        // POST: api/equipamentos
        [HttpPost]
        public async Task<ActionResult<Equipamento>> PostEquipamento([FromBody] Equipamento equipamento)
        {
            if (equipamento == null)
            {
                return BadRequest("Dados inválidos.");
            }

            _context.Equipamentos.Add(equipamento);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEquipamento), new { id = equipamento.Id }, equipamento);
        }

        // PUT: api/equipamentos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEquipamento(int id, [FromBody] Equipamento equipamento)
        {
            if (equipamento == null || id != equipamento.Id)
            {
                return BadRequest("ID inválido ou dados inconsistentes.");
            }

            if (!await _context.Equipamentos.AnyAsync(e => e.Id == id))
            {
                return NotFound();
            }

            _context.Entry(equipamento).State = EntityState.Modified;

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

        // DELETE: api/equipamentos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEquipamento(int id)
        {
            var equipamento = await _context.Equipamentos.FindAsync(id);
            if (equipamento == null)
            {
                return NotFound();
            }

            _context.Equipamentos.Remove(equipamento);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
