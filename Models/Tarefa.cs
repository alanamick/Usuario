using System.ComponentModel.DataAnnotations;

namespace Usuario.Models
{
    public class Tarefa
    {
        public int Id {get; set;}
        public string Descricao {get; set;}
        public bool Status {get; set;}
        public int Idusuario { get; set; }

    }
}
