namespace TypinExamples.Services.Terminal
{
    using System.Text.Json.Serialization;

    public class TerminalOptions
    {
        [JsonPropertyName("allowTransparency")]
        public bool? AllowTransparency { get; set; }

        [JsonPropertyName("bellSound")]
        public string BellSound { get; set; }

        [JsonPropertyName("bellStyle")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public BellStyle? BellStyle { get; set; }

        [JsonPropertyName("convertEol")]
        public bool? ConvertEOL { get; set; }

        [JsonPropertyName("cols")]
        public int? Columns { get; set; }

        [JsonPropertyName("cursorBlink")]
        public bool? CursorBlink { get; set; }

        [JsonPropertyName("cursorStyle")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CursorStyle? CursorStyle { get; set; }

        [JsonPropertyName("disableStdin")]
        public bool? DisableStdin { get; set; }

        [JsonPropertyName("drawBoldTextInBrightColors")]
        public bool? DrawBoldTextInBrightColors { get; set; }

        [JsonPropertyName("fontSize")]
        public int? FontSize { get; set; }

        [JsonPropertyName("fontFamily")]
        public string FontFamily { get; set; }

        [JsonPropertyName("fontWeight")]
        public string FontWeight { get; set; }

        [JsonPropertyName("fontWeightBold")]
        public string FontWeightBold { get; set; }

        [JsonPropertyName("letterSpacing")]
        public int? LetterSpacing { get; set; }

        [JsonPropertyName("lineHeight")]
        public int? LineHeight { get; set; }

        [JsonPropertyName("macOptionIsMeta")]
        public bool? MacOptionIsMeta { get; set; }

        [JsonPropertyName("macOptionClickForcesSelection")]
        public bool? MacOptionClickForcesSelection { get; set; }

        [JsonPropertyName("rendererType")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RendererType? RendererType { get; set; }

        [JsonPropertyName("rightClickSelectsWord")]
        public bool? RightClickSelectsWord { get; set; }

        [JsonPropertyName("rows")]
        public int? Rows { get; set; }

        [JsonPropertyName("screenReaderMode")]
        public bool? ScreenReaderMode { get; set; }

        [JsonPropertyName("scrollback")]
        public int? Scrollback { get; set; }

        [JsonPropertyName("tabStopWidth")]
        public int? TabStopWidth { get; set; }

        [JsonPropertyName("windowsMode")]
        public bool? WindowsMode { get; set; }
    }

    public enum RendererType
    {
        canvas,
        dom
    }

    public enum BellStyle
    {
        none,
        sound
    }

    public enum CursorStyle
    {
        block,
        underline,
        bar
    }
}
