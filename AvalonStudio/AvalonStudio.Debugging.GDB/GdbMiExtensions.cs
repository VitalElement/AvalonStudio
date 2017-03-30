using AvalonStudio.Platforms;
using System;
using System.Collections.Generic;
using static AvalonStudio.Debugging.LiveBreakPoint;

namespace AvalonStudio.Debugging
{
	public static class GdbMiExtensions
	{
        public static VariableObjectChange VariableObjectChangeFromDataString(this string data)
        {
            var result = new VariableObjectChange();

            var pairs = data.ToNameValuePairs();

            foreach (var pair in pairs)
            {
                switch (pair.Name)
                {
                    case "name":
                        result.Expression = pair.Value;
                        break;

                    case "value":
                        result.Value = pair.Value;
                        break;

                    case "in_scope":
                        switch (pair.Value)
                        {
                            case "true":
                                result.InScope = true;
                                break;

                            case "false":
                            case "invalid":
                                result.InScope = false;
                                break;
                        }
                        break;

                    case "type_changed":
                        switch (pair.Value)
                        {
                            case "true":
                                result.TypeChanged = true;
                                break;

                            case "false":
                                result.TypeChanged = false;
                                break;
                        }
                        break;

                    case "has_more":
                        result.HasMore = Convert.ToInt32(pair.Value);
                        break;
                }
            }

            return result;
        }

        public static VariableObject VariableObjectFromDataString(this string data, VariableObject parent, string expression = "")
        {
            var result = new VariableObject();

            result.Expression = expression;

            var pairs = data.ToNameValuePairs();

            foreach (var pair in pairs)
            {
                switch (pair.Name)
                {
                    case "name":
                        result.Id = pair.Value;
                        break;

                    case "numchild":
                        result.NumChildren = Convert.ToInt32(pair.Value);
                        break;

                    case "value":
                        result.Value = pair.Value;
                        break;

                    case "type":
                        result.Type = pair.Value;
                        break;

                    case "thread-id":
                        break;

                    case "has_more":
                        break;

                    case "exp":
                        result.Expression = pair.Value;
                        break;

                    default:
                        Console.WriteLine("Unimplemented variable object field.");
                        break;
                }
            }

            result.Parent = parent;

            return result;
        }

        public static Variable VariableFromDataString(this string data)
        {
            var result = new Variable();

            foreach (var pair in data.ToNameValuePairs())
            {
                switch (pair.Name)
                {
                    case "name":
                        result.Name = pair.Value;
                        break;

                    case "arg":
                        result.IsArgument = pair.Value == "1";
                        break;

                    case "type":
                        result.Type = pair.Value;
                        break;

                    case "value":
                        result.Value = pair.Value;
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }

            return result;
        }

        public static StopRecord StopRecordFromArgumentList(this NameValuePair[] arguments)
        {
            var result = new StopRecord();

            foreach (var arg in arguments)
            {
                switch (arg.Name)
                {
                    case "reason":
                        result.Reason = arg.Value.ToStopReason();
                        break;

                    case "frame":
                        result.Frame = arg.Value.FrameFromDataString();
                        break;

                    case "thread-id":
                        result.ThreadId = Convert.ToUInt32(arg.Value);
                        break;

                    case "stopped-threads":
                        result.StoppedThreads = arg.Value;
                        break;

                    case "bkptno":
                        result.BreakPointNumber = Convert.ToUInt32(arg.Value);
                        break;

                    case "disp":
                        result.KeepBreakPoint = arg.Value == "keep";
                        break;

                    default:
                        //Console.WriteLine ("Unimplemented stop record field detected.");
                        break;
                }
            }

            return result;
        }

        public static byte[] ToByteArray(this string hex)
        {
            var NumberChars = hex.Length;
            var bytes = new byte[NumberChars / 2];

            for (var i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
        }

        public static List<MemoryBytes> MemoryBytesListFromDataString(this string data)
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
                                block.Data = pair.Value.ToByteArray();
                                block.Values = pair.Value;
                                break;
                        }
                    }

