﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using HobiHobi.Core.Framework;
using System.Web;

namespace HobiHobi.Core.Identity
{
    public class User
    {
        public static Key NewId(string value = null)
        {
            return Key.Generate("User/", value);
        }

        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public AccountStatus Status { get; set; }
        public AccountLevel Level { get; set; }
        public DateTime DateCreated { get; set; }

        public static string HashPassword(string password)
        {
            var hash = BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(15));
            return hash;
        }

        public static bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);

        }

        public UserInfo GetUserInfo()
        {
            var info = new UserInfo
            {
                Id = Id,
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                Level = Level
            };

            return info;
        }
    }
}
