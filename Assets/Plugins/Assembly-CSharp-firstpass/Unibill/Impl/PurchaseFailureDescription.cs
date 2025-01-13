using System;
using System.Collections.Generic;

namespace Unibill.Impl
{
	public class PurchaseFailureDescription
	{
		public string ProductId { get; private set; }

		public PurchaseFailureReason Reason { get; private set; }

		public string Message { get; private set; }

		public PurchaseFailureDescription(string jsonHash)
		{
			Dictionary<string, object> dic = jsonHash.hashtableFromJson();
			ProductId = dic.getString("productId", string.Empty);
			string @string = dic.getString("reason", string.Empty);
			if (Enum.IsDefined(typeof(PurchaseFailureReason), @string))
			{
				Reason = (PurchaseFailureReason)Enum.Parse(typeof(PurchaseFailureReason), @string);
			}
			else
			{
				Reason = PurchaseFailureReason.UNKNOWN;
			}
			Message = dic.getString("message", string.Empty);
		}

		public PurchaseFailureDescription(string productId, PurchaseFailureReason reason, string message)
		{
			ProductId = productId;
			Reason = reason;
			Message = message;
		}
	}
}
