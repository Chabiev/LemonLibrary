import { Component, OnInit } from '@angular/core';
import { Book } from '../Models/book';
import { BookService } from '../services/book.service';
import {ActivatedRoute, Router} from "@angular/router";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {BookDataService} from "../services/book-data.service";

@Component({
  selector: 'app-edit-book',
  templateUrl: './edit-book.component.html',
  styleUrls: ['./edit-book.component.css']
})
export class EditBookComponent implements OnInit {

  editForm: FormGroup;
  book: Book;
  bookId: number;



  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private formBuilder: FormBuilder,
    private bookService: BookService,
    private bookDataService: BookDataService
  ) {}

  ngOnInit(): void {
    // // Retrieve the selected book ID from the BookService
    // this.bookId = this.bookService.getEditId();
    //
    // // Fetch the book details using the BookService
    // this.bookService.getBookById(this.bookId).subscribe({
    //   next: (book: Book) => {
    //     this.book = book; // Make sure this is set correctly
    //     this.initForm();
    //   },
    //   error: (error) => {
    //     console.error('Error loading book details: ', error);
    //   }
    // });

    this.book = this.bookDataService.getSelectedBook(); // Get the selected book from the data service

    if (!this.book) {
      // If the selected book is not found, navigate back to the details page
      this.router.navigate(['book', this.bookService.getEditId()]);
    } else {
      this.initForm();
    }
  }

  initForm() {
    // Initialize the form with book details
    this.editForm = this.formBuilder.group({
      title: [this.book.title, Validators.required],
      description: [this.book.description, Validators.required],
      rating: [this.book.rating, [Validators.required, Validators.min(1), Validators.max(5)]],
      available: [this.book.available]
      // Add other form controls for other book properties if needed
    });
  }

  onSubmit() {
    // Update the book object with form values
    this.book.title = this.editForm.value.title;
    this.book.description = this.editForm.value.description;
    this.book.rating = this.editForm.value.rating;
    this.book.available = this.editForm.value.available;
    console.log('Book object before PUT request:', this.book);

    // Make the PUT request to update the book
    this.bookService.updateBook(this.book).subscribe({
      next: (response: any) => {
        // Redirect back to the book details page after successful update
        console.log(response);
        this.router.navigate(['book', this.bookId]);
      },
      error: (error) => {
        console.error('Error updating book: ', error);
      }
    });
  }

}
