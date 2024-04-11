using System.Text;

namespace Intrinsic.WebApi.ExampleApp.Tests.SUTs;

// Followed article from:
// https://goatreview.com/property-based-testing-fscheck/?utm_source=newsletter.csharpdigest.net&utm_medium=newsletter&utm_campaign=bing-on-net-8-the-impact-of-dynamic-pgo
public class GoatConverter
{
    private static readonly Dictionary<int, string> IntToGoatNumerals = new()
    {
        {1000, "🐐"},
        {900, "Meu🐐"},
        {500, "Baaa"},
        {400, "MeehBaaa"},
        {100, "Meeh"},
        {90, "MehMeeh"},
        {50, "Baa"},
        {40, "MehBaa"},
        {10, "Meh"},
        {9, "MMeh"},
        {5, "Ba"},
        {4, "MBa"},
        {1, "M"}
    };

    // Ww choose to return an empty string for edge cases (0 here)
    public static string Convert(int number)
    {
        if (number != 0)
        {
            var goatNumerals = new StringBuilder();
            var remaining = number;

            foreach (var toGoat in IntToGoatNumerals)
            {
                while (remaining >= toGoat.Key)
                {
                    goatNumerals.Append(toGoat.Value);
                    remaining -= toGoat.Key;
                }
            }

            return goatNumerals.ToString();
        }
        else
        {
            return string.Empty;
        }
    }
}
