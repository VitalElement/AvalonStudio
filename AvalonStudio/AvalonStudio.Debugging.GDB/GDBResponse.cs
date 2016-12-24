namespace AvalonStudio.Debugging.GDB
{
	public enum ResponseCode
	{
		Done,
		Error,
		Exit,
		Timeout
	}

	public class GDBResponse<T>
	{
		public GDBResponse()
		{
			Response = ResponseCode.Timeout;
		}

		public GDBResponse(ResponseCode response)
		{
			Response = response;
		}

		public ResponseCode Response { get; private set; }
		public T Value { get; set; }
	}
}