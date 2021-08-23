using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CAF.Core.Enums
{
    public enum CachePath : int
    {
        [Display(Name = "/api/Users/Login")]
        UsersLogin = 1,
        [Display(Name = "/api/Users/SearchAutoComplete")]
        SearchAutoComplete = 2,
        [Display(Name = "/api/Users/GetUIMenu")]
        GetUIMenu = 3
    }
}
