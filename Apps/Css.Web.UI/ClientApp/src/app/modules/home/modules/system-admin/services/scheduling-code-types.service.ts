import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { HttpBaseService } from 'src/app/core/services/http-base.service';
import { environment } from 'src/environments/environment';
import { SchedulingCodeType } from '../models/scheduling-code-type.model';

@Injectable()
export class SchedulingCodeTypesService extends HttpBaseService {

  private baseURL = '';

  constructor(
    private http: HttpClient
  ) {
    super();
    this.baseURL = environment.services.adminService;
  }

  getSchedulingCodeTypes() {
    const url = `${this.baseURL}/schedulingCodeTypes`;

    return this.http.get<SchedulingCodeType>(url)
      .pipe(catchError(this.handleError));
  }
}
