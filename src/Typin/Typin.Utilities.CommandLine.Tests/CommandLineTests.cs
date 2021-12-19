namespace Typin.Utilities.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using Newtonsoft.Json;
    using Xunit;
    using Xunit.Abstractions;

    public class CommandLineTests
    {
        private readonly ITestOutputHelper _output;

        public CommandLineTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [InlineData("One", new[] { "One" })]
        [InlineData("One ", new[] { "One" })]
        [InlineData(" One", new[] { "One" })]
        [InlineData(" One ", new[] { "One" })]
        [InlineData("One Two", new[] { "One", "Two" })]
        [InlineData("One  Two", new[] { "One", "Two" })]
        [InlineData("One   Two", new[] { "One", "Two" })]
        [InlineData("One \"Two Three\"", new[] { "One", "Two Three" })]
        [InlineData("One \"Two Three\" Four", new[] { "One", "Two Three", "Four" })]
        [InlineData("One=\"Two Three\" Four", new[] { "One=Two Three", "Four" })]
        [InlineData("One\"Two Three\" Four", new[] { "OneTwo Three", "Four" })]
        [InlineData("One\"Two Three   Four", new[] { "OneTwo Three   Four" })]
        [InlineData("\"One Two\"", new[] { "One Two" })]
        [InlineData("One\" \"Two", new[] { "One Two" })]
        [InlineData("\"One\"  \"Two\"", new[] { "One", "Two" })]
        [InlineData("One\\\"  Two", new[] { "One\"", "Two" })]
        [InlineData("\\\"One\\\"  Two", new[] { "\"One\"", "Two" })]
        [InlineData("One\"", new[] { "One" })]
        [InlineData("\"One", new[] { "One" })]
        [InlineData("One \"\"", new[] { "One", "" })]
        [InlineData("One \"", new[] { "One", "" })]
        [InlineData("1 A=\"B C\"=D 2", new[] { "1", "A=B C=D", "2" })]
        [InlineData("1 A=\"B \\\" C\"=D 2", new[] { "1", "A=B \" C=D", "2" })]
        [InlineData("1 \\A 2", new[] { "1", "\\A", "2" })]
        [InlineData("1 \\\" 2", new[] { "1", "\"", "2" })]
        [InlineData("1 \\\\\" 2", new[] { "1", "\\\"", "2" })]
        [InlineData("\"", new[] { "" })]
        [InlineData("\\\"", new[] { "\"" })]
        [InlineData("'A B'", new[] { "'A", "B'" })]
        [InlineData("^", new[] { "^" })]
        [InlineData("^A", new[] { "A" })]
        [InlineData("^^", new[] { "^" })]
        [InlineData("\\^^", new[] { "\\^" })]
        [InlineData("^\\\\", new[] { "\\\\" })]
        [InlineData("^\"A B\"", new[] { "A B" })]
        [InlineData(@"/src:""C:\tmp\Some Folder\Sub Folder"" /users:""abcdefg@hijkl.com"" tasks:""SomeTask,Some Other Task"" -someParam foo",
                    new[] { @"/src:C:\tmp\Some Folder\Sub Folder", @"/users:abcdefg@hijkl.com", @"tasks:SomeTask,Some Other Task", @"-someParam", @"foo" })]
        [InlineData("", new string[] { })]
        [InlineData("a", new[] { "a" })]
        [InlineData(" abc ", new[] { "abc" })]
        [InlineData("a b ", new[] { "a", "b" })]
        [InlineData("a b \"c d\"", new[] { "a", "b", "c d" })]
        [InlineData("this is a test ", new[] { "this", "is", "a", "test" })]
        [InlineData("this \"is a\" test", new[] { "this", "is a", "test" })]
        [InlineData("\"C:\\Program Files\"", new[] { "C:\\Program Files" })]
        [InlineData("\"He whispered to her \\\"I love you\\\".\"", new[] { "He whispered to her \"I love you\"." })]
        public void Should_split(string commandLine, string[] results)
        {
            //Act
            IEnumerable<string> split = CommandLine.Split(commandLine);

            _output.WriteLine(commandLine);
            _output.WriteLine(JsonConvert.SerializeObject(split));

            //Assert
            split.Count().Should().Be(results.Length);
            split.Should().Equal(results);
        }

        [Theory]
        [InlineData("\"\"", "")]
        [InlineData("\"\\\"\\\"\"", "\"\"")]
        [InlineData("\"\\\"a\\\"\"", "\"a\"")]
        [InlineData("\\", "\\")]
        [InlineData("\"a b\"", "a b")]
        [InlineData("a\\\\b", "a\\\\b")]
        [InlineData("\"a\\\\b c\"", "a\\\\b c")]
        [InlineData("\" \\\\\"", " \\")]
        [InlineData("\" \\\\\\\"\"", " \\\"")]
        [InlineData("\" \\\\\\\\\"", " \\\\")]
        [InlineData("\"C:\\Program Files\\\\\"", "C:\\Program Files\\")]
        [InlineData("\"dafc\\\"\\\"\\\"a\"", "dafc\"\"\"a")]
        [InlineData("One", "One")]
        [InlineData("\"One \"", "One ")]
        [InlineData("\" One\"", " One")]
        [InlineData("\" One \"", " One ")]
        [InlineData("\"One Two\"", "One Two")]
        //[InlineData("\"", "")]
        //[InlineData("\\\"", "\"")]
        [InlineData("^", "^")]
        //[InlineData("\\^^", "\\^")]
        //[InlineData("^\\\\", "\\\\")]
        //[InlineData("^\"A B\"", "A B")]
        public void Should_encode_argument(string expected, string argument)
        {
            //Act
            string encoded = CommandLine.EncodeArgument(argument);

            _output.WriteLine($"[{expected}]");
            _output.WriteLine($"[{encoded}]");

            //Assert
            encoded.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("One", new[] { "One" })]
        [InlineData("\"One \"", new[] { "One " })]
        [InlineData("\" One\"", new[] { " One" })]
        [InlineData("\" One \"", new[] { " One " })]
        [InlineData("One Two", new[] { "One", "Two" })]
        [InlineData("One \"Two Three\"", new[] { "One", "Two Three" })]
        [InlineData("One \"Two Three\" Four", new[] { "One", "Two Three", "Four" })]
        [InlineData("\"One Two\"", new[] { "One Two" })]
        [InlineData("One \"\"", new[] { "One", "" })]
        [InlineData("1 \\A 2", new[] { "1", "\\A", "2" })]
        [InlineData("'A B'", new[] { "'A", "B'" })]
        [InlineData("^", new[] { "^" })]
        [InlineData("", new string[] { })]
        [InlineData("a", new[] { "a" })]
        [InlineData("a b \"c d\"", new[] { "a", "b", "c d" })]
        [InlineData("this is a test", new[] { "this", "is", "a", "test" })]
        [InlineData("this \"is a\" test", new[] { "this", "is a", "test" })]
        [InlineData("\"C:\\Program Files\"", new[] { "C:\\Program Files" })]
        [InlineData("\"He whispered to her \\\"I love you\\\".\"", new[] { "He whispered to her \"I love you\"." })]
        public void Should_encode_arguments(string commandLine, string[] arguments)
        {
            //Act
            string encoded = CommandLine.EncodeArguments(arguments);

            _output.WriteLine($"[{commandLine}]");
            _output.WriteLine($"[{encoded}]");

            //Assert
            encoded.Should().BeEquivalentTo(commandLine);
        }
    }
}
