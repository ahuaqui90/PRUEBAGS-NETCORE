using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PRUEBAGS.ApplicationDbContext;
using PRUEBAGS.Models;

namespace PRUEBAGS.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        //private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly ILogger<UsuarioController> _logger;
        private readonly AppDbContext _context;

        public UsuarioController(AppDbContext context, ILogger<UsuarioController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> ListarUsuarios()
        {
            _logger.LogInformation("UsuarioController: Get method Listar Usuarios");
            return await _context.Usuarios.ToListAsync();
        }

        // POST: api/Uusario
        [HttpPost]
        public async Task<ActionResult<Usuario>> GrabarUsuario(Usuario usuarioItem)
        {
            _context.Usuarios.Add(usuarioItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ListarUsuarios), new { id = usuarioItem.Id }, usuarioItem);
        }

        // PUT: api/Usario/5
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarUsuario(long id, Usuario todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(todoItem).State = EntityState.Modified;

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


        // DELETE: api/Usuario/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarUsuario(long id)
        {
            var todoItem = await _context.Usuarios.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool UsuarioExists(long id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }

    }
}
