using CodeBook.Business.App.DTOs;
using System;

namespace CodeBook.Business.App.Interfaces
{
	public interface IModerationService
    {
        void RemovePost(int PostId, int reportId,int removerId);
        void RemoveComment(int PostId, int reportId, int removerId);
    }
}
