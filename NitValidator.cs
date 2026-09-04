using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace identificadores_de_contribuyentes
{
    public class NitValidator
    {
        public static bool IsNitValid(string nit)
        {
            if (string.IsNullOrEmpty(nit))
            {
                return true; // Asumiendo que una cadena vacía o nula es válida
            }

            var nitRegExp = @"^[0-9]+(-?[0-9kK])?$";

            // Validar formato usando expresión regular
            if (!System.Text.RegularExpressions.Regex.IsMatch(nit, nitRegExp))
            {
                return false;
            }

            // Eliminar guiones
            nit = nit.Replace("-", string.Empty);

            // Obtener la última parte (verificador) y el número
            var lastChar = nit.Length - 1;
            var number = nit.Substring(0, lastChar);
            var expectedChecker = nit.Substring(lastChar, 1).ToLower();

            var factor = number.Length + 1;
            var total = 0;

            // Calcular el total
            for (int i = 0; i < number.Length; i++)
            {
                var character = number.Substring(i, 1);
                var digit = int.Parse(character);

                total += (digit * factor);
                factor -= 1;
            }

            // Calcular el módulo
            var modulus = (11 - (total % 11)) % 11;
            var computedChecker = (modulus == 10 ? "k" : modulus.ToString());

            return expectedChecker == computedChecker;
        }
    }
}