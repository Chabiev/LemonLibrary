import {ChangeDetectorRef, Component, OnInit} from '@angular/core';
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
  books: Book[];
  book: any;

  constructor(private bookService: BookService , private sanitizer: DomSanitizer , private cdRef: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.getAllBooks();
  }


  getAllBooks(): void {
    this.bookService.getAllBooks().subscribe({
      next: (response: any[]) => {
        console.log('log books: ', response);
        this.books = response;
        this.processImages();
        this.cdRef.detectChanges();
      },
      error: (error) => {
        console.error('Error loading books: ', error);
      },
    });
  }



  private processImages(): void {
    for (const book of this.books) {
      if (book.image && book.image.length > 0) {
        const byteString = atob(book.image); // Convert base64 string to binary
        const bytes = new Uint8Array(byteString.length);
        for (let i = 0; i < byteString.length; i++) {
          bytes[i] = byteString.charCodeAt(i);
        }

        const blob = new Blob([bytes], { type: 'image/jpeg' });
        book.imageUrl = URL.createObjectURL(blob);
      } else {
        // Set a placeholder URL or a default image URL if no image is available
        book.imageUrl = '/assets/Book-Cover-Template.jpg';
      }
    }
  }
  private arrayBufferToBase64(buffer: any): string {
    let binary = '';
    const bytes = new Uint8Array(buffer);
    const len = bytes.byteLength;
    for (let i = 0; i < len; i++) {
      binary += String.fromCharCode(bytes[i]);
    }
    return btoa(binary); // Use btoa() to convert binary to base64
  }


}
