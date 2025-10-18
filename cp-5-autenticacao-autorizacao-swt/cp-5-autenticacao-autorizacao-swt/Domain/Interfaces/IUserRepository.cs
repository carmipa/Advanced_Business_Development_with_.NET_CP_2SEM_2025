using cp_5_autenticacao_autorizacao_swt.Domain.Entities;

namespace cp_5_autenticacao_autorizacao_swt.Domain.Interfaces
{
    /// <summary>
    /// Interface que define as operações de repositório para a entidade User
    /// </summary>
    /// <remarks>
    /// Esta interface define o contrato para o repositório de usuários, seguindo o padrão Repository.
    /// Abstrai o acesso aos dados da entidade User, permitindo diferentes implementações
    /// (Entity Framework, Dapper, etc.) sem afetar a camada de aplicação.
    /// </remarks>
    public interface IUserRepository
    {
        /// <summary>
        /// Busca um usuário pelo ID
        /// </summary>
        /// <param name="id">ID único do usuário</param>
        /// <returns>Entidade User encontrada ou null se não existir</returns>
        Task<User?> GetByIdAsync(int id);
        
        /// <summary>
        /// Busca um usuário pelo email
        /// </summary>
        /// <param name="email">Email único do usuário</param>
        /// <returns>Entidade User encontrada ou null se não existir</returns>
        Task<User?> GetByEmailAsync(string email);
        
        /// <summary>
        /// Lista todos os usuários cadastrados no sistema
        /// </summary>
        /// <returns>Lista de todas as entidades User</returns>
        Task<IEnumerable<User>> GetAllAsync();
        
        /// <summary>
        /// Adiciona um novo usuário ao sistema
        /// </summary>
        /// <param name="user">Entidade User a ser persistida</param>
        /// <returns>Entidade User com ID gerado pelo banco de dados</returns>
        Task<User> AddAsync(User user);
        
        /// <summary>
        /// Atualiza um usuário existente no sistema
        /// </summary>
        /// <param name="user">Entidade User com dados atualizados</param>
        /// <returns>Entidade User atualizada</returns>
        Task<User> UpdateAsync(User user);
        
        /// <summary>
        /// Remove um usuário do sistema permanentemente
        /// </summary>
        /// <param name="id">ID do usuário a ser removido</param>
        /// <returns>True se removido com sucesso, false se não encontrado</returns>
        Task<bool> DeleteAsync(int id);
        
        /// <summary>
        /// Verifica se um email já está em uso por outro usuário
        /// </summary>
        /// <param name="email">Email a ser verificado</param>
        /// <param name="excludeUserId">ID do usuário a ser excluído da verificação (útil para atualizações)</param>
        /// <returns>True se email já está em uso, false caso contrário</returns>
        Task<bool> EmailExistsAsync(string email, int? excludeUserId = null);
    }
}
