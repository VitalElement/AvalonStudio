using System;
using System.Collections.Generic;

namespace AvalonStudio.Debugging
{
	public static class Extensions
	{
		public static StopReason ToStopReason(this string data)
		{
			switch (data)
			{
				case "breakpoint-hit":
					return StopReason.BreakPointHit;

				case "end-stepping-range":
					return StopReason.EndSteppingRange;

				case "exited-normally":
					return StopReason.ExitedNormally;

				case "exited":
					return StopReason.Exited;

				case "exited-signalled":
					return StopReason.ExitedSignalled;

				case "function-finished":
					return StopReason.FunctionFinished;

				case "signal-received":
					return StopReason.SignalReceived;

                case "entry-point-hit":
                    return StopReason.EntryPointHit;

				default:
					throw new NotImplementedException();
			}
		}

		public static string RemoveBraces(this string data)
		{
			return data.Substring(1, data.Length - 2);
		}

		public static NameValuePair ToNameValuePair(this string data)
		{
			var split = data.Split(new[] {'='}, 2);

			var result = new NameValuePair();
			result.Name = split[0];
			result.Value = split[1];

			if (result.Value[0] == '"')
			{
				result.Value = result.Value.RemoveBraces();
			}

			return result;
		}

		public static string[] ToArray(this string data)
		{
			var results = new List<string>();

			if (data != string.Empty)
			{
				var stateStack = new Stack<ArrayProcessState>();
				var currentState = ArrayProcessState.parsingText;

				var currentString = string.Empty;

				if (data[0] != '[' || data[data.Length - 1] != ']')
				{
					throw new Exception("Array is not contained in braces.");
				}

				foreach (var c in data.RemoveBraces()) //TODO Throw an exception if not surrounded by '[]'
				{
					switch (currentState)
					{
						case ArrayProcessState.parsingText:
							switch (c)
							{
								case '{':
									stateStack.Push(currentState);
									currentState = ArrayProcessState.parsingObject;
									currentString += c;
									break;

								case ',':
									results.Add(currentString);
									currentString = string.Empty;
									break;

								default:
									currentString += c;
									break;
							}
							break;

						case ArrayProcessState.parsingObject:
							switch (c)
							{
								case '{':
									stateStack.Push(currentState);
									currentState = ArrayProcessState.parsingObject;
									currentString += c;
									break;

								case '}':
									currentState = stateStack.Pop();
									currentString += c;
									break;

								default:
									currentString += c;
									break;
							}
							break;
					}
				}

				if (currentString.Trim() != string.Empty)
				{
					results.Add(currentString);
				}
			}

			return results.ToArray();
		}

		public static NameValuePair[] ToNameValuePairs(this string data)
		{
			var results = new List<NameValuePair>();

			var stateStack = new Stack<NameValueProcessState>();
			var currentState = NameValueProcessState.parsingText;
			var escapeChars = new Stack<char>();
			var escapeChar = '\0';

			var currentString = string.Empty;

			foreach (var c in data)
			{
				switch (currentState)
				{
					case NameValueProcessState.parsingText:
						switch (c)
						{
							case '[':
								stateStack.Push(currentState);
								currentState = NameValueProcessState.parsingArray;
								currentString += c;
								break;

							case '{':
								stateStack.Push(currentState);
								currentState = NameValueProcessState.parsingObject;
								currentString += c;
								break;

							case '"':
								stateStack.Push(currentState);
								currentState = NameValueProcessState.parsingValue;
								currentString += c;
								break;

							case ',':
								results.Add(currentString.ToNameValuePair());
								currentString = string.Empty;
								break;


							default:
								currentString += c;
								break;
						}
						break;

					case NameValueProcessState.parsingValue:
						switch (c)
						{
							case '[':
								stateStack.Push(currentState);
								currentState = NameValueProcessState.parsingArray;
								currentString += c;
								break;

							case '{':
								stateStack.Push(currentState);
								currentState = NameValueProcessState.parsingObject;
								currentString += c;
								break;

							case '"':
								currentState = stateStack.Pop();
								currentString += c;
								break;

							default:
								currentString += c;
								break;
						}
						break;

					case NameValueProcessState.enteringEscapeSequence:
					{
						escapeChars.Push(escapeChar);
						escapeChar = c;

						currentState = NameValueProcessState.parsingEscapeSequence;
					}
						break;

					case NameValueProcessState.exitingEscapeSequence:
					{
						escapeChar = escapeChars.Pop();

						currentState = stateStack.Pop();
					}
						break;

					case NameValueProcessState.parsingEscapeSequence:
						switch (c)
						{
							case '\'':
							case '"':
								// case '`':
								currentState = stateStack.Pop();
								currentString += c;
								break;

							default:
								currentString += c;
								break;
						}
						break;

					case NameValueProcessState.parsingArray:
						switch (c)
						{
							case '\'':
							case '"':
								//case '`':
								stateStack.Push(currentState);
								currentState = NameValueProcessState.parsingEscapeSequence;
								currentString += c;
								break;

							case '[':
								stateStack.Push(currentState);
								currentString += c;
								break;

							case ']':
								currentState = stateStack.Pop();
								currentString += c;
								break;

							default:
								currentString += c;
								break;
						}
						break;

					case NameValueProcessState.parsingObject:
						switch (c)
						{
							case '{':
								stateStack.Push(currentState);
								currentString += c;
								break;

							case '}':
								currentState = stateStack.Pop();
								currentString += c;
								break;

							default:
								currentString += c;
								break;
						}
						break;
				}
			}

			if (currentString.Trim() != string.Empty)
			{
				results.Add(currentString.ToNameValuePair());
			}

			return results.ToArray();
		}

		private enum ArrayProcessState
		{
			parsingText,
			parsingObject
		}

		private enum NameValueProcessState
		{
			enteringEscapeSequence,
			exitingEscapeSequence,
			parsingEscapeSequence,
			parsingText,
			parsingObject,
			parsingValue,
			parsingArray
		}
	}
}