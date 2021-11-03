﻿namespace Typin.Utilities
{
    using System;
    using Typin.Console;

    /// <summary>
    /// Utility for rendering current progress to the console that erases and rewrites output on every tick.
    /// </summary>
    public class ProgressTicker : IProgress<double>
    {
        private readonly IConsole _console;

        private int? _originalCursorLeft;
        private int? _originalCursorTop;

        /// <summary>
        /// Initializes an instance of <see cref="ProgressTicker"/>.
        /// </summary>
        public ProgressTicker(IConsole console)
        {
            _console = console;
        }

        private void RenderProgress(double progress)
        {
            if (_originalCursorLeft is null || _originalCursorTop is null)
            {
                _originalCursorLeft = _console.CursorLeft;
                _originalCursorTop = _console.CursorTop;
            }
            else
            {
                _console.CursorLeft = _originalCursorLeft.Value;
                _console.CursorTop = _originalCursorTop.Value;
            }

            string str = progress.ToString("P2", _console.Output.FormatProvider);
            _console.Output.Write(str);
        }

        /// <summary>
        /// Erases previous output and renders new progress to the console.
        /// If stdout is redirected, this method returns without doing anything.
        /// </summary>
        public void Report(double progress)
        {
            // We don't do anything if stdout is redirected to avoid polluting output
            // when there's no active console window.
            if (!_console.Output.IsRedirected)
            {
                RenderProgress(progress);
            }
        }
    }

    /// <summary>
    /// Extensions for <see cref="ProgressTicker"/>.
    /// </summary>
    public static class ProgressTickerExtensions
    {
        /// <summary>
        /// Creates a <see cref="ProgressTicker"/> bound to this console.
        /// </summary>
        public static ProgressTicker CreateProgressTicker(this IConsole console)
        {
            return new ProgressTicker(console);
        }
    }
}