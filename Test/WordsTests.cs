using static Dewordle.Model.Color;
namespace Test;
public class OneWordsTests
{

    [Fact]
    public void OneWord_OneOption_AllGreen()
    {
        var words = new Words("pt-BR", new[] { "aceno" });
        var suggestion = words.Suggest(new Word(new('a', Green), new('s', None), new('t', None), new('r', None), new('o', Green))); // astro
        suggestion.ShouldBe(new[] { "aceno" }, Case.Sensitive);
    }

    [Fact]
    public void OneWord_TwoOptions_AllGreen()
    {
        var words = new Words("pt-BR", new[] { "aceno", "pinte" });
        var suggestion = words.Suggest(new Word(new('p', Green), new('o', None), new('r', None), new('c', None), new('a', None))); // porca
        suggestion.ShouldBe(new[] { "pinte" }, Case.Sensitive);
    }

    [Fact]
    public void OneWorld_ThreeWords_AllGreen()
    {
        var words = new Words("pt-BR", new[] { "abada", "abade", "abado" });
        var suggestion = words.Suggest(new Word(new('a', Green), new('b', Green), new('a', Green), new('f', None), new('a', Green))); // abafa
        suggestion.ShouldBe(new[] { "abada" }, Case.Sensitive);
    }

    [Fact]
    public void OneWorld_ThreeWords_FirstAlreadyMatched_AllGreen()
    {
        var words = new Words("pt-BR", new[] { "abada", "abade", "abado" });
        var suggestion = words.Suggest(new Word(new('a', Green), new('b', Green), new('a', Green), new('d', Green), new('a', None))); // abada
        suggestion.ShouldBe(new[] { "abade", "abado" }, Case.Sensitive);
    }

    [Fact]
    public void OneWorld_ThreeWords_AllGreens_LastLetterDoesNotMatch()
    {
        var words = new Words("pt-BR", new[] { "abada", "abade", "abado" });
        var suggestion = words.Suggest(new Word(new('a', Green), new('b', Green), new('a', Green), new('f', None), new('a', None))); // abafa
        suggestion.ShouldBe(new[] { "abade", "abado" }, Case.Sensitive);
    }

    [Fact]
    public void OneWorld_ThreeWords_GreensAndYellows()
    {
        var words = new Words("pt-BR", new[] { "abada", "abade", "abado" });
        var suggestion = words.Suggest(new Word(new('a', Green), new('b', Green), new('o', Yellow), new('l', None), new('i', None))); // aboli
        suggestion.ShouldBe(new[] { "abado" }, Case.Sensitive);
    }

    [Fact]
    public void OneWorld_ThreeWords_GreensAndYellows_YellowInGreenPosition()
    {
        var words = new Words("pt-BR", new[] { "abada", "abade", "abado", "aboar" }); // aboar
        var suggestion = words.Suggest(new Word(new('a', Green), new('b', Green), new('a', Yellow), new('f', None), new('o', Yellow))); // abafo
        suggestion.ShouldBe(new[] { "aboar" }, Case.Sensitive);
    }

    [Fact]
    public void ExcludePreviousWrongLetters()
    {
        var words = new Words("pt-BR", new[] { "abada", "abade", "abado", "abate", "abone" }); // abone
        var suggestions1 = words.Suggest(new Word(new('a', Green), new('b', Green), new('d', None), new('a', None), new('l', None))); // abdal
        suggestions1.ShouldBe(new[] { "abone" }, Case.Sensitive);
    }
}

public class TwoWordsTests
{
    [Fact]
    public void TwoWorlds_ThreeWords_Greens()
    {
        var words = new Words("pt-BR", new[] { "abada", "abade", "abado" }); // abada
        var suggestions1 = words.Suggest(new Word(new('a', Green), new('b', Green), new('a', Green), new('f', None), new('o', None))); // abafo
        suggestions1.ShouldBe(new[] { "abada", "abade" }, Case.Sensitive);
        var suggestions2 = words.Suggest(new Word(new('a', Green), new('b', Green), new('a', Green), new('n', None), new('e', None))); // abane
        suggestions2.ShouldBe(new[] { "abada" }, Case.Sensitive);
    }

    //[Fact]
    //public void ExcludePreviousWrongLetters()
    //{
    //    var words = new Words("pt-BR", new[] { "abada", "abade", "abado", "abato", "abone" }); // abone
    //    var suggestions1 = words.Suggest(new Word(new('a', Green), new('b', Green), new('e', Yellow), new('d', None), new('o', Yellow))); // abedo
    //    suggestions1.ShouldBe(new[] { "abato", "abone" }, Case.Sensitive);
    //    var suggestions2 = words.Suggest(new Word(new('a', Green), new('b', Green), new('a', None), new('n', None), new('e', Yellow))); // abane
    //    suggestions2.ShouldBe(new[] { "abone" }, Case.Sensitive);
    //}
}

public class FirstLetterTests
{
    [Fact]
    public void Fist()
    {
        var words = new Words("pt-BR", new[] { "garfo" });
        var word = words.SuggestRandomWord();
        word.ShouldBe("garfo");
    }

    [Fact]
    public void ExcludeLettersWithDoubledLetters()
    {
        var words = new Words("pt-BR", new[] { "abade", "garfo" });
        var word = words.SuggestRandomWord();
        word.ShouldBe("garfo");
    }
}
