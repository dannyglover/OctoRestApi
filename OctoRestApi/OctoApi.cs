// OctoRestApi copyright 2022 Danny Glover.

using OctoRestApi.DataModels;
using OctoRestApi.Internal.Endpoints;

namespace OctoRestApi;

public class OctoApi
{
    internal HttpClient OctoHttpClient { get; }
    public OctoDataModel OctoDataModel { get; }
    private Authentication OctoAuthentication { get; }
    private PrinterTools OctoPrinterTools { get; }
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
        OctoPrinterTools = new PrinterTools(this);
        OctoHttpClient.BaseAddress = new Uri(OctoprintUrl);
    }

    #region Authentication

    public async Task Login(string username, string password)
    {
        await OctoAuthentication.Login(username, password);
    }

    #endregion

    #region PrintTools

    public async Task SetTargetToolTemperature(IEnumerable<int> toolIndexes, int temperature)
    {
        await OctoPrinterTools.SetTargetTemperature(toolIndexes, temperature);
    }
    
    #endregion
    
    
}