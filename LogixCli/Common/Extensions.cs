using System.Text.RegularExpressions;

namespace LogixCli.Common;

public static class Extensions
{
    public static bool Like(this string text, string pattern)
    {
        // Convert the SQL pattern to a regex pattern
        // '%' -> '.*' (0 or more of any character)
        // '_' -> '.'  (any single character)
        var regex = "^" + Regex.Escape(pattern).Replace("%", ".*").Replace("_", ".") + "$";
        return Regex.IsMatch(text, regex, RegexOptions.IgnoreCase);
    }
}