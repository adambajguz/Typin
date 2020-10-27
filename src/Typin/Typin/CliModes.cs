namespace Typin
{
    using System;

    /// <summary>
    /// CLI modes.
    /// </summary>
    [Flags]
    public enum CliModes
    {
        /// <summary>
        /// Direct CLI tool mode.
        /// </summary>
        Direct = 1,

        /// <summary>
        /// Interactive CLI mode.
        /// </summary>
        Interactive = 2

        ///// <summary>
        ///// Interactive CLI mode.
        ///// </summary>
        //Batch = 4
    }
}
