import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService, AuthService } from '../core';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss']
})
export class AccountComponent implements OnInit {

  _permission = false;
  get permission() {
    if (this._permission != null) {
      return this._permission;
    }
  }

  set permission(value: boolean) {
    if (value) {
      this._permission = value;
    }
  }

  constructor(private router: Router, private accountService: AccountService, private authService: AuthService) { }

  ngOnInit() {
    this.getPermission();
  }

  onLogout() {
    this.authService.logout();
    this.permission = false;
    this.router.navigate(['/']);
  }

  private getPermission() {
    this.accountService.getPermission().subscribe(res => {
      this.permission = res.result;
      return this.permission;
    });
    return this.permission;
  }

  isLogged() {
    if (!!this.authService.getToken()) {
      return true;
    } else {
      return false;
    }
  }

}
