using System;

namespace CodeBook.Business.App.DTOs
{
	public class UpdateProfileDto
    {
		public string UserName{ get; set; }
		public string Bio { get; set; }
		public string AvatarUrl { get; set; }
	}
}
