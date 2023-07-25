import {BookAuthor} from "./book-author";

export interface Book {

  id: number;
  title: string;
  description: string;
  image: any; // Assuming image is of type 'any' (byte array)
  rating: number;
  dateAdded: Date;
  available: boolean;
  authors: any[];
  imageUrl?: string; // New property for storing the safe URL
  imageBase64?: string;
}
