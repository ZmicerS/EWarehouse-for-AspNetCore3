import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { Subscription } from 'rxjs';
import { ViewChild, ElementRef } from '@angular/core';

import { forkJoin } from 'rxjs';

import { Book, Language } from '../../core';
import { StoreService } from '../../core';


@Component({
  selector: 'app-store-edit',
  templateUrl: './store-edit.component.html',
  styleUrls: ['./store-edit.component.scss']
})
export class StoreEditComponent implements OnInit {

  @ViewChild('fileInput', { static: false }) fileInput: ElementRef<any>;
  selected: boolean;
  imagePath: any;
  _imgURL: any;
  _submitting = false;

  private id: number;
  private routeSubscription: Subscription;
  book$: Observable<Book>;
  book: Book;
  languages: Language[];

  imageToShow: any;
  isImageLoading: boolean;
  bookStoreEditForm: FormGroup;

  get imgURL() {
    if (this._imgURL != null) {
      return this._imgURL;
    }
  }

  set imgURL(value: any) {
    if (value) {
      this._imgURL = value;
    }
  }

  get submitting() {
    return this._submitting;
  }

  set submitting(value: boolean) {
    if (value) {
      this._submitting = value;
    }
  }

  constructor(
    private storeService: StoreService,
    private activatedRoute: ActivatedRoute,
    private router: Router) { }

  ngOnInit() {
    this.getBook();
  }

  private getBook() {
    this.activatedRoute.paramMap.subscribe(
      (params: ParamMap) => {
        this.id = +params.get('id');
        this.getCover(this.id);
        const languagesReq = this.storeService.getLanguages();
        const bookReq = this.storeService.getBook(this.id);
        forkJoin([languagesReq, bookReq]).subscribe(results => {
          const resultLanguages = results[0];
          const resultBook = results[1];
          this.languages = resultLanguages as Language[]
          this.book = resultBook as Book;
          this.book.languageName = this.languages.find(c => c.id === this.book.languageId).fullName;
          this.initForm();
        });
      });
  }

  private createImageFromBlob(image: Blob) {
    const reader = new FileReader();
    reader.addEventListener('load', () => {
      this.imgURL = reader.result;
    }, false);
    if (image) {
      reader.readAsDataURL(image);
    }
  }

  private getCover(id: number | string) {
    this.isImageLoading = true;
    this.storeService.getImage(id).subscribe(data => {
      this.createImageFromBlob(data);
      this.isImageLoading = true;
    }, error => {
      this.isImageLoading = false;
      console.log(error);
    });
  }

  private initForm() {
    this.bookStoreEditForm = new FormGroup({
      bookName: new FormControl(this.book.name, Validators.required),
      bookAuthor: new FormControl(this.book.author, [Validators.required]),
      bookIsbn: new FormControl(this.book.isbn, [Validators.required]),
      bookPrice: new FormControl(this.book.price, [Validators.required, Validators.pattern(/^[.\d]+$/)]),
      bookQuantity: new FormControl(this.book.quantity, [Validators.required, Validators.pattern(/^[.\d]+$/)]),
      bookContent: new FormControl(this.book.content, [Validators.required]),

      bookLanguage: new FormControl(this.languages[this.book.languageId - 1])
    });
  }

  isControlInvalid(controlName: string): boolean {
    const control = this.bookStoreEditForm.controls[controlName];
    const result = control.invalid && control.touched;
    return result;
  }

  upload(files: Blob[]) {
    if (files.length === 0) { return; }
    const reader = new FileReader();
    reader.readAsDataURL(files[0]);
    reader.onload = (event) => {
      this.imgURL = reader.result;
      this.selected = true;
    };
  }

  onSubmit() {
    const controls = this.bookStoreEditForm.controls;

    if (this.bookStoreEditForm.invalid) {
      Object.keys(controls)
        .forEach(controlName => controls[controlName].markAsTouched());
      return;
    }

    const currentBook = {} as Book;
    currentBook.id = this.book.id;
    currentBook.name = this.bookStoreEditForm.value.bookName;
    currentBook.author = this.bookStoreEditForm.value.bookAuthor;
    currentBook.isbn = this.bookStoreEditForm.value.bookIsbn;
    currentBook.price = this.bookStoreEditForm.value.bookPrice;
    currentBook.content = this.bookStoreEditForm.value.bookContent;
    currentBook.quantity = this.bookStoreEditForm.value.bookQuantity;
    currentBook.languageId = this.bookStoreEditForm.value.bookLanguage.id;
    this._submitting = true;
    this.updateProduct(currentBook);
  }

  private updateProduct(book: Book): any {
    const formData = new FormData();
    formData.append('id', (book.id) ? book.id.toString() : '');
    formData.append('name', book.name);
    formData.append('author', book.author);
    formData.append('isbn', book.isbn);
    formData.append('price', (book.price) ? book.price.toString() : '');
    formData.append('quantity', (book.quantity) ? book.quantity.toString() : '');
    formData.append('content', book.content);
    formData.append('languageid', (book.languageId) ? book.languageId.toString() : '');
    for (const file of this.fileInput.nativeElement.files) {
      formData.append(file.name, file);
    }
    this.storeService.updateBook(formData).subscribe(event => {
      this.router.navigate(['../../list'], { relativeTo: this.activatedRoute });
    });
  }

  onCancel() {
    this.router.navigate(['../../list'], { relativeTo: this.activatedRoute })
  }

  canDeactivate(): Observable<boolean> | boolean {
    if (this.bookStoreEditForm.dirty && !this._submitting) {
      return window.confirm('Do you want leave this page?');
    }
    return true;
  }

}
