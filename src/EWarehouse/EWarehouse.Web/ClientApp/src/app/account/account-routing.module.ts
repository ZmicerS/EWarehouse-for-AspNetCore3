import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CoreModule } from '../core';

import { AccountComponent } from './account.component';
import { RegisterComponent } from './register/register.component';
import { UserListComponent } from './user-list/user-list.component';
import { UserManageComponent } from './user-manage/user-manage.component';
import { UserDeleteComponent } from './user-delete/user-delete.component';
import { AuthGuard, CanDeactivateGuard } from '../core';

const routes: Routes = [
  {
    path: '', component: AccountComponent,
    children: [],
  },
  { path: 'register', component: RegisterComponent,  canDeactivate: [CanDeactivateGuard] },
  { path: 'list', component: UserListComponent, canActivate: [AuthGuard] },
  { path: 'edit/:id', component: UserManageComponent },
  { path: 'delete/:id', component: UserDeleteComponent }
];

@NgModule({
  imports: [
    CoreModule,
    RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AccountRoutingModule { }
