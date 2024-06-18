using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PessoaMicroservice.Model
{
    [Table("pessoa")]
    public class Pessoa
    {
        [Key]
        [StringLength(11)]
        public required string CPF { get; set; }

        [StringLength(100)]
        public required string Nome { get; set; }

        public required int Idade { get; set; }

        [StringLength(255)]
        [EmailAddress]
        public required string Email { get; set; }

        public DateTime DataDeCriacao {get; set;}

        public DateTime? DataDeAtualizacao {get; set;}
    }
}