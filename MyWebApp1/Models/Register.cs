using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyWebApp1.Models
{
    public class Register
    {
        [DisplayName("帳號")]
        [Required(ErrorMessage = "帳號不可空白")]
        public string PlayerId { get; set; }

        [DisplayName("密碼")]
        [Required]
        public string Password { get; set; }

        [DisplayName("再次輸入密碼")]
        [Required]
        [Compare("Password", ErrorMessage = "輸入密碼必須相同")]
        public string Password2 { get; set; }

        [DisplayName("暱稱")]
        [Required(ErrorMessage = "暱稱不可空白")]
        public string PlayerName { get; set; }
    }
}