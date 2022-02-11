// OctoRestApi copyright 2022 Danny Glover.

using Newtonsoft.Json;

namespace OctoRestApi.DataModels.Response.Authentication;

public class AppApiKeyResponse
{
	[JsonProperty("app_token")] public string? AppToken { get; set; }
}