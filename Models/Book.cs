using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment_2_PRO670.Models
{
    public class Book
    {
        //Data Members
        [Key]
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public int LibraryId { get; set; }
        public Library Library { get; set; }

        //Zero Arg Constructor
        public Book()
        {
            BookId = 0;
            Title = "";
            Summary = "";
            LibraryId = 0;
            Library = null;
        }

        //Two Arg Constructor
        public Book(string title, string summary)
        {
            Title = title;
            Summary = summary;
        }
    }

    public class BookDTO
    {
        //Data Members
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }

        //Zero Arg Constructor
        public BookDTO()
        {
            BookId = 0;
            Title = "";
            Summary = "";
        }

        //One Arg Constructor
        public BookDTO(Book book)
        {
            BookId = book.BookId;
            Title = book.Title;
            Summary = book.Summary;
        }
    }
}
