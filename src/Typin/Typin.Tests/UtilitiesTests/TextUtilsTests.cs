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
            string t = new string(' ', 2);
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
            string t = new string(' ', 8);
            output.Should().Be($"Lorem ipsum dolor sit amet,{t} consectetur adipiscing elit.{t}{t} Proin in quam enim. Integer consequat efficitur maximus.{t}{t}{t}In egestas eget magna viverra varius.");

            _output.WriteLine(text);
        }
    }
}