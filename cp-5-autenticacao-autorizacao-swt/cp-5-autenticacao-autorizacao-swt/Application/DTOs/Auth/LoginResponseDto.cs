namespace cp_5_autenticacao_autorizacao_swt.Application.DTOs.Auth
{
    /// <summary>
    /// DTO para resposta de login bem-sucedido
    /// </summary>
    /// <remarks>
    /// Este DTO é retornado quando um login é realizado com sucesso.
    /// Contém todos os tokens necessários para autenticação e as informações do usuário.
    /// É usado nos endpoints de login e registro para retornar os dados de autenticação.
    /// </remarks>
    public class LoginResponseDto
    {
        /// <summary>
        /// Token JWT para autenticação em requisições subsequentes
        /// </summary>
        /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...</example>
        public string Token { get; set; } = string.Empty;
        
        /// <summary>
        /// Token de refresh para renovação automática do JWT
        /// </summary>
        /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...</example>
        public string RefreshToken { get; set; } = string.Empty;
        
        /// <summary>
        /// Data e hora de expiração do token JWT
        /// </summary>
        /// <example>2024-01-01T12:00:00Z</example>
        public DateTime ExpiresAt { get; set; }
        
        /// <summary>
        /// Informações básicas do usuário autenticado
        /// </summary>
        public UserDto User { get; set; } = new();
    }
    
    /// <summary>
    /// DTO com informações básicas do usuário
    /// </summary>
    /// <remarks>
    /// Este DTO contém as informações básicas de um usuário que são retornadas
    /// em respostas da API, excluindo dados sensíveis como senha.
    /// É usado em diversos endpoints para retornar dados do usuário.
    /// </remarks>
    public class UserDto
    {
        /// <summary>
        /// ID único do usuário no sistema
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }
        
        /// <summary>
        /// Nome completo do usuário
        /// </summary>
        /// <example>João Silva</example>
        public string Nome { get; set; } = string.Empty;
        
        /// <summary>
        /// Email do usuário
        /// </summary>
        /// <example>joao@exemplo.com</example>
        public string Email { get; set; } = string.Empty;
        
        /// <summary>
        /// Role/perfil do usuário (User, Admin, Moderator)
        /// </summary>
        /// <example>User</example>
        public string Role { get; set; } = string.Empty;
    }
}
