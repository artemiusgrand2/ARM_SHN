using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace TrafficTrain.Impulsesver.Client.requests
{
	[StructLayout(LayoutKind.Explicit, Size=8)]
	unsafe struct ErrorRequestAnswer
	{
		public const int Size = 8;

		[FieldOffset(0)]
		public RequestHeader Header;

		[FieldOffset(4)]
		public short FailedRequestType;

		[FieldOffset(6)]
		public short Error;
	}
}