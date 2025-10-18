using cp_5_autenticacao_autorizacao_swt.Domain.Entities;
using cp_5_autenticacao_autorizacao_swt.Domain.Interfaces;
using cp_5_autenticacao_autorizacao_swt.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace cp_5_autenticacao_autorizacao_swt.Infrastructure.Repositories
{
    /// <summary>
    /// Implementação do repositório de usuários usando Entity Framework Core
    /// </summary>
    /// <remarks>
    /// Esta classe implementa o padrão Repository para a entidade User,
    /// fornecendo operações de acesso a dados usando Entity Framework Core.
    /// Abstrai as operações de banco de dados da camada de aplicação.
    /// </remarks>
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Construtor do repositório de usuários
        /// </summary>
        /// <param name="context">Contexto do Entity Framework</param>
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Busca um usuário pelo ID
        /// </summary>
        /// <param name="id">ID único do usuário</param>
        /// <returns>Entidade User encontrada ou null se não existir</returns>
        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        /// <summary>
        /// Busca um usuário pelo email
        /// </summary>
        /// <param name="email">Email único do usuário</param>
        /// <returns>Entidade User encontrada ou null se não existir</returns>
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        /// <summary>
        /// Lista todos os usuários cadastrados no sistema
        /// </summary>
        /// <returns>Lista de todas as entidades User ordenadas por nome</returns>
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .OrderBy(u => u.Nome)
                .ToListAsync();
        }

        /// <summary>
        /// Adiciona um novo usuário ao sistema
        /// </summary>
        /// <param name="user">Entidade User a ser persistida</param>
        /// <returns>Entidade User com ID gerado pelo banco de dados</returns>
        public async Task<User> AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        /// <summary>
        /// Atualiza um usuário existente no sistema
        /// </summary>
        /// <param name="user">Entidade User com dados atualizados</param>
        /// <returns>Entidade User atualizada</returns>
        public async Task<User> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        /// <summary>
        /// Remove um usuário do sistema permanentemente
        /// </summary>
        /// <param name="id">ID do usuário a ser removido</param>
        /// <returns>True se removido com sucesso, false se não encontrado</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Verifica se um email já está em uso por outro usuário
        /// </summary>
        /// <param name="email">Email a ser verificado</param>
        /// <param name="excludeUserId">ID do usuário a ser excluído da verificação (útil para atualizações)</param>
        /// <returns>True se email já está em uso, false caso contrário</returns>
        public async Task<bool> EmailExistsAsync(string email, int? excludeUserId = null)
        {
            var query = _context.Users.Where(u => u.Email == email);
            
            if (excludeUserId.HasValue)
                query = query.Where(u => u.Id != excludeUserId.Value);
            
            return await query.AnyAsync();
        }
    }
}
