namespace Mp3MusicZone.Common.Constants
{
    using System;

    public static class ModelConstants
    {
        public const int NameMinLength = 2;
        public const int NameMaxLength = 100;

        public const int UsernameMinLength = 3;
        public const int UsernameMaxLength = 20;

        public const int PasswordMinLength = 6;
        public const int PasswordMaxLength = 100;

        public const int UserMinAge = 16;

        public const string MinLengthErrorMessage =
            "The {0} must be at least {1} characters long.";

        public const string MaxLengthErrorMessage =
            "The {0} must be at max {1} characters long.";

        public const string MinAgeErrorMessage = "Your age does not meet the requirements to create an account.";
    }
}
