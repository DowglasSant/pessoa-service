using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PessoaMicroservice.Model
{
    [Table("pessoa")]
    public class Pessoa : IEquatable<Pessoa>
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

        public bool Equals(Pessoa other)
        {
            if (other == null)
                return false;

            return CPF == other.CPF &&
                   Nome == other.Nome &&
                   Idade == other.Idade &&
                   Email == other.Email;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Pessoa))
                return false;

            return Equals(obj as Pessoa);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CPF, Nome, Idade, Email);
        }
    }
}