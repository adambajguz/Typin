namespace Typin.Tests.ArgumentTests
{
    using System.Globalization;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Typin.Tests.Data.Commands.Valid;
    using Typin.Tests.Data.CustomTypes.Initializable;
    using Typin.Tests.Extensions;
    using Xunit;
    using Xunit.Abstractions;

    public class ArgumentConversionTests
    {
        private readonly ITestOutputHelper _output;

        public ArgumentConversionTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --obj value", @"{ ""obj"": ""value"" }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --str value", @"{ ""str"": ""value"" }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --str", @"{ ""str"": null }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --str --bool", @"{ ""str"": null, ""bool"": true }")]

        //Simple types
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --bool true", @"{ ""bool"": true }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --bool", @"{ ""bool"": true }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --bool false", @"{ ""bool"": false }")]

        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --char a", @"{ ""char"": ""a"" }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --char 0", @"{ ""char"": ""0"" }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --char", @"{ ""char"": ""\u0000"" }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --char \0", @"{ ""char"": ""\u0000"" }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --char \a", @"{ ""char"": ""\u0007"" }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --char \b", @"{ ""char"": ""\b"" }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --char \f", @"{ ""char"": ""\f"" }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --char \n", @"{ ""char"": ""\n"" }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --char \r", @"{ ""char"": ""\r"" }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --char \t", @"{ ""char"": ""\t"" }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --char \v", @"{ ""char"": ""\u000b"" }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --char \\", @"{ ""char"": ""\\"" }")]

        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --byte 15", @"{ ""byte"": 15 }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --sbyte 15", @"{ ""sbyte"": 15 }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --sbyte -15", @"{ ""sbyte"": -15 }")]

        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --short -15", @"{ ""short"": -15 }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --short 15", @"{ ""short"": 15 }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --ushort 15", @"{ ""ushort"": 15 }")]

        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --int -15", @"{ ""int"": -15 }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --int 15", @"{ ""int"": 15 }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --uint 15", @"{ ""uint"": 15 }")]

        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --long -15", @"{ ""long"": -15 }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --long 15", @"{ ""long"": 15 }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --ulong 15", @"{ ""ulong"": 15 }")]

        //[InlineData(nameof(SupportedArgumentTypesCommand) + @" --half -15.123", @"{ ""half"": -15.123 }")]
        //[InlineData(nameof(SupportedArgumentTypesCommand) + @" --half 15.123", @"{ ""half"": 15.123 }")]

        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --float -15.123", @"{ ""float"": -15.123 }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --float 15.123", @"{ ""float"": 15.123 }")]

        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --double -15.123", @"{ ""double"": -15.123 }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --double 15.123", @"{ ""double"": 15.123 }")]

        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --decimal -15.123", @"{ ""decimal"": -15.123 }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --decimal 15.123", @"{ ""decimal"": 15.123 }")]

        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --guid ""{47BDE31E-6F2D-44E6-B585-A0037ADCE901}""", @"{ ""guid"": ""{47bde31e-6f2d-44e6-b585-a0037adce901}"" }")]

        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --datetime ""28 Apr 1995""", @"{ ""datetime"": ""1995-04-28T00:00:00"" }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --datetime 2020-11-12", @"{ ""datetime"": ""2020-11-12T00:00:00"" }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --datetime ""2020-11-12 12:00:56""", @"{ ""datetime"": ""2020-11-12T12:00:56"" }")]

        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --datetime-offset ""28 Apr 1995""", @"{ ""datetime-offset"": ""1995-04-28T00:00:00"" }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --datetime-offset 2020-11-12", @"{ ""datetime-offset"": ""2020-11-12T00:00:00"" }")]

        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --timespan 00:14:59", @"{ ""timespan"": ""00:14:59"" }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --timespan 40:17:00", @"{ ""timespan"": ""40:17:00"" }")]

        //Simple nullable types
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --bool-nullable true", @"{ ""bool-nullable"": true }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --bool-nullable", @"{ ""bool-nullable"": null }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --bool-nullable false", @"{ ""bool-nullable"": false }")]

        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --char-nullable a", @"{ ""char-nullable"": ""a"" }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --char-nullable", @"{ ""char-nullable"": null }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --char-nullable 0", @"{ ""char-nullable"": ""0"" }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --char-nullable \0", @"{ ""char-nullable"": ""\u0000"" }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --char-nullable \a", @"{ ""char-nullable"": ""\u0007"" }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --char-nullable \b", @"{ ""char-nullable"": ""\b"" }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --char-nullable \f", @"{ ""char-nullable"": ""\f"" }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --char-nullable \n", @"{ ""char-nullable"": ""\n"" }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --char-nullable \r", @"{ ""char-nullable"": ""\r"" }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --char-nullable \t", @"{ ""char-nullable"": ""\t"" }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --char-nullable \v", @"{ ""char-nullable"": ""\u000b"" }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --char-nullable \\", @"{ ""char-nullable"": ""\\"" }")]

        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --byte-nullable 15", @"{ ""byte-nullable"": 15 }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --byte-nullable", @"{ ""byte-nullable"": null }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --sbyte-nullable 15", @"{ ""sbyte-nullable"": 15 }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --sbyte-nullable -15", @"{ ""sbyte-nullable"": -15 }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --sbyte-nullable", @"{ ""sbyte-nullable"": null }")]

        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --short-nullable -15", @"{ ""short-nullable"": -15 }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --short-nullable 15", @"{ ""short-nullable"": 15 }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --short-nullable", @"{ ""short-nullable"": null }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --ushort-nullable 15", @"{ ""ushort-nullable"": 15 }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --ushort-nullable", @"{ ""ushort-nullable"": null }")]

        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --int-nullable -15", @"{ ""int-nullable"": -15 }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --int-nullable 15", @"{ ""int-nullable"": 15 }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --int-nullable", @"{ ""int-nullable"": null }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --uint-nullable 15", @"{ ""uint-nullable"": 15 }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --uint-nullable", @"{ ""uint-nullable"": null }")]

        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --long-nullable -15", @"{ ""long-nullable"": -15 }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --long-nullable 15", @"{ ""long-nullable"": 15 }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --long-nullable", @"{ ""long-nullable"": null }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --ulong-nullable 15", @"{ ""ulong-nullable"": 15 }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --ulong-nullable", @"{ ""ulong-nullable"": null }")]

        //[InlineData(nameof(SupportedArgumentTypesCommand) + @" --half-nullable -15.123", @"{ ""half"": -15.123 }")]
        //[InlineData(nameof(SupportedArgumentTypesCommand) + @" --half-nullable 15.123", @"{ ""half"": 15.123 }")]

        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --float-nullable -15.123", @"{ ""float-nullable"": -15.123 }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --float-nullable 15.123", @"{ ""float-nullable"": 15.123 }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --float-nullable", @"{ ""float-nullable"": null }")]

        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --double-nullable -15.123", @"{ ""double-nullable"": -15.123 }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --double-nullable 15.123", @"{ ""double-nullable"": 15.123 }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --double-nullable", @"{ ""double-nullable"": null }")]

        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --decimal-nullable -15.123", @"{ ""decimal-nullable"": -15.123 }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --decimal-nullable 15.123", @"{ ""decimal-nullable"": 15.123 }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --decimal-nullable", @"{ ""decimal-nullable"": null }")]

        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --guid-nullable ""{47BDE31E-6F2D-44E6-B585-A0037ADCE901}""", @"{ ""guid-nullable"": ""{47bde31e-6f2d-44e6-b585-a0037adce901}"" }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --guid-nullable", @"{ ""guid-nullable"": null }")]

        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --datetime-nullable ""28 Apr 1995""", @"{ ""datetime-nullable"": ""1995-04-28T00:00:00"" }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --datetime-nullable 2020-11-12", @"{ ""datetime-nullable"": ""2020-11-12T00:00:00"" }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --datetime-nullable ""2020-11-12 12:00:56""", @"{ ""datetime-nullable"": ""2020-11-12T12:00:56"" }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --datetime-nullable", @"{ ""datetime-nullable"": null }")]

        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --datetime-offset-nullable ""28 Apr 1995""", @"{ ""datetime-offset-nullable"": ""1995-04-28T00:00:00"" }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --datetime-offset-nullable 2020-11-12", @"{ ""datetime-offset-nullable"": ""2020-11-12T00:00:00"" }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --datetime-offset-nullable", @"{ ""datetime-offset-nullable"": null }")]

        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --timespan-nullable 00:14:59", @"{ ""timespan-nullable"": ""00:14:59"" }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --timespan-nullable 40:17:00", @"{ ""timespan-nullable"": ""40:17:00"" }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --timespan-nullable", @"{ ""timespan-nullable"": null }")]

        //Custom enum
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --enum value2", @"{ ""enum"": ""value2"" }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --enum 2", @"{ ""enum"": ""value2"" }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --enum-nullable value2", @"{ ""enum-nullable"": ""value2"" }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --enum-nullable 2", @"{ ""enum-nullable"": ""value2"" }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --enum-nullable", @"{ ""enum-nullable"": null }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --enum-nullable --enum 2", @"{ ""enum-nullable"": null, ""enum"": ""value2"" }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --enum-array 1 2", @"{ ""enum-array"": [""value1"", ""value2""] }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --enum-array value1 2", @"{ ""enum-array"": [""value1"", ""value2""] }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --enum-array value1 value3", @"{ ""enum-array"": [""value1"", ""value3""] }")]

        //Parasable or constructible
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --str-parsable foobar", @"{ ""str-parsable"": { ""Value"": ""foobar""} }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --str-parsable-format foobar", @"{ ""str-parsable-format"": { ""Value"": ""foobar CultureInfo""} }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --str-constructible foobar", @"{ ""str-constructible"": { ""Value"": ""foobar""} }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --str-constructible-array foo bar ""foo bar""", @"{ ""str-constructible-array"": [ { ""Value"": ""foo""}, { ""Value"": ""bar""}, { ""Value"": ""foo bar""}] }")]

        //Collections
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --obj-array foo bar", @"{ ""obj-array"": [""foo"", ""bar""] }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --obj-array foo "" """, @"{ ""obj-array"": [""foo"", "" ""] }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --obj-array", @"{ ""obj-array"": [] }")]

        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --str-array foo bar", @"{ ""str-array"": [""foo"", ""bar""] }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --str-array foo "" """, @"{ ""str-array"": [""foo"", "" ""] }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --str-array", @"{ ""str-array"": [] }")]

        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --str-enumerable foo bar", @"{ ""str-enumerable"": [""foo"", ""bar""] }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --str-enumerable foo "" """, @"{ ""str-enumerable"": [""foo"", "" ""] }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --str-enumerable", @"{ ""str-enumerable"": [] }")]

        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --str-read-only-list foo bar", @"{ ""str-read-only-list"": [""foo"", ""bar""] }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --str-read-only-list foo "" """, @"{ ""str-read-only-list"": [""foo"", "" ""] }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --str-read-only-list", @"{ ""str-read-only-list"": [] }")]

        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --str-list foo bar", @"{ ""str-list"": [""foo"", ""bar""] }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --str-list foo "" """, @"{ ""str-list"": [""foo"", "" ""] }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --str-list", @"{ ""str-list"": [] }")]

        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --str-set foo bar", @"{ ""str-set"": [""foo"", ""bar""] }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --str-set foo "" """, @"{ ""str-set"": [""foo"", "" ""] }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --str-set", @"{ ""str-set"": [] }")]

        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --int-array 1 -1 0 -0", @"{ ""int-array"": [1, -1, 0, 0] }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --int-array", @"{ ""int-array"": [] }")]

        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --int-nullable-array 1 -1 0 -0", @"{ ""int-nullable-array"": [1, -1, 0, 0] }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --int-nullable-array 1 -1 """" -0", @"{ ""int-nullable-array"": [1, -1, null, 0] }")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --int-nullable-array", @"{ ""int-nullable-array"": [] }")]
        public async Task Property_should_be_bound(string args, string output)
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<SupportedArgumentTypesCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, args);

            var commandInstance = stdOut.GetString().DeserializeJson<SupportedArgumentTypesCommand>();
            var testInstance = output.DeserializeJson<SupportedArgumentTypesCommand>();

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
            stdErr.GetString().Should().BeNullOrWhiteSpace();
            commandInstance.Should().BeEquivalentTo(testInstance);
        }

        [Fact]
        public async Task Property_of_a_type_that_has_a_static_Parse_method_accepting_a_string_and_format_provider_is_bound_by_invoking_the_method()
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<SupportedArgumentTypesCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                nameof(SupportedArgumentTypesCommand), "--str-parsable-format", "foobar"
            });

            var commandInstance = stdOut.GetString().DeserializeJson<SupportedArgumentTypesCommand>();

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
            stdErr.GetString().Should().BeNullOrWhiteSpace();
            commandInstance.Should().BeEquivalentTo(new SupportedArgumentTypesCommand(null!)
            {
                StringParsableWithFormatProvider = CustomStringParsableWithFormatProvider.Parse("foobar", CultureInfo.InvariantCulture)
            });
        }

        [Theory]
        [InlineData("24.12", "89.9")]
        [InlineData(" 24.12 ", " 89.9 ")]
        [InlineData("-24.12", "-89.9")]
        [InlineData(" -24.12 ", "-89.9")]
        [InlineData("-24.12", "")]
        [InlineData("-24.12", " ")]
        [InlineData("0", "0")]
        [InlineData(" 0 ", "0 ")]
        [InlineData("0", " ")]
        [InlineData("24.", "89.")]
        public async Task Property_of_a_type_half_and_nullable_half_should_be_converted(string half, string nullableHalf)
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<HalfCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "half", "--half", half, "--half-nullable", nullableHalf
            });

            // Assert
            exitCode.Should().Be(ExitCodes.Success);
            stdOut.GetString().Should().ContainAll($"Value:{half.Trim().Trim('.')}", $"NullableValue:{(string.IsNullOrWhiteSpace(nullableHalf) ? "null" : nullableHalf.Trim().Trim('.'))}");
            stdErr.GetString().Should().BeNullOrWhiteSpace();
        }

        [Theory]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --char \x")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --char \\n")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --char ~\")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --char \\\")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --char 00Z")]
        [InlineData(nameof(SupportedArgumentTypesCommand) + @" --char \u000Z")]
        public async Task Should_not_parse_unknown_char_escape_sequence(string args)
        {
            // Arrange
            var builder = new CliApplicationBuilder()
                .AddCommand<SupportedArgumentTypesCommand>();

            // Act
            var (exitCode, stdOut, stdErr) = await builder.BuildAndRunTestAsync(_output, args);

            // Assert
            exitCode.Should().NotBe(ExitCodes.Success);
            stdOut.GetString().Should().BeNullOrWhiteSpace();
            stdErr.GetString().Should().NotBeNullOrWhiteSpace();
        }
    }
}