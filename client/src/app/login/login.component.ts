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

  username: string;
  password: string;

  login() {
    const loginData = { username: this.username, password: this.password };
    this.http.post('https://localhost:44330/api/User/login', loginData, { responseType: 'text' }).subscribe({
      next: (response) => {
        console.log(response);

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
