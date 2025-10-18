using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using cp_5_autenticacao_autorizacao_swt.Application.DTOs.Notes;
using cp_5_autenticacao_autorizacao_swt.Application.Exceptions;
using cp_5_autenticacao_autorizacao_swt.Application.Interfaces;
using cp_5_autenticacao_autorizacao_swt.Domain.Entities;
using cp_5_autenticacao_autorizacao_swt.Domain.Enums;

namespace cp_5_autenticacao_autorizacao_swt.Presentation.Controllers;

/// <summary>
/// Controller responsável por gerenciar operações relacionadas a notas.
/// Implementa controle de acesso baseado em roles conforme especificado no PDF.
/// </summary>
/// <remarks>
/// Este controller é protegido por autenticação e implementa lógica de autorização
/// específica para cada operação, garantindo que usuários só acessem suas próprias
/// notas, exceto administradores que têm acesso total.
/// </remarks>
[ApiController]
[Route("api/v1/[controller]")]
[Authorize] // Controller protegido por autenticação
public class NotasController : ControllerBase
{
    private readonly INoteService _noteService;
    private readonly ILogger<NotasController> _logger;

    /// <summary>
    /// Inicializa uma nova instância do NotasController.
    /// </summary>
    /// <param name="noteService">Serviço para operações de notas</param>
    /// <param name="logger">Logger para registrar operações</param>
    public NotasController(INoteService noteService, ILogger<NotasController> logger)
    {
        _noteService = noteService;
        _logger = logger;
    }

