import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MatSnackBarModule } from '@angular/material';

import { StoreRoutingModule } from './store-routing.module';

@NgModule({
  imports: [
    CommonModule,
    StoreRoutingModule,
    MatSnackBarModule
  ]
})
export class StoreModule { }
