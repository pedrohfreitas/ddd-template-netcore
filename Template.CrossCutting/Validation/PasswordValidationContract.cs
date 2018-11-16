using System;
using System.Collections.Generic;
using System.Text;

namespace Template.CrossCutting.FluentValidator.Validation
{
    public partial class ValidationContract
    {
        /// <summary>
        /// Caso a senha estiver preenchida, é obrigatório inserir a nova Senha e ela deve estar igual a senha
        /// </summary>
        /// <param name="password"></param>
        /// <param name="newPassowrd"></param>
        /// <param name="property"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public ValidationContract ConfirmPassword(string password, string newPassowrd, string property, string message)
        {
            if (!string.IsNullOrEmpty(password))
            {
                if (!password.Equals(newPassowrd))
                {
                    AddNotification(property, message);
                }
            }

            return this;
        }
    }
}
