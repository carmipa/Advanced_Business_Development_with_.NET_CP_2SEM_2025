namespace cp_5_autenticacao_autorizacao_swt.Domain.Enums
{
    /// <summary>
    /// Enum que define os status possíveis de um usuário no sistema
    /// </summary>
    /// <remarks>
    /// Este enum define os diferentes estados que um usuário pode ter no sistema.
    /// É usado para controlar o acesso e determinar se um usuário pode fazer login.
    /// </remarks>
    public enum UserStatus
    {
        /// <summary>
        /// Usuário ativo e pode acessar o sistema normalmente
        /// </summary>
        /// <remarks>
        /// Status padrão para usuários que podem fazer login e usar o sistema normalmente.
        /// </remarks>
        Active = 1,
        
        /// <summary>
        /// Usuário inativo e não pode acessar o sistema
        /// </summary>
        /// <remarks>
        /// Usuários inativos não conseguem fazer login. Geralmente usado para desativar contas temporariamente.
        /// </remarks>
        Inactive = 2,
        
        /// <summary>
        /// Usuário bloqueado temporariamente
        /// </summary>
        /// <remarks>
        /// Usuários bloqueados não conseguem fazer login. Usado para bloqueios por segurança ou violação de regras.
        /// </remarks>
        Blocked = 3,
        
        /// <summary>
        /// Usuário pendente de ativação
        /// </summary>
        /// <remarks>
        /// Usuários que se registraram mas ainda não confirmaram o email ou foram aprovados por um administrador.
        /// </remarks>
        Pending = 4
    }
}
