using Template.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Template.Domain.Models;
using Template.Domain.Repositories;

namespace Template.Domain.Services
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly IAuthenticateRepository _authenticateRepository;

        public AuthenticateService(IAuthenticateRepository authenticateRepository)
        {
            _authenticateRepository = authenticateRepository;
        }

        public Usuario Authenticate(string emailcpf, string senha)
        {
            Usuario usuario = _authenticateRepository.Authenticate(emailcpf, senha);

            return usuario;
        }

        public string EmailByTokenResetEmail(string token)
        {
            return _authenticateRepository.EmailByTokenResetEmail(token);
        }

        public bool ResetSenha(string token, string senha)
        {
            return _authenticateRepository.ResetSenha(token, senha);
        }

        public bool SalvarTokenResetEmail(string email, string token)
        {
            return _authenticateRepository.SalvarTokenResetEmail(email, token);
        }
    }
}
