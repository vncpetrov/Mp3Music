namespace Mp3MusicZone.Common.ValidationAttributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyValidationAttribute : ValidationAttribute
    { 
    }
}
