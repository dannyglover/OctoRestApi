// See https://aka.ms/new-console-template for more information

using OctoRestApi;
using OctoRestApi.Internal.Utils;
using TestConsoleApp.Utils;

namespace TestConsoleApp;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        ConsoleUtil.WriteLine("Welcome to the OctoRestApi Test App!", ConsoleColor.Yellow);
        Console.WriteLine("-------------------------------------");
        Console.WriteLine("Let's get you logged in to your printer.");
        Console.WriteLine("Enter your credentials below:");
        var octoprintUrl = InputUtil.InputReadField("Octoprint Server Url: ", ConsoleColor.Green);
        var username = InputUtil.InputReadField("Octoprint Username: ", ConsoleColor.Green);
        var password = InputUtil.InputReadFieldPassword("Octoprint Password: ", ConsoleColor.Green);
        Console.WriteLine();
        
        // abort if the octoprintUrl is malformed
        if (!UriUtil.IsValidHttpUri(octoprintUrl))
        {
            ConsoleUtil.WriteLine(@$"Invalid Octoprint Url: {octoprintUrl}. Please check your input and try running the program again. Aborting",
                ConsoleColor.Red);
            return;
        }
        
        // create the OctoApi instance
        var octoApi = new OctoApi(octoprintUrl)
        {
            DebugMode = true
        };

        // login
        await octoApi.Login(username, password);
        
        // get the response data model
        var loginResponseDataModel = octoApi.OctoDataModel.OctoLoginResponseDataModel;

        Console.WriteLine(loginResponseDataModel == null
            ? "LoginResponseDataModel not initialized, due to unexpected response."
            : $@"User name is: {loginResponseDataModel.Name}");
        
        // set tool 0 (hot-end) to 50 degrees
        await octoApi.SetTargetToolTemperature(new[] {0}, 40);
        
        Console.WriteLine($@"Tool 0 temperature request success: {octoApi.OctoDataModel.OctoPrintToolResponseDataModel is {Success: true}}");
    }
}