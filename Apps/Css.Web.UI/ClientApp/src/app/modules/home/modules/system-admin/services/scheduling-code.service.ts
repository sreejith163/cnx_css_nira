import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { HttpBaseService } from 'src/app/core/services/http-base.service';
import { ApiResponseModel } from 'src/app/shared/models/api-response.model';
import { QueryStringParameters } from 'src/app/shared/models/query-string-parameters.model';
import { environment } from 'src/environments/environment';
import { AddSchedulingCode } from '../models/add-scheduling-code.model';
import { SchedulingCodeDetails } from '../models/scheduling-code-details.model';
import { UpdateSchedulingCode } from '../models/update-scheduling-code.mode';

@Injectable()
export class SchedulingCodeService extends HttpBaseService {

  private baseURL = '';

  constructor(
    private http: HttpClient
  ) {
    super();
    this.baseURL = environment.services.schedulingService;
  }

  getSchedulingCodes(schedulingCodeQueryParams: QueryStringParameters) {
    const url = `${this.baseURL}/schedulingCodes`;

    return this.http.get<SchedulingCodeDetails>(url, { params: this.convertToHttpParam(schedulingCodeQueryParams), observe: 'response' })
      .pipe(catchError(this.handleError));
  }

  getSchedulingCode(schedulingCodeId: number) {
    const url = `${this.baseURL}/schedulingCodes/${schedulingCodeId}`;

    return this.http.get<SchedulingCodeDetails>(url)
      .pipe(catchError(this.handleError));
  }

  addSchedulingCode(schedulingCode: AddSchedulingCode) {
    const url = `${this.baseURL}/schedulingCodes`;

    return this.http.post<ApiResponseModel>(url, schedulingCode)
      .pipe(catchError(this.handleError));
  }

  updateSchedulingCode(schedulingCodeId: number, schedulingCode: UpdateSchedulingCode) {
    const url = `${this.baseURL}/schedulingCodes/${schedulingCodeId}`;

    return this.http.put<ApiResponseModel>(url, schedulingCode)
      .pipe(catchError(this.handleError));
  }

  deleteSchedulingCode(schedulingCodeId: number) {
    const url = `${this.baseURL}/schedulingCodes/${schedulingCodeId}`;

    return this.http.delete<ApiResponseModel>(url)
      .pipe(catchError(this.handleError));
  }
}