                    result.Add(block);
                }
            }

            return result;
        }

        public static Frame FrameFromDataString(this string data)
        {
            var result = new Frame();

            if (data[0] != '{')
            {
                throw new Exception("Object expected to begin with '{'");
            }

            var pairs = data.RemoveBraces().ToNameValuePairs();

            foreach (var pair in pairs)
            {
                switch (pair.Name)
                {
                    case "addr":
                        result.Address = Convert.ToUInt64(pair.Value.Replace("0x", ""), 16);
                        break;

                    case "func":
                        result.Function = pair.Value;
                        break;

                    case "args":
                        result.Arguments = new List<Variable>();

                        var arguments = pair.Value.ToArray();

                        foreach (var argument in arguments)
                        {
                            var newArgument = argument.RemoveBraces().VariableFromDataString();
                            newArgument.IsArgument = true;
                            result.Arguments.Add(newArgument);
                        }
                        break;

                    case "file":
                        result.File = pair.Value;
                        break;

                    case "fullname":
                        result.FullFileName = pair.Value;
                        break;

                    case "line":
                        if (pair.Value != string.Empty)
                        {
                            result.Line = Convert.ToInt32(pair.Value);
                        }
                        break;

                    case "level":
                        if (pair.Value != string.Empty)
                        {
                            result.Level = Convert.ToUInt32(pair.Value);
                        }
                        break;
                }
            }

            return result;
        }

        public static List<InstructionLine> InstructionLineListFromDataString(this string data)
        {
            var result = new List<InstructionLine>();

            var pairs = data.ToNameValuePairs();

            if (data[0] == '{')
            {
                result = new List<InstructionLine>();

                result.Add(data.RemoveBraces().InstructionLineFromDataString());
            }

            return result;
        }

        public static InstructionLine InstructionLineFromDataString(this string data)
        {
            var result = new InstructionLine();

            var pairs = data.ToNameValuePairs();

            foreach (var pair in pairs)
            {
                switch (pair.Name)
                {
                    case "address":
                        result.Address = Convert.ToUInt64(pair.Value.Substring(2), 16);
                        break;

                    case "func-name":
                        result.FunctionName = pair.Value;
                        break;

                    case "offset":
                        result.Offset = Convert.ToInt32(pair.Value);
                        break;

                    case "opcodes":
                        result.OpCodes = pair.Value;
                        break;

                    case "inst":
                        result.Instruction = pair.Value.Replace("\\t", "\t");
                        break;

                    default:
                        break; //throw new NotImplementedException ();
                }
            }

            return result;
        }

        public static LiveBreakPoint LiveBreakPointFromArgumentList(this NameValuePair[] argumentList)
        {
            var result = new LiveBreakPoint();

            foreach (var argument in argumentList)
            {
                switch (argument.Name)
                {
                    case "number":
                        result.Number = Convert.ToInt32(argument.Value);
                        break;

                    case "type":
                        if (argument.Value == "breakpoint")
                        {
                            result.Type = BreakPointType.BreakPoint;
                        }
                        else
                        {
                            throw new Exception("Breakpoint type not implmented.");
                        }
                        break;

                    case "disp":
                        result.Visible = argument.Value == "keep";
                        break;

                    case "enabled":
                        result.Enabled = argument.Value == "y";
                        break;

                    case "addr":
                        if (argument.Value != "<MULTIPLE>")
                        {
                            result.Address = Convert.ToUInt64(argument.Value.Replace("0x", ""), 16);
                        }
                        break;

                    case "func":
                        result.Function = argument.Value;
                        break;

                    case "file":
                        result.File = argument.Value.Replace("\\\\", "\\").NormalizePath();
                        break;

                    case "fullname":
                        result.FullFileName = argument.Value.Replace("\\\\", "\\").NormalizePath();
                        break;

                    case "line":
                        result.Line = Convert.ToUInt32(argument.Value);
                        break;

                    case "times":
                        result.HitCount = Convert.ToInt32(argument.Value);
                        break;

                    case "original-location":
                        result.OriginalLocation = argument.Value;
                        break;

                    case "thread-groups":

                        break;

                    default:
                        Console.WriteLine("Unknown field in breakpoint data");
                        break;
                }
            }

            return result;
        }

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

                            case '{':
                                stateStack.Push(currentState);
                                currentState = NameValueProcessState.parsingObject;
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