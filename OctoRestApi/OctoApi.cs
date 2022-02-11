// OctoRestApi copyright 2022 Danny Glover.

using OctoRestApi.DataModels;
using OctoRestApi.Internal.Apis;

namespace OctoRestApi;

public class OctoApi
{
	internal HttpClient OctoHttpClient { get; }
	public OctoDataModel OctoDataModel { get; }
	private Authentication OctoAuthentication { get; }
	private PrinterTools OctoPrinterTools { get; }
	private string DebugMessagePrefix { get; }
	private string OctoprintUrl { get; set; }
	public bool DebugMode { get; set; }

	public OctoApi(string octoprintUrl)
	{
		DebugMessagePrefix = "OctoApi:";
		OctoprintUrl = octoprintUrl;
		OctoHttpClient = new HttpClient();
		OctoDataModel = new OctoDataModel();
		OctoAuthentication = new Authentication(this);
		OctoPrinterTools = new PrinterTools(this);
		OctoHttpClient.BaseAddress = new Uri(OctoprintUrl);
	}

	#region Authentication

	public async Task IssueAppApiKeyRequest(string appName, string username)
	{
		await OctoAuthentication.IssueAppApiKeyRequest(appName, username);
	}

	public async Task CheckAppApiKeyRequestStatus(string? appToken)
	{
		await OctoAuthentication.CheckAppApiKeyRequestStatus(appToken);
	}

	#endregion

	#region PrintTools

	public async Task SetTargetToolTemperature(IEnumerable<int> toolIndexes, int temperature)
	{
		await OctoPrinterTools.SetTargetTemperature(toolIndexes, temperature);
	}

	#endregion
}