import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  private urls = {
    login: 'https://localhost:7266/api/auth/login'
  };

  constructor(private http: HttpClient) { }

  login(loginInfo:any) : Observable<any>{
    return this.http.post(this.urls.login, loginInfo);
  }

}