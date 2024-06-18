using PessoaMicroservice.Model;
using PessoaMicroservice.Repository;

namespace PessoaMicroservice.Service
{
    public class PessoaService
    {
        private readonly ILogger<PessoaService> _logger;
        private readonly IPessoaRepository _pessoaRepository;

        public PessoaService(ILogger<PessoaService> logger, IPessoaRepository pessoaRepository)
        {
            _logger = logger;
            _pessoaRepository = pessoaRepository;
        }

        public async Task AdicionarPessoa(Pessoa pessoa)
        {
            try
            {
                _logger.LogInformation("[AdicionarPessoa]: Iniciando adição da pessoa...");

                pessoa.CPF = NormalizarCPF(pessoa.CPF);
                pessoa.DataDeCriacao = DateTime.UtcNow;

                await _pessoaRepository.AdicionarPessoa(pessoa);
                _logger.LogInformation("$[AdicionarPessoa]: Pessoa adcionada de email: " + pessoa.Email); 
            }
            catch (Exception e)
            {
                _logger.LogError("$[AdicionarPessoa]: Erro ao adicionar pessoa: }" + e);
            }
        }

        private string NormalizarCPF(string cpf)
        {
            if (string.IsNullOrEmpty(cpf))
                throw new ArgumentException("O campo CPF é requerido e não pode ser vazio.");

            cpf = cpf.Replace(".", "").Replace("-", "").Replace(" ", "");

            if (cpf.Length != 11)
                throw new ArgumentException("CPF deve conter 11 dígitos.");

            return cpf;
        }
    }
}