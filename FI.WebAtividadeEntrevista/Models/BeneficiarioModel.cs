using FI.WebAtividadeEntrevista.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace ebAtividadeEntrevista.Models
{
    public class BeneficiarioModel
    {
        /// <summary>
        /// CPF
        /// </summary>
        [Required]
        [CPFValidate(ErrorMessage = "Digite um CPF válido")]
        public string CPF { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        [Required]
        public string Nome { get; set; }
    }
}