using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using TaleLearnCode;

namespace HttRequestDataExtensionsFunctions
{
	public class ExampleFunction
	{

		private readonly JsonSerializerOptions _jsonSerializerOptions;


		public ExampleFunction(JsonSerializerOptions jsonSerializerOptions)
		{
			_jsonSerializerOptions = jsonSerializerOptions;
		}


		[Function("WithoutRouteValues")]
		public async Task<HttpResponseData> WithoutRouteValuesAsync(
			[HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData request,
			FunctionContext executionContext)
		{

			var logger = executionContext.GetLogger("ExampleFunction");
			logger.LogInformation("C# HTTP trigger function processed a request.");


			HttpResponseData response = default;
			try
			{
				ExampleRequest exampleRequest = await request.GetRequestParametersAsync<ExampleRequest>(_jsonSerializerOptions);


				// This is to show creating a bad request response by catching the ArgumentMissingException
				if (string.IsNullOrWhiteSpace(exampleRequest.FirstName)) throw new ArgumentMissingException(nameof(exampleRequest.FirstName));
				if (string.IsNullOrWhiteSpace(exampleRequest.LastName)) throw new ArgumentMissingException(nameof(exampleRequest.LastName));

				ExampleResponse exampleResponse = new()
				{
					FirstName = exampleRequest.FirstName,
					LastName = exampleRequest.LastName
				};

				response = await request.CreateResponseAsync(exampleResponse);

			}
			catch (ArgumentMissingException ex)
			{
				response = request.CreateBadRequestResponse(ex);
			}
			catch (HttpRequestDataException ex)
			{
				response = request.CreateBadRequestResponse(ex);
			}
			catch (Exception ex)
			{
				response = request.CreateErrorResponse();
			}


			return response;
		}

		[Function("WithRouteValues")]
		public async Task<HttpResponseData> WithRouteValuesAsync(
			[HttpTrigger(AuthorizationLevel.Function, "get", Route = "Example/{firstName}/{lastName}")] HttpRequestData request,
			string firstName,
			string lastName,
			FunctionContext executionContext)
		{

			var logger = executionContext.GetLogger("ExampleFunction");
			logger.LogInformation("C# HTTP trigger function processed a request.");

			Dictionary<string, string> routeValues = new()
			{
				{ nameof(firstName), firstName },
				{ nameof(lastName), lastName }
			};


			HttpResponseData response = default;
			try
			{
				ExampleRequest exampleRequest = await request.GetRequestParametersAsync<ExampleRequest>(routeValues);


				// This is to show creating a bad request response by catching the ArgumentMissingException
				if (string.IsNullOrWhiteSpace(exampleRequest.FirstName)) throw new ArgumentMissingException(nameof(exampleRequest.FirstName));
				if (string.IsNullOrWhiteSpace(exampleRequest.LastName)) throw new ArgumentMissingException(nameof(exampleRequest.LastName));

				ExampleResponse exampleResponse = new()
				{
					FirstName = exampleRequest.FirstName,
					LastName = exampleRequest.LastName
				};

				response = await request.CreateResponseAsync(exampleResponse);

			}
			catch (ArgumentMissingException ex)
			{
				response = request.CreateBadRequestResponse(ex);
			}
			catch (HttpRequestDataException ex)
			{
				response = request.CreateBadRequestResponse(ex);
			}
			catch (Exception ex)
			{
				response = request.CreateErrorResponse();
			}


			return response;
		}


	}
}
