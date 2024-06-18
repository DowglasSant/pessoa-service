using System.Text.Json;
using PessoaMicroservice.Model;
using StackExchange.Redis;

namespace PessoaMicroservice.Redis
{
    public class RedisCache
    {
        private readonly ConnectionMultiplexer _redis;

        public RedisCache(string connectionString)
        {
            _redis = ConnectionMultiplexer.Connect(connectionString);
        }

        public Pessoa GetPessoa(string cpf)
        {
            var db = _redis.GetDatabase();
            var pessoaJson = db.StringGet(cpf);

            return pessoaJson.IsNullOrEmpty ? null : JsonSerializer.Deserialize<Pessoa>(pessoaJson);
        }

        public void SetPessoa(Pessoa pessoa)
        {
            var db = _redis.GetDatabase();
            var pessoaJson = JsonSerializer.Serialize(pessoa);
            db.StringSet(pessoa.CPF, pessoaJson);
        }
    }
}