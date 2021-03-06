﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using KT.DB;
using KT.DB.CRUD;
using KT.DTOs.Objects;
using KT.ServiceInterfaces;
using KT.Services.Mappers;

namespace KT.Services.Services
{
	public class KtUsersService : IKtUsersService
	{
		private static readonly ICrud<User> Repository = CrudFactory<User>.Get();

		public UserDto[] GetAll()
		{
			return (new UsersMapper().Map(Repository.ReadArray(a => true))).ToArray();
		}

		public UserDto GetWithTests(string userName)
		{
			var relatedObjects = new[] { "Tests" };

			return (new UsersMapper().Map(Repository.Read(a => a.Username.Equals(userName), relatedObjects)));
		}

		public UserDto GetByKey(string userName)
		{
			var user = Repository.Read(a => a.Username.Equals(userName));

			return user == null ? null : (new UsersMapper()).Map(user);
		}

		public bool Exists(string userName)
		{
			var st = GetByKey(userName);
			return st != null;
		}

		public string GetHint(string userName)
		{
			var st = GetByKey(userName);
			if (st != null)
				return st.PasswordHint;
			return "There aren't any hints for " + userName;
		}

		public UserDto Insert(UserDto st)
		{
			var mapper = new UsersMapper();

			var user = mapper.Map(st);
			var entity = Repository.Create(user);
			return mapper.Map(entity);
		}

		public bool Authenticate(string username, string password)
		{
			var user = GetByKey(username);

			return user != null && user.Password.Equals(password);
		}

		public void Subscribe(string username, Guid testId)
		{
			var relatedObjects = new[] { "Tests" };
			var testRepository = CrudFactory<Test>.Get();

			var user = Repository.Read(a => a.Username.Equals(username), relatedObjects);
			var test = testRepository.Read(a => a.Id.Equals(testId));
			if (user != null)
			{
				var subscriptionsRepository = CrudFactory<User>.GetSubscriptionsRepository();

				subscriptionsRepository.Subscribe(user,test);
			}
		}

		public void Unsubscribe(string username, Guid testId)
		{
			var relatedObjects = new[] { "Tests" };
			var testRepository = CrudFactory<Test>.Get();

			var user = Repository.Read(a => a.Username.Equals(username), relatedObjects);
			var test = testRepository.Read(a => a.Id.Equals(testId));
			if (user != null)
			{
				var subscriptionsRepository = CrudFactory<User>.GetSubscriptionsRepository();

				subscriptionsRepository.Unsubscribe(user, test);
			}
		}

		/// <summary>
		/// Method used for testing purposes, is not accessible from IKtUsersService
		/// </summary>
		/// <param name="id"></param>
		public void Delete(string username)
		{
			Repository.Delete(a=>a.Username==username);
		}
	}
}
