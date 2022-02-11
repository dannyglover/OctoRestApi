// OctoRestApi copyright 2022 Danny Glover.

using Newtonsoft.Json;

namespace OctoRestApi.DataModels.Request.Authentication;

public class LoginRequest
{
	[JsonProperty("user")] public string? Username { get; set; }
	[JsonProperty("pass")] public string? Password { get; set; }
	[JsonProperty("remember")] public bool RememberLogin { get; set; }
}