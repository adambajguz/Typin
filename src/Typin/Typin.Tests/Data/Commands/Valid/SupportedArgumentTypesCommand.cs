namespace Typin.Tests.Data.Commands.Valid
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Tests.Data.Commands;
    using Typin.Tests.Data.CustomTypes.Initializable;

    [Command("cmd")]
    public class SupportedArgumentTypesCommand : SelfSerializeCommandBase
    {
        [JsonProperty("obj")]
        [Option("obj")]
        public object? Object { get; init; } = 42;

        [JsonProperty("str")]
        [Option("str")]
        public string? String { get; init; } = "foo bar";

        #region Simple types
        [JsonProperty("bool")]
        [Option("bool")]
        public bool Bool { get; init; }

        [JsonProperty("char")]
        [Option("char")]
        public char Char { get; init; }

        [JsonProperty("sbyte")]
        [Option("sbyte")]
        public sbyte Sbyte { get; init; }

        [JsonProperty("byte")]
        [Option("byte")]
        public byte Byte { get; init; }

        [JsonProperty("short")]
        [Option("short")]
        public short Short { get; init; }

        [JsonProperty("ushort")]
        [Option("ushort")]
        public ushort Ushort { get; init; }

        [JsonProperty("int")]
        [Option("int")]
        public int Int { get; init; }

        [JsonProperty("uint")]
        [Option("uint")]
        public uint Uint { get; init; }

        [JsonProperty("long")]
        [Option("long")]
        public long Long { get; init; }

        [JsonProperty("ulong")]
        [Option("ulong")]
        public ulong Ulong { get; init; }

        //[JsonProperty("half")]
        //[Option("half")]
        //public Half Half { get; init; } = (Half)98.1245;

        [JsonProperty("float")]
        [Option("float")]
        public float Float { get; init; }

        [JsonProperty("double")]
        [Option("double")]
        public double Double { get; init; }

        [JsonProperty("decimal")]
        [Option("decimal")]
        public decimal Decimal { get; init; }

        [JsonProperty("guid")]
        [Option("guid")]
        public Guid Guid { get; init; }

        [JsonProperty("datetime")]
        [Option("datetime")]
        public DateTime DateTime { get; init; }

        [JsonProperty("datetime-offset")]
        [Option("datetime-offset")]
        public DateTimeOffset DateTimeOffset { get; init; }

        [JsonProperty("timespan")]
        [Option("timespan")]
        public TimeSpan TimeSpan { get; init; }
        #endregion

        #region Simple nullable types
        [JsonProperty("bool-nullable")]
        [Option("bool-nullable")]
        public bool? BoolNullabe { get; init; } = true;

        [JsonProperty("char-nullable")]
        [Option("char-nullable")]
        public char? CharNullable { get; init; } = 'a';

        [JsonProperty("sbyte-nullable")]
        [Option("sbyte-nullable")]
        public sbyte? SbyteNullable { get; init; } = -1;

        [JsonProperty("byte-nullable")]
        [Option("byte-nullable")]
        public byte? ByteNullable { get; init; } = 1;

        [JsonProperty("short-nullable")]
        [Option("short-nullable")]
        public short? ShortNullable { get; init; } = -18;

        [JsonProperty("ushort-nullable")]
        [Option("ushort-nullable")]
        public ushort? UshortNullable { get; init; } = 18;

        [JsonProperty("int-nullable")]
        [Option("int-nullable")]
        public int? IntNullable { get; init; } = -18;

        [JsonProperty("uint-nullable")]
        [Option("uint-nullable")]
        public uint? UintNullable { get; init; } = 18;

        [JsonProperty("long-nullable")]
        [Option("long-nullable")]
        public long? LongNullable { get; init; } = -180;

        [JsonProperty("ulong-nullable")]
        [Option("ulong-nullable")]
        public ulong? UlongNullable { get; init; } = 180;

        //[JsonProperty("half-nullable")]
        //[Option("half-nullable")]
        //public Half? HalfNullable { get; init; } = (Half)98.1245;

        [JsonProperty("float-nullable")]
        [Option("float-nullable")]
        public float? FloatNullable { get; init; } = 98.1245f;

        [JsonProperty("double-nullable")]
        [Option("double-nullable")]
        public double? DoubleNullable { get; init; } = 98.1245d;

        [JsonProperty("decimal-nullable")]
        [Option("decimal-nullable")]
        public decimal? DecimalNullable { get; init; } = 98.1245M;

        [JsonProperty("guid-nullable")]
        [Option("guid-nullable")]
        public Guid? GuidNullable { get; init; } = (Guid?)Guid.Parse("{12345678-abcd-2222-3333-111111111111}");

        [JsonProperty("datetime-nullable")]
        [Option("datetime-nullable")]
        public DateTime? DateTimeNullable { get; init; } = new(1898, 10, 20);

        [JsonProperty("datetime-offset-nullable")]
        [Option("datetime-offset-nullable")]
        public DateTimeOffset? DateTimeOffsetNullable { get; init; } = DateTime.UnixEpoch;

        [JsonProperty("timespan-nullable")]
        [Option("timespan-nullable")]
        public TimeSpan? TimeSpanNullable { get; init; } = new(1, 10, 30);
        #endregion

        #region Custom enum
        [JsonProperty("enum")]
        [Option("enum")]
        public CustomEnum Enum { get; init; }

        [JsonProperty("enum-nullable")]
        [Option("enum-nullable")]
        public CustomEnum? EnumNullable { get; init; } = CustomEnum.Value1;

        [JsonProperty("enum-array")]
        [Option("enum-array")]
        public CustomEnum[]? EnumArray { get; init; }
        #endregion

        #region Parasable or constructible
        [JsonProperty("str-constructible")]
        [Option("str-constructible")]
        public CustomStringConstructible? StringConstructible { get; init; }

        [JsonProperty("str-parsable")]
        [Option("str-parsable")]
        public CustomStringParsable? StringParsable { get; init; }

        [JsonProperty("str-parsable-format")]
        [Option("str-parsable-format")]
        public CustomStringParsableWithFormatProvider? StringParsableWithFormatProvider { get; init; }

        [JsonProperty("str-constructible-array")]
        [Option("str-constructible-array")]
        public CustomStringConstructible[]? StringConstructibleArray { get; init; }
        #endregion

        #region Collections
        [JsonProperty("obj-array")]
        [Option("obj-array")]
        public object[]? ObjectArray { get; init; }

        [JsonProperty("str-array")]
        [Option("str-array")]
        public string[]? StringArray { get; init; }

        [JsonProperty("str-enumerable")]
        [Option("str-enumerable")]
        public IEnumerable<string>? StringEnumerable { get; init; }

        [JsonProperty("str-read-only-list")]
        [Option("str-read-only-list")]
        public IReadOnlyList<string>? StringReadOnlyList { get; init; }

        [JsonProperty("str-list")]
        [Option("str-list")]
        public List<string>? StringList { get; init; }

        [JsonProperty("str-set")]
        [Option("str-set")]
        public HashSet<string>? StringHashSet { get; init; }

        [JsonProperty("int-array")]
        [Option("int-array")]
        public int[]? IntArray { get; init; }

        [JsonProperty("int-nullable-array")]
        [Option("int-nullable-array")]
        public int?[]? IntNullableArray { get; init; }
        #endregion

        public SupportedArgumentTypesCommand(IConsole console) : base(console)
        {

        }
    }
}