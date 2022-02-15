// See https://aka.ms/new-console-template for more information

using System.Net;
using OctoRestApi;
using OctoRestApi.Internal.Utils;
using TestConsoleApp.Utils;

namespace TestConsoleApp;

internal static class Program
{
	private static async Task Main(string[] args)
	{
		var octoApiTestDataPath = "octoApiTest.bin";
		var apiKey = SecureDataUtil.ReadData(octoApiTestDataPath);
		string octoprintUrl;

		// get the octoprint details from the user
		ConsoleUtil.WriteLine("Welcome to the OctoRestApi Test App!", ConsoleColor.Yellow);
		Console.WriteLine("-------------------------------------");
		Console.WriteLine("First, we need the Url to your Octoprint instance. Please enter it below.");
		octoprintUrl = InputUtil.InputReadField("Octoprint Server Url (defaults to octopi.local if omitted): ",
			ConsoleColor.Green);

		// let the user know if we already have an ApiKey
		if (!string.IsNullOrEmpty(apiKey))
		{
			ConsoleUtil.WriteLine($@"You have already granted the App an ApiKey. Let's continue!",
				ConsoleColor.Magenta);
		}

		// default to octoprint defaults if values not provided
		if (string.IsNullOrEmpty(octoprintUrl))
		{
			octoprintUrl = "http://octopi.local";
		}

		// abort if the octoprintUrl is malformed
		if (!UriUtil.IsValidHttpUri(octoprintUrl))
		{
			ConsoleUtil.WriteLine(
				@$"Invalid Octoprint Url: {octoprintUrl}. Please check your input and try running the program again. Aborting",
				ConsoleColor.Red);
			return;
		}

		// create the OctoApi instance
		var octoApi = new OctoApi(octoprintUrl)
		{
			DebugMode = true
		};

		/*
		await octoApi.Login("pi", "#sNoT33ykG%Bz3");

		if (octoApi.OctoDataModel.OctoLoginResponse?.Data != null)
		{
			Console.WriteLine($@"LOGIN > returned username {octoApi.OctoDataModel.OctoLoginResponse?.Data.Name}");
		}*/

		// set the ApiKey from the saved data (if any)
		octoApi.SetApiKey(apiKey);

		// request an ApiKey if one doesn't already exist
		if (string.IsNullOrEmpty(apiKey))
		{
			Console.WriteLine(
				"To finish authentication, we need an ApiKey from Octoprint. We'll request one after you enter your username.");
			var username =
				InputUtil.InputReadField("Octoprint Username (defaults to pi if omitted): ", ConsoleColor.Green);
			ConsoleUtil.WriteLine(
				$@"We'll try and automatically open your browser at: {octoprintUrl} for you to accept the access request we sent. Come back here after you're done.",
				ConsoleColor.Magenta);

			// default to pi username if username not provided
			if (string.IsNullOrEmpty(username))
			{
				username = "pi";
			}

			// probe for ApiKey workflow support. If not supported, user must manually create and provide an ApiKey
			await octoApi.ProbeForApiKeyWorkflowSupport();

			// ensure ApiKey workflow support is enabled/installed
			if (octoApi.OctoDataModel.OctoApiKeyWorkflowResponse?.Data != null)
			{
				if (!octoApi.OctoDataModel.OctoApiKeyWorkflowResponse.Data.WorkflowSupported)
				{
					ConsoleUtil.WriteLine(
						$@"Your Octoprint installation either does not support the ApiKey request model or its plugin is disabled/uninstalled. You'll have to create your ApiKey manually on your Octoprint server at {octoprintUrl}",
						ConsoleColor.Red);

					var userApiKey = string.Empty;

					// force input for ApiKey
					while (string.IsNullOrEmpty(userApiKey))
					{
						userApiKey = InputUtil.InputReadField("Octoprint ApiKey: ", ConsoleColor.Green);
						octoApi.SetApiKey(userApiKey);
					}
				}
			}

			// start ApiKey request
			await octoApi.IssueApiKeyRequest("OctoRestApiTest", username);

			// open the octoprint url with the systems browser
			Platform.OpenBrowser(octoprintUrl);

			// poll the ApiKey request status to await the users decision
			var apiKeyRequestDataModel =
				octoApi.OctoDataModel.OctoApiKeyRequestResponse?.Data;
			await octoApi.CheckApiKeyRequestStatus(apiKeyRequestDataModel?.AppToken);

			// if the user denied the request, post message and return
			if (octoApi.OctoDataModel.OctoApiKeyStatusResponse?.HttpMessage != null)
			{
				if (octoApi.OctoDataModel.OctoApiKeyStatusResponse.HttpMessage.StatusCode != HttpStatusCode.OK)
				{
					if (octoApi.OctoDataModel.OctoApiKeyStatusResponse.HttpMessage.StatusCode ==
					    HttpStatusCode.NotFound)
					{
						ConsoleUtil.WriteLine("You either denied the access request, or it timed out. Exiting.",
							ConsoleColor.Red);
						return;
					}
				}
			}

			// save the ApiKey
			if (octoApi.OctoDataModel.OctoApiKeyStatusResponse?.Data?.ApiKey != null)
			{
				var writeSuccess = SecureDataUtil.WriteData(
					octoApi.OctoDataModel.OctoApiKeyStatusResponse.Data.ApiKey,
					octoApiTestDataPath);

				if (writeSuccess)
				{
					ConsoleUtil.WriteLine($@"Thank you for granting access. We now have an ApiKey. Let's continue!",
						ConsoleColor.Magenta);
				}
			}
		}

		// set tool 0 (hot-end) to 50 degrees
		await octoApi.SetTargetToolTemperature(new[] {0}, 80);

		Console.WriteLine(
			$@"Tool 0 temperature request success: {octoApi.OctoDataModel.OctoPrintToolResponse?.Data is {Success: true}}");
	}
}
