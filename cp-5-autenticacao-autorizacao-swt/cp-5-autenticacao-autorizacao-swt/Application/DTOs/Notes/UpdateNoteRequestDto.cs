using System.ComponentModel.DataAnnotations;

namespace cp_5_autenticacao_autorizacao_swt.Application.DTOs.Notes;

/// <summary>
/// DTO para requisição de atualização de nota.
/// </summary>
/// <remarks>
/// Este DTO contém os dados que podem ser atualizados em uma nota existente.
/// Todos os campos são opcionais para permitir atualizações parciais.
/// </remarks>
public class UpdateNoteRequestDto
{
    /// <summary>
    /// Novo título da nota.
    /// </summary>
    /// <example>Reunião de Planejamento Q4 - Atualizado</example>
    [StringLength(200, MinimumLength = 3, ErrorMessage = "O título deve ter entre 3 e 200 caracteres")]
    public string? Title { get; set; }

    /// <summary>
    /// Novo conteúdo da nota.
    /// </summary>
    /// <example>Discussão atualizada sobre estratégias para o próximo trimestre, incluindo novas metas e objetivos revisados.</example>
    [StringLength(5000, MinimumLength = 10, ErrorMessage = "O conteúdo deve ter entre 10 e 5000 caracteres")]
    public string? Content { get; set; }

    /// <summary>
    /// Nova indicação se a nota é sensível/confidencial.
    /// </summary>
    /// <example>true</example>
    public bool? IsSensitive { get; set; }

    /// <summary>
    /// Novas tags associadas à nota.
    /// </summary>
    /// <example>planejamento,q4,estratégia,revisado</example>
    [StringLength(500, ErrorMessage = "As tags não podem exceder 500 caracteres")]
    public string? Tags { get; set; }
}

