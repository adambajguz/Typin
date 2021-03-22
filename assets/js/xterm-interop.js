const xtermInterop = {};

xtermInterop.terminals = new Map();

xtermInterop.initialize = function (id) {
    if (!xtermInterop.terminals.has(id)) {
        Terminal.applyAddon(fit);

        const terminal = new Terminal();

        xtermInterop.terminals.set(id, terminal);

        terminal.open(document.getElementById(id));
        terminal.fit();

        const shellprompt = '$ ';
        //terminal.on("data", (data) => {
        //    terminal.write(data);
        //});
        terminal.id = id;

        terminal.prompt = function () {
            terminal.write('\r\n' + shellprompt);
        };

        terminal.welcome = function () {
            terminal.writeln("Welcome to TypinExamples.");
            terminal.writeln("This is a local terminal emulation.");
            terminal.writeln("Type './run' to run an example or 'help' for more informations.");
        };

        terminal.help = function () {
            terminal.writeln("");
            terminal.writeln("Emulated commands:");
            terminal.writeln("  ./run <args>         run an example with or without optional arguments");
            terminal.writeln("  ./run.exe <args>");
            terminal.writeln("  run.exe <args>");
            terminal.writeln("  run <args>");
            terminal.writeln("");
            terminal.writeln("  clear                clears screen");
            terminal.writeln("  cls");
            terminal.writeln("");
            terminal.writeln("  help                 show this help");
        };

        terminal.welcome();

        terminal.cmd = '';
        terminal.on('key', function (key, ev) {
            let printable = (
                !ev.altKey &&
                !ev.altGraphKey &&
                !ev.ctrlKey &&
                !ev.metaKey &&
                !(ev.keyCode >= 112 && ev.keyCode <= 123) // filter F1-F12
            );

            if (!printable) {
                return;
            }

            //console.log("KEY_TYPE: " + ev.keyCode + " " + printable);
            //console.log(ev);

            if (ev.keyCode == 13) {
                if (terminal.cmd === 'clear' || terminal.cmd === 'cls') {
                    terminal.reset();
                    terminal.cmd = '';
                    terminal.prompt();
                }
                else if (terminal.cmd === 'help') {
                    terminal.help();
                    terminal.cmd = '';
                    terminal.prompt();
                }
                else if (terminal.cmd.startsWith("./run ") || //".\\run.exe" does not work
                    terminal.cmd.startsWith("run.exe ") ||
                    terminal.cmd.startsWith("./run.exe ") ||
                    terminal.cmd.startsWith("run ") ||
                    terminal.cmd == "./run" ||
                    terminal.cmd == "run.exe" ||
                    terminal.cmd == "./run.exe" ||
                    terminal.cmd == "run") {

                    terminal.writeln("");

                    let tmpCmd = terminal.cmd;

                    DotNet.invokeMethodAsync('TypinExamples', 'TerminalManager_ExampleInit', terminal.id, tmpCmd)
                        .then(() => {
                            terminal.write('\u001b[39m')
                            terminal.prompt();
                            terminal.cmd = '';
                        });
                }
                else if (terminal.cmd.isNullOrWhiteSpace()) {
                    terminal.cmd = '';
                    terminal.prompt();
                }
                else {
                    terminal.writeln("");
                    terminal.writeln(terminal.cmd + ": command not found");
                    terminal.cmd = '';
                    terminal.prompt();
                }
            } else if (ev.keyCode == 8) {
                // Do not delete the prompt
                if (terminal.cmd.length > 0) {
                    terminal.cmd = terminal.cmd.slice(0, -1);
                    terminal.write('\b \b');
                }
            } else {
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
        xtermInterop.terminals.delete(id);
    }
}

window.xtermInterop = xtermInterop;
