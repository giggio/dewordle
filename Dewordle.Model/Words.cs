using static Dewordle.Model.Color;
using System.Text;
using System.Linq;
using System.Globalization;

namespace Dewordle.Model;
public class Words
{
    private readonly string language;
    private List<string> wordList;
    private readonly List<string> originalWordList;
    private readonly List<string> originalWordListWithDistinctChars;
    private readonly List<Word> wordsAlreadyTried = new();
    private List<string>? wordsWithUniqueLetters;

    public Words(string language, IEnumerable<string> wordList)
    {
        this.language = language; // todo: check most use letters and use that to suggest better words
        this.wordList = wordList.OrderBy(w => w).ToList();
        originalWordList = this.wordList;
        originalWordListWithDistinctChars = originalWordList.Where(w => w.Distinct().Count() == 5).ToList();
    }

    public Suggestion Suggest(Word word)
    {
        var words = (from tentativeWord in wordList
                     where word.Matches(tentativeWord)
                     select tentativeWord).ToList();
        wordsAlreadyTried.Add(word);
        wordList = words;
        var preferred = words.Where(w => w.Distinct().Count() == 5).ToList();
        var others = words.Except(preferred).ToList();
        var allTriedChars = wordsAlreadyTried.SelectMany(w => w.Chars).Distinct().OrderBy(c => c).ToList();
        var unmatched = originalWordListWithDistinctChars.Where(w => !w.Any(c => allTriedChars.Contains(c))).ToList();
        return new(preferred, others, unmatched);
    }

    public string SuggestRandomWord()
    {
        if (wordsWithUniqueLetters == null)
            wordsWithUniqueLetters = wordList.Select(w => w.ToCharArray()).Where(a => a.Distinct().Count() == 5).Select(a => new string(a)).ToList();
        return wordsWithUniqueLetters[Random.Shared.Next(wordsWithUniqueLetters.Count)];
    }
}

public record struct Suggestion(List<string> Preferred, List<string> Others, List<string> unmatched);

public record struct Word(Letter L0, Letter L1, Letter L2, Letter L3, Letter L4)
{
    public static Word FromString(string text)
    {
        // text is letter-color pairs, example, word 'aboar' where the first two letters are green, the 3rd and 5th are yellow, and the 4th does not match (spaces are optional)";
        if (text == null)
            throw new ArgumentNullException(nameof(text));
        if (text.Length == 14)
            text = string.Join("", text.Split(' '));
        if (text.Length != 10)
            throw new WordParseException("Must have 10 characters after removing spaces.");
        return new Word(
            new(text[0], GetColor(text[1])),
            new(text[2], GetColor(text[3])),
            new(text[4], GetColor(text[5])),
            new(text[6], GetColor(text[7])),
            new(text[8], GetColor(text[9])));
    }

    private static Color GetColor(char character) => character switch
    {
        'y' => Yellow,
        'g' => Green,
        'n' => None,
        _ => throw new WordParseException("Only y, g and n are supported for colors."),
    };

    public bool Matches(string word)
    {
        if (word[0] == L0.Character && word[1] == L1.Character && word[2] == L2.Character && word[3] == L3.Character && word[4] == L4.Character) // this already chosen
            return false;
        // word does not match if we do not have letters matching existing greens
        if (((word[0] != L0.Character) && L0.Color == Green)
            || ((word[1] != L1.Character) && L1.Color == Green)
            || ((word[2] != L2.Character) && L2.Color == Green)
            || ((word[3] != L3.Character) && L3.Color == Green)
            || ((word[4] != L4.Character) && L4.Color == Green))
            return false;
        // word does not match if it has a None letter in a non green position
        string wordWithoutGreens = RemoveGreens(word);
        if ((L0.Color == None && wordWithoutGreens.Contains(L0.Character))
            || (L1.Color == None && wordWithoutGreens.Contains(L1.Character))
            || (L2.Color == None && wordWithoutGreens.Contains(L2.Character))
            || (L3.Color == None && wordWithoutGreens.Contains(L3.Character))
            || (L4.Color == None && wordWithoutGreens.Contains(L4.Character)))
            return false;
        // word does not match if we do not have yellow in non green positions or in exact position (should be green, not yellow)
        if ((L0.Color == Yellow && (!wordWithoutGreens.Contains(L0.Character) || word[0] == L0.Character))
            || (L1.Color == Yellow && (!wordWithoutGreens.Contains(L1.Character) || word[1] == L1.Character))
            || (L2.Color == Yellow && (!wordWithoutGreens.Contains(L2.Character) || word[2] == L2.Character))
            || (L3.Color == Yellow && (!wordWithoutGreens.Contains(L3.Character) || word[3] == L3.Character))
            || (L4.Color == Yellow && (!wordWithoutGreens.Contains(L4.Character) || word[4] == L4.Character)))
            return false;
        return true;
    }

    private string RemoveGreens(string word)
    {
        var chars = new List<char>();
        if (L0.Color != Green)
            chars.Add(word[0]);
        if (L1.Color != Green)
            chars.Add(word[1]);
        if (L2.Color != Green)
            chars.Add(word[2]);
        if (L3.Color != Green)
            chars.Add(word[3]);
        if (L4.Color != Green)
            chars.Add(word[4]);
        return new string(chars.ToArray());
    }

    public char[] Chars => new[] { L0.Character, L1.Character, L2.Character, L3.Character, L4.Character };
}

public record Letter
{
    public Letter(char character, Color color = None)
    {
        if (!char.IsLetter(character))
            throw new LetterParseException("Must be a letter.");
        if (IsLetterWithDiacritics(character))
            throw new LetterParseException("Letter cannot have diacritics.");
        Character = character;
        Color = color;
    }
    public char Character { get; init; }
    public Color Color { get; init; }

    private static bool IsLetterWithDiacritics(char c)
    {
        var s = c.ToString().Normalize(NormalizationForm.FormD);
        return (s.Length > 1) &&
               char.IsLetter(s[0]) &&
               s.Skip(1).All(c2 => CharUnicodeInfo.GetUnicodeCategory(c2) == UnicodeCategory.NonSpacingMark);
    }

    public static string RemoveDiacritics(string text) //todo
    {
        string stFormD = text.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();
        for (int ich = 0; ich < stFormD.Length; ich++)
        {
            var uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
            if (uc != UnicodeCategory.NonSpacingMark)
                sb.Append(stFormD[ich]);
        }
        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
}

public enum Color { None, Yellow, Green }

public class WordParseException : ApplicationException
{
    public WordParseException(string message) : base(message) { }
}

public class LetterParseException : ApplicationException
{
    public LetterParseException(string message) : base(message) { }
}
