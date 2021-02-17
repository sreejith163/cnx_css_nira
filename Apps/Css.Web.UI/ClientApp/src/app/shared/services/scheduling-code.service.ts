import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { HttpBaseService } from 'src/app/core/services/http-base.service';
import { ApiResponseModel } from 'src/app/shared/models/api-response.model';
import { environment } from 'src/environments/environment';
import { AddSchedulingCode } from '../../modules/home/modules/system-admin/models/add-scheduling-code.model';
import { SchedulingCodeQueryParams } from '../../modules/home/modules/system-admin/models/scheduling-code-query-params.model';
import { SchedulingCode } from '../../modules/home/modules/system-admin/models/scheduling-code.model';
import { UpdateSchedulingCode } from '../../modules/home/modules/system-admin/models/update-scheduling-code.mode';

@Injectable()
export class SchedulingCodeService extends HttpBaseService {

  private baseURL = '';

  constructor(
    private http: HttpClient
  ) {
    super();
    this.baseURL = environment.services.gatewayService;
  }

  getSchedulingCodes(schedulingCodeQueryParams: SchedulingCodeQueryParams) {
    const url = `${this.baseURL}/schedulingCodes`;

    return this.http.get<SchedulingCode>(url, {
      params: this.convertToHttpParam(schedulingCodeQueryParams),
      observe: 'response'
    }).pipe(catchError(this.handleError));
  }

  getSchedulingCode(schedulingCodeId: number) {
    const url = `${this.baseURL}/schedulingCodes/${schedulingCodeId}`;

    return this.http.get<SchedulingCode>(url)
      .pipe(catchError(this.handleError));
  }

  addSchedulingCode(schedulingCode: AddSchedulingCode): Observable<ApiResponseModel> {
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
