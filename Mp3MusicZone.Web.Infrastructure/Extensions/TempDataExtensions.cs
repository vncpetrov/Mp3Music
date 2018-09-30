namespace Mp3MusicZone.Web.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using System;

    using static Mp3MusicZone.Common.Constants.WebConstants;
    
    public static class TempDataExtensions
    {
        [Obsolete]
        public static void AddSuccessMessage(
            this ITempDataDictionary tempData,
            string message)
        {
            tempData[TempDataSuccessMessageKey] = message;
        }

        [Obsolete]
        public static void AddErrorMessage(
            this ITempDataDictionary tempData,
            string message)
        {
            tempData[TempDataErrorMessageKey] = message;
        }
    }
}
