import { Component } from '@angular/core';
import { UserService } from '../services/user/user.service';

@Component({
  selector: 'app-user-info',
  imports: [],
  templateUrl: './user-info.component.html',
  styleUrl: './user-info.component.css'
})
export class UserInfoComponent {
  user:any = {};
  
  constructor(private userService: UserService){}
  ngOnInit(): void {
    const userInfoString = localStorage.getItem("userInfo");

    if(userInfoString != null){
      this.user = JSON.parse(userInfoString);
    }
  }
}
