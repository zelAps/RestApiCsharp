using BookApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace BookApi.Services
{
    public class BookService
    {
        private static List<Book> Books = new List<Book>()
        {
            new Book { Id = 1, Title = "Clean Code", Author = "Robert C. Martin", Year = 2008 },
            new Book { Id = 2, Title = "The Pragmatic Programmer", Author = "Andrew Hunt", Year = 1999 }
        };

        public List<Book> GetAll() => Books;

        public Book? Get(int id) => Books.FirstOrDefault(b => b.Id == id);

        public void Add(Book book)
        {
            book.Id = Books.Max(b => b.Id) + 1;
            Books.Add(book);
        }

        public void Update(int id, Book book)
        {
            var index = Books.FindIndex(b => b.Id == id);
            if (index == -1) return;
            book.Id = id;
            Books[index] = book;
        }

        public void Delete(int id)
        {
            var book = Get(id);
            if (book != null)
            {
                Books.Remove(book);
            }
        }
    }
}
