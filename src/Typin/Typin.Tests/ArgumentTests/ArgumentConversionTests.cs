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
        [InlineData(@"cmd --obj value", @"{ ""obj"": ""value"" }")]
        [InlineData(@"cmd --str value", @"{ ""str"": ""value"" }")]
        [InlineData(@"cmd --str", @"{ ""str"": null }")]
        [InlineData(@"cmd --str --bool", @"{ ""str"": null, ""bool"": true }")]

        //Simple types
        [InlineData(@"cmd --bool true", @"{ ""bool"": true }")]
        [InlineData(@"cmd --bool", @"{ ""bool"": true }")]
        [InlineData(@"cmd --bool false", @"{ ""bool"": false }")]

        [InlineData(@"cmd --char a", @"{ ""char"": ""a"" }")]
        [InlineData(@"cmd --char 0", @"{ ""char"": ""0"" }")]
        [InlineData(@"cmd --char", @"{ ""char"": ""\u0000"" }")]
        [InlineData(@"cmd --char \0", @"{ ""char"": ""\u0000"" }")]
        [InlineData(@"cmd --char \a", @"{ ""char"": ""\u0007"" }")]
        [InlineData(@"cmd --char \b", @"{ ""char"": ""\b"" }")]
        [InlineData(@"cmd --char \f", @"{ ""char"": ""\f"" }")]
        [InlineData(@"cmd --char \n", @"{ ""char"": ""\n"" }")]
        [InlineData(@"cmd --char \r", @"{ ""char"": ""\r"" }")]
        [InlineData(@"cmd --char \t", @"{ ""char"": ""\t"" }")]
        [InlineData(@"cmd --char \v", @"{ ""char"": ""\u000b"" }")]
        [InlineData(@"cmd --char \\", @"{ ""char"": ""\\"" }")]

        [InlineData(@"cmd --byte 15", @"{ ""byte"": 15 }")]
        [InlineData(@"cmd --sbyte 15", @"{ ""sbyte"": 15 }")]
        [InlineData(@"cmd --sbyte -15", @"{ ""sbyte"": -15 }")]

        [InlineData(@"cmd --short -15", @"{ ""short"": -15 }")]
        [InlineData(@"cmd --short 15", @"{ ""short"": 15 }")]
        [InlineData(@"cmd --ushort 15", @"{ ""ushort"": 15 }")]

        [InlineData(@"cmd --int -15", @"{ ""int"": -15 }")]
        [InlineData(@"cmd --int 15", @"{ ""int"": 15 }")]
        [InlineData(@"cmd --uint 15", @"{ ""uint"": 15 }")]

        [InlineData(@"cmd --long -15", @"{ ""long"": -15 }")]
        [InlineData(@"cmd --long 15", @"{ ""long"": 15 }")]
        [InlineData(@"cmd --ulong 15", @"{ ""ulong"": 15 }")]

        //[InlineData(@"cmd --half -15.123", @"{ ""half"": -15.123 }")]
        //[InlineData(@"cmd --half 15.123", @"{ ""half"": 15.123 }")]

        [InlineData(@"cmd --float -15.123", @"{ ""float"": -15.123 }")]
        [InlineData(@"cmd --float 15.123", @"{ ""float"": 15.123 }")]

        [InlineData(@"cmd --double -15.123", @"{ ""double"": -15.123 }")]
        [InlineData(@"cmd --double 15.123", @"{ ""double"": 15.123 }")]

        [InlineData(@"cmd --decimal -15.123", @"{ ""decimal"": -15.123 }")]
        [InlineData(@"cmd --decimal 15.123", @"{ ""decimal"": 15.123 }")]

        [InlineData(@"cmd --datetime ""28 Apr 1995""", @"{ ""datetime"": ""1995-04-28T00:00:00"" }")]
        [InlineData(@"cmd --datetime 2020-11-12", @"{ ""datetime"": ""2020-11-12T00:00:00"" }")]
        [InlineData(@"cmd --datetime ""2020-11-12 12:00:56""", @"{ ""datetime"": ""2020-11-12T12:00:56"" }")]

        [InlineData(@"cmd --datetime-offset ""28 Apr 1995""", @"{ ""datetime-offset"": ""1995-04-28T00:00:00"" }")]
        [InlineData(@"cmd --datetime-offset 2020-11-12", @"{ ""datetime-offset"": ""2020-11-12T00:00:00"" }")]

        [InlineData(@"cmd --timespan 00:14:59", @"{ ""timespan"": ""00:14:59"" }")]
        [InlineData(@"cmd --timespan 40:17:00", @"{ ""timespan"": ""40:17:00"" }")]

        //Simple nullable types
        [InlineData(@"cmd --bool-nullable true", @"{ ""bool-nullable"": true }")]
        [InlineData(@"cmd --bool-nullable", @"{ ""bool-nullable"": null }")]
        [InlineData(@"cmd --bool-nullable false", @"{ ""bool-nullable"": false }")]

        [InlineData(@"cmd --char-nullable a", @"{ ""char-nullable"": ""a"" }")]
        [InlineData(@"cmd --char-nullable", @"{ ""char-nullable"": null }")]
        [InlineData(@"cmd --char-nullable 0", @"{ ""char-nullable"": ""0"" }")]
        [InlineData(@"cmd --char-nullable \0", @"{ ""char-nullable"": ""\u0000"" }")]
        [InlineData(@"cmd --char-nullable \a", @"{ ""char-nullable"": ""\u0007"" }")]
        [InlineData(@"cmd --char-nullable \b", @"{ ""char-nullable"": ""\b"" }")]
        [InlineData(@"cmd --char-nullable \f", @"{ ""char-nullable"": ""\f"" }")]
        [InlineData(@"cmd --char-nullable \n", @"{ ""char-nullable"": ""\n"" }")]
        [InlineData(@"cmd --char-nullable \r", @"{ ""char-nullable"": ""\r"" }")]
        [InlineData(@"cmd --char-nullable \t", @"{ ""char-nullable"": ""\t"" }")]
        [InlineData(@"cmd --char-nullable \v", @"{ ""char-nullable"": ""\u000b"" }")]
        [InlineData(@"cmd --char-nullable \\", @"{ ""char-nullable"": ""\\"" }")]

        [InlineData(@"cmd --byte-nullable 15", @"{ ""byte-nullable"": 15 }")]
        [InlineData(@"cmd --byte-nullable", @"{ ""byte-nullable"": null }")]
        [InlineData(@"cmd --sbyte-nullable 15", @"{ ""sbyte-nullable"": 15 }")]
        [InlineData(@"cmd --sbyte-nullable -15", @"{ ""sbyte-nullable"": -15 }")]
        [InlineData(@"cmd --sbyte-nullable", @"{ ""sbyte-nullable"": null }")]

        [InlineData(@"cmd --short-nullable -15", @"{ ""short-nullable"": -15 }")]
        [InlineData(@"cmd --short-nullable 15", @"{ ""short-nullable"": 15 }")]
        [InlineData(@"cmd --short-nullable", @"{ ""short-nullable"": null }")]
        [InlineData(@"cmd --ushort-nullable 15", @"{ ""ushort-nullable"": 15 }")]
        [InlineData(@"cmd --ushort-nullable", @"{ ""ushort-nullable"": null }")]

        [InlineData(@"cmd --int-nullable -15", @"{ ""int-nullable"": -15 }")]
        [InlineData(@"cmd --int-nullable 15", @"{ ""int-nullable"": 15 }")]
        [InlineData(@"cmd --int-nullable", @"{ ""int-nullable"": null }")]
        [InlineData(@"cmd --uint-nullable 15", @"{ ""uint-nullable"": 15 }")]
        [InlineData(@"cmd --uint-nullable", @"{ ""uint-nullable"": null }")]

        [InlineData(@"cmd --long-nullable -15", @"{ ""long-nullable"": -15 }")]
        [InlineData(@"cmd --long-nullable 15", @"{ ""long-nullable"": 15 }")]
        [InlineData(@"cmd --long-nullable", @"{ ""long-nullable"": null }")]
        [InlineData(@"cmd --ulong-nullable 15", @"{ ""ulong-nullable"": 15 }")]
        [InlineData(@"cmd --ulong-nullable", @"{ ""ulong-nullable"": null }")]

        //[InlineData(@"cmd --half-nullable -15.123", @"{ ""half"": -15.123 }")]
        //[InlineData(@"cmd --half-nullable 15.123", @"{ ""half"": 15.123 }")]

        [InlineData(@"cmd --float-nullable -15.123", @"{ ""float-nullable"": -15.123 }")]
        [InlineData(@"cmd --float-nullable 15.123", @"{ ""float-nullable"": 15.123 }")]
        [InlineData(@"cmd --float-nullable", @"{ ""float-nullable"": null }")]

        [InlineData(@"cmd --double-nullable -15.123", @"{ ""double-nullable"": -15.123 }")]
        [InlineData(@"cmd --double-nullable 15.123", @"{ ""double-nullable"": 15.123 }")]
        [InlineData(@"cmd --double-nullable", @"{ ""double-nullable"": null }")]

        [InlineData(@"cmd --decimal-nullable -15.123", @"{ ""decimal-nullable"": -15.123 }")]
        [InlineData(@"cmd --decimal-nullable 15.123", @"{ ""decimal-nullable"": 15.123 }")]
        [InlineData(@"cmd --decimal-nullable", @"{ ""decimal-nullable"": null }")]

        [InlineData(@"cmd --datetime-nullable ""28 Apr 1995""", @"{ ""datetime-nullable"": ""1995-04-28T00:00:00"" }")]
        [InlineData(@"cmd --datetime-nullable 2020-11-12", @"{ ""datetime-nullable"": ""2020-11-12T00:00:00"" }")]
        [InlineData(@"cmd --datetime-nullable ""2020-11-12 12:00:56""", @"{ ""datetime-nullable"": ""2020-11-12T12:00:56"" }")]
        [InlineData(@"cmd --datetime-nullable", @"{ ""datetime-nullable"": null }")]

        [InlineData(@"cmd --datetime-offset-nullable ""28 Apr 1995""", @"{ ""datetime-offset-nullable"": ""1995-04-28T00:00:00"" }")]
        [InlineData(@"cmd --datetime-offset-nullable 2020-11-12", @"{ ""datetime-offset-nullable"": ""2020-11-12T00:00:00"" }")]
        [InlineData(@"cmd --datetime-offset-nullable", @"{ ""datetime-offset-nullable"": null }")]

        [InlineData(@"cmd --timespan-nullable 00:14:59", @"{ ""timespan-nullable"": ""00:14:59"" }")]
        [InlineData(@"cmd --timespan-nullable 40:17:00", @"{ ""timespan-nullable"": ""40:17:00"" }")]
        [InlineData(@"cmd --timespan-nullable", @"{ ""timespan-nullable"": null }")]

        //Custom enum
        [InlineData(@"cmd --enum value2", @"{ ""enum"": ""value2"" }")]
        [InlineData(@"cmd --enum 2", @"{ ""enum"": ""value2"" }")]
        [InlineData(@"cmd --enum-nullable value2", @"{ ""enum-nullable"": ""value2"" }")]
        [InlineData(@"cmd --enum-nullable 2", @"{ ""enum-nullable"": ""value2"" }")]
        [InlineData(@"cmd --enum-nullable", @"{ ""enum-nullable"": null }")]
        [InlineData(@"cmd --enum-nullable --enum 2", @"{ ""enum-nullable"": null, ""enum"": ""value2"" }")]
        [InlineData(@"cmd --enum-array 1 2", @"{ ""enum-array"": [""value1"", ""value2""] }")]
        [InlineData(@"cmd --enum-array value1 2", @"{ ""enum-array"": [""value1"", ""value2""] }")]
        [InlineData(@"cmd --enum-array value1 value3", @"{ ""enum-array"": [""value1"", ""value3""] }")]

        //Parasable or constructible
        [InlineData(@"cmd --str-parsable foobar", @"{ ""str-parsable"": { ""Value"": ""foobar""} }")]
        [InlineData(@"cmd --str-parsable-format foobar", @"{ ""str-parsable-format"": { ""Value"": ""foobar CultureInfo""} }")]
        [InlineData(@"cmd --str-constructible foobar", @"{ ""str-constructible"": { ""Value"": ""foobar""} }")]
        [InlineData(@"cmd --str-constructible-array foo bar ""foo bar""", @"{ ""str-constructible-array"": [ { ""Value"": ""foo""}, { ""Value"": ""bar""}, { ""Value"": ""foo bar""}] }")]

        //Collections
        [InlineData(@"cmd --obj-array foo bar", @"{ ""obj-array"": [""foo"", ""bar""] }")]
        [InlineData(@"cmd --obj-array foo "" """, @"{ ""obj-array"": [""foo"", "" ""] }")]
        [InlineData(@"cmd --obj-array", @"{ ""obj-array"": [] }")]

        [InlineData(@"cmd --str-array foo bar", @"{ ""str-array"": [""foo"", ""bar""] }")]
        [InlineData(@"cmd --str-array foo "" """, @"{ ""str-array"": [""foo"", "" ""] }")]
        [InlineData(@"cmd --str-array", @"{ ""str-array"": [] }")]

        [InlineData(@"cmd --str-enumerable foo bar", @"{ ""str-enumerable"": [""foo"", ""bar""] }")]
        [InlineData(@"cmd --str-enumerable foo "" """, @"{ ""str-enumerable"": [""foo"", "" ""] }")]
        [InlineData(@"cmd --str-enumerable", @"{ ""str-enumerable"": [] }")]

        [InlineData(@"cmd --str-read-only-list foo bar", @"{ ""str-read-only-list"": [""foo"", ""bar""] }")]
        [InlineData(@"cmd --str-read-only-list foo "" """, @"{ ""str-read-only-list"": [""foo"", "" ""] }")]
        [InlineData(@"cmd --str-read-only-list", @"{ ""str-read-only-list"": [] }")]

        [InlineData(@"cmd --str-list foo bar", @"{ ""str-list"": [""foo"", ""bar""] }")]
        [InlineData(@"cmd --str-list foo "" """, @"{ ""str-list"": [""foo"", "" ""] }")]
        [InlineData(@"cmd --str-list", @"{ ""str-list"": [] }")]

        [InlineData(@"cmd --str-set foo bar", @"{ ""str-set"": [""foo"", ""bar""] }")]
        [InlineData(@"cmd --str-set foo "" """, @"{ ""str-set"": [""foo"", "" ""] }")]
        [InlineData(@"cmd --str-set", @"{ ""str-set"": [] }")]

        [InlineData(@"cmd --int-array 1 -1 0 -0", @"{ ""int-array"": [1, -1, 0, 0] }")]
        [InlineData(@"cmd --int-array", @"{ ""int-array"": [] }")]

        [InlineData(@"cmd --int-nullable-array 1 -1 0 -0", @"{ ""int-nullable-array"": [1, -1, 0, 0] }")]
        [InlineData(@"cmd --int-nullable-array 1 -1 """" -0", @"{ ""int-nullable-array"": [1, -1, null, 0] }")]
        [InlineData(@"cmd --int-nullable-array", @"{ ""int-nullable-array"": [] }")]
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
            var (exitCode, stdOut, _) = await builder.BuildAndRunTestAsync(_output, new[]
            {
                "cmd", "--str-parsable-format", "foobar"
            });

            var commandInstance = stdOut.GetString().DeserializeJson<SupportedArgumentTypesCommand>();

            // Assert
            exitCode.Should().Be(ExitCodes.Success);

            commandInstance.Should().BeEquivalentTo(new SupportedArgumentTypesCommand
            {
                StringParsableWithFormatProvider = CustomStringParsableWithFormatProvider.Parse("foobar", CultureInfo.InvariantCulture)
            });
        }

        [Theory]
        [InlineData(@"cmd --char \x")]
        [InlineData(@"cmd --char \\n")]
        [InlineData(@"cmd --char ~\")]
        [InlineData(@"cmd --char \\\")]
        [InlineData(@"cmd --char 00Z")]
        [InlineData(@"cmd --char \u000Z")]
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