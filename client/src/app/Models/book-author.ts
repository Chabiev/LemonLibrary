import {Book} from "./book";
import {Author} from "./author";

export interface BookAuthor {
  bookId: number;
  book: Book;
  authorId: number;
  author: Author;
}
