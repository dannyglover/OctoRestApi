// OctoRestApi copyright 2022 Danny Glover.

using System.Net;
using OctoRestApi.DataModels.Request.PrintTool;
using OctoRestApi.DataModels.Response.PrintTool;

namespace OctoRestApi.Internal.Apis;

internal class PrinterTools : Api
{
	private const string PrinterToolEndpoint = $@"{Endpoint.Printer}/tool";

	public PrinterTools(OctoApi octoApi) : base(octoApi)
	{
		DebugMessagePrefix = "PrinterTools ";
	}

	/// <summary>
	/// Issues a printer tool request to the Octoprint server
	/// </summary>
	/// <param name="toolIndexes">Array of printer tool (hot-end) index(es) to use</param>
	/// <param name="temperature">The printer tool temperature to set</param>
	/// <remarks>
	/// <para />Endpoint: octoprint_url/api/printer/tool
	/// <para />Request Type: POST
	/// <para />
	/// HTTP Headers:
	///     Content-Type: application/json
	/// <para />
	/// HTTP Body (required):
	/// command, target
	/// </remarks>
	///
	public async Task SetTargetTemperature(IEnumerable<int> toolIndexes, int temperature)
	{
		// setup route information for this api
		const string route = PrinterToolEndpoint;
		const string routeCommandPrefix = "> SetTargetTemperature |";

		// setup the request data
		var requestData = new PrintToolRequest
		{
			Command = "target",
			Targets = toolIndexes.ToDictionary(index => $@"tool{index}", _ => temperature)
		};

		// issue the request
		var webResponse = await IssueRequest(route, WebRequestMethods.Http.Post, requestData);

		// handle errors
		if (webResponse?.JsonString == null)
		{
			return;
		}

		// assign the response data model
		OctoDataModel.OctoPrintToolResponse = new PrintToolResponse
		{
			Success = webResponse.ResponseMessage is {StatusCode: HttpStatusCode.NoContent}
		};
	}
}