// OctoRestApi copyright 2022 Danny Glover.

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace TestConsoleApp.Utils;

public static class Platform
{
	public static bool IsWindows()
	{
		return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
	}

	public static bool IsMac()
	{
		return RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
	}

	public static bool IsLinux()
	{
		return RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
	}

	public static void OpenBrowser(string url)
	{
		// defaults to linux command
		var command = "xdg-open";

		if (IsMac())
		{
			command = "open";
		}
		else if (IsWindows())
		{
			command = "cmd.exe /c start";
		}

		Process.Start(command, url);
	}
}
