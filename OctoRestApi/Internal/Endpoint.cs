// OctoRestApi copyright 2022 Danny Glover.

using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using OctoRestApi.DataModels;
using OctoRestApi.Internal.Utils;

namespace OctoRestApi.Internal;

public abstract class Endpoint
{
    private OctoApi OctoApiInstance { get; }
    private HttpClient OctoHttpClient { get; }
    protected OctoDataModel OctoDataModel { get; }
    protected string? Route { get; init; }
    protected string? RouteCommandPrefix { get; init; }
    protected string? DebugMessagePrefix { get; init; }

    protected class WebResponse
    {
        public string? JsonString { get; init; }
        public HttpStatusCode? StatusCode { get; init; }
        public HttpRequestException? RequestException { get; set; }
    }

    protected Endpoint(OctoApi octoApi)
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

    protected async Task<WebResponse?> IssueRequest(string requestType, object requestDataModel)
    {
        // set headers
        OctoHttpClient.DefaultRequestHeaders.Accept.Clear();
        OctoHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

        // issue the request and get the response
        try
        {
            var httpRequestTask = OctoHttpClient.PostAsync(Route, JsonUtil.SerializeObject(requestDataModel));
            var httpResponse = await httpRequestTask;
            httpResponse.EnsureSuccessStatusCode();
            
            // get the json data from the response
            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            
            return new WebResponse()
            {
                JsonString = jsonResponse,
                StatusCode = httpResponse.StatusCode
            };
        }
        catch (HttpRequestException exception)
        {
            if (OctoApiInstance.DebugMode)
            {
                Console.WriteLine($"Exception caught: {exception.Message}");

                return new WebResponse()
                {
                    JsonString = null,
                    StatusCode = exception.StatusCode,
                    RequestException = exception
                };
            }
        }

        return null;
    }
}