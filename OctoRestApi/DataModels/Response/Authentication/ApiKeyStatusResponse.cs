// OctoRestApi copyright 2022 Danny Glover.

using Newtonsoft.Json;

namespace OctoRestApi.DataModels.Response.Authentication;

public class ApiKeyStatusResponse
{
	[JsonProperty("api_key")] public string? ApiKey { get; set; }
}