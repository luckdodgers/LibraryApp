using LibraryApp.Application.Common.Models;
using MediatR;
using System.Collections.Generic;

namespace LibraryApp.Application.Books.Commands.AddBookToLibrary
{
    public class AddBookToLibraryCommand : IRequest<BaseResult>
    {
        public string Title { get; set; }
        public List<string> Authors { get; set; }
    }
}