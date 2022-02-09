// OctoRestApi copyright 2022 Danny Glover.

using Newtonsoft.Json;

namespace OctoRestApi.DataModels.Request;

public class LoginRequestDataModel
{
    [JsonProperty("user")] public string? Username { get; set; }
    [JsonProperty("pass")] public string? Password { get; set; }
}