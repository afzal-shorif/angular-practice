import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RegistrationService {

  private urls = {
    registration: 'https://localhost:7266/api/user/register'
  };

  constructor(private http: HttpClient ) { }

  registerUser(userInfo:any): Observable<any>{
    return this.http.post(this.urls.registration, userInfo);
  }
}
