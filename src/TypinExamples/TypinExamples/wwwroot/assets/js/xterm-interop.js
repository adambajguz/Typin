var xtermInterop = {};

xtermInterop.terminals = new Map();

xtermInterop.initialize = function (id) {
    if (!this._terminals.has(newLocal)) {
        var terminal = new Terminal();
        this._terminals.set(id, terminal);

        terminal.open(document.getElementById(id));
        terminal.blur();
    }
}

xtermInterop.reset = function (id) {
    const terminal = this._terminals.get(id);
    if (terminal) {
        terminal.reset();
    }
}

xtermInterop.clear = function (id) {
    const terminal = this._terminals.get(id);
    if (terminal) {
        terminal.clear();
    }
}

xtermInterop.focus = function (id) {
    const terminal = this._terminals.get(id);
    if (terminal) {
        terminal.focus();
    }
}

xtermInterop.blur = function (id) {
    const terminal = this._terminals.get(id);
    if (terminal) {
        terminal.blur();
    }
}

xtermInterop.getRows = function (id) {
    const terminal = this._terminals.get(id);
    if (terminal) {
        return terminal.rows;
    }

    return 0;
}

xtermInterop.getColumns = function (id) {
    const terminal = this._terminals.get(id);
    if (terminal) {
        return terminal.cols;
    }

    return 0;
}

xtermInterop.write = function (id, str) {
    const terminal = this._terminals.get(id);
    if (terminal) {
        terminal.write(str);
    }
}

xtermInterop.writeLine = function (id, str) {
    const terminal = this._terminals.get(id);
    if (terminal) {
        terminal.writeln(str);
    }
}

xtermInterop.scrollLines = function (id, lines) {
    const terminal = this._terminals.get(id);
    if (terminal) {
        terminal.scrollLines(lines);
    }
}

xtermInterop.scrollPages = function (id, pagesCount) {
    const terminal = this._terminals.get(id);
    if (terminal) {
        terminal.scrollPages(pagesCount);
    }
}

xtermInterop.scrollToBottom = function (id) {
    const terminal = this._terminals.get(id);
    if (terminal) {
        terminal.scrollToBottom();
    }
}

xtermInterop.scrollToTop = function (id) {
    const terminal = this._terminals.get(id);
    if (terminal) {
        terminal.scrollToTop();
    }
}

xtermInterop.scrollToLine = function (id, lineNumber) {
    const terminal = this._terminals.get(id);
    if (terminal) {
        terminal.scrollToLine(lineNumber);
    }
}

xtermInterop.dispose = function (id) {
    const terminal = this._terminals.get(id);

    if (terminal) {
        terminal.dispose();
        this._terminals.delete(id);
    }
}

window.xtermInterop = xtermInterop;
