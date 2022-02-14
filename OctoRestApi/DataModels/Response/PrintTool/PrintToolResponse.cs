// OctoRestApi copyright 2022 Danny Glover.

namespace OctoRestApi.DataModels.Response.PrintTool;

public class PrintToolResponse
{
	public HttpResponseMessage? HttpMessage { get; set; }
	public DataModel? Data { get; set; }

	public class DataModel
	{
		public bool Success { get; set; }
	}
}
