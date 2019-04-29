using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AssinaturaDigital.Validations
{
    public class TextValidator : IValidator
    {
        public string Message { get; set; } = "Apenas letras são permitidas";

        public bool Check(string value) =>
             !string.IsNullOrEmpty(value) && value.All(c => Char.IsLetter(c) || c == ' ');
    }
}
