namespace Mp3MusicZone.Web.Infrastructure
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Threading.Tasks;

    using static Mp3MusicZone.Common.Constants.WebConstants;

    public class WithMessageResult : IActionResult
    {
        private readonly IActionResult action;

        // can be exposed as properties for testing purposes
        private readonly string type;
        private readonly string message;

        public WithMessageResult(IActionResult action, string type, string message)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));

            this.action = action;
            this.type = type;
            this.message = message;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            ITempDataDictionaryFactory tempDataFactory = context.HttpContext
                .RequestServices
                .GetService<ITempDataDictionaryFactory>();

            ITempDataDictionary tempData = tempDataFactory.GetTempData(context.HttpContext);

            tempData[TempDataMessageTypeKey] = this.type;
            tempData[TempDataMessageTextKey] = this.message;

            await this.action.ExecuteResultAsync(context);
        }
    }
}
