import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable, tap, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  private urls = {
    login: 'https://localhost:7266/api/auth/login',
    refresh: 'https://localhost:7266/api/auth/refresh'
  };

  constructor(private http: HttpClient) { }

  login(loginInfo:any) : Observable<any>{
    return this.http.post(this.urls.login, loginInfo);
  }

  refresh(tokenInfo:any): Observable<any>{
    const refreshToken = localStorage.getItem('refreshToken');
    return this.http.post<any>(this.urls.refresh, tokenInfo).pipe(
      tap((response) => {
        localStorage.setItem('accessToken', response.data.accessToken);
        localStorage.setItem('refreshToken', response.data.refreshToken);
      }),
      catchError((error) => {
        console.error('Error refreshing access token:', error);
        return throwError(error);
      })
    );

    //return this.http.post(this.urls.refresh, tokenInfo);
  }
}