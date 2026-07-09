using System;
namespace CodeBook.Business.App.Interfaces
{
    public interface IEmailService
    {
        void SendVerificationEmail(string email, string cerificationLink);
        void SendPasswordResetEmail(string email, string resetLink);
    }
}
