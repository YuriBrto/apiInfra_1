using apiInfra_1.Data;
using apiInfra_1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apiInfra_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaquinasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MaquinasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/maquinas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Maquina>>> GetMaquinas()
        {
            return Ok(await _context.Maquinas
                .AsNoTracking()
                .Include(m => m.Setor)
                .Include(m => m.Usuario)
                .Include(m => m.TipoEquipamento)
                .ToListAsync());
        }

        // GET: api/maquinas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Maquina>> GetMaquina(int id)
        {
            var maquina = await _context.Maquinas
                .AsNoTracking()
                .Include(m => m.Setor)
                .Include(m => m.Usuario)
                .Include(m => m.TipoEquipamento)
                .FirstOrDefaultAsync(m => m.ID_maquina == id);

            if (maquina == null)
            {
                return NotFound();
            }
            return Ok(maquina);
        }

        // POST: api/maquinas
        [HttpPost]
        public async Task<ActionResult<Maquina>> PostMaquina([FromBody] Maquina maquina)
        {
            if (maquina == null)
            {
                return BadRequest("Dados inválidos.");
            }

            // Validação dos relacionamentos
            if (!await _context.Setores.AnyAsync(s => s.ID_setor == maquina.FK_ID_setor))
            {
                return BadRequest("Setor não encontrado.");
            }

            if (!await _context.Usuarios.AnyAsync(u => u.ID_usuario == maquina.FK_ID_usuario))
            {
                return BadRequest("Usuário não encontrado.");
            }

            if (!await _context.TipoEquipamento.AnyAsync(t => t.ID_equipamento == maquina.FK_ID_equipamento))
            {
                return BadRequest("Tipo de equipamento não encontrado.");
            }

            _context.Maquinas.Add(maquina);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMaquina), new { id = maquina.ID_maquina }, maquina);
        }

        // PUT: api/maquinas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMaquina(int id, [FromBody] Maquina maquina)
        {
            if (maquina == null || id != maquina.ID_maquina)
            {
                return BadRequest("ID inválido ou dados inconsistentes.");
            }

            if (!await _context.Maquinas.AnyAsync(m => m.ID_maquina == id))
            {
                return NotFound();
            }

            // Validação dos relacionamentos
            if (!await _context.Setores.AnyAsync(s => s.ID_setor == maquina.FK_ID_setor))
            {
                return BadRequest("Setor não encontrado.");
            }

            if (!await _context.Usuarios.AnyAsync(u => u.ID_usuario == maquina.FK_ID_usuario))
            {
                return BadRequest("Usuário não encontrado.");
            }

            if (!await _context.TipoEquipamento.AnyAsync(t => t.ID_equipamento == maquina.FK_ID_equipamento))
            {
                return BadRequest("Tipo de equipamento não encontrado.");
            }

            _context.Entry(maquina).State = EntityState.Modified;

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

        // DELETE: api/maquinas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaquina(int id)
        {
            var maquina = await _context.Maquinas.FindAsync(id);
            if (maquina == null)
            {
                return NotFound();
            }

            _context.Maquinas.Remove(maquina);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}