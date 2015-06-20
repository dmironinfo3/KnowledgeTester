using KT.EmailSender;
using KT.ServiceInterfaces;
using KT.Services.Helpers;
using Ninject;

namespace KT.Services.Ninject
{
	public class Bindings
	{
		public IKernel LoadNinjectBindings()
		{
			var kernel = new StandardKernel();

			kernel.Bind<IEmailSender>().ToMethod(context => KtEmailSender.Sender);

			return kernel;
		}
	}
}