using System;
using System.Linq;
using System.Linq.Expressions;
using KT.DTOs.Objects;
using KT.Services.Services;
using NUnit.Framework;

namespace KT.Services.Tests
{
	[TestFixture]
	public class KtFullFlow
	{
		private KtUsersService _userService;
		private UserDto _admin;
		private UserDto _user;
		private KtCategoriesService _categoryService;
		private CategoryDto _category;
		private CategoryDto _category2;

		private KtSubcategoriesService _subcategoryService;
		private SubcategoryDto _subcategory;
		private KtQuestionsService _questionService;
		private QuestionDto _question;
		private AnswerDto _answer;
		private KtAnswersService _answerService;
		private SubcategoryDto _subcategory2;
		private KtTestService _testSevice;
		private TestDto _test;
		private KtUserTestsService _userTestService;
		private GeneratedTestDto _generatedTest;

		[TestFixtureSetUp]
		public void Init()
		{
			_userService = new KtUsersService();

			_categoryService = new KtCategoriesService();

			_subcategoryService = new KtSubcategoriesService();

			_questionService = new KtQuestionsService();

			_answerService = new KtAnswersService();

			_testSevice = new KtTestService();

			_userTestService = new KtUserTestsService();
		}

		#region [A - User service]
		[Test]
		public void A_Add_Admin()
		{
			//arrange
			_admin = new UserDto
			{
				FirstName = "TestAdmin",
				LastName = "TestAdmin",
				Email = "Test@Admin.com",
				IsAdmin = true,
				Password = "TestAdminPass",
				PasswordHint = "TestAdminHint",
				Username = "testadmin"
			};

			//act
			_userService.Insert(_admin);
			var valid = _userService.Authenticate(_admin.Username, _admin.Password);

			//assert
			Assert.That(valid, Is.True);
		}

		[Test]
		public void A_Add_Student()
		{
			//arrange
			_user = new UserDto
			{
				FirstName = "Test",
				LastName = "Test",
				Email = "Test@user.com",
				IsAdmin = false,
				Password = "TestPass",
				PasswordHint = "TestHint",
				Username = "test"
			};

			//act
			_userService.Insert(_user);
			var valid = _userService.Authenticate(_user.Username, _user.Password);

			//assert
			Assert.That(valid, Is.True);
		}

		[Test]
		public void A1_Get_All_Users()
		{
			//arrange

			//act
			var all = _userService.GetAll();

			//assert
			Assert.That(all, Is.Not.Null);

			Assert.That(all.Length, Is.GreaterThanOrEqualTo(1));

			Assert.That(all.Select(a => a.Username).Contains(_admin.Username), Is.True);
		}

		[Test]
		public void A2_Get_Hint()
		{
			//arrange

			//act
			var hint = _userService.GetHint(_admin.Username);

			//assert
			Assert.That(hint, Is.Not.Null);

			Assert.That(hint, Is.EqualTo(_admin.PasswordHint));
		}

		[Test]
		public void A2_1_Get_Hint()
		{
			//arrange

			//act
			var hint = _userService.GetHint("unexistingUser");

			//assert
			Assert.That(hint, Is.Not.Null);

			Assert.That(hint.Contains("There aren't any hints for"), Is.True);
		}

		[Test]
		public void A3_Is_Existent()
		{
			//arrange

			//act
			var exists = _userService.Exists(_admin.Username);

			//assert
			Assert.That(exists, Is.True);
		}
		#endregion

		#region [B - Category service]
		[Test]
		public void B_Add_Category()
		{
			//arrange
			_category = new CategoryDto
			{
				CreatedBy = _admin.Username,
				Id = Guid.NewGuid(),
				Name = "TestCategory",
			};

			//act
			_categoryService.Insert(_category);
			var cat = _categoryService.GetById(_category.Id);

			//assert
			Assert.That(cat, Is.Not.Null);

			Assert.That(cat.Name, Is.EqualTo(_category.Name));

			Assert.That(cat.CreatedBy, Is.EqualTo(_category.CreatedBy));
		}

		[Test]
		public void B1_Get_All_Categories()
		{
			//arrange

			//act
			var categories = _categoryService.GetAll();

			//assert
			Assert.That(categories, Is.Not.Null);

			Assert.That(categories.Length, Is.GreaterThanOrEqualTo(1));

			Assert.That(categories.Select(a => a.Id).Contains(_category.Id), Is.True);
		}

