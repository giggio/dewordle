string? wordsFile = null;
if (args.Length > 0)
{
    wordsFile = args[0];
}
else
{
    var wordsFiles = new[] { "word.txt", "ptBR-all.txt" };
    var dirs = new[] { Environment.CurrentDirectory, Path.Combine(Environment.CurrentDirectory, ".."), Path.Combine(Environment.CurrentDirectory, "..", "words"), Path.Combine(Environment.CurrentDirectory, "words") };
    wordsFile = dirs.SelectMany(dir => wordsFiles.Select(f => Path.GetFullPath(Path.Combine(dir, f)))).Where(f => File.Exists(f)).OrderBy(f => f).FirstOrDefault();
}
if (wordsFile is null)
{
    Error.WriteLine("Could not find words file.");
    return 1;
}
WriteLine($"Reading words from {wordsFile}...");
var wordList = File.ReadAllLines(wordsFile);
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
return 0;
