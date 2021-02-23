namespace Typin.Internal.StackTrace
{
    using System.Collections.Generic;

    internal class StackFrame
    {
        public string ParentType { get; }
        public string MethodName { get; }
        public IReadOnlyList<StackFrameParameter> Parameters { get; }
        public string? FilePath { get; }
        public string? LineNumber { get; }

        public StackFrame(string parentType,
                          string methodName,
                          IReadOnlyList<StackFrameParameter> parameters,
                          string? filePath,
                          string? lineNumber)
        {
            ParentType = parentType;
            MethodName = methodName;
            Parameters = parameters;
            FilePath = filePath;
            LineNumber = lineNumber;
        }
    }
}
