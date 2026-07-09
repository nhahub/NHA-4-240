using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBook.Business.App.DTOs
{
    public class ResetPasswordDto
    {
       public string password {  get; set; }
       public string newPassword { get; set; }
       public int userId { get; set; }
    }
}
