
var monacoInterop = {};
monacoInterop.editors = {};
monacoInterop.initialize = function initialize(elementId, initialCode, language, theme, readOnly) {
    require.config({ paths: { 'vs': 'assets/monaco-editor/min/vs' } });
    require(['vs/editor/editor.main'], function initializeEditor() {
        var editor = monaco.editor.create(document.getElementById(elementId), {
            value: initialCode,
            language: language,
            theme: theme,
            readOnly: readOnly
        });
        monacoInterop.editors[elementId] = editor;
    });
}
monacoInterop.getCode = function getCode(elementId) {
    return monacoInterop.editors[elementId].getValue();
}
monacoInterop.setCode = function setCode(elementId, code) {
    monacoInterop.editors[elementId].setValue(code);
}
window.monacoInterop = monacoInterop;