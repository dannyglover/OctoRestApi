// OctoRestApi copyright 2022 Danny Glover.

using Newtonsoft.Json;

namespace OctoRestApi.DataModels.Request.PrintTool;

public class PrintToolRequest
{
	[JsonProperty("command")] public string? Command { get; set; }
	[JsonProperty("targets")] public object? Targets { get; set; }
}