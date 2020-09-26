using LibraryApp.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Application.Books.Commands.AddBookToLibrary
{
    public class AddBookCommand : IRequest<Result>
    {
        public string Title { get; set; }
        public List<string> Authors { get; set; }
    }
}