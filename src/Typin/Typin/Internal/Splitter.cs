using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Typin.Internal
{
    /*
      Solution                      | Failed Tests
------------------------------|-------------------------------------
Jeffrey L Whitledge (298968)  | 1, 2, 3, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 25, 26, 27, 28, 31, 32, 33, 34, 35, 36, 37, 39, 40, 41, 42, 43, 44, 45
Daniel Earwicker (298990)     | 10, 11, 12, 14, 16, 17, 18, 19, 20, 21, 22, 23, 25, 26, 27, 28, 31, 32, 33, 34, 35, 36, 45
Anton (299795)                | 12, 16, 17, 18, 19, 21, 23, 25, 26, 27, 28, 31, 32, 33, 34, 35, 45
CS. (467313)                  | 12, 18, 19, 21, 27, 31, 32, 33, 34, 35
Vapour in the Alley (2132004) | 10, 11, 12, 14, 16, 17, 20, 21, 22, 23, 25, 26, 27, 28, 29, 31, 32, 33, 34, 35, 36, 45
Monoman (7774211)             | 14, 16, 17, 20, 21, 22, 23, 25, 26, 27, 28, 31, 32, 33, 34, 35, 45
Thomas Petersson (19091999)   | 2, 3, 10, 11, 12, 14, 16, 17, 18, 19, 20, 21, 22, 23, 25, 26, 27, 28, 31, 32, 33, 34, 35, 36, 39, 45
Fabio Iotti (19725880)        | 1, 2, 3, 7, 10, 11, 12, 13, 14, 15, 16, 17, 19, 21, 22, 23, 25, 26, 28, 29, 30, 35, 36, 37, 39, 40, 42, 44, 45
ygoe (23961658)               | 26, 31, 32, 33, 34, 35
Kevin Thach (24829691)        | 10, 11, 12, 14, 18, 19, 20, 21, 22, 23, 26, 27, 31, 32, 33, 34, 35, 36
Lucas De Jesus (31621370)     | 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45
HarryP (48008872)             | 24, 26, 31, 32, 33, 34, 35
TylerY86 (53290784)           | 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 25, 26, 27, 28, 31, 32, 33, 34, 35, 36, 41, 43, 44, 45
Louis Somers (55903304)       | 2, 3, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 25, 26, 27, 28, 29, 31, 32, 33, 34, 35, 36, 39, 41, 43, 44, 45
user2126375 (58233585)        | 5, 6, 15, 16, 17, 31, 32, 33, 34, 35
DilipNannaware (59131568)     | 1, 2, 3, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 25, 26, 27, 28, 31, 32, 33, 34, 35, 36, 37, 39, 40, 41, 42, 43, 44, 45
Mikescher (this)              | -
    */

    class Splitter
    {
        List<int> err = new List<int>();

        void Main()
        {
            var x = Mikescher("^\\\\");

            Test("Jeffrey L Whitledge (298968)", JeffreyLWhitledge);
            Test("Daniel Earwicker (298990)", DanielEarwicker);
            Test("Anton (299795)", Anton);
            Test("CS. (467313)", CS);
            Test("Vapour in the Alley (2132004)", VapourintheAlley);
            Test("Monoman (7774211)", Monoman);
            Test("Thomas Petersson (19091999)", ThomasPetersson);
            Test("Fabio Iotti (19725880)", FabioIotti);
            Test("ygoe (23961658)", ygoe);
            Test("Kevin Thach (24829691)", KevinThach);
            Test("Lucas De Jesus (31621370)", LucasDeJesus);
            Test("HarryP (48008872)", HarryP);
            Test("TylerY86 (53290784)", TylerY86);
            Test("Louis Somers (55903304)", LouisSomers);
            Test("user2126375 (58233585)", user2126375);
            Test("DilipNannaware (59131568)", DilipNannaware);
            Test("Mikescher (this)", Mikescher);
        }

        void Test(string name, Func<string, IEnumerable<string>> m)
        {
            err.Clear();

            Test(0, m, "One", new[] { "One" });
            Test(1, m, "One ", new[] { "One" });
            Test(2, m, " One", new[] { "One" });
            Test(3, m, " One ", new[] { "One" });
            Test(4, m, "One Two", new[] { "One", "Two" });
            Test(5, m, "One  Two", new[] { "One", "Two" });
            Test(6, m, "One   Two", new[] { "One", "Two" });
            Test(7, m, "\"One Two\"", new[] { "One Two" });
            Test(8, m, "One \"Two Three\"", new[] { "One", "Two Three" });
            Test(9, m, "One \"Two Three\" Four", new[] { "One", "Two Three", "Four" });
            Test(10, m, "One=\"Two Three\" Four", new[] { "One=Two Three", "Four" });
            Test(11, m, "One\"Two Three\" Four", new[] { "OneTwo Three", "Four" });
            Test(12, m, "One\"Two Three   Four", new[] { "OneTwo Three   Four" });
            Test(13, m, "\"One Two\"", new[] { "One Two" });
            Test(14, m, "One\" \"Two", new[] { "One Two" });
            Test(15, m, "\"One\"  \"Two\"", new[] { "One", "Two" });
            Test(16, m, "One\\\"  Two", new[] { "One\"", "Two" });
            Test(17, m, "\\\"One\\\"  Two", new[] { "\"One\"", "Two" });
            Test(18, m, "One\"", new[] { "One" });
            Test(19, m, "\"One", new[] { "One" });
            Test(20, m, "One \"\"", new[] { "One", "" });
            Test(21, m, "One \"", new[] { "One", "" });
            Test(22, m, "1 A=\"B C\"=D 2", new[] { "1", "A=B C=D", "2" });
            Test(23, m, "1 A=\"B \\\" C\"=D 2", new[] { "1", "A=B \" C=D", "2" });
            Test(24, m, "1 \\A 2", new[] { "1", "\\A", "2" });
            Test(25, m, "1 \\\" 2", new[] { "1", "\"", "2" });
            Test(26, m, "1 \\\\\" 2", new[] { "1", "\\\"", "2" });
            Test(27, m, "\"", new[] { "" });
            Test(28, m, "\\\"", new[] { "\"" });
            Test(29, m, "'A B'", new[] { "'A", "B'" });
            Test(30, m, "^", new[] { "^" });
            Test(31, m, "^A", new[] { "A" });
            Test(32, m, "^^", new[] { "^" });
            Test(33, m, "\\^^", new[] { "\\^" });
            Test(34, m, "^\\\\", new[] { "\\\\" });
            Test(35, m, "^\"A B\"", new[] { "A B" });

            // Test cases Anton

            Test(36, m, @"/src:""C:\tmp\Some Folder\Sub Folder"" /users:""abcdefg@hijkl.com"" tasks:""SomeTask,Some Other Task"" -someParam foo", new[] { @"/src:C:\tmp\Some Folder\Sub Folder", @"/users:abcdefg@hijkl.com", @"tasks:SomeTask,Some Other Task", @"-someParam", @"foo" });

            // Test cases Daniel Earwicker

            Test(37, m, "", new string[] { });
            Test(38, m, "a", new[] { "a" });
            Test(39, m, " abc ", new[] { "abc" });
            Test(40, m, "a b ", new[] { "a", "b" });
            Test(41, m, "a b \"c d\"", new[] { "a", "b", "c d" });

            // Test cases Fabio Iotti

            Test(42, m, "this is a test ", new[] { "this", "is", "a", "test" });
            Test(43, m, "this \"is a\" test", new[] { "this", "is a", "test" });

            // Test cases Kevin Thach

            Test(44, m, "\"C:\\Program Files\"", new[] { "C:\\Program Files" });
            Test(45, m, "\"He whispered to her \\\"I love you\\\".\"", new[] { "He whispered to her \"I love you\"." });

            System.Console.WriteLine($"{name,-29} | {string.Join(", ", err)}");
        }

        void Test(int id, Func<string, IEnumerable<string>> m, string cmd, string[] r)
        {
            try
            {
                var sr = m(cmd).ToArray();
                if (r.Length != sr.Length)
                {
                    err.Add(id);
                    return;
                }
                for (int i = 0; i < r.Length; i++)
                    if (r[i] != sr[i])
                    {
                        err.Add(id);
                        return;
                    }
            }
            catch (Exception ex)
            {
                err.Add(id);
                return;
            }
        }

        static string[] JeffreyLWhitledge(string commandLine)
        {
            char[] parmChars = commandLine.ToCharArray();
            bool inQuote = false;
            for (int index = 0; index < parmChars.Length; index++)
            {
                if (parmChars[index] == '"')
                    inQuote = !inQuote;
                if (!inQuote && parmChars[index] == ' ')
                    parmChars[index] = '\n';
            }
            return (new string(parmChars)).Split('\n');
        }

        public static IEnumerable<string> DanielEarwicker(string commandLine)
        {
            bool inQuotes = false;

            return DanielEarwicker_Split(commandLine, c =>
            {
                if (c == '\"')
                    inQuotes = !inQuotes;

                return !inQuotes && c == ' ';
            })
                              .Select(arg => DanielEarwicker_TrimMatchingQuotes(arg.Trim(), '\"'))
                              .Where(arg => !string.IsNullOrEmpty(arg));
        }

        public static IEnumerable<string> DanielEarwicker_Split(string str,
                                                Func<char, bool> controller)
        {
            int nextPiece = 0;

            for (int c = 0; c < str.Length; c++)
            {
                if (controller(str[c]))
                {
                    yield return str.Substring(nextPiece, c - nextPiece);
                    nextPiece = c + 1;
                }
            }

            yield return str.Substring(nextPiece);
        }

        public static string DanielEarwicker_TrimMatchingQuotes(string input, char quote)
        {
            if ((input.Length >= 2) &&
                (input[0] == quote) && (input[input.Length - 1] == quote))
                return input.Substring(1, input.Length - 2);

            return input;
        }

        private String[] Anton(String argumentString)
        {
            StringBuilder translatedArguments = new StringBuilder(argumentString);
            bool escaped = false;
            for (int i = 0; i < translatedArguments.Length; i++)
            {
                if (translatedArguments[i] == '"')
                {
                    escaped = !escaped;
                }
                if (translatedArguments[i] == ' ' && !escaped)
                {
                    translatedArguments[i] = '\n';
                }
            }

            string[] toReturn = translatedArguments.ToString().Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < toReturn.Length; i++)
            {
                toReturn[i] = Anton_RemoveMatchingQuotes(toReturn[i]);
            }
            return toReturn;
        }

        public static string Anton_RemoveMatchingQuotes(string stringToTrim)
        {
            int firstQuoteIndex = stringToTrim.IndexOf('"');
            int lastQuoteIndex = stringToTrim.LastIndexOf('"');
            while (firstQuoteIndex != lastQuoteIndex)
            {
                stringToTrim = stringToTrim.Remove(firstQuoteIndex, 1);
                stringToTrim = stringToTrim.Remove(lastQuoteIndex - 1, 1); //-1 because we've shifted the indicies left by one
                firstQuoteIndex = stringToTrim.IndexOf('"');
                lastQuoteIndex = stringToTrim.LastIndexOf('"');
            }
            return stringToTrim;
        }

        public static string[] CS(String argumentString)
        {
            StringBuilder translatedArguments = new StringBuilder(argumentString).Replace("\\\"", "\r");
            bool InsideQuote = false;
            for (int i = 0; i < translatedArguments.Length; i++)
            {
                if (translatedArguments[i] == '"')
                {
                    InsideQuote = !InsideQuote;
                }
                if (translatedArguments[i] == ' ' && !InsideQuote)
                {
                    translatedArguments[i] = '\n';
                }
            }

            string[] toReturn = translatedArguments.ToString().Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < toReturn.Length; i++)
            {
                toReturn[i] = CS_RemoveMatchingQuotes(toReturn[i]);
                toReturn[i] = toReturn[i].Replace("\r", "\"");
            }
            return toReturn;
        }

        public static string CS_RemoveMatchingQuotes(string stringToTrim)
        {
            int firstQuoteIndex = stringToTrim.IndexOf('"');
            int lastQuoteIndex = stringToTrim.LastIndexOf('"');
            while (firstQuoteIndex != lastQuoteIndex)
            {
                stringToTrim = stringToTrim.Remove(firstQuoteIndex, 1);
                stringToTrim = stringToTrim.Remove(lastQuoteIndex - 1, 1); //-1 because we've shifted the indicies left by one
                firstQuoteIndex = stringToTrim.IndexOf('"');
                lastQuoteIndex = stringToTrim.LastIndexOf('"');
            }
            return stringToTrim;
        }

        public static string[] VapourintheAlley(string commandLine)
        {
            var parmChars = commandLine.ToCharArray();
            var inSingleQuote = false;
            var inDoubleQuote = false;
            for (var index = 0; index < parmChars.Length; index++)
            {
                if (parmChars[index] == '"' && !inSingleQuote)
                {
                    inDoubleQuote = !inDoubleQuote;
                    parmChars[index] = '\n';
                }
                if (parmChars[index] == '\'' && !inDoubleQuote)
                {
                    inSingleQuote = !inSingleQuote;
                    parmChars[index] = '\n';
                }
                if (!inSingleQuote && !inDoubleQuote && parmChars[index] == ' ')
                    parmChars[index] = '\n';
            }
            return (new string(parmChars)).Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static IEnumerable<string> Monoman(string commandLine)
        {
            if (string.IsNullOrWhiteSpace(commandLine))
                yield break;

            var sb = new StringBuilder();
            bool inQuote = false;
            foreach (char c in commandLine)
            {
                if (c == '"' && !inQuote)
                {
                    inQuote = true;
                    continue;
                }

                if (c != '"' && !(char.IsWhiteSpace(c) && !inQuote))
                {
                    sb.Append(c);
                    continue;
                }

                if (sb.Length > 0)
                {
                    var result = sb.ToString();
                    sb.Clear();
                    inQuote = false;
                    yield return result;
                }
            }

            if (sb.Length > 0)
                yield return sb.ToString();
        }

        public static IEnumerable<string> ThomasPetersson(string CmdLine)
        {
            var re = @"\G(""((""""|[^""])+)""|(\S+)) *";
            var ms = Regex.Matches(CmdLine, re);
            var list = ms.Cast<Match>()
                         .Select(m => Regex.Replace(
                             m.Groups[2].Success
                                 ? m.Groups[2].Value
                                 : m.Groups[4].Value, @"""""", @"""")).ToArray();
            return list;
        }

        public static string[] FabioIotti(string args)
        {
            char[] parmChars = args.ToCharArray();
            bool inSingleQuote = false;
            bool inDoubleQuote = false;
            bool escaped = false;
            bool lastSplitted = false;
            bool justSplitted = false;
            bool lastQuoted = false;
            bool justQuoted = false;

            int i, j;

            for (i = 0, j = 0; i < parmChars.Length; i++, j++)
            {
                parmChars[j] = parmChars[i];

                if (!escaped)
                {
                    if (parmChars[i] == '^')
                    {
                        escaped = true;
                        j--;
                    }
                    else if (parmChars[i] == '"' && !inSingleQuote)
                    {
                        inDoubleQuote = !inDoubleQuote;
                        parmChars[j] = '\n';
                        justSplitted = true;
                        justQuoted = true;
                    }
                    else if (parmChars[i] == '\'' && !inDoubleQuote)
                    {
                        inSingleQuote = !inSingleQuote;
                        parmChars[j] = '\n';
                        justSplitted = true;
                        justQuoted = true;
                    }
                    else if (!inSingleQuote && !inDoubleQuote && parmChars[i] == ' ')
                    {
                        parmChars[j] = '\n';
                        justSplitted = true;
                    }

                    if (justSplitted && lastSplitted && (!lastQuoted || !justQuoted))
                        j--;

                    lastSplitted = justSplitted;
                    justSplitted = false;

                    lastQuoted = justQuoted;
                    justQuoted = false;
                }
                else
                {
                    escaped = false;
                }
            }

            if (lastQuoted)
                j--;

            return (new string(parmChars, 0, j)).Split(new[] { '\n' });
        }

        public string[] ygoe(string argsString)
        {
            // Collects the split argument strings
            List<string> args = new List<string>();
            // Builds the current argument
            var currentArg = new StringBuilder();
            // Indicates whether the last character was a backslash escape character
            bool escape = false;
            // Indicates whether we're in a quoted range
            bool inQuote = false;
            // Indicates whether there were quotes in the current arguments
            bool hadQuote = false;
            // Remembers the previous character
            char prevCh = '\0';
            // Iterate all characters from the input string
            for (int i = 0; i < argsString.Length; i++)
            {
                char ch = argsString[i];
                if (ch == '\\' && !escape)
                {
                    // Beginning of a backslash-escape sequence
                    escape = true;
                }
                else if (ch == '\\' && escape)
                {
                    // Double backslash, keep one
                    currentArg.Append(ch);
                    escape = false;
                }
                else if (ch == '"' && !escape)
                {
                    // Toggle quoted range
                    inQuote = !inQuote;
                    hadQuote = true;
                    if (inQuote && prevCh == '"')
                    {
                        // Doubled quote within a quoted range is like escaping
                        currentArg.Append(ch);
                    }
                }
                else if (ch == '"' && escape)
                {
                    // Backslash-escaped quote, keep it
                    currentArg.Append(ch);
                    escape = false;
                }
                else if (char.IsWhiteSpace(ch) && !inQuote)
                {
                    if (escape)
                    {
                        // Add pending escape char
                        currentArg.Append('\\');
                        escape = false;
                    }
                    // Accept empty arguments only if they are quoted
                    if (currentArg.Length > 0 || hadQuote)
                    {
                        args.Add(currentArg.ToString());
                    }
                    // Reset for next argument
                    currentArg.Clear();
                    hadQuote = false;
                }
                else
                {
                    if (escape)
                    {
                        // Add pending escape char
                        currentArg.Append('\\');
                        escape = false;
                    }
                    // Copy character from input, no special meaning
                    currentArg.Append(ch);
                }
                prevCh = ch;
            }
            // Save last argument
            if (currentArg.Length > 0 || hadQuote)
            {
                args.Add(currentArg.ToString());
            }
            return args.ToArray();
        }

        public static IEnumerable<string> KevinThach(string commandLine)
        {
            bool inQuotes = false;
            bool isEscaping = false;

            return KevinThach_Split(commandLine, c =>
            {
                if (c == '\\' && !isEscaping) { isEscaping = true; return false; }

                if (c == '\"' && !isEscaping)
                    inQuotes = !inQuotes;

                isEscaping = false;

                return !inQuotes && Char.IsWhiteSpace(c)/*c == ' '*/;
            })
                .Select(arg => KevinThach_TrimMatchingQuotes(arg.Trim(), '\"').Replace("\\\"", "\""))
                .Where(arg => !string.IsNullOrEmpty(arg));
        }

        public static IEnumerable<string> KevinThach_Split(string str,
                                                Func<char, bool> controller)
        {
            int nextPiece = 0;

            for (int c = 0; c < str.Length; c++)
            {
                if (controller(str[c]))
                {
                    yield return str.Substring(nextPiece, c - nextPiece);
                    nextPiece = c + 1;
                }
            }

            yield return str.Substring(nextPiece);
        }

        public static string KevinThach_TrimMatchingQuotes(string input, char quote)
        {
            if ((input.Length >= 2) &&
                (input[0] == quote) && (input[input.Length - 1] == quote))
                return input.Substring(1, input.Length - 2);

            return input;
        }

        string[] LucasDeJesus(string str) => LucasDeJesus_(str, out _);

        string[] LucasDeJesus_(string str, out int argumentos)
        {
            string[] linhaComando = new string[32];
            bool entre_aspas = false;
            int posicao_ponteiro = 0;
            int argc = 0;
            int inicio = 0;
            int fim = 0;
            string sub;

            for (int i = 0; i < str.Length;)
            {
                if (entre_aspas)
                {
                    // Está entre aspas
                    sub = str.Substring(inicio + 1, fim - (inicio + 1));
                    linhaComando[argc - 1] = sub;

                    posicao_ponteiro += ((fim - posicao_ponteiro) + 1);
                    entre_aspas = false;
                    i = posicao_ponteiro;
                }
                else
                {
                tratar_aspas:
                    if (str.ElementAt(i) == '\"')
                    {
                        inicio = i;
                        fim = str.IndexOf('\"', inicio + 1);
                        entre_aspas = true;
                        argc++;
                    }
                    else
                    {
                        // Se não for aspas, então ler até achar o primeiro espaço em branco
                        if (str.ElementAt(i) == ' ')
                        {
                            if (str.ElementAt(i + 1) == '\"')
                            {
                                i++;
                                goto tratar_aspas;
                            }

                            // Pular os espaços em branco adiconais
                            while (str.ElementAt(i) == ' ') i++;

                            argc++;
                            inicio = i;
                            fim = str.IndexOf(' ', inicio);
                            if (fim == -1) fim = str.Length;
                            sub = str.Substring(inicio, fim - inicio);
                            linhaComando[argc - 1] = sub;
                            posicao_ponteiro += (fim - posicao_ponteiro);

                            i = posicao_ponteiro;
                            if (posicao_ponteiro == str.Length) break;
                        }
                        else
                        {
                            argc++;
                            inicio = i;
                            fim = str.IndexOf(' ', inicio);
                            if (fim == -1) fim = str.Length;

                            sub = str.Substring(inicio, fim - inicio);
                            linhaComando[argc - 1] = sub;
                            posicao_ponteiro += fim - posicao_ponteiro;
                            i = posicao_ponteiro;
                            if (posicao_ponteiro == str.Length) break;
                        }
                    }
                }
            }

            argumentos = argc;

            return linhaComando;
        }

        public static IEnumerable<String> HarryP(string commandLine)
        {
            Char quoteChar = '"';
            Char escapeChar = '\\';
            Boolean insideQuote = false;
            Boolean insideEscape = false;

            StringBuilder currentArg = new StringBuilder();

            // needed to keep "" as argument but drop whitespaces between arguments
            Int32 currentArgCharCount = 0;

            for (Int32 i = 0; i < commandLine.Length; i++)
            {
                Char c = commandLine[i];
                if (c == quoteChar)
                {
                    currentArgCharCount++;

                    if (insideEscape)
                    {
                        currentArg.Append(c);       // found \" -> add " to arg
                        insideEscape = false;
                    }
                    else if (insideQuote)
                    {
                        insideQuote = false;        // quote ended
                    }
                    else
                    {
                        insideQuote = true;         // quote started
                    }
                }
                else if (c == escapeChar)
                {
                    currentArgCharCount++;

                    if (insideEscape)   // found \\ -> add \\ (only \" will be ")
                        currentArg.Append(escapeChar + escapeChar);

                    insideEscape = !insideEscape;
                }
                else if (Char.IsWhiteSpace(c))
                {
                    if (insideQuote)
                    {
                        currentArgCharCount++;
                        currentArg.Append(c);       // append whitespace inside quote
                    }
                    else
                    {
                        if (currentArgCharCount > 0)
                            yield return currentArg.ToString();

                        currentArgCharCount = 0;
                        currentArg.Clear();
                    }
                }
                else
                {
                    currentArgCharCount++;
                    if (insideEscape)
                    {
                        // found non-escaping backslash -> add \ (only \" will be ")
                        currentArg.Append(escapeChar);
                        currentArgCharCount = 0;
                        insideEscape = false;
                    }
                    currentArg.Append(c);
                }
            }

            if (currentArgCharCount > 0)
                yield return currentArg.ToString();
        }

        private static readonly Regex RxWinArgs
          = new Regex("([^\\s\"]+\"|((?<=\\s|^)(?!\"\"(?!\"))\")+)(\"\"|.*?)*\"[^\\s\"]*|[^\\s]+",
            RegexOptions.Compiled
            | RegexOptions.Singleline
            | RegexOptions.ExplicitCapture
            | RegexOptions.CultureInvariant);

        internal static IEnumerable<string> TylerY86(string args)
        {
            var match = RxWinArgs.Match(args);

            while (match.Success)
            {
                yield return match.Value;
                match = match.NextMatch();
            }
        }

        public static string[] LouisSomers(string commandLine)
        {
            List<string> args = new List<string>();
            List<char> currentArg = new List<char>();
            char? quoteSection = null; // Keeps track of a quoted section (and the type of quote that was used to open it)
            char[] quoteChars = new[] { '\'', '\"' };
            char previous = ' '; // Used for escaping double quotes

            for (var index = 0; index < commandLine.Length; index++)
            {
                char c = commandLine[index];
                if (quoteChars.Contains(c))
                {
                    if (previous == c) // Escape sequence detected
                    {
                        previous = ' '; // Prevent re-escaping
                        if (!quoteSection.HasValue)
                        {
                            quoteSection = c; // oops, we ended the quoted section prematurely
                            continue;         // don't add the 2nd quote (un-escape)
                        }

                        if (quoteSection.Value == c)
                            quoteSection = null; // appears to be an empty string (not an escape sequence)
                    }
                    else if (quoteSection.HasValue)
                    {
                        if (quoteSection == c)
                            quoteSection = null; // End quoted section
                    }
                    else
                        quoteSection = c; // Start quoted section
                }
                else if (char.IsWhiteSpace(c))
                {
                    if (!quoteSection.HasValue)
                    {
                        args.Add(new string(currentArg.ToArray()));
                        currentArg.Clear();
                        previous = c;
                        continue;
                    }
                }

                currentArg.Add(c);
                previous = c;
            }

            if (currentArg.Count > 0)
                args.Add(new string(currentArg.ToArray()));

            return args.ToArray();
        }

        public static IList<string> user2126375(string commandLineArgsString)
        {
            List<string> args = new List<string>();

            commandLineArgsString = commandLineArgsString.Trim();
            if (commandLineArgsString.Length == 0)
                return args;

            int index = 0;
            while (index != commandLineArgsString.Length)
            {
                args.Add(user2126375_ReadOneArgFromCommandLineArgsString(commandLineArgsString, ref index));
            }

            return args;
        }

        private static string user2126375_ReadOneArgFromCommandLineArgsString(string line, ref int index)
        {
            if (index >= line.Length)
                return string.Empty;

            var sb = new StringBuilder(512);
            int state = 0;
            while (true)
            {
                char c = line[index];
                index++;
                switch (state)
                {
                    case 0: //string outside quotation marks
                        if (c == '\\') //possible escaping character for quotation mark otherwise normal character
                        {
                            state = 1;
                        }
                        else if (c == '"') //opening quotation mark for string between quotation marks
                        {
                            state = 2;
                        }
                        else if (c == ' ') //closing arg
                        {
                            return sb.ToString();
                        }
                        else
                        {
                            sb.Append(c);
                        }

                        break;
                    case 1: //possible escaping \ for quotation mark or normal character
                        if (c == '"') //If escaping quotation mark only quotation mark is added into result
                        {
                            state = 0;
                            sb.Append(c);
                        }
                        else // \ works as not-special character
                        {
                            state = 0;
                            sb.Append('\\');
                            index--;
                        }

                        break;
                    case 2: //string between quotation marks
                        if (c == '"') //quotation mark in string between quotation marks can be escape mark for following quotation mark or can be ending quotation mark for string between quotation marks
                        {
                            state = 3;
                        }
                        else if (c == '\\') //escaping \ for possible following quotation mark otherwise normal character
                        {
                            state = 4;
                        }
                        else //text in quotation marks
                        {
                            sb.Append(c);
                        }

                        break;
                    case 3: //quotation mark in string between quotation marks
                        if (c == '"') //Quotation mark after quotation mark - that means that this one is escaped and can added into result and we will stay in string between quotation marks state
                        {
                            state = 2;
                            sb.Append(c);
                        }
                        else //we had two consecutive quotation marks - this means empty string but the following chars (until space) will be part of same arg result as well
                        {
                            state = 0;
                            index--;
                        }

                        break;
                    case 4: //possible escaping \ for quotation mark or normal character in string between quotation marks
                        if (c == '"') //If escaping quotation mark only quotation mark added into result
                        {
                            state = 2;
                            sb.Append(c);
                        }
                        else
                        {
                            state = 2;
                            sb.Append('\\');
                            index--;
                        }

                        break;
                }

                if (index == line.Length)
                    return sb.ToString();
            }
        }

        static string[] DilipNannaware(string commandLine)
        {
            var isLastCharSpace = false;
            char[] parmChars = commandLine.ToCharArray();
            bool inQuote = false;
            for (int index = 0; index < parmChars.Length; index++)
            {
                if (parmChars[index] == '"')
                    inQuote = !inQuote;
                if (!inQuote && parmChars[index] == ' ' && !isLastCharSpace)
                    parmChars[index] = '\n';

                isLastCharSpace = parmChars[index] == '\n' || parmChars[index] == ' ';
            }

            return (new string(parmChars)).Split('\n');
        }

        public static IEnumerable<string> Mikescher(string commandLine)
        {
            var result = new StringBuilder();

            var quoted = false;
            var escaped = false;
            var started = false;
            var allowcaret = false;
            for (int i = 0; i < commandLine.Length; i++)
            {
                var chr = commandLine[i];

                if (chr == '^' && !quoted)
                {
                    if (allowcaret)
                    {
                        result.Append(chr);
                        started = true;
                        escaped = false;
                        allowcaret = false;
                    }
                    else if (i + 1 < commandLine.Length && commandLine[i + 1] == '^')
                    {
                        allowcaret = true;
                    }
                    else if (i + 1 == commandLine.Length)
                    {
                        result.Append(chr);
                        started = true;
                        escaped = false;
                    }
                }
                else if (escaped)
                {
                    result.Append(chr);
                    started = true;
                    escaped = false;
                }
                else if (chr == '"')
                {
                    quoted = !quoted;
                    started = true;
                }
                else if (chr == '\\' && i + 1 < commandLine.Length && commandLine[i + 1] == '"')
                {
                    escaped = true;
                }
                else if (chr == ' ' && !quoted)
                {
                    if (started) yield return result.ToString();
                    result.Clear();
                    started = false;
                }
                else
                {
                    result.Append(chr);
                    started = true;
                }
            }

            if (started) yield return result.ToString();
        }

    }
}
