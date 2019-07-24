﻿using System.Collections.Generic;
using CliFx.Models;

namespace CliFx.Services
{
    public interface ICommandHelpTextBuilder
    {
        string Build(IReadOnlyList<CommandSchema> commandSchemas, CommandSchema commandSchema);
    }
}