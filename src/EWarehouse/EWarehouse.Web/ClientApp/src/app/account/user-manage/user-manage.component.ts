import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, FormArray } from '@angular/forms';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

import { forkJoin } from 'rxjs';

import { Role, User, AssignedRole, UserRoles } from '../../core';
import { AccountService } from '../../core';

@Component({
  selector: 'app-user-manage',
  templateUrl: './user-manage.component.html',
  styleUrls: ['./user-manage.component.scss']
})
export class UserManageComponent implements OnInit {

  roleForm: FormGroup;

  private id: number;
  user: User;
  roles: Role[];
  userRoles: UserRoles = {} as UserRoles;
  assignedRoles: AssignedRole[];

  constructor(
    private fb: FormBuilder,
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
          this.initForm();
        });
      });
  }

  private initForm() {
    const formControls = this.assignedRoles.map(control => new FormControl(control.assigned));
    this.roleForm = this.fb.group({
      assignedRoles: new FormArray(formControls)
    });
  }

  onSubmit() {
    this.assignRoles();
  }

  private assignRoles() {
    this.roles = new Array<Role>();
    this.roleForm.value.assignedRoles
      .map((checked: boolean, index: number): any => {
        if (checked) {
          this.assignedRoles[index].assigned = checked;
          const role = {} as Role;
          role.id = this.assignedRoles[index].roleId;
          role.roleName = this.assignedRoles[index].roleName;
          this.roles.push(role);
        }
      });

    this.user = {} as User;
    this.user.id = this.userRoles.id;
    this.user.userName = this.userRoles.userName;
    this.user.assignedRoles = this.roles;
    this.accountService.assignRoles(this.user).subscribe(() => {
      this.router.navigate(['../../list'], { relativeTo: this.activatedRoute });
    });
  }

  onCancel() {
    this.router.navigate(['../../list'], { relativeTo: this.activatedRoute });
  }

}
