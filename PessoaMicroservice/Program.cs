using Microsoft.EntityFrameworkCore;
using PessoaMicroservice.Component;
using PessoaMicroservice.Context;
using PessoaMicroservice.Repository;
using PessoaMicroservice.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<PessoaConsumer>();

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
