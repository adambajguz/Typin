namespace TypinExamples.CalculatOR.Domain
{
    using System;
    using System.Globalization;
    using System.Numerics;
    using TypinExamples.CalculatOR.Extensions;

    public struct Number
    {
        public BigInteger Value { get; }

        public NumberBase Base { get; }

        public Number(BigInteger value, NumberBase @base)
        {
            Value = value;
            Base = @base;
        }

        public static Number Parse(string number)
        {
            BigInteger bigInt;
            if (number.Length >= 2 && number[0] == '0')
            {
                if (char.ToLower(number[1]) == 'x')
                {
                    string tmp = number.Replace("0x", string.Empty, true, CultureInfo.InvariantCulture);
                    bigInt = BigInteger.Parse(tmp, NumberStyles.HexNumber);

                    return new Number(bigInt, NumberBase.HEX);
                }
                else if (char.ToLower(number[1]) == 'b')
                {
                    bigInt = number.BinaryToBigInteger();

                    return new Number(bigInt, NumberBase.BIN);
                }
            }

            bigInt = BigInteger.Parse(number, NumberStyles.Integer);

            return new Number(bigInt, NumberBase.DEC);
        }

        public override bool Equals(object? obj)
        {
            return obj is Number num ? num.Value == Value && num.Base == Base : false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value, Base);
        }

        public override string ToString()
        {
            return $"[{Base}: {Value}]";
        }

        public static bool operator ==(Number left, Number right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Number left, Number right)
        {
            return !left.Equals(right);
        }
    }
}
