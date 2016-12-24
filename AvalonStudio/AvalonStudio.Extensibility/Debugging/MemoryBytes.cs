using System;
using System.Collections.Generic;

namespace AvalonStudio.Debugging
{
	public class MemoryBytes
	{
		public ulong Address { get; set; }

		public ulong Offset { get; private set; }

		public ulong End { get; private set; }

		public byte[] Data { get; set; }

		public string Values { get; set; }

		public static byte[] StringToByteArray(string hex)
		{
			var NumberChars = hex.Length;
			var bytes = new byte[NumberChars/2];

			for (var i = 0; i < NumberChars; i += 2)
			{
				bytes[i/2] = Convert.ToByte(hex.Substring(i, 2), 16);
			}

			return bytes;
		}

		public static List<MemoryBytes> FromDataString(string data)
		{
			var result = new List<MemoryBytes>();

			var responsePair = data.Substring(6).ToNameValuePair();

			if (responsePair.Name == "memory")
			{
				var memoryBlocks = responsePair.Value.ToArray();

				foreach (var memoryBlock in memoryBlocks)
				{
					var block = new MemoryBytes();

					var pairs = memoryBlocks[0].RemoveBraces().ToNameValuePairs();

					foreach (var pair in pairs)
					{
						switch (pair.Name)
						{
							case "begin":
								block.Address = Convert.ToUInt64(pair.Value, 16);
								break;

							case "offset":
								block.Offset = Convert.ToUInt64(pair.Value, 16);
								break;

							case "end":
								block.End = Convert.ToUInt64(pair.Value, 16);
								break;

							case "contents":
								block.Data = StringToByteArray(pair.Value);
								block.Values = pair.Value;
								break;
						}
					}

					result.Add(block);
				}
			}

			return result;
		}
	}
}