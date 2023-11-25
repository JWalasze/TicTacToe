import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { Router } from '@angular/router';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  constructor(private fb: FormBuilder, private router: Router) { }

 

  ngOnInit(): void {
    console.log('Login form submitted:');
    //this.loginForm = this.fb.group({
    //  username: ['', Validators.required],
    //  password: ['', Validators.required],
    //});
  }

  onSubmit() {
    // Handle login logic here
    console.log('Login form submitted:');
    this.router.navigate(["/wait_room"]);
  }
}
