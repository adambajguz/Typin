namespace Typin.Tests.AutoCompleteTests
{
    using System.Linq;
    using FluentAssertions;
    using Typin.AutoCompletion;
    using Typin.Console;
    using Xunit;

    public sealed class HistoryTests
    {
        private readonly string[] _history = new string[] { "ls -a", "dotnet run", "git init" };

        [Fact]
        public void Should_add_collection_to_history()
        {
            // Arrange
            AutoCompleteInput input = new AutoCompleteInput(new SystemConsole());
            InputHistoryProvider history = input.History;
            history.IsEnabled = true;

            // Act
            history.AddEntry(_history);

            // Assert
            history.GetEntries().Should().Equal(_history);
        }

        [Fact]
        public void Should_add_single_item_to_history()
        {
            // Arrange
            AutoCompleteInput input = new AutoCompleteInput(new SystemConsole());
            InputHistoryProvider history = input.History;
            history.IsEnabled = true;

            // Act
            history.AddEntry(_history);

            // Assert
            history.GetEntries().Should().Equal(_history);

            // Act
            history.AddEntry("mkdir");

            // Assert
            history.GetEntries().Should().ContainInOrder(_history);
            history.GetEntries().Last().Should().Be("mkdir");
        }

        [Fact]
        public void Should_add_single_item_to_empty_history()
        {
            // Arrange
            AutoCompleteInput input = new AutoCompleteInput(new SystemConsole());
            InputHistoryProvider history = input.History;
            history.IsEnabled = true;

            // Act
            history.AddEntry("mkdir");

            // Assert
            history.GetEntries().Should().Equal(new string[] { "mkdir" });
        }

        [Fact]
        public void Should_clear_history()
        {
            // Arrange
            AutoCompleteInput input = new AutoCompleteInput(new SystemConsole());
            InputHistoryProvider history = input.History;
            history.IsEnabled = true;

            // Act
            history.AddEntry(_history);

            // Assert
            history.GetEntries().Count.Should().Be(3);

            // Act
            history.Clear();

            // Assert
            history.GetEntries().Count.Should().Be(0);
        }

        [Fact]
        public void Should_add_after_history_clearing()
        {
            // Arrange
            AutoCompleteInput input = new AutoCompleteInput(new SystemConsole());
            InputHistoryProvider history = input.History;
            history.IsEnabled = true;

            // Act
            history.AddEntry(_history);
            history.Clear();

            // Assert
            history.GetEntries().Count.Should().Be(0);

            // Act
            history.AddEntry("mkdir");

            // Assert
            history.GetEntries().Should().Equal(new string[] { "mkdir" });
        }
    }
}
