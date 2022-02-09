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
        Endpoint = "login";
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
        
        // issue the request and get the response
        try
        {
            var httpRequestTask = OctoHttpClient.PostAsync(Endpoint, JsonUtil.SerializeObject(requestData));
            var httpResponse = await httpRequestTask;
            httpResponse.EnsureSuccessStatusCode();
            
            // get the json data from the response
            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            OctoDataModel.OctoLoginResponseDataModel = JsonConvert.DeserializeObject<LoginResponseDataModel>(jsonResponse);
        }
        catch (HttpRequestException exception)
        {
            if (OctoApiInstance.DebugMode)
            {
                Console.WriteLine($"Exception caught: {exception.Message}");
                ConsoleUtil.WriteLine(@$"{DebugMessagePrefix} StatusCode: {exception.StatusCode}",
                    ConsoleColor.Red);
            }
        }
    }
}