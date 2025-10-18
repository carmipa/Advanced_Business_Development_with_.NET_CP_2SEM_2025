using AutoMapper;
using cp_5_autenticacao_autorizacao_swt.Application.DTOs.Auth;
using cp_5_autenticacao_autorizacao_swt.Domain.Entities;

namespace cp_5_autenticacao_autorizacao_swt.Application
{
    /// <summary>
    /// Perfil de mapeamento do AutoMapper
    /// </summary>
    /// <remarks>
    /// Esta classe define os mapeamentos entre entidades de domínio e DTOs usando AutoMapper.
    /// Facilita a conversão de dados entre as camadas da aplicação, mantendo a separação de responsabilidades.
    /// </remarks>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Construtor do perfil de mapeamento
        /// </summary>
        /// <remarks>
        /// Define os mapeamentos bidirecionais entre User e UserDto:
        /// - User -> UserDto: converte enum Role para string
        /// - UserDto -> User: ignora campos sensíveis e de auditoria, converte string Role para enum
        /// </remarks>
        public MappingProfile()
        {
            // Mapeamento de User para UserDto (entidade para DTO)
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));
            
            // Mapeamento de UserDto para User (DTO para entidade - usado em atualizações)
            CreateMap<UserDto, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // ID não deve ser alterado
                .ForMember(dest => dest.SenhaHash, opt => opt.Ignore()) // Senha não é atualizada por este mapeamento
                .ForMember(dest => dest.Status, opt => opt.Ignore()) // Status é gerenciado separadamente
                .ForMember(dest => dest.DataCriacao, opt => opt.Ignore()) // Data de criação não muda
                .ForMember(dest => dest.DataAtualizacao, opt => opt.Ignore()) // Data de atualização é definida no serviço
                .ForMember(dest => dest.UltimoLogin, opt => opt.Ignore()) // Último login é gerenciado pelo AuthService
                .ForMember(dest => dest.TentativasLoginFalhadas, opt => opt.Ignore()) // Tentativas são gerenciadas pelo AuthService
                .ForMember(dest => dest.DataBloqueio, opt => opt.Ignore()) // Data de bloqueio é gerenciada pelo UserService
                .ForMember(dest => dest.RefreshToken, opt => opt.Ignore()) // Refresh token é gerenciado pelo AuthService
                .ForMember(dest => dest.RefreshTokenExpiryTime, opt => opt.Ignore()) // Expiração é gerenciada pelo AuthService
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => Enum.Parse<Domain.Enums.UserRole>(src.Role)));
        }
    }
}
