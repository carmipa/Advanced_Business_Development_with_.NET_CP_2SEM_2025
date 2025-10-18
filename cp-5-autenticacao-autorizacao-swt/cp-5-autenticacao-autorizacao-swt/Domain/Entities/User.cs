using cp_5_autenticacao_autorizacao_swt.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace cp_5_autenticacao_autorizacao_swt.Domain.Entities
{
    /// <summary>
    /// Entidade que representa um usuário do sistema
    /// </summary>
    /// <remarks>
    /// Esta entidade representa um usuário completo do sistema, contendo todas as informações
    /// necessárias para autenticação, autorização e gerenciamento de usuários.
    /// Herda de BaseEntity para ter ID, timestamps de criação e atualização.
    /// Inclui funcionalidades de segurança como controle de tentativas de login e refresh tokens.
    /// </remarks>
    public class User : BaseEntity
    {
        /// <summary>
        /// Nome completo do usuário
        /// </summary>
        /// <example>João Silva</example>
        [Required]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;
        
        /// <summary>
        /// Endereço de email único do usuário (usado para login)
        /// </summary>
        /// <example>joao@exemplo.com</example>
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;
        
        /// <summary>
        /// Hash da senha do usuário (nunca armazenar senha em texto plano)
        /// </summary>
        /// <example>$2a$11$...</example>
        [Required]
        [MaxLength(255)]
        public string SenhaHash { get; set; } = string.Empty;
        
        /// <summary>
        /// Role/perfil do usuário no sistema (User, Admin, Moderator)
        /// </summary>
        /// <example>User</example>
        public UserRole Role { get; set; } = UserRole.User;
        
        /// <summary>
        /// Status atual do usuário (ativo, inativo, bloqueado, etc.)
        /// </summary>
        /// <example>Active</example>
        public UserStatus Status { get; set; } = UserStatus.Active;
        
        /// <summary>
        /// Data e hora da última tentativa de login bem-sucedida
        /// </summary>
        /// <example>2024-01-01T10:00:00Z</example>
        public DateTime? UltimoLogin { get; set; }
        
        /// <summary>
        /// Número de tentativas de login falhadas consecutivas
        /// </summary>
        /// <example>0</example>
        public int TentativasLoginFalhadas { get; set; } = 0;
        
        /// <summary>
        /// Data e hora em que o usuário foi bloqueado (se aplicável)
        /// </summary>
        /// <example>2024-01-01T12:00:00Z</example>
        public DateTime? DataBloqueio { get; set; }
        
        /// <summary>
        /// Token de refresh para renovação automática de tokens JWT
        /// </summary>
        /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...</example>
        public string? RefreshToken { get; set; }
        
        /// <summary>
        /// Data e hora de expiração do refresh token
        /// </summary>
        /// <example>2024-01-08T10:00:00Z</example>
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
