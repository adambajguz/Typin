namespace TypinExamples.CalculatOR.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Threading.Tasks;
    using Typin.Console;
    using TypinExamples.CalculatOR.Domain;
    using TypinExamples.CalculatOR.Extensions;

    public class OperationEvaluatorService
    {
        private readonly IConsole _console;

        public OperationEvaluatorService(IConsole console)
        {
            _console = console;
        }

        public async Task<BigInteger> Eval(Number a,
                                           IEnumerable<Number> b,
                                           string operationSymbol,
                                           bool printSteps,
                                           NumberBase? @base,
                                           Func<BigInteger, BigInteger, BigInteger> operation)
        {
            operationSymbol = string.Concat(" ", operationSymbol.Trim());

            BigInteger result = a.Value;
            int pad = printSteps ? CalculateMaxDigitsLength(a, b, @base) : 0;

            if (printSteps)
                await _console.Output.WriteLineAsync($"{new string(' ', operationSymbol.Length)} {result.ToString(@base ?? a.Base).PadLeft(pad)}");

            foreach (Number x in b)
            {
                result = operation.Invoke(result, x.Value);

                if (printSteps)
                {
                    _console.WithForegroundColor(ConsoleColor.DarkCyan, () => _console.Output.Write(operationSymbol));
                    await _console.Output.WriteAsync(' ');

                    PrintColored(x.Value, @base ?? x.Base, pad);

                    _console.WithForegroundColor(ConsoleColor.DarkGray, () => _console.Output.Write($" (= "));
                    PrintColored(result, @base ?? x.Base, 0, ConsoleColor.DarkGray);
                    _console.WithForegroundColor(ConsoleColor.DarkGray, () => _console.Output.WriteLine($")"));
                }
            }

            if (printSteps)
            {
                _console.WithForegroundColor(ConsoleColor.DarkCyan, () => _console.Output.Write("=".PadLeft(operationSymbol.Length)));
                await _console.Output.WriteAsync(' ');

                PrintColored(result, @base ?? a.Base, pad, ConsoleColor.Green);
            }
            else
                _console.WithForegroundColor(ConsoleColor.Green, () => _console.Output.WriteLine(result.ToString(@base ?? a.Base)));

            await _console.Output.WriteLineAsync();

            return result;
        }

        private static int CalculateMaxDigitsLength(Number a, IEnumerable<Number> b, NumberBase? @base)
        {
            int bMax = b.Select(x => CalculateDigitsLength(x.Value, @base ?? x.Base)).Max();

            return Math.Max(CalculateDigitsLength(a.Value, @base ?? a.Base), bMax);
        }

        private static int CalculateDigitsLength(BigInteger value, NumberBase @base)
        {
            if (@base == NumberBase.DEC && value > 0)
                return (int)Math.Floor(BigInteger.Log10(value) + 1);

            return value.ToString(@base).Length;
        }

        private void PrintColored(BigInteger value, NumberBase @base, int pad = 0, ConsoleColor valueColor = ConsoleColor.Gray)
        {
            _console.WithForegroundColor(valueColor, () => _console.Output.Write(value.ToString(@base).PadLeft(pad)));
        }
    }
}
