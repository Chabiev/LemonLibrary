import {Component, OnInit} from '@angular/core';
import {AuthService} from "../services/auth.service";
import {BookService} from "../services/book.service";
import {AuthorService} from "../services/author.service";

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css']
})
export class NavBarComponent implements OnInit{

  constructor(private auth: AuthService, private bookService: BookService, private authorService: AuthorService) {
  }

  originalBooks: any[];
  originalAuthors: any[];
  books: any[];
  authors: any[];
  searchTerm: string = '';
  showDropdown: boolean = false;


  ngOnInit(): void {
    this.populateBooks();
    this.populateAuthors();
  }



  populateBooks() {
    this.bookService.getAllBooks().subscribe({
      next: (response: any[]) => {
        console.log('log books: ', response);
        this.originalBooks = response;
        this.books = [...this.originalBooks]; // Clone the original list
      },
      error: (error) => {
        console.error('Error loading books: ', error);
      },
    });
  }

  populateAuthors() {
    this.authorService.getAllAuthors().subscribe({
      next: (response) => {
        this.originalAuthors = response;
        this.authors = [...this.originalAuthors]; // Clone the original list
        console.log(response);
      },
      error: (error) => {
        console.log('Error loading authors', error);
      },
    });
  }

  onSearch() {
    // Filter books based on the search term
    // In this example, we are filtering based on the book title
    this.books = this.originalBooks.filter(
      (book) =>
        book.title.toLowerCase().includes(this.searchTerm.toLowerCase())
    );

    // Filter authors based on the search term
    // In this example, we are filtering based on the author's full name
    this.authors = this.originalAuthors.filter(
      (author) =>
        author.firstName.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        author.lastName.toLowerCase().includes(this.searchTerm.toLowerCase())
    );

    // Show or hide the dropdown based on search results
    this.showDropdown = this.books.length > 0 || this.authors.length > 0;
  }

  hideDropdown() {
    this.showDropdown = false;
  }


  logout(){
    this.auth.logout();

  }
}
