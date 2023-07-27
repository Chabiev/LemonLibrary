import { Injectable } from '@angular/core';
import {Router} from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private router: Router) {
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token'); // Convert the token to a boolean value
  }

  // Function to log out the user
  logout(): void {
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }

}
