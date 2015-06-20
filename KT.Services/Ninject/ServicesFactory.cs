using Ninject;

namespace KT.Services.Ninject
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