var wordsFile = args[0];
var wordsCsvText = File.ReadAllLines(wordsFile)[0];
// read csv from text file
var wordList = wordsCsvText.Split(',').Select(w => w.Trim()[1..^1]).ToList();
var words = new Words("pt-BR", wordList);
WriteLine($"Your first word could be: '{words.SuggestRandomWord()}'.");
const string instructions = "Inform results with letter-color pairs, example, word 'aboar' where the first two letters are green, the 3rd and 5th are yellow, and the 4th does not match (spaces are optional):";
WriteLine(instructions);
WriteLine("ag bg oy ay rn"); // correct would be abafo
WriteLine();
while (true)
{
    WriteLine("Inform result:");
    var result = ReadLine();
    if (result == null)
    {
        WriteLine("Try again, or write 'exit' to exit.");
        continue;
    }
    if (result == "exit")
        break;
    List<string>? suggestedWords;
    try
    {
        suggestedWords = words.Suggest(Word.FromString(result));
    }
    catch (Exception ex) when (ex is WordParseException || ex is LetterParseException)
    {
        Console.WriteLine(ex.Message);
        WriteLine("Please follow the suggested format:");
        WriteLine(instructions);
        continue;
    }
    if (suggestedWords.Any())
    {
        WriteLine("Suggested words are:");
        for (int i = 0; i < suggestedWords.Count; i++)
        {
            var word = suggestedWords[i];
            Write(word);
            if (i != suggestedWords.Count - 1)
                Write(", ");
        }
        WriteLine();
        if (suggestedWords.Count == 1)
            break;
    }
    else
    {
        WriteLine("Could not find a word.");
        break;
    }
}
WriteLine("Done!");
