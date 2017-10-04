using System;
using System.Text.RegularExpressions;

namespace FileManager.Extensions
{
    public static class StringExtensions
    {
        public static bool Contains(this string source, string toSearch, StringComparison comp)
        {
            return source.IndexOf(toSearch, comp) >=0;
        }

        public static string ReplaceInsensitive(this string str, string old, string _new)
        {
            str = Regex.Replace(str, old, _new, RegexOptions.IgnoreCase);
            return str;
        }
    }
}
