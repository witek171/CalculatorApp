namespace Calculator.Core.Providers;

public partial class RegexProvider
{
    [System.Text.RegularExpressions.GeneratedRegex(@"^0,?0*$")]
    public static partial System.Text.RegularExpressions.Regex ZeroRegex();

    [System.Text.RegularExpressions.GeneratedRegex(@"^-?\d+([.,]\d+)?[e][+-]?\d+$")]
    public static partial System.Text.RegularExpressions.Regex ScientificNotationRegex();
}