using Microsoft.Azure.Functions.Worker.Http;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace TaleLearnCode
{

	public static class HttpRequestDataExtensions
	{

		/// <summary>
		/// Creates a response with a body for the provided <see cref="HttpRequestData"/>.
		/// </summary>
		/// <param name="httpRequestData">The <see cref="HttpRequestData"/> for this response.</param>
		/// <param name="responseObject">The object to add to the response body in JSON format.</param>
		/// <param name="httpStatusCode">The HTTP status code to return in the response.</param>
		/// <param name="noResponseObjectStatusCode">The HTTP status code to return when the <paramref name="responseObject"/> is default/null.</param>
		/// <returns>A <see cref="HttpResponseData"/> representing the response with the <paramref name="responseObject"/> packaged in the body.</returns>
		public static Task<HttpResponseData> CreateResponseAsync(
			this HttpRequestData httpRequestData,
			object responseObject,
			HttpStatusCode httpStatusCode = HttpStatusCode.OK,
			HttpStatusCode noResponseObjectStatusCode = HttpStatusCode.NotFound)
		{
			return CreateResponseAsync(
				httpRequestData,
				responseObject,
				new JsonSerializerOptions(),
				httpStatusCode,
				noResponseObjectStatusCode);
		}

		/// <summary>
		/// Creates a response with a body for the provided <see cref="HttpRequestData"/>.
		/// </summary>
		/// <param name="httpRequestData">The <see cref="HttpRequestData"/> for this response.</param>
		/// <param name="responseObject">The object to add to the response body in JSON format.</param>
		/// <param name="jsonSerializerOptions">The serializer options for formatting the response body JSON.</param>
		/// <param name="httpStatusCode">The HTTP status code to return in the response.</param>
		/// <param name="noResponseObjectStatusCode">The HTTP status code to return when the <paramref name="responseObject"/> is default/null.</param>
		/// <returns>A <see cref="HttpResponseData"/> representing the response with the <paramref name="responseObject"/> packaged in the body.</returns>
		public static async Task<HttpResponseData> CreateResponseAsync(
			this HttpRequestData httpRequestData,
			object responseObject,
			JsonSerializerOptions jsonSerializerOptions,
			HttpStatusCode httpStatusCode = HttpStatusCode.OK,
			HttpStatusCode noResponseObjectStatusCode = HttpStatusCode.NotFound)
		{

			HttpResponseData response;

			if (responseObject != default)
			{

				response = httpRequestData.CreateResponse(httpStatusCode);
				response.Headers.Add("Content-Type", "application/json; charset=utf-8");
				await response.WriteStringAsync(JsonSerializer.Serialize(responseObject, jsonSerializerOptions));
			}
			else
			{
				response = httpRequestData.CreateResponse(noResponseObjectStatusCode);
			}

			return response;


		}

		/// <summary>
		/// Creates an Internal Server Error response.
		/// </summary>
		/// <param name="httpRequestData">The <see cref="HttpRequestData"/> for this response.</param>
		/// <returns>A <see cref="HttpResponseData"/> with a status of Internal Server Error (500).</returns>
		public static HttpResponseData CreateErrorResponseAsync(this HttpRequestData httpRequestData)
		{
			return httpRequestData.CreateResponse(HttpStatusCode.InternalServerError);
		}

		/// <summary>
		/// Creates an Internal Server Error response with the exception message included.
		/// </summary>
		/// <param name="httpRequestData">The <see cref="HttpRequestData"/> for this response.</param>
		/// <param name="exception">The <see cref="Exception"/> causing the Internal Server Error to be returned.</param>
		/// <returns>A <see cref="HttpResponseData"/> with a status of Internal Server Error (500) and a response body with the message from <paramref name="exception"/>.</returns>
		public static HttpResponseData CreateErrorResponseAsync(this HttpRequestData httpRequestData, Exception exception)
		{
			if (exception == default) throw new ArgumentNullException(nameof(exception));
			HttpResponseData response = httpRequestData.CreateResponse(HttpStatusCode.InternalServerError);
			response.WriteString(exception.Message);
			return response;
		}

		/// <summary>
		/// Get a request object from the provide <see cref="HttpRequestData"/>.
		/// </summary>
		/// <typeparam name="T">The type of the request object to be returned.</typeparam>
		/// <param name="httpRequestData">The <see cref="HttpRequestData"/> to be interrogated for the request object.</param>
		/// <returns>A <typeparamref name="T"/> representing the request object from the request.</returns>
		/// <exception cref="HttpRequestDataException">Thrown if there is an error reading the request object from the request.</exception>
		public static Task<T> GetRequestParameters<T>(this HttpRequestData httpRequestData) where T : new()
		{
			return GetRequestParametersAsync<T>(httpRequestData, new Dictionary<string, string>(), new JsonSerializerOptions());
		}

		/// <summary>
		/// Get a request object from the provide <see cref="HttpRequestData"/>.
		/// </summary>
		/// <typeparam name="T">The type of the request object to be returned.</typeparam>
		/// <param name="httpRequestData">The <see cref="HttpRequestData"/> to be interrogated for the request object.</param>
		/// <param name="routeValues">Any route values supplied to the Azure Function.</param>
		/// <returns>A <typeparamref name="T"/> representing the request object from the request.</returns>
		/// <exception cref="HttpRequestDataException">Thrown if there is an error reading the request object from the request.</exception>
		public static Task<T> GetRequestParameters<T>(this HttpRequestData httpRequestData, Dictionary<string, string> routeValues) where T : new()
		{
			return GetRequestParametersAsync<T>(httpRequestData, routeValues, new JsonSerializerOptions());
		}

		/// <summary>
		/// Get a request object from the provide <see cref="HttpRequestData"/>.
		/// </summary>
		/// <typeparam name="T">The type of the request object to be returned.</typeparam>
		/// <param name="httpRequestData">The <see cref="HttpRequestData"/> to be interrogated for the request object.</param>
		/// <param name="routeValues">Any route values supplied to the Azure Function.</param>
		/// <param name="jsonSerializerOptions">The serializer options for formatting the request body.</param>
		/// <returns>A <typeparamref name="T"/> representing the request object from the request.</returns>
		/// <exception cref="HttpRequestDataException">Thrown if there is an error reading the request object from the request.</exception>
		public static async Task<T> GetRequestParametersAsync<T>(
			this HttpRequestData httpRequestData,
			Dictionary<string, string> routeValues,
			JsonSerializerOptions jsonSerializerOptions) where T : new()
		{

			string queryString = httpRequestData.Url.Query;
			NameValueCollection queryValues = HttpUtility.ParseQueryString(queryString);
			bool queryValuesAvailalbe = (queryValues.Count == 1 && queryValues.GetKey(0).ToLower() != "code") || queryValues.Count > 1;

			if (httpRequestData.Body == Stream.Null && !queryValuesAvailalbe && (routeValues != default || !routeValues.Any()))
				throw new HttpRequestDataException("There are no query string values, no route values, or the request body is missing or it is unreadable.");

			T requestObject = default;

			if (httpRequestData.Body != Stream.Null)
			{
				requestObject = await JsonSerializer.DeserializeAsync<T>(httpRequestData.Body, jsonSerializerOptions);
				if (requestObject == null) throw new HttpRequestDataException("The request body is not correctly formatted.");
			}

			if (queryValuesAvailalbe || routeValues.Any())
				foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
					if (propertyInfo.CanWrite)
					{
						if (queryValuesAvailalbe)
							foreach (string key in queryValues.AllKeys)
								if (key.ToLower() == propertyInfo.Name.ToLower())
									if (propertyInfo.PropertyType == typeof(int) || propertyInfo.PropertyType == typeof(int?))
										propertyInfo.SetValue(requestObject, Convert.ToInt32(queryValues[key]));
									else
										propertyInfo.SetValue(requestObject, queryValues[key]);
						if (routeValues.TryGetValue(propertyInfo.Name.ToLower(), out string routeValue))
							if (propertyInfo.PropertyType == typeof(int) || propertyInfo.PropertyType == typeof(int?))
								propertyInfo.SetValue(requestObject, Convert.ToInt32(routeValue));
							else
								propertyInfo.SetValue(requestObject, routeValue);
					}

			if (requestObject == null) throw new HttpRequestDataException("The request body is not correctly formatted.");

			return requestObject;

		}

	}

}