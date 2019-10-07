import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { OrderComponent } from './order.component';
import { OrderAcceptComponent } from './order-accept/order-accept.component';

const routes: Routes = [
  { path: 'orderaccept', component: OrderAcceptComponent },
  { path: '', component: OrderComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class OrderRoutingModule { }