    /// <summary>
    /// Cria uma nova nota no sistema.
    /// </summary>
    /// <param name="request">Dados da nota a ser criada</param>
    /// <returns>Dados da nota criada</returns>
    /// <remarks>
    /// Requer autorização de Editor ou Admin.
    /// O UserId é automaticamente obtido do token JWT do usuário autenticado.
    /// </remarks>
    /// <response code="201">Nota criada com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="403">Permissão negada</response>
    [HttpPost]
    [Authorize(Roles = "Editor,Admin")]
    [ProducesResponseType(typeof(NoteResponseDto), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    public async Task<ActionResult<NoteResponseDto>> CreateNote([FromBody] CreateNoteRequestDto request)
    {
        try
        {
            // Obtém o ID do usuário autenticado do token JWT
            var userIdClaim = User.FindFirst("userId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                _logger.LogWarning("Token JWT sem claim de ID de usuário válido");
                throw new UnauthorizedException("Token JWT inválido ou malformado.");
            }

            var result = await _noteService.CreateNoteAsync(request, userId);
            
            _logger.LogInformation("Nota criada com sucesso para usuário {UserId}", userId);
            return CreatedAtAction(nameof(GetNote), new { id = result.Id }, result);
        }
        catch (Exception ex) when (!(ex is BusinessException))
        {
            _logger.LogError(ex, "Erro ao criar nota");
            throw new BusinessException("Erro interno ao processar criação de nota.", ex, "NOTE_CREATION_ERROR");
        }
    }

    /// <summary>
    /// Obtém uma nota específica por ID.
    /// </summary>
    /// <param name="id">ID da nota</param>
    /// <returns>Dados da nota</returns>
    /// <remarks>
    /// Lógica de Segurança: Leitores e Editores só podem acessar suas próprias notas.
    /// Admins podem acessar qualquer nota.
    /// </remarks>
    /// <response code="200">Nota encontrada</response>
    /// <response code="404">Nota não encontrada</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="403">Permissão negada</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(NoteResponseDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<NoteResponseDto>> GetNote(int id)
    {
        try
        {
            var userIdClaim = User.FindFirst("userId");
            var userRoleClaim = User.FindFirst(ClaimTypes.Role);
            
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized("Token inválido");
            }

            var note = await _noteService.GetNoteByIdAsync(id);
            if (note == null)
            {
                return NotFound("Nota não encontrada");
            }

            // Verifica permissões de acesso
            if (!await HasPermissionToAccessNote(note, userId, userRoleClaim?.Value))
            {
                return Forbid("Você não tem permissão para acessar esta nota");
            }

            return Ok(note);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar nota {NoteId}", id);
            return BadRequest("Erro ao buscar nota");
        }
    }

    /// <summary>
    /// Atualiza uma nota existente.
    /// </summary>
    /// <param name="id">ID da nota</param>
    /// <param name="request">Dados atualizados da nota</param>
    /// <returns>Dados da nota atualizada</returns>
    /// <remarks>
    /// Lógica de Segurança: Editores só podem atualizar suas próprias notas.
    /// Admins podem atualizar qualquer nota.
    /// </remarks>
    /// <response code="200">Nota atualizada com sucesso</response>
    /// <response code="404">Nota não encontrada</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="403">Permissão negada</response>
    [HttpPut("{id}")]
    [Authorize(Roles = "Editor,Admin")]
    [ProducesResponseType(typeof(NoteResponseDto), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
        public async Task<ActionResult<NoteResponseDto>> UpdateNote(int id, [FromBody] UpdateNoteRequestDto request)
    {
        try
        {
            var userIdClaim = User.FindFirst("userId");
            var userRoleClaim = User.FindFirst(ClaimTypes.Role);
            
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized("Token inválido");
            }

            var existingNote = await _noteService.GetNoteByIdAsync(id);
            if (existingNote == null)
            {
                return NotFound("Nota não encontrada");
            }

            // Verifica permissões de acesso
            if (!await HasPermissionToAccessNote(existingNote, userId, userRoleClaim?.Value))
            {
                return Forbid("Você não tem permissão para editar esta nota");
            }

            var result = await _noteService.UpdateNoteAsync(id, request);
            
            _logger.LogInformation("Nota {NoteId} atualizada com sucesso", id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar nota {NoteId}", id);
            return BadRequest("Erro ao atualizar nota");
        }
    }

    /// <summary>
    /// Exclui uma nota do sistema.
    /// </summary>
    /// <param name="id">ID da nota</param>
    /// <returns>Resultado da operação</returns>
    /// <remarks>
    /// Requer autorização estrita. Apenas Admins podem excluir notas.
    /// </remarks>
    /// <response code="204">Nota excluída com sucesso</response>
    /// <response code="404">Nota não encontrada</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="403">Permissão negada</response>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
        public async Task<IActionResult> DeleteNote(int id)
    {
        try
        {
            var success = await _noteService.DeleteNoteAsync(id);
            if (!success)
            {
                return NotFound("Nota não encontrada");
            }

            _logger.LogInformation("Nota {NoteId} excluída com sucesso", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir nota {NoteId}", id);
            return BadRequest("Erro ao excluir nota");
        }
    }

    /// <summary>
    /// Lista todas as notas do usuário autenticado.
    /// </summary>
    /// <returns>Lista de notas do usuário</returns>
    /// <remarks>
    /// Retorna apenas as notas do usuário autenticado.
    /// Admins podem ver todas as notas (implementação futura).
    /// </remarks>
    /// <response code="200">Lista de notas retornada com sucesso</response>
    /// <response code="401">Não autorizado</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<NoteResponseDto>), 200)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<IEnumerable<NoteResponseDto>>> GetUserNotes()
    {
        try
        {
            var userIdClaim = User.FindFirst("userId");
            
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized("Token inválido");
            }

            var notes = await _noteService.GetUserNotesAsync(userId);
            return Ok(notes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar notas do usuário");
            return BadRequest("Erro ao listar notas");
        }
    }

    /// <summary>
    /// Verifica se o usuário tem permissão para acessar uma nota específica.
    /// </summary>
    /// <param name="note">Nota a ser verificada</param>
    /// <param name="userId">ID do usuário autenticado</param>
    /// <param name="userRole">Role do usuário autenticado</param>
    /// <returns>True se tem permissão, false caso contrário</returns>
    private async Task<bool> HasPermissionToAccessNote(NoteResponseDto note, int userId, string? userRole)
    {
        // Admins têm acesso total
        if (userRole == UserRole.Admin.ToString())
        {
            return true;
        }

        // Usuários só podem acessar suas próprias notas
        return note.UserId == userId;
    }
}
