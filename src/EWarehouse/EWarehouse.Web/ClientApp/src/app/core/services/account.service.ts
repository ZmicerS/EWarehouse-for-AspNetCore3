import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

import { RegisterModel, LoginModel, Role, User } from '../models';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  httpOptionsJson = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };

  private baseUrl: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  register(registerData: RegisterModel): Observable<any> {
    const registerUrl = this.baseUrl + 'account/register';
    return this.http.post<RegisterModel>(`${registerUrl}`, registerData, this.httpOptionsJson);
  }

  login(loginData: LoginModel): Observable<any> {
    const loginUrl = this.baseUrl + 'token';
    return this.http.post<LoginModel>(`${loginUrl}`, loginData, this.httpOptionsJson);
  }

  getRoles(): Observable<Role[]> {
    const rolesUrl = this.baseUrl + 'account/getroles';
    return this.http.get<Role[]>(`${rolesUrl}`);
  }

  getUsers(): Observable<User[]> {
    const usersUrl = this.baseUrl + 'account/getusers';
    return this.http.get<User[]>(`${usersUrl}`);
  }

  getUser(id: number | string): Observable<User> {
    const userUrl = this.baseUrl + 'account/getuser';
    return this.http.get<User>(`${userUrl}/${id}`);
  }

  assignRoles(user: User): Observable<any> {
    const assignRolesUrl = this.baseUrl + 'account/assignroles';
    return this.http.post<User>(`${assignRolesUrl}`, user, this.httpOptionsJson);
  }

  delete(id: number | string): Observable<any> {
    const deleteUrl = this.baseUrl + 'account/delete';
    return this.http.delete(`${deleteUrl}/${id}`);
  }

  getPermission(): Observable<any> {
    const url = this.baseUrl + 'account/getpermission';
    return this.http.get<boolean>(`${url}`);
  }

}
