import {ChangeDetectorRef, Component, OnInit} from '@angular/core';
import {Book} from "../Models/book";
import {ActivatedRoute, Router} from "@angular/router";
import {BookService} from "../services/book.service";
import {BookDataService} from "../services/book-data.service";

@Component({
  selector: 'app-book-details',
  templateUrl: './book-details.component.html',
  styleUrls: ['./book-details.component.css']
})
export class BookDetailsComponent implements OnInit{

  book: Book;


  constructor(private route: ActivatedRoute,
              private bookService: BookService,
              private cdRef: ChangeDetectorRef,
              private bookDataService: BookDataService,
              private router: Router) { }


  ngOnInit(): void {
    // Get the book ID from the route parameters
    const id = this.route.snapshot.paramMap.get('id');

    // Fetch the book details using the BookService
    this.bookService.getBookById(id).subscribe({
      next: (book: Book) => {
        console.log(book);
        this.book = book;

        this.processImages();
        this.cdRef.detectChanges();
        // Process the book image here if needed

        this.bookDataService.setSelectedBook(book);
      },
      error: (error) => {
        console.error('Error loading book details: ', error);
      },
    });
    this.bookService.takeBookById(id);
  }

  goToEdit() {
    // Set the selected book ID in the BookService
    this.bookService.takeBookById(this.book.id);
    this.router.navigate(['book', 'edit']);
  }

  private processImages(): void {

      if (this.book.image && this.book.image.length > 0) {
        const byteString = atob(this.book.image); // Convert base64 string to binary
        const bytes = new Uint8Array(byteString.length);
        for (let i = 0; i < byteString.length; i++) {
          bytes[i] = byteString.charCodeAt(i);
        }

        const blob = new Blob([bytes], { type: 'image/jpeg' });
        this.book.imageUrl = URL.createObjectURL(blob);
      } else {
        // Set a placeholder URL or a default image URL if no image is available
        this.book.imageUrl = '/assets/Book-Cover-Template.jpg';
      }
  }

}
