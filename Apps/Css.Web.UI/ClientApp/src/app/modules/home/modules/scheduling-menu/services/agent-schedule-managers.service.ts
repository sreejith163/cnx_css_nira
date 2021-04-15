import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { HttpBaseService } from 'src/app/core/services/http-base.service';
import { ApiResponseModel } from 'src/app/shared/models/api-response.model';
import { environment } from 'src/environments/environment';
import { AgentChartResponse } from '../models/agent-chart-response.model';
import { AgentScheduleManagersQueryParams } from '../models/agent-schedule-mangers-query-params.model';
import { CopyAgentScheduleManagerChart } from '../models/copy-agent-schedule-manager-chart.model';
import { ScheduleManagerChartUpdate, ScheduleManagerGridChartDisplay } from '../models/schedule-manager-chart.model';

@Injectable()
export class AgentScheduleManagersService extends HttpBaseService {

  private baseURL = '';
  scheduleMangerChartsGridSubject$ = new BehaviorSubject<Array<ScheduleManagerGridChartDisplay>>([]); 
  scheduleMangerChartsGrid$ = this.scheduleMangerChartsGridSubject$.asObservable();

  constructor(
    private http: HttpClient
  ) {
    super();
    this.baseURL = environment.services.gatewayService;
  }

  getAgentScheduleManagers(queryParams: AgentScheduleManagersQueryParams) {
    const url = `${this.baseURL}/AgentScheduleManagers`;

    return this.http.get<AgentChartResponse>(url, {
      params: this.convertToHttpParam(queryParams),
      observe: 'response'
    }).pipe(catchError(this.handleError));
  }

  updateScheduleManagerChart(updateAgentScheduleMangagerChart: ScheduleManagerChartUpdate) {
    const url = `${this.baseURL}/AgentScheduleManagers/charts`;

    return this.http.put<ApiResponseModel>(url, updateAgentScheduleMangagerChart)
    .pipe(catchError(this.handleError));

  }

  copyAgentScheduleManagerChart(employeeId: string, copyAgentScheduleManagerChart: CopyAgentScheduleManagerChart) {
    const url = `${this.baseURL}/AgentScheduleManagers/${employeeId}/copy`;

    return this.http.put<ApiResponseModel>(url, copyAgentScheduleManagerChart)
    .pipe(catchError(this.handleError));
  }
}
