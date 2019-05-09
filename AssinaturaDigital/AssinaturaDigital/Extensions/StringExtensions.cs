using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AssinaturaDigital.Extensions
{
    public static class StringExtensions
    {
        public static string Clean(this string cpf)
            => cpf
                .Trim()
                .Replace(".", "")
                .Replace("-", "")
                .Replace("(", "")
                .Replace(")", "");

        public static bool IsValidName(this string name)
            => !string.IsNullOrEmpty(name) && name.All(c => char.IsLetter(c) || c == ' ');

        public static bool IsValidCPF(this string cpf)
        {
            if (string.IsNullOrEmpty(cpf))
                return false;

            var multiplier1 = new int[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplier2 = new int[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            cpf = cpf.Clean();

            if (cpf.Length != 11)
                return false;

            var tempCpf = cpf.Substring(0, 9);
            var sum = 0;

            for (var i = 0; i < 9; i++)
                sum += int.Parse(tempCpf[i].ToString()) * multiplier1[i];

            var rest = sum % 11;

            if (rest < 2)
                rest = 0;
            else
                rest = 11 - rest;

            var digit = rest.ToString();
            tempCpf += digit;
            sum = 0;

            for (var i = 0; i < 10; i++)
                sum += int.Parse(tempCpf[i].ToString()) * multiplier2[i];

            rest = sum % 11;

            if (rest < 2)
                rest = 0;
            else
                rest = 11 - rest;

            digit += rest.ToString();

            return cpf.EndsWith(digit, StringComparison.Ordinal);
        }

        public static bool IsValidCellPhoneNumber(this string cellPhoneNumber)
            => !string.IsNullOrEmpty(cellPhoneNumber)
                && new Regex(@"^\([1-9]{2}\) (?:[2-8]|9[1-9])[0-9]{3}\-[0-9]{4}$")
                    .IsMatch(cellPhoneNumber);

        public static bool IsValidEmail(this string email)
            => !string.IsNullOrEmpty(email)
                && new Regex(@"^.+@[^\.].*\.[a-z]{2,}$")
                    .IsMatch(email.Trim());
    }
}
