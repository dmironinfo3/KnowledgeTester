using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KnowledgeTester.Helpers;
using KnowledgeTester.Models;
using KnowledgeTester.Ninject;
using KT.DTOs.Objects;
using KT.ServiceInterfaces;

namespace KnowledgeTester.Controllers
{
    public class TestReviewController : Controller
    {
        //
        // GET: /TestReview/

        public ActionResult Index(Guid? id, string user)
        {
			// user rights
			if (!SessionWrapper.UserIsAdmin)
			{
				return RedirectToAction("Index", "Home");
			}

			if (id == null || id == Guid.Empty || String.IsNullOrEmpty(user))
			{
				ViewBag.Message = "Please select a valid test!";
				return RedirectToAction("Index", "AdminPanel");
			}

	        var dto = ServicesFactory.GetService<IKtUserTestsService>().GetTestReview(id.Value, user);

	        var model = new TestReviewModel(dto)
	        {
		        Username = user,
		        TestId = id.Value
	        };


	        return View(model);
        }

	    public ActionResult Validate(Guid testId, string username, int? score)
	    {
		    if (score != null)
		    {
			    ServicesFactory.GetService<IKtUserTestsService>().UpdateScore(testId, username, score.Value);
		    }

			ServicesFactory.GetService<IKtUserTestsService>().Validate(testId, username);

		    return RedirectToAction("Index", "TestResults", new {id = SessionWrapper.CurrentTestResultId});
	    }
    }
}
