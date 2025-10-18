using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cp_5_autenticacao_autorizacao_swt.Domain.Entities;

/// <summary>
/// Representa uma nota no sistema SafeScribe.
/// Cada nota pertence a um usuário específico e contém informações sobre gestão de documentos sensíveis.
/// </summary>
public class Note : BaseEntity
{
    /// <summary>
    /// Título da nota.
    /// </summary>
    /// <example>Reunião de Planejamento Q4</example>
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Conteúdo da nota.
    /// </summary>
    /// <example>Discussão sobre estratégias para o próximo trimestre...</example>
    [Required]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Data de criação da nota.
    /// </summary>
    /// <example>2025-01-18T10:30:00Z</example>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Data da última atualização da nota.
    /// </summary>
    /// <example>2025-01-18T15:45:00Z</example>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Identificador do usuário proprietário da nota.
    /// </summary>
    /// <example>1</example>
    [Required]
    public int UserId { get; set; }

    /// <summary>
    /// Navegação para o usuário proprietário da nota.
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = null!;

    /// <summary>
    /// Indica se a nota foi marcada como sensível/confidencial.
    /// </summary>
    /// <example>true</example>
    public bool IsSensitive { get; set; } = false;

    /// <summary>
    /// Tags associadas à nota para facilitar organização.
    /// </summary>
    /// <example>["planejamento", "q4", "estratégia"]</example>
    [MaxLength(500)]
    public string? Tags { get; set; }
}
