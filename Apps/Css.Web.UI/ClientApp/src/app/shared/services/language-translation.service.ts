import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { HttpBaseService } from 'src/app/core/services/http-base.service';
import { TranslationQueryParams } from 'src/app/shared/models/translation-query-params.model';
import { environment } from 'src/environments/environment';
import { AddTranslation } from '../models/add-translation.model';
import { ApiResponseModel } from '../models/api-response.model';
import { TranslationDetails } from '../models/translation-details.model';
import { UpdateTranslation } from '../models/update-translation.model';

@Injectable()

export class LanguageTranslationService extends HttpBaseService {

  private baseURL = '';

  constructor(
    private http: HttpClient
  ) {
    super();
    this.baseURL = environment.services.adminService;
  }

  getLanguageTranslations(translationQueryParams: TranslationQueryParams) {
    const url = `${this.baseURL}/LanguageTranslations`;

    return this.http.get<TranslationDetails>(url,
      { params: this.convertToHttpParam(translationQueryParams), observe: 'response' })
      .pipe(catchError(this.handleError));
  }

  getLanguageTranslation(translationId: number) {
    const url = `${this.baseURL}/LanguageTranslations/${translationId}`;

    return this.http.get<TranslationDetails>(url)
      .pipe(catchError(this.handleError));
  }

  getMenuTranslations(languageId: number, menuId: number) {
    const url = `${this.baseURL}/LanguageTranslations/languages/${languageId}/menus/${menuId}`;
    return this.http.get<TranslationDetails>(url)
    .pipe(catchError(this.handleError));
  }

  addLanguageTranslation(translation: AddTranslation) {
    const url = `${this.baseURL}/LanguageTranslations`;

    return this.http.post<ApiResponseModel>(url, translation)
      .pipe(catchError(this.handleError));
  }

  updateLanguageTranslation(translationId: number, translation: UpdateTranslation) {
    const url = `${this.baseURL}/LanguageTranslations/${translationId}`;

    return this.http.put<ApiResponseModel>(url, translation)
      .pipe(catchError(this.handleError));
  }

  deleteLanguageTranslation(translationId: number) {
    const url = `${this.baseURL}/LanguageTranslations/${translationId}`;

    return this.http.delete<ApiResponseModel>(url)
      .pipe(catchError(this.handleError));
  }
}
