// OctoRestApi copyright 2022 Danny Glover.

using System.Net;
using Newtonsoft.Json;
using OctoRestApi.DataModels.Request.Authentication;
using OctoRestApi.DataModels.Response.Authentication;

namespace OctoRestApi.Internal.Apis;

internal class Authentication : Api
{
	private const string ApiKeyProbeEndpoint = $@"{Endpoint.Plugin}/appkeys/probe";
	private const string ApiKeyRequestEndpoint = $@"{Endpoint.Plugin}/appkeys/request";

	public Authentication(OctoApi octoApi) : base(octoApi)
	{
		DebugMessagePrefix = "Authentication ";
	}

	#region Login

	/// <summary>
	/// Issues a login request to the Octoprint server
	/// </summary>
	/// <param name="username">The users octoprint username</param>
	/// <param name="password">The users octoprint password</param>
	/// <remarks>
	/// <para />Endpoint: octoprint_url/api/login
	/// <para />Request Type: POST
	/// <para />
	/// HTTP Headers:
	///     Content-Type: application/json
	/// <para />
	/// HTTP Body (required):
	/// user, pass
	/// </remarks>
	///
	public async Task Login(string username, string password)
	{
		// setup route information for this api
		const string route = $"{Endpoint.Api}/login";

		// setup the request data
		var requestData = new LoginRequest()
		{
			Username = username,
			Password = password,
			RememberLogin = true
		};

		// issue the request
		var webResponse = await IssueRequest(route, WebRequestMethods.Http.Post, requestData);

		// assign the response data model
		if (webResponse != null)
		{
			OctoDataModel.OctoLoginResponse = new LoginResponse
			{
				HttpMessage = webResponse.ResponseMessage,
				Data = !string.IsNullOrEmpty(webResponse.JsonString)
					? JsonConvert.DeserializeObject<LoginResponse.DataModel>(webResponse.JsonString)
					: null
			};
		}
	}

	#endregion

	#region Probe For Workflow Support

	/// <summary>
	/// Issues a request to probe for ApiKey workflow support to the Octoprint server
	/// </summary>
	/// <remarks>
	/// <para />Endpoint: octoprint_url/plugin/appkeys/probe
	/// <para />Request Type: GET
	/// <para />
	/// HTTP Headers:
	///     Content-Type: application/json
	/// </remarks>
	///
	public async Task ProbeForApiKeyWorkflowSupport()
	{
		// setup route information for this api
		const string route = ApiKeyProbeEndpoint;
		RequiresApiKey = false;

		// issue the request
		var webResponse = await IssueRequest(route, WebRequestMethods.Http.Get, null);

		// assign the response data model
		if (webResponse != null)
		{
			OctoDataModel.OctoApiKeyWorkflowResponse = new ApiKeyWorkflowResponse
			{
				HttpMessage = webResponse.ResponseMessage,
				Data = new ApiKeyWorkflowResponse.DataModel()
				{
					WorkflowSupported = webResponse.ResponseMessage is {StatusCode: HttpStatusCode.NoContent}
				}
			};
		}
	}

	#endregion

	#region Issue App Api Key Request

	/// <summary>
	/// Issues an apikey request to the Octoprint server
	/// </summary>
	/// <param name="appName">The name of the app requesting the api key</param>
	/// <param name="username">The users octoprint username</param>
	/// <remarks>
	/// <para />Endpoint: octoprint_url/plugin/appkeys/request
	/// <para />Request Type: POST
	/// <para />
	/// HTTP Headers:
	///     Content-Type: application/json
	/// <para />
	/// HTTP Body (required):
	/// app, user
	/// </remarks>
	///
	public async Task IssueApiKeyRequest(string appName, string username)
	{
		// setup route information for this api
		const string route = ApiKeyRequestEndpoint;
		RequiresApiKey = false;

		// setup the request data
		var requestData = new ApiKeyRequest
		{
			AppName = appName,
			Username = username
		};

		// issue the request
		var webResponse = await IssueRequest(route, WebRequestMethods.Http.Post, requestData);

		// assign the response data model
		if (webResponse != null)
		{
			OctoDataModel.OctoApiKeyRequestResponse = new ApiKeyRequestResponse
			{
				HttpMessage = webResponse.ResponseMessage,
				Data = !string.IsNullOrEmpty(webResponse.JsonString)
					? JsonConvert.DeserializeObject<ApiKeyRequestResponse.DataModel>(webResponse.JsonString)
					: null
			};
		}
	}

	#endregion

	#region Check Api Key Request Status

	/// <summary>
	/// Issues an apikey request status update request to the Octoprint server
	/// </summary>
	/// <param name="appToken">The app token retrieved from the IssueAppApiRequest function</param>
	/// <remarks>
	/// <para />Endpoint: octoprint_url/plugin/appkeys/request
	/// <para />Request Type: GET
	/// <para />
	/// HTTP Headers:
	///     Content-Type: application/json
	/// <para />
	/// HTTP query param (required):
	/// app_token
	/// </remarks>
	///
	public async Task CheckApiKeyRequestStatus(string? appToken)
	{
		// setup route information for this api
		var route = $@"{ApiKeyRequestEndpoint}/{appToken}";
		RequiresApiKey = false;

		// issue the request
		var webResponse = await IssueRequest(route, WebRequestMethods.Http.Get, null);

		// assign the response data model
		if (webResponse != null)
		{
			OctoApiInstance.OctoDataModel
				.OctoApiKeyStatusResponse = new ApiKeyStatusResponse
			{
				HttpMessage = webResponse.ResponseMessage,
				Data = !string.IsNullOrEmpty(webResponse.JsonString)
					? JsonConvert.DeserializeObject<ApiKeyStatusResponse.DataModel>(webResponse
						.JsonString)
					: null
			};

			// poll the apikey request octoprint plugin to wait for the users decision
			if (webResponse.ResponseMessage != null)
			{
				var statusCode = webResponse.ResponseMessage.StatusCode;

				// if the request hasn't been approved or denied, continue polling
				if (statusCode != HttpStatusCode.OK && statusCode != HttpStatusCode.NotFound)
				{
					await Task.Delay(TimeSpan.FromSeconds(1));
					await CheckApiKeyRequestStatus(appToken);
				}
			}
		}
	}

	#endregion
}
