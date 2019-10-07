import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ViewChild, ElementRef } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';

import { MatSnackBar } from '@angular/material/snack-bar';

import { Book, Language } from '../../core';
import { StoreService } from '../../core';


@Component({
  selector: 'app-store-add',
  templateUrl: './store-add.component.html',
  styleUrls: ['./store-add.component.scss']
})
export class StoreAddComponent implements OnInit {
  @ViewChild('fileInput', { static: false }) fileInput: ElementRef<any>;

  selected: boolean;
  imagePath: any;
  imgURL: any;

  book: Book;
  languages: Language[];

  bookStoreAddForm: FormGroup;
  _submitting = false;

  get submitting() {
    return this._submitting;
  }

  set submitting(value: boolean) {
    if (value) {
      this._submitting = value;
    }
  }
  constructor(
    private fb: FormBuilder,
    private storeService: StoreService,
    private router: Router,
    public snackBar: MatSnackBar) { }


  ngOnInit() {
    this.getLanguages();
  }

  showSnack() {
    this.snackBar.open('Ok!', 'Add', {
      duration: 2000,
      horizontalPosition: 'center',
      verticalPosition: 'bottom',
      panelClass: ['blue-snackbar']
    });
  }

  private initForm() {
    this.bookStoreAddForm = this.fb.group({
      bookName: ['', Validators.required],
      bookAuthor: ['', Validators.required],
      bookIsbn: ['', Validators.required],
      bookPrice: ['', [Validators.required, Validators.pattern(/^[.\d]+$/)]],
      bookQuantity: ['', [Validators.required, Validators.pattern(/^[.\d]+$/)]],
      bookContent: ['', Validators.required],
      bookLanguage: [this.languages[0]]
    });

  }

  isControlInvalid(controlName: string): boolean {
    const control = this.bookStoreAddForm.controls[controlName];
    const result = control.invalid && control.touched;
    return result;
  }

  upload(files: Blob[]) {
    if (files.length === 0) { return; }
    const reader = new FileReader();
    this.imagePath = files;
    reader.readAsDataURL(files[0]);
    reader.onload = (_) => {
      this.imgURL = reader.result;
      this.selected = true;
    };

  }

  onSubmit() {
    const controls = this.bookStoreAddForm.controls;

    if (this.bookStoreAddForm.invalid) {
      Object.keys(controls)
        .forEach(controlName => controls[controlName].markAsTouched());
      return;
    }

    this._submitting = true;

    const currentBook = {} as Book;
    currentBook.name = this.bookStoreAddForm.value.bookName;
    currentBook.author = this.bookStoreAddForm.value.bookAuthor;
    currentBook.isbn = this.bookStoreAddForm.value.bookIsbn;
    currentBook.price = this.bookStoreAddForm.value.bookPrice;
    currentBook.content = this.bookStoreAddForm.value.bookContent;
    currentBook.quantity = this.bookStoreAddForm.value.bookQuantity;
    currentBook.languageId = this.bookStoreAddForm.value.bookLanguage.id;
    this.bookStoreAddForm.disable();
    this.addBook(currentBook);
  }

  private addBook(book: Book): any {
    const formData = new FormData();
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

    this.storeService.addBook(formData).subscribe(result => {
      this.showSnack();
      this.bookStoreAddForm.reset();
      const controlName = 'bookLanguage';
      const control = this.bookStoreAddForm.controls[controlName];
      control.setValue(this.languages[0]);
      this.imgURL = 'undefined';
      this._submitting = false;
      this.bookStoreAddForm.enable();
    });
  }

  private getLanguages(): void {
    this.storeService.getLanguages().subscribe(res => {
      this.languages = res as Language[];
      this.initForm();
    });
  }

  onCancel() {
    this.router.navigate(['/']);
  }

  onReset() {
    this.bookStoreAddForm.reset();
  }

  canDeactivate(): Observable<boolean> | boolean {

    if (this.bookStoreAddForm.dirty && !this._submitting) {
      return window.confirm('Do you want leave this page?');
    }
    return true;
  }

}
