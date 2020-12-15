var monacoInterop = {};

monacoInterop.editors = {};

monacoInterop.initialize = function initialize(elementId, initialCode, language, theme, readOnly) {
    require.config({ paths: { 'vs': 'assets/libraries/monaco-editor/min/vs' } });
    require(['vs/editor/editor.main'], function initializeEditor() {
        let editor = monaco.editor.create(document.getElementById(elementId), {
            value: initialCode,
            language: language,
            theme: theme,
            readOnly: readOnly
        });
        monacoInterop.editors[elementId] = editor;
    });
}

monacoInterop.getText = function getText(elementId) {
    return monacoInterop.editors[elementId].getValue();
}

monacoInterop.setText = function setText(elementId, code) {
    monacoInterop.editors[elementId].setValue(code);
}

monacoInterop.showLineNumbers = function showLineNumbers(elementId) {
    monacoInterop.editors[elementId].updateOptions({
        lineNumbers: "on"
    });
}

monacoInterop.hideLineNumbers = function hideLineNumbers(elementId) {
    monacoInterop.editors[elementId].updateOptions({
        lineNumbers: "off"
    });
}

monacoInterop.toggleLineNumbersVisibility = function toggleLineNumbersVisibility(elementId) {
    let editor = monacoInterop.editors[elementId];
    let currentLineNumberType = editor.getOptions().get(monaco.editor.EditorOption.lineNumbers).renderType;

    editor.updateOptions({
        lineNumbers: currentLineNumberType == 0 ? "on" : "off"
    });
}

window.monacoInterop = monacoInterop;