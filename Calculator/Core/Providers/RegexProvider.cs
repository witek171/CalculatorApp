namespace Calculator.Core.Providers;

public partial class RegexProvider
{
    [System.Text.RegularExpressions.GeneratedRegex(@"^-?\d+([.,]\d+)?[E][+-]?\d+$")]
    public static partial System.Text.RegularExpressions.Regex ScientificNotationRegex();
}