import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';

@Injectable({
	providedIn: 'root'
})
export class UserService {
	urls = {
		userList: "https://localhost:7266/api/user/list",
		userInfo: "https://localhost:7266/api/user/get",
		updateStatus: "https://localhost:7266/api/user/update/status",
		logout: "https://localhost:7266/api/auth/logout"
	}

	profilePictureBehavior = new BehaviorSubject("");

	constructor(private http: HttpClient) { }
	getUsers(): Observable<any> {
		return this.http.get<any>(this.urls.userList);
	}

	getProfilePicture(): any {
		return this.profilePictureBehavior;
	}

	getUserInfo(): Observable<any> {
		return this.http.get<any>(this.urls.userInfo);
	}

	updateUserStatus(statusInfo: any): Observable<any> {
		return this.http.post(this.urls.updateStatus, statusInfo);
	}

	setProfilePicture(value: any): void {
		this.profilePictureBehavior.next(value);
	}

	logout(): Observable<any> {
		return this.http.post(this.urls.logout, {});
	}
}
