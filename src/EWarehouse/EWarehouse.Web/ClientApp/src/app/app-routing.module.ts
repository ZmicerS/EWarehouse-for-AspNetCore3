import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ErrorsComponent } from './core/errors/errors.component';

const routes: Routes = [
  { path: 'error', component: ErrorsComponent },
  // todo It works for JIT compilation but doesn't work for AOT.
  // { path: 'account', loadChildren: () => import(`./account/account.module`).then(m => m.AccountModule) },
  // { path: 'store', loadChildren: () => import(`./store/store.module`).then(m => m.StoreModule) },
  //  { path: 'order', loadChildren: () => import(`./order/order.module`).then(m => m.OrderModule) }
  // todo It works for AOT compilation. Need cli  npm install @angular/compiler-cli --save
  { path: 'account', loadChildren: './account/account.module#AccountModule' },
  { path: 'store', loadChildren: './store/store.module#StoreModule' },
  { path: 'order', loadChildren: './order/order.module#OrderModule' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes,
    {
      enableTracing: false // for debug
    })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
/*
[routerLink]="['/absolute']"
[routerLink]="['../../parent']"
[routerLink]="['../sibling']"
[routerLink]="['./child']"     // or
[routerLink]="['child']"

 with route param     ../../parent;abc=xyz
[routerLink]="['../../parent', {abc: 'xyz'}]"
 with query param and fragment   ../../parent?p1=value1&p2=v2#frag
[routerLink]="['../../parent']" [queryParams]="{p1: 'value', p2: 'v2'}" fragment="frag"
*/
