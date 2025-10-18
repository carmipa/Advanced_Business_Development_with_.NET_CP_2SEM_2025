using System.ComponentModel.DataAnnotations;

namespace cp_5_autenticacao_autorizacao_swt.Application.DTOs.Auth
{
    /// <summary>
    /// DTO para requisição de login de usuário
    /// </summary>
    /// <remarks>
    /// Este DTO contém os dados necessários para realizar o login de um usuário no sistema.
    /// É usado no endpoint POST /api/auth/login para autenticar usuários existentes.
    /// Contém validações para garantir que os dados estejam no formato correto.
    /// </remarks>
    public class LoginRequestDto
    {
        /// <summary>
        /// Email do usuário para autenticação
        /// </summary>
        /// <example>usuario@exemplo.com</example>
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email deve ter um formato válido")]
        public string Email { get; set; } = string.Empty;
        
        /// <summary>
        /// Senha do usuário para autenticação
        /// </summary>
        /// <example>MinhaSenh@123</example>
        [Required(ErrorMessage = "Senha é obrigatória")]
        [MinLength(6, ErrorMessage = "Senha deve ter pelo menos 6 caracteres")]
        public string Senha { get; set; } = string.Empty;
    }
}
