import {Component, OnInit} from '@angular/core';
import {AuthService} from "./services/auth.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  constructor(private authService: AuthService, private router: Router){}

  ngOnInit(): void {
    if (this.authService.isLoggedIn()) {
      // If the user is logged in, navigate to the BookComponent
      this.router.navigate(['/book']);
    } else {
      // If the user is not logged in, navigate to the LoginComponent
      this.router.navigate(['/login']);
    }
  }

  // Function to check if the user is logged in
  isLoggedIn(): boolean {
    return this.authService.isLoggedIn();
  }

  title = 'client';
}