		[Test]
		public void B2_EditCategory()
		{
			//arrange
			_category.Name = "updated_category_name";

			//act
			var categories = _categoryService.Save(_admin.Username, _category.Name, _category.Id);
			var cat = _categoryService.GetById(_category.Id);

			//assert
			Assert.That(cat, Is.Not.Null);

			Assert.That(cat.Name, Is.EqualTo(_category.Name));
		}

		[Test]
		public void B3_Save_Empty_Category()
		{
			//arrange
			_category2 = new CategoryDto
			{
				CreatedBy = _admin.Username,
				Name = "EmptyTestCategory",
			};

			//act
			_category2.Id = _categoryService.Save(_admin.Username, _category2.Name);
			var cat = _categoryService.GetById(_category2.Id);

			//assert
			Assert.That(cat, Is.Not.Null);

			Assert.That(cat.Id, Is.EqualTo(_category2.Id));

			Assert.That(cat.Name, Is.EqualTo(_category2.Name));
		}
		#endregion

		#region [C - Subcategory service]
		[Test]
		public void C_Add_SubCategory()
		{
			//arrange
			_subcategory = new SubcategoryDto()
			{
				Id = Guid.NewGuid(),
				Name = "TestSubCategory",
				CategoryId = _category.Id,
			};

			//act
			_subcategoryService.Insert(_subcategory);
			var subCat = _subcategoryService.GetById(_subcategory.Id);

			//assert
			Assert.That(subCat, Is.Not.Null);

			Assert.That(subCat.Name, Is.EqualTo(_subcategory.Name));

			Assert.That(subCat.CategoryId, Is.EqualTo(_category.Id));
		}

		[Test]
		public void C1_Get_All()
		{
			//arrange


			//act
			var subCats = _subcategoryService.GetAll();

			//assert
			Assert.That(subCats, Is.Not.Null);

			Assert.That(subCats.Length, Is.GreaterThanOrEqualTo(1));

			Assert.That(subCats.Select(a => a.Id).Contains(_subcategory.Id), Is.True);
		}

		[Test]
		public void C2_Get_By_Category()
		{
			//arrange


			//act
			var subCats = _subcategoryService.GetByCategory(_category.Id);

			//assert
			Assert.That(subCats, Is.Not.Null);

			Assert.That(subCats.Length, Is.GreaterThanOrEqualTo(1));

			Assert.That(subCats.Select(a => a.Id).Contains(_subcategory.Id), Is.True);
		}

		[Test]
		public void C3_Get_Count_By_Category()
		{
			//arrange


			//act
			var count = _subcategoryService.GetCountByCategory(_category.Id);

			//assert
			Assert.That(count, Is.Not.Null);

			Assert.That(count, Is.GreaterThanOrEqualTo(1));
		}

		[Test]
		public void C4_EditSubcategory()
		{
			//arrange
			_subcategory.Name = "updated_subcategory_name";

			//act
			_subcategoryService.Save(_subcategory.Name, _category.Id, _subcategory.Id);
			var subcat = _subcategoryService.GetById(_subcategory.Id);

			//assert
			Assert.That(subcat, Is.Not.Null);

			Assert.That(subcat.Name, Is.EqualTo(_subcategory.Name));
		}

		[Test]
		public void C5_Save_Empty_Subcategory()
		{
			//arrange
			_subcategory2 = new SubcategoryDto
			{
				Name = "EmptyTestSubCategory",
				CategoryId = _category.Id,
			};

			//act
			_subcategory2.Id = _subcategoryService.Save(_subcategory2.Name, _category.Id);
			var subcat = _subcategoryService.GetById(_subcategory2.Id);

			//assert
			Assert.That(subcat, Is.Not.Null);

			Assert.That(subcat.Id, Is.EqualTo(_subcategory2.Id));

			Assert.That(subcat.Name, Is.EqualTo(_subcategory2.Name));
		}
		#endregion

		#region [D - Question Service]
		[Test]
		public void D_Add_Question()
		{
			//arrange
			_question = new QuestionDto()
			{
				Argument = "QuestionArgument",
				MultipleResponse = true,
				SubcategoryId = _subcategory.Id,
				Text = "QuestionText"
			};

			//act - inserting blank question
			_question.Id = _questionService.Save(_question.Text, _question.SubcategoryId,
				argument: _question.Argument);

			var question = _questionService.GetById(_question.Id);

			//assert
			Assert.That(question, Is.Not.Null);

			Assert.That(question.Text, Is.EqualTo(_question.Text));

			Assert.That(question.Argument, Is.EqualTo(_question.Argument));
		}

