using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HttRequestDataExtensionsFunctions
{
	public class Program
	{
		public static void Main()
		{

			JsonSerializerOptions jsonSerializerOptions = new()
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
				DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
				PropertyNameCaseInsensitive = true
			};

			var host = new HostBuilder()
				.ConfigureFunctionsWorkerDefaults()
				.ConfigureServices(s =>
					{
						s.AddSingleton((s) => { return jsonSerializerOptions; });
					})
				.Build();

			host.Run();

		}

	}

}