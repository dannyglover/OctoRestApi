// See https://aka.ms/new-console-template for more information

using System.Text;
using TestConsoleApp;

Console.WriteLine("Welcome to the OctoRestApi Test App!");
Console.WriteLine("-------------------------------------");
var username = ConsoleTools.InputReadField("Octoprint Username: ");
var pass = ConsoleTools.InputReadFieldPassword("Octoprint Password: ");
