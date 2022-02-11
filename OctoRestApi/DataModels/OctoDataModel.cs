// OctoRestApi copyright 2022 Danny Glover.

using OctoRestApi.DataModels.Response.Authentication;
using OctoRestApi.DataModels.Response.PrintTool;

namespace OctoRestApi.DataModels;

public class OctoDataModel
{
	#region Authentication

	public LoginResponse? OctoLoginResponse { get; internal set; }
	public AppApiKeyResponse? OctoAppApiKeyResponse { get; internal set; }
	public ApiKeyStatusResponse? OctoApiKeyStatusResponse { get; internal set; }

	#endregion

	#region PrintTool

	public PrintToolResponse? OctoPrintToolResponse { get; internal set; }

	#endregion
}