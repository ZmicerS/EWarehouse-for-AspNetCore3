import { Component, OnInit } from '@angular/core';
import { AccountService } from '../../core';
import { User } from '../../core';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss']
})
export class UserListComponent implements OnInit {
  users: User[];

  constructor(
    private accountService: AccountService) { }

  ngOnInit() {
    this.getUsers();
  }

 private getUsers(): void {
    this.accountService.getUsers()
      .subscribe(users => {
        this.users = users as User[];
      });
  }
}
