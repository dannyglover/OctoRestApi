// OctoRestApi copyright 2022 Danny Glover.

using Newtonsoft.Json;

namespace OctoRestApi.DataModels.Response.Authentication;

public class ApiKeyRequestResponse
{
	public HttpResponseMessage? HttpMessage { get; set; }
	public DataModel? Data { get; set; }

	public class DataModel
	{
		[JsonProperty("app_token")] public string? AppToken { get; set; }
	}
}
