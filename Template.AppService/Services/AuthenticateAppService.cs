using Template.AppService.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Template.Domain.Models;
using Template.Domain.Interfaces;
using AutoMapper;
using Template.AppService.ViewModels;
using Template.CrossCutting.Util;
using System.Security.Cryptography;
using Template.API.Commands.Authenticate;
using Template.CrossCutting;
using Unean.Core.Notifications.Notify.Models;
using Newtonsoft.Json.Linq;

namespace Template.AppService.Services
{
    public class AuthenticateAppService : IAuthenticateAppService
    {
        private readonly IAuthenticateService _authenticateService;

        public AuthenticateAppService(IAuthenticateService authenticateService)
        {
            _authenticateService = authenticateService;
        }

        public UsuarioViewModel Authenticate(string emailcpf, string senha)
        {
            senha = Password.MD5Hash(senha);

            Usuario usuario = _authenticateService.Authenticate(emailcpf, senha);

            UsuarioViewModel usuarioViewModel = Mapper.Map<UsuarioViewModel>(usuario);

            return usuarioViewModel;
        }

        public string Email(string token)
        {
            return _authenticateService.EmailByTokenResetEmail(token);
        }

        public bool RecuperarSenha(string email)
        {
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                Byte[] bytes = new Byte[256];
                rng.GetBytes(bytes);

                string sendThisInEmailAndStoreInDB = Convert.ToBase64String(bytes);

                if (_authenticateService.SalvarTokenResetEmail(email, sendThisInEmailAndStoreInDB))
                {
                    var myEncodedString = System.Net.WebUtility.UrlEncode(sendThisInEmailAndStoreInDB);

                    string parametros = "{" + string.Format("\"@urlRecuperar\":\"http://youtapp.com.br/#/nova-senha/{0}\"", myEncodedString) + "}";
                    
                    return true;
                };

                return false;
            }
        }

        public bool ResetarSenha(ResetPasswordCommand command)
        {
            return _authenticateService.ResetSenha(command.Token, Password.MD5Hash(command.Senha));
        }
    }
}
