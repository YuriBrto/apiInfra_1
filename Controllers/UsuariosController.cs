using apiInfra_1.Data;
using apiInfra_1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apiInfra_1.Controllers
{
    [Route("api/[controller]")]
        [ApiController]
        public class UsuariosController : ControllerBase
        {
            private readonly AppDbContext _context;
            private readonly ILogger<UsuariosController> _logger;

            public UsuariosController(AppDbContext context, ILogger<UsuariosController> logger)
            {
                _context = context;
                _logger = logger;
            }

            // GET: api/usuarios
            [HttpGet]
            public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
            {
            try
            {
                var usuarios = await _context.Usuarios
                    .AsNoTracking()
                    .ToListAsync();

                // Mapeamento manual para DTO
                var usuariosDto = usuarios.Select(u => new UsuarioDto
                {
                    ID_usuario = u.ID_usuario,
                    Username = u.username
                }).ToList();

                return Ok(usuariosDto);
            }
            catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao buscar usuários");
                    return StatusCode(500, "Erro interno ao processar a requisição");
                }
            }

            // GET: api/usuarios/5
            [HttpGet("{id}")]
            public async Task<ActionResult<UsuarioDto>> GetUsuario(int id)
            {
                try
                {
                    var usuario = await _context.Usuarios
                        .AsNoTracking()
                        .Where(u => u.ID_usuario == id)
                        .Select(u => new UsuarioDto
                        {
                            ID_usuario = u.ID_usuario,
                            Username = u.username
                        })
                        .FirstOrDefaultAsync();

                    if (usuario == null)
                    {
                        return NotFound();
                    }

                    return usuario;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Erro ao buscar usuário com ID {id}");
                    return StatusCode(500, "Erro interno ao processar a requisição");
                }
            }

            // POST: api/usuarios
            [HttpPost]
            public async Task<ActionResult<Usuario>> PostUsuario([FromBody] UsuarioCreateDto usuarioDto)
            {
                try
                {
                    if (usuarioDto == null)
                    {
                        return BadRequest("Dados do usuário inválidos");
                    }

                    // Verificar se username já existe
                    if (await _context.Usuarios.AnyAsync(u => u.username == usuarioDto.Username))
                    {
                        return Conflict("Nome de usuário já existe");
                    }

                    // Hash da senha (simplificado - na prática use lib especializada)
                    var salt = BCrypt.Net.BCrypt.GenerateSalt();
                    var senhaHash = BCrypt.Net.BCrypt.HashPassword(usuarioDto.Senha, salt);

                    var usuario = new Usuario
                    {
                        username = usuarioDto.Username,
                        senha = senhaHash
                    };

                    _context.Usuarios.Add(usuario);
                    await _context.SaveChangesAsync();

                    // Retorna sem a senha
                    return CreatedAtAction(nameof(GetUsuario), new { id = usuario.ID_usuario },
                        new UsuarioDto
                        {
                            ID_usuario = usuario.ID_usuario,
                            Username = usuario.username
                        });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao criar usuário");
                    return StatusCode(500, "Erro interno ao criar usuário");
                }
            }

            // PUT: api/usuarios/5
            [HttpPut("{id}")]
            public async Task<IActionResult> PutUsuario(int id, [FromBody] UsuarioUpdateDto usuarioDto)
            {
                try
                {
                    if (id != usuarioDto.ID_usuario)
                    {
                        return BadRequest("ID inválido");
                    }

                    var usuarioExistente = await _context.Usuarios.FindAsync(id);
                    if (usuarioExistente == null)
                    {
                        return NotFound();
                    }

                    // Atualiza apenas o username (senha deve ter endpoint separado)
                    usuarioExistente.username = usuarioDto.Username;

                    try
                    {
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
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Erro ao atualizar usuário com ID {id}");
                    return StatusCode(500, "Erro interno ao atualizar usuário");
                }
            }

            // DELETE: api/usuarios/5
            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteUsuario(int id)
            {
                try
                {
                    var usuario = await _context.Usuarios.FindAsync(id);
                    if (usuario == null)
                    {
                        return NotFound();
                    }

                    // Verificar se o usuário está associado a máquinas
                    if (await _context.Maquinas.AnyAsync(m => m.FK_ID_usuario == id))
                    {
                        return BadRequest("Não é possível excluir usuário associado a máquinas");
                    }

                    _context.Usuarios.Remove(usuario);
                    await _context.SaveChangesAsync();

                    return NoContent();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Erro ao excluir usuário com ID {id}");
                    return StatusCode(500, "Erro interno ao excluir usuário");
                }
            }

            // PATCH: api/usuarios/5/senha
            [HttpPatch("{id}/senha")]
            public async Task<IActionResult> UpdateSenha(int id, [FromBody] SenhaUpdateDto senhaDto)
            {
                try
                {
                    var usuario = await _context.Usuarios.FindAsync(id);
                    if (usuario == null)
                    {
                        return NotFound();
                    }

                    // Validar senha atual se necessário
                    // if (!BCrypt.Net.BCrypt.Verify(senhaDto.SenhaAtual, usuario.senha))
                    // {
                    //     return Unauthorized("Senha atual incorreta");
                    // }

                    // Atualizar senha
                    var salt = BCrypt.Net.BCrypt.GenerateSalt();
                    usuario.senha = BCrypt.Net.BCrypt.HashPassword(senhaDto.NovaSenha, salt);

                    await _context.SaveChangesAsync();

                    return NoContent();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Erro ao atualizar senha do usuário com ID {id}");
                    return StatusCode(500, "Erro interno ao atualizar senha");
                }
            }

            private bool UsuarioExists(int id)
            {
                return _context.Usuarios.Any(e => e.ID_usuario == id);
            }
        }

        // DTOs para transferência de dados
        public class UsuarioDto
        {
            public int ID_usuario { get; set; }
            public string Username { get; set; }
        }

        public class UsuarioCreateDto
        {
            public string Username { get; set; }
            public string Senha { get; set; }
        }

        public class UsuarioUpdateDto
        {
            public int ID_usuario { get; set; }
            public string Username { get; set; }
        }

        public class SenhaUpdateDto
        {
            public string NovaSenha { get; set; }
            // public string SenhaAtual { get; set; } // Para validação
        }
    }

