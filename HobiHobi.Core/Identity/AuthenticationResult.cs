using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HobiHobi.Core.Identity
{
    public enum AuthenticationResult
    {
        UsernameNotFound,
        PasswordDoNotMatch,
        AccountDisabled,
        Unknown,
        OK
    }
}
