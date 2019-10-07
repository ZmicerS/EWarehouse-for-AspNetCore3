import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

import { forkJoin } from 'rxjs';

import { Role, User, AssignedRole, UserRoles } from '../../core';
import { AccountService } from '../../core';

@Component({
  selector: 'app-user-delete',
  templateUrl: './user-delete.component.html',
  styleUrls: ['./user-delete.component.scss']
})
export class UserDeleteComponent implements OnInit {

  private id: number;
  user: User;
  roles: Role[];
  userRoles: UserRoles = {} as UserRoles;
  assignedRoles: AssignedRole[];

  constructor(
    private accountService: AccountService,
    private activatedRoute: ActivatedRoute,
    private router: Router) {
  }

  ngOnInit() {
    this.getAssignedRoles();
  }

  private getAssignedRoles(): void {
    this.activatedRoute.paramMap.subscribe(
      (params: ParamMap) => {
        this.id = +params.get('id');
        const roles = this.accountService.getRoles();
        const user = this.accountService.getUser(this.id);
        forkJoin([roles, user]).subscribe(results => {
          const resRoles = results[0];
          const resUser = results[1];
          this.user = {} as User;
          this.user.id = resUser.id;
          this.user.userName = resUser.userName;

          this.assignedRoles = new Array<AssignedRole>();
          for (const index in resRoles) {
            if (index) {
              const element = resUser.assignedRoles.find(x => x.id === resRoles[index].id);
              const assignedRole: AssignedRole = {
                roleId: resRoles[index].id,
                roleName: resRoles[index].roleName,
                assigned: (element !== null && typeof element !== 'undefined') ? true : false
              };
              this.assignedRoles.push(assignedRole);
            }
          }
          this.userRoles.id = resUser.id;
          this.userRoles.userName = resUser.userName;
          this.userRoles.assignedRoles = this.assignedRoles;
        });
      });
  }

  onDelete() {
    this.accountService.delete(this.id).subscribe(() => {
      this.router.navigate(['../../list'], { relativeTo: this.activatedRoute });
    });
  }

  onCancel() {
    this.router.navigate(['../../list'], { relativeTo: this.activatedRoute });
  }

}

