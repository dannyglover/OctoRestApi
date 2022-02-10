// OctoRestApi copyright 2022 Danny Glover.

using System.Net;
using Newtonsoft.Json;
using OctoRestApi.DataModels.Request;
using OctoRestApi.DataModels.Response;
using OctoRestApi.Internal.Utils;

namespace OctoRestApi.Internal.Endpoints;

internal class Authentication: Endpoint
{
    public Authentication(OctoApi octoApi) : base(octoApi)
    {
        Route = "login";
        RouteCommandPrefix = "> Login |";
        DebugMessagePrefix = "Authentication ";
    }

    /// <summary>
    /// Issues a login request to the Octoprint server
    /// </summary>
    /// <param name="username">The users username</param>
    /// <param name="password">The users password</param>
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
        // setup the request data
        var requestData = new LoginRequestDataModel()
        {
            Username = username,
            Password = password
        };

        // issue the request
        var webResponse = await IssueRequest(WebRequestMethods.Http.Post, requestData);

        // handle errors
        if (webResponse?.JsonString == null)
        {
            if (webResponse == null) return;
            
            var responseMessage = webResponse.StatusCode switch
            {
                HttpStatusCode.Forbidden => @"Incorrect username or password.",
                _ => $@"StatusCode: {webResponse.StatusCode}"
            };

            ConsoleUtil.WriteLine(@$"{DebugMessagePrefix}{RouteCommandPrefix} {responseMessage}",
                ConsoleColor.Red);

            return;
        }

        // assign the response data model
        OctoDataModel.OctoLoginResponseDataModel =
            JsonConvert.DeserializeObject<LoginResponseDataModel>(webResponse.JsonString);
    }
}
