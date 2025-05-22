using Microsoft.AspNetCore.Mvc;
using BookApi.Models;
using BookApi.Services;

namespace BookApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly BookService _bookService;

        public BooksController(BookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public ActionResult<List<Book>> GetAll() => _bookService.GetAll();

        [HttpGet("{id}")]
        public ActionResult<Book> Get(int id)
        {
            var book = _bookService.Get(id);
            if (book == null) return NotFound();
            return book;
        }

        [HttpPost]
        public IActionResult Create(Book book)
        {
            _bookService.Add(book);
            return CreatedAtAction(nameof(Get), new { id = book.Id }, book);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Book book)
        {
            var existing = _bookService.Get(id);
            if (existing == null) return NotFound();
            _bookService.Update(id, book);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existing = _bookService.Get(id);
            if (existing == null) return NotFound();
            _bookService.Delete(id);
            return NoContent();
        }
    }
}
