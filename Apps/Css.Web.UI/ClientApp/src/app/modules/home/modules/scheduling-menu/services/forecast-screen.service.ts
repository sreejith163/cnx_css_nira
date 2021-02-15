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
  updateForecast(skillgroupID: number, updateForecast: UpdateForecastData) {
    const url = `${this.baseURL}/forecastscreen/${skillgroupID}`;

    return this.http.put<ApiResponseModel>(url, updateForecast)
      .pipe(catchError(this.handleError));
  }

  getForecastData(): Observable<any> {

    return this.http.get("/assets/time-table.json");
  }
  getForecastDataById(skillGroupId: number, Date: string) {
    const url = `${this.baseURL}/forecastscreen/${skillGroupId}?date=${Date}`;

    return this.http.get<ForecastDataModel>(url)
      .pipe(catchError(this.handleError));
  }


}
