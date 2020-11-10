﻿namespace TypinExamples.Services
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;
    using Microsoft.JSInterop;
    using TypinExamples.Shared.Components;

    //https://medium.com/codingtown/xterm-js-terminal-2b19ccd2a52
    public sealed class XTermService : IDisposable
    {
        private static readonly Dictionary<string, XTerm> _terminals = new Dictionary<string, XTerm>();

        private const string MODULE_NAME = "xtermInterop";

        private readonly IJSInProcessRuntime _runtime;
        private readonly ILogger<XTermService> _logger;

        public XTermService(IJSRuntime runtime, ILogger<XTermService> logger)
        {
            _runtime = runtime as IJSInProcessRuntime;
            _logger = logger;
        }

        public void Initialize(string id, XTerm terminal)
        {
            _runtime.Invoke<object>($"{MODULE_NAME}.initialize", id);
            _terminals[id] = terminal;
        }

        public void Finalize(string id)
        {
            _runtime.Invoke<string>($"{MODULE_NAME}.dispose", id);

            if (_terminals.ContainsKey(id))
                _terminals.Remove(id);
        }

        public string Write(string id, string str)
        {
            return _runtime.Invoke<string>($"{MODULE_NAME}.write", id, str);
        }

        public string WriteLine(string id, string str)
        {
            return _runtime.Invoke<string>($"{MODULE_NAME}.writeLine", id, str);
        }

        public void Dispose()
        {
            foreach (string id in _terminals.Keys)
            {
                Finalize(id);
            }
        }
    }
}