import { HttpClient, HttpParams } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { PaginatedResult } from '../_models/pagination';

// Previous get method would get body.  Now that we're passing up the params we'll get the full response back and we need to look into the body to get the body.
export function getPaginatedResult<T>(url, params: HttpParams, http: HttpClient) {
    const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();

    return http.get<T>(url, { observe: 'response', params }).pipe(
      map(response => {
        paginatedResult.result = response.body;
        if (response.headers.get('Pagination') !== null) {
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return paginatedResult;
      })
    );
  }

export function getPaginationHeaders(pageNumber: number, pageSize: number) {
    let params = new HttpParams();

    params = params.append('pageNumber', pageNumber.toString());
    params = params.append('pageSize', pageSize.toString());

    return params;
}