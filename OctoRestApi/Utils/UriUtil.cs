// OctoRestApi copyright 2022 Danny Glover.

namespace OctoRestApi.Utils;

public static class UriUtil
{
	public static bool IsValidHttpUri(string uri)
	{
		if (!Uri.IsWellFormedUriString(uri, UriKind.Absolute))
		{
			return false;
		}

		// test if the uri is of type http/https
		if (Uri.TryCreate(uri, UriKind.Absolute, out var testUri))
		{
			return testUri.Scheme == Uri.UriSchemeHttp || testUri.Scheme == Uri.UriSchemeHttps;
		}

		return false;
	}
}
