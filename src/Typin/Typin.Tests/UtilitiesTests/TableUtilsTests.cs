﻿namespace Typin.Tests.UtilitiesTests
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using Typin.Console;
    using Typin.Utilities;
    using Xunit;
    using Xunit.Abstractions;

    public class TableUtilsTests
    {
        private class Data
        {
            public int Integer { get; init; }
            public string? Str { get; init; }
            public string? Group { get; init; }
        }

        private readonly List<Data> _testData = new()
        {
            new Data { Integer = 0, Str = "testA", Group = "X" },
            new Data { Integer = 1, Str = "testB", Group = "Z" },
            new Data { Integer = 2, Str = "testC", Group = null },
            new Data { Integer = 3, Str = "testD", Group = "X" },
            new Data { Integer = 4, Str = null, Group = "Z" }
        };

        private readonly ITestOutputHelper _output;

        public TableUtilsTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Table_utils_write_should_write_a_table()
        {
            // Arrange
            var (console, stdOut, _) = VirtualConsole.CreateBuffered(isOutputRedirected: false, isErrorRedirected: false);

            // Act
            TableUtils.Write(console.Output,
                             _testData,
                             new string[] { "Header0", "Header1", "Header2" },
                             null,
                             x => x.Integer.ToString(),
                             x => x.Str ?? string.Empty);

            // Assert
            string output = stdOut.GetString();
            _output.WriteLine(output);

            output.Should().NotBeNullOrWhiteSpace();
            output.Should().ContainAll("Header0", "Header1",
                                       "testA", "testB", "testC", "testD",
                                       "0", "1", "2", "3", "4");
            output.Should().NotContainAll("Header2",
                                          "X", "Z",
                                          "X (2)", "Z (2)", "(1)");
        }

        [Fact]
        public void Table_utils_write_should_write_a_table_with_grouping()
        {
            // Arrange
            var (console, stdOut, _) = VirtualConsole.CreateBuffered(isOutputRedirected: false, isErrorRedirected: false);

            // Act
            TableUtils.Write(console.Output,
                             _testData.GroupBy(x => x.Group),
                             new string[] { "Header0", "Header1", "Header2" },
                             null,
                             x => x.Integer.ToString(),
                             x => x.Str ?? string.Empty);

            // Assert
            string output = stdOut.GetString();
            _output.WriteLine(output);

            output.Should().NotBeNullOrWhiteSpace();
            output.Should().ContainAll("Header0", "Header1",
                                       "testA", "testB", "testC", "testD",
                                       "0", "1", "2", "3", "4",
                                       "X", "Z",
                                       "X (2)", "Z (2)", "(1)");

            output.Should().NotContainAll("Header2");
        }

        [Fact]
        public void Table_utils_write_should_not_write_a_table_when_empty_or_null()
        {
            // Arrange
            var (console, stdOut, _) = VirtualConsole.CreateBuffered(isOutputRedirected: false, isErrorRedirected: false);

            // Act
            TableUtils.Write(console.Output,
                             new List<Data>(),
                             new string[] { "Header0", "Header1", "Header2" },
                             null,
                             x => x.Integer.ToString(),
                             x => x.Str ?? string.Empty);

            // Assert
            string output = stdOut.GetString();
            _output.WriteLine(output);

            output.Should().NotBeNullOrWhiteSpace();
            output.Should().ContainAll("Header0", "Header1");
            output.Should().NotContainAll("Header2");
        }

        [Fact]
        public void Table_utils_write_should_write_a_table_with_grouping_when_empty_or_null()
        {
            // Arrange
            var (console, stdOut, _) = VirtualConsole.CreateBuffered(isOutputRedirected: false, isErrorRedirected: false);

            // Act
            TableUtils.Write(console.Output,
                             new List<Data>().GroupBy(x => x.Group),
                             new string[] { "Header0", "Header1", "Header2" },
                             null,
                             x => x.Integer.ToString(),
                             x => x.Str ?? string.Empty);

            // Assert
            string output = stdOut.GetString();
            _output.WriteLine(output);

            output.Should().NotBeNullOrWhiteSpace();
            output.Should().ContainAll("Header0", "Header1");
            output.Should().NotContainAll("Header2");
        }
    }
}