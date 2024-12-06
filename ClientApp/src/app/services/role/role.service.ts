import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RoleService {

  private apiUrl = 'https://localhost:7266/roles';

  constructor(private http: HttpClient) {}
    getRoles(): Observable<any>{
      return this.http.get<any>(this.apiUrl);
    }
}
