import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpRequest,
  HttpResponse,
  HttpHandler,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry, map, finalize } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor( ) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const startTime = Date.now();
    return next.handle(request)
      .pipe(
        map((event: HttpEvent<any>) => {
          if (event instanceof HttpResponse) {
          }
          return event;
        }),
        retry(2),
        catchError((error: HttpErrorResponse) => {
          let errorMessage = '';
          console.log(error);
          if (error && error.error && error.error instanceof ErrorEvent) {
            errorMessage = `Error: ${error.error.message}`;
          } else {
            errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
            if (error && error.error && error.error instanceof Array) {
              let errorsList: string;
              error.error.forEach((item, index) => {
                if (index !== 0) {
                  errorsList += ',';
                }
                errorsList += item;
              });
              errorMessage += ' ' + errorsList;
            }
          }
          const errorStatus = error.status || '';
          const errorObject = {
            status: errorStatus,
            message: errorMessage
          };
          return throwError(errorObject);
        }),
        finalize(() => {
          const elapsedTime = Date.now() - startTime;
          const message = request.method + ' ' + request.urlWithParams + ' ' + status
            + ' in ' + elapsedTime + 'ms';
          console.log(message);
        })
      );
  }

}
