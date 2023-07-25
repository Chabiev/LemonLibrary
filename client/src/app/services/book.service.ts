import {Injectable, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {Book} from "../Models/book";
import {DomSanitizer, SafeUrl} from "@angular/platform-browser";

@Injectable({
  providedIn: 'root'
})
export class BookService implements OnInit{
  private apiUrl = 'http://localhost:5000/api/Books';

  let

  constructor(private http: HttpClient) { }



  ngOnInit(): void {
  }



  // Get all books
  getAllBooks(): Observable<Book[]> {
    return this.http.get<Book[]>(this.apiUrl);
  }


}
