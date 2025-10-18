namespace cp_5_autenticacao_autorizacao_swt.Application.DTOs.Notes;

/// <summary>
/// DTO para resposta de dados de nota.
/// </summary>
/// <remarks>
/// Este DTO contém todos os dados de uma nota que são retornados nas respostas da API.
/// Inclui informações completas sobre a nota e metadados de criação/atualização.
/// </remarks>
public class NoteResponseDto
{
    /// <summary>
    /// Identificador único da nota.
    /// </summary>
    /// <example>1</example>
    public int Id { get; set; }

    /// <summary>
    /// Título da nota.
    /// </summary>
    /// <example>Reunião de Planejamento Q4</example>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Conteúdo da nota.
    /// </summary>
    /// <example>Discussão sobre estratégias para o próximo trimestre, incluindo metas e objetivos.</example>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Data de criação da nota.
    /// </summary>
    /// <example>2025-01-18T10:30:00Z</example>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Data da última atualização da nota.
    /// </summary>
    /// <example>2025-01-18T15:45:00Z</example>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Identificador do usuário proprietário da nota.
    /// </summary>
    /// <example>1</example>
    public int UserId { get; set; }

    /// <summary>
    /// Nome do usuário proprietário da nota.
    /// </summary>
    /// <example>João Silva</example>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Indica se a nota é sensível/confidencial.
    /// </summary>
    /// <example>false</example>
    public bool IsSensitive { get; set; }

    /// <summary>
    /// Tags associadas à nota.
    /// </summary>
    /// <example>planejamento,q4,estratégia</example>
    public string? Tags { get; set; }
}
