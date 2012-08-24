using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HobiHobi.Core.Identity;

namespace HobiHobi.Core.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }
        public AccountStatus Status { get; set; }

        public UserViewModel()
        {

        }

        public UserViewModel(User usr)
        {
            Id = usr.Id;
            FirstName = usr.FirstName;
            LastName = usr.LastName;
            Password = usr.Password;
            RepeatPassword = usr.Password;
            Status = usr.Status;
        }
    }
}
