using cp_5_autenticacao_autorizacao_swt.Application.DTOs.Auth;
using cp_5_autenticacao_autorizacao_swt.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace cp_5_autenticacao_autorizacao_swt.Presentation.Controllers
{
    /// <summary>
    /// Controller responsável por operações de gerenciamento de usuários
    /// Endpoints para CRUD de usuários e operações relacionadas
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Requer autenticação para todos os endpoints
    [Produces("application/json")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Lista todos os usuários cadastrados no sistema
        /// </summary>
        /// <remarks>
        /// Este endpoint retorna uma lista completa de todos os usuários cadastrados no sistema.
        /// Apenas usuários com role "Admin" podem acessar este endpoint.
        /// A lista inclui informações básicas como ID, nome, email e role de cada usuário.
        /// 
        /// Exemplo de requisição:
        /// ```
        /// GET /api/users
        /// Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
        /// ```
        /// </remarks>
        /// <returns>Lista de todos os usuários cadastrados no sistema</returns>
        /// <response code="200">Lista de usuários retornada com sucesso</response>
        /// <response code="401">Token JWT inválido ou expirado</response>
        /// <response code="403">Acesso negado. Apenas administradores podem listar usuários</response>
        /// <response code="500">Erro interno do servidor durante a consulta</response>
        [HttpGet]
        [Authorize(Roles = "Admin")] // Apenas administradores
        [ProducesResponseType(typeof(IEnumerable<UserDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _userService.GetAllAsync();
                _logger.LogInformation("Lista de usuários consultada");
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar usuários");
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Busca um usuário específico pelo ID
        /// </summary>
        /// <remarks>
        /// Este endpoint permite buscar um usuário específico pelo seu ID.
        /// Usuários comuns só podem visualizar seus próprios dados, enquanto administradores podem visualizar qualquer usuário.
        /// 
        /// Exemplo de requisição:
        /// ```
        /// GET /api/users/123
        /// Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
        /// ```
        /// </remarks>
        /// <param name="id">ID único do usuário a ser buscado</param>
        /// <returns>Dados completos do usuário encontrado</returns>
        /// <response code="200">Usuário encontrado e retornado com sucesso</response>
        /// <response code="401">Token JWT inválido ou expirado</response>
        /// <response code="403">Acesso negado. Usuário não pode visualizar dados de outros usuários</response>
        /// <response code="404">Usuário não encontrado com o ID fornecido</response>
        /// <response code="500">Erro interno do servidor durante a busca</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                var currentUserRole = GetCurrentUserRole();

                // Usuários só podem ver seus próprios dados, exceto admins
                if (currentUserId != id && currentUserRole != "Admin")
                {
                    _logger.LogWarning("Tentativa de acesso negado para usuário {CurrentUserId} a dados do usuário {UserId}", 
                        currentUserId, id);
                    return Forbid();
                }

                var user = await _userService.GetByIdAsync(id);
                if (user == null)
                {
                    _logger.LogWarning("Usuário não encontrado: {UserId}", id);
                    return NotFound(new { message = "Usuário não encontrado" });
                }

                _logger.LogInformation("Usuário consultado: {UserId}", id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar usuário: {UserId}", id);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Atualiza as informações de um usuário existente
        /// </summary>
        /// <remarks>
        /// Este endpoint permite atualizar as informações de um usuário existente.
        /// Usuários comuns só podem atualizar seus próprios dados, enquanto administradores podem atualizar qualquer usuário.
        /// Usuários comuns não podem alterar o role, apenas administradores têm essa permissão.
        /// 
        /// Exemplo de requisição:
        /// ```
        /// PUT /api/users/123
        /// Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
        /// {
        ///   "nome": "João Silva Atualizado",
        ///   "email": "joao.novo@exemplo.com"
        /// }
        /// ```
        /// </remarks>
        /// <param name="id">ID único do usuário a ser atualizado</param>
        /// <param name="userDto">Dados atualizados do usuário (nome, email, role)</param>
        /// <returns>Dados do usuário após a atualização</returns>
        /// <response code="200">Usuário atualizado com sucesso</response>
        /// <response code="400">Dados de entrada inválidos ou formato incorreto</response>
        /// <response code="401">Token JWT inválido ou expirado</response>
        /// <response code="403">Acesso negado. Usuário não pode atualizar dados de outros usuários</response>
        /// <response code="404">Usuário não encontrado com o ID fornecido</response>
        /// <response code="500">Erro interno do servidor durante a atualização</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Update(int id, [FromBody] UserDto userDto)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                var currentUserRole = GetCurrentUserRole();

                // Usuários só podem atualizar seus próprios dados, exceto admins
                if (currentUserId != id && currentUserRole != "Admin")
                {
                    _logger.LogWarning("Tentativa de atualização negada para usuário {CurrentUserId} do usuário {UserId}", 
                        currentUserId, id);
                    return Forbid();
                }

                // Usuários comuns não podem alterar o role
                if (currentUserRole != "Admin" && !string.IsNullOrEmpty(userDto.Role))
                {
                    userDto.Role = string.Empty; // Remove o role da atualização
                }

                var result = await _userService.UpdateAsync(id, userDto);
                if (result == null)
                {
                    _logger.LogWarning("Usuário não encontrado para atualização: {UserId}", id);
                    return NotFound(new { message = "Usuário não encontrado" });
                }

                _logger.LogInformation("Usuário atualizado: {UserId}", id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar usuário: {UserId}", id);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Remove um usuário do sistema permanentemente
        /// </summary>
        /// <remarks>
        /// Este endpoint permite remover um usuário do sistema de forma permanente.
        /// Apenas usuários com role "Admin" podem executar esta operação.
        /// A remoção é irreversível e todos os dados do usuário serão perdidos.
        /// 
        /// Exemplo de requisição:
        /// ```
        /// DELETE /api/users/123
        /// Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
        /// ```
        /// </remarks>
        /// <param name="id">ID único do usuário a ser removido</param>
        /// <returns>Confirmação da remoção realizada com sucesso</returns>
        /// <response code="200">Usuário removido com sucesso do sistema</response>
        /// <response code="401">Token JWT inválido ou expirado</response>
        /// <response code="403">Acesso negado. Apenas administradores podem remover usuários</response>
        /// <response code="404">Usuário não encontrado com o ID fornecido</response>
        /// <response code="500">Erro interno do servidor durante a remoção</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Apenas administradores
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _userService.DeleteAsync(id);
                if (!result)
                {
                    _logger.LogWarning("Usuário não encontrado para remoção: {UserId}", id);
                    return NotFound(new { message = "Usuário não encontrado" });
                }

                _logger.LogInformation("Usuário removido: {UserId}", id);
                return Ok(new { message = "Usuário removido com sucesso" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover usuário: {UserId}", id);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Bloqueia ou desbloqueia um usuário no sistema
        /// </summary>
        /// <remarks>
        /// Este endpoint permite bloquear ou desbloquear um usuário no sistema.
        /// Apenas usuários com role "Admin" podem executar esta operação.
        /// Usuários bloqueados não conseguem fazer login no sistema.
        /// 
        /// Exemplo de requisição:
        /// ```
        /// PATCH /api/users/123/toggle-block?bloquear=true
        /// Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
        /// ```
        /// </remarks>
        /// <param name="id">ID único do usuário a ser bloqueado/desbloqueado</param>
        /// <param name="bloquear">True para bloquear o usuário, false para desbloquear</param>
        /// <returns>Confirmação da operação de bloqueio/desbloqueio</returns>
        /// <response code="200">Operação de bloqueio/desbloqueio realizada com sucesso</response>
        /// <response code="401">Token JWT inválido ou expirado</response>
        /// <response code="403">Acesso negado. Apenas administradores podem bloquear/desbloquear usuários</response>
        /// <response code="404">Usuário não encontrado com o ID fornecido</response>
        /// <response code="500">Erro interno do servidor durante a operação</response>
        [HttpPatch("{id}/toggle-block")]
        [Authorize(Roles = "Admin")] // Apenas administradores
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ToggleBlock(int id, [FromQuery] bool bloquear)
        {
            try
            {
                var result = await _userService.ToggleBlockAsync(id, bloquear);
                if (!result)
                {
                    _logger.LogWarning("Usuário não encontrado para bloqueio/desbloqueio: {UserId}", id);
                    return NotFound(new { message = "Usuário não encontrado" });
                }

                var action = bloquear ? "bloqueado" : "desbloqueado";
                _logger.LogInformation("Usuário {Action}: {UserId}", action, id);
                return Ok(new { message = $"Usuário {action} com sucesso" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao bloquear/desbloquear usuário: {UserId}", id);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Obtém o perfil do usuário autenticado
        /// </summary>
        /// <remarks>
        /// Este endpoint retorna as informações do perfil do usuário atualmente autenticado.
        /// O usuário só pode visualizar seus próprios dados através deste endpoint.
        /// 
        /// Exemplo de requisição:
        /// ```
        /// GET /api/users/profile
        /// Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
        /// ```
        /// </remarks>
        /// <returns>Dados completos do perfil do usuário autenticado</returns>
        /// <response code="200">Perfil do usuário retornado com sucesso</response>
        /// <response code="401">Token JWT inválido ou expirado</response>
        /// <response code="404">Perfil do usuário não encontrado</response>
        /// <response code="500">Erro interno do servidor durante a consulta</response>
        [HttpGet("profile")]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                var user = await _userService.GetByIdAsync(currentUserId);
                
                if (user == null)
                {
                    _logger.LogWarning("Perfil não encontrado para usuário: {UserId}", currentUserId);
                    return NotFound(new { message = "Perfil não encontrado" });
                }

                _logger.LogInformation("Perfil consultado: {UserId}", currentUserId);
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar perfil");
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Obtém o ID do usuário atual a partir do token JWT
        /// </summary>
        /// <returns>ID do usuário autenticado ou 0 se não encontrado</returns>
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("userId");
            return userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId) ? userId : 0;
        }

        /// <summary>
        /// Obtém o role do usuário atual a partir do token JWT
        /// </summary>
        /// <returns>Role do usuário autenticado ou "User" como padrão</returns>
        private string GetCurrentUserRole()
        {
            return User.FindFirst(ClaimTypes.Role)?.Value ?? "User";
        }
    }
}
