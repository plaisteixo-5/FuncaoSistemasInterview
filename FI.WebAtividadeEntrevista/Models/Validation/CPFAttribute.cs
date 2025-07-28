using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FI.WebAtividadeEntrevista.Models.Validation
{
    class CPFAttribute : ValidationAttribute
    {
        private static readonly Regex _digitsOnly = new Regex(@"^\d{11}$");

        public CPFAttribute()
        {
            ErrorMessage = "CPF inválido.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult(ErrorMessage); // [Required] notation will not allow this to happen, but, just to be shure.
            }

            var digits = new string(value.ToString()
                .Where(char.IsDigit)
                .ToArray());

            if(!_digitsOnly.IsMatch(digits) || !CheckDigits(digits))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    
        private static bool CheckDigits(string cpf)
        {
            int Mod11(int sum) => (sum * 10) % 11 % 10;
            int Sum(int len) => Enumerable.Range(0, len)
                .Sum(i => (cpf[i] - '0') * (len + 1 - i));

            return Mod11(Sum(9)) == cpf[9] - '0'
                && Mod11(Sum(10)) == cpf[10] - '0';
        }
    }
}