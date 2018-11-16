using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;

namespace Template.Tests.Util
{
    public static class NotificationUtil
    {
        /// <summary>
        /// Verique se a propriedade deu erro
        /// </summary>
        /// <param name="property"></param>
        /// <param name="actionResult"></param>
        /// <returns></returns>
        public static bool ExistError(string property, IActionResult actionResult)
        {
            BadRequestObjectResult badRequst = actionResult as BadRequestObjectResult;

            //Não tem erros
            if (badRequst == null)
                return false;

            var json = JsonConvert.SerializeObject(badRequst.Value);
            var response = JsonConvert.DeserializeObject<ResponseMvc>(json);

            return (response.errors.Any(e => e.Property == property));
        }
    }
}
