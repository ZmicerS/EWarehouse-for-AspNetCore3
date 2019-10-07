import { ErrorHandler, Injectable, Injector, NgZone } from '@angular/core';
import { Router } from '@angular/router';

@Injectable()
export class GlobalErrorHandler implements ErrorHandler {
  constructor(private injector: Injector) { }
  handleError(error: any) {
    const router = this.injector.get(Router);
    const zone = this.injector.get(NgZone);

    if (error && error.status && error.status === 401) {
      zone.run(() => { router.navigate(['/account']); });
    } else {
      zone.run(() => { router.navigate(['/error'], { queryParams: { error: error.message } }); });
    }
  }
}
