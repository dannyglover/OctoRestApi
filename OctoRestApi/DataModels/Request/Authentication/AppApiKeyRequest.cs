// OctoRestApi copyright 2022 Danny Glover.

using Newtonsoft.Json;

namespace OctoRestApi.DataModels.Request.Authentication;

public class AppApiKeyRequest
{
	[JsonProperty("app")] public string? AppName { get; set; }
	[JsonProperty("user")] public string? Username { get; set; }
}