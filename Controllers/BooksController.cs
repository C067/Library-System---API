using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assignment_2_PRO670.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Assignment_2_PRO670.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibraryDbContext dbContext;

        //Constructor, set the dbContext to the database
        public BooksController(LibraryDbContext libraryDbContext)
        {
            dbContext = libraryDbContext;
        }

        //GetBooks()
        //Return a list of Books to the user, each book being a BookDTO Object
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooks()
        {
            return await dbContext.Books.Select(b => new BookDTO(b)).ToListAsync();
        }

        //GetBookById(int id) 
        //Get the book from the database and return a BookDTO object 
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDTO>> GetBookById(int id)
        {
            //Using the id find the Book 
            var book = await dbContext.Books.FindAsync(id);

            //Compare if the book was returned
            if (book == null)
            {
                //Return a Not Found response if the book wasn't found
                return NotFound();
            }

            //Return the book as a BookDTO object
            return new BookDTO(book);
        }

        //CreateBook(Book newBook) 
        //Using the request body, create a new Book entry and return the book to the user
        [HttpPost]
        public async Task<ActionResult<BookDTO>> CreateBook(Book newBook)
        {
            //Using the libraryId, find the library in the database
            Library library = await dbContext.Libraries.FindAsync(newBook.LibraryId);

            //Compare if the library was found
            if (library == null)
            {
                //Return a Not Found response if the Library wasn't found
                return NotFound();
            }
            
            //Update the title and set the LibraryId and reference to the new book
            newBook.Title += " (verified at: " + DateTime.Now + ")";
            newBook.Library = library;
            newBook.LibraryId = library.LibraryId;

            //Add the book to the database
            dbContext.Books.Add(newBook);
            await dbContext.SaveChangesAsync();
            
            //Return a new BookDTO object of the newly created book
            return CreatedAtAction(nameof(GetBookById), new { id = newBook.BookId }, new BookDTO(newBook));
        }

        //UpdateBook(int id, Book updatedBook) 
        //Using the request body, update the book in the database with the values in updatedBook
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, Book updatedBook)
        {
            //Compare if the id in the parameter matches the BookId in updatedBook
            if (id != updatedBook.BookId)
            {
                //Return a Bad Request to the user if it doesn't
                return BadRequest();
            }

            //Using the id, find the book in the database
            var book = await dbContext.Books.FindAsync(id);

            //Compare if a book was found
            if (book == null)
            {
                //Return a Not Found response if the Book wasn't found
                return NotFound();
            }

            //Using the id, find the Library in the database
            var library = await dbContext.Libraries.FindAsync(updatedBook.LibraryId);
            
            //Compare if the library was found
            if (library == null)
            {
                //Return a Not Found response if the Library wasn't found
                return NotFound();
            }

            //Update the values of the book
            book.Title = updatedBook.Title + " (verified at: " + DateTime.Now + ")";
            book.Summary = updatedBook.Summary;
            book.LibraryId = library.LibraryId;

            //Try to save the changes, if there's an error return a Not Found Request
            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!BookExists(id))
            {
                return NotFound();
            }

            //Return a No Content reponse to the user
            return NoContent();
        }

        //GetAllBooks()
        //Get all books from the database and return each book as a List of BookDTO objects
        [Route("~/api/allbooks")]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetAllBooks()
        {
            //Set sort to an empty state
            bool sort = false;

            //Compare if the Header contains a key called Sort/sort
            if (Request.Headers.ContainsKey("Sort"))
            {
                //If there is a header, convert the value to a bool and set it
                sort = Boolean.Parse(Request.Headers["Sort"]);
            }

            if (sort)
            {
                //Get all Books from the database and convert it to a list of BookDTO objects
                var books = await dbContext.Books.Select(b => new BookDTO(b)).ToListAsync();
                //Order the books by their book title and return it
                return books.OrderBy(b => b.Title).ToList();
            }
            else
            {
                //Get all Books from the database and convert it to a list of BookDTO objects
                return await dbContext.Books.Select(b => new BookDTO(b)).ToListAsync();
            }
        }

        //BookExists(int id)
        //Using the id, find compare to see if a book exists in the database
        private bool BookExists(int id)
        {
            return dbContext.Books.Any(b => b.BookId == id);
        }
    }
}
