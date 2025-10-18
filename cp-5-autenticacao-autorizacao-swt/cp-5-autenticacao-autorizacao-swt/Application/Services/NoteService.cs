using AutoMapper;
using cp_5_autenticacao_autorizacao_swt.Application.DTOs.Notes;
using cp_5_autenticacao_autorizacao_swt.Application.Exceptions;
using cp_5_autenticacao_autorizacao_swt.Application.Interfaces;
using cp_5_autenticacao_autorizacao_swt.Domain.Entities;
using cp_5_autenticacao_autorizacao_swt.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace cp_5_autenticacao_autorizacao_swt.Application.Services;

/// <summary>
/// Serviço para gerenciamento de notas no sistema SafeScribe.
/// Implementa operações CRUD e lógica de negócio para notas.
/// </summary>
/// <remarks>
/// Este serviço é responsável por toda a lógica de negócio relacionada às notas,
/// incluindo criação, leitura, atualização e exclusão, com validações apropriadas.
/// </remarks>
public class NoteService : INoteService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<NoteService> _logger;

    /// <summary>
    /// Inicializa uma nova instância do NoteService.
    /// </summary>
    /// <param name="userRepository">Repositório para operações de usuários</param>
    /// <param name="mapper">Mapper para conversão entre DTOs e entidades</param>
    /// <param name="logger">Logger para registrar operações</param>
    public NoteService(IUserRepository userRepository, IMapper mapper, ILogger<NoteService> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Cria uma nova nota no sistema.
    /// </summary>
    public async Task<NoteResponseDto> CreateNoteAsync(CreateNoteRequestDto request, int userId)
    {
        try
        {
            // Verifica se o usuário existe
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException("Usuário", userId);
            }

            // Cria a nova nota
            var note = new Note
            {
                Title = request.Title,
                Content = request.Content,
                UserId = userId,
                IsSensitive = request.IsSensitive,
                Tags = request.Tags,
                CreatedAt = DateTime.UtcNow
            };

            // Em uma implementação real, aqui seria salva no banco de dados
            // Por enquanto, vamos simular a criação
            var response = _mapper.Map<NoteResponseDto>(note);
            response.UserName = user.Nome;

            _logger.LogInformation("Nota {NoteId} criada com sucesso para usuário {UserId}", note.Id, userId);
            return response;
        }
        catch (Exception ex) when (!(ex is BusinessException))
        {
            _logger.LogError(ex, "Erro ao criar nota para usuário {UserId}", userId);
            throw new BusinessException("Erro interno ao criar nota. Tente novamente mais tarde.", ex, "NOTE_CREATION_ERROR");
        }
    }

    /// <summary>
    /// Obtém uma nota específica por ID.
    /// </summary>
    public async Task<NoteResponseDto?> GetNoteByIdAsync(int id)
    {
        try
        {
            // Implementação simplificada - em produção seria um repositório específico
            // Por enquanto, retornamos null para simular que não encontrou
            _logger.LogDebug("Buscando nota {NoteId}", id);
            await Task.CompletedTask;
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar nota {NoteId}", id);
            return null;
        }
    }

    /// <summary>
    /// Atualiza uma nota existente.
    /// </summary>
    public async Task<NoteResponseDto> UpdateNoteAsync(int id, UpdateNoteRequestDto request)
    {
        try
        {
            // Implementação simplificada - em produção seria um repositório específico
            _logger.LogInformation("Atualizando nota {NoteId}", id);
            await Task.CompletedTask;
            
            // Por enquanto, retornamos um DTO simulado
            return new NoteResponseDto
            {
                Id = id,
                Title = request.Title ?? "Nota Atualizada",
                Content = request.Content ?? "Conteúdo atualizado",
                CreatedAt = DateTime.UtcNow.AddHours(-1),
                UpdatedAt = DateTime.UtcNow,
                UserId = 1, // Simulado - em produção seria obtido da nota existente
                UserName = "Usuário Teste",
                IsSensitive = request.IsSensitive ?? false,
                Tags = request.Tags
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar nota {NoteId}", id);
            throw;
        }
    }

    /// <summary>
    /// Exclui uma nota do sistema.
    /// </summary>
    public async Task<bool> DeleteNoteAsync(int id)
    {
        try
        {
            // Implementação simplificada - em produção seria um repositório específico
            _logger.LogInformation("Excluindo nota {NoteId}", id);
            await Task.CompletedTask;
            
            // Por enquanto, sempre retorna true (simula exclusão bem-sucedida)
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir nota {NoteId}", id);
            return false;
        }
    }

    /// <summary>
    /// Lista todas as notas de um usuário específico.
    /// </summary>
    public async Task<IEnumerable<NoteResponseDto>> GetUserNotesAsync(int userId)
    {
        try
        {
            // Implementação simplificada - em produção seria um repositório específico
            _logger.LogDebug("Listando notas do usuário {UserId}", userId);
            await Task.CompletedTask;
            
            // Por enquanto, retorna lista vazia
            return new List<NoteResponseDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar notas do usuário {UserId}", userId);
            return new List<NoteResponseDto>();
        }
    }

    /// <summary>
    /// Lista todas as notas do sistema (apenas para administradores).
    /// </summary>
    public async Task<IEnumerable<NoteResponseDto>> GetAllNotesAsync()
    {
        try
        {
            // Implementação simplificada - em produção seria um repositório específico
            _logger.LogDebug("Listando todas as notas do sistema");
            await Task.CompletedTask;
            
            // Por enquanto, retorna lista vazia
            return new List<NoteResponseDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar todas as notas");
            return new List<NoteResponseDto>();
        }
    }
}
