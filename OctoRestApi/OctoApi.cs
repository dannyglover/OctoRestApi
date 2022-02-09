// OctoRestApi copyright 2022 Danny Glover.

using OctoRestApi.DataModels;
using OctoRestApi.Internal;
using OctoRestApi.Internal.Utils;

namespace OctoRestApi;

public class OctoApi
{
    internal HttpClient OctoHttpClient { get; }
    public OctoDataModel OctoDataModel { get; }
    private Authentication OctoAuthentication { get; }
    private string _octoprintUrl;
    private string DebugMessagePrefix { get; }
    public string OctoprintUrl
    {
        get => _octoprintUrl;
        init => _octoprintUrl = @$"{value}/api/";
    }
    public bool DebugMode { get; set; }

    public OctoApi(string octoprintUrl)
    {
        DebugMessagePrefix = "OctoApi:";
        OctoprintUrl = octoprintUrl;
        OctoHttpClient = new HttpClient();
        OctoDataModel = new OctoDataModel();
        OctoAuthentication = new Authentication(this);
        OctoHttpClient.BaseAddress = new Uri(OctoprintUrl);
    }

    public async Task Login(string username, string password)
    {
        await OctoAuthentication.Login(username, password);
    }
}