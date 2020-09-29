# Library-System - REST API

## Purpose 
The purpose of this assignment is to take the code we originally created for a minimalistic Library System and convert it into a REST Web API using .NET Core Framework. A REST API is a system where we can retrieve, update, delete and create objects using a GET, POST, PUT, and DELETE requests over a network. These requests will communicate with the database on that network to perform CRUD operators to return, update, delete and or create new data from the database. In this assignment, certain routes will be programmed to perform CRUD operations on the Libraries table and on the Books table from our database. For accurate routing and organization, two different controllers will be made, LibrariesController and BooksController.

## LibrariesController
The LibrariesController is designed to return certain information from the Libraries Table in the database. To ensure that we are returning minimal information, a new class called, LibraryDTO was created to return only the id of the Library and the address.

### Routes
* GET /api/libraries
  * Return a list of all LibraryDTO objects from the Libraries table in the database. This list will consist of all libraries with their respected id and address.
  * Request: https://localhost:8080.com/api/libraries 
  
* GET /api/libraries/{id}
  * Using the id parameter return a LibraryDTO object from the Libraries table in the database. If no object was found, a Not Found status will be returned.
  * Request: https://localhost:8080.com/api/libraries/2

  
* POST /api/libraries
  * This POST request will allow the user to add a new library entry into the Libraries table in the database. Book information does not have to be entered/will not     be saved. Once the request is made, a successful creation will return a LibraryDTO object to the user.
  * Request: https://localhost:8080.com/api/libraries
  * Request Body:
  ```cs
    { "address": "55 Ste Marie St, Collingwood, ON L9Y 0W6" } 
  ```
  
* PUT /api/libraries/{id}
  * This PUT request will allow the user to update the address of any library in the Libraries table. A successful update will return no information in the response message. If the id of the parameter doesn’t match the id of the request data, then a Bad Request status will be returned. If a library object isn’t found, a Not Found request will be returned. 
  * Request: https://localhost:8080.com/api/libraries/4
  * Request Body:
  ```cs
	  { "libraryId": 5, "address": "60 Worsley St, Barrie, ON L4M 1L6" }
  ```
  
* GET /api/libraries/{id}/books
  * Using the id parameter, return a List of BookDTO objects from the Books table in the database. Each book returned, will be books that are associated to that specific library. If the library object wasn’t found, a Not Found request will be returned.
  * Request: https://localhost:8080.com/api/libraries/2/books 


### BooksController
The BooksController is designed to return certain information from the Books table in the database. The following information being returned; Book’s id, title and summary, will be returned as a BookDTO object to the user. To ensure only certain information will be sent back, the BookDTO class was created. 

## Routes
* GET /api/books
  * Return a list of all books from the Books table in the database as a BookDTO object for each book. This list will consist of the book’s id, title, and summary for each book. No information about the Library will be returned. 
  * Request: https://localhost:8080.com/api/books 
  
* GET /api/books/{id}
  * Using the id parameter, return a certain BookDTO object from the Books table in the database. If a book object wasn’t found, return a Not Found response to the user.
  * Request: https://localhost:8080.com/api/books/2
  
* POST /api/books
  * This POST request will allow the user to create a Book entry into the Books table in the database. When creating a book, the id of the associated library must be a valid id. If the library object wasn’t found, a Not Found request will be returned. If a successful creation was made, a BookDTO object of the created book will be returned to the user. 
  *	Request: https://localhost:8080.com/api/books
  * Request Body: 
  ```cs
  { 
    "title": "The Ballad Of Songbirds And Snakes", "summary": "It is the morning of the reaping that will kick off the tenth annual Hunger Games. In the Capitol, eighteen year old Coriolanus Snow is preparing for his one shot at glory as a mentor in the Games.", 
    "libraryId": 2
  }
  ```
  
* PUT /api/books/{id}
  * This PUT request will allow the user to edit the title, summary, and the associated library id of the book. A successful update will return no information in the response message. If the id in the parameter and the id in the body doesn’t match, a Bad Request response will be sent to the user. If either a book object or a library object wasn’t found, a Not Found response will be returned.
  * Request: https://localhost:8080.com/api/books
  * Request Body:
  ```cs	
  {
    "bookId": 1,
    "title": "The C Programming Language",
    "summary": "This book was central to the development and popularization of the C programming language and is still widely read and used today.",
    "libraryId": 2
  }
  ```
  
* GET /api/allbooks
  * This GET request will return all books that are currently in the Library System (Books Table). If a HTTP request header called, Sort/sort was set to true or false in the header, the request will return all books as a list which is sorted by the title of each book.
  * Request: https://localhost:8080/api/allbooks
  * Request Header: (Sort: true) or (Sort: false) / (sort: true) or (sort: false)
