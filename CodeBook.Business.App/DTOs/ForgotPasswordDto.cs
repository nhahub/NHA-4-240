using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBook.Business.App.DTOs
{
    public class ForgotPasswordDto
    {
        public string newPassword { get; set; }
        public string email { get; set; }
    }
}