using FsCheck.Xunit;
using FsCheck;
using System.Text.RegularExpressions;
using Intrinsic.WebApi.ExampleApp.Tests.SUTs;

namespace Intrinsic.WebApi.ExampleApp.Tests;

public partial class GoatConverterTests
{
    // We use a Regex to check the result is valid
    [GeneratedRegex("^(?:M|Ba|Meh|Baa|Meeh|Baaa|Meu|🐐)+$")]
    private static partial Regex ValidGoatRegex();

    private static bool AllGoatCharactersAreValid(string goatNumber)
        => ValidGoatRegex().IsMatch(goatNumber);

    // We define a new Property using this FsCheck Attribute
    [Property]
    public void ReturnsOnlyValidSymbolsForValidNumbers() =>
        // for all(validNumbers) such as n in [1; 3999] holds
        Prop.ForAll(ValidNumbers,
                // We call the Convert method for each n value and return true when the string value is valid regarding the Regex
                n => AllGoatCharactersAreValid(GoatConverter.Convert(n)))
            // Glue for failing the tests through xUnit
            .QuickCheckThrowOnFailure();

    // We define how our machine can generate valid numbers by using Arb and Gen classes (from FsCheck)
    private static readonly Arbitrary<int> ValidNumbers = Gen.Choose(1, 3999).ToArbitrary();

}