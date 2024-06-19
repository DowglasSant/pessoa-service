using Microsoft.EntityFrameworkCore;
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

        public async Task AdicionarOuAtualizarPessoa(Pessoa pessoa)
        {
            var existingPessoa = await _pessoaDbContext.PessoaContext
                .FirstOrDefaultAsync(p => p.CPF == pessoa.CPF);

            if (existingPessoa != null)
            {
                existingPessoa.Nome = pessoa.Nome;
                existingPessoa.Idade = pessoa.Idade;
                existingPessoa.Email = pessoa.Email;
                existingPessoa.DataDeAtualizacao = DateTime.UtcNow;
            }
            else
            {
                pessoa.DataDeCriacao = DateTime.UtcNow;
                pessoa.DataDeAtualizacao = DateTime.UtcNow;
                _pessoaDbContext.PessoaContext.Add(pessoa);
            }

            await _pessoaDbContext.SaveChangesAsync();
        }
    }
}