using cp_5_autenticacao_autorizacao_swt.Application.DTOs.Notes;

namespace cp_5_autenticacao_autorizacao_swt.Application.Interfaces;

/// <summary>
/// Interface para serviço de gerenciamento de notas.
/// Define operações CRUD para notas no sistema SafeScribe.
/// </summary>
/// <remarks>
/// Este serviço implementa a lógica de negócio para gerenciamento de notas,
/// incluindo criação, leitura, atualização e exclusão, com controle de acesso
/// baseado nas regras de autorização definidas no PDF.
/// </remarks>
public interface INoteService
{
    /// <summary>
    /// Cria uma nova nota no sistema.
    /// </summary>
    /// <param name="request">Dados da nota a ser criada</param>
    /// <param name="userId">ID do usuário criador da nota</param>
    /// <returns>Dados da nota criada</returns>
    /// <exception cref="ArgumentException">Quando os dados da requisição são inválidos</exception>
    Task<NoteResponseDto> CreateNoteAsync(CreateNoteRequestDto request, int userId);

    /// <summary>
    /// Obtém uma nota específica por ID.
    /// </summary>
    /// <param name="id">ID da nota</param>
    /// <returns>Dados da nota ou null se não encontrada</returns>
    Task<NoteResponseDto?> GetNoteByIdAsync(int id);

    /// <summary>
    /// Atualiza uma nota existente.
    /// </summary>
    /// <param name="id">ID da nota a ser atualizada</param>
    /// <param name="request">Dados atualizados da nota</param>
    /// <returns>Dados da nota atualizada</returns>
    /// <exception cref="ArgumentException">Quando a nota não é encontrada ou dados são inválidos</exception>
    Task<NoteResponseDto> UpdateNoteAsync(int id, UpdateNoteRequestDto request);

    /// <summary>
    /// Exclui uma nota do sistema.
    /// </summary>
    /// <param name="id">ID da nota a ser excluída</param>
    /// <returns>True se excluída com sucesso, false se não encontrada</returns>
    Task<bool> DeleteNoteAsync(int id);

    /// <summary>
    /// Lista todas as notas de um usuário específico.
    /// </summary>
    /// <param name="userId">ID do usuário</param>
    /// <returns>Lista de notas do usuário</returns>
    Task<IEnumerable<NoteResponseDto>> GetUserNotesAsync(int userId);

    /// <summary>
    /// Lista todas as notas do sistema (apenas para administradores).
    /// </summary>
    /// <returns>Lista de todas as notas</returns>
    Task<IEnumerable<NoteResponseDto>> GetAllNotesAsync();
}
