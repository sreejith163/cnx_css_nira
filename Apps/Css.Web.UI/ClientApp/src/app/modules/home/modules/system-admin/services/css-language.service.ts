import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { HttpBaseService } from 'src/app/core/services/http-base.service';
import { KeyValue } from 'src/app/shared/models/key-value.model';
import { environment } from 'src/environments/environment';

@Injectable()

export class CssLanguageervice extends HttpBaseService {
  private baseURL = '';
  constructor(
    private http: HttpClient
  ) {
    super();
    this.baseURL = environment.services.adminService;
  }

  getCssLanguage() {
    const url = `${this.baseURL}/CssLanguage`;
    return this.http.get<KeyValue>(url)
      .pipe(catchError(this.handleError));
  }
}
