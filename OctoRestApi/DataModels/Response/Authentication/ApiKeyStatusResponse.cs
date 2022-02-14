// OctoRestApi copyright 2022 Danny Glover.

using Newtonsoft.Json;

namespace OctoRestApi.DataModels.Response.Authentication;

public class ApiKeyStatusResponse
{
	public HttpResponseMessage? HttpMessage { get; set; }
	public DataModel? Data { get; set; }

	public class DataModel
	{
		[JsonProperty("api_key")] public string? ApiKey { get; set; }
	}
}
