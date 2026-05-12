using Microsoft.EntityFrameworkCore;
using Usuario.Models;
namespace Usuario.Data
{
    public class UsuarioContext : DbContext
    {
        public DbSet<User> Usuarios { get; set; }

        public DbSet<Tarefa> Tarefas { get; set; }

        public UsuarioContext(DbContextOptions<UsuarioContext> options) : base(options)
        {
        }
    }
}
