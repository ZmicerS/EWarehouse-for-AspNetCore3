import { Component, OnInit } from '@angular/core';

import { Book } from '../core';
import { StoreService } from '../core';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.scss']
})
export class OrderComponent implements OnInit {

  books: Book[];
  orderValue: number;
  selectedBooks = new Array<any>();

  constructor(private sroreService: StoreService) { }

  ngOnInit() {
    this.getBooks();
  }

  private getBooks(): void {
    this.sroreService.getBooks()
      .subscribe(books => {
        this.books = books as Book[];
      });
  }

  changeProductCount(p: any) {
    this.orderValue = p.updatedProductCount;
    const bookName = this.books.find(b => b.id === p.id).name;
    const selectedBook = { id: p.id, count: p.updatedProductCount, name: bookName };
    const ob = this.selectedBooks.find(v => v.id === p.id);
    if (ob) {
      const index = this.selectedBooks.indexOf(ob);
      this.selectedBooks[index] = selectedBook;
    } else {
      this.selectedBooks.push({ id: p.id, name: bookName, count: p.updatedProductCount });
    }
  }

  deleteSelectedBook(id: number) {
    const index = this.selectedBooks.findIndex(f => f.id === id);
    this.selectedBooks.splice(index, 1);
  }

}
