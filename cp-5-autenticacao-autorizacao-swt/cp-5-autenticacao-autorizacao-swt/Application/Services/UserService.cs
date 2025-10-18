using AutoMapper;
using cp_5_autenticacao_autorizacao_swt.Application.DTOs.Auth;
using cp_5_autenticacao_autorizacao_swt.Application.Interfaces;
using cp_5_autenticacao_autorizacao_swt.Domain.Entities;
using cp_5_autenticacao_autorizacao_swt.Domain.Enums;
using cp_5_autenticacao_autorizacao_swt.Domain.Interfaces;
using BCrypt.Net;

namespace cp_5_autenticacao_autorizacao_swt.Application.Services
{
    /// <summary>
    /// Serviço responsável por operações de gerenciamento de usuários
    /// </summary>
    /// <remarks>
    /// Este serviço implementa todas as operações relacionadas ao gerenciamento de usuários,
    /// incluindo CRUD básico, busca por email, atualização de senha e controle de bloqueio.
    /// Utiliza AutoMapper para conversão entre DTOs e entidades.
    /// </remarks>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Construtor do serviço de usuários
        /// </summary>
        /// <param name="userRepository">Repositório para operações com usuários</param>
        /// <param name="mapper">Mapper para conversão entre DTOs e entidades</param>
        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Busca um usuário pelo ID
        /// </summary>
        /// <param name="id">ID único do usuário</param>
        /// <returns>DTO do usuário encontrado ou null se não encontrado</returns>
        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user != null ? _mapper.Map<UserDto>(user) : null;
        }

        /// <summary>
        /// Busca um usuário pelo email
        /// </summary>
        /// <param name="email">Email único do usuário</param>
        /// <returns>DTO do usuário encontrado ou null se não encontrado</returns>
        public async Task<UserDto?> GetByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return user != null ? _mapper.Map<UserDto>(user) : null;
        }

        /// <summary>
        /// Lista todos os usuários cadastrados no sistema
        /// </summary>
        /// <returns>Lista de DTOs de todos os usuários</returns>
        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        /// <summary>
        /// Atualiza as informações de um usuário existente
        /// </summary>
        /// <param name="id">ID do usuário a ser atualizado</param>
        /// <param name="userDto">Dados atualizados do usuário</param>
        /// <returns>DTO do usuário atualizado ou null se não encontrado ou email já existe</returns>
        public async Task<UserDto?> UpdateAsync(int id, UserDto userDto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return null;

            // Verifica se o email já existe em outro usuário
            if (user.Email != userDto.Email && await _userRepository.EmailExistsAsync(userDto.Email, id))
                return null;

            // Atualiza as propriedades
            user.Nome = userDto.Nome;
            user.Email = userDto.Email;
            user.Role = Enum.Parse<UserRole>(userDto.Role);
            user.DataAtualizacao = DateTime.UtcNow;

            var updatedUser = await _userRepository.UpdateAsync(user);
            return _mapper.Map<UserDto>(updatedUser);
        }

        /// <summary>
        /// Remove um usuário do sistema permanentemente
        /// </summary>
        /// <param name="id">ID do usuário a ser removido</param>
        /// <returns>True se removido com sucesso, false se não encontrado</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            return await _userRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Atualiza a senha de um usuário
        /// </summary>
        /// <param name="id">ID do usuário</param>
        /// <param name="novaSenha">Nova senha em texto plano (será hasheada)</param>
        /// <returns>True se senha atualizada com sucesso, false se usuário não encontrado</returns>
        public async Task<bool> UpdatePasswordAsync(int id, string novaSenha)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return false;

            user.SenhaHash = BCrypt.Net.BCrypt.HashPassword(novaSenha);
            user.DataAtualizacao = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            return true;
        }

        /// <summary>
        /// Bloqueia ou desbloqueia um usuário no sistema
        /// </summary>
        /// <param name="id">ID do usuário</param>
        /// <param name="bloquear">True para bloquear, false para desbloquear</param>
        /// <returns>True se operação realizada com sucesso, false se usuário não encontrado</returns>
        public async Task<bool> ToggleBlockAsync(int id, bool bloquear)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return false;

            if (bloquear)
            {
                user.Status = UserStatus.Blocked;
                user.DataBloqueio = DateTime.UtcNow;
            }
            else
            {
                user.Status = UserStatus.Active;
                user.DataBloqueio = null;
                user.TentativasLoginFalhadas = 0;
            }

            user.DataAtualizacao = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);
            return true;
        }
    }
}
