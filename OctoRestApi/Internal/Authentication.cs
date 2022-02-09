// OctoRestApi copyright 2022 Danny Glover.

using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using Newtonsoft.Json;
using OctoRestApi.DataModels;
using OctoRestApi.DataModels.Request;
using OctoRestApi.DataModels.Response;
using OctoRestApi.Internal.Utils;

namespace OctoRestApi.Internal;

internal class Authentication
{
    private OctoApi OctoApiInstance { get; }
    private HttpClient OctoHttpClient { get; }
    private OctoDataModel OctoDataModel { get; }
    private string Endpoint { get; }
    private string DebugMessagePrefix { get; }

    public Authentication(OctoApi octoApi)
    {
        OctoApiInstance = octoApi;
        OctoHttpClient = OctoApiInstance.OctoHttpClient;
        OctoDataModel = OctoApiInstance.OctoDataModel;
        Endpoint = @"/api/login";
        DebugMessagePrefix = "Authentication > Login |";
    }

    /// <summary>
    /// Issues a login request to the Octopi server
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
        // set headers
        OctoHttpClient.DefaultRequestHeaders.Accept.Clear();
        OctoHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

        // setup the request data
        var requestData = new LoginRequestDataModel()
        {
            Username = username,
            Password = password
        };
        
        // TODO: handle connection refused exception
        // happens for instance if you enter an incorrect url
        
        // issue the request and get the response
        var httpRequestTask = OctoHttpClient.PostAsync(Endpoint, JsonUtil.SerializeObject(requestData));
        var httpResponse = await httpRequestTask;

        // if the request wasn't successful
        if (httpResponse.StatusCode != HttpStatusCode.OK)
        {
            if (OctoApiInstance.DebugMode)
            {
                ConsoleUtil.WriteLine(@$"{DebugMessagePrefix} StatusCode: {httpResponse.StatusCode}",
                    ConsoleColor.Red);
            }
            
            return;
        }
        
        // get the json data from the response
        var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
        OctoDataModel.OctoLoginResponseDataModel = JsonConvert.DeserializeObject<LoginResponseDataModel>(jsonResponse);
    }
}