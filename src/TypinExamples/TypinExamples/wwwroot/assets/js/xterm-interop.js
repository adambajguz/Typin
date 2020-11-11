const xtermInterop = {};

xtermInterop.terminals = new Map();

xtermInterop.initialize = function (id) {
    if (!xtermInterop.terminals.has(id)) {
        const terminal = new Terminal();
        xtermInterop.terminals.set(id, terminal);

        terminal.open(document.getElementById(id));
        shellprompt = '$ ';
        //terminal.on("data", (data) => {
        //    terminal.write(data);
        //});
        terminal.id = id;
        terminal.prompt = function () {
            terminal.write('\r\n' + shellprompt);
        };
        terminal.cmd = '';
        terminal.on('key', function (key, ev) {
            let printable = (
                !ev.altKey && !ev.altGraphKey && !ev.ctrlKey && !ev.metaKey
            );

            if (ev.keyCode == 13) {
                if (terminal.cmd === 'clear' || terminal.cmd === 'cls') {
                    terminal.reset();
                    terminal.cmd = '';
                    terminal.prompt();
                }
                else if (terminal.cmd.startsWith(".\run") || terminal.cmd.startsWith("./run")
                    || terminal.cmd.startsWith("run.exe") || terminal.cmd.startsWith(".\run.exe")
                    || terminal.cmd.startsWith("./run.exe")) {
                    terminal.writeln("");
                    DotNet.invokeMethodAsync('TypinExamples', 'ExampleInit', terminal.id, terminal.cmd)
                        .then(() => {
                            terminal.prompt();
                            terminal.cmd = '';
                        });
                }
                else {
                    terminal.writeln("");
                    terminal.writeln(terminal.cmd + ": command not found");
                    terminal.cmd = '';
                    terminal.prompt();
                }
            } else if (ev.keyCode == 8) {
                // Do not delete the prompt
                console.log(terminal.rows);
                if (terminal.cmd.length > 0) {
                    terminal.cmd = terminal.cmd.slice(0, -1);
                    terminal.write('\b \b');
                }
            } else if (printable) {
                terminal.cmd += key;
                terminal.write(key);
            }
        });

        terminal.on('paste', function (data, ev) {
            terminal.write(data);
        });

        terminal.prompt();
    }
}

xtermInterop.reset = function (id) {
    const terminal = xtermInterop.terminals.get(id);
    if (terminal) {
        terminal.reset();
    }
}

xtermInterop.clear = function (id) {
    const terminal = xtermInterop.terminals.get(id);
    if (terminal) {
        terminal.clear();
    }
}

xtermInterop.focus = function (id) {
    const terminal = xtermInterop.terminals.get(id);
    if (terminal) {
        terminal.focus();
    }
}

xtermInterop.blur = function (id) {
    const terminal = xtermInterop.terminals.get(id);
    if (terminal) {
        terminal.blur();
    }
}

xtermInterop.getRows = function (id) {
    const terminal = xtermInterop.terminals.get(id);
    if (terminal) {
        return terminal.rows;
    }

    return 0;
}

xtermInterop.getColumns = function (id) {
    const terminal = xtermInterop.terminals.get(id);
    if (terminal) {
        return terminal.cols;
    }

    return 0;
}

xtermInterop.write = function (id, str) {
    const terminal = xtermInterop.terminals.get(id);
    if (terminal) {
        terminal.write(str);
    }
}

xtermInterop.writeLine = function (id, str) {
    const terminal = xtermInterop.terminals.get(id);
    if (terminal) {
        terminal.writeln(str);
    }
}

xtermInterop.scrollLines = function (id, lines) {
    const terminal = xtermInterop.terminals.get(id);
    if (terminal) {
        terminal.scrollLines(lines);
    }
}

xtermInterop.scrollPages = function (id, pagesCount) {
    const terminal = xtermInterop.terminals.get(id);
    if (terminal) {
        terminal.scrollPages(pagesCount);
    }
}

xtermInterop.scrollToBottom = function (id) {
    const terminal = xtermInterop.terminals.get(id);
    if (terminal) {
        terminal.scrollToBottom();
    }
}

xtermInterop.scrollToTop = function (id) {
    const terminal = xtermInterop.terminals.get(id);
    if (terminal) {
        terminal.scrollToTop();
    }
}

xtermInterop.scrollToLine = function (id, lineNumber) {
    const terminal = xtermInterop.terminals.get(id);
    if (terminal) {
        terminal.scrollToLine(lineNumber);
    }
}

xtermInterop.dispose = function (id) {
    const terminal = xtermInterop.terminals.get(id);

    if (terminal) {
        terminal.dispose();
        terminals.terminals.delete(id);
    }
}

window.xtermInterop = xtermInterop;
