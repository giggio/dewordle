using static Dewordle.Model.Color;
namespace Test;

public class WordTests
{
    [Fact]
    public void ConstructFromString()
    {
        var word = Word.FromString("agbgogagrg");
        word.ShouldBe(new Word(new('a', Green), new('b', Green), new('o', Green), new('a', Green), new('r', Green)));
    }

    [Fact]
    public void MatchesGreenAndYellowAndNoneWithSpaces()
    {
        var word = Word.FromString("ag bg oy ay rn");
        word.ShouldBe(new Word(new('a', Green), new('b', Green), new('o', Yellow), new('a', Yellow), new('r', None)));
    }

    [Fact] public void IncorrectColor() => Assert.Throws<WordParseException>(() => Word.FromString("ax bg og ag rg"));

    [Fact] public void IncorrectNumberOfSpaces() => Assert.Throws<WordParseException>(() => Word.FromString("ax bgogag rg"));

    [Fact] public void IncorrectLetterNumber() => Assert.Throws<LetterParseException>(() => Word.FromString("1g bg og ag rg"));

    [Fact] public void IncorrectLetterSymbol() => Assert.Throws<LetterParseException>(() => Word.FromString("%g bg og ag rg"));

    [Fact] public void IncorrectLetterAccented() => Assert.Throws<LetterParseException>(() => Word.FromString("ég bg og ag rg"));

    [Fact] public void LetterDefault() => Assert.Throws<LetterParseException>(() => new Letter(default, None));
}

public class WordMatchTests
{
    [Fact] public void TwoGreensThreNonesMatches() => Word.FromString("agbgoglnin").Matches("abone").ShouldBeTrue(); // aboli
    [Fact] public void GreensYellowsAndNoneMatches() => Word.FromString("agbgayfnoy").Matches("aboar").ShouldBeTrue(); // abafo
    [Fact] public void GreensYellowsAndNoneDoesNotMatch() => Word.FromString("agbgayfnoy").Matches("abada").ShouldBeFalse(); // abafo
    [Fact] public void ExcludePreviousWrongLetters1() => Word.FromString("agbgdnanln").Matches("abada").ShouldBeFalse(); // abdal
    [Fact] public void ExcludePreviousWrongLetters2() => Word.FromString("agbgdnanln").Matches("abade").ShouldBeFalse(); // abdal
    [Fact] public void ExcludePreviousWrongLetters3() => Word.FromString("agbgdnanln").Matches("abado").ShouldBeFalse(); // abdal
    [Fact] public void ExcludePreviousWrongLetters4() => Word.FromString("agbgdnanln").Matches("abate").ShouldBeFalse(); // abdal
    [Fact] public void ExcludePreviousWrongLettersMatches() => Word.FromString("agbgdnanln").Matches("abone").ShouldBeTrue(); // abdal

    [Fact] public void ExcludeX() => Word.FromString("agbgendnay").Matches("abafo").ShouldBeTrue(); // abeda
    [Fact] public void ExcludeY() => Word.FromString("agbgaydnen").Matches("abono").ShouldBeFalse(); // abade

    //[Fact]
    //public void ExcludePreviousWrongLetters()
    //{
    //    var words = new Words("pt-BR", new[] { "abada", "abade", "abado", "abate", "abone" }); // abone
    //    var suggestions1 = words.Suggest(new Word(new('a', Green), new('b', Green), new('d', None), new('a', None), new('l', None))); // abdal
    //    suggestions1.ShouldBe(new[] { "abone" }, Case.Sensitive);
    //}
}