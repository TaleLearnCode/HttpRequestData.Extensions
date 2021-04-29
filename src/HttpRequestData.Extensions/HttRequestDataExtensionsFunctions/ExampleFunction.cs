using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using TaleLearnCode;

namespace HttRequestDataExtensionsFunctions
{
	public static class ExampleFunction
	{
		[Function("ExampleFunction")]
		public static async Task<HttpResponseData> RunAsync(
			[HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData request,
			FunctionContext executionContext)
		{
			var logger = executionContext.GetLogger("ExampleFunction");
			logger.LogInformation("C# HTTP trigger function processed a request.");

			ExampleRequest exampleRequest = await request.GetRequestParametersAsync<ExampleRequest>(new Dictionary<string, string>(), new JsonSerializerOptions());

			ExampleResponse exampleResponse = new()
			{
				FirstName = exampleRequest.FirstName,
				LastName = exampleRequest.LastName
			};

			HttpResponseData response = await request.CreateResponseAsync(exampleResponse);

			return response;
		}
	}
}
