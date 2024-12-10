import { Component, OnInit, input } from '@angular/core';
import { RoleService } from '../services/role/role.service';
import { RegistrationService } from '../services/registration/registration.service';
import { ReactiveFormsModule, FormControl, FormGroup, Validators } from '@angular/forms';
import { RouterModule, RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-registration',
  imports: [ReactiveFormsModule, RouterModule, RouterLink],
  templateUrl: './user-registration.component.html',
  styleUrl: './user-registration.component.css'
})
export class UserRegistrationComponent implements OnInit {
  roles:{ id: string, name: string }[] = [];
  errors:{code:string, description: string}[] = []; 

  registrationForm = new FormGroup({
    firstName: new FormControl('', 
      [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(15)
    ]),
    lastName: new FormControl('',
      [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(15)
      ]
    ),
    username: new FormControl('', 
      [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(15)
      ]
    ),
    email: new FormControl('',
      [
        Validators.required,
        Validators.pattern("^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$")
      ]
    ),
    password: new FormControl('', 
      [
        Validators.required,
        Validators.minLength(8),
        Validators.maxLength (15)
      ]
    ),
    conPassword: new FormControl('', 
      [
        Validators.required,
        Validators.minLength(8),
        Validators.maxLength (15)
      ]
    ),
    selectedRole: new FormControl({}, [Validators.required])
  });

  constructor(private roleService: RoleService, private registrationService: RegistrationService, private router: Router) {}

  ngOnInit(): void {
    this.loadRoles();
  }

  loadRoles(): void {
    this.roleService.getRoles().subscribe(
      (data) => {
        this.roles = data;
      },
      (error) => {
        console.error('Error fetching roles:', error);
      }
    );
  }

  onSubmit(): void {
    const userData = {
      firstName: this.registrationForm.value.firstName,
      lastName: this.registrationForm.value.lastName,
      username: this.registrationForm.value.username,
      email: this.registrationForm.value.email,
      password: this.registrationForm.value.password,
      roleId: this.registrationForm.value.selectedRole
    };
    
    
    if(!this.registrationForm.invalid){
      this.registrationService.registerUser(userData).subscribe(
        (data) => {
          if(data.status){
            alert(data.message);
            this.router.navigateByUrl("/login");
          }else{
            this.errors = data.errors;
            console.log(this.errors);
            console.log(data.errors);
          }
        },
        (error) => {
          console.log(error);
        }
      );
    }
  }
}
