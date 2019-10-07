import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { httpInterceptorProviders } from './interceptors';
import { AuthGuard, CanDeactivateGuard } from './guards';

import {
  StoreService,
  AccountService
} from './services';


@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ],
  providers: [
    httpInterceptorProviders,
    StoreService,
    AccountService,
    AuthGuard,
    CanDeactivateGuard
  ]
})
export class CoreModule { }
