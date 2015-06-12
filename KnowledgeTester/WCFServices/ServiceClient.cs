using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Web;

namespace KnowledgeTester.WCFServices
{
	public class ServiceClient<T> : ClientBase<T> where T : class
	{
		private readonly Object _thisLock = new Object();

		public T Proxy
		{
			get
			{
				this.EnsureOpened();
				return Channel;
			}
		}

		private void EnsureOpened()
		{
			lock (_thisLock)
			{
				if (this.State == CommunicationState.Created
					&& this.ChannelFactory.State == CommunicationState.Created)
				{
					this.Open();
				}
			}

			if (this.State != CommunicationState.Opened)
			{
				//TODO log the error
			}
		}

		public ServiceClient(string configurationName) :
			base(configurationName)
		{ }
	}
}