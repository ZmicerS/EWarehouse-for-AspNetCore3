import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { StoreComponent } from './store.component';
import { StoreAddComponent } from './store-add/store-add.component';
import { StoreDeleteComponent } from './store-delete/store-delete.component';
import { StoreEditComponent } from './store-edit/store-edit.component';
import { StoreListComponent } from './store-list/store-list.component';

import { AuthGuard, CanDeactivateGuard } from '../core';

const routes: Routes = [
  {
    path: '', component: StoreComponent, canActivate: [AuthGuard],
    children: [
      { path: '', redirectTo: 'list', pathMatch: 'full' },
      { path: 'add', component: StoreAddComponent, canDeactivate: [CanDeactivateGuard] },
      { path: 'list', component: StoreListComponent },
      { path: 'edit/:id', component: StoreEditComponent, canDeactivate: [CanDeactivateGuard] },
      { path: 'delete/:id', component: StoreDeleteComponent }
    ]
  }
];

@NgModule({
  declarations: [StoreComponent, StoreAddComponent, StoreDeleteComponent, StoreEditComponent, StoreListComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule
  ],
  exports: [RouterModule,
    StoreComponent, StoreAddComponent, StoreDeleteComponent, StoreEditComponent, StoreListComponent
  ]
})
export class StoreRoutingModule { }
