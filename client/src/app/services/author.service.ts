import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {Author} from "../Models/author";

@Injectable({
  providedIn: 'root'
})
export class AuthorService {
  private apiUrl = 'http://localhost:5000/api/authors';

  constructor(private http: HttpClient) {}

  // Get all authors
  getAllAuthors(): Observable<Author[]> {
    return this.http.get<Author[]>(this.apiUrl);
  }

  // Get a single author by ID
  getAuthorById(id: number): Observable<Author> {
    return this.http.get<Author>(`${this.apiUrl}/${id}`);
  }

  // Add a new author
  addAuthor(author: Author): Observable<Author> {
    return this.http.post<Author>(this.apiUrl, author);
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
