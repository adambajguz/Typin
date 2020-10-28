namespace TypinExamples.CalculatOR.Domain
{
    using System;

    public struct Number
    {
        public string Value { get; }

        public NumberBase Base { get; }

        public Number(string value, NumberBase @base)
        {
            Value = value;
            Base = @base;
        }

        public static Number Parser(string number)
        {
            throw new NotImplementedException();
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
