using cp_5_autenticacao_autorizacao_swt.Application.DTOs.Auth;
using FluentValidation;

namespace cp_5_autenticacao_autorizacao_swt.Application.Validators
{
    /// <summary>
    /// Validador para RegisterRequestDto usando FluentValidation
    /// </summary>
    /// <remarks>
    /// Este validador define as regras de validação para dados de registro de novo usuário.
    /// Utiliza FluentValidation para validar nome, email, senha e confirmação de senha.
    /// Garante que os dados estejam no formato correto e atendam aos requisitos de segurança.
    /// </remarks>
    public class RegisterRequestValidator : AbstractValidator<RegisterRequestDto>
    {
        /// <summary>
        /// Construtor do validador de registro
        /// </summary>
        /// <remarks>
        /// Define as regras de validação para todos os campos:
        /// - Nome: obrigatório, 2-100 caracteres, apenas letras e espaços
        /// - Email: obrigatório, formato válido, máximo 100 caracteres
        /// - Senha: obrigatória, 6-50 caracteres, deve conter maiúscula, minúscula e número
        /// - ConfirmarSenha: obrigatória, deve ser igual à senha
        /// </remarks>
        public RegisterRequestValidator()
        {
            // Validação do nome
            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("Nome é obrigatório")
                .MinimumLength(2)
                .WithMessage("Nome deve ter pelo menos 2 caracteres")
                .MaximumLength(100)
                .WithMessage("Nome deve ter no máximo 100 caracteres")
                .Matches(@"^[a-zA-ZÀ-ÿ\s]+$")
                .WithMessage("Nome deve conter apenas letras e espaços");
            
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
                .WithMessage("Senha deve ter no máximo 50 caracteres")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)")
                .WithMessage("Senha deve conter pelo menos uma letra minúscula, uma maiúscula e um número");
            
            // Validação da confirmação de senha
            RuleFor(x => x.ConfirmarSenha)
                .NotEmpty()
                .WithMessage("Confirmação de senha é obrigatória")
                .Equal(x => x.Senha)
                .WithMessage("Senhas não coincidem");
        }
    }
}
