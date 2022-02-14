// OctoRestApi copyright 2022 Danny Glover.

namespace OctoRestApi.DataModels.Response.Authentication;

public class ApiKeyWorkflowResponse
{
	public HttpResponseMessage? HttpMessage { get; set; }
	public DataModel? Data { get; set; }

	public class DataModel
	{
		public bool WorkflowSupported { get; set; }
	}
}
