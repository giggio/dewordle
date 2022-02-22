using static Dewordle.Model.Color;
namespace Test;
public class OneWordsTests
{

    [Fact]
    public void OneWord_OneOption_AllGreen()
    {
        var words = new Words("pt-BR", new[] { "aceno" });
        var (preferred, others) = words.Suggest(new Word(new('a', Green), new('s', None), new('t', None), new('r', None), new('o', Green))); // astro
        preferred.ShouldBe(new[] { "aceno" }, Case.Sensitive);
    }

    [Fact]
    public void OneWord_TwoOptions_AllGreen()
    {
        var words = new Words("pt-BR", new[] { "aceno", "pinte" });
        var (preferred, others) = words.Suggest(new Word(new('p', Green), new('o', None), new('r', None), new('c', None), new('a', None))); // porca
        preferred.ShouldBe(new[] { "pinte" }, Case.Sensitive);
    }

    [Fact]
    public void OneWorld_ThreeWords_AllGreen()
    {
        var words = new Words("pt-BR", new[] { "abada", "abade", "abado" });
        var (preferred, others) = words.Suggest(new Word(new('a', Green), new('b', Green), new('a', Green), new('f', None), new('a', Green))); // abafa
        preferred.ShouldBeEmpty();
        others.ShouldBe(new[] { "abada" }, Case.Sensitive);
    }

    [Fact]
    public void OneWorld_ThreeWords_FirstAlreadyMatched_AllGreen()
    {
        var words = new Words("pt-BR", new[] { "abada", "abade", "abado" });
        var (preferred, others) = words.Suggest(new Word(new('a', Green), new('b', Green), new('a', Green), new('d', Green), new('a', None))); // abada
        preferred.ShouldBeEmpty();
        others.ShouldBe(new[] { "abade", "abado" }, Case.Sensitive);
    }

    [Fact]
    public void OneWorld_ThreeWords_AllGreens_LastLetterDoesNotMatch()
    {
        var words = new Words("pt-BR", new[] { "abada", "abade", "abado" });
        var (preferred, others) = words.Suggest(new Word(new('a', Green), new('b', Green), new('a', Green), new('f', None), new('a', None))); // abafa
        preferred.ShouldBeEmpty();
        others.ShouldBe(new[] { "abade", "abado" }, Case.Sensitive);
    }

    [Fact]
    public void OneWorld_ThreeWords_GreensAndYellows()
    {
        var words = new Words("pt-BR", new[] { "abada", "abade", "abado" });
        var (preferred, others) = words.Suggest(new Word(new('a', Green), new('b', Green), new('o', Yellow), new('l', None), new('i', None))); // aboli
        preferred.ShouldBeEmpty();
        others.ShouldBe(new[] { "abado" }, Case.Sensitive);
    }

    [Fact]
    public void OneWorld_ThreeWords_GreensAndYellows_YellowInGreenPosition()
    {
        var words = new Words("pt-BR", new[] { "abada", "abade", "abado", "aboar" }); // aboar
        var (preferred, others) = words.Suggest(new Word(new('a', Green), new('b', Green), new('a', Yellow), new('f', None), new('o', Yellow))); // abafo
        preferred.ShouldBeEmpty();
        others.ShouldBe(new[] { "aboar" }, Case.Sensitive);
    }

    [Fact]
    public void ExcludePreviousWrongLetters()
    {
        var words = new Words("pt-BR", new[] { "abada", "abade", "abado", "abate", "abone" }); // abone
        var (preferred, others) = words.Suggest(new Word(new('a', Green), new('b', Green), new('d', None), new('a', None), new('l', None))); // abdal
        preferred.ShouldBe(new[] { "abone" }, Case.Sensitive);
    }

    [Fact]
    public void PreferredLettersMaximizeResult()
    {
        var words = new Words("pt-BR", new[] { "ababa", "ababf", "abijk", "abmno", "abrst" });
        var (preferred, others) = words.Suggest(new Word(new('a', Green), new('b', Green), new('c', None), new('d', None), new('e', None))); // abdal
        preferred.ShouldBe(new[] { "abijk", "abmno", "abrst" }, Case.Sensitive);
        others.ShouldBe(new[] { "ababa", "ababf" }, Case.Sensitive);
    }
}

public class TwoWordsTests
{
    [Fact]
    public void TwoWorlds_ThreeWords_Greens()
    {
        var words = new Words("pt-BR", new[] { "abada", "abade", "abado" }); // abada
        var (preferred1, others1) = words.Suggest(new Word(new('a', Green), new('b', Green), new('a', Green), new('f', None), new('o', None))); // abafo
        preferred1.ShouldBeEmpty();
        others1.ShouldBe(new[] { "abada", "abade" }, Case.Sensitive);
        var (preferred2, others2) = words.Suggest(new Word(new('a', Green), new('b', Green), new('a', Green), new('n', None), new('e', None))); // abane
        preferred2.ShouldBeEmpty();
        others2.ShouldBe(new[] { "abada" }, Case.Sensitive);
    }

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
