using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KnowledgeTester.Helpers;

namespace KnowledgeTester.Controllers
{
    public class BaseController : Controller
    {
	    public bool AdminAllowed
	    {
		    get { return SessionWrapper.CurrentUserRights == UserRights.Admin; }
	    }

		public bool UserAllowed
		{
			get { return SessionWrapper.CurrentUserRights == UserRights.User; }
		}
    }
}
