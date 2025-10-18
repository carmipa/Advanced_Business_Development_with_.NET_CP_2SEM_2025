using cp_5_autenticacao_autorizacao_swt.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace cp_5_autenticacao_autorizacao_swt.Infrastructure.Data
{
    /// <summary>
    /// Contexto do Entity Framework para acesso aos dados
    /// </summary>
    /// <remarks>
    /// Esta classe define o contexto do Entity Framework Core para a aplicação,
    /// incluindo todas as entidades do domínio e suas configurações.
    /// É responsável por mapear as entidades para tabelas do banco de dados.
    /// </remarks>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Construtor do contexto do Entity Framework
        /// </summary>
        /// <param name="options">Opções de configuração do contexto</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// DbSet para a entidade User
        /// </summary>
        /// <remarks>
        /// Representa a tabela de usuários no banco de dados.
        /// Permite operações CRUD na tabela Users.
        /// </remarks>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Configurações das entidades e relacionamentos
        /// </summary>
        /// <param name="modelBuilder">Builder para configuração do modelo</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração da entidade User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(100);
                
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);
                
                entity.Property(e => e.SenhaHash)
                    .IsRequired()
                    .HasMaxLength(255);
                
                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasConversion<int>();
                
                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasConversion<int>();
                
                entity.Property(e => e.RefreshToken)
                    .HasMaxLength(500);
                
                // Índice único para email
                entity.HasIndex(e => e.Email)
                    .IsUnique();
                
                // Índice para refresh token
                entity.HasIndex(e => e.RefreshToken);
            });
        }
    }
}
