using Microsoft.AspNetCore.Mvc;
using Usuario.Data;
using Usuario.Models;

namespace Usuario.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioContext _context;

        public UsuarioController(UsuarioContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult RetornaUsuarios()
        {
            var usuarios = _context.Usuarios.ToList();
            return Ok(usuarios);
        }

      
        [HttpGet("{id}")]
        public IActionResult RetornaUsuario(int id)
        {
            var usuario = _context.Usuarios.Find(id);

            if (usuario == null)
            {
                return NotFound("Usuário não encontrado");
            }

            return Ok(usuario);
        }

        [HttpPost]
        public IActionResult CadastraUsuario(User usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
            return Created("", usuario);
        }

        [HttpPut("{id}")]
        public IActionResult AtualizaUsuario(int id, User usuarioAtualizado)
        {
            var usuarioDoBanco = _context.Usuarios.Find(id);

            if (usuarioDoBanco == null)
            {
                return NotFound("Usuário não existe no banco!");
            }

            usuarioDoBanco.Nome = usuarioAtualizado.Nome;
            usuarioDoBanco.Email = usuarioAtualizado.Email;
            usuarioDoBanco.Senha = usuarioAtualizado.Senha;

            _context.SaveChanges();

            return Ok("Atualizado");
        }

        [HttpPost("login")]
        public IActionResult Login(User usuario)
        {
            var Usuariobanco =  _context.Usuarios
               .Where(u => u.Email.Equals(usuario.Email) && u.Senha.Equals(usuario.Senha))
               .ToList();

            if (Usuariobanco.Count==0)
            {
                return Unauthorized("Usuário ou senha incorretos");
            }

            HttpContext.Session.SetString("Idlogado", Usuariobanco[0].Id.ToString());

            Response.Cookies.Append("Idlogado", Usuariobanco[0].Id.ToString(),
                new CookieOptions
                {
                    Expires = DateTime.Now.AddMinutes(30),
                    HttpOnly = true
                }

            );

            return Ok("login realizado com suucesso!");
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete(".AspNetCore.Session");

            HttpContext.Session.Clear();

            Response.Cookies.Delete("Email");

            return Ok("Logout realizado com sucesso!");
        }

        // DELETAR
        [HttpDelete("{id}")]
        public IActionResult DeletaUsuario(int id)
        {
            var usuarioDoBanco = _context.Usuarios.Find(id);

            if (usuarioDoBanco == null)
            {
                return NotFound("Usuário não encontrado!");
            }

            _context.Usuarios.Remove(usuarioDoBanco);

            _context.SaveChanges();

            return Ok("Deletado");

        }
    }
}