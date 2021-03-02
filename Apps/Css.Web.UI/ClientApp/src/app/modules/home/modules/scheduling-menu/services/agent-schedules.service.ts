import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { HttpBaseService } from 'src/app/core/services/http-base.service';
import { environment } from 'src/environments/environment';
import { AgentSchedulesResponse } from '../models/agent-schedules-response.model';
import { AgentSchedulesQueryParams } from '../models/agent-schedules-query-params.model';
import { UpdateAgentSchedule } from '../models/update-agent-schedule.model';
import { ApiResponseModel } from 'src/app/shared/models/api-response.model';
import { UpdateAgentschedulechart } from '../models/update-agent-schedule-chart.model';
import { CopyAgentScheduleChart } from '../models/copy-agent-schedule-chart.model';
import { AgentChartResponse } from '../models/agent-chart-response.model';
import { ImportShceduleChart } from '../models/import-schedule-chart.model';
import { UpdateAgentScheduleMangersChart } from '../models/update-agent-schedule-managers-chart.model';
import { ScheduleChartQueryParams } from '../models/schedule-chart-query-params.model';
import { AgentScheduleChartResponse } from '../models/agent-schedule-chart-response.model';

@Injectable()
export class AgentSchedulesService extends HttpBaseService {

  private baseURL = '';

  constructor(
    private http: HttpClient
  ) {
    super();
    this.baseURL = environment.services.gatewayService;
  }

  getAgentSchedules(agentSchedulingQueryParams: AgentSchedulesQueryParams) {
    const url = `${this.baseURL}/AgentSchedules`;

    return this.http.get<AgentSchedulesResponse>(url, {
      params: this.convertToHttpParam(agentSchedulingQueryParams),
      observe: 'response'
    }).pipe(catchError(this.handleError));
  }

  getAgentSchedule(agentScheduleId: string) {
    const url = `${this.baseURL}/AgentSchedules/${agentScheduleId}`;

    return this.http.get<AgentSchedulesResponse>(url)
    .pipe(catchError(this.handleError));
  }

  getAgentScheduleRange(agentScheduleId: string) {
    const url = `${this.baseURL}/AgentSchedules/${agentScheduleId}/exists`;

    return this.http.get<AgentSchedulesResponse>(url)
    .pipe(catchError(this.handleError));
  }

  getCharts(agentScheduleId: string) {
    const url = `${this.baseURL}/AgentSchedules/${agentScheduleId}/charts`;

    return this.http.get<AgentScheduleChartResponse>(url)
    .pipe(catchError(this.handleError));
  }

  updateAgentSchedule(agentScheduleId: string, updateAgent: UpdateAgentSchedule) {
    const url = `${this.baseURL}/AgentSchedules/${agentScheduleId}`;

    return this.http.put<ApiResponseModel>(url, updateAgent)
    .pipe(catchError(this.handleError));
  }

  updateAgentScheduleChart(agentScheduleId: string, updateAgentScheduleChart: UpdateAgentschedulechart) {
    const url = `${this.baseURL}/AgentSchedules/${agentScheduleId}/charts`;

    return this.http.put<ApiResponseModel>(url, updateAgentScheduleChart)
    .pipe(catchError(this.handleError));
  }

  updateScheduleManagerChart(updateAgentScheduleMangagerChart: UpdateAgentScheduleMangersChart) {
    const url = `${this.baseURL}/AgentSchedules/managercharts`;

    return this.http.put<ApiResponseModel>(url, updateAgentScheduleMangagerChart)
    .pipe(catchError(this.handleError));
  }

  importAgentScheduleChart(importAgentScheduleChart: ImportShceduleChart) {
    const url = `${this.baseURL}/AgentSchedules/import`;

    return this.http.put<ApiResponseModel>(url, importAgentScheduleChart)
    .pipe(catchError(this.handleError));
  }

  copyAgentScheduleChart(agentScheduleId: string, copyAgentScheduleChart: CopyAgentScheduleChart) {
    const url = `${this.baseURL}/AgentSchedules/${agentScheduleId}/copy`;

    return this.http.put<ApiResponseModel>(url, copyAgentScheduleChart)
    .pipe(catchError(this.handleError));
  }

  // deleteAgentScheduleRange(agentScheduleId: string) {
  //   const url = `${this.baseURL}/AgentSchedules/${agentScheduleId}`;

  //   return this.http.put<ApiResponseModel>(url, copyAgentScheduleChart)
  //   .pipe(catchError(this.handleError));
  // }
}
