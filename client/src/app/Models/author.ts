import {BookAuthor} from "./book-author";

export interface Author {
  id: number;
  firstName: string;
  lastName: string;
  birthDate: Date;
  bookAuthors?: BookAuthor[]; // Make sure to define the BookAuthor model or interface as well
}
