import { Component } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  constructor(private http: HttpClient, private router: Router) {
  }

// Variables to store user input
  username: string;
  password: string;

  // Function to handle login
  login() {
    const loginData = { username: this.username, password: this.password };
    this.http.post('https://localhost:5002/api/User/login', loginData, { responseType: 'text' }).subscribe({
      next: (response) => {
        console.log(response);

        // No need to parse response as JSON, since it's already the JWT token
        const jwtToken = response;
        localStorage.setItem('token', jwtToken);

        this.router.navigate(['/book']);
      },
      error: (error) => {
        console.log('Login failed: ', error);
      }
    });
  }

  goToRegister(){
    this.router.navigate(['/register'])
  }

}
