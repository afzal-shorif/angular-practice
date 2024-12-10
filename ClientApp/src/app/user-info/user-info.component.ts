import { Component, ElementRef, ViewChild } from '@angular/core';
import { UserService } from '../services/user/user.service';
import { ReactiveFormsModule, FormControl, FormGroup, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
	selector: 'app-user-info',
	imports: [ReactiveFormsModule, CommonModule],
	templateUrl: './user-info.component.html',
	styleUrl: './user-info.component.css'
})
export class UserInfoComponent {
	user: any = {};
	isPreview = true;
	profilePicture: string = "";
	validationErrors: any = null;

	updateProfileForm = new FormGroup({
		firstName: new FormControl('', [Validators.required]),
		lastName: new FormControl('', [Validators.required]),
		userName: new FormControl('', [Validators.required]),
		email: new FormControl('',[Validators.required]),
		profilePictureControl: new FormControl('', [Validators.required])
	});

	constructor(private userService: UserService) { }
	
	ngOnInit(): void {
		const userInfoString = localStorage.getItem("userInfo");
		this.validationErrors = null;

		if (userInfoString != null) {
			this.user = JSON.parse(userInfoString);
			this.updateProfileForm.controls.firstName.setValue(this.user.firstName);
			this.updateProfileForm.controls.lastName.setValue(this.user.lastName);
			this.updateProfileForm.controls.userName.setValue(this.user.userName);
			this.updateProfileForm.controls.email.setValue(this.user.email);
		}

		this.userService.userInfo.subscribe((response)=>{
			this.user = response;
		});

		this.userService.getProfilePicture().subscribe(
			(data: any) => {
				this.profilePicture = data;
			}
		)
	}

	togglePreview(): void{
		this.isPreview = !this.isPreview;
	}

	changePicture(event: any): void {
		const file = event.target.files[0];
		const reader = new FileReader();
		reader.readAsDataURL(file);
		reader.onload = () => {
			if (reader.result != null) {
				//this.userService.setProfilePicture(reader.result);
				this.profilePicture = reader.result.toString();
			}
		}
	}

	updateProfile(): void{
		if(!this.updateProfileForm.invalid){
			const userInfo = {
				firstName: this.updateProfileForm.controls.firstName.value,
				lastName: this.updateProfileForm.controls.lastName.value,
				userName: this.updateProfileForm.controls.userName.value,
				email: this.updateProfileForm.controls.email.value,
				photo: this.profilePicture
			};

			this.userService.updateUserInfo(userInfo).subscribe(
				(response: any) => {
					
					if(response.status){
						this.userService.setProfilePicture(response.data.user.photo.base64String);
						this.userService.setUserInfo(response.data.user);
						//alert("Profile Picture Uploa Successfully");
						this.isPreview = true;
					}else{
						console.log(response);
					}
				},
				(error) => {
					this.validationErrors = error.error.errors ;
					console.log(this.validationErrors);			
				});
		}
	}
}
