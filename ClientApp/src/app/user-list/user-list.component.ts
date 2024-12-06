import { Component, CSP_NONCE } from '@angular/core';
import { UserService } from '../services/user/user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-list',
  imports: [],
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.css'
})
export class UserListComponent {
  users:{id:string, firstName: string, lastName: string, email: string, userName: string, isActive: boolean }[] = [];

  constructor(private userService : UserService, private router: Router){}
  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.userService.getUsers().subscribe(
      (data) => {
        if(data.status){
          this.users = data.data;
        }else{
          alert(data.message);
        }
      },
      (error) => {
        console.error('Error fetching roles:', error);
      }
    );
  }

  updateUserStatus(userId: string, status: string): void{
    if(confirm("Do you want to update the user status?")){
      const statusInfo:any = {
          userId: userId,
          status: status
      }
      
      this.userService.updateUserStatus(statusInfo).subscribe(
        (data) => {
          if(data.status){
            let indexToUpdate = this.users.findIndex(item => item.id === data.data.id);
            this.users[indexToUpdate] = data.data;
          }else{
            alert(data.message);
          }
        },
        (error) => {
          console.log(error);
        }
      );
    }
  }
}
