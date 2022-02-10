// OctoRestApi copyright 2022 Danny Glover.

using System.Net;
using OctoRestApi.DataModels.Request;
using OctoRestApi.DataModels.Response;

namespace OctoRestApi.Internal.Endpoints;

internal class PrinterTools: Endpoint
{
    public PrinterTools(OctoApi octoApi) : base(octoApi)
    {
        Route = "printer/tool";
        RouteCommandPrefix = "> Printer > Tool |";
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
        // setup the request data
        var requestData = new PrintToolRequestDataModel
        {
            Command = "target",
            Targets = toolIndexes.ToDictionary(index => $@"tool{index}", _ => temperature)
        };
        
        // issue the request
        var webResponse = await IssueRequest(WebRequestMethods.Http.Post, requestData);
        
        // handle errors
        if (webResponse?.JsonString == null) return;

        // assign the response data model
        OctoDataModel.OctoPrintToolResponseDataModel = new PrintToolResponseDataModel
        {
            Success = webResponse.StatusCode == HttpStatusCode.NoContent
        };
    }
}