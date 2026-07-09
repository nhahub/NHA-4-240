
namespace CodeBook.Business.App.Middleware
{
	public class ErrorResponse
	{
		public bool Success { get; set; }
		//public int StatusCode { get; set; }
		public string Message { get; set; } = string.Empty;
		//public string? Details { get; set; }
	}
}