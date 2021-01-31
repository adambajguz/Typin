namespace TypinExamples.Services.Terminal
{
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class TerminalOptions
    {
        [JsonPropertyName("allowTransparency")]
        public bool? AllowTransparency { get; init; }

        [JsonPropertyName("bellSound")]
        public string? BellSound { get; init; }

        [JsonPropertyName("bellStyle")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public BellStyle? BellStyle { get; init; }

        [JsonPropertyName("cols")]
        public int? Columns { get; init; }

        [JsonPropertyName("convertEol")]
        public bool? ConvertEOL { get; init; }

        [JsonPropertyName("cursorBlink")]
        public bool? CursorBlink { get; init; }

        [JsonPropertyName("cursorStyle")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CursorStyle? CursorStyle { get; init; }

        [JsonPropertyName("disableStdin")]
        public bool? DisableStdin { get; init; }

        [JsonPropertyName("drawBoldTextInBrightColors")]
        public bool? DrawBoldTextInBrightColors { get; init; }

        [JsonPropertyName("fastScrollModifier")]
        public FastScrollModifiers? FastScrollModifier { get; init; }

        [JsonPropertyName("fastScrollSensitivity")]
        public double? FastScrollSensitivity { get; init; }

        [JsonPropertyName("fontFamily")]
        public string? FontFamily { get; init; }

        [JsonPropertyName("fontSize")]
        public double? FontSize { get; init; }

        [JsonPropertyName("fontWeight")]
        public FontWeights? FontWeight { get; init; }

        [JsonPropertyName("fontWeightBold")]
        public FontWeights? FontWeightBold { get; init; }

        [JsonPropertyName("letterSpacing")]
        public double? LetterSpacing { get; init; }

        [JsonPropertyName("lineHeight")]
        public double? LineHeight { get; init; }

        [JsonPropertyName("logLevel")]
        public XTermLogLevels? LogLevel { get; init; }

        [JsonPropertyName("macOptionClickForcesSelection")]
        public bool? MacOptionClickForcesSelection { get; init; }

        [JsonPropertyName("macOptionIsMeta")]
        public bool? MacOptionIsMeta { get; init; }

        [JsonPropertyName("minimumContrastRatio")]
        public double? MinimumContrastRatio { get; init; }

        [JsonPropertyName("rendererType")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RendererType? RendererType { get; init; }

        [JsonPropertyName("rightClickSelectsWord")]
        public bool? RightClickSelectsWord { get; init; }

        [JsonPropertyName("rows")]
        public int? Rows { get; init; }

        [JsonPropertyName("screenReaderMode")]
        public bool? ScreenReaderMode { get; init; }

        [JsonPropertyName("scrollSensitivity")]
        public double? ScrollSensitivity { get; init; }

        [JsonPropertyName("scrollback")]
        public double? Scrollback { get; init; }

        [JsonPropertyName("tabStopWidth")]
        public double? TabStopWidth { get; init; }

        //TODO:
        //[JsonPropertyName("theme")]
        //public object? Theme { get; set; }

        //[JsonPropertyName("windowOptions")]
        //public object? WindowOptions { get; set; }

        [JsonPropertyName("windowsMode")]
        public bool? WindowsMode { get; init; }

        [JsonPropertyName("wordSeparator")]
        public string? WordSeparator { get; init; }
    }

    public enum FontWeights
    {
        [EnumMember(Value = "bold")]
        Bold,

        [EnumMember(Value = "100")]
        W100,

        [EnumMember(Value = "200")]
        W200,

        [EnumMember(Value = "300")]
        W300,

        [EnumMember(Value = "400")]
        W400,

        [EnumMember(Value = "500")]
        W500,

        [EnumMember(Value = "600")]
        W600,

        [EnumMember(Value = "700")]
        W700,

        [EnumMember(Value = "normal")]
        Normal
    }

    public enum FastScrollModifiers
    {
        [EnumMember(Value = "alt")]
        Alt,

        [EnumMember(Value = "ctrl")]
        Ctrl,

        [EnumMember(Value = "shift")]
        Shift
    }

    public enum RendererType
    {
        [EnumMember(Value = "dom")]
        Dom,

        [EnumMember(Value = "canvas")]
        Canvas
    }

    public enum BellStyle
    {
        [EnumMember(Value = "none")]
        None,

        [EnumMember(Value = "sound")]
        Sound
    }

    public enum CursorStyle
    {
        [EnumMember(Value = "block")]
        Block,

        [EnumMember(Value = "underline")]
        Underline,

        [EnumMember(Value = "bar")]
        Bar
    }

    public enum XTermLogLevels
    {
        [EnumMember(Value = "debug")]
        Debug,

        [EnumMember(Value = "info")]
        Info,

        [EnumMember(Value = "warn")]
        Warn,

        [EnumMember(Value = "error")]
        Error,

        [EnumMember(Value = "off")]
        Off
    }
}
