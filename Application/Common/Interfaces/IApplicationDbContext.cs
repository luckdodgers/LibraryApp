using LibraryApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryApp.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Book> Books { get; set; }
        DbSet<Author> Authors { get; set; }
        DbSet<BookAuthor> BookAuthors { get; set; }
        DbSet<Card> Cards { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
