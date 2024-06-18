using Microsoft.EntityFrameworkCore;
using PessoaMicroservice.Model;

namespace PessoaMicroservice.Context
{
    public class PessoaDbContext : DbContext
    {
        public DbSet<Pessoa> PessoaContext {get; set;}

        public PessoaDbContext(DbContextOptions<PessoaDbContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<Pessoa>()
            .HasKey(p => p.CPF);

            modelBuilder.Entity<Pessoa>()
                .Property(p => p.DataDeAtualizacao)
                .IsRequired(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}