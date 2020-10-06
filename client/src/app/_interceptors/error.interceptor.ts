import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router, private toastr: ToastrService) {}

  // We can intercept the request that goes out of the response that goes back (next)
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    //return next.handle(request);
    return next.handle(request).pipe(
      catchError(error => {
        if (error) {
          switch (error.status) {
            // Bad Request - server couldn't understand request due to invalid syntax
            case 400:
              if (error.error.errors) {
                const modalStateErrors = [];
                for (const key in error.error.errors) {
                  if (error.error.errors[key]) {
                    modalStateErrors.push(error.error.errors[key])
                  }
                }
                throw modalStateErrors.flat();
              } else {
                if (error.statusText == "OK")
                  this.toastr.error(error.message, error.status);
                  //this.toastr.error("Bad Request", error.status);
                else
                  this.toastr.error(error.statusText, error.status);
              }
              break;
            // Unauthorized
            case 401:
              this.toastr.error("Unauthorized", error.status);
              //this.toastr.error(error.statusText, error.status);
              break;
            // Not Found
            case 404:
              this.router.navigateByUrl('/not-found');
              break;
              // Internal Server error
            case 500:
              const navigationExtras: NavigationExtras = {state: {error: error.error}};
              this.router.navigateByUrl('/server-error', navigationExtras);
              break;
            default:
              this.toastr.error('Something unexpected went wrong');
              console.log(error);
              break;
          }
        }
        return throwError(error); // We shouldn't hit this line
      })
    )
  }
}
