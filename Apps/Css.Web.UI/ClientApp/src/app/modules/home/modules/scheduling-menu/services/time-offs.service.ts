import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { HttpBaseService } from 'src/app/core/services/http-base.service';
import { ApiResponseModel } from 'src/app/shared/models/api-response.model';
import { QueryStringParameters } from 'src/app/shared/models/query-string-parameters.model';
import { environment } from 'src/environments/environment';
import { AddTimeOffs } from '../models/add-time-offs.model';
import { TimeOffResponse } from '../models/time-offs-response.model';
import { UpdateTimeOffs } from '../models/update-time-offs.model';

@Injectable()
export class TimeOffsService extends HttpBaseService {

  private baseURL = '';

  constructor(
    private http: HttpClient
  ) {
    super();
    this.baseURL = environment.services.gatewayService;
  }

  getTimeOffs(queryParams: QueryStringParameters) {
    const url = `${this.baseURL}/TimeOffs`;

    return this.http.get<TimeOffResponse>(url, {
      params: this.convertToHttpParam(queryParams),
      observe: 'response'
    }).pipe(catchError(this.handleError));
  }

  addTimeOff(data: AddTimeOffs) {
    const url = `${this.baseURL}/TimeOffs`;

    return this.http.post<ApiResponseModel>(url, data)
    .pipe(catchError(this.handleError));
  }

  updateTimeOff(data: UpdateTimeOffs) {
    const url = `${this.baseURL}/TimeOffs`;

    return this.http.put<ApiResponseModel>(url, data)
    .pipe(catchError(this.handleError));
  }

  deleteTimeoff(id: number) {
    const url = `${this.baseURL}/TimeOffs/${id}`;

    return this.http.delete<ApiResponseModel>(url)
    .pipe(catchError(this.handleError));
  }

}
