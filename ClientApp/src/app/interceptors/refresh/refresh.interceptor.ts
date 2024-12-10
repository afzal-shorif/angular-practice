import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpInterceptorFn, HttpRequest, HttpResponse, HttpStatusCode } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { LoginService } from '../../services/login/login.service';
import { Router } from '@angular/router';
import { catchError, Observable, switchMap, tap, throwError } from 'rxjs';

@Injectable()
export class NewAuthInterceptor implements HttpInterceptor {
  
  constructor(private loginService: LoginService, private router: Router){}
  
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const accessToken = localStorage.getItem('accessToken');
    const refreshToken = localStorage.getItem('refreshToken');

    if (accessToken) {
      req = this.addToken(req, accessToken);
    }
    
    return next.handle(req).pipe(
      catchError(
        (err) => {
          if (err.status === 401) {
            return this.handleTokenExpired(req, next);
          }

          return throwError(err);
        })
    );
  }
  
  private addToken(request: HttpRequest<any>, token: string): HttpRequest<any> {
    return request.clone({
      setHeaders: {
        withCredentials: 'false',
        Authorization: `Bearer ${token}`,
      },
    });
  }

  private handleTokenExpired(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const accessToken = localStorage.getItem('accessToken');
    const refreshToken = localStorage.getItem('refreshToken');

    const requestData = {
      accessToken: accessToken,
      refreshToken: refreshToken,
    }

    return this.loginService.refresh(requestData).pipe(
      switchMap(() => {
        const newAccessToken:any = localStorage.getItem('accessToken');

        return next.handle(this.addToken(request, newAccessToken));
      }),
      catchError((error) => {
        console.error('Error handling expired access token:', error);
        return throwError(error);
      })
    );
  }

}