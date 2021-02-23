namespace Typin.Tests.Data.Commands.Valid
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Newtonsoft.Json;
    using Typin.Attributes;
    using Typin.Tests.Data.Commands;
    using Typin.Tests.Data.CustomTypes.Initializable;

    [Command("cmd")]
    [SuppressMessage("Performance", "CA1819:Properties should not return arrays")]
    public class SupportedArgumentTypesCommand : SelfSerializeCommandBase
    {
        [JsonProperty("obj")]
        [CommandOption("obj")]
        public object? Object { get; init; } = 42;

        [JsonProperty("str")]
        [CommandOption("str")]
        public string? String { get; init; } = "foo bar";

        #region Simple types
        [JsonProperty("bool")]
        [CommandOption("bool")]
        public bool Bool { get; init; }

        [JsonProperty("char")]
        [CommandOption("char")]
        public char Char { get; init; }

        [JsonProperty("sbyte")]
        [CommandOption("sbyte")]
        public sbyte Sbyte { get; init; }

        [JsonProperty("byte")]
        [CommandOption("byte")]
        public byte Byte { get; init; }

        [JsonProperty("short")]
        [CommandOption("short")]
        public short Short { get; init; }

        [JsonProperty("ushort")]
        [CommandOption("ushort")]
        public ushort Ushort { get; init; }

        [JsonProperty("int")]
        [CommandOption("int")]
        public int Int { get; init; }

        [JsonProperty("uint")]
        [CommandOption("uint")]
        public uint Uint { get; init; }

        [JsonProperty("long")]
        [CommandOption("long")]
        public long Long { get; init; }

        [JsonProperty("ulong")]
        [CommandOption("ulong")]
        public ulong Ulong { get; init; }

        //[JsonProperty("half")]
        //[CommandOption("half")]
        //public Half Half { get; init; } = (Half)98.1245;

        [JsonProperty("float")]
        [CommandOption("float")]
        public float Float { get; init; }

        [JsonProperty("double")]
        [CommandOption("double")]
        public double Double { get; init; }

        [JsonProperty("decimal")]
        [CommandOption("decimal")]
        public decimal Decimal { get; init; }

        [JsonProperty("datetime")]
        [CommandOption("datetime")]
        public DateTime DateTime { get; init; }

        [JsonProperty("datetime-offset")]
        [CommandOption("datetime-offset")]
        public DateTimeOffset DateTimeOffset { get; init; }

        [JsonProperty("timespan")]
        [CommandOption("timespan")]
        public TimeSpan TimeSpan { get; init; }
        #endregion

        #region Simple nullable types
        [JsonProperty("bool-nullable")]
        [CommandOption("bool-nullable")]
        public bool? BoolNullabe { get; init; } = true;

        [JsonProperty("char-nullable")]
        [CommandOption("char-nullable")]
        public char? CharNullable { get; init; } = 'a';

        [JsonProperty("sbyte-nullable")]
        [CommandOption("sbyte-nullable")]
        public sbyte? SbyteNullable { get; init; } = -1;

        [JsonProperty("byte-nullable")]
        [CommandOption("byte-nullable")]
        public byte? ByteNullable { get; init; } = 1;

        [JsonProperty("short-nullable")]
        [CommandOption("short-nullable")]
        public short? ShortNullable { get; init; } = -18;

        [JsonProperty("ushort-nullable")]
        [CommandOption("ushort-nullable")]
        public ushort? UshortNullable { get; init; } = 18;

        [JsonProperty("int-nullable")]
        [CommandOption("int-nullable")]
        public int? IntNullable { get; init; } = -18;

        [JsonProperty("uint-nullable")]
        [CommandOption("uint-nullable")]
        public uint? UintNullable { get; init; } = 18;

        [JsonProperty("long-nullable")]
        [CommandOption("long-nullable")]
        public long? LongNullable { get; init; } = -180;

        [JsonProperty("ulong-nullable")]
        [CommandOption("ulong-nullable")]
        public ulong? UlongNullable { get; init; } = 180;

        //[JsonProperty("half-nullable")]
        //[CommandOption("half-nullable")]
        //public Half? HalfNullable { get; init; } = (Half)98.1245;

        [JsonProperty("float-nullable")]
        [CommandOption("float-nullable")]
        public float? FloatNullable { get; init; } = 98.1245f;

        [JsonProperty("double-nullable")]
        [CommandOption("double-nullable")]
        public double? DoubleNullable { get; init; } = 98.1245d;

        [JsonProperty("decimal-nullable")]
        [CommandOption("decimal-nullable")]
        public decimal? DecimalNullable { get; init; } = 98.1245M;

        [JsonProperty("datetime-nullable")]
        [CommandOption("datetime-nullable")]
        public DateTime? DateTimeNullable { get; init; } = new DateTime(1898, 10, 20);

        [JsonProperty("datetime-offset-nullable")]
        [CommandOption("datetime-offset-nullable")]
        public DateTimeOffset? DateTimeOffsetNullable { get; init; } = DateTime.UnixEpoch;

        [JsonProperty("timespan-nullable")]
        [CommandOption("timespan-nullable")]
        public TimeSpan? TimeSpanNullable { get; init; } = new TimeSpan(1, 10, 30);
        #endregion

        #region Custom enum
        [JsonProperty("enum")]
        [CommandOption("enum")]
        public CustomEnum Enum { get; init; }

        [JsonProperty("enum-nullable")]
        [CommandOption("enum-nullable")]
        public CustomEnum? EnumNullable { get; init; } = CustomEnum.Value1;

        [JsonProperty("enum-array")]
        [CommandOption("enum-array")]
        public CustomEnum[]? EnumArray { get; init; }
        #endregion

        #region Parasable or constructible
        [JsonProperty("str-constructible")]
        [CommandOption("str-constructible")]
        public CustomStringConstructible? StringConstructible { get; init; }

        [JsonProperty("str-parsable")]
        [CommandOption("str-parsable")]
        public CustomStringParsable? StringParsable { get; init; }

        [JsonProperty("str-parsable-format")]
        [CommandOption("str-parsable-format")]
        public CustomStringParsableWithFormatProvider? StringParsableWithFormatProvider { get; init; }

        [JsonProperty("str-constructible-array")]
        [CommandOption("str-constructible-array")]
        public CustomStringConstructible[]? StringConstructibleArray { get; init; }
        #endregion

        #region Collections
        [JsonProperty("obj-array")]
        [CommandOption("obj-array")]
        public object[]? ObjectArray { get; init; }

        [JsonProperty("str-array")]
        [CommandOption("str-array")]
        public string[]? StringArray { get; init; }

        [JsonProperty("str-enumerable")]
        [CommandOption("str-enumerable")]
        public IEnumerable<string>? StringEnumerable { get; init; }

        [JsonProperty("str-read-only-list")]
        [CommandOption("str-read-only-list")]
        public IReadOnlyList<string>? StringReadOnlyList { get; init; }

        [JsonProperty("str-list")]
        [CommandOption("str-list")]
        public List<string>? StringList { get; init; }

        [JsonProperty("str-set")]
        [CommandOption("str-set")]
        public HashSet<string>? StringHashSet { get; init; }

        [JsonProperty("int-array")]
        [CommandOption("int-array")]
        public int[]? IntArray { get; init; }

        [JsonProperty("int-nullable-array")]
        [CommandOption("int-nullable-array")]
        public int?[]? IntNullableArray { get; init; }
        #endregion
    }
}