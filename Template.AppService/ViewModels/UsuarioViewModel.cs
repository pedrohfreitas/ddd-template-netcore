using System;
using System.Linq;

namespace Template.AppService.ViewModels
{
      public class UsuarioViewModel
    {
        public int Id { get; set; }
        public long IdPessoa { get; set; }
        public long IdPessoaPai { get; set; }
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string FirstName
        {
            get
            {
                string[] nomeArray = Nome.Split(' ');
                if (nomeArray != null && nomeArray.Count() > 0)
                {
                    return nomeArray[0];
                }
                return "";
            }
        }
        public int IdUsuarioStatus { get; set; }
        public DateTime? DataNascimento { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        
        public byte[] Imagem { get; set; }
        public int Idade
        {
            get
            {
                if (DataNascimento != null)
                    return DateTime.Now.Year - DataNascimento.Value.Year;
                else
                    return 0;
            }
        }

        public string Cargo { get; set; }
    }
}
