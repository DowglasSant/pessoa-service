using System.Text.Json;
using PessoaMicroservice.Model;
using StackExchange.Redis;

namespace PessoaMicroservice.Redis
{
    public class RedisCache
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly ILogger<RedisCache> _logger;

        public RedisCache(IConnectionMultiplexer redis, ILogger<RedisCache> logger)
        {
            _redis = redis;
            _logger = logger;
        }

        public async Task<Pessoa> AcharPessoaRedis(string cpf)
        {   
            try 
            {
                var db = _redis.GetDatabase();
                var pessoaJson = db.StringGet(cpf);

                return pessoaJson.IsNullOrEmpty ? null : JsonSerializer.Deserialize<Pessoa>(pessoaJson);
            }
            catch (Exception e)
            {
                _logger.LogError("[AcharPessoaRedis]: Erro ao buscar pessoa no redis: " + e);
                throw new Exception(e.Message);
            }   
        }

        public async Task AdcionarPessoaRedis(Pessoa pessoa)
        {
            try
            {
                var db = _redis.GetDatabase();
                var pessoaJson = JsonSerializer.Serialize(pessoa);
                db.StringSet(pessoa.CPF, pessoaJson);
            }
            catch (Exception e)
            {
                _logger.LogError("[AcharPessoaRedis]: Erro ao adcionar pessoa no redis: " + e);
                throw new Exception(e.Message);
            }
        }
    }
}
