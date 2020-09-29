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
    public class LibrariesController : ControllerBase
    {
        private readonly LibraryDbContext dbContext;

        //Constructor, set the dbContext to the database
        public LibrariesController(LibraryDbContext libraryDbContext)
        {
            dbContext = libraryDbContext;
        }

        //GetLibraries()
        //Return a list of Libraries to the user, each library being a LibraryDTO Object
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LibraryDTO>>> GetLibraries()
        {
            return await dbContext.Libraries.Select(l => new LibraryDTO(l)).ToListAsync();
        }

        //GetLibraryById(int id) 
        //Get the library from the database and return a LibraryDTO object 
        [HttpGet("{id}")]
        public async Task<ActionResult<LibraryDTO>> GetLibraryById(int id)
        {
            //Using the id find the Library 
            var library = await dbContext.Libraries.FindAsync(id);

            //Compare if the library was returned
            if (library == null)
            {
                //Return a Not Found response if the book wasn't found
                return NotFound();
            }

            //Return the library as a LibraryDTO object
            return new LibraryDTO(library);
        }

        //CreateLibrary(LibraryDTO libraryDTO) 
        //Using the request body, create a new Library entry and return the library to the user
        [HttpPost]
        public async Task<ActionResult<Library>> CreateLibrary(LibraryDTO libraryDTO)
        {
            //Create a new Library Object using the LibraryDTO
            Library library = new Library(libraryDTO.Address);
            //Update the Address
            library.Address += " (verified at: " + DateTime.Now + ")";

            //Save the new Library into the database
            dbContext.Libraries.Add(library);
            await dbContext.SaveChangesAsync();

            //Return a new LibraryDTO object of the newly created library
            return CreatedAtAction(nameof(GetLibraryById), new { id = library.LibraryId }, new LibraryDTO(library));
        }

        //UpdateLibrary(int id, LibraryDTO libraryDTO)
        //Using the request body, update the library in the database with the values in libraryDTO
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLibrary(int id, LibraryDTO libraryDTO)
        {
            //Compare if the id in the parameter matches the LibraryId in libraryDTO
            if (id != libraryDTO.LibraryId)
            {
                //Return a Bad Request to the user if it doesn't
                return BadRequest();
            }

            //Using the id, find the Library in the database
            var library = await dbContext.Libraries.FindAsync(id);

            //Compare if the library was found
            if (library == null)
            {
                //Return a Not Found response if the Library wasn't found
                return NotFound();
            }

            //Update the values of the Library
            library.Address = libraryDTO.Address + " (verified at: " + DateTime.Now + ")";

            //Try to save the changes, if there's an error return a Not Found Request
            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException) when (!LibraryExists(id))
            {
                return NotFound();
            }

            //Return a No Content reponse to the user
            return NoContent();
        }

        //GetAllBooksFromLibrary(int id)
        //Using the id the Library, return a List of BookDTO objects that belong to the requested Library
        [Route("{id}/books")]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetAllBooksFromLibrary(int id)
        {
            //Using the id, find the Library in the database
            Library library = await dbContext.Libraries.FindAsync(id);

            //Compare if the library was found
            if (library == null)
            {
                //Return a Not Found response if the Library wasn't found
                return NotFound();
            }

            //Get all books from the database
            var books = await dbContext.Books.Select(b => b).ToListAsync();

            //Compare if there are any books in the database
            if (books.Count == 0)
            {
                //Return a No Content request if there isn't
                return NoContent();
            }

            //Get all Books where the LibraryId of each book equals the id in the parameter
            var booksFromLibrary = books.Select(b => b).Where(b => b.LibraryId == id).ToList();

            //Compare if there are any books
            if (booksFromLibrary.Count == 0)
            {
                //Return a No Content request if there isn't
                return NoContent();
            }

            //Convert all books in the List to a list of BookDTO objects
            var bookDTOList = booksFromLibrary.Select(b => new BookDTO(b)).ToList();

            //Return all books found
            return bookDTOList;
        }

        //LibraryExists(int id)
        //Using the id, find compare to see if a library exists in the database
        private bool LibraryExists(int id)
        {
            return dbContext.Libraries.Any(l => l.LibraryId == id);
        }
    }
}
