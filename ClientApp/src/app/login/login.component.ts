import { Component } from '@angular/core';
import { LoginService } from '../services/login/login.service';
import { ReactiveFormsModule, FormControl, FormGroup, Validators } from '@angular/forms';
import { RouterModule, RouterOutlet, RouterLink, RouterLinkActive, Router } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, RouterModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  
  title = "Login";
  error = "";
  loginForm = new FormGroup({
    username: new FormControl('', [Validators.required, Validators.minLength(3), Validators.maxLength(15)]),
    password: new FormControl('', [Validators.required, Validators.minLength(3), Validators.maxLength(15)]),
  });

  constructor(private loginService: LoginService, private router: Router){}

  onSubmit(): void {
    const userData = {
      username: this.loginForm.value.username,
      password: this.loginForm.value.password,
    };

    if(!this.loginForm.invalid){
      this.loginService.login(userData).subscribe(
        (data) => {
          if(data.status){
            localStorage.setItem('token', data.data.token);
            this.router.navigateByUrl("/dashboard/info");
          }else{
            alert(data.message);
          }
        },
        (error) => {
          this.error = "Unable to login. Please try later";
          console.log(error);
        }
      );
    }else{

    }
  }
}