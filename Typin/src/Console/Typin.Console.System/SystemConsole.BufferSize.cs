namespace Typin.Console
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public partial class SystemConsole
    {
        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
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