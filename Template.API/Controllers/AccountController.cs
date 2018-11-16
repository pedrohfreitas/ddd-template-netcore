using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Template.API.Commands.Authenticate;
using Template.API.Security;
using Template.AppService.Interfaces;
using Template.AppService.ViewModels;
using Template.CrossCutting.FluentValidator;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Template.API.Controllers
{
    [EnableCors("Cors")]
    public class AccountController : BaseController
    {
        private UsuarioViewModel _usuario { get; set; }
        private readonly TokenOptions _tokenOptions;
        private readonly IAuthenticateAppService _authenticateAppService;

        public AccountController(IOptions<TokenOptions> jwtOptions, IAuthenticateAppService authenticateAppService)
        {
            _tokenOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_tokenOptions);
            _authenticateAppService = authenticateAppService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/authenticate")]
        public async Task<IActionResult> Post([FromForm] AuthenticateUserCommand command)
        {
            try
            {
                if (command == null)
                    return await Response(null, new List<Notification> { new Notification("User", "Usuário ou senha inválidos") });

                var contract = new AuthenticateUserCommandContract(command);

                if (contract.Contract.Invalid)
                {
                    return await Response(command, contract.Contract.Notifications);
                }

                var identity = await GetClaims(command);
                if (identity == null)
                    return await Response(null, new List<Notification> { new Notification("User", "Usuário ou senha inválidos") });

                var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, command.CPF),
                new Claim(JwtRegisteredClaimNames.NameId, command.CPF),
                new Claim(JwtRegisteredClaimNames.Email, command.CPF),
                new Claim(JwtRegisteredClaimNames.Sub, command.CPF),
                new Claim(JwtRegisteredClaimNames.Jti, await _tokenOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_tokenOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                identity.FindFirst("Nome"),
                identity.FindFirst("IdPessoaPai"),
                identity.FindFirst("IdPessoa")
            };

                //Adiciona um ou mais perfis
                foreach (var item in identity.FindAll("Perfil"))
                {
                    claims.Add(item);
                }

                var jwt = new JwtSecurityToken(
                    issuer: _tokenOptions.Issuer,
                    audience: _tokenOptions.Audience,
                    claims: claims.AsEnumerable(),
                    notBefore: _tokenOptions.NotBefore,
                    expires: _tokenOptions.Expiration,
                    signingCredentials: _tokenOptions.SigningCredentials);

                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                var response = new
                {
                    token = encodedJwt,
                    expires = (int)_tokenOptions.ValidFor.TotalSeconds,
                    user = new
                    {
                        id = _usuario.Id,
                        name = _usuario.Nome,
                        idPessoaPai = _usuario.IdPessoaPai,
                        idPessoa = _usuario.IdPessoa,
                        firstName = _usuario.FirstName,
                        imagem = _usuario.Imagem
                    }
                };

                var json = JsonConvert.SerializeObject(response);
                return new OkObjectResult(json);
            }
            catch (Exception ex)
            {
                return await Response(null, new List<Notification> { new Notification("User", "Usuário ou senha inválidos") });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/forgetpassword")]
        public async Task<IActionResult> ForgetPassword([FromBody]ForgetPasswordCommand command)
        {
            try
            {
                if (command == null || string.IsNullOrEmpty(command.Email))
                    return await Response(null, new List<Notification> { new Notification("Email", "Emmail inválido") });


                var result = _authenticateAppService.RecuperarSenha(command.Email);

                if (result)
                    return await Response(result, null);
                else
                    return await Response(null, new List<Notification> { new Notification("Email", "Ocorreu um erro ao recuperar a senha") });

            }
            catch (Exception ex)
            {
                return await Response(null, new List<Notification> { new Notification("ResetSenha", ex.Message) });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("api/emailbytoken")]
        public async Task<IActionResult> EmailByToken(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                    return await Response(null, new List<Notification> { new Notification("Token", "Token inválido") });


                var result = _authenticateAppService.Email(token);

                return await Response(result, null);

            }
            catch (Exception ex)
            {
                return await Response(null, new List<Notification> { new Notification("ResetSenha", ex.Message) });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordCommand command)
        {
            try
            {
                if (command == null)
                    return await Response(null, new List<Notification> { new Notification("Reset", "Reset Inválido") });

                var contract = new AuthenticateUserCommandContract(command);

                if (contract.Contract.Invalid)
                {
                    return await Response(command, contract.Contract.Notifications);
                }

                var result = _authenticateAppService.ResetarSenha(command);

                return await Response(result, null);

            }
            catch (Exception ex)
            {
                return await Response(null, new List<Notification> { new Notification("ResetSenha", ex.Message) });
            }
        }

        private static void ThrowIfInvalidOptions(TokenOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
                throw new ArgumentException("O período deve ser maior que zero", nameof(TokenOptions.ValidFor));

            if (options.SigningCredentials == null)
                throw new ArgumentNullException(nameof(TokenOptions.SigningCredentials));

            if (options.JtiGenerator == null)
                throw new ArgumentNullException(nameof(TokenOptions.JtiGenerator));
        }

        private static long ToUnixEpochDate(DateTime date) => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        private Task<ClaimsIdentity> GetClaims(AuthenticateUserCommand command)
        {
            //Authenticar o usuário
            UsuarioViewModel usuarioViewModel = _authenticateAppService.Authenticate(command.CPF, command.Senha);

            if (usuarioViewModel == null || usuarioViewModel.Id <= 0)
                return Task.FromResult<ClaimsIdentity>(null);

            //switch (command.AplicacaoAcesso)
            //{
            //case AplicacaoAcesso.Manager:
            //    if (usuarioViewModel.Perfis.Any(p => p == Perfil.ClienteGestor) || usuarioViewModel.Perfis.Any(p => p == Perfil.Recurso))
            //        return Task.FromResult<ClaimsIdentity>(null);
            //    break;
            //case AplicacaoAcesso.App:
            //    if (usuarioViewModel.Perfis.FirstOrDefault(p => p == Perfil.ClienteGestor) == 0)
            //        return Task.FromResult<ClaimsIdentity>(null);
            //    break;
            //case AplicacaoAcesso.Ponto:
            //    if (usuarioViewModel.Perfis.FirstOrDefault(p => p == Perfil.Recurso) == 0)
            //        return Task.FromResult<ClaimsIdentity>(null);
            //    break;
            //}

            _usuario = usuarioViewModel;

            List<Claim> claims = new List<Claim>();

            //foreach (var item in _usuario.Perfis)
            //{
            //    claims.Add(new Claim("Perfil", item.ToString()));
            //}

            claims.Add(new Claim("Nome", _usuario.Login));
            claims.Add(new Claim("IdPessoa", _usuario.IdPessoa.ToString()));
            claims.Add(new Claim("IdPessoaPai", _usuario.IdPessoaPai.ToString()));


            return Task.FromResult(new ClaimsIdentity(new GenericIdentity(_usuario.Login, "Token"), claims.ToArray()));
        }
    }
}
