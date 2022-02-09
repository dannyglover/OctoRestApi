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
        
        // TODO: verify url is valid (format wise) before continuing
        
        // create the OctoApi instance
        var octoApi = new OctoApi(octoprintUrl);
        octoApi.SetDebugMode(true);
        
        // login
        await octoApi.Login(username, password);
        
        // get the users name
        var loginResponseDataModel = octoApi.OctoDataModel.OctoLoginResponseDataModel;
        
        Console.WriteLine($@"User name is: {loginResponseDataModel?.Name}");
    }
}