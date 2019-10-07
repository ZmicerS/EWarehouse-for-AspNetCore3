import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { forkJoin } from 'rxjs';

import { Book, BookLanguagetext, Language } from '../../core';
import { StoreService } from '../../core';

@Component({
  selector: 'app-store-delete',
  templateUrl: './store-delete.component.html',
  styleUrls: ['./store-delete.component.scss']
})
export class StoreDeleteComponent implements OnInit {

  private id: number;
  private _book: Book;

  bookCollection: BookLanguagetext;
  languages: Language[];
  imageToShow: any;

  get book() {
    if (this._book != null) {
      return this._book;
    }
  }

  set book(value: Book) {
    if (value) {
      this._book = value;
    }
  }

  constructor(private storeService: StoreService,
              private activatedRoute: ActivatedRoute,
              private router: Router) {
               }

  ngOnInit() {
    this.book = {} as Book;
    this.book.languageName = '';
    this.getBookForDelete();
  }

  private getBookForDelete() {
    this.activatedRoute.paramMap.subscribe(
      (params: ParamMap) => {
        this.id = +params.get('id');
        this.getCover(this.id);
        const languagesReq = this.storeService.getLanguages();
        const bookReq = this.storeService.getBook(this.id);
        forkJoin([languagesReq, bookReq]).subscribe(results => {
          const resultLanguages = results[0];
          const resultBook = results[1];
          this.book = resultBook as Book;
          this.book.languageName = resultLanguages.find(c => c.id === this.book.languageId).fullName;
        });
      });
  }

  private createImageFromBlob(image: Blob) {
    const reader = new FileReader();
    reader.addEventListener('load', () => {
      this.imageToShow = reader.result;
    }, false);
    if (image) {
      reader.readAsDataURL(image);
    }
  }

  private getCover(id: number | string) {
    this.storeService.getImage(id).subscribe(data => {
      this.createImageFromBlob(data);
    }, error => {
      console.log(error);
    });
  }

  onDelete() {
    this.storeService.deleteBook(this.id).subscribe(_ => {
      this.router.navigate(['../../list'], { relativeTo: this.activatedRoute });
    });
  }

  onCancel() {
    this.router.navigate(['../../list'], { relativeTo: this.activatedRoute });
  }

}
