import { Component } from '@angular/core';
import { RouterModule, RouterLink, Router } from '@angular/router';
import { UserService } from '../services/user/user.service';

@Component({
	selector: 'app-dashboard',
	imports: [RouterModule, RouterLink],
	templateUrl: './dashboard.component.html',
	styleUrl: './dashboard.component.css'
})

export class DashboardComponent {
	user: any = {};
	isLoading: boolean = true;
	userRoles: string[] = [];
	profilePicture: any = "";
	constructor(private userService: UserService, private router: Router) { }

	ngOnInit(): void {
		this.getUserInfo();

		this.userService.getProfilePicture().subscribe(
			(data: any) => {
				this.profilePicture = data;
			}
		);
	}

	getUserInfo(): void {
		this.userService.getUserInfo().subscribe(
			(data: any): void => {
				console.log(data);
				if (data.status) {
					// set the user role somewhere globally
					this.user = data.data.user;
					this.userRoles = data.data.role;
					localStorage.setItem('userInfo', JSON.stringify(this.user));
					localStorage.setItem('roles', JSON.stringify(this.userRoles));
					this.isLoading = false;
				} else {
					alert(data.message);
				}
			},
			(error) => {
				console.log(error);
			}
		);
	};



	logout(): void {
		this.userService.logout().subscribe(
			(data) => {
				if (data.status) {
					localStorage.clear();
					this.router.navigateByUrl("/login");
				} else {
					alert(data.message);
				}
			},
			(error) => {

			}
		);
	}
}
