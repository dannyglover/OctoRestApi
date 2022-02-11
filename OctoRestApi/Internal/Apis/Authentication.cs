﻿// OctoRestApi copyright 2022 Danny Glover.

using System.Net;
using Newtonsoft.Json;
using OctoRestApi.DataModels.Request.Authentication;
using OctoRestApi.DataModels.Response.Authentication;
using OctoRestApi.Internal.Utils;

namespace OctoRestApi.Internal.Apis;

internal class Authentication : Api
{
	private const string AppKeyRequestEndpoint = $@"{Endpoint.Plugin}appkeys/request";

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
		const string route = $"{Endpoint.Api}login";
		const string routeCommandPrefix = "> Login |";

		// setup the request data
		var requestData = new LoginRequest()
		{
			Username = username,
			Password = password,
			RememberLogin = true
		};

		// issue the request
		var webResponse = await IssueRequest(route, WebRequestMethods.Http.Post, requestData);

		// handle errors
		if (webResponse?.JsonString == null)
		{
			if (webResponse?.ResponseMessage == null)
			{
				return;
			}

			var responseMessage = webResponse.ResponseMessage.StatusCode switch
			{
				HttpStatusCode.Forbidden => @"Incorrect username or password.",
				_ => $@"StatusCode: {webResponse.ResponseMessage.StatusCode}"
			};

			ConsoleUtil.WriteLine(@$"{DebugMessagePrefix}{routeCommandPrefix} {responseMessage}",
				ConsoleColor.Red);

			return;
		}

		// assign the response data model
		OctoDataModel.OctoLoginResponse =
			JsonConvert.DeserializeObject<LoginResponse>(webResponse.JsonString);
	}

	#endregion

	#region IssueAppApiKeyRequest

	/// <summary>
	/// Issues an app apikey request to the Octoprint server
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
	public async Task IssueAppApiKeyRequest(string appName, string username)
	{
		// setup route information for this api
		const string route = AppKeyRequestEndpoint;
		const string routeCommandPrefix = "> IssueAppApiKeyRequest |";

		// setup the request data
		var requestData = new AppApiKeyRequest
		{
			AppName = appName,
			Username = username
		};

		// issue the request
		var webResponse = await IssueRequest(route, WebRequestMethods.Http.Post, requestData);

		// handle errors
		if (webResponse?.JsonString == null)
		{
			return;
		}

		// check if everything is as expected
		if (webResponse.ResponseMessage is {StatusCode: HttpStatusCode.Created})
			// assign the response data model
		{
			OctoDataModel.OctoAppApiKeyResponse =
				JsonConvert.DeserializeObject<AppApiKeyResponse>(webResponse.JsonString);
		}
	}

	#endregion

	#region CheckAppApiKeyRequestStatus

	/// <summary>
	/// Issues an app apikey request status update request to the Octoprint server
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
	public async Task CheckAppApiKeyRequestStatus(string? appToken)
	{
		// setup route information for this api
		var route = $@"{AppKeyRequestEndpoint}/{appToken}";
		const string routeCommandPrefix = "> CheckAppApiKeyRequestStatus |";

		// issue the request
		var webResponse = await IssueRequest(route, WebRequestMethods.Http.Get, null);

		// handle errors
		if (webResponse?.JsonString == null)
		{
			return;
		}

		// check if everything is as expected
		if (webResponse.ResponseMessage != null)
		{
			switch (webResponse.ResponseMessage.StatusCode)
			{
				case HttpStatusCode.OK:
					// assign the response data model
					OctoDataModel.OctoApiKeyStatusResponse =
						JsonConvert.DeserializeObject<ApiKeyStatusResponse>(webResponse
							.JsonString);
					Console.WriteLine($@"{routeCommandPrefix} requested accepted");
					break;

				case HttpStatusCode.Accepted:
					// still waiting for a decision, keep polling
					Console.WriteLine($@"{routeCommandPrefix} still waiting for approval");
					break;

				case HttpStatusCode.NotFound:
					// access denied or request timed out (due to 5 second timeout rule when requesting keys)
					Console.WriteLine($@"{routeCommandPrefix} access denied or request timed out");
					break;
			}

			// poll the apikey request octoprint plugin to wait for the users decision
			if (webResponse.ResponseMessage.StatusCode != HttpStatusCode.OK)
			{
				await Task.Delay(TimeSpan.FromSeconds(1));
				await CheckAppApiKeyRequestStatus(OctoApiInstance.OctoDataModel
					.OctoAppApiKeyResponse
					?.AppToken);
			}
		}
	}

	#endregion
}