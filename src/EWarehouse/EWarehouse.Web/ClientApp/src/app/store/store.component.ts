import { Component, OnInit } from '@angular/core';
import { StoreService } from '../core';

@Component({
  selector: 'app-store',
  templateUrl: './store.component.html',
  styleUrls: ['./store.component.scss']
})
export class StoreComponent implements OnInit {

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
  }

  private getPermission() {
    this.storeService.getPermission().subscribe(res => {
      this.permission = res.result;
      return this.permission;
    });
    return this.permission;
  }

}
