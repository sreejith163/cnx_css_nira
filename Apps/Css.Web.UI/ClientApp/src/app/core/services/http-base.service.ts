import { HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { throwError as observableThrowError , Observable } from 'rxjs';

@Injectable()
export class HttpBaseService {

  constructor() { }

  protected convertToHttpParam(data): HttpParams {
    let httpParams = new HttpParams();
    Object.keys(data).forEach((key) => {
        httpParams = httpParams.append(key, data[key]);
      });
    return httpParams;
  }

  protected extractData(response: Response) {
    return response !== undefined && response !== null ? response : {};
  }

  protected handleError(error: Response): Observable<any> {
    return observableThrowError(error || 'Server error');
  }
}
