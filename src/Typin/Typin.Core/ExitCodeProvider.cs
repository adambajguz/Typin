namespace Typin
{
    /// <summary>
    /// A class that can be used to retrive an exit code from CLI application.
    /// </summary>
    public sealed record ExitCodeProvider
    {
        /// <summary>
        /// Host exit code.
        /// </summary>
        public int ExitCode { get; set; }
    }
}
