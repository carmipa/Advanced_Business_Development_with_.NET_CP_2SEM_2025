using System.ComponentModel.DataAnnotations;

namespace cp_5_autenticacao_autorizacao_swt.Application.DTOs.Auth
{
    /// <summary>
    /// DTO para requisição de registro de novo usuário
    /// </summary>
    /// <remarks>
    /// Este DTO contém todas as informações necessárias para criar uma nova conta de usuário no sistema.
    /// É usado no endpoint POST /api/auth/register para registrar novos usuários.
    /// Inclui validações para garantir que os dados estejam corretos e as senhas coincidam.
    /// </remarks>
    public class RegisterRequestDto
    {
        /// <summary>
        /// Nome completo do usuário
        /// </summary>
        /// <example>João Silva</example>
        [Required(ErrorMessage = "Nome é obrigatório")]
        [MinLength(2, ErrorMessage = "Nome deve ter pelo menos 2 caracteres")]
        [MaxLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;
        
        /// <summary>
        /// Email do usuário (deve ser único no sistema)
        /// </summary>
        /// <example>joao@exemplo.com</example>
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email deve ter um formato válido")]
        [MaxLength(100, ErrorMessage = "Email deve ter no máximo 100 caracteres")]
        public string Email { get; set; } = string.Empty;
        
        /// <summary>
        /// Senha do usuário
        /// </summary>
        /// <example>MinhaSenh@123</example>
        [Required(ErrorMessage = "Senha é obrigatória")]
        [MinLength(6, ErrorMessage = "Senha deve ter pelo menos 6 caracteres")]
        [MaxLength(50, ErrorMessage = "Senha deve ter no máximo 50 caracteres")]
        public string Senha { get; set; } = string.Empty;
        
        /// <summary>
        /// Confirmação da senha (deve ser igual à senha)
        /// </summary>
        /// <example>MinhaSenh@123</example>
        [Required(ErrorMessage = "Confirmação de senha é obrigatória")]
        [Compare(nameof(Senha), ErrorMessage = "Senhas não coincidem")]
        public string ConfirmarSenha { get; set; } = string.Empty;
    }
}
