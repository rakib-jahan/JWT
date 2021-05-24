using System;
using System.ComponentModel.DataAnnotations;

namespace JWT.Model
{
    public class UserModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Password { get; set; }
    }
}
