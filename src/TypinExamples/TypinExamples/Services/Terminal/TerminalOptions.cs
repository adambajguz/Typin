namespace TypinExamples.Services.Terminal
{
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class TerminalOptions
    {
        [JsonPropertyName("allowTransparency")]
        public bool? AllowTransparency { get; set; }

        [JsonPropertyName("bellSound")]
        public string? BellSound { get; set; }

        [JsonPropertyName("bellStyle")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public BellStyle? BellStyle { get; set; }

        [JsonPropertyName("cols")]
        public int? Columns { get; set; }

        [JsonPropertyName("convertEol")]
        public bool? ConvertEOL { get; set; }

        [JsonPropertyName("cursorBlink")]
        public bool? CursorBlink { get; set; }

        [JsonPropertyName("cursorStyle")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CursorStyle? CursorStyle { get; set; }

        [JsonPropertyName("disableStdin")]
        public bool? DisableStdin { get; set; }

        [JsonPropertyName("drawBoldTextInBrightColors")]
        public bool? DrawBoldTextInBrightColors { get; set; }

        [JsonPropertyName("fastScrollModifier")]
        public FastScrollModifiers? FastScrollModifier { get; set; }

        [JsonPropertyName("fastScrollSensitivity")]
        public double? FastScrollSensitivity { get; set; }

        [JsonPropertyName("fontFamily")]
        public string? FontFamily { get; set; }

        [JsonPropertyName("fontSize")]
        public double? FontSize { get; set; }

        [JsonPropertyName("fontWeight")]
        public FontWeights? FontWeight { get; set; }

        [JsonPropertyName("fontWeightBold")]
        public FontWeights? FontWeightBold { get; set; }

        [JsonPropertyName("letterSpacing")]
        public double? LetterSpacing { get; set; }

        [JsonPropertyName("lineHeight")]
        public double? LineHeight { get; set; }

        [JsonPropertyName("logLevel")]
        public XTermLogLevels? LogLevel { get; set; }

        [JsonPropertyName("macOptionClickForcesSelection")]
        public bool? MacOptionClickForcesSelection { get; set; }

        [JsonPropertyName("macOptionIsMeta")]
        public bool? MacOptionIsMeta { get; set; }

        [JsonPropertyName("minimumContrastRatio")]
        public double? MinimumContrastRatio { get; set; }

        [JsonPropertyName("rendererType")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RendererType? RendererType { get; set; }

        [JsonPropertyName("rightClickSelectsWord")]
        public bool? RightClickSelectsWord { get; set; }

        [JsonPropertyName("rows")]
        public int? Rows { get; set; }

        [JsonPropertyName("screenReaderMode")]
        public bool? ScreenReaderMode { get; set; }

        [JsonPropertyName("scrollSensitivity")]
        public double? ScrollSensitivity { get; set; }

        [JsonPropertyName("scrollback")]
        public double? Scrollback { get; set; }

        [JsonPropertyName("tabStopWidth")]
        public double? TabStopWidth { get; set; }

        //TODO:
        //[JsonPropertyName("theme")]
        //public object? Theme { get; set; }

        //[JsonPropertyName("windowOptions")]
        //public object? WindowOptions { get; set; }

        [JsonPropertyName("windowsMode")]
        public bool? WindowsMode { get; set; }

        [JsonPropertyName("wordSeparator")]
        public string? WordSeparator { get; set; }
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
