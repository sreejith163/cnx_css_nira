import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ApiResponseModel } from 'src/app/shared/models/api-response.model';
import { HttpBaseService } from 'src/app/core/services/http-base.service';

import { Forecast } from '../models/forecast.model';
import { catchError } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { ForecastDataModel } from '../models/forecast-data.model';
import { UpdateAgentAdmin } from '../models/update-agent-admin.model';
import { UpdateForecastData } from '../models/update-forecast-data.model';
import { ForecastExcelData } from '../models/forecast-excel.model';
import { ScheduledOpenResponse } from '../models/scheduled-open-response.model';
import { ForecastImportModel } from '../models/forecast-import.model';


@Injectable({
  providedIn: 'root'
})
export class ForecastScreenService extends HttpBaseService {
  test: any = [];
  private baseURL = '';

  constructor(
    private http: HttpClient
  ) {
    super();
    this.baseURL = environment.services.gatewayService;
  }
  addForecast(foreCast: ForecastDataModel) {
    const url = `${this.baseURL}/forecastscreen`;

    return this.http.post<ApiResponseModel>(url, foreCast)
      .pipe(catchError(this.handleError));
  }
  updateForecast(skillgroupID: number,Date: string, updateForecast: UpdateForecastData) {
    const url = `${this.baseURL}/forecastscreen/${skillgroupID}/date/${Date}`;

    return this.http.put<ApiResponseModel>(url, updateForecast)
      .pipe(catchError(this.handleError));
  }

  getForecastData(): Observable<any> {
    return this.http.get('/assets/time-table.json');
  }

  getForecastDataById(skillGroupId: number, Date: string) {
    const url = `${this.baseURL}/forecastscreen/${skillGroupId}/date/${Date}`;

    return this.http.get<ForecastDataModel>(url)
      .pipe(catchError(this.handleError));
  }

  importForecastData(importForecastDataModel: any,skillgroupID: number){
    const url = `${this.baseURL}/forecastscreen/${skillgroupID}/import`;

    return this.http.put<ApiResponseModel>(url, importForecastDataModel)
      .pipe(catchError(this.handleError));
  }
  getScheduleOpen(skillGroupId: number, date: string) {
    const url = `${this.baseURL}/forecastscreen/${skillGroupId}/scheduledopen?date=${date}`;
    return this.http.get<ScheduledOpenResponse>(url).pipe(catchError(this.handleError));
  }
}
