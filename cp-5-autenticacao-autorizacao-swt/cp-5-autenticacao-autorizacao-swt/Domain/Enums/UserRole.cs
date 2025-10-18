namespace cp_5_autenticacao_autorizacao_swt.Domain.Enums
{
    /// <summary>
    /// Enum que define os tipos de roles/perfis de usuário no sistema
    /// </summary>
    /// <remarks>
    /// Este enum define os diferentes níveis de acesso e permissões que um usuário pode ter no sistema.
    /// É usado para controle de autorização e determina quais operações cada usuário pode realizar.
    /// </remarks>
    public enum UserRole
    {
        /// <summary>
        /// Usuário administrador com acesso total ao sistema
        /// </summary>
        /// <remarks>
        /// Administradores têm acesso completo a todas as funcionalidades do sistema,
        /// incluindo gerenciamento de usuários, visualização de logs e configurações.
        /// </remarks>
        Admin = 1,
        
        /// <summary>
        /// Usuário comum com acesso limitado
        /// </summary>
        /// <remarks>
        /// Usuários comuns têm acesso apenas às funcionalidades básicas do sistema,
        /// podendo visualizar e editar apenas seus próprios dados.
        /// </remarks>
        User = 2,
        
        /// <summary>
        /// Usuário moderador com privilégios intermediários
        /// </summary>
        /// <remarks>
        /// Moderadores têm privilégios intermediários, podendo gerenciar alguns usuários
        /// e acessar funcionalidades administrativas limitadas.
        /// </remarks>
        Moderator = 3
    }
}
