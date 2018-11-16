using System;
using System.Collections.Generic;
using System.Text;
using Template.Domain.Models;

namespace Template.Domain.Repositories
{
    public interface IAuthenticateRepository
    {
        Usuario Authenticate(string emailcpf, string senha);
        string EmailByTokenResetEmail(string token);
        bool SalvarTokenResetEmail(string email, string token);
        bool ResetSenha(string token, string senha);
    }
}
