import { Component } from '@angular/core';
import {Book} from "../Models/book";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Author} from "../Models/author";
import {DatePipe} from "@angular/common";


@Component({
  selector: 'app-add-book',
  templateUrl: './add-book.component.html',
  styleUrls: ['./add-book.component.css']
})
export class AddBookComponent {

  constructor(private http:HttpClient, private datePipe: DatePipe) {
  }

  book: any = {
    Title: '',
    Description: '',
    Rating: 0,
    DateAdded: new Date(),
    Available: false,
    AuthorId: '',
    FirstName: '',
    LastName: '',
    BirthDate: new Date(),
    Image: null,
  };




  onImageSelected(event: any) {
    // const file = event.target.files[0];
    // if (file) {
    //   const reader = new FileReader();
    //   reader.onloadend = () => {
    //     this.book.Image = reader.result as string; // Update the property name to Image
    //   };
    //   reader.readAsDataURL(file);
    // }

    const file = event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onloadend = () => {
        if (typeof reader.result === 'string') {
          const base64String = reader.result.split(',')[1];
          this.book.Image = base64String; // Update the property name to Image
        } else {
          console.error('Invalid image format.');
        }
      };
      reader.onerror = (error) => {
        console.error('Error reading image:', error);
      };
      reader.readAsDataURL(file);
    }
  }

  onSubmit() {
    const formData = new FormData();
    formData.append('Title', this.book.Title);
    formData.append('Description', this.book.Description);
    formData.append('Rating', this.book.Rating.toString());
    formData.append('DateAdded', this.datePipe.transform(this.book.DateAdded, 'yyyy/MM/dd'));
    formData.append('Available', this.book.Available.toString());
    formData.append('AuthorId', this.book.AuthorId);
    formData.append('FirstName', this.book.FirstName);
    formData.append('LastName', this.book.LastName);
    formData.append('BirthDate', this.datePipe.transform(this.book.BirthDate, 'yyyy/MM/dd'));

    // Convert the base64 string to a Blob object
    const byteCharacters = atob(this.book.Image);
    const byteArrays = [];
    for (let offset = 0; offset < byteCharacters.length; offset += 512) {
      const slice = byteCharacters.slice(offset, offset + 512);
      const byteNumbers = new Array(slice.length);
      for (let i = 0; i < slice.length; i++) {
        byteNumbers[i] = slice.charCodeAt(i);
      }
      const byteArray = new Uint8Array(byteNumbers);
      byteArrays.push(byteArray);
    }
    const blob = new Blob(byteArrays, { type: 'image/jpeg' });

    // Append the Blob object to the formData with the key 'imageFile'
    formData.append('imageFile', blob, 'image.jpg');

    // Here, you can send the `formData` object to the API endpoint for creating the book.
    // Make sure your API endpoint URL is correct based on your backend setup.
    this.http.post('https://localhost:44330/api/Books/add', formData).subscribe({
      next: (response) => {
        console.log('Book added successfully:', response);
      },
      error: (error) => {
        console.error('Error adding book:', error);
      }
    });
  }


}
