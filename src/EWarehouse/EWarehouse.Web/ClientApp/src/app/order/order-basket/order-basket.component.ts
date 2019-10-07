import { Component, Input, EventEmitter, Output, OnInit } from '@angular/core';

@Component({
  selector: 'app-order-basket',
  templateUrl: './order-basket.component.html',
  styleUrls: ['./order-basket.component.scss']
})
export class OrderBasketComponent implements OnInit {

  @Input() orderedProducts: Array<any>;
  @Output() deleteSelectedBook = new EventEmitter();

  constructor() { }

  ngOnInit() {
  }

  onDeleteBook(o: any) {
    this.deleteSelectedBook.emit( {id: o.id} );
  }

}
