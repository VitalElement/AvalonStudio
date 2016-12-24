using System.Threading.Tasks;

namespace AvalonStudio.Debugging.GDB
{
	public abstract class Command
	{
		protected const int DefaultCommandTimeout = 5000;

		public abstract int TimeoutMs { get; }

		public abstract string Encode();

		protected static ResponseCode DecodeResponseCode(string response)
		{
			var result = ResponseCode.Error;

			if (response == string.Empty)
			{
				result = ResponseCode.Timeout;
			}
			else
			{
				var split = response.Split(new[] {','}, 2);

				switch (split[0])
				{
					case "^done":
					case "^running":
					case "^connected":
						result = ResponseCode.Done;
						break;

					case "^exit":
						result = ResponseCode.Exit;
						break;
				}
			}

			return result;
		}

		public abstract void OutOfBandDataReceived(string data);
	}

	public abstract class Command<T> : Command
	{
		protected abstract T Decode(string response);

		public async Task<T> Execute(GDBDebugger gdb)
		{
			var response = await gdb.SendCommand(this, TimeoutMs);

			T result;

			result = Decode(response);

			return result;
		}
	}
}