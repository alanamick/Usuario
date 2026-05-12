using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Usuario.Data;
using Usuario.Models;

namespace Usuario.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class TarefaController : ControllerBase
    {
        private readonly UsuarioContext _context;

        public TarefaController(UsuarioContext context)
        {
            _context = context;
        }

      
        [HttpGet("{id}")]
        public IActionResult RetornaTarefa(int id)
        {
            var usuario = HttpContext.Session.GetString("Idlogado");
            if (usuario == null)
                return Unauthorized(new { mensagem = "Não autenticado" });
            var tarefa = _context.Tarefas.Find(id);

            if (tarefa == null)
            {
                return NotFound("Tarefa não encontrada");
            }

            return Ok(tarefa);
        }

      
        [HttpGet("tarefasUsuario/{usuarioId}")]
        public IActionResult TarefasUsuario(int usuarioId)
        {
            var usuario = HttpContext.Session.GetString("Idlogado");
            if (usuario == null)
                return Unauthorized(new { mensagem = "Não autenticado" });
            var resultado =
                from u in _context.Usuarios
                join t in _context.Tarefas
               on u.Id equals t.Idusuario
                where usuarioId == u.Id
                select new
                {
                    Usuario = u.Nome,
                    u.Email,

                    Tarefa = t.Descricao,
                    t.Status
                };

            return Ok(resultado.ToList());
        }

        
        [HttpPost]
        public IActionResult CadastraTarefa(Tarefa tarefa)
        {
            var usuario = HttpContext.Session.GetString("Idlogado");
            if (usuario == null)
                return Unauthorized(new { mensagem = "Não autenticado"});
            var idCookie = Request.Cookies["Idlogado"];
            if (idCookie != null)
                tarefa.Idusuario = int.Parse(idCookie);

            _context.Tarefas.Add(tarefa);

            _context.SaveChanges();

            return Created("", tarefa);
        }

        
        [HttpPut("{id}")]
        public IActionResult AtualizaTarefa(int id, Tarefa tarefa)
        {
            var usuario = HttpContext.Session.GetString("Idlogado");
            if (usuario == null)
                return Unauthorized(new { mensagem = "Não autenticado" });
            var tarefaDoBanco = _context.Tarefas.Find(id);

            if (tarefaDoBanco == null)
            {
                return NotFound("Tarefa não existe no banco!");
            }

            tarefaDoBanco.Descricao = tarefa.Descricao;
            tarefaDoBanco.Status = tarefa.Status;
            tarefaDoBanco.Idusuario = tarefa.Idusuario;

            _context.SaveChanges();

            return Ok("Atualizado");
        }

        
        [HttpDelete("{id}")]
        public IActionResult DeletaTarefa(int id)
        {
            var usuario = HttpContext.Session.GetString("Idlogado");
            if (usuario == null)
                return Unauthorized(new { mensagem = "Não autenticado" });
            var tarefaDoBanco = _context.Tarefas.Find(id);

            if (tarefaDoBanco == null)
            {
                return NotFound("Tarefa não encontrada!");
            }

            _context.Tarefas.Remove(tarefaDoBanco);

            _context.SaveChanges();

            return Ok("Deletado");
        }
    }
}