using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HobiHobi.Core.Framework;
using Raven.Client;

namespace HobiHobi.Core.Identity
{
    public class Authentication
    {
        IDocumentSession _session;

        public Authentication(IDocumentSession session)
        {
            _session = session;
        }

        /// <summary>
        /// Authenticate a user login 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Result2<AuthenticationResult, User> Authenticate(string username, string password)
        {
            var res = _session.Query<User>().Where(x => x.Email == username).FirstOrDefault();

            if (res == null)
                return Result2<AuthenticationResult, User>.True(AuthenticationResult.UsernameNotFound, new User());
            else
            {
                bool isMatch = User.VerifyPassword(password, res.Password);
                if (isMatch)
                {
                    if (res.Status == AccountStatus.Disabled)
                        return Result2<AuthenticationResult, User>.True(AuthenticationResult.AccountDisabled, new User());
                    else if (res.Status == AccountStatus.Enabled)
                        return Result2<AuthenticationResult, User>.True(AuthenticationResult.OK, res);
                    else
                        return Result2<AuthenticationResult, User>.True(AuthenticationResult.Unknown, res);
                }
                else
                    return Result2<AuthenticationResult, User>.True(AuthenticationResult.PasswordDoNotMatch, new User());
            }
        }
    }
}
