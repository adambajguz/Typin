namespace TypinExamples.Application.Configuration
{
    public sealed class ExampleDescriptor
    {
        public string? Key { get; init; }
        public string? Name { get; init; }
        public bool IsNew { get; init; }
        public string[]? Modes { get; init; }
        public string? ProgramClass { get; init; }
        public string? WebProgramClass { get; init; }
        public string? Description { get; init; }
        public string? QuickStart { get; init; }

        public static ExampleDescriptor CreateDynamic()
        {
            return new ExampleDescriptor
            {
                Key = null,
                Name = "<dynamic>",
                IsNew = true,
                ProgramClass = "TypinExamples.DynamicCode.Program, TypinExamples.DynamicCode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
                WebProgramClass = "TypinExamples.DynamicCode.WebProgram, TypinExamples.DynamicCode, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
                Description = "User implemented example.",
                QuickStart = string.Empty
            };
        }

        public override string? ToString()
        {
            return $"('{Key}', '{Name}', '{ProgramClass}', '{WebProgramClass}')";
        }
    }
}
