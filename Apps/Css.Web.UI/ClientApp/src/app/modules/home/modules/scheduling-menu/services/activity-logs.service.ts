import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { HttpBaseService } from 'src/app/core/services/http-base.service';
import { environment } from 'src/environments/environment';
import { ActivityLogsQueryParams } from '../models/activity-logs-query-params.model';
import { ActivityLogsBase } from '../models/activity-logs-base.model';
import { ActivityLogsResponse } from '../models/activity-logs-response.model';
import { of } from 'rxjs';
import { AgentScheduleManagerChart } from '../models/agent-schedule-manager-chart.model';

@Injectable()
export class ActivityLogsService extends HttpBaseService {

  private baseURL = '';

  constructor(
    private http: HttpClient
  ) {
    super();
    this.baseURL = environment.services.gatewayService;
  }

  getActivityLogs(queryParams: ActivityLogsQueryParams) {
    const url = `${this.baseURL}/activityLogs`;
    return this.http.get<ActivityLogsBase>(url, {
      params: this.convertToHttpParam(queryParams),
      observe: 'response'
    }).pipe(catchError(this.handleError));
  }
}
