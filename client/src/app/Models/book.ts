import {BookAuthor} from "./book-author";

export interface Book {
  id: number;
  title: string;
  description: string;
  image?: string | null; // Use 'Uint8Array | null' for nullable byte array
  rating: number;
  dateAdded: Date;
  available: boolean;
  bookAuthors?: BookAuthor[] | number[]; // Make sure to define the BookAuthor model or interface as well
}
