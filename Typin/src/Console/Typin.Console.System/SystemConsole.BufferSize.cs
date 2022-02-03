namespace Typin.Console
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public partial class SystemConsole
    {
        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
        public override int BufferWidth
        {
            get
            {
                int @default = base.BufferWidth;

                return this.IsEnabled(ConsoleFeatures.BufferSize)
                    ? Console.BufferWidth
                    : @default;
            }

            set
            {
                base.BufferWidth = value;

                if (this.IsEnabled(ConsoleFeatures.BufferSize))
                {
                    Console.BufferWidth = value;
                }
            }
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
        public override int BufferHeight
        {
            get
            {
                int @default = base.BufferHeight;

                return this.IsEnabled(ConsoleFeatures.BufferSize)
                    ? Console.BufferHeight
                    : @default;
            }

            set
            {
                base.BufferHeight = value;

                if (this.IsEnabled(ConsoleFeatures.BufferSize))
                {
                    Console.BufferHeight = value;
                }
            }
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
        public override void SetBufferSize(int width, int height)
        {
            base.SetBufferSize(width, height);

            if (this.IsEnabled(ConsoleFeatures.BufferSize))
            {
                Console.SetBufferSize(width, height);
            }
        }
    }
}