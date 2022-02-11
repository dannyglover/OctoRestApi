// OctoRestApi copyright 2022 Danny Glover.

using Newtonsoft.Json;

namespace OctoRestApi.DataModels.Response.Authentication;

public class LoginResponse
{
	[JsonProperty("_is_external_client")] public bool IsExternalClient { get; set; }
	[JsonProperty("_login_mechanism")] public string? LoginMechanism { get; set; }
	[JsonProperty("active")] public bool Active { get; set; }
	[JsonProperty("admin")] public bool Admin { get; set; }
	[JsonProperty("apikey")] public object? ApiKey { get; set; }
	[JsonProperty("groups")] public string[]? Groups { get; set; }
	[JsonProperty("name")] public string? Name { get; set; }
	[JsonProperty("needs")] public Needs? Needs { get; set; }
	[JsonProperty("permissions")] public object[]? Permissions { get; set; }
	[JsonProperty("roles")] public string[]? Roles { get; set; }
	[JsonProperty("session")] public string? Session { get; set; }
	[JsonProperty("settings")] public Settings? Settings { get; set; }
	[JsonProperty("user")] public bool User { get; set; }
}

public class Needs
{
	[JsonProperty("group")] public string[]? Group { get; set; }
	[JsonProperty("role")] public string[]? Role { get; set; }
}

public class Settings
{
}