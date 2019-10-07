import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MatSnackBarModule } from '@angular/material/snack-bar';
import { FormsModule } from '@angular/forms';

import { OrderRoutingModule } from './order-routing.module';
import { OrderComponent } from './order.component';
import { OrderAcceptComponent } from './order-accept/order-accept.component';
import { OrderBasketComponent } from './order-basket/order-basket.component';
import { OrderSelectComponent } from './order-select/order-select.component';


@NgModule({
  declarations: [OrderComponent, OrderAcceptComponent, OrderBasketComponent, OrderSelectComponent],
  imports: [
    CommonModule,
    FormsModule,
    MatSnackBarModule,
    OrderRoutingModule
  ]
})
export class OrderModule { }
