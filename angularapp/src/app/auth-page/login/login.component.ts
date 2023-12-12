import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;

  username = 'Rachet1234';

  playerId = 1;

  constructor(private fb: FormBuilder, private router: Router) { }

  ngOnInit(): void {
    console.log('Login form submitted:');
    //this.loginForm = this.fb.group({
    //  username: ['', Validators.required],
    //  password: ['', Validators.required],
    //});
  }

  onSubmit() {
    console.log("here I am")
    this.router.navigate(["/wait_room"], { state: { playerId: this.playerId, username: this.username } });
  }

  createProfile() {
    console.log("whatthefuck");
    this.router.navigate(["/create_profile"]);
  }
}
