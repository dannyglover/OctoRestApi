// OctoRestApi copyright 2022 Danny Glover.

using System.Net.Mime;
using System.Text;
using Newtonsoft.Json;

namespace OctoRestApi.Internal.Utils;

internal static class JsonUtil
{
	public static StringContent SerializeObject(object? requestData)
	{
		var serializedObject = JsonConvert.SerializeObject(requestData);
		return new StringContent(serializedObject, Encoding.UTF8, MediaTypeNames.Application.Json);
	}
}
