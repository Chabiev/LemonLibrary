import {Component, OnInit} from '@angular/core';
import {Book} from "../Models/book";
import {BookService} from "../services/book.service";
import {DomSanitizer, SafeUrl} from "@angular/platform-browser";
import {getSafeImageUrl} from "../utils/image-utils";

@Component({
  selector: 'app-book',
  templateUrl: './book.component.html',
  styleUrls: ['./book.component.css']
})
export class BookComponent implements OnInit{
  books: any[];
  book: any;

  constructor(private bookService: BookService , private sanitizer: DomSanitizer) { }

  ngOnInit(): void {
    this.getAllBooks();
  }


  getAllBooks(): void {
    this.bookService.getAllBooks().subscribe(
      {
        next: (response: any[]) =>{
          console.log('log books: ', response);
          this.books = response;
          this.processImageUrls();
          console.log('log books: ', response);
          this.books = response;

        },
        error: (error) => {
          console.error('Error loading books: ', error)
        }});
  }


  private processImageUrls(): void {
    for (const book of this.books) {
      book.image = getSafeImageUrl(this.sanitizer, book.image);
    }
  }
  // getImageUrl(imageBytes: Uint8Array | null): SafeUrl | null {
  //   if (imageBytes && imageBytes.length > 0) {
  //     const base64String = btoa(String.fromCharCode(...imageBytes));
  //     const dataUrl = `data:image/jpeg;base64,${base64String}`;
  //     return this.sanitizer.bypassSecurityTrustUrl(dataUrl);
  //   }
  //   return null;
  // }

  // getSafeImageUrl(imageBytes: Uint8Array | null): SafeUrl | null {
  //   return getSafeImageUrl(this.sanitizer, imageBytes); // Pass the DomSanitizer instance
  // }

  // getImageUrl(imageBytes: Uint8Array | null): SafeUrl | null {
  //   if (imageBytes && imageBytes.length > 0) {
  //     const base64String = btoa(String.fromCharCode(...imageBytes));
  //     const dataUrl = `data:image/jpeg;base64,${base64String}`;
  //     return this.sanitizer.bypassSecurityTrustUrl(dataUrl);
  //   }
  //   return null;
  // }
}
