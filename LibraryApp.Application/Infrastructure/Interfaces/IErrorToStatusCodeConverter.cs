using LibraryApp.Application.Common.Enums;

namespace LibraryApp.Infrastructure.Interfaces
{
    public interface IErrorToStatusCodeConverter
    {
        int Convert(RequestError error);
    }
}