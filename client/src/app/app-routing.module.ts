import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {BookComponent} from "./book/book.component";
import {AuthorComponent} from "./author/author.component";
import {LoginComponent} from "./login/login.component";
import {RegisterComponent} from "./register/register.component";
import {BookDetailsComponent} from "./book-details/book-details.component";
import {EditBookComponent} from "./edit-book/edit-book.component";

const routes: Routes = [

  { path: 'books', component: BookComponent },
  { path: 'books/:id', component: BookDetailsComponent },
  { path: 'author', component: AuthorComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'book/edit', component: EditBookComponent },
  { path: '', redirectTo: '/books', pathMatch: 'full' }, // Default route
  { path: '**', redirectTo: '/books', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
