namespace SeniorAPI.Validacao
{
    public class CPFValidacao
    {
        public bool EhCPFValido(string cpf, out string mensagemErro)
        {
            mensagemErro = string.Empty;

            string cpfFormatado = cpf.Replace(".", "").Replace("-", "");

            if (cpfFormatado.Length != 11)
            {
                mensagemErro = "O CPF deve conter exatamente 11 números.";
                return false;
            }

            if (cpfFormatado.All(c => c == cpfFormatado[0]))
            {
                mensagemErro = "O CPF não pode ter todos os números iguais.";
                return false;
            }

            int[] cpfArray = [.. cpfFormatado.Select(c => int.Parse(c.ToString()))];

            int primeiroDigito = CalcularDigito([.. cpfArray.Take(9)], [10, 9, 8, 7, 6, 5, 4, 3, 2]);
            if (cpfArray[9] != primeiroDigito)
            {
                mensagemErro = "O CPF informado é inválido.";
                return false;
            }

            int segundoDigito = CalcularDigito([.. cpfArray.Take(10)], [11, 10, 9, 8, 7, 6, 5, 4, 3, 2]);
            if (cpfArray[10] != segundoDigito)
            {
                mensagemErro = "O CPF informado é inválido.";
                return false;
            }

            return true;
        }

        private static int CalcularDigito(int[] cpfArray, int[] pesos)
        {
            int soma = 0;
            for (int i = 0; i < cpfArray.Length; i++)
            {
                soma += cpfArray[i] * pesos[i];
            }

            int resto = soma % 11;
            return resto < 2 ? 0 : 11 - resto;
        }
    }
}