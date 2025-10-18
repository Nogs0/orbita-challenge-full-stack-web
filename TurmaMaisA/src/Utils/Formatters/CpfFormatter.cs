using System.Text.RegularExpressions;

namespace TurmaMaisA.Utils.Formatters
{
    public class CpfFormatter
    {
        public static string Format(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
            {
                return string.Empty;
            }

            string numbersOnly = Regex.Replace(cpf, @"\D", "");
            if (numbersOnly.Length != 11)
            {
                return numbersOnly;
            }
            return Convert.ToInt64(numbersOnly).ToString(@"000\.000\.000\-00");
        }
    }
}
