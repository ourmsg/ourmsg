using System;
using System.Collections.Generic;
using System.Text;

namespace Helper.Net.RUDP
{
	internal sealed class FragmentInformation
	{
		internal RUDPSocket rudp;
		internal int Size;
		internal int Offset;
		internal byte[] Payload;
		internal bool IsReliable;
		internal RUDPSendIAsyncResult AsyncResult;
		internal RUDPSocketError Error = RUDPSocketError.Success;

		internal FragmentInformation(RUDPSocket rudpSocket, bool isReliable, byte[] payload, int offset, int size, RUDPSendIAsyncResult asyncResult)
		{
			rudp = rudpSocket;
			IsReliable = isReliable;
			Offset = offset;
			Size = size;
			Payload = payload;
			AsyncResult = asyncResult;
		}
	}
}
