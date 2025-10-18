using System.ComponentModel.DataAnnotations;

namespace cp_5_autenticacao_autorizacao_swt.Domain.Entities
{
    /// <summary>
    /// Classe base abstrata para todas as entidades do domínio
    /// </summary>
    /// <remarks>
    /// Esta classe fornece propriedades comuns que todas as entidades do sistema devem ter.
    /// Implementa o padrão de auditoria básica com timestamps de criação e atualização.
    /// Todas as entidades do domínio devem herdar desta classe base.
    /// </remarks>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Identificador único da entidade (chave primária)
        /// </summary>
        /// <example>1</example>
        [Key]
        public int Id { get; set; }
        
        /// <summary>
        /// Data e hora de criação do registro (UTC)
        /// </summary>
        /// <example>2024-01-01T10:00:00Z</example>
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// Data e hora da última atualização do registro (UTC)
        /// </summary>
        /// <example>2024-01-01T12:00:00Z</example>
        public DateTime? DataAtualizacao { get; set; }
    }
}
