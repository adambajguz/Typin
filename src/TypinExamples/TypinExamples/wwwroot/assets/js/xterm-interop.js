var xtermInterop = {};

xtermInterop.terminals = {};

xtermInterop.initialize = function initialize(elementId) {
    var term = new Terminal();
    term.open(document.getElementById(elementId));
    term.write('Hello from \x1B[1;3;31mxterm.js\x1B[0m $ ');
    xtermInterop.terminals[elementId] = term;
}

xtermInterop.write = function getCode(elementId, str) {
    xtermInterop.terminals[elementId].write(str);
}

window.xtermInterop = xtermInterop;