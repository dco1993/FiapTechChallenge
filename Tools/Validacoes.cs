using System.Text.RegularExpressions;
using Domain.Entity;

namespace Tools
{
    public class Validacoes
    {
        public static string ValidaContato(Contato contato)
        {
            string erros = "";

            if ((contato.Email is null) || (!ValidaEmail(contato.Email)))
                erros += "E-mail fornecido está inválido. ";

            if ((contato.NomeCompleto is null) || (!ValidaNome(contato.NomeCompleto)))
                erros += "Nome inválido, de 5 a 100 caracteres são necessários. ";

            if (!(contato.TelefoneNum is null))
            {
                if (!ValidaTelefoneTamanho(contato.TelefoneNum))
                    erros += "Número de telefone inválido, 8 ou 9 números são necessários. ";

                if ((contato.TelefoneNum.Trim().Length > 0) && (!ValidaTelefoneApenasNumeros(contato.TelefoneNum)))
                    erros += "Número de telefone inválido, apenas números são permitidos. ";
            }
            else
                erros += "O telefone deve ser preenchido. ";

            return erros;
        }

        public static bool ValidaEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            return regex.IsMatch(email);
        }

        public static bool ValidaNome(string nome) => nome.Trim().Length > 4 && nome.Trim().Length <= 100;

        public static bool ValidaTelefoneTamanho(string telefone) => telefone.Trim().Length == 8 || telefone.Trim().Length == 9;

        public static bool ValidaTelefoneApenasNumeros(string telefone) => int.TryParse(telefone.Trim(), out int result);

    }
}
