using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment_2_PRO670.Models
{
    public class Library
    {
        //Data Members
        [Key]
        public int LibraryId { get; set; }
        public string Address { get; set; }
        public List<Book> Books { get; set; }

        //Zero Arg Constructor
        public Library()
        {
            LibraryId = 0;
            Address = "";
            Books = new List<Book>();
        }

        //One Arg Constructor
        public Library(string address)
        {
            Address = address;
            Books = new List<Book>();
        }
    }

    public class LibraryDTO
    { 
        //Data Members
        public int LibraryId { get; set; }
        public string Address { get; set; }

        //Zero Arg Constructor
        public LibraryDTO()
        {
            LibraryId = 0;
            Address = "";
        }

        //One Arg Constructor
        public LibraryDTO(Library library)
        {
            LibraryId = library.LibraryId;
            Address = library.Address;
        }
    }
}
