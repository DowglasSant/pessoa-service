using PessoaMicroservice.Model;

namespace PessoaMicroservice.Repository
{
    public interface IPessoaRepository
    {
        Task AdicionarOuAtualizarPessoa(Pessoa pessoa);
    }
}