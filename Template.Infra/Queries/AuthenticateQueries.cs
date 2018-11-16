using System;
using System.Collections.Generic;
using System.Text;

namespace Template.Infra.Repositories
{
    public partial class AuthenticateRepository
    {
        #region [ SelectUsuario ]
        private const string SelectUsuario = @"
            SELECT 
                 us.[Id]
                ,us.[Login]
                ,us.[IdUsuarioStatus]
                ,us.[DataCadastro]
                ,us.[Imagem]
                ,p.NomeFantasia as Nome
                ,p.Id AS IdPessoa
                ,p.IdPessoaPai
            FROM [Usuario] us
            JOIN Pessoa as p ON us.IdPessoa = p.Id
            where us.login = @login and us.Senha = @senha";
        #endregion

        #region SELECT EMAIL BY TOKEN
        private const string selectEmailByToken = @"
            SELECT [Email]
            FROM [dbo].[Usuario]
            WHERE TokenResetSenha = @Token";
        #endregion

        #region UPDATE TOKEN BY EMAIL
        private const string UpdateTokenByEmail = @"
          update Usuario set TokenResetSenha = @Token where Login = @Email";
        #endregion

        #region UPDATE TOKEN E SENHA
        private const string UpdateTokenESenha = @"
          update Usuario set TokenResetSenha = null, Senha = @Senha where TokenResetSenha = @Token";
        #endregion
    }
}
