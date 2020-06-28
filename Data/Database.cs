using System;
using System.Collections.Generic;
using System.Linq;
using VueCoreJwt.App;
using VueCoreJwt.Models;

namespace VueCoreJwt.Data
{
	public class Database : IDatabase
	{
		private List<AppUser> Users { get; set; }

		public Database()
		{
			var salt = Security.GenerateSalt();
			var password = Security.HashPassword("password", salt);

			Users = new List<AppUser>
			{
				new AppUser
				{
					Id = 1,
					Name = "Idris Elba",
					Email = "admin@test.com",
					LastLogin = DateTime.Now.AddDays(-10),
					CustomInfo = "52 cats",
					Role = "Admin",
					Salt = salt,
					PasswordHash = password,
					EmailToken = null,
					Active = true,
				},
				new AppUser
				{
					Id = 2,
					Name = "Halle Berry",
					Email = "normal@test.com",
					LastLogin = DateTime.Now.AddDays(-1),
					CustomInfo = "2 dogs",
					Role = "Normal",
					Salt = salt,
					PasswordHash = password,
					EmailToken = null,
					Active = true,
				},
				new AppUser
				{
					Id = 3,
					Name = "Gene Kelly",
					Email = "unconfirmed@test.com",
					LastLogin = null,
					CustomInfo = "likes rain",
					Role = "Normal",
					Salt = salt,
					PasswordHash = password,
					EmailToken = "db06011dca3a4276aaba2fab9547286b",
					Active = false,
				},
				new AppUser
				{
					Id = 4,
					Name = "Audry Hepburn",
					Email = "pwreset@test.com",
					LastLogin = DateTime.Now.AddDays(-5),
					CustomInfo = "",
					Role = "Normal",
					Salt = salt,
					PasswordHash = password,
					EmailToken = "272065a0222948c2ad5b6cdefb11066e",
					Active = true,
				}
			};
		}

		public int AddUser(AppUser user)
		{
			user.Id = Users.Count + 1;
			Users.Add(user);
			return user.Id;
		}

		public AppUser GetUserById(int id)
		{
			return Users.SingleOrDefault(u => u.Id == id);
		}

		public AppUser GetUserByEmail(string email)
		{
			return Users.SingleOrDefault(u => u.Email == email);
		}

		public AppUser GetUserByToken(string token)
		{
			return Users.SingleOrDefault(u => u.EmailToken == token);
		}

		public void UpdateUser(AppUser user)
		{
			Users[Users.FindIndex(u => u.Id == user.Id)] = user;
		}
	}
}
