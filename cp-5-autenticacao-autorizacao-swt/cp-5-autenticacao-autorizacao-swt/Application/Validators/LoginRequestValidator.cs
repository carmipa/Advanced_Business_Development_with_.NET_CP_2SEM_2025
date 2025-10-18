using cp_5_autenticacao_autorizacao_swt.Application.DTOs.Auth;
using FluentValidation;

namespace cp_5_autenticacao_autorizacao_swt.Application.Validators
{
    /// <summary>
    /// Validador para LoginRequestDto usando FluentValidation
    /// </summary>
    /// <remarks>
    /// Este validador define as regras de validação para dados de login de usuário.
    /// Utiliza FluentValidation para validar email e senha antes do processamento.
    /// Garante que os dados estejam no formato correto e atendam aos requisitos de segurança.
    /// </remarks>
    public class LoginRequestValidator : AbstractValidator<LoginRequestDto>
    {
        /// <summary>
        /// Construtor do validador de login
        /// </summary>
        /// <remarks>
        /// Define as regras de validação para email e senha:
        /// - Email: obrigatório, formato válido, máximo 100 caracteres
        /// - Senha: obrigatória, mínimo 6 caracteres, máximo 50 caracteres
        /// </remarks>
        public LoginRequestValidator()
        {
            // Validação do email
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email é obrigatório")
                .EmailAddress()
                .WithMessage("Email deve ter um formato válido")
                .MaximumLength(100)
                .WithMessage("Email deve ter no máximo 100 caracteres");
            
            // Validação da senha
            RuleFor(x => x.Senha)
                .NotEmpty()
                .WithMessage("Senha é obrigatória")
                .MinimumLength(6)
                .WithMessage("Senha deve ter pelo menos 6 caracteres")
                .MaximumLength(50)
                .WithMessage("Senha deve ter no máximo 50 caracteres");
        }
    }
}
