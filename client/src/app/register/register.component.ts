import { Component } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  constructor(private http: HttpClient, private router: Router) {
  }

// Variables to store user input
  username: string;
  password: string;

  // Function to handle login

  register() {
    const regData = { username: this.username, password: this.password };
    this.http.post('https://localhost:44330/api/User/register', regData, { responseType: 'text' }).subscribe({
      next: (response) => {
        console.log(response);

        this.router.navigate(['/login']);

      },
      error: (error) => {
        console.log('Login failed: ', error);
      }
    });
  }
}
