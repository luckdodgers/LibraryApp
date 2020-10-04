using LibraryApp.Application.Common.Enums;
using LibraryApp.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Infrastructure.Interfaces
{
    public interface IErrorToStatusCodeConverter
    {
        int Convert(RequestError error);
    }
}