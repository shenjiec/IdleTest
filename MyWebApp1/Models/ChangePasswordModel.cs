using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyWebApp1.Models
{
    public class ChangePasswordModel
    {
        [DisplayName("輸入舊密碼")]
        [Required]
        public string OldPassword { get; set; }

        [DisplayName("輸入新密碼密碼")]
        [Required]
        public string NewPassword { get; set; }

        [DisplayName("再次輸入密碼")]
        [Required]
        [Compare("NewPassword", ErrorMessage = "輸入新密碼必須相同")]
        public string NewPassword2 { get; set; }
    }
}