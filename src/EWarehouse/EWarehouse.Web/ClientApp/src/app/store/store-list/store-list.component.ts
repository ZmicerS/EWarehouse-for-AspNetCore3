import { Component, OnInit } from '@angular/core';

import { forkJoin } from 'rxjs';

import { Book, BookLanguagetext, Language } from '../../core';
import { StoreService } from '../../core';


@Component({
  selector: 'app-store-list',
  templateUrl: './store-list.component.html',
  styleUrls: ['./store-list.component.scss']
})
export class StoreListComponent implements OnInit {
  books: Book[];
  bookCollection: BookLanguagetext[];
  _languages: Language[];

  get languages() {
    if (this._languages != null) {
      return this._languages;
    }
  }

  set languages(value: Language[]) {
    if (value) {
      this._languages = value;
    }
  }

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
    this.getBooks();
  }

  private getBooks(): void {
    const languagesReq = this.storeService.getLanguages();
    const bookReq = this.storeService.getBooks();
    forkJoin([languagesReq, bookReq]).subscribe(results => {
      const resultLanguages = results[0];
      const resultBooks = results[1];
      this.books = resultBooks as Book[];
      this.languages = resultLanguages as Language[];
      this.bookCollection = this.books.map(b => {
        const book = {} as BookLanguagetext;
        book.id = b.id;
        book.name = b.name;
        book.author = b.author;
        book.isbn = b.isbn;
        book.price = b.price;
        book.quantity = b.quantity;
        book.content = b.content;
        book.languageName = this.languages.find(c => c.id === b.languageId).fullName;
        return book;
      });
    });
  }

  private getPermission() {
    this.storeService.getPermission().subscribe(res => {
      this.permission = res.result;
      return this.permission;
    });
    return this.permission;
  }

}
