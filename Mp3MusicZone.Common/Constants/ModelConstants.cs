namespace Mp3MusicZone.Common.Constants
{
    using System;

    public static class ModelConstants
    {
        private const int MB = 1_048_576;

        public const int StringMinLength = 2;
        public const int StringMaxLength = 100;

        public const int UsernameMinLength = 3;
        public const int UsernameMaxLength = 20;

        public const int PasswordMinLength = 6;
        public const int PasswordMaxLength = 100;

        public const int UserMinAge = 16;

        public const int ProfileImageMaxLength = 5 * MB;

        public const int SongMaxMBs = 25;
        public const int SongMaxLength = SongMaxMBs * MB;
        public const int SongMinYear = 1950;

        public const string MinLengthErrorMessage =
            "The {0} must be at least {1} characters long.";

        public const string MaxLengthErrorMessage =
            "The {0} must be at max {1} characters long.";

        public const string MinAgeErrorMessage = 
            "Your age does not meet the requirements to create an account.";
    }
}
