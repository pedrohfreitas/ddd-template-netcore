using Template.CrossCutting.FluentValidator.Validation;

namespace Template.API.Commands.Authenticate
{
    public class AuthenticateUserCommandContract : IContract
    {
        public ValidationContract Contract { get; }

        public AuthenticateUserCommandContract(AuthenticateUserCommand command)
        {
            Contract = new ValidationContract()
           .Requires()
           .IsNotNullOrEmpty(command.CPF, "Username", "O campo UserName � obrigat�rio")
           .IsNotNullOrEmpty(command.Senha, "Password", "O campo Password � obrigat�rio")
           .HasMinLen(command.Senha, 6, "Password", "O campo Password precisa ter no m�nimo 6 caracteres");
        }

        public AuthenticateUserCommandContract(ResetPasswordCommand command)
        {
            Contract = new ValidationContract()
           .Requires()
           .IsNotNullOrEmpty(command.Token, "Token", "O campo Token � obrigat�rio")
           .IsNotNullOrEmpty(command.Senha, "Password", "O campo Password � obrigat�rio")
           .HasMinLen(command.Senha, 6, "Password", "O campo Password precisa ter no m�nimo 6 caracteres");
        }
    }
}
