using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KT.ServiceInterfaces;
using Ninject;

namespace KnowledgeTester.Ninject
{
	public static class ServicesFactory
	{
		private static IKernel _kernel;

		public static void Init(IKernel kernel)
		{
			_kernel = kernel;
		}

		public static T GetService<T>()
		{
			return (T)_kernel.TryGet(typeof(T));
		}
	}
}