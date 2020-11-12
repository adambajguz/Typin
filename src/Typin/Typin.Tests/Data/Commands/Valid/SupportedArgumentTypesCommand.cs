namespace Typin.Tests.Data.Commands.Valid
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Typin.Attributes;
    using Typin.Tests.Data.Commands;
    using Typin.Tests.Data.CustomTypes.Initializable;

    [Command("cmd")]
    [SuppressMessage("Performance", "CA1819:Properties should not return arrays")]
    public class SupportedArgumentTypesCommand : SelfSerializeCommandBase
    {
        [CommandOption("obj")]
        public object? Object { get; set; } = 42;

        [CommandOption("str")]
        public string? String { get; set; } = "foo bar";

        #region Simple types
        [CommandOption("bool")]
        public bool Bool { get; set; }

        [CommandOption("char")]
        public char Char { get; set; }

        [CommandOption("sbyte")]
        public sbyte Sbyte { get; set; }

        [CommandOption("byte")]
        public byte Byte { get; set; }

        [CommandOption("short")]
        public short Short { get; set; }

        [CommandOption("ushort")]
        public ushort Ushort { get; set; }

        [CommandOption("int")]
        public int Int { get; set; }

        [CommandOption("uint")]
        public uint Uint { get; set; }

        [CommandOption("long")]
        public long Long { get; set; }

        [CommandOption("ulong")]
        public ulong Ulong { get; set; }

        [CommandOption("float")]
        public float Float { get; set; }

        [CommandOption("double")]
        public double Double { get; set; }

        [CommandOption("decimal")]
        public decimal Decimal { get; set; }

        [CommandOption("datetime")]
        public DateTime DateTime { get; set; }

        [CommandOption("datetime-offset")]
        public DateTimeOffset DateTimeOffset { get; set; }

        [CommandOption("timespan")]
        public TimeSpan TimeSpan { get; set; }
        #endregion

        #region Simple nullable types
        [CommandOption("bool-nullable")]
        public bool? BoolNullabe { get; set; }

        [CommandOption("char-nullable")]
        public char? CharNullable { get; set; }

        [CommandOption("sbyte-nullable")]
        public sbyte? SbyteNullable { get; set; }

        [CommandOption("byte-nullable")]
        public byte? ByteNullable { get; set; }

        [CommandOption("short-nullable")]
        public short? ShortNullable { get; set; }

        [CommandOption("ushort-nullable")]
        public ushort? UshortNullable { get; set; }

        [CommandOption("int-nullable")]
        public int? IntNullable { get; set; }

        [CommandOption("uint-nullable")]
        public uint? UintNullable { get; set; }

        [CommandOption("long-nullable")]
        public long? LongNullable { get; set; }

        [CommandOption("ulong-nullable")]
        public ulong? UlongNullable { get; set; }

        [CommandOption("float-nullable")]
        public float? FloatNullable { get; set; }

        [CommandOption("double-nullable")]
        public double? DoubleNullable { get; set; }

        [CommandOption("decimal-nullable")]
        public decimal? DecimalNullable { get; set; }

        [CommandOption("datetime-nullable")]
        public DateTime? DateTimeNullable { get; set; }

        [CommandOption("datetime-offset-nullable")]
        public DateTimeOffset? DateTimeOffsetNullable { get; set; }

        [CommandOption("timespan-nullable")]
        public TimeSpan? TimeSpanNullable { get; set; }
        #endregion

        #region Custom enum
        [CommandOption("enum")]
        public CustomEnum Enum { get; set; }

        [CommandOption("enum-nullable")]
        public CustomEnum? EnumNullable { get; set; }
        #endregion

        #region Parasable or constructible
        [CommandOption("str-constructible")]
        public CustomStringConstructible? StringConstructible { get; set; }

        [CommandOption("str-parseable")]
        public CustomStringParseable? StringParseable { get; set; }

        [CommandOption("str-parseable-format")]
        public CustomStringParseableWithFormatProvider? StringParseableWithFormatProvider { get; set; }
        #endregion

        #region Collections
        [CommandOption("obj-array")]
        public object[]? ObjectArray { get; set; }

        [CommandOption("str-array")]
        public string[]? StringArray { get; set; }

        [CommandOption("int-array")]
        public int[]? IntArray { get; set; }

        [CommandOption("enum-array")]
        public CustomEnum[]? EnumArray { get; set; }

        [CommandOption("int-nullable-array")]
        public int?[]? IntNullableArray { get; set; }

        [CommandOption("str-constructible-array")]
        public CustomStringConstructible[]? StringConstructibleArray { get; set; }

        [CommandOption("str-enumerable")]
        public IEnumerable<string>? StringEnumerable { get; set; }

        [CommandOption("str-read-only-list")]
        public IReadOnlyList<string>? StringReadOnlyList { get; set; }

        [CommandOption("str-list")]
        public List<string>? StringList { get; set; }

        [CommandOption("str-set")]
        public HashSet<string>? StringHashSet { get; set; }
        #endregion
    }
}