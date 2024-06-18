using PessoaMicroservice.Model;

namespace PessoaMicroservice.Repository
{
    public interface IPessoaRepository
    {
        Task AdicionarPessoa(Pessoa pessoa);
    }
}