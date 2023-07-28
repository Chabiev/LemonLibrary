import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Observable} from "rxjs";
import {Author} from "../Models/author";
import {AuthorBooks} from "../Models/author-books";
import {Book} from "../Models/book";

@Injectable({
  providedIn: 'root'
})
export class AuthorService {
  private apiUrl = 'https://localhost:44330/api/Authors';

  constructor(private http: HttpClient) {}

  private selectedBook: Book;
  authorId:number;




  // Get all authors
  getAllAuthors(): Observable<Author[]> {
    return this.http.get<Author[]>(this.apiUrl);
  }

  // Get a single author by ID
  getAuthorById(id: number): Observable<AuthorBooks> {
    return this.http.get<AuthorBooks>(`${this.apiUrl}/${id}`);
  }

  // Add a new author
  addAuthor(author: any): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.post<any>('https://localhost:44330/api/authors/add/',author,{headers});
  }

  updateAuthor(updatedAuthor){
    return this.http.put('https://localhost:44330/api/authors/edit/',updatedAuthor);
  }

  // Edit an existing author
  editAuthor(author: Author): Observable<Author> {
    return this.http.put<Author>(`${this.apiUrl}/${author.id}`, author);
  }

  // Delete an author by ID
  deleteAuthor(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`);
  }
}
