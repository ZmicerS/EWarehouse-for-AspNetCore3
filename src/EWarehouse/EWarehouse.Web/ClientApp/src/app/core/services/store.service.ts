import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Book, Language } from '../models';

@Injectable({
  providedIn: 'root'
})
export class StoreService {
  books: Book[];
  languages: Language[];

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }

  addBook(formData: any): Observable<any> {
    const addUrl = this.baseUrl + 'store/addbook';
    return this.http.post(`${addUrl}`, formData);
  }

  getBooks(): Observable<Book[]> {
    const getBooksUrl = this.baseUrl + 'store/getbooks';
    return this.http.get<Book[]>(`${getBooksUrl}`);
  }

  updateBook(formData: any): Observable<any> {
    const updateUrl = this.baseUrl + 'store/updatebook';
    return this.http.put(`${updateUrl}`, formData);
  }

  getBook(id: number | string): Observable<Book> {
    const getBookUrl = this.baseUrl + 'store/getbook';
    return this.http.get<Book>(`${getBookUrl}/${id}`);
  }

  getImage(id: number | string) {
    const getImageUrl = this.baseUrl + 'store/getcover';
    return this.http.get(`${getImageUrl}/${id}`, { responseType: 'blob' });
  }

  deleteBook(id: number | string): Observable<any> {
    const deleteUrl = this.baseUrl + 'store/deletebook';
    return this.http.post(`${deleteUrl}`, id);
  }

  getLanguages(): Observable<Language[]> {
    const getLanguagesUrl = this.baseUrl + 'store/getlanguages';
    return this.http.get<Language[]>(`${getLanguagesUrl}`);
  }

  getPermission(): Observable<any> {
    const url = this.baseUrl + 'store/getpermissionstore';
    return this.http.get<boolean>(`${url}`);
  }

  getPermissionOrder(): Observable<any> {
    const url = this.baseUrl + 'store/getpermissionorder';
    return this.http.get<boolean>(`${url}`);
  }
}
