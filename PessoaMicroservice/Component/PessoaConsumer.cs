using Confluent.Kafka;
using Newtonsoft.Json;
using PessoaMicroservice.Model;
using PessoaMicroservice.Service;

namespace PessoaMicroservice.Component
{
    public class PessoaConsumer : IHostedService
    {
        private readonly IConsumer<Null, string> _consumer;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<PessoaConsumer> _logger;
        private readonly string _topic;

        public PessoaConsumer(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory, ILogger<PessoaConsumer> logger)
        {
            var config = new ConsumerConfig
            {
                GroupId = configuration["kafka:GroupId"],
                BootstrapServers = configuration["kafka:BootstrapServers"],
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _topic = configuration["kafka:Topic"];
            _consumer = new ConsumerBuilder<Null, String>(config).Build();
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe(_topic);
            _logger.LogInformation("Kafka consumer subscribed to topic: {Topic}", _topic);

            _ = Task.Run(async () =>
            {
                try
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        var result = _consumer.Consume(stoppingToken);

                        if (result?.Message?.Value != null)
                        {
                            _logger.LogInformation("Mensagem consumida: {Message}", result.Message.Value);

                            using (var scope = _serviceScopeFactory.CreateScope())
                            {
                                var pessoaService = scope.ServiceProvider.GetRequiredService<PessoaService>();
                                Pessoa pessoaRecebida = JsonConvert.DeserializeObject<Pessoa>(result.Message.Value);
                                await pessoaService.ProcessarPessoa(pessoaRecebida);
                            }
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Consumo cancelado.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao consumir mensagens do Kafka.");
                }
            }, stoppingToken);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _consumer.Close();
            _consumer.Dispose();
            _logger.LogInformation("Kafka consumer closed and disposed.");
            return Task.CompletedTask;
        }
    }
}
