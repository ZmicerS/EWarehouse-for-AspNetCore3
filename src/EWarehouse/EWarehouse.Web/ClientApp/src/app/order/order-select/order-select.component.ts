import { Component, Input, EventEmitter, Output, OnChanges, OnInit } from '@angular/core';
import { StoreService } from '../../core';

@Component({
  selector: 'app-order-select',
  templateUrl: './order-select.component.html',
  styleUrls: ['./order-select.component.scss']
})
export class OrderSelectComponent implements OnInit {
  @Input() bookId: number;
  @Output() productCountChange = new EventEmitter();
  @Input() productCount: number;
  updatedProductCount: number;

  _permission = false;
  get permission() {
    if (this._permission != null) {
      return this._permission;
    }
  }

  set permission(value: boolean) {
    if (value) {
      this._permission = value;
    }
  }

  constructor(private storeService: StoreService) { }

  ngOnInit() {
    this.getPermission();
    this.updatedProductCount = this.productCount;
  }

  productCountChanged() {
    if (this.updatedProductCount > 0) {
      this.productCountChange.emit({ id: this.bookId, updatedProductCount: this.updatedProductCount });
    }
  }

  private getPermission() {
    this.storeService.getPermissionOrder().subscribe(res => {
      this.permission = res.result;
      return this.permission;
    });
    return this.permission;
  }

}
