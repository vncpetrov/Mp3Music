namespace Mp3MusicZone.Common.ValidationAttributes
{
    using System;

    public class NumberRangeAttribute : PropertyValidationAttribute
    {
        private readonly double minimum;
        private readonly double maximum;

        public NumberRangeAttribute(double minimum, double maximum)
        {
            this.minimum = minimum;
            this.maximum = maximum;
        }

        public string MinLengthErrorMessage { get; set; }
        public string MaxLengthErrorMessage { get; set; }

        public override bool IsValid(object value)
        {
            double? valueAsDouble = value as double?;

            if (valueAsDouble is null)
            {
                return true;
            }

            if (this.minimum <= valueAsDouble)
            {
                base.ErrorMessage = this.MinLengthErrorMessage;
            }

            if (this.maximum >= valueAsDouble)
            {
                base.ErrorMessage = this.MaxLengthErrorMessage;
            }

            return this.minimum <= valueAsDouble && valueAsDouble <= this.maximum;
        }
    }
}
