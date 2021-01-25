import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { HttpBaseService } from 'src/app/core/services/http-base.service';
import { environment } from 'src/environments/environment';
import { ApiResponseModel } from '../models/api-response.model';
import { LanguagePreference } from '../models/language-preference.model';

@Injectable({
  providedIn: 'root'
})
export class LanguagePreferenceService extends HttpBaseService {
  private baseURL = '';

  userLanguageChanged = new BehaviorSubject<number>(undefined);

  constructor(
    private http: HttpClient
  ) {
    super();
    this.baseURL = environment.services.gatewayService;
  }

  getLanguagePreference(employeeId: string) {
    const url = `${this.baseURL}/userpermissions/${employeeId}`;
    return this.http.get<LanguagePreference>(url)
      .pipe(catchError(this.handleError));
  }

  setLanguagePreference(employeeId: string, language: string) {
    const languagePreference: LanguagePreference = { languagePreference: language };
    const url = `${this.baseURL}/user/${employeeId}/language/`;

    return this.http.put<ApiResponseModel>(url, languagePreference)
      .pipe(
        tap(() => {
          this.userLanguageChanged.next(1);
        }),
        catchError(this.handleError)
      );
  }
}