		[Test]
		public void D1_Get_All_Questions()
		{
			//arrange	

			//act
			var questions = _questionService.GetAll();

			//assert
			Assert.That(questions, Is.Not.Null);

			Assert.That(questions.Length, Is.GreaterThanOrEqualTo(1));

			Assert.That(questions.Select(a => a.Id).Contains(_question.Id), Is.True);
		}

		[Test]
		public void D2_Get_By_Subcategory()
		{
			//arrange	

			//act
			var questions = _questionService.GetBySubcategory(_subcategory.Id);

			//assert
			Assert.That(questions, Is.Not.Null);

			Assert.That(questions.Length, Is.GreaterThanOrEqualTo(1));

			Assert.That(questions.Select(a => a.Id).Contains(_question.Id), Is.True);
		}

		[Test]
		public void D3_Get_Count_By_Subcategory()
		{
			//arrange	

			//act
			var count = _questionService.GetCountBySubcategory(_subcategory.Id);

			//assert
			Assert.That(count, Is.Not.Null);

			Assert.That(count, Is.GreaterThanOrEqualTo(1));
		}

		[Test]
		public void D4_Edit_Question()
		{
			//arrange	
			_question.Text = "updatedQuestionText";

			//act
			_question.Id = _questionService.Save("updatedQuestionText", _subcategory.Id, _question.Id);
			var question = _questionService.GetById(_question.Id);
			//assert
			Assert.That(question, Is.Not.Null);

			Assert.That(question.Text, Is.EqualTo(_question.Text));
		}
		#endregion

		#region [E - Answer service]
		[Test]
		public void E_Add_Answer()
		{
			//arrange
			_answer = new AnswerDto
			{
				IsCorrect = true,
				QuestionId = _question.Id,
				Text = "TestAnswer"
			};

			//act - inserting answer
			_answer.Id = _answerService.Save(_answer.Id, _answer.QuestionId, _answer.Text, _answer.IsCorrect);

			var question = _questionService.GetById(_question.Id);

			//assert
			Assert.That(question, Is.Not.Null);

			Assert.That(question.Answers, Is.Not.Null);

			Assert.That(question.Answers.Count(), Is.GreaterThanOrEqualTo(1));
		}

		[Test]
		public void E1_Add_Empty_Answer()
		{
			//arrange

			//act - inserting answer
			_answerService.AddEmpyFor(_question.Id);

			var question = _questionService.GetById(_question.Id);

			//assert
			Assert.That(question, Is.Not.Null);

			Assert.That(question.Answers, Is.Not.Null);

			Assert.That(question.Answers.Count(), Is.GreaterThanOrEqualTo(2));
		}

		[Test]
		public void E2_Edit_Answer()
		{
			//arrange
			_answer.Text = "updatedAnswerText";

			//act - inserting answer
			_answerService.Save(_answer.Id, _question.Id, _answer.Text, _answer.IsCorrect);

			var question = _questionService.GetById(_question.Id);

			//assert
			Assert.That(question, Is.Not.Null);

			Assert.That(question.Answers, Is.Not.Null);

			Assert.That(question.Answers.First(a => a.Id == _answer.Id).Text, Is.EqualTo(_answer.Text));
		}
		#endregion

		#region [F - Test Service]

		[Test]
		public void F_Add_Test()
		{
			//arrange
			_test = new TestDto()
			{
				Name = "Test",
				Duration = 10,
				StartTime = DateTime.Now,
				EndTime = DateTime.Now.AddHours(1),
				SubcategoryId = _subcategory.Id,
				Questions = 1
			};

			//act - inserting answer
			_test.Id = _testSevice.Save(_test.Name, _test.StartTime, _test.EndTime, _test.Duration, _test.SubcategoryId,
				_test.Questions);

			var test = _testSevice.GetById(_test.Id);

			//assert
			Assert.That(test, Is.Not.Null);

			Assert.That(test.Name, Is.EqualTo(_test.Name));

			Assert.That(test.Duration, Is.EqualTo(_test.Duration));
		}

		[Test]
		public void F2_Get_All_Tests()
		{
			//arrange

			//act - inserting answer
			var tests = _testSevice.GetAll();

			//assert
			Assert.That(tests, Is.Not.Null);

			Assert.That(tests.Count(), Is.GreaterThanOrEqualTo(1));

			Assert.That(tests.Select(a => a.Id).Contains(_test.Id), Is.True);
		}

