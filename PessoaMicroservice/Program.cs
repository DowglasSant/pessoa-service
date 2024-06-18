using Microsoft.EntityFrameworkCore;
using PessoaMicroservice.Component;
using PessoaMicroservice.Context;
using PessoaMicroservice.Redis;
using PessoaMicroservice.Repository;
using PessoaMicroservice.Service;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<PessoaConsumer>();

builder.Services.AddSingleton<ConnectionMultiplexer>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var redisConnectionString = configuration.GetSection("Redis:ConnectionString").Value;
    return ConnectionMultiplexer.Connect(redisConnectionString);
});

builder.Services.AddSingleton<RedisCache>();

builder.Services.AddDbContext<PessoaDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DbString"));
});

builder.Services.AddScoped<IPessoaRepository, PessoaRepository>();
builder.Services.AddScoped<PessoaService>();

builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
