using System.Globalization;

namespace SeniorAPI.Validacao
{
    public class DataNascimentoValidacao
    {
        public bool EhDataNascimentoValida(string data, out string mensagemErro)
        {
            mensagemErro = string.Empty;

            bool isValid = DateTime.TryParseExact(data, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime dataConvertida);

            if (!isValid)
            {
                mensagemErro = "A data deve estar no formato dd/MM/yyyy.";
                return false;
            }

            if (dataConvertida > DateTime.Now)
            {
                mensagemErro = "A data de nascimento não pode ser no futuro.";
                return false;
            }

            return true;
        }
    }
}