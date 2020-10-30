import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpErrorResponse, HTTP_INTERCEPTORS } from '@angular/common/http';
import { Observable, BehaviorSubject, throwError } from 'rxjs';
import { catchError, switchMap, filter, take } from 'rxjs/operators';
import { CookieService } from 'ngx-cookie-service';

@Injectable()
export class UnAuthorizedInterceptor implements HttpInterceptor {

  private isRefreshing = false;
  private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);

  constructor(
    public cookieService: CookieService) {
  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(catchError(error => {
      if (error instanceof HttpErrorResponse && error.status === 401) {
        const accessToken = this.cookieService.get('');
        if (!accessToken) {
          // Revoke token;
        }
        return this.handle401Error(request, next);
      } else {
        return throwError(error);
      }
    }));
  }

  private handle401Error(request: HttpRequest<any>, next: HttpHandler) {
    if (!this.isRefreshing) {
      this.isRefreshing = true;
      this.refreshTokenSubject.next(null);

      // return this.authorizationService.refreshToken().pipe(
      //   switchMap(() => {
      //     this.isRefreshing = false;
      //     this.refreshTokenSubject.next(this.cookieService.get(''));
      //     return next.handle(this.addTokenToRequest(request, this.cookieService.get('')));
      //   }));
    } else {
      return this.refreshTokenSubject.pipe(
        filter(token => token),
        take(1),
        switchMap(token => {
          return next.handle(this.addTokenToRequest(request, token));
        }));
    }
  }

  private addTokenToRequest(request: HttpRequest<any>, token: string) {
    return request.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }
}

export const UnAuthorizedInterceptorProviders = [
  { provide: HTTP_INTERCEPTORS, useClass: UnAuthorizedInterceptor, multi: true },
];


