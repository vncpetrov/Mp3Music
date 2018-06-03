namespace Mp3MusicZone.Common.ValidationAttributes
{
    using Domain.Contracts;
    using Providers;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class MinAgeAttribute : ValidationAttribute
    {
        private readonly int minAge;
        private IDateTimeProvider dateTimeProvider;

        public MinAgeAttribute(int minAge)
        {
            this.minAge = minAge;
        }

        public IDateTimeProvider DateTimeService
        {
            get
            {
                if (this.dateTimeProvider is null)
                {
                    this.dateTimeProvider = new SystemDateTimeProvider();
                }

                return this.dateTimeProvider;
            }
            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                this.dateTimeProvider = value;
            }
        }

        public override bool IsValid(object value)
        {
            DateTime? valueAsDateTime = value as DateTime?;

            if (valueAsDateTime is null)
            {
                return true;
            }

            DateTime birthdate = valueAsDateTime.Value;
            DateTime currentDate = this.DateTimeService.UtcNow;

            int age = currentDate.Year - birthdate.Year;

            if (birthdate > currentDate.AddYears(-age))
            {
                age--;
            }

            return age >= this.minAge; 
        }
    }
}
