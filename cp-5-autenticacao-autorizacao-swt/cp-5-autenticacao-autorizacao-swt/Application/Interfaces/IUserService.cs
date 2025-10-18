using cp_5_autenticacao_autorizacao_swt.Application.DTOs.Auth;
using cp_5_autenticacao_autorizacao_swt.Domain.Entities;

namespace cp_5_autenticacao_autorizacao_swt.Application.Interfaces
{
    /// <summary>
    /// Interface que define as operações de gerenciamento de usuários
    /// </summary>
    /// <remarks>
    /// Esta interface define o contrato para todas as operações relacionadas ao gerenciamento
    /// de usuários no sistema. Inclui métodos para CRUD básico, busca por email, atualização
    /// de senha e controle de bloqueio/desbloqueio de usuários.
    /// </remarks>
    public interface IUserService
    {
        /// <summary>
        /// Busca um usuário pelo ID
        /// </summary>
        /// <param name="id">ID único do usuário</param>
        /// <returns>DTO do usuário encontrado ou null se não encontrado</returns>
        Task<UserDto?> GetByIdAsync(int id);
        
        /// <summary>
        /// Busca um usuário pelo email
        /// </summary>
        /// <param name="email">Email único do usuário</param>
        /// <returns>DTO do usuário encontrado ou null se não encontrado</returns>
        Task<UserDto?> GetByEmailAsync(string email);
        
        /// <summary>
        /// Lista todos os usuários cadastrados no sistema
        /// </summary>
        /// <returns>Lista de DTOs de todos os usuários</returns>
        Task<IEnumerable<UserDto>> GetAllAsync();
        
        /// <summary>
        /// Atualiza as informações de um usuário existente
        /// </summary>
        /// <param name="id">ID do usuário a ser atualizado</param>
        /// <param name="userDto">Dados atualizados do usuário</param>
        /// <returns>DTO do usuário atualizado ou null se não encontrado</returns>
        Task<UserDto?> UpdateAsync(int id, UserDto userDto);
        
        /// <summary>
        /// Remove um usuário do sistema permanentemente
        /// </summary>
        /// <param name="id">ID do usuário a ser removido</param>
        /// <returns>True se removido com sucesso, false se não encontrado</returns>
        Task<bool> DeleteAsync(int id);
        
        /// <summary>
        /// Atualiza a senha de um usuário
        /// </summary>
        /// <param name="id">ID do usuário</param>
        /// <param name="novaSenha">Nova senha em texto plano (será hasheada)</param>
        /// <returns>True se senha atualizada com sucesso, false se usuário não encontrado</returns>
        Task<bool> UpdatePasswordAsync(int id, string novaSenha);
        
        /// <summary>
        /// Bloqueia ou desbloqueia um usuário no sistema
        /// </summary>
        /// <param name="id">ID do usuário</param>
        /// <param name="bloquear">True para bloquear, false para desbloquear</param>
        /// <returns>True se operação realizada com sucesso, false se usuário não encontrado</returns>
        Task<bool> ToggleBlockAsync(int id, bool bloquear);
    }
}
