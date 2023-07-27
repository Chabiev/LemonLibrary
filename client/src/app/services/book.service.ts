import {Injectable, OnInit} from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Observable, Subject} from "rxjs";
import {Book} from "../Models/book";
import {DomSanitizer, SafeUrl} from "@angular/platform-browser";
import {Router} from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class BookService implements OnInit{
  private apiUrl = 'http://localhost:5000/api/Books/';

  editId:number

  private editIdSubject: Subject<number> = new Subject<number>();
  editId$: Observable<number> = this.editIdSubject.asObservable();

  constructor(private http: HttpClient, private router: Router) { }



  ngOnInit(): void {
  }




  // Get all books
  getAllBooks(): Observable<Book[]> {
    return this.http.get<Book[]>(this.apiUrl);
  }

  getBookById(Id): Observable<Book>{
    return this.http.get<Book>(this.apiUrl + Id);
  }

  takeBookById(Id){
    this.editId = Id;
    this.editIdSubject.next(Id);
  }

  getEditId(): number {
    return this.editId;
  }
  updateBook(book: Book): Observable<any> {
    const formData = new FormData();
    formData.append('Title', book.title);
    formData.append('Description', book.description);
    formData.append('Rating', book.rating.toString());
    formData.append('Available', book.available.toString());

    // Get the token from local storage
    const token = localStorage.getItem('token');

    // Add the token to the request headers
    const headers = new HttpHeaders({
      'Content-Type': 'multipart/form-data',
      Authorization: `Bearer ${token}`,
    });
    console.log(headers);

    return this.http.put(this.apiUrl + 'edit', formData, { headers });
  }





  // editBook(book: Book): Observable<any> {
  //   book.id= this.Id
  //   const url = `${this.apiUrl}/books/edit`;
  //   return this.http.put(url, book);
  // }


}
