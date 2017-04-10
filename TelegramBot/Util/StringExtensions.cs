using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Util
{
    internal static class StringExtensions
    {
        public static string StringJoin(this IEnumerable<string> items, string separator)
        {
            return string.Join(separator, items);
        }

        public static string StartWithUppercase(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            if (value.Length == 1) return value.ToUpper();
            return value[0].ToString().ToUpper() + value.Substring(1);
        }
    }
}
