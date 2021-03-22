namespace Typin.Tests.UtilitiesTests
{
    using System;
    using FluentAssertions;
    using Typin.Utilities;
    using Xunit;
    using Xunit.Abstractions;

    public class TextUtilsTests
    {
        private readonly ITestOutputHelper _output;

        public TextUtilsTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void AdjustNewLines_should_adjust_new_lines_to_current_system()
        {
            // Arrange
            string text = "Lorem ipsum dolor sit amet,\r consectetur adipiscing elit.\n Proin in quam enim.\r\n Integer consequat efficitur maximus.\r\n\r In egestas eget magna viverra varius.\n\r";

            // Act
            string output = TextUtils.AdjustNewLines(text);

            // Assert
            string le = Environment.NewLine;
            output.Should().Be($"Lorem ipsum dolor sit amet,{le} consectetur adipiscing elit.{le} Proin in quam enim.{le} Integer consequat efficitur maximus.{le}{le} In egestas eget magna viverra varius.{le}{le}");

            _output.WriteLine(text);
        }

        [Fact]
        public void ConvertTabsToSpaces_should_replace_tabs_with_2_spaces()
        {
            // Arrange
            string text = "Lorem ipsum dolor sit amet,\t consectetur adipiscing elit.\t\t Proin in quam enim. Integer consequat efficitur maximus.\t\t\tIn egestas eget magna viverra varius.";

            // Act
            string output = TextUtils.ConvertTabsToSpaces(text);

            // Assert
            string t = new(' ', 2);
            output.Should().Be($"Lorem ipsum dolor sit amet,{t} consectetur adipiscing elit.{t}{t} Proin in quam enim. Integer consequat efficitur maximus.{t}{t}{t}In egestas eget magna viverra varius.");

            _output.WriteLine(text);
        }

        [Fact]
        public void ConvertTabsToSpaces_should_replace_tabs_with_given_amount_of_spaces()
        {
            // Arrange
            string text = "Lorem ipsum dolor sit amet,\t consectetur adipiscing elit.\t\t Proin in quam enim. Integer consequat efficitur maximus.\t\t\tIn egestas eget magna viverra varius.";

            // Act
            string output = TextUtils.ConvertTabsToSpaces(text, 8);

            // Assert
            string t = new(' ', 8);
            output.Should().Be($"Lorem ipsum dolor sit amet,{t} consectetur adipiscing elit.{t}{t} Proin in quam enim. Integer consequat efficitur maximus.{t}{t}{t}In egestas eget magna viverra varius.");

            _output.WriteLine(text);
        }

        [Theory]
        [InlineData("The Quick Brown Fox", "the-quick-brown-fox")]
        [InlineData("theQuickBrownFox", "the-quick-brown-fox")]
        [InlineData("the-quick-brown-fox", "the-quick-brown-fox")]
        [InlineData("TheQuickBrownFox", "the-quick-brown-fox")]
        [InlineData("the_quick_brown_fox", "the-quick-brown-fox")]
        [InlineData("The-Quick-Brown-Fox", "the-quick-brown-fox")]
        [InlineData("TheQuick Brown Fox", "the-quick-brown-fox")]
        [InlineData("TheQuick  Brown  Fox", "the-quick-brown-fox")]
        [InlineData("  TheQuick  Brown  Fox  ", "the-quick-brown-fox")]
        [InlineData("someActionToDo", "some-action-to-do")]
        [InlineData("SomeActionToDo", "some-action-to-do")]
        [InlineData("some-action-to-do", "some-action-to-do")]
        [InlineData("Some-Action-To-Do", "some-action-to-do")]
        [InlineData("some_action_to_do", "some-action-to-do")]
        [InlineData("Some_Action_to_do", "some-action-to-do")]
        public void Kebab_case_conversion_should_return_proper_string(string input, string output)
        {
            // Act
            string result = TextUtils.ToKebabCase(input);

            // Assert
            result.Should().Be(output);
        }

        [Theory]
        [InlineData(@"0", '0')]
        [InlineData(@"1", '1')]
        [InlineData(@"9", '9')]
        [InlineData(@"-", '-')]
        [InlineData(@"`", '`')]
        [InlineData(@"!", '!')]
        [InlineData(@"a", 'a')]
        [InlineData(@"q", 'q')]
        [InlineData(@"z", 'z')]
        [InlineData(@"\0", '\0')]
        [InlineData(@"\a", '\a')]
        [InlineData(@"\b", '\b')]
        [InlineData(@"\f", '\f')]
        [InlineData(@"\n", '\n')]
        [InlineData(@"\r", '\r')]
        [InlineData(@"\t", '\t')]
        [InlineData(@"\v", '\v')]
        [InlineData(@"\\", '\\')]
        [InlineData(@"\", '\\')]
        [InlineData(@"     ", '\0')]
        [InlineData(null!, '\0')]
        [InlineData(@"\u0000", '\0')]
        [InlineData(@"\u006a", '\u006a')]
        [InlineData(@"\u006A", '\u006A')]
        [InlineData(@"\U006a", '\u006a')]
        [InlineData(@"\U006A", '\u006A')]
        public void UnescapeChar_should_return_proper_char(string input, char output)
        {
            // Act
            char result = TextUtils.UnescapeChar(input);

            // Assert
            result.Should().Be(output);
        }

        [Theory]
        [InlineData(@"\1")]
        [InlineData(@"\z")]
        [InlineData(@"  \t   ")]
        [InlineData(@"\u00a")]
        [InlineData(@"\u06A")]
        [InlineData(@"\006a")]
        [InlineData(@"\U006Z")]
        [InlineData(@"\u006Z")]
        [InlineData(@"012")]
        [InlineData(@"012346")]
        [InlineData(@"0123456")]
        public void UnescapeChar_should_not_parse(string input)
        {
            // Arrange
            Action act = () =>
            {
                TextUtils.UnescapeChar(input);
            };

            // Assert
            act.Should().Throw<FormatException>().WithMessage($"Cannot parse '{input}' to char.*");
        }
    }
}