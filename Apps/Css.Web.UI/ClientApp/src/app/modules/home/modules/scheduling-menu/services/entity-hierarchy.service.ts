import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ApiResponseModel } from 'src/app/shared/models/api-response.model';
import { HttpBaseService } from 'src/app/core/services/http-base.service';

import { catchError } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { EntityHierarchyModel } from '../models/entity-hierarchy.model';


@Injectable({
  providedIn: 'root'
})
export class EntityHierarchyService extends HttpBaseService {
  test: any = [];
  private baseURL = '';

  constructor(
    private http: HttpClient
  ) {
    super();
    this.baseURL = environment.services.gatewayService;
  }

  getEntityHierarchyDataById(clientId: number) {
    const url = `${this.baseURL}/EntityHierarchy/${clientId}?`;

    return this.http.get<EntityHierarchyModel>(url)
      .pipe(catchError(this.handleError));
  }


}
