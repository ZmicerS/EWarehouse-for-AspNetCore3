import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor() { }
  isLogged() {
    const token = sessionStorage.getItem('access_token');
    if (token) {
      return true;
    } else {
      return false;
    }
  }

  login(token: string) {
    sessionStorage.setItem('access_token', token);
  }

  logout() {
    sessionStorage.removeItem('access_token');
  }

  getToken(): string {
   return  sessionStorage.getItem('access_token');
  }
}
