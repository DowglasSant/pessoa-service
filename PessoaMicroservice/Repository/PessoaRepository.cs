using PessoaMicroservice.Context;
using PessoaMicroservice.Model;

namespace PessoaMicroservice.Repository
{
    public class PessoaRepository : IPessoaRepository
    {
        private readonly PessoaDbContext _pessoaDbContext;

        public PessoaRepository(PessoaDbContext pessoaDbContext)
        {
            _pessoaDbContext = pessoaDbContext;
        }

        public async Task AdicionarPessoa(Pessoa pessoa)
        {
            _pessoaDbContext.PessoaContext.Add(pessoa);
            await _pessoaDbContext.SaveChangesAsync();
        }
    }
}