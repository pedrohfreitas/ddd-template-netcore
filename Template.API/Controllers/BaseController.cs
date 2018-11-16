using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Template.CrossCutting.FluentValidator;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Template.API.Controllers
{
    public class BaseController : Controller
    {
        public async Task<IActionResult> Response(object result, IReadOnlyCollection<Notification> notifications)
        {
            if (notifications == null || !notifications.Any())
            {
                try
                {
                    return Ok(new
                    {
                        success = true,
                        data = result
                    });
                }
                catch
                {
                    // Logar o erro (Elmah)
                    return BadRequest(new
                    {
                        success = false,
                        errors = new[] { "Ocorreu uma falha interna no servidor." }
                    });
                }
            }
            else
            {
                return BadRequest(new
                {
                    success = false,
                    errors = notifications
                });
            }
        }

        public long GetIdPessoaPai()
        {
            var claims = (ClaimsIdentity)User.Identity;
            var pessoa = claims.Claims.Where(p => p.Type == "IdPessoaPai").FirstOrDefault();
            return pessoa != null ? long.Parse(pessoa.Value) : 0;
        }

        public long GetIdPessoaLogada()
        {
            var claims = (ClaimsIdentity)User.Identity;
            var pessoa = claims.Claims.Where(p => p.Type == "IdPessoa").FirstOrDefault();
            return pessoa != null ? long.Parse(pessoa.Value) : 0;
        }
    }
}

