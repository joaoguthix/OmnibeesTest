using System.Globalization;
using System.Text;

namespace Domain.Utils
{
    public static class StringExtensions
    {
        public static string NormalizeToCompare(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return string.Empty;

            var normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var ch in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                    sb.Append(char.ToUpperInvariant(ch));
            }

            return sb.ToString();
        }
    }
}
