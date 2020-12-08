import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { HttpBaseService } from 'src/app/core/services/http-base.service';
import { environment } from 'src/environments/environment';
import { Translation } from '../models/translation.model';

@Injectable()
export class TranslationService extends HttpBaseService {

  private baseURL = '';

  constructor(
    private http: HttpClient
  ) {
    super();
    this.baseURL = environment.services.gatewayService;
  }

  getMenuTranslations(menuId: number, languageId: number) {
    const url = `${this.baseURL}/translations/languages/${languageId}/menus/${menuId}`;

    return this.http.get<Translation>(url)
      .pipe(catchError(this.handleError));
  }
}
