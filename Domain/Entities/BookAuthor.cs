namespace LibraryApp.Domain.Entities
{
    public class BookAuthor
    {
        public BookAuthor(int authorId, Author author, int bookId, Book book)
        {
            AuthorId = authorId;
            Author = author;
            BookId = bookId;
            Book = book;
        }

        private BookAuthor() { }

        public int AuthorId { get; }
        public Author Author { get; }
        public int BookId { get; }
        public Book Book { get; }
    }
}