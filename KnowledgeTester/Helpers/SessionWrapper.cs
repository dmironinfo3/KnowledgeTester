using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KT.DTOs.Objects;
using KnowledgeTester.Models;

namespace KnowledgeTester.Helpers
{
	public class SessionWrapper
	{
		private const string UserIsLoggedInId = "IsUserLoggedId";
		public static bool UserIsLoggedIn
		{
			get
			{
				return Convert.ToBoolean(HttpContext.Current.Session[UserIsLoggedInId]);
			}
			set { HttpContext.Current.Session[UserIsLoggedInId] = value; }
		}

		private const string UserIsAdminId = "UserIsAdmin";
		public static bool UserIsAdmin
		{
			get
			{
				return Convert.ToBoolean(HttpContext.Current.Session[UserIsAdminId]);
			}
			set { HttpContext.Current.Session[UserIsAdminId] = value; }
		}

		private const string StudentId = "StudentId";
		public static UserDto User
		{
			get
			{
				return (UserDto)HttpContext.Current.Session[StudentId];
			}
			set
			{
				UserIsLoggedIn = true; HttpContext.Current.Session[StudentId] = value;
			}
		}

		private const string CurrentCategoryIdId = "CurrentCategoryIdId";
		public static Guid CurrentCategoryId
		{
			get
			{
				return (Guid)HttpContext.Current.Session[CurrentCategoryIdId];
			}
			set
			{
				HttpContext.Current.Session[CurrentCategoryIdId] = value;
			}
		}

		private const string CurrentSubategoryIdId = "CurrentSubategoryIdId";
		public static Guid CurrentSubcategoryId
		{
			get
			{
				return (Guid)HttpContext.Current.Session[CurrentSubategoryIdId];
			}
			set
			{
				HttpContext.Current.Session[CurrentSubategoryIdId] = value;
			}
		}

		private const string CurrentQuestionAnswersId = "CurrentQuestionAnswersId";
		public static List<AnswerModel> CurrentQuestionAnswers
		{
			get
			{
				return (List<AnswerModel>)HttpContext.Current.Session[CurrentQuestionAnswersId];
			}
			set
			{
				HttpContext.Current.Session[CurrentQuestionAnswersId] = value;
			}
		}

		private const string CurrentTestResultIdId = "CurrentTestResultIdId";
		public static Guid CurrentTestResultId
		{
			get
			{
				return (Guid)HttpContext.Current.Session[CurrentTestResultIdId];
			}
			set
			{
				HttpContext.Current.Session[CurrentTestResultIdId] = value;
			}
		}

		public static UserRights CurrentUserRights
		{
			get
			{
				if (User != null && UserIsAdmin)
					return UserRights.Admin;
				if (User != null)
					return UserRights.User;
				return UserRights.None;
			}
		}

		internal static void Logout()
		{
			HttpContext.Current.Session.Clear();
		}
	}
}