using PessoaMicroservice.Model;
using PessoaMicroservice.Redis;
using PessoaMicroservice.Repository;

namespace PessoaMicroservice.Service
{
    public class PessoaService
    {
        private readonly ILogger<PessoaService> _logger;
        private readonly IPessoaRepository _pessoaRepository;
        private readonly RedisCache _redisCache;

        public PessoaService(ILogger<PessoaService> logger, IPessoaRepository pessoaRepository, RedisCache redisCache)
        {
            _logger = logger;
            _pessoaRepository = pessoaRepository;
            _redisCache = redisCache;
        }

        public async Task ProcessarPessoa(Pessoa pessoa)
        {
            try
            {
                _logger.LogInformation("[ProcessarPessoa]: Iniciando processamento da pessoa...");

                pessoa.CPF = NormalizarCPF(pessoa.CPF);

                var cachedPessoa = _redisCache.GetPessoa(pessoa.CPF);
                if (cachedPessoa != null && cachedPessoa.Equals(pessoa))
                {
                    _logger.LogInformation($"[ProcessarPessoa]: Pessoa encontrada no cache Redis. CPF: {pessoa.CPF}, Email: {pessoa.Email}. Nenhuma ação necessária.");
                    return;
                }

                var result = await AdicionaOuAtualiza(pessoa);

                if(result) 
                {
                    _logger.LogInformation("[ProcessarPessoa]: Pessoa processada com sucesso!");
                } else 
                {
                    _logger.LogError("$[ProcessarPessoa]: Erro ao processar pessoa: Pessoa não persistida.");
                }
            }
            catch (Exception e)
            {
                _logger.LogError("$[ProcessarPessoa]: Erro ao processar pessoa: " + e);
            }
        }

        private async Task<bool> AdicionaOuAtualiza(Pessoa pessoa) 
        {
            try 
            {
                _logger.LogInformation("[AdicionaOuAtualiza]: Iniciando persistência de pessoa...");

                pessoa.DataDeCriacao = DateTime.UtcNow;
                pessoa.DataDeAtualizacao = DateTime.UtcNow;

                await _pessoaRepository.AdicionarPessoa(pessoa);
                _logger.LogInformation("$[ProcessarPessoa]: Pessoa de email: " + pessoa.Email + " persistida no banco de dados.");
                return true;
            }
            catch (Exception e) 
            {
                _logger.LogError("$[AdicionaOuAtualiza]: Erro ao persistir pessoa: }" + e);
                return false;
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