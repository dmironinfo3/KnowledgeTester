using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KT.Logger;
using KT.ServiceInterfaces;
using KnowledgeTester.Code;
using KnowledgeTester.WCFServices;
using Ninject;
using Ninject.Modules;

namespace KnowledgeTester.Ninject
{
	public class Bindings
	{
		public IKernel LoadNinjectBindings()
		{
			var kernel = new StandardKernel();

			kernel.Bind<IKtAnswersService>().ToMethod(context => KtServices.AnswersService);
			kernel.Bind<IKtCategoriesService>().ToMethod(context => KtServices.CategoriesService);
			kernel.Bind<IKtQuestionsService>().ToMethod(context => KtServices.QuestionsService);
			kernel.Bind<IKtSubcategoriesService>().ToMethod(context => KtServices.SubcategoriesService);
			kernel.Bind<IKtTestService>().ToMethod(context => KtServices.TestService);
			kernel.Bind<IKtUsersService>().ToMethod(context => KtServices.UsersService);
			kernel.Bind<IKtUserTestsService>().ToMethod(context => KtServices.UserTestsService);

			kernel.Bind<ILogger>().ToMethod(context => KtLogger.Log);

			return kernel;
		}
	}
}