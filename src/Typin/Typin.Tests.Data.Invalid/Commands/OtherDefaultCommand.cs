﻿namespace Typin.Tests.Data.Commands.Invalid
{
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.Common.Commands;

    [Command]
    public class OtherDefaultCommand : SelfSerializeCommandBase
    {
        public OtherDefaultCommand(IConsole console) : base(console)
        {

        }
    }
}