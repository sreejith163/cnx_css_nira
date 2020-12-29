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
import { CopyAgentSchedulechart } from '../models/copy-agent-schedule-chart.model';

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

  importAgentScheduleChart(agentScheduleId: string, updateAgentScheduleChart: UpdateAgentschedulechart) {
    const url = `${this.baseURL}/AgentSchedules/${agentScheduleId}/import`;

    return this.http.put<ApiResponseModel>(url, updateAgentScheduleChart)
    .pipe(catchError(this.handleError));
  }

  copyAgentScheduleChart(agentScheduleId: string, copyAgentScheduleChart: CopyAgentSchedulechart) {
    const url = `${this.baseURL}/AgentSchedules/${agentScheduleId}/copy`;

    return this.http.put<ApiResponseModel>(url, copyAgentScheduleChart)
    .pipe(catchError(this.handleError));
  }
}
