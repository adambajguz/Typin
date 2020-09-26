namespace Typin.Analyzers
{
    using Microsoft.CodeAnalysis;

    public static class DiagnosticDescriptors
    {
#pragma warning disable RS2008 // Enable analyzer release tracking
        public static readonly DiagnosticDescriptor Typin0001 =
            new DiagnosticDescriptor(nameof(Typin0001),
                "Type must implement the 'Typin.ICommand' interface in order to be a valid command",
                "Type must implement the 'Typin.ICommand' interface in order to be a valid command",
                "Usage", DiagnosticSeverity.Error, true);

        public static readonly DiagnosticDescriptor Typin0002 =
            new DiagnosticDescriptor(nameof(Typin0002),
                "Type must be annotated with the 'Typin.Attributes.CommandAttribute' in order to be a valid command",
                "Type must be annotated with the 'Typin.Attributes.CommandAttribute' in order to be a valid command",
                "Usage", DiagnosticSeverity.Error, true);

        public static readonly DiagnosticDescriptor Typin0021 =
            new DiagnosticDescriptor(nameof(Typin0021),
                "Parameter order must be unique within its command",
                "Parameter order must be unique within its command",
                "Usage", DiagnosticSeverity.Error, true);

        public static readonly DiagnosticDescriptor Typin0022 =
            new DiagnosticDescriptor(nameof(Typin0022),
                "Parameter order must have unique name within its command",
                "Parameter order must have unique name within its command",
                "Usage", DiagnosticSeverity.Error, true);

        public static readonly DiagnosticDescriptor Typin0023 =
            new DiagnosticDescriptor(nameof(Typin0023),
                "Only one non-scalar parameter per command is allowed",
                "Only one non-scalar parameter per command is allowed",
                "Usage", DiagnosticSeverity.Error, true);

        public static readonly DiagnosticDescriptor Typin0024 =
            new DiagnosticDescriptor(nameof(Typin0024),
                "Non-scalar parameter must be last in order",
                "Non-scalar parameter must be last in order",
                "Usage", DiagnosticSeverity.Error, true);

        public static readonly DiagnosticDescriptor Typin0041 =
            new DiagnosticDescriptor(nameof(Typin0041),
                "Option must have a name or short name specified",
                "Option must have a name or short name specified",
                "Usage", DiagnosticSeverity.Error, true);

        public static readonly DiagnosticDescriptor Typin0042 =
            new DiagnosticDescriptor(nameof(Typin0042),
                "Option name must be at least 2 characters long",
                "Option name must be at least 2 characters long",
                "Usage", DiagnosticSeverity.Error, true);

        public static readonly DiagnosticDescriptor Typin0043 =
            new DiagnosticDescriptor(nameof(Typin0043),
                "Option name must be unique within its command",
                "Option name must be unique within its command",
                "Usage", DiagnosticSeverity.Error, true);

        public static readonly DiagnosticDescriptor Typin0044 =
            new DiagnosticDescriptor(nameof(Typin0044),
                "Option short name must be unique within its command",
                "Option short name must be unique within its command",
                "Usage", DiagnosticSeverity.Error, true);

        public static readonly DiagnosticDescriptor Typin0100 =
            new DiagnosticDescriptor(nameof(Typin0100),
                "Use the provided IConsole abstraction instead of System.Console to ensure that the command can be tested in isolation",
                "Use the provided IConsole abstraction instead of System.Console to ensure that the command can be tested in isolation",
                "Usage", DiagnosticSeverity.Warning, true);
#pragma warning restore RS2008 // Enable analyzer release tracking
    }
}