import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {BookComponent} from "./book/book.component";
import {AuthorComponent} from "./author/author.component";

const routes: Routes = [
  // { path: 'books', component: BookComponent }, // Route for the BooksComponent
  //
  // { path: '', redirectTo: 'books', pathMatch: 'full' },
  // { path: 'author', component: AuthorComponent },
  { path: 'books', component: BookComponent },
  { path: 'author', component: AuthorComponent },
  { path: '', redirectTo: '/books', pathMatch: 'full' }, // Default route
  { path: '**', redirectTo: '/books', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
