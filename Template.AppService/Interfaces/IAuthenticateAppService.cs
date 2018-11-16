using Template.API.Commands.Authenticate;
using Template.AppService.ViewModels;
using Template.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Template.AppService.Interfaces
{
    public interface IAuthenticateAppService
    {
        UsuarioViewModel Authenticate(string emailcpf, string senha);
        bool RecuperarSenha(string email);
        string Email(string token);
        bool ResetarSenha(ResetPasswordCommand command);
    }
}
