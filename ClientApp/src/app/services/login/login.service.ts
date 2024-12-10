import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  private urls = {
    login: 'https://localhost:7266/api/auth/login',
    refresh: 'https://localhost:7266/api/auth/refresh'
  };

  constructor(private http: HttpClient, private router: Router) { }

  login(loginInfo:any) : Observable<any>{
    return this.http.post(this.urls.login, loginInfo);
  }

  refresh(tokenInfo:any): Observable<any>{

    return this.http.post<any>(this.urls.refresh, tokenInfo).pipe(
      tap((response) => {
        if(response.status){
          localStorage.setItem('accessToken', response.data.accessToken);
          localStorage.setItem('refreshToken', response.data.refreshToken);
        }else{
          localStorage.clear();
          this.router.navigateByUrl("/login");
        }
      }),
      catchError((error) => {
        console.log('Error refreshing access token:', error);
        this.router.navigateByUrl("/login");
        return throwError(error);
      })
    );

    //return this.http.post(this.urls.refresh, tokenInfo);
  }
}