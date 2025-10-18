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
        /// Leitor - Pode apenas visualizar as suas próprias notas.
        /// </summary>
        /// <remarks>
        /// Usuários com role Leitor têm acesso somente de leitura às suas próprias notas,
        /// não podendo criar, editar ou excluir conteúdo.
        /// </remarks>
        Leitor = 1,
        
        /// <summary>
        /// Editor - Pode criar e editar as suas próprias notas.
        /// </summary>
        /// <remarks>
        /// Usuários com role Editor podem criar, visualizar e editar suas próprias notas,
        /// mas não têm acesso às notas de outros usuários.
        /// </remarks>
        Editor = 2,
        
        /// <summary>
        /// Admin - Possui controle total, podendo visualizar, editar e apagar as notas de qualquer usuário.
        /// </summary>
        /// <remarks>
        /// Administradores têm acesso completo a todas as funcionalidades do sistema,
        /// incluindo gerenciamento de usuários, visualização de logs e acesso total às notas.
        /// </remarks>
        Admin = 3
    }
}
