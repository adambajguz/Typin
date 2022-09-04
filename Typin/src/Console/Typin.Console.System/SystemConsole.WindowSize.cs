namespace Typin.Console
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public partial class SystemConsole
    {
        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
        public override int WindowWidth
        {
            get
            {
                int @default = base.WindowWidth;

                return this.IsEnabled(ConsoleFeatures.WindowSize)
                    ? Console.WindowWidth
                    : @default;
            }

            set
            {
                base.WindowWidth = value;

                if (this.IsEnabled(ConsoleFeatures.WindowSize))
                {
                    Console.WindowWidth = value;
                }
            }
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
        public override int WindowHeight
        {
            get
            {
                int @default = base.WindowHeight;

                return this.IsEnabled(ConsoleFeatures.WindowSize)
                    ? Console.WindowHeight
                    : @default;
            }

            set
            {
                base.WindowHeight = value;

                if (this.IsEnabled(ConsoleFeatures.WindowSize))
                {
                    Console.WindowHeight = value;
                }
            }
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        public override int LargestWindowWidth
        {
            get
            {
                int @default = base.LargestWindowWidth;

                return this.IsEnabled(ConsoleFeatures.WindowSize)
                    ? Console.LargestWindowWidth
                    : @default;
            }
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        public override int LargestWindowHeight
        {
            get
            {
                int @default = base.LargestWindowHeight;

                return this.IsEnabled(ConsoleFeatures.WindowSize)
                    ? Console.LargestWindowHeight
                    : @default;
            }
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
        public override void SetWindowSize(int width, int height)
        {
            base.SetWindowSize(width, height);

            if (this.IsEnabled(ConsoleFeatures.WindowSize))
            {
                Console.SetWindowSize(width, height);
            }
        }
    }
}