namespace TypinExamples.CalculatOR.Domain
{
    using Microsoft.Extensions.Primitives;
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Numerics;

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
            if (number.Length >= 2 && number[0] == '0' && char.ToLower(number[1]) == 'x')
            {
                bigInt = BigInteger.Parse(number, NumberStyles.HexNumber);
            return new Number(bigInt, NumberBase.Hexadecimal);
            }

            bigInt = BigInteger.Parse(number, NumberStyles.Integer);
            return new Number(bigInt, NumberBase.Decimal);

        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public static bool operator ==(Number left, Number right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Number left, Number right)
        {
            return !(left == right);
        }
    }
}
