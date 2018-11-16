using Template.Domain.Models;

namespace Template.Domain.Interfaces
{
    public interface IAuthenticateService
    {
        Usuario Authenticate(string emailcpf, string senha);

        bool SalvarTokenResetEmail(string email, string token);

        string EmailByTokenResetEmail(string token);

        bool ResetSenha(string token, string senha);
    }
}
