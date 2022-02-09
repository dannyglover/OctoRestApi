// OctoRestApi copyright 2022 Danny Glover.

using OctoRestApi.DataModels;
using OctoRestApi.Internal;

namespace OctoRestApi;

public class OctoApi
{
    internal HttpClient OctoHttpClient { get; }
    public OctoDataModel OctoDataModel { get; }
    private Authentication OctoAuthentication { get; }
    public string? OctoprintUrl { get; set; }
    public bool DebugMode { get; set; }

    public OctoApi(string? octoprintUrl)
    {
        OctoprintUrl = octoprintUrl;
        OctoHttpClient = new HttpClient();
        OctoDataModel = new OctoDataModel();
        OctoAuthentication = new Authentication(this);
        
        OctoHttpClient.BaseAddress = new Uri(octoprintUrl ?? throw new ArgumentNullException(nameof(octoprintUrl)));
    }

    public async Task Login(string username, string password)
    {
        await OctoAuthentication.Login(username, password);
    }
}