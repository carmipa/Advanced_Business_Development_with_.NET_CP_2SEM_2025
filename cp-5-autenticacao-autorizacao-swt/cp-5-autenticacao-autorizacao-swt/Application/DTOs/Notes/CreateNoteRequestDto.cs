using System.ComponentModel.DataAnnotations;

namespace cp_5_autenticacao_autorizacao_swt.Application.DTOs.Notes;

/// <summary>
/// DTO para requisição de criação de nota.
/// </summary>
/// <remarks>
/// Este DTO contém os dados necessários para criar uma nova nota no sistema.
/// O UserId é automaticamente obtido do token JWT do usuário autenticado.
/// </remarks>
public class CreateNoteRequestDto
{
    /// <summary>
    /// Título da nota.
    /// </summary>
    /// <example>Reunião de Planejamento Q4</example>
    [Required(ErrorMessage = "O título é obrigatório")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "O título deve ter entre 3 e 200 caracteres")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Conteúdo da nota.
    /// </summary>
    /// <example>Discussão sobre estratégias para o próximo trimestre, incluindo metas e objetivos.</example>
    [Required(ErrorMessage = "O conteúdo é obrigatório")]
    [StringLength(5000, MinimumLength = 10, ErrorMessage = "O conteúdo deve ter entre 10 e 5000 caracteres")]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Indica se a nota é sensível/confidencial.
    /// </summary>
    /// <example>false</example>
    public bool IsSensitive { get; set; } = false;

    /// <summary>
    /// Tags associadas à nota para facilitar organização.
    /// </summary>
    /// <example>planejamento,q4,estratégia</example>
    [StringLength(500, ErrorMessage = "As tags não podem exceder 500 caracteres")]
    public string? Tags { get; set; }
}

