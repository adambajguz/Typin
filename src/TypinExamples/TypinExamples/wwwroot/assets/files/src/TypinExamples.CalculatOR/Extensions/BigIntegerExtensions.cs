namespace TypinExamples.CalculatOR.Extensions
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Numerics;
    using System.Text;
    using System.Text.RegularExpressions;
    using TypinExamples.CalculatOR.Domain;

    public static class BigIntegerExtensions
    {
        public static BigInteger BinaryToBigInteger(this string value)
        {
            value = value.Replace("0b", string.Empty, true, CultureInfo.InvariantCulture);

            if (!Regex.IsMatch(value, @"^[0-1]+$"))
                throw new FormatException("String must only contain '0', '1', and optional '0b' or '0B' identifier to be converted from binary to BigInteger.");

            if (value.Count(b => b == '1') + value.Count(b => b == '0') != value.Length)
                return 0;

            BigInteger result = 0;
            foreach (char c in value)
            {
                result <<= 1;
                result += c == '1' ? 1 : 0;
            }

            return result;
        }

        public static string ToString(this BigInteger value, NumberBase numberBase)
        {
            return numberBase switch
            {
                NumberBase.BIN => value.ToBinaryString(),
                NumberBase.HEX => value.ToHexadecimalString(),
                _ => value.ToString(),
            };
        }

        //https://stackoverflow.com/questions/14048476/biginteger-to-hex-decimal-octal-binary-strings
        public static string ToBinaryString(this BigInteger bigint)
        {
            byte[] bytes = bigint.ToByteArray();
            int idx = bytes.Length - 1;

            // Create a StringBuilder having appropriate capacity.
            StringBuilder base2 = new StringBuilder(bytes.Length * 8);
            base2.Append("0b");

            // Convert first byte to binary.
            string binary = Convert.ToString(bytes[idx], 2);

            // Ensure leading zero exists if value is positive.
            if (binary[0] != '0' && bigint.Sign == 1)
            {
                base2.Append('0');
            }

            // Append binary string to StringBuilder.
            base2.Append(binary);

            // Convert remaining bytes adding leading zeros.
            for (idx--; idx >= 0; idx--)
            {
                base2.Append(Convert.ToString(bytes[idx], 2).PadLeft(8, '0'));
            }

            return base2.ToString();
        }

        public static string ToHexadecimalString(this BigInteger bigint)
        {
            return string.Concat("0x", bigint.ToString("X"));
        }
    }
}
