using System;
using System.Text.RegularExpressions;

namespace identificadores_de_contribuyentes
{
    public class CUIValidator
    {
        public static bool CuiIsValid(string cui)
        {
            if (string.IsNullOrEmpty(cui))
            {
                Console.WriteLine("CUI vacío");
                return true;
            }

            // Validar la escritura del CUI
            Regex cuiRegExp = new Regex(@"^[0-9]{4}\s?[0-9]{5}\s?[0-9]{4}$");

            if (!cuiRegExp.IsMatch(cui))
            {
                Console.WriteLine("CUI con formato inválido");
                return false;
            }

            // Separar por porciones el CUI para validar sus respectivas partes
            string formattedCui = Regex.Replace(cui, @"\s", "");
            int depto = int.Parse(formattedCui.Substring(9, 2));
            int muni = int.Parse(formattedCui.Substring(11, 2));
            string numero = formattedCui.Substring(0, 8);
            int verificador = int.Parse(formattedCui.Substring(8, 1));

            // Listado de los departamentos con su cantidad de municipios
            int[] munisPorDepto = {
            17,  // Guatemala
            8,   // El Progreso
            16,  // Sacatepéquez
            16,  // Chimaltenango
            13,  // Escuintla
            14,  // Santa Rosa
            19,  // Sololá
            8,   // Totonicapán
            24,  // Quetzaltenango
            21,  // Suchitepéquez
            9,   // Retalhuleu
            30,  // San Marcos
            32,  // Huehuetenango
            21,  // Quiché
            8,   // Baja Verapaz
            17,  // Alta Verapaz
            14,  // Petén
            5,   // Izabal
            11,  // Zacapa
            11,  // Chiquimula
            7,   // Jalapa
            17   // Jutiapa
        };

            // Validar el código de municipio y departamento
            if (depto == 0 || muni == 0)
            {
                Console.WriteLine("CUI con código de municipio o departamento inválido.");
                return false;
            }

            if (depto > munisPorDepto.Length)
            {
                Console.WriteLine("CUI con código de departamento inválido.");
                return false;
            }

            if (muni > munisPorDepto[depto - 1])
            {
                Console.WriteLine("CUI con código de municipio inválido.");
                return false;
            }

            // Verificar el correlativo con base en el algoritmo del complemento 11
            int total = 0;

            for (int i = 0; i < numero.Length; i++)
            {
                total += (numero[i] - '0') * (i + 2);
            }

            int modulo = total % 11;

            Console.WriteLine("CUI con módulo: " + modulo);
            return modulo == verificador;
        }
    }
}