using System.ComponentModel.DataAnnotations;

namespace cp_5_autenticacao_autorizacao_swt.Application.DTOs.Auth
{
    /// <summary>
    /// DTO para requisição de renovação de token JWT
    /// </summary>
    /// <remarks>
    /// Este DTO é usado para solicitar a renovação de um token JWT expirado.
    /// É utilizado no endpoint POST /api/auth/refresh-token para obter novos tokens
    /// usando um refresh token válido e não expirado.
    /// </remarks>
    public class RefreshTokenRequestDto
    {
        /// <summary>
        /// Token de refresh válido e não expirado
        /// </summary>
        /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...</example>
        [Required(ErrorMessage = "Refresh token é obrigatório")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
