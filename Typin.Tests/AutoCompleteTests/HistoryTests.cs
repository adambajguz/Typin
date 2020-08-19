namespace Typin.Tests.AutoCompleteTests
{
    using System.Linq;
    using FluentAssertions;
    using Typin.Internal.AutoComplete;
    using Xunit;

    public sealed class HistoryTests
    {
        private string[] _history = new string[] { "ls -a", "dotnet run", "git init" };

        [Fact]
        public void Should_add_collection_to_history()
        {
            AutoCompleteInput input = new AutoCompleteInput(new SystemConsole());
            input.AddHistory(_history);

            input.GetHistory().Should().Equal(_history);
        }

        [Fact]
        public void Should_add_single_item_to_history()
        {
            AutoCompleteInput input = new AutoCompleteInput(new SystemConsole());
            input.AddHistory(_history);

            input.GetHistory().Should().Equal(_history);

            input.AddHistory("mkdir");
            input.GetHistory().Should().ContainInOrder(_history);
            input.GetHistory().Last().Should().Be("mkdir");
        }

        [Fact]
        public void Should_add_single_item_to_empty_history()
        {
            AutoCompleteInput input = new AutoCompleteInput(new SystemConsole());

            input.AddHistory("mkdir");
            input.GetHistory().Should().Equal(new string[] { "mkdir" });
        }

        [Fact]
        public void Should_clear_history()
        {
            AutoCompleteInput input = new AutoCompleteInput(new SystemConsole());
            input.AddHistory(_history);
            input.GetHistory().Count.Should().Be(3);

            input.ClearHistory();
            input.GetHistory().Count.Should().Be(0);
        }

        [Fact]
        public void Should_add_after_history_clearing()
        {
            AutoCompleteInput input = new AutoCompleteInput(new SystemConsole());
            input.AddHistory(_history);
            input.ClearHistory();
            input.GetHistory().Count.Should().Be(0);

            input.AddHistory("mkdir");
            input.GetHistory().Should().Equal(new string[] { "mkdir" });
        }
    }
}
