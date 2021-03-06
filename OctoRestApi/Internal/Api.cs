// OctoRestApi copyright 2022 Danny Glover.

using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using OctoRestApi.DataModels;
using OctoRestApi.Internal.Utils;

namespace OctoRestApi.Internal;

internal abstract class Api
{
	protected OctoApi OctoApiInstance { get; }
	private HttpClient OctoHttpClient { get; }
	protected OctoDataModel OctoDataModel { get; }
	protected bool RequiresApiKey { get; set; } = true;
	protected string? DebugMessagePrefix { get; init; }

	protected class WebResponse
	{
		public string? JsonString { get; init; }
		public HttpResponseMessage? ResponseMessage { get; init; }
		public HttpRequestException? RequestException { get; set; }
	}

	protected Api(OctoApi octoApi)
	{
		OctoApiInstance = octoApi;
		OctoHttpClient = OctoApiInstance.OctoHttpClient;
		OctoDataModel = OctoApiInstance.OctoDataModel;
	}

	protected void SetRequestHeaders(List<MediaTypeWithQualityHeaderValue> headers)
	{
		OctoHttpClient.DefaultRequestHeaders.Accept.Clear();

		foreach (var header in headers)
		{
			OctoHttpClient.DefaultRequestHeaders.Accept.Add(header);
		}
	}

	protected async Task<WebResponse?> IssueRequest(string route, string requestType, object? requestDataModel)
	{
		// set headers
		OctoHttpClient.DefaultRequestHeaders.Accept.Clear();
		OctoHttpClient.DefaultRequestHeaders.Accept.Add(
			new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

		// set the api key
		if (RequiresApiKey)
		{
			OctoHttpClient.DefaultRequestHeaders.Add(@"X-Api-Key", OctoApiInstance.GetApiKey());
		}

		// issue the request and get the response
		try
		{
			var httpResponseMessage = requestType switch
			{
				WebRequestMethods.Http.Get => await OctoHttpClient.GetAsync(route),
				WebRequestMethods.Http.Post => await OctoHttpClient.PostAsync(route,
					JsonUtil.SerializeObject(requestDataModel)),
				"Delete" => await OctoHttpClient.DeleteAsync(route),
				_ => null
			};

			// abort if the response was null
			if (httpResponseMessage == null)
			{
				return null;
			}

			// get the json data from the response
			var jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();

			return new WebResponse()
			{
				JsonString = jsonResponse,
				ResponseMessage = httpResponseMessage
			};
		}
		catch (HttpRequestException exception)
		{
			if (OctoApiInstance.DebugMode)
			{
				Console.WriteLine($"Exception caught: {exception.Message} - StatusCode: {exception.StatusCode}");
			}

			return new WebResponse()
			{
				JsonString = null,
				RequestException = exception
			};
		}
	}
}