		[Test]
		public void F3_Get_Subcategory_Name()
		{
			//arrange

			//act - inserting answer
			var subCatName = _testSevice.GetSubcategoryName(_test.Id);

			//assert
			Assert.That(subCatName, Is.Not.Null);

			Assert.That(subCatName, Is.EqualTo(_subcategory.Name));
		}

		[Test]
		public void F4_Edit_Test()
		{
			//arrange
			_test.Name = "updatedTestName";


			//act - inserting answer
			_testSevice.Save(_test.Name, _test.StartTime, _test.EndTime, _test.Duration, _subcategory.Id, _test.Questions, _test.Id);
			var test = _testSevice.GetById(_test.Id);

			//assert
			Assert.That(test, Is.Not.Null);

			Assert.That(test.Name, Is.EqualTo(_test.Name));
		}

		#endregion

		#region [G - UserTests service]

		[Test]
		public void G_Get_All_Other_Than()
		{
			//arrange

			//act - inserting answer
			var tests = _testSevice.GetAllOtherThan(_user.Username);

			//assert
			Assert.That(tests, Is.Not.Null);

			Assert.That(tests.Length, Is.GreaterThanOrEqualTo(1));

			Assert.That(tests.Select(a => a.Id).Contains(_test.Id), Is.True);
		}

		[Test]
		public void G2_Subscribe_Unsubscribe()
		{
			//subscribe
			_userService.Subscribe(_user.Username, _test.Id);
			var upcoming = _testSevice.GetAllUpcoming(_user.Username);

			Assert.That(upcoming, Is.Not.Null);

			Assert.That(upcoming.Length, Is.GreaterThanOrEqualTo(1));

			Assert.That(upcoming.Select(a => a.Id).Contains(_test.Id), Is.True);

			//unsubscribe
			_userService.Unsubscribe(_user.Username, _test.Id);
			upcoming = _testSevice.GetAllUpcoming(_user.Username);

			//assert
			Assert.That(upcoming, Is.Not.Null);

			Assert.That(upcoming.Length, Is.EqualTo(0));

			_userService.Subscribe(_user.Username, _test.Id);
		}

		[Test]
		public void G3_Get_subscriptions_For()
		{
			var subscriptions = _testSevice.GetSubscriptionsFor(_test.Id);

			Assert.That(subscriptions, Is.Not.Null);

			Assert.That(subscriptions, Is.GreaterThanOrEqualTo(1));
		}

		[Test]
		public void G4_Get_With_Tests()
		{
			var user = _userService.GetWithTests(_user.Username);

			Assert.That(user, Is.Not.Null);

			Assert.That(user.Subscriptions.Count(), Is.GreaterThanOrEqualTo(1));

			Assert.That(user.Subscriptions.Select(a => a.Id).Contains(_test.Id), Is.True);
		}

		[Test]
		public void G5_GenerateTest()
		{
			_generatedTest = _userTestService.GenerateTest(_test.Id, _user.Username);

			var notExistingTest = _userTestService.GenerateTest(Guid.NewGuid(), _user.Username);

			Assert.That(_generatedTest, Is.Not.Null);

			Assert.That(_generatedTest.GeneratedQuestions.Count(), Is.EqualTo(1));

			Assert.That(notExistingTest, Is.Null);
		}

		[Test]
		public void G6_GetUsability()
		{
			var usability = _questionService.GetUsability(_question.Id);
			var zeroUsability = _questionService.GetUsability(Guid.NewGuid());
			
			Assert.That(usability, Is.Not.Null);

			Assert.That(usability, Is.EqualTo(100));

			Assert.That(zeroUsability, Is.Not.Null);

			Assert.That(zeroUsability, Is.EqualTo(0));
		}

		[Test]
		public void G7_Is_Test_Generated()
		{
			GeneratedTestDto outVal;
			var generated = _userTestService.IsTestGenerated(_test.Id,_user.Username,out outVal);

			Assert.That(generated, Is.Not.Null);

			Assert.That(generated, Is.True);
		}

		#endregion

		[TestFixtureTearDown]
		public void TearDown()
		{
			_testSevice.Delete(_test.Id);

			_subcategoryService.Delete(_subcategory.Id);

			_subcategoryService.Delete(_subcategory2.Id);

			_categoryService.Delete(_category.Id);

			_categoryService.Delete(_category2.Id);

			_userService.Delete(_admin.Username);

			_userService.Delete(_user.Username);

			_questionService.Delete(_question.Id);

			_answerService.Delete(_answer.Id);
		}
	}
}
