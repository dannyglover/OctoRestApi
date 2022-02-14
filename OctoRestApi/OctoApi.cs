// OctoRestApi copyright 2022 Danny Glover.

using OctoRestApi.DataModels;
using OctoRestApi.DataModels.Response.Authentication;
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

	#region Value Accessors

	public string? GetApiKey()
	{
		return OctoDataModel.OctoApiKeyStatusResponse?.Data?.ApiKey;
	}

	#endregion

	#region Value Setters

	public void SetApiKey(string apiKey)
	{
		OctoDataModel.OctoApiKeyStatusResponse = new ApiKeyStatusResponse
		{
			HttpMessage = null,
			Data = new ApiKeyStatusResponse.DataModel
			{
				ApiKey = apiKey
			}
		};
	}

	#endregion

	#region Authentication

	public async Task Login(string username, string password)
	{
		await OctoAuthentication.Login(username, password);
	}

	public async Task ProbeForApiKeyWorkflowSupport()
	{
		await OctoAuthentication.ProbeForApiKeyWorkflowSupport();
	}

	public async Task IssueApiKeyRequest(string appName, string username)
	{
		await OctoAuthentication.IssueApiKeyRequest(appName, username);
	}

	public async Task CheckApiKeyRequestStatus(string? appToken)
	{
		await OctoAuthentication.CheckApiKeyRequestStatus(appToken);
	}

	#endregion

	#region PrintTools

	public async Task SetTargetToolTemperature(IEnumerable<int> toolIndexes, int temperature)
	{
		await OctoPrinterTools.SetTargetTemperature(toolIndexes, temperature);
	}

	#endregion
}
