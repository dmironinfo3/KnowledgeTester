using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using KT.ServiceInterfaces;

namespace KnowledgeTester.WCFServices
{
	public class KtServices
	{
		private static ServiceClient<IKtAnswersService> _answersService;
		public static IKtAnswersService AnswersService
		{
			get
			{
				return _answersService.Proxy;
			}
		}

		private static ServiceClient<IKtCategoriesService> _categoriesService;
		public static IKtCategoriesService CategoriesService
		{
			get
			{
				return _categoriesService.Proxy;
			}
		}

		private static ServiceClient<IKtQuestionsService> _questionsService;
		public static IKtQuestionsService QuestionsService
		{
			get
			{
				return _questionsService.Proxy;
			}
		}

		private static ServiceClient<IKtSubcategoriesService> _subcategoriesService;
		public static IKtSubcategoriesService SubcategoriesService
		{
			get
			{
				return _subcategoriesService.Proxy;
			}
		}

		private static ServiceClient<IKtTestService> _testService;
		public static IKtTestService TestService
		{
			get
			{
				return _testService.Proxy;
			}
		}

		private static ServiceClient<IKtUsersService> _usersService;
		public static IKtUsersService UsersService
		{
			get
			{
				return _usersService.Proxy;
			}
		}

		private static ServiceClient<IKtUserTestsService> _userTestsService;
		public static IKtUserTestsService UserTestsService
		{
			get
			{
				return _userTestsService.Proxy;
			}
		}

		public static void Init()
		{
			_answersService = new ServiceClient<IKtAnswersService>("BasicHttpBinding_IKtAnswersService");

			_categoriesService = new ServiceClient<IKtCategoriesService>("BasicHttpBinding_IKtCategoriesService");

			_questionsService = new ServiceClient<IKtQuestionsService>("BasicHttpBinding_IKtQuestionsService");

			_subcategoriesService = new ServiceClient<IKtSubcategoriesService>("BasicHttpBinding_IKtSubcategoriesService");

			_testService = new ServiceClient<IKtTestService>("BasicHttpBinding_IKtTestService");

			_userTestsService = new ServiceClient<IKtUserTestsService>("BasicHttpBinding_IKtUserTestsService");

			_usersService = new ServiceClient<IKtUsersService>("BasicHttpBinding_IKtUsersService");
		}
	}
}